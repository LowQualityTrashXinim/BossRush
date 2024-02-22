using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace BossRush.Contents.Items {
	internal class PowerEnergy : ModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultToConsume(54, 20);
			Item.rare = ItemRarityID.Red;
			Item.material = true;
		}
	}
}
