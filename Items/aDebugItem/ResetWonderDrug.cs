using BossRush.Items.Potion;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Items.aDebugItem
{
    internal class ResetWonderDrug : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Real Vitamin");
            Tooltip.SetDefault("Reset Wonder drug value back to normal");
        }
        public override void SetDefaults() { }
        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<WonderDrugPlayer>().DrugDealer = 0;
            return true;
        }
    }
}
