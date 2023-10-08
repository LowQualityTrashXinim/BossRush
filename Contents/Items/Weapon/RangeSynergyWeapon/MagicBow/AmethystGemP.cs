using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow {
	internal class AmethystGemP : ModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Amethyst);
		public override void SetDefaults() {
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.light = 1f;
		}
		int count = 0;
		public override void AI() {
			if (Main.rand.NextBool(7)) {
				int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemAmethyst, Projectile.velocity.X + Main.rand.Next(-5, 5), Projectile.velocity.Y + Main.rand.Next(-5, 5), 0, default, Main.rand.NextFloat(0.75f, 1.25f));
				Main.dust[dustnumber].noGravity = true;
			}
			if (Projectile.velocity != Vector2.Zero && count == 0) {
				Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.Y * 5);
				Projectile.velocity *= 0.95f;
			}
			if (!Projectile.velocity.IsLimitReached(1)) {
				Projectile.velocity = Vector2.Zero;
				count++;
			}
			if (count >= 1) {
				Projectile.ai[0]++;
				if (Projectile.ai[0] >= 30) {
					Projectile.netUpdate = true;
					Projectile.tileCollide = true;
					Projectile.penetrate = 1;

					if (Projectile.velocity.Y < 16) Projectile.velocity.Y += 1f;
				}
			}
		}
		public override void OnKill(int timeLeft) {
			for (int i = 0; i < 15; i++) {
				Vector2 RandomCircular = Main.rand.NextVector2Circular(5.5f, 5.5f);
				Vector2 newVelocity = new Vector2(RandomCircular.X, RandomCircular.Y);
				int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemAmethyst, newVelocity.X, newVelocity.Y, 0, default, Main.rand.NextFloat(1.75f, 2.25f));
				Main.dust[dustnumber].noGravity = true;
			}
		}
	}
}