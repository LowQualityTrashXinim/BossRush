using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.Swotaff {
	internal class TopazSwotaff : SwotaffGemItem {
		public override void PreSetDefaults(out int damage, out int ProjectileType, out int ShootType) {
			damage = 15;
			ProjectileType = ModContent.ProjectileType<TopazSwotaffProjectile>();
			ShootType = ProjectileID.TopazBolt;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.TinBroadsword)
				.AddIngredient(ItemID.TopazStaff)
				.Register();
		}
	}
	public class TopazSwotaffProjectile : SwotaffProjectile {
		public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<TopazSwotaff>();
		public override void SwotaffCustomSetDefault(out float AltAttackAmountProjectile, out int AltAttackProjectileType, out int NormalBoltProjectile, out int DustType, out int ManaCost) {
			AltAttackAmountProjectile = 4;
			AltAttackProjectileType = ModContent.ProjectileType<TopazGemProjectile>();
			NormalBoltProjectile = ProjectileID.TopazBolt;
			DustType = DustID.GemTopaz;
			ManaCost = 50;
		}
	}
	public class TopazGemProjectile : SynergyModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Topaz);
		public override void SetDefaults() {
			Projectile.width = 18;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 500;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Magic;
		}
		int cooktimer = 30;
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			Projectile.velocity -= Projectile.velocity * .05f;
			if (cooktimer <= 0) {
				Projectile.damage += 5;
				for (int i = 0; i < 15; i++) {
					int dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.GemTopaz);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = Main.rand.NextVector2Circular(2f, 2f);
				}
				cooktimer = 30;
				return;
			}
			cooktimer--;
		}
		public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<GhostHitBox>(), Projectile.damage, 0, Projectile.owner);
			for (int i = 0; i < 25; i++) {
				Vector2 RandomCircular = Main.rand.NextVector2Circular(4f, 4f);
				Vector2 newVelocity = new Vector2(RandomCircular.X, RandomCircular.Y);
				int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemTopaz, newVelocity.X, newVelocity.Y, 0, default, Main.rand.NextFloat(1.75f, 2.25f));
				Main.dust[dustnumber].noGravity = true;
			}
		}
	}
}
