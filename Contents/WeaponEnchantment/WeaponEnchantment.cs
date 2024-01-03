using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Systems;

namespace BossRush.Contents.WeaponEnchantment;
internal class WeaponEnchantment : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.width = Item.height = 20;
		Item.useTime = Item.useAnimation = 15;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.rare = ItemRarityID.Red;
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
			if (uiSystemInstance.userInterface.CurrentState == null) {
				uiSystemInstance.Enchant_uiState.WhoAmI = player.whoAmI;
				uiSystemInstance.userInterface.SetState(uiSystemInstance.Enchant_uiState);
			}
		}
		return false;
	}
}
