using BossRush.Common.RoguelikeChange.ItemOverhaul;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.NoneSynergy.FrozenEnchantedSword {
	public class FrozenEnchantedSword : ModItem {
		public override void SetDefaults() {
			Item.BossRushSetDefault(34, 40, 29, 7f, 15, 15, ItemUseStyleID.Swing, true);

			Item.DamageType = DamageClass.Melee;
			Item.rare = ItemRarityID.Orange;
			Item.shoot = ProjectileID.EnchantedBeam;
			Item.shootSpeed = 15;
			Item.value = Item.buyPrice(gold: 50);
			Item.UseSound = SoundID.Item1;
			if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul global)) {
				global.SwingType = BossRushUseStyle.Swipe;
			}
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.IceBolt, damage, knockback, player.whoAmI);
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
		//public override void AddRecipes() {
		//	CreateRecipe()
		//		.AddIngredient(ItemID.EnchantedSword)
		//		.AddIngredient(ItemID.IceBlade)
		//		.Register();
		//}
	}
}
