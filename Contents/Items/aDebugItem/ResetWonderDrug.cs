using BossRush.Contents.Items.Potion;
using BossRush.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.aDebugItem {
	internal class ResetWonderDrug : ModItem {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override void SetDefaults() {
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useTurn = true;
			Item.maxStack = 30;
			Item.consumable = true;
		}
		public override bool? UseItem(Player player) {
			player.GetModPlayer<WonderDrugPlayer>().DrugDealer = 0;
			player.statLife += player.statLifeMax2 - player.statLife;
			return true;
		}
	}
}
