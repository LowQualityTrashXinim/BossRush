using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossRush.Contents.Items;
using System.Collections.Generic;
using BossRush.Contents.Items.Artifact;
using BossRush.Contents.Items.Chest;

namespace BossRush.Common.Global
{
    internal class ArtifactSystem : ModSystem
    {
        public override void AddRecipes()
        {
            ArtifactRecipe();
        }
        private static void ArtifactRecipe()
        {
            foreach (var itemSample in ContentSamples.ItemsByType)
            {
                ModItem item = itemSample.Value.ModItem;
                if (item is IArtifactItem)
                {
                    if (item is SkillIssuedArtifact)
                    {
                        item.CreateRecipe()
                        .AddIngredient(ModContent.ItemType<BrokenArtifact>())
                        .AddIngredient(ModContent.ItemType<PowerEnergy>())
                        .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                        .AddIngredient(ModContent.ItemType<WoodenTreasureChest>())
                        .Register();
                        continue;
                    }
                    item.CreateRecipe()
                        .AddIngredient(ModContent.ItemType<BrokenArtifact>())
                        .Register();
                }
            }
        }
    }
    class ArtifactGlobalItem : GlobalItem
    {
        public override bool CanUseItem(Item item, Player player)
        {
            if(item.ModItem is IArtifactItem)
            {
                if(item.consumable)
                {
                    return player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactCount < 1;
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
                    tooltips.Add(new TooltipLine(Mod, "ArtifactCursed", "Only 1 of artifact can be consume"));
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