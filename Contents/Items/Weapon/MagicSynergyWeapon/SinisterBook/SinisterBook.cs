using BossRush.Contents.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.SinisterBook {
	internal class SinisterBook : SynergyModItem {
		public override void Synergy_SetStaticDefaults() {
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.DemonScythe, $"[i:{ItemID.DemonScythe}] You occasionally shoot out a ring of demon scythe");
		}
		public override void SetDefaults() {
			Item.BossRushDefaultMagic(10, 10, 17, 2f, 9, 9, ItemUseStyleID.Shoot, ModContent.ProjectileType<SinisterBolt>(), 2.5f, 14, true);
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(platinum: 5);
			Item.UseSound = SoundID.Item8;
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.DemonScythe);
		}
		public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			position = position.PositionOFFSET(velocity, 30);
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			for (int i = 0; i < Main.rand.Next(3, 5); i++) {
				Vector2 vel = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.Next(80, 120) * Main.rand.NextBool().ToDirectionInt()));
				Projectile.NewProjectile(source, position, vel, ModContent.ProjectileType<SinisterBolt>(), damage, knockback, player.whoAmI);
			}
			if (modplayer.SinisterBook_DemonScythe_Counter >= 20) {
				for (int i = 0; i < 10; i++) {
					Vector2 vel = velocity.Vector2DistributeEvenly(10, 360, i);
					Projectile.NewProjectile(source, position, vel, ProjectileID.DemonScythe, damage, knockback, player.whoAmI);
				}
				modplayer.SinisterBook_DemonScythe_Counter = 0;
			}
			CanShootItem = false;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.BookofSkulls)
				.AddIngredient(ItemID.WaterBolt)
				.Register();
		}
	}
	internal class SinisterBolt : SynergyModProjectile {
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.light = 0.8f;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 6;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
		}
		Vector2 MousePosFixed;
		bool DirectionFace;
		int count = 0;
		int CountMain = 0;
		int CountCount = 0;
		bool CheckRotation;
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			if (CountMain == 0) {
				MousePosFixed = Main.MouseWorld;
				DirectionFace = player.direction == 1;
			}
			CountMain++;
			if (CountMain == 10)
				CheckRotation = Projectile.velocity.Y < 0;
			if (CountMain > 10)
				Projectile.velocity = !DirectionFace ?
					CheckRotation ? Projectile.velocity.RotatedBy(MathHelper.ToRadians(1f)) : Projectile.velocity.RotatedBy(MathHelper.ToRadians(-1f)) :
					CheckRotation ? Projectile.velocity.RotatedBy(MathHelper.ToRadians(-1f)) : Projectile.velocity.RotatedBy(MathHelper.ToRadians(1f));
			if (CountMain >= 120) {
				if (Math.Round(Projectile.velocity.X, 2) != 0 && Math.Round(Projectile.velocity.Y, 2) != 0 && count == 0) {
					Projectile.velocity -= Projectile.velocity * 0.01f;
				}
				else {
					count++;
				}
				if (count >= 12) {
					CountCount++;
					Projectile.velocity = (MousePosFixed - Projectile.Center).SafeNormalize(Vector2.UnitX) * CountCount * .033333f;
					if (Vector2.Distance(MousePosFixed, Projectile.Center) <= 5) {
						Projectile.Kill();
					}
				}
			}
		}
		public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
			if (SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<SinisterBook>(), ItemID.DemonScythe)) {
				modplayer.SinisterBook_DemonScythe_Counter++;
			}
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<GhostHitBox>(), Projectile.damage, 0, Projectile.owner);
			for (int i = 0; i < 25; i++) {
				Vector2 Rotate = Main.rand.NextVector2Circular(9f, 9f);
				int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, Rotate.X, Rotate.Y, 0, default, Main.rand.NextFloat(0.75f, 1f));
				Main.dust[dustnumber].noGravity = true;
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			Projectile.DrawTrailWithoutColorAdjustment(lightColor, .02f);
			return true;
		}
	}
}
