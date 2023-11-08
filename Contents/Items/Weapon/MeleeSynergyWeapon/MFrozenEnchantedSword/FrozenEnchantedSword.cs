using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.MFrozenEnchantedSword {
	public class FrozenEnchantedSword : SynergyModItem {
		public override void SetDefaults() {
			Item.BossRushSetDefault(34, 40, 29, 7f, 15, 15, ItemUseStyleID.Swing, true);

			Item.DamageType = DamageClass.Melee;
			Item.rare = ItemRarityID.Orange;
			Item.shoot = ProjectileID.EnchantedBeam;
			Item.shootSpeed = 15;
			Item.value = Item.buyPrice(gold: 50);
			Item.UseSound = SoundID.Item1;
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.IceBolt, damage, knockback, player.whoAmI);
			CanShootItem = true;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.EnchantedSword)
				.AddIngredient(ItemID.IceBlade)
				.Register();
		}
	}
}
