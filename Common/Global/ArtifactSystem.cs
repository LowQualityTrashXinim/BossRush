using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Items;
using System.Collections.Generic;
using BossRush.Contents.Items.Artifact;

namespace BossRush.Common.Global
{
    internal class ArtifactSystem : ModSystem
    {
    }
    class ArtifactGlobalItem : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.ModItem is IArtifactItem)
            {
                if (item.consumable)
                {
                    tooltips.Add(new TooltipLine(Mod, "ArtifactCursed", "Can only consume before you beat any boss and only 1 can be consume"));
                }
                if (item.accessory)
                {
                    tooltips.Add(new TooltipLine(Mod, "ArtifactCursed", "Once equipped, effect will never disappear and stay active"));
                }
            }
        }
    }
    class ArtifactPlayerHandleLogic : ModPlayer
    {
        /// <summary>
        /// This bool is to check if artifact can be active, use mostly to change the value of item can be drop from chest
        /// </summary>
        public bool ArtifactAllowance = false;
        /// <summary>
        /// This bool is to check whenever if player remove artifact mid fight in boss and then get it back in the game
        /// <br/>Useful to prevent confliction between 2 artifacts that modify player damage
        /// </summary>
        public bool ForceArtifact = true;
        /// <summary>
        /// This is to see if player have more artifact than they need, useful if you want artifact to not contradict each other
        /// </summary>
        public int ArtifactCount = 0;
        //ArtifactList
        int[] ArtifactList = new int[]{
            ModContent.ItemType<TokenofGreed>(),
            ModContent.ItemType<TokenofPride>(),
            ModContent.ItemType<SkillIssuedArtifact>(),
            ModContent.ItemType<GodDice>(),
            ModContent.ItemType<VampirismCrystal>() };
        private void ArtifactHandle()
        {
            if (ArtifactCount == 1 && ForceArtifact)
            {
                ArtifactAllowance = true;
            }
            else
            {
                ArtifactAllowance = false;
                ForceArtifact = false;
            }
        }

        public override void PostUpdate()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if ((npc.boss ||
                    npc.type == NPCID.EaterofWorldsBody
                    || npc.type == NPCID.EaterofWorldsHead
                    || npc.type == NPCID.EaterofWorldsTail)
                    && npc.active)
                {
                    // What happen when boss is alive
                    ArtifactHandle();
                }
                else if (i == Main.maxNPCs - 1 && Player.GetModPlayer<ModdedPlayer>().HowManyBossIsAlive == 0) // What happen when boss is inactive
                {
                    ForceArtifact = true;
                }
            }
            ArtifactCount = 0;
            for (int i = 0; i < ArtifactList.Length; i++)
            {
                if (Player.HasItem(ArtifactList[i])) ArtifactCount++;
            }
        }
    }
}