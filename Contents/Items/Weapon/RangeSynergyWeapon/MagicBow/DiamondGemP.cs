using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow {
	internal class DiamondGemP : ModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Diamond);
		public override void SetDefaults() {
			Projectile.width = 18;
			Projectile.height = 16;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 1000;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = -1;
		}
		public override void AI() {
			if (Projectile.velocity != Vector2.Zero) { Projectile.velocity -= Projectile.velocity * 0.05f; }
			if (!Projectile.velocity.IsLimitReached(.1f)) Projectile.velocity = Vector2.Zero;
			if (Main.rand.NextBool(5)) {
				int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, 0, 0, 0, default, Main.rand.NextFloat(1f, 1.5f));
				Main.dust[dustnumber].noGravity = true;
				Main.dust[dustnumber].velocity = Main.rand.NextVector2Circular(4f, 4f);
			}
		}
		public override void OnKill(int timeLeft) {
			for (int i = 0; i < 15; i++) {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, 0, 0, 0, default, Main.rand.NextFloat(1f, 1.5f));
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(3f, 3f);
			}
		}
	}
}