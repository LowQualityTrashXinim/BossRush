using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;
using BossRush.Common.Systems;
using BossRush.Common.RoguelikeChange.ItemOverhaul;

namespace BossRush.Contents.Items.Weapon {
	public class FairFrozen : ModItem {
		public override void SetDefaults() {
			Item.damage = 26;
			Item.mana = 10;
			Item.width = 38;
			Item.height = 48;
			Item.useTime = Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(gold: 30);
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.Item1;
			Item.crit = 15;
			Item.knockBack = 9f;

			Item.DamageType = DamageClass.Summon;

			Item.shoot = 1;
			Item.scale = 1.25f;

			if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul globalitem)) {
				globalitem.SwingType = BossRushUseStyle.Swipe;
				Item.useTurn = false;
				Item.Set_ItemCriticalDamage(1f);
			}
		}
		public override void ModifyManaCost(Player player, ref float reduce, ref float mult) {
			if (player.GetModPlayer<PlayerStatsHandle>().CurrentMinionAmount >= player.maxMinions) {
				mult *= 0;
			}
		}
		public override bool AltFunctionUse(Player player) {
			return true;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			float amount = player.GetModPlayer<PlayerStatsHandle>().CurrentMinionAmount;
			if (amount < player.maxMinions) {
				SoundEngine.PlaySound(SoundID.Item44);
				player.AddBuff(ModContent.BuffType<FairFrozenModbuff>(), 2);
				var projectile = Projectile.NewProjectileDirect(source, player.Center, velocity, ModContent.ProjectileType<FairFrozenMinion>(), damage, knockback, player.whoAmI);
				projectile.originalDamage = Item.damage;
				projectile.OriginalCritChance = Item.crit;
				projectile.knockBack = 0;
			}
			if(player.altFunctionUse == 2) {
				player.MinionNPCTargetAim(false);
			}
			return false;
		}
		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
			if (target.HasBuff(BuffID.Frostburn)) {
				player.Heal(1);
			}
			else if (Main.rand.NextBool(5)) {
				target.AddBuff(BuffID.Frostburn, BossRushUtils.ToSecond(Main.rand.Next(1, 5)));
			}
		}
	}
	public class FairFrozenModbuff : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultBuff();
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) {
			// If the minions exist reset the buff time, otherwise remove the buff from the player
			if (player.ownedProjectileCounts[ModContent.ProjectileType<FairFrozenMinion>()] > 0) {
				player.buffTime[buffIndex] = 18000;
			}
			else {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
	public class FairFrozenMinion : ModProjectile {
		public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<FairFrozen>();
		public override void SetStaticDefaults() {
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Type] = true;
			ProjectileID.Sets.MinionSacrificable[Type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Type] = true;
		}
		public override void SetDefaults() {
			Projectile.width = 38;
			Projectile.height = 48;
			Projectile.minion = true;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.timeLeft = 999;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.minionSlots = 1;
		}
		public override bool MinionContactDamage() {
			return npc != null;
		}
		NPC npc = null;
		/// <summary>
		/// Only goes up to 1000 before resetting back to 0
		/// </summary>
		public int AliveTimer = 0;
		public float AICooldown { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
		public float AIattackState { get => Projectile.ai[1]; set => Projectile.ai[1] = value; }
		public bool Status_HitNPC = false;
		public override void AI() {
			Dust dustEff = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Frost);
			dustEff.noGravity = true;
			dustEff.velocity = Vector2.Zero;
			dustEff.position = Main.rand.NextVector2FromRectangle(Projectile.Hitbox);
			dustEff.scale = Main.rand.NextFloat(.9f, 1.2f);

			//This is a timer, that all lol
			AliveTimer = BossRushUtils.Safe_SwitchValue(AliveTimer, 1000);


			Player player = Main.player[Projectile.owner];
			Projectile.direction = player.direction;
			Projectile.spriteDirection = player.direction;
			if (player.dead || !player.active) {
				player.ClearBuff(ModContent.BuffType<FairFrozenModbuff>());
			}
			if (player.HasBuff(ModContent.BuffType<FairFrozenModbuff>())) {
				Projectile.timeLeft = 2;
			}

			if (Projectile.OwnerMinionAttackTargetNPC != null) {
				npc = Projectile.OwnerMinionAttackTargetNPC;
			}
			//searching for npc
			if (npc == null) {
				if (Projectile.Center.LookForHostileNPC(out NPC target, 1500)) {
					//found a target so return
					npc = target;
					return;
				}
				else {
					//Not found anyone yet ? idle behind player then
					int index = Projectile.minionPos;
					int playerdirection = -player.direction;
					Vector2 ToPos = player.Center.Add((Projectile.width * .25f + 5) * (index + 1) * player.direction, 0);
					Projectile.velocity = (ToPos - Projectile.Center).SafeNormalize(Vector2.Zero) * ((ToPos - Projectile.Center).Length() / 4f);
					Projectile.velocity.Y += (AliveTimer % 50 < 25 ? -.25f : .25f);

					Projectile.rotation = MathHelper.PiOver4 - MathHelper.PiOver2;
					Projectile.rotation -= MathHelper.ToRadians(10 * index) * playerdirection;
				}
			}
			else {
				//This mean the targetted npc is dead
				if (!npc.active || npc.life <= 0) {
					npc = null;
					return;
				}
				float offsetsize = npc.Size.Length() / 4f;
				//Fly to the targetted NPC
				if (!Projectile.Center.IsCloseToPosition(npc.Center, 250 + offsetsize)) {
					Reset();
				}
				if (AICooldown <= 0) {
					Reset();
					//Fly to target
					if (!Projectile.Center.IsCloseToPosition(npc.Center, 150 + offsetsize)) {
						Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * ((npc.Center - Projectile.Center).Length() / 32f) + npc.velocity * .5f;
					}
					else {
						//Attempt to hit target and also play a sound
						SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
						AICooldown = 90;
						Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 20;
						Projectile.ai[2] = Main.rand.NextBool().ToDirectionInt();
						for (int i = 0; i < 30; i++) {
							Vector2 posToSpawm = Main.rand.NextVector2CircularEdge(12.5f, 4.5f).RotatedBy(Projectile.velocity.ToRotation() + MathHelper.PiOver2) * 2;
							Dust dust = Dust.NewDustDirect(Projectile.Center.PositionOFFSET(Projectile.velocity, -30), 0, 0, DustID.Frost);
							dust.noGravity = true;
							dust.velocity = posToSpawm.SafeNormalize(Vector2.Zero) * 2f - Projectile.velocity * .35f;
							dust.position += posToSpawm;
							dust.fadeIn = 1f;
						}
					}
				}
				else {
					if (Status_HitNPC) {
						//Do a special movement
						Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(5 * Projectile.ai[2]));
					}
					AICooldown = BossRushUtils.CountDown((int)AICooldown);
				}
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			}
			if (Projectile.spriteDirection == -1) {
				Projectile.rotation += MathHelper.PiOver2;
			}
		}
		public void Reset() {
			Status_HitNPC = false;
			AICooldown = 0;
			AIattackState = 0;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			Status_HitNPC = true;

			if (Main.rand.NextBool(5)) {
				target.AddBuff(BuffID.Frostburn, BossRushUtils.ToSecond(Main.rand.Next(1, 5)));
			}
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
			if (Main.rand.Next(1, 101) <= Projectile.CritChance) {
				modifiers.SetCrit();
			}
		}
	}
}
