using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items {
	internal class SynergyEnergy : ModItem {
		public override void SetDefaults() {
			Item.rare = ItemRarityID.Red;
			Item.width = 54;
			Item.height = 20;
			Item.material = true;
		}
	}
}
