using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using System.Collections.Generic;

namespace BossRush
{
    public static partial class BossRushUtils
    {
        public static bool IsDebugPlayer(this Player player) => 
            player.name.Contains("Test") || 
            player.name.Contains("Debug") || 
            player.name == "LowQualityTrashXinim" ||
            player.name.Contains("#Beta");

        public static bool DoesStatsRequiredWholeNumber(PlayerStats stats) =>
                    stats is PlayerStats.Defense
                    || stats is PlayerStats.MaxMinion
                    || stats is PlayerStats.MaxSentry
                    || stats is PlayerStats.MaxHP
                    || stats is PlayerStats.MaxMana
                    || stats is PlayerStats.CritChance
                    || stats is PlayerStats.ChestLootDropIncrease;
        public static PlayerStats SetStatsToAddBaseOnTier(List<PlayerStats> CardStats, int Tier)
        {
            List<PlayerStats> stats = new List<PlayerStats>();
            if (Tier >= 4)
            {
                //list.Add(PlayerStats.Luck);
            }
            if (Tier >= 3)
            {
                stats.Add(PlayerStats.MaxSentry);
                stats.Add(PlayerStats.MaxMinion);
                stats.Add(PlayerStats.ChestLootDropIncrease);
            }
            if (Tier >= 2)
            {
                stats.Add(PlayerStats.Thorn);
                stats.Add(PlayerStats.DefenseEffectiveness);
                stats.Add(PlayerStats.CritDamage);
                stats.Add(PlayerStats.CritChance);
                stats.Add(PlayerStats.DamageUniverse);
            }
            if (Tier >= 1)
            {
                stats.Add(PlayerStats.Defense);
                stats.Add(PlayerStats.RegenMana);
                stats.Add(PlayerStats.MaxMana);
                stats.Add(PlayerStats.RegenHP);
                stats.Add(PlayerStats.MaxHP);
                stats.Add(PlayerStats.MovementSpeed);
                stats.Add(PlayerStats.JumpBoost);
                stats.Add(PlayerStats.SummonDMG);
                stats.Add(PlayerStats.MagicDMG);
                stats.Add(PlayerStats.RangeDMG);
                stats.Add(PlayerStats.MeleeDMG);
            }
            if (CardStats.Count > 0 && CardStats.Count != stats.Count)
            {
                foreach (var item in CardStats)
                {
                    if (stats.Contains(item))
                    {
                        stats.Remove(item);
                    }
                }
            }
            return Main.rand.Next(stats);
        }
        public static string StatNumberAsText(PlayerStats stat, float number)
        {
            string value = "";
            if (number > 0)
            {
                value = "+";
            }
            if (DoesStatsRequiredWholeNumber(stat))
            {
                return value + $"{number} {stat}";
            }
            return value + $"{(int)(number * 100)}% {stat}";
        }
    }
    public enum PlayerStats
    {
        MeleeDMG,
        RangeDMG,
        MagicDMG,
        SummonDMG,
        MovementSpeed,
        JumpBoost,
        MaxHP,
        RegenHP,
        MaxMana,
        RegenMana,
        Defense,
        DamageUniverse,
        CritChance,
        CritDamage,
        DefenseEffectiveness,
        ChestLootDropIncrease,
        MaxMinion,
        MaxSentry,
        Thorn
        //Luck
    }

    /// <summary>
    /// This player class will hold additional infomation that the base Player or ModPlayer class don't provide<br/>
    /// The logic to get to those infomation is automatic <br/>
    /// It make more sense to have a modplayer file do all the logic so we don't have to worry about it when implement
    /// </summary>
    public class BossRushUtilsPlayer : ModPlayer
    {
        public const float PLAYERARMLENGTH = 12f;
        public Vector2 MouseLastPositionBeforeAnimation = Vector2.Zero;
        public bool IsPlayerStillUsingTheSameItem = false;
        private int oldHeldItem = 0;
        Item item;
        public override void PreUpdate()
        {
            item = Player.HeldItem;
            if (oldHeldItem != item.type)
            {
                IsPlayerStillUsingTheSameItem = false;
            }
            else
            {
                IsPlayerStillUsingTheSameItem = true;
            }
            base.PreUpdate();
        }
        public override void PostUpdate()
        {
            if(!Player.ItemAnimationActive)
            {
                MouseLastPositionBeforeAnimation = Main.MouseWorld;
            }
            oldHeldItem = item.type;
        }
    }
}