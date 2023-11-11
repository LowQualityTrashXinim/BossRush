using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
//TODO : remove the current weapon modification system
namespace BossRush.Contents.WeaponModification {
	internal class PowerEnergy : ModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultToConsume(54, 20);
			Item.rare = ItemRarityID.Red;
		}
		public override bool? UseItem(Player player) {
			if (player.altFunctionUse != 2) {
				WeaponModificationSystem uiSystemInstance = ModContent.GetInstance<WeaponModificationSystem>();
				uiSystemInstance.WM_PowerUp.whoAmI = player.whoAmI;
				uiSystemInstance.userInterface.SetState(uiSystemInstance.WM_PowerUp);
			}
			return true;
		}
	}
}
