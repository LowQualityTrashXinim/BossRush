using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.Swotaff {
	internal class EmeraldSwotaff : SwotaffGemItem {
		public override void PreSetDefaults(out int damage, out int ProjectileType, out int ShootType) {
			damage = 16;
			ProjectileType = ModContent.ProjectileType<EmeraldSwotaffP>();
			ShootType = ProjectileID.EmeraldBolt;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.TungstenBroadsword)
				.AddIngredient(ItemID.EmeraldStaff)
				.Register();
		}
	}
	public class EmeraldSwotaffP : SwotaffProjectile {
		public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<EmeraldSwotaff>();
		public override void SwotaffCustomSetDefault(out float AltAttackAmountProjectile, out int AltAttackProjectileType, out int NormalBoltProjectile, out int DustType, out int ManaCost) {
			AltAttackAmountProjectile = 5;
			AltAttackProjectileType = ModContent.ProjectileType<EmeraldSwotaffGemProjectile>();
			NormalBoltProjectile = ProjectileID.EmeraldBolt;
			DustType = DustID.GemEmerald;
			ManaCost = 75;
		}
	}
	public class EmeraldSwotaffGemProjectile : SynergyModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Emerald);
		public override void SetDefaults() {
			Projectile.width = 18;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 300;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Magic;
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			Projectile.velocity = (Projectile.Center - player.Center).SafeNormalize(Vector2.Zero) * 2f;

			for (int i = 0; i < 10; i++) {
				int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2CircularEdge(200f, 200f), 0, 0, DustID.GemEmerald);
				Main.dust[dust].noGravity = true;
			}
			if (Projectile.ai[0] > 0) {
				Projectile.ai[0]--;
				return;
			}
			if (Projectile.position.LookForHostileNPC(out NPC npc, 200)) {
				if (npc is null) {
					return;
				}
				npc.StrikeNPC(npc.CalculateHitInfo((int)(Projectile.damage * .25f), 1, false, 0, DamageClass.Magic, false));
				Projectile.ai[0] = 20;
			}
		}
	}
}