using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Items.Accessories.EnragedBossAccessories.EvilEye;
using BossRush.Contents.Items.Artifact;
using System.Collections.Generic;

namespace BossRush.Common.Global
{
    class GlobalItemMod : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.EoCShield)
            {
                player.GetModPlayer<EvilEyePlayer>().EoCShieldUpgrade = true;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            ArtifactPlayerHandleLogic artifactplayer = Main.LocalPlayer.GetModPlayer<ArtifactPlayerHandleLogic>();
            if (artifactplayer.ArtifactDefinedID == 0)
            {
                TooltipLine line = new TooltipLine(Mod, "Can't Summon the boss",
                    $"You haven't use a artifact, please use at least one or use to continue [i:{ModContent.ItemType<BrokenArtifact>()}]");
                line.OverrideColor = BossRushModSystem.RedToBlack;
                tooltips.Add(line);
            }
        }
        public override bool CanUseItem(Item item, Player player)
        {
            ArtifactPlayerHandleLogic artifactplayer = player.GetModPlayer<ArtifactPlayerHandleLogic>();
            if(item.type == ItemID.SlimeCrown || item.type == ItemID.SuspiciousLookingEye)
            {
                return artifactplayer.ArtifactDefinedID != 0;
            }
            return base.CanUseItem(item, player);
        }
    }
}