using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.IceStorm {
	internal class IceStorm : SynergyModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(42, 98, 50, 1f, 75, 75, ItemUseStyleID.Shoot, ProjectileID.FrostArrow, 10f, true, AmmoID.Arrow);
			Item.crit = 5;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 50);
			Item.scale = 0.7f;
			Item.UseSound = SoundID.Item5;
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			base.ModifySynergyToolTips(ref tooltips, modplayer);
			if (modplayer.IceStorm_SnowBallCannon) {
				tooltips.Add(new TooltipLine(Mod, "IceStorm_SnowBallCannon", $"[i:{ItemID.SnowballCannon}] Charge attack up can shoot snowballs and summon itself"));
			}
			if (modplayer.IceStorm_FlowerofFrost) {
				tooltips.Add(new TooltipLine(Mod, "IceStorm_FlowerofFrost", $"[i:{ItemID.FlowerofFrost}] Charge attack up can shoot ball of frost and summon itself"));
			}
			if (modplayer.IceStorm_BlizzardStaff) {
				tooltips.Add(new TooltipLine(Mod, "IceStorm_BlizzardStaff", $"[i:{ItemID.BlizzardStaff}] Max charge can now rain down frost spike"));
			}
		}
		public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
			base.HoldSynergyItem(player, modplayer);
			int type;
			if (player.HasItem(ItemID.SnowballCannon)) {
				modplayer.IceStorm_SnowBallCannon = true;
				type = ModContent.ProjectileType<IceStormSnowBallCannonMinion>();
				if (player.ownedProjectileCounts[type] < 1) {
					Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, Vector2.Zero, type, Item.damage, Item.knockBack, player.whoAmI);
				}
				modplayer.SynergyBonus++;
			}
			if (player.HasItem(ItemID.FlowerofFrost)) {
				modplayer.IceStorm_FlowerofFrost = true;
				type = ModContent.ProjectileType<IceStormFrostFlowerMinion>();
				if (player.ownedProjectileCounts[type] < 1) {
					Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, Vector2.Zero, type, Item.damage, Item.knockBack, player.whoAmI);
				}
				modplayer.SynergyBonus++;
			}
			if (player.HasItem(ItemID.BlizzardStaff)) {
				modplayer.IceStorm_BlizzardStaff = true;
				modplayer.SynergyBonus++;
			}
			if (!Main.mouseLeft && modplayer.IceStorm_SpeedMultiplier >= 1) {
				modplayer.IceStorm_SpeedMultiplier -= 0.025f;
			}
		}
		public override bool CanConsumeAmmo(Item ammo, Player player) {
			return Main.rand.NextFloat() >= 0.2f;
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-3, 0);
		}
		public override float UseSpeedMultiplier(Player player) {
			return player.GetModPlayer<PlayerSynergyItemHandle>().IceStorm_SpeedMultiplier;
		}
		int count = 0;
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			ChargeUpHandle(player);
			float projectile = (int)(modplayer.IceStorm_SpeedMultiplier * .5f);
			if (modplayer.IceStorm_SnowBallCannon) {
				for (int i = 0; i < projectile; i++) {
					Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(projectile * 7).Vector2RandomSpread(7) * 1.5f, ProjectileID.SnowBallFriendly, damage, knockback, player.whoAmI);
				}
			}
			if (modplayer.IceStorm_FlowerofFrost) {
				projectile = (int)(modplayer.IceStorm_SpeedMultiplier * .1666667f);
				for (int i = 0; i < projectile; i++) {
					Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(projectile * 5).Vector2RandomSpread(12), ProjectileID.BallofFrost, damage, knockback, player.whoAmI);
				}
			}
			if (modplayer.IceStorm_BlizzardStaff && modplayer.IceStorm_SpeedMultiplier >= 8) {
				Vector2 SkyPos = new Vector2(player.Center.X, player.Center.Y - 800);
				Vector2 SkyVelocity = (Main.MouseWorld - SkyPos).SafeNormalize(Vector2.UnitX);
				for (int i = 0; i < 5; i++) {
					SkyPos += Main.rand.NextVector2Circular(200, 200);
					int FinalCharge = Projectile.NewProjectile(source, SkyPos, SkyVelocity * 30, ProjectileID.Blizzard, damage, knockback, player.whoAmI);
					Main.projectile[FinalCharge].tileCollide = false;
					Main.projectile[FinalCharge].timeLeft = 100;
				}
			}
			projectile = 1 + (int)(modplayer.IceStorm_SpeedMultiplier * .2f);
			for (int i = 0; i < projectile; ++i) {
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(projectile * 3) * 2f, ProjectileID.FrostburnArrow, damage, knockback, player.whoAmI);
			}
			projectile = (int)(modplayer.IceStorm_SpeedMultiplier * .2f);
			for (int i = 0; i < projectile; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(5).Vector2RandomSpread(4, Main.rand.NextFloat(0.5f, 1f)), ProjectileID.IceBolt, damage, knockback, player.whoAmI);
			}
			projectile = modplayer.IceStorm_SpeedMultiplier / 7f;
			if (projectile >= 1) {
				Projectile.NewProjectile(source, position, velocity * 2f, ProjectileID.FrostArrow, damage, knockback, player.whoAmI);
			}
			if (modplayer.IceStorm_SpeedMultiplier >= 8) {
				if (count == 0) {
					DustExplosion(player.Center);
					count++;
				}
			}
			else if (modplayer.IceStorm_SpeedMultiplier <= 7) {
				count = 0;
			}
			CanShootItem = false;
		}
		private void ChargeUpHandle(Player player) {
			if (Main.mouseLeft && player.GetModPlayer<PlayerSynergyItemHandle>().IceStorm_SpeedMultiplier <= 8) {
				if (player.GetModPlayer<PlayerSynergyItemHandle>().IceStorm_SpeedMultiplier <= 2) {
					player.GetModPlayer<PlayerSynergyItemHandle>().IceStorm_SpeedMultiplier += 0.1f;
				}
				if (player.GetModPlayer<PlayerSynergyItemHandle>().IceStorm_SpeedMultiplier <= 6) {
					player.GetModPlayer<PlayerSynergyItemHandle>().IceStorm_SpeedMultiplier += 0.1f;
				}
				else {
					player.GetModPlayer<PlayerSynergyItemHandle>().IceStorm_SpeedMultiplier += 0.02f;
				}
			}
		}
		private void DustExplosion(Vector2 center) {
			for (int i = 0; i < 400; i++) {
				Vector2 circular = Main.rand.NextVector2CircularEdge(25, 25);
				int dustNum = Dust.NewDust(center, 0, 0, DustID.FrostHydra, 0, 0, 0, default, Main.rand.NextFloat(0.75f, 1.5f));
				Main.dust[dustNum].noGravity = true;
				Main.dust[dustNum].noLight = false;
				Main.dust[dustNum].noLightEmittence = false;
				Main.dust[dustNum].velocity = circular;
			}
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.IceBlade)
				.AddIngredient(ItemID.IceBow)
				.Register();
		}
	}
	class IceStormSnowBallCannonMinion : ModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.SnowballCannon);
		public override void SetDefaults() {
			Projectile.height = 26;
			Projectile.width = 50;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 9000;
		}
		public override bool? CanHitNPC(NPC target) {
			return false;
		}
		int timer = 0;
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if (player.active && player.HeldItem.type == ModContent.ItemType<IceStorm>() && player.HasItem(ItemID.SnowballCannon)) {
				Projectile.timeLeft = 2;
			}
			Projectile.IdleFloatMovement(player, out Vector2 vec, out float dis);
			Projectile.MoveToIdle(vec, dis, 10, 10);
			Projectile.rotation = (Projectile.Center - Main.MouseWorld).ToRotation();
			Projectile.spriteDirection = Projectile.Center.X < Main.MouseWorld.X ? 1 : -1;
			Projectile.rotation += Projectile.spriteDirection == -1 ? 0 : MathHelper.Pi;
			if (player.Center.LookForHostileNPC(out NPC npc, 500) && npc != null) {
				Vector2 velocityToNpc = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
				Projectile.spriteDirection = Projectile.Center.X < npc.Center.X ? 1 : -1;
				Projectile.rotation = velocityToNpc.ToRotation();
				Projectile.rotation += Projectile.spriteDirection == 1 ? 0 : MathHelper.Pi;
				if (timer <= 0) {
					timer = (int)(20 * Math.Clamp(1 - player.GetModPlayer<PlayerSynergyItemHandle>().IceStorm_SpeedMultiplier * .125f, .1f, 1f));
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.PositionOFFSET(velocityToNpc, 45f), velocityToNpc * 20f, ProjectileID.SnowBallFriendly, Projectile.damage, Projectile.knockBack, Projectile.owner);
					return;
				}
			}
			timer = BossRushUtils.CountDown(timer);
		}
	}
	class IceStormFrostFlowerMinion : ModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.FlowerofFrost);
		public override void SetDefaults() {
			Projectile.width = 28;
			Projectile.height = 28;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 9000;
		}
		public override bool? CanHitNPC(NPC target) {
			return false;
		}
		int timer = 0;
		public override void AI() {
			Dust.NewDust(Projectile.Center, 0, 0, DustID.Frost, 0, 0, 0, default, Main.rand.NextFloat(.5f, .75f));
			Player player = Main.player[Projectile.owner];
			if (player.active && player.HeldItem.type == ModContent.ItemType<IceStorm>() && player.HasItem(ItemID.FlowerofFrost)) {
				Projectile.timeLeft = 2;
			}
			Vector2 positionToIdle = player.Center + new Vector2(0, -50);
			Vector2 VelocityRaw = positionToIdle - Projectile.Center;
			Projectile.rotation = -MathHelper.PiOver4;
			Projectile.MoveToIdle(VelocityRaw, 11, 10, true);
			Projectile.ResetMinion(positionToIdle, 1500);
			if (player.Center.LookForHostileNPC(out NPC npc, 500) && npc != null) {
				if (timer <= 0) {
					Vector2 velocityToNpc = (npc.Center - player.Center).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(5f, 8f);
					timer = (int)(30 * Math.Clamp(1 - player.GetModPlayer<PlayerSynergyItemHandle>().IceStorm_SpeedMultiplier * .125f, .1f, 1f));
					int proj = Projectile.NewProjectile(
						Projectile.GetSource_FromThis(),
						Projectile.Center - new Vector2(0, 20),
						velocityToNpc,
						ProjectileID.BallofFrost,
						Projectile.damage, Projectile.knockBack, Projectile.owner);
					Main.projectile[proj].timeLeft = 250;
					return;
				}
			}
			timer = BossRushUtils.CountDown(timer);
		}
	}
}
