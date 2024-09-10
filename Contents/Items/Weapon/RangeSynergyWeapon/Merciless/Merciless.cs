using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.RoguelikeChange.ItemOverhaul;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Merciless {
	internal class Merciless : SynergyModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(102, 26, 20, 5f, 15, 30, ItemUseStyleID.Shoot, ProjectileID.Bullet, 8, true, AmmoID.Bullet);
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 50);
			Item.crit = 10;
			Item.reuseDelay = 30;
			Item.scale -= 0.15f;
			Item.UseSound = SoundID.Item38;
			if (Item.TryGetGlobalItem(out RangeWeaponOverhaul weapon)) {
				weapon.SpreadAmount = 20;
				weapon.AdditionalSpread = 3;
				weapon.AdditionalMulti = .55f;
				weapon.OffSetPost = 60;
				weapon.NumOfProjectile = 10;
				weapon.itemIsAShotgun = true;
			}
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-26, 0);
		}
		public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (type == ProjectileID.Bullet) {
				type = ProjectileID.ExplosiveBullet;
			}
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			if (++player.GetModPlayer<Item_MercilessPlayer>().count == 1) {
				Projectile.NewProjectile(source, position, velocity * 1.5f, ProjectileID.CannonballFriendly, damage * 4, knockback, player.whoAmI);
			}
			else {
				player.GetModPlayer<Item_MercilessPlayer>().count = 0;
			}
			CanShootItem = false;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Boomstick)
				.AddIngredient(ItemID.QuadBarrelShotgun)
				.Register();
		}
	}
	public class Item_MercilessPlayer : ModPlayer {
		public int count = 0;
		public override void Initialize() {
			count = 0;
		}
	}
}
