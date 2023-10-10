using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.EnhancedKatana {
	internal class PlatinumKatana : SynergyModItem {
		public override void SetDefaults() {
			Item.BossRushSetDefault(50, 52, 43, 4, 20, 20, ItemUseStyleID.Swing, true);
			Item.BossRushSetDefaultSpear(ModContent.ProjectileType<KatanaSlash>(), 3);
			Item.rare = 1;
			Item.value = Item.buyPrice(gold: 50);
			Item.UseSound = SoundID.Item1;
		}
		int count = 0;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, count);
			count++;
			return false;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Katana)
				.AddRecipeGroup("OreBroadSword")
				.Register();
		}
	}
	public class KatanaSlash : ModProjectile {
		protected virtual float HoldoutRangeMax => 50f;
		protected virtual float HoldoutRangeMin => 10f;
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 10;
		}
		public override void SetDefaults() {
			Projectile.DamageType = DamageClass.Melee;
			Projectile.width = 80;
			Projectile.height = 104;
			Projectile.penetrate = -1;
			Projectile.light = 0.1f;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.wet = false;
		}
		public override void AI() {
			SelectFrame();
			Projectile.ai[1]++;
			Player player = Main.player[Projectile.owner];
			player.heldProj = Projectile.whoAmI;
			int duration = player.itemAnimationMax;
			if (Projectile.timeLeft > duration) {
				Projectile.timeLeft = duration;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.ai[0] % 2 == 0) {
				if (Projectile.timeLeft == duration) {
					Projectile.spriteDirection = -Projectile.spriteDirection;
				}
				Projectile.rotation += MathHelper.ToRadians(180);
			}
			Projectile.velocity = Vector2.Normalize(Projectile.velocity);
			float halfDuration = duration * .5f;
			float progress = (duration - Projectile.timeLeft) / halfDuration;
			Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
			if (Projectile.ai[1] >= 5) {
				Projectile.scale -= 0.03f;
				Projectile.alpha += 20;
			}
		}
		public void SelectFrame() {
			if (++Projectile.frameCounter >= 1) {
				Projectile.frameCounter = 0;
				Projectile.frame += 1;
				if (Projectile.frame >= Main.projFrames[Projectile.type]) {
					Projectile.frame = Main.projFrames[Projectile.type] - 1;
				}
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.immune[Projectile.owner] = 8;
		}
	}
}