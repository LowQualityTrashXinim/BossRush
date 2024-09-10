using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Consumable.Spawner {
	public class PlanteraSpawn : BaseSpawnerItem {
		public override int[] NPCtypeToSpawn => new int[] { NPCID.Plantera };
		public override void SetSpawnerDefault(out int width, out int height) {
			width = 55; height = 52;
		}
		public override bool CanUseItem(Player player) {
			return !NPC.AnyNPCs(NPCID.Plantera);
		}
		public override void AddRecipes() {
			CreateRecipe()
			.AddIngredient(ModContent.ItemType<PlanteraEssence>(), 3)
			.Register();
		}
	}
}
