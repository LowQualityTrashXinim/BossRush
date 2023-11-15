using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
//TODO : remove the current weapon modification system
namespace BossRush.Contents.Items {
	internal class PowerEnergy : ModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultToConsume(54, 20);
			Item.rare = ItemRarityID.Red;
		}
	}
}
