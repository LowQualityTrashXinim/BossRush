using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace BossRush.Contents.Items.Spawner {
	class PlanteraEssence : ModItem {
		public override void SetDefaults() {
			Item.height = 30;
			Item.width = 30;
			Item.material = true;
			Item.rare = ItemRarityID.LightPurple;
			Item.value = 0;
			Item.maxStack = 999;
		}
	}
}
