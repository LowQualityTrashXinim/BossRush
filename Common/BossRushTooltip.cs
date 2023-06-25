using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Global;
using BossRush.Contents.Items;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Common
{
    internal class BossRushTooltip : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            //BossRushModConfig config = ModContent.GetInstance<BossRushModConfig>();
            //if(!config.SynergyMode)
            //{
            //    return;
            //}
            //if (item.ModItem is ISynergyItem)
            //{
            //    TooltipLine line = new TooltipLine(Mod, "Synergy", "Synergy Weapon");
            //    line.OverrideColor = BossRushColor.MultiColor(new List<Color> {new Color(50,175,175), Color.White }, 3);
            //    tooltips.Add(line);
            //}
        }
    }
}
