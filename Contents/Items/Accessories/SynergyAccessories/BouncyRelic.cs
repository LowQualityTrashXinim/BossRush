using BossRush.Contents.Items.Weapon;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.SynergyAccessories {
	internal class BouncyRelic : SynergyModItem {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Item.DefaultToAccessory(30, 30);
			Item.value = 1000000;
		}
		public override void UpdateEquip(Player player) {
			player.endurance += .15f;
			player.GetModPlayer<PlayerRelic>().Bouncy = true;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.KingSlimeTrophy)
				.AddIngredient(ItemID.SlimeGun)
				.Register();
		}
	}
	public class PlayerRelic : ModPlayer {
		public bool Bouncy = false;
		public override void ResetEffects() {
			Bouncy = false;
		}
	}
	public class BouncyProjectileGlobal : GlobalProjectile {
		public override bool InstancePerEntity => true;
		int counter = 0;
		public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity) {
			var player = Main.player[projectile.owner];
			if (player.GetModPlayer<PlayerRelic>().Bouncy && !projectile.minion && !projectile.hostile) {
				projectile.tileCollide = true;
				Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
				if (projectile.velocity.X != oldVelocity.X) 					projectile.velocity.X = -oldVelocity.X;
				if (projectile.velocity.Y != oldVelocity.Y) 					projectile.velocity.Y = -oldVelocity.Y;
				if (projectile.timeLeft > 180) 					projectile.timeLeft = 180;
				counter++;
				if (counter > 10) 					return false;
				projectile.damage = (int)(projectile.damage * 1.2f);
				return false;
			}
			return true;
		}
		public override void PostAI(Projectile projectile) {
		}
	}
}
