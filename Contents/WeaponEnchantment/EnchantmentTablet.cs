using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Systems;

namespace BossRush.Contents.WeaponEnchantment;
internal class EnchantmentTablet : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.width = Item.height = 20;
		Item.useTime = Item.useAnimation = 15;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.rare = ItemRarityID.Red;
	}
	public override bool? UseItem(Player player) {
		return false;
	}
}
