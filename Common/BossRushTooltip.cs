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
            if (item.ModItem is ISynergyItem)
            {
                TooltipLine line = new TooltipLine(Mod, "Synergy", "Synergy Weapon");
                line.OverrideColor = BossRushModSystem.SynergyColor;
                tooltips.Add(line);
            }
            if (ModContent.GetInstance<BossRushModConfig>().DisableWeaponOverhaul)
            {
                return;
            }
            if (item.useStyle == BossRushUseStyle.GenericSwingDownImprove)
            {
                TooltipLine line = new TooltipLine(Mod, "SwingImprove", "Sword can swing in all direction");
                line.OverrideColor = Color.LightYellow;
                tooltips.Add(line);
            }
            if (item.useStyle == BossRushUseStyle.Swipe || item.useStyle == BossRushUseStyle.Poke)
            {
                TooltipLine line = new TooltipLine(Mod, "SwingImprove", "Sword can swing in all direction, on 3rd attack will do a special attack");
                line.OverrideColor = BossRushModSystem.YellowPulseYellowWhite;
                tooltips.Add(line);
            }
        }
    }
}
