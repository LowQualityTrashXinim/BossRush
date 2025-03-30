using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.LegacyItem.TrueEnchantedSword {
	internal class TrueEnchantedSwordBeam : ModProjectile {
		public override void SetDefaults() {
			Projectile.width = 56;
			Projectile.height = 56;
			Projectile.friendly = true;
			Projectile.light = 5f;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.extraUpdates = 6;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}

		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		}

		public override void OnKill(int timeLeft) {
			var player = Main.player[Projectile.owner];
			var source = new EntitySource_ItemUse_WithAmmo(player, new Item(ModContent.ItemType<TrueEnchantedSword>()), AmmoID.Arrow);
			float proNum = 32f;
			float rotate = MathHelper.ToRadians(180);
			for (int i = 0; i < proNum; i++) {
				var Rotate = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.Lerp(rotate, -rotate, i / (proNum - 1)));
				Projectile.NewProjectile(source, Projectile.position, Rotate * 2f, ProjectileID.EnchantedBeam, (int)(Projectile.damage * 0.35f), Projectile.knockBack, player.whoAmI);
			}
			for (int i = 0; i < 10; i++) {
				var SkyPos = new Vector2(Projectile.position.X + Main.rand.Next(-200, 200), Projectile.position.Y - 800 + Main.rand.Next(-300, 100));
				var Aimto = Main.MouseWorld - SkyPos;
				SkyPos += new Vector2(Main.rand.Next(-200, 200), Main.rand.Next(-200, 200));
				var safeAim = Aimto.SafeNormalize(Vector2.UnitX);
				Projectile.NewProjectile(source, SkyPos, safeAim * 14f, ProjectileID.SuperStar, (int)(Projectile.damage * 1.1f), Projectile.knockBack, player.whoAmI);
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			Projectile.DrawTrail(lightColor);
			return true;
		}

	}
}
