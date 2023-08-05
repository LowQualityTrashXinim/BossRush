using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.ModLoader;
using System;

namespace BossRush.Contents.Items.Card
{
    internal class SolarCard : CardItem
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.FragmentSolar);
        public override void PostCardSetDefault()
        {
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Red;
        }
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
            modplayer.ChestLoot.MeleeChanceMutilplier += .5f;
        }
    }
    internal class VortexCard : CardItem
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.FragmentVortex);
        public override void PostCardSetDefault()
        {
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Red;
        }
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
            modplayer.ChestLoot.RangeChanceMutilplier += .5f;
        }
    }
    internal class NebulaCard : CardItem
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.FragmentNebula);
        public override void PostCardSetDefault()
        {
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Red;
        }
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
            modplayer.ChestLoot.MagicChanceMutilplier += .5f;
        }
    }
    internal class StarDustCard : CardItem
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.FragmentStardust);
        public override void PostCardSetDefault()
        {
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Red;
        }
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
            modplayer.ChestLoot.SummonChanceMutilplier += .5f;
        }
    }
    internal class ResetCard : CardItem
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.LunarBar);
        public override void PostCardSetDefault()
        {
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Red;
        }
        public override bool CanBeCraft => false;
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
            modplayer.ChestLoot.MeleeChanceMutilplier = 1;
            modplayer.ChestLoot.RangeChanceMutilplier = 1;
            modplayer.ChestLoot.MagicChanceMutilplier = 1;
            modplayer.ChestLoot.SummonChanceMutilplier = 1;
            modplayer.MeleeDMG = 0;
            modplayer.RangeDMG = 0;
            modplayer.MagicDMG = 0;
            modplayer.SummonDMG = 0;
            modplayer.Movement = 0;
            modplayer.JumpBoost = 0;
            modplayer.HPMax = 0;
            modplayer.HPRegen = 0;
            modplayer.ManaMax = 0;
            modplayer.ManaRegen = 0;
            modplayer.DefenseBase = 0;
            modplayer.DamagePure = 0;
            modplayer.CritStrikeChance = 0;
            modplayer.CritDamage = 1;
            modplayer.DefenseEffectiveness = 1;
            modplayer.DropAmountIncrease = 0;
            modplayer.MinionSlot = 0;
            modplayer.SentrySlot = 0;
            modplayer.CardLuck = 0;
            modplayer.listCursesID.Clear();
        }
    }
    internal class CopperCard : CardItem
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.CopperBar);
        public override void PostCardSetDefault()
        {
            Item.rare = ItemRarityID.Red;
            Item.maxStack = 99;
        }
        public override int Tier => 1;
        public override bool CanBeCraft => false;
    }
    internal class SilverCard : CardItem
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.SilverBar);
        public override void PostCardSetDefault()
        {
            Item.rare = ItemRarityID.Red;
            Item.maxStack = 99;
        }
        public override int Tier => 2;
        public override bool CanBeCraft => false;
    }
    internal class GoldCard : CardItem
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.GoldBar);
        public override void PostCardSetDefault()
        {
            Item.rare = ItemRarityID.Red;
            Item.maxStack = 99;
        }
        public override int Tier => 3;
        public override bool CanBeCraft => false;
    }
    internal class PlatinumCard : CardItem
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.PlatinumBar);
        public override void PostCardSetDefault()
        {
            Item.rare = ItemRarityID.Red;
            Item.maxStack = 99;
        }
        public override int Tier => 4;
        public override bool CanBeCraft => false;
    }
    class CopperCardNormalizer : CardItem
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.CopperBrickWall);
        public override void PostCardSetDefault()
        {
            Item.rare = ItemRarityID.Red;
            Item.maxStack = 99;
        }
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
            modplayer.CardLuck = Math.Clamp(modplayer.CardLuck - Main.rand.Next(11), 0, 200);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CopperCard>(), 10)
                .Register();
        }
    }
    class SilverCardNormalizer : CardItem
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.SilverBrickWall);
        public override void PostCardSetDefault()
        {
            Item.rare = ItemRarityID.Red;
            Item.maxStack = 99;
        }
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
            modplayer.CardLuck = Math.Clamp(modplayer.CardLuck - Main.rand.Next(5, 21), 0, 200);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SilverCard>(), 10)
                .Register();
        }
    }
    class GoldCardNormalizer : CardItem
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.GoldBrickWall);
        public override void PostCardSetDefault()
        {
            Item.rare = ItemRarityID.Red;
            Item.maxStack = 99;
        }
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
            modplayer.CardLuck = Math.Clamp(modplayer.CardLuck - Main.rand.Next(15, 36), 0, 200);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GoldCard>(), 10)
                .Register();
        }
    }
    class PlatinumCardNormalizer : CardItem
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.PlatinumBrickWall);
        public override void PostCardSetDefault()
        {
            Item.rare = ItemRarityID.Red;
            Item.maxStack = 99;
        }
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
            modplayer.CardLuck = Math.Clamp(modplayer.CardLuck - Main.rand.Next(25, 66), 0, 200);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PlatinumCard>(), 10)
                .Register();
        }
    }

    internal class EmptyCard : CardItem
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Glass);
        public override void SetDefaults()
        {
            Item.width = 0;
            Item.height = 0;
            Item.material = true;
            Item.maxStack = 99;
        }
        public override bool CanBeCraft => false;
    }
    internal class CursedCard : CardItem
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.HellstoneBar);
        public override void SetDefaults()
        {
            Item.width = 1;
            Item.height = 1;
            Item.material = true;
            Item.maxStack = 99;
            Item.useTime = Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
        int cursesID = 1;
        public override void ModifyCardToolTip(ref List<TooltipLine> tooltips, PlayerCardHandle modplayer)
        {
            tooltips.Add(new TooltipLine(Mod, "CursesShowcase", "Current cursed : " + modplayer.CursedStringStats(cursesID)));
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        //public override bool CanRightClick() => true;
        //public override void RightClick(Player player)
        //{
        //    int item = player.QuickSpawnItem(null, ModContent.ItemType<CursedCard>());
        //    if (Main.item[item].ModItem is CursedCard card)
        //    {
        //        if (card.cursesID > 12)
        //        {
        //            card.cursesID = 0;
        //        }
        //        else
        //        {
        //            card.cursesID++;
        //        }
        //    }
        //}
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
            if (player.altFunctionUse == 2)
            {
                if (cursesID > 12)
                {
                    cursesID = 1;
                }
                else
                {
                    cursesID++;
                }
            }
            else
            {
                if (!modplayer.listCursesID.Contains(CursedID))
                {
                    modplayer.listCursesID.Add(cursesID);
                }
            }
        }
        public override bool CanBeCraft => false;
    }
}