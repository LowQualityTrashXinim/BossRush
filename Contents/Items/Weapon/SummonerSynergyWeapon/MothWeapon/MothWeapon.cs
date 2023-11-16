using BossRush.Texture;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.SummonerSynergyWeapon.MothWeapon {
	public class StreetLamp : SynergyModItem {
		public override void SetStaticDefaults() {
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(15, 6));
		}
		public override void SetDefaults() {
			Item.BossRushSetDefault(62, 62, 20, 0, 32, 32, ItemUseStyleID.HoldUp, true);
			Item.DamageType = DamageClass.Summon;
			Item.shoot = ModContent.ProjectileType<MothProj>();
			Item.mana = 15;
			Item.noMelee = true;
			Item.holdStyle = ItemHoldStyleID.HoldUp;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item46;
		}

		public override Color? GetAlpha(Color lightColor) {
			return Color.White;
		}
		public override void SynergyUpdateInventory(Player player, PlayerSynergyItemHandle modplayer) {
			if (player.HasItem(ItemID.VampireFrogStaff)) {
				modplayer.SynergyBonus++;
				modplayer.StreetLamp_VampireFrogStaff = true;
			}
			if (player.HasItem(ItemID.FireWhip)) {
				modplayer.SynergyBonus++;
				modplayer.StreetLamp_Firecracker = true;
			}

		}
		public override void UseStyle(Player player, Rectangle heldItemFrame) {
			LampHold(player);
		}
		public override void HoldStyle(Player player, Rectangle heldItemFrame) {
			LampHold(player);
		}
		public override void PostUpdate() {
			Lighting.AddLight(Item.Center + new Vector2(0, -64), 1f, 1f, 1f);
			Lighting.AddLight(Item.Center + new Vector2(0, -64 * .5f), 1f, 1f, 0f);
		}

		// custom hold and use style to fit the theme :D
		private void LampHold(Player player) {
			player.itemRotation = MathHelper.ToRadians(45 * -player.direction);
			player.itemLocation = player.Center + new Vector2(8 * player.direction, 16);
		}

		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			tooltips.Add(new TooltipLine(Mod, "StreetLampTooltip", "Summons a Moth to protect their Lamp"));
			if (modplayer.StreetLamp_VampireFrogStaff) {
				tooltips.Add(new TooltipLine(Mod, "StreetLamp_VampireFrogStaff",
					$"[i:{ItemID.VampireFrogStaff}] Moth's dash speed increased by 15% and every 3 successful hit will heal player for 2.5% of dash attack damage dealt"));
			}
			if (modplayer.StreetLamp_Firecracker) {
				tooltips.Add(new TooltipLine(Mod, "StreetLamp_Firecracker",
					$"[i:{ItemID.FireWhip}] Moth's Dash attacks have 10% chance to do small explosion that deal 3 time the damage and inflict Hellfire for 5 seconds"));
			}
		}
		public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			//modify the moths spawn location to be at the lamp
			position = player.Center + new Vector2(0, -64);
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			player.AddBuff(ModContent.BuffType<MothBuff>(), 1000);
			Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
			CanShootItem = false;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.FlinxStaff)
				.AddIngredient(ItemID.HornetStaff)
				.Register();
		}
	}

	internal class MothBuff : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<MothProj>()] > 0) {
				player.buffTime[buffIndex] = 180000;
			}
			else {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
	internal class MothProj : SynergyModProjectile {

		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 4;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
		}

		public override void SetDefaults() {
			Projectile.width = 48;
			Projectile.height = 24;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.minionSlots = 1;
			Projectile.penetrate = -1;
			Projectile.minion = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}
		public override bool? CanCutTiles() => false;
		public override bool MinionContactDamage() => true;
		public override void OnSpawn(IEntitySource source) {
			BossRushUtils.CombatTextRevamp(Main.player[Projectile.owner].Hitbox, Color.Beige, "LAMP");
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			if (!CheckActive(player)) {
				return;
			}
			//get all the important informations about the target
			getInfos(player, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
			//the movement, attacks, etc... is here
			behavior(player, vectorToIdlePosition, distanceToIdlePosition, foundTarget, distanceFromTarget, targetCenter, modplayer);
			SelectFrame();
			//sprite juice
			if (currentState == State.dashing) {
				Projectile.rotation = Projectile.spriteDirection == 1 ? Projectile.velocity.ToRotation() : Projectile.velocity.ToRotation() - MathHelper.ToRadians(180f);

			}
			else
				Projectile.rotation = Projectile.velocity.X * 0.05f;
			if (currentState != State.windup)
				Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;
			else
				Projectile.direction = Projectile.spriteDirection = targetCenter.X > Projectile.Center.X ? 1 : -1;
		}

		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
			if (currentState == State.dashing) {
				//deal more damage if dashing
				int DashBonusDamage = (int)(damageDone * .5f);
				if (modplayer.StreetLamp_VampireFrogStaff) {
					modplayer.StreetLamp_VampireFrogStaff_HitCounter++;
					if (modplayer.StreetLamp_VampireFrogStaff_HitCounter >= 3) {
						int SynergyhealAmount = (int)(DashBonusDamage * 0.025f);
						player.Heal(SynergyhealAmount);
						modplayer.StreetLamp_VampireFrogStaff_HitCounter = 0;
					}
				}
				if (modplayer.StreetLamp_Firecracker && Main.rand.NextBool(10)) {
					npc.Center.LookForHostileNPC(out List<NPC> npclist, 200);
					foreach (var entity in npclist) {
						SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot);
						entity.StrikeNPC(npc.CalculateHitInfo(Projectile.damage * 3, (Projectile.Center.X <= entity.Center.X).ToDirectionInt(), false, 8, DamageClass.Summon, true));
						entity.AddBuff(BuffID.OnFire3, 300);
						player.addDPS(Projectile.damage * 3);
					}
					for (int i = 0; i < 50; i++) {
						int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke);
						Main.dust[dust].velocity = Main.rand.NextVector2Circular(5, 5);
						Main.dust[dust].scale = Main.rand.NextFloat(1, 1.75f);
						Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.Torch);
					}
				}
				state = State.hit;
				Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.Next(160, 200))) * Main.rand.NextFloat(1, 3);
				npc.StrikeNPC(npc.CalculateHitInfo(DashBonusDamage, (Projectile.Center.X <= npc.Center.X).ToDirectionInt(), false));
				player.addDPS(DashBonusDamage);
			}
		}

		private void getInfos(Player owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter) {
			Vector2 idlePosition = owner.Center + new Vector2(0, -80);
			vectorToIdlePosition = idlePosition - Projectile.Center;
			distanceToIdlePosition = vectorToIdlePosition.Length();
			if (Main.myPlayer == owner.whoAmI && distanceToIdlePosition > 2000f) {
				Projectile.position = idlePosition;
				Projectile.velocity *= 0.1f;
				Projectile.netUpdate = true;
			}
			// Starting search distance
			distanceFromTarget = 350f;
			targetCenter = owner.Center;
			foundTarget = false;
			// This code is required if your minion weapon has the targeting feature
			if (owner.HasMinionAttackTargetNPC) {
				NPC npc = Main.npc[owner.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, owner.Center);

				if (between < 2000f) {
					distanceFromTarget = between;
					targetCenter = npc.Center;
					foundTarget = true;
				}
			}
			if (!foundTarget) {
				for (int i = 0; i < Main.maxNPCs; i++) {
					NPC npc = Main.npc[i];

					if (npc.CanBeChasedBy()) {
						float between = Vector2.Distance(npc.Center, owner.Center);
						bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;


						if ((closest && inRange || !foundTarget)) {
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
						}
					}
				}
			}
		}

		public void behavior(Player owner, Vector2 vectorToIdlePosition, float distanceToIdlePosition, bool foundTarget, float distanceFromTarget, Vector2 targetCenter, PlayerSynergyItemHandle modplayer) {
			float baseSpeed = 20f;
			// projectile speed scales with whip attack speed for extra synergy juice
			float speed = (baseSpeed * owner.GetAttackSpeed(DamageClass.SummonMeleeSpeed)) + (modplayer.StreetLamp_VampireFrogStaff == true ? baseSpeed * 0.15f : 0f);
			//placeholder
			Vector2 dashAt = owner.Center;
			// dashDurtion = distance/speed + baseDashDuration
			int baseDashDuration = 10;
			int baseWindUpDuration = 20;
			float inertia;
			bool attackable = foundTarget && distanceFromTarget <= 1000f;

			//dash location setup
			if (currentState != State.dashing && foundTarget) {
				dashAt = targetCenter;
			}
			//reset everything here
			if (!attackable) {
				state = State.followOwner;
				dashDuration = 0;
				windingUp = 0;
				attackCooldown = 0;
			}

			//machine state juice
			switch (currentState, attackable) {
				case (State.followOwner, false):
					Vector2 idlePosition = owner.Center;
					float minionPositionOffsetX = (10 + Projectile.minionPos * 40) * -owner.direction;
					idlePosition.X += minionPositionOffsetX;
					float overlapVelocity = 0.4f;
					for (int i = 0; i < Main.maxProjectiles; i++) {
						Projectile other = Main.projectile[i];
						if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width) {
							if (Projectile.position.X < other.position.X) {
								Projectile.velocity.X -= overlapVelocity;
							}
							else {
								Projectile.velocity.X += overlapVelocity;
							}

							if (Projectile.position.Y < other.position.Y) {
								Projectile.velocity.Y -= overlapVelocity;
							}
							else {
								Projectile.velocity.Y += overlapVelocity;
							}
						}
					}
					if (distanceToIdlePosition > 600f) {
						speed = 12f;
						inertia = 10f;
					}
					else {
						speed = 4f;
						inertia = 20f;
					}
					if (distanceToIdlePosition > 20f) {

						vectorToIdlePosition.Normalize();
						vectorToIdlePosition *= speed;
						Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
					}
					else if (Projectile.velocity == Vector2.Zero) {
						Projectile.velocity.X = -0.15f;
						Projectile.velocity.Y = -0.05f;
					}

					break;
				//ready, set, GO!
				case (State.followOwner, true):
					windingUp = baseWindUpDuration;
					state = State.windup;
					Projectile.velocity.Y = -15;
					break;

				case (State.windup, true):
					if (windingUp > 0) {
						windingUp--;
						Projectile.velocity.Y *= 0.90f;
					}
					else {
						for (int i = 0; i < 15; i++) {
							Dust.NewDustPerfect(Projectile.Center, DustID.WhiteTorch, Main.rand.NextVector2CircularEdge(2, 2));
						}
						Projectile.velocity = (((dashAt + new Vector2(Main.rand.Next(-25, 25), Main.rand.Next(-25, 25))) - Projectile.Center).SafeNormalize(Vector2.UnitX) * (speed));
						state = State.dashing;
						dashDuration = (int)(Projectile.Center.Distance(dashAt) / speed) + baseDashDuration;
						Projectile.ResetLocalNPCHitImmunity();
					}
					break;

				case (State.dashing, true):
					dashDuration--;
					if (dashDuration <= 0) {
						attackCooldown = 20;
						state = State.miss;
					}
					break;
				case (State.hit, false || true):
					dashDuration = 0;
					Projectile.velocity *= 0.75f;
					attackCooldown--;
					if (attackCooldown <= 0) {
						Projectile.velocity *= 0;
						state = State.followOwner;
					}
					break;
				case (State.miss, true || false):
					state = State.followOwner;
					Projectile.velocity *= 0;
					break;
			}
		}

		//renames for easier readabilty
		public int windingUp {
			get => (int)Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public int dashDuration {
			get => (int)Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public int attackCooldown {
			get => (int)Projectile.ai[2];
			set => Projectile.ai[2] = value;
		}

		public enum State {
			//always windup before dashing 
			windup,
			dashing,

			miss,
			hit,

			followOwner
		}

		//change states with this
		private State state = State.followOwner;

		//ready only
		public State currentState { get { return state; } }

		private bool CheckActive(Player owner) {
			if (owner.dead || !owner.active) {
				owner.ClearBuff(ModContent.BuffType<MothBuff>());
				return false;
			}
			if (owner.HasBuff(ModContent.BuffType<MothBuff>())) {
				Projectile.timeLeft = 2;
			}
			return true;
		}
		public void SelectFrame() {
			if (++Projectile.frameCounter >= 6) {
				Projectile.frameCounter = 0;
				Projectile.frame += 1;
				if (Projectile.frame >= Main.projFrames[Projectile.type]) {
					Projectile.frame = 0;
				}
			}
		}
	}
}
