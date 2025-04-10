using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.BurningPassion {
	class BurningPassion : SynergyModItem {
		public override void Synergy_SetStaticDefaults() {
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.WandofFrosting);
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.SkyFracture);
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.ChainKnife);
		}
		public override void SetDefaults() {
			Item.BossRushSetDefault(74, 74, 25, 6.7f, 28, 28, ItemUseStyleID.Shoot, true);
			Item.BossRushSetDefaultSpear(ModContent.ProjectileType<BurningPassionP>(), 3.7f);
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(silver: 1000);
			Item.UseSound = SoundID.Item1;
		}
		public override bool AltFunctionUse(Player player) {
			return player.GetModPlayer<BurningPassionPlayer>().BurningPassion_Cooldown <= 0;
		}
		public override bool CanUseItem(Player player) {
			return player.ownedProjectileCounts[ModContent.ProjectileType<BurningPassionP>()] < 1;
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			base.ModifySynergyToolTips(ref tooltips, modplayer);
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.WandofFrosting)) {
				tooltips.Add(new TooltipLine(Mod, "WandOfFrosting", $"[i:{ItemID.WandofFrosting}] Inflict frost burn on hit and shoot out spark flame on peak"));
			}
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.SkyFracture)) {
				tooltips.Add(new TooltipLine(Mod, "SkyFracture", $"[i:{ItemID.SkyFracture}] Attacking summon 3 sky fracture toward your foes dealing 45% of your weapon damage"));
			}
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.ChainKnife)) {
				tooltips.Add(new TooltipLine(Mod, "ChainKnife", $"[i:{ItemID.ChainKnife}] Massively reduce alt attack cool down"));
			}
		}
		public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
			if (player.GetModPlayer<BurningPassionPlayer>().BurningPassion_Cooldown == 1)
				for (int i = 0; i < 25; i++) {
					int dust = Dust.NewDust(player.Center, 0, 0, DustID.Torch);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(5, 5);
					Main.dust[dust].scale = Main.rand.NextFloat(1.5f, 2.75f);
					Main.dust[dust].fadeIn = 1;
				}
			player.GetModPlayer<BurningPassionPlayer>().BurningPassion_Cooldown = BossRushUtils.CountDown(player.GetModPlayer<BurningPassionPlayer>().BurningPassion_Cooldown);
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			BurningPassionPlayer burning = player.GetModPlayer<BurningPassionPlayer>();
			if (player.altFunctionUse == 2 && burning.BurningPassion_Cooldown <= 0) {
				if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.ChainKnife)) {
					burning.BurningPassion_Cooldown = 30;
				}
				else {
					burning.BurningPassion_Cooldown = 120;
				}
				player.velocity = velocity * 5f;
			}
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.SkyFracture)) {
				float speed = velocity.Length() * 4;
				for (int i = 0; i < 3; i++) {
					Vector2 newPos = position + Main.rand.NextVector2Circular(75, 75);
					Vector2 newVel = (Main.MouseWorld - newPos).SafeNormalize(Vector2.Zero) * speed;
					Projectile.NewProjectile(source, newPos, newVel, ProjectileID.SkyFracture, (int)(damage * .45f), knockback, player.whoAmI);
				}
			}
			CanShootItem = true;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Spear)
				.AddIngredient(ItemID.WandofSparking)
				.Register();
		}
	}
	public class BurningPassionP : SynergyModProjectile {
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 30;
			Projectile.penetrate = -1;
			Projectile.aiStyle = 19;
			Projectile.alpha = 0;

			Projectile.hide = true;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
		}
		protected virtual float HoldoutRangeMin => 50f;
		protected virtual float HoldoutRangeMax => 200f;
		public override void SynergyPreAI(Player player, PlayerSynergyItemHandle modplayer, out bool runAI) {
			bool synergycheck = SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<BurningPassion>(), ItemID.WandofFrosting);
			int duration = player.itemAnimationMax;
			player.heldProj = Projectile.whoAmI;
			if (Projectile.timeLeft > duration) {
				if (player.altFunctionUse == 2) {
					Projectile.width += 30;
					Projectile.height += 30;
					Projectile.ai[2]++;
				}
				Projectile.timeLeft = duration;
			}
			Projectile.velocity = Vector2.Normalize(Projectile.velocity);
			float halfDuration = duration * 0.5f;
			float progress;
			if (Projectile.timeLeft == (int)(halfDuration + 5) && SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<BurningPassion>(), ItemID.WandofFrosting)) {
				for (int i = 0; i < 5; i++) {
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (Projectile.velocity * Main.rand.NextFloat(3, 6)).Vector2RotateByRandom(5).Vector2RandomSpread(1, Main.rand.NextFloat(.75f, 1.25f)), ProjectileID.WandOfSparkingSpark, (int)(Projectile.damage * .25f), 0f, player.whoAmI);
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (Projectile.velocity * Main.rand.NextFloat(3, 6)).Vector2RotateByRandom(5).Vector2RandomSpread(1, Main.rand.NextFloat(.75f, 1.25f)), ProjectileID.WandOfFrostingFrost, (int)(Projectile.damage * .25f), 0f, player.whoAmI);
				}
			}
			if (Projectile.timeLeft < halfDuration) {
				progress = Projectile.timeLeft / halfDuration;
			}
			else {
				progress = (duration - Projectile.timeLeft) / halfDuration;
			}
			Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
			Projectile.rotation += Projectile.spriteDirection == -1 ? MathHelper.PiOver4 : MathHelper.PiOver4 + MathHelper.PiOver2;
			runAI = false;
			for (int i = 0; i < 5; i++) {
				Dust.NewDust(Projectile.position, (int)(Projectile.width * 0.5f), (int)(Projectile.height * 0.5f), DustID.Torch, Projectile.velocity.X * 0.75f, -5, 0, default, Main.rand.NextFloat(0.5f, 1.2f));
				if (synergycheck) {
					Dust.NewDust(Projectile.position, (int)(Projectile.width * 0.5f), (int)(Projectile.height * 0.5f), DustID.IceTorch, Projectile.velocity.X * 0.75f, -5, 0, default, Main.rand.NextFloat(0.5f, 1.2f));
				}
			}
			float animationprogress = 1 - Projectile.timeLeft / duration;
			if (Projectile.ai[2] > 0) {
				for (int i = 0; i < 3; i++) {
					Dust vertidust1 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Torch, Scale: .1f);
					vertidust1.noGravity = true;
					vertidust1.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(90)) * (4 * animationprogress);
					vertidust1.fadeIn = 1.2f;
					vertidust1.scale += animationprogress;
					if (synergycheck) {
						Dust vertidust2 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.IceTorch, Scale: .1f);
						vertidust2.noGravity = true;
						vertidust2.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(270)) * (4 * animationprogress);
						vertidust2.fadeIn = 1.2f;
						vertidust2.scale += animationprogress;
					}
				}
			}
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
			if (SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<BurningPassion>(), ItemID.WandofFrosting)) {
				npc.AddBuff(BuffID.Frostburn, 90);
			}
			Projectile.damage = (int)(Projectile.damage * .9f);
			npc.AddBuff(BuffID.OnFire, 90);
			npc.immune[Projectile.owner] = 5;
		}
	}
	public class BurningPassionPlayer : ModPlayer {
		public int BurningPassion_Cooldown = 0;
		int check = 0;
		public override void PostUpdate() {
			Item item = Player.HeldItem;
			if (item.type == ModContent.ItemType<BurningPassion>()) {
				if (!Player.ItemAnimationActive && check == 0) {
					Player.velocity *= .25f;
					check++;
				}
				else if (Player.ItemAnimationActive && Main.mouseRight) {
					Player.gravity = 0;
					Player.velocity.Y -= 0.3f;
					Player.ignoreWater = true;
					check = 0;
				}
			}
		}
		public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable) {
			if (Player.ItemAnimationActive && Player.HeldItem.type == ModContent.ItemType<BurningPassion>() && Player.ownedProjectileCounts[ModContent.ProjectileType<BurningPassionP>()] > 0) {
				return true;
			}
			return base.ImmuneTo(damageSource, cooldownCounter, dodgeable);
		}

	}
}
