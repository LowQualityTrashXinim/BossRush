using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Items.Potion;

namespace BossRush.Items.aDebugItem
{
    internal class ResetWonderDrug : ModItem
    {
        public override string Texture => ItemTexture.MISSINGTEXTURE;
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
