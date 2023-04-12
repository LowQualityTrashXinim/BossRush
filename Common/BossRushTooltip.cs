using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using BossRush.Common.Global;

namespace BossRush.Common
{
    internal class BossRushTooltip : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
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
                TooltipLine line = new TooltipLine(Mod, "SwingImprove", $"Sword can swing in all direction, on 3rd attack will do a special attack");
                line.OverrideColor = BossRushRecipe.YellowPulseYellowWhite;
                tooltips.Add(line);
            }
            Player player = Main.LocalPlayer;
            RangerWeaponOverhaulPlayerDataHandle modplayer = player.GetModPlayer<RangerWeaponOverhaulPlayerDataHandle>();

        }
    }
}
