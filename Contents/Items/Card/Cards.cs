using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace BossRush.Contents.Items.Card
{
    internal class SolarCard : Card
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.FragmentSolar);
        public override void PostCardSetDefault()
        {
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
            Item.rare = ItemRarityID.Red;
        }
        public override void OnUseItem(Player player, PlayerCardHandle modplayer)
        {
            modplayer.ChestLoot.MeleeChanceMutilplier = 1f;
            modplayer.ChestLoot.RangeChanceMutilplier = 1f;
            modplayer.ChestLoot.MagicChanceMutilplier = 1f;
            modplayer.ChestLoot.SummonChanceMutilplier = 1f;
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
        }
        public override bool CanBeCraft => false;
    }
}
