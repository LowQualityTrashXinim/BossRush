using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.Swotaff {
	internal class AmethystSwotaff : SwotaffGemItem {
		public override void PreSetDefaults(out int damage, out int ProjectileType, out int ShootType) {
			damage = 15;
			ProjectileType = ModContent.ProjectileType<AmethystSwotaffP>();
			ShootType = ProjectileID.AmethystBolt;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.CopperBroadsword)
				.AddIngredient(ItemID.AmethystStaff)
				.Register();
		}
	}
	public class AmethystSwotaffP : SwotaffProjectile {
		public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<AmethystSwotaff>();
		public override void SwotaffCustomSetDefault(out float AltAttackAmountProjectile, out int AltAttackProjectileType, out int NormalBoltProjectile, out int DustType, out int ManaCost) {
			AltAttackAmountProjectile = 8;
			AltAttackProjectileType = ModContent.ProjectileType<AmethystSwotaffGemProjectile>();
			NormalBoltProjectile = ProjectileID.AmethystBolt;
			DustType = DustID.GemAmethyst;
			ManaCost = 50;
		}
	}
	public class AmethystSwotaffGemProjectile : SynergyModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Amethyst);
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 200;
			Projectile.penetrate = 3;
			Projectile.DamageType = DamageClass.Magic;
		}
		Vector2 firstframePos = Vector2.Zero;
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			if (Projectile.timeLeft == 200) {
				firstframePos = Main.MouseWorld;
			}
			if (Main.rand.NextBool(5)) {
				int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemAmethyst);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = Main.rand.NextFloat(1f);
			}
			if (++Projectile.ai[0] >= 20) {
				if (Projectile.ai[0] == 20) {
					firstframePos = (firstframePos - Projectile.Center).SafeNormalize(Vector2.Zero);
				}
				Projectile.velocity = firstframePos * 15f;
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			}
		}
		public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
			for (int i = 0; i < 20; i++) {
				int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemAmethyst);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = Main.rand.NextFloat(1.25f, 1.75f);
			}
		}
	}
}
