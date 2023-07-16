using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Spawner
{
    public class PlanteraSpawn : BaseSpawnerItem
    {
        public override int[] NPCtypeToSpawn => new int[] { NPCID.Plantera };
        public override void SetSpawnerDefault(out int width, out int height)
        {
            width = 55; height = 52;
        }
        public override bool CanUseItem(Player player)
        {
            return Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && !NPC.AnyNPCs(NPCID.Plantera);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<PlanteraEssence>(), 3)
            .Register();
        }
    }
}