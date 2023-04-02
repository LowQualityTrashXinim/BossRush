using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Items.Potion;

namespace BossRush.Items.aDebugItem
{
    internal class ResetWonderDrug : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Real Vitamin");
            Tooltip.SetDefault("Reset Wonder drug value back to normal");
        }
        public override void SetDefaults() 
        {
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.consumable = true;
        }
        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<WonderDrugPlayer>().DrugDealer = 0;
            player.statLife += player.statLifeMax2 - player.statLife;
            return true;
        }
    }
}
