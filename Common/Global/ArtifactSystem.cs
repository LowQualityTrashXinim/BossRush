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
            if (item.ModItem is IArtifactItem)
            {
                if (item.consumable)
                {
                    return player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID < 1;
                }
            }
            return base.CanUseItem(item, player);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.ModItem is IArtifactItem)
            {
                if (item.consumable)
                {
                    tooltips.Add(new TooltipLine(Mod, "ArtifactCursed", "Only 1 artifact can be consume"));
                }
                if(Main.LocalPlayer.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID != 0)
                {
                    tooltips.Add(new TooltipLine(Mod, "ArtifactAlreadyConsumed", "You can't no longer consume anymore artifact"));
                }
            }
        }
    }
    class ArtifactPlayerHandleLogic : ModPlayer
    {
        public int ArtifactDefinedID = 0;
        bool Greed = false;//ID = 1
        bool Pride = false;//ID = 2
        public override void ResetEffects()
        {
            Greed = ArtifactDefinedID == 1 ? true : false;
            Pride = ArtifactDefinedID == 2 ? true : false;
        }
        public override void PreUpdate()
        {
            switch (ArtifactDefinedID)
            {
                case 1:
                    Greed = true;
                    break;
                case 2:
                    Pride = true;
                    break;
            }
        }
        public override void PostUpdate()
        {
            if (Greed)
            {
                Player.GetModPlayer<ChestLootDropPlayer>().amountModifier += 4;
            }
            if (Pride)
            {
                Player.GetModPlayer<ChestLootDropPlayer>().multiplier = true;
                Player.GetModPlayer<ChestLootDropPlayer>().amountModifier = .5f;
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (Greed)
            {
                damage *= .65f;
            }
            if (Pride)
            {
                damage += .45f;
            }
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRushNetCodeHandle.MessageType.ArtifactRegister);
            packet.Write((byte)Player.whoAmI);
            packet.Write(ArtifactDefinedID);
            packet.Send(toWho, fromWho);
        }
        public override void SaveData(TagCompound tag)
        {
            tag["ArtifactDefinedID"] = ArtifactDefinedID;
        }
        public override void LoadData(TagCompound tag)
        {
            ArtifactDefinedID = (int)tag["ArtifactDefinedID"];
        }
    }
}