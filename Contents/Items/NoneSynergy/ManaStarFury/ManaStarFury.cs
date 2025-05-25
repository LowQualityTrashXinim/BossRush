using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.NoneSynergy.ManaStarFury {
	internal class ManaStarFury : ModItem {
		public override void SetDefaults() {
			Item.BossRushSetDefault(34, 40, 35, 6f, 20, 20, ItemUseStyleID.Swing, true);
			Item.DamageType = DamageClass.Melee;
			Item.useTurn = true;
			Item.rare = ItemRarityID.Orange;
			Item.shoot = ProjectileID.Starfury;
			Item.shootSpeed = 15;
			Item.value = Item.buyPrice(gold: 50);
			Item.UseSound = SoundID.Item1;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			var SkyPos = new Vector2(Main.MouseWorld.X + Main.rand.Next(-200, 200), Main.MouseWorld.Y - 800 + Main.rand.Next(-300, 100));
			var Aimto = Main.MouseWorld - SkyPos;
			var safeAim = Aimto.SafeNormalize(Vector2.UnitX);
			Projectile.NewProjectile(source, position, safeAim * velocity.Length(), ProjectileID.MagicMissile, (int)(damage * 1.67f), knockback, player.whoAmI);
			return false;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Starfury)
				.AddIngredient(ItemID.ManaCrystal)
				.AddIngredient(ItemID.BandofStarpower)
				.Register();
		}
	}
}
