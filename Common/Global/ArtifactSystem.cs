using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Items;
using System.Collections.Generic;
using BossRush.Contents.Items.Artifact;
using Terraria.ModLoader.IO;

namespace BossRush.Common.Global
{
    internal class ArtifactSystem : ModSystem
    {
    }
    class ArtifactGlobalItem : GlobalItem
    {
        public override bool CanUseItem(Item item, Player player)
        {
            if(item.ModItem is IArtifactItem)
            {
                if(item.consumable)
                {
                    return player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactCount < 1 && !NPC.downedSlimeKing;
                }
            }
            return base.CanUseItem(item, player);
        }
        public override bool? UseItem(Item item, Player player)
        {
            if(item.ModItem is IArtifactItem && item.consumable)
            {
                player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactCount++;
                return true;
            }
            return base.UseItem(item, player);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.ModItem is IArtifactItem)
            {
                if (item.consumable)
                {
                    tooltips.Add(new TooltipLine(Mod, "ArtifactCursed", "Can only consume before you beat any boss and only 1 of artifact can be consume"));
                }
            }
        }
    }
    class ArtifactPlayerHandleLogic : ModPlayer
    {
        public int ArtifactCount = 0;
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRushNetCodeHandle.MessageType.ArtifactRegister);
            packet.Write((byte)Player.whoAmI);
            packet.Write(ArtifactCount);
            packet.Send(toWho, fromWho);
        }
        public override void SaveData(TagCompound tag)
        {
            tag["ArtifactCount"] = ArtifactCount;
        }

        public override void LoadData(TagCompound tag)
        {
            ArtifactCount = (int)tag["ArtifactCount"];
        }
    }
}