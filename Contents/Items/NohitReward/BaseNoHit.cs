using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.NohitReward
{
    abstract class BaseNoHit : ModItem
    {
        public const int HP = 50;
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("\"Overcoming a small challenge, tho sadly not place-able\"\nReward for not getting hit\nIncrease max HP by 50\nCan only be uses once");
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LifeCrystal);
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.sellPrice(platinum: 5, gold: 0, silver: 0, copper: 0);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Text == "challenge") line.OverrideColor = Main.DiscoColor;
            }
        }
    }
}
