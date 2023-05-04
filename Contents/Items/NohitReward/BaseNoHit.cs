using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.NohitReward
{
    abstract class BaseNoHit : ModItem
    {
        public const int HP = 50;
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LifeCrystal);
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.sellPrice(platinum: 5, gold: 0, silver: 0, copper: 0);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "NoHitReward",
                "Overcoming a small challenge, tho sadly not place-able \n" +
                "Reward for not getting hit \n" +
                "Increase max HP by 50 \n" +
                "Can only be uses once \n"
                ));
            foreach (TooltipLine line in tooltips)
            {
                if (line.Text == "challenge") line.OverrideColor = Main.DiscoColor;
            }
        }
    }
}
