﻿using Terraria;
using Terraria.ID;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Card
{
    internal class SolarCard : Card
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
    internal class VortexCard : Card
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
    internal class NebulaCard : Card
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
    internal class StarDustCard : Card
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
    internal class ResetCard : Card
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
            modplayer.ChestLoot.MeleeChanceMutilplier = 0;
            modplayer.ChestLoot.RangeChanceMutilplier = 0;
            modplayer.ChestLoot.MagicChanceMutilplier = 0;
            modplayer.ChestLoot.SummonChanceMutilplier = 0;
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
        }
    }
    internal class CopperCard : Card
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.CopperBar);
        public override void PostCardSetDefault()
        {
            Item.rare = ItemRarityID.Red;
        }
        public override int Tier => 1;
        public List<PlayerStats> stats = new List<PlayerStats>();
        public List<float> statsNumber = new List<float>();
        public override void OnTierItemSpawn()
        {
            base.OnTierItemSpawn();
            for (int i = 0; i < Tier; i++)
            {
                stats.Add(SetStatsToAddBaseOnTier());
                statsNumber.Add(statsCalculator(stats[i]));
            }
        }
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
        }
        public override List<PlayerStats> CardStats => stats;
        public override List<float> CardStatsNumber => statsNumber;
        public override bool CanBeCraft => false;
    }
    internal class SilverCard : Card
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.SilverBar);
        public override void PostCardSetDefault()
        {
            Item.rare = ItemRarityID.Red;
        }
        public override int Tier => 2;
        public List<PlayerStats> stats = new List<PlayerStats>();
        public List<float> statsNumber = new List<float>();
        public override void OnTierItemSpawn()
        {
            base.OnTierItemSpawn();
            for (int i = 0; i < Tier; i++)
            {
                stats.Add(SetStatsToAddBaseOnTier());
                statsNumber.Add(statsCalculator(stats[i]));
            }
        }
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
        }
        public override List<PlayerStats> CardStats => stats;
        public override List<float> CardStatsNumber => statsNumber;
        public override bool CanBeCraft => false;
    }
    internal class GoldCard : Card
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.GoldBar);
        public override void PostCardSetDefault()
        {
            Item.rare = ItemRarityID.Red;
        }
        public override int Tier => 3;
        public List<PlayerStats> stats = new List<PlayerStats>();
        public List<float> statsNumber = new List<float>();
        public override void OnTierItemSpawn()
        {
            base.OnTierItemSpawn();
            for (int i = 0; i < Tier; i++)
            {
                stats.Add(SetStatsToAddBaseOnTier());
                statsNumber.Add(statsCalculator(stats[i]));
            }
        }
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
        }
        public override List<PlayerStats> CardStats => stats;
        public override List<float> CardStatsNumber => statsNumber;
        public override bool CanBeCraft => false;
    }
    internal class PlatinumCard : Card
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.PlatinumBar);
        public override void PostCardSetDefault()
        {
            Item.rare = ItemRarityID.Red;
        }
        public override int Tier => 4;
        public List<PlayerStats> stats = new List<PlayerStats>();
        public List<float> statsNumber = new List<float>();
        public override void OnTierItemSpawn()
        {
            base.OnTierItemSpawn();
            for (int i = 0; i < Tier; i++)
            {
                stats.Add(SetStatsToAddBaseOnTier());
                statsNumber.Add(statsCalculator(stats[i]));
            }
        }
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
        }
        public override List<PlayerStats> CardStats => stats;
        public override List<float> CardStatsNumber => statsNumber;
        public override bool CanBeCraft => false;
    }
    internal class EmptyCard : Card
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
}