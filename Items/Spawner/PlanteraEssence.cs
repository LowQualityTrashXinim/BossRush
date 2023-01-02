using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Spawner
{
    class PlanteraEssence : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 30;
            Item.width = 30;
            Item.material = true;
            Item.rare = 6;
            Item.value = 0;
            Item.maxStack = 999;
        }
    }
}
