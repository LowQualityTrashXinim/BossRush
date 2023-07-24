using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Contents.BuffAndDebuff;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.Card;
using System;
using BossRush.Contents.Items.Chest;

namespace BossRush.Contents.Items.Potion
{
    internal class MysteriousPotion : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTUREPOTION;
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(1, 1);
            Item.maxStack = 99;
            base.SetDefaults();
        }
        public override bool CanUseItem(Player player)
        {
            return !player.HasBuff(ModContent.BuffType<MysteriousPotionBuff>());
        }
        public override bool? UseItem(Player player)
        {
            StatsCalculation(player, player.GetModPlayer<MysteriousPotionPlayer>());
            player.AddBuff(ModContent.BuffType<MysteriousPotionBuff>(), 14400);
            return true;
        }
        public PlayerStats SetStatsToAdd(MysteriousPotionPlayer modplayer)
        {
            List<PlayerStats> stats = new List<PlayerStats>
            { PlayerStats.MaxSentry,
            PlayerStats.MaxMinion,
            PlayerStats.Thorn,
            PlayerStats.DefenseEffectiveness,
            PlayerStats.CritDamage,
            PlayerStats.CritChance,
            PlayerStats.DamageUniverse,
            PlayerStats.Defense,
            PlayerStats.RegenMana,
            PlayerStats.MaxMana,
            PlayerStats.RegenHP,
            PlayerStats.MaxHP,
            PlayerStats.MovementSpeed,
            PlayerStats.JumpBoost,
            PlayerStats.SummonDMG,
            PlayerStats.MagicDMG,
            PlayerStats.RangeDMG,
            PlayerStats.MeleeDMG };
            if (modplayer.Stats.Count > 0 && modplayer.Stats.Count != stats.Count)
            {
                foreach (var item in modplayer.Stats)
                {
                    if (stats.Contains(item))
                    {
                        stats.Remove(item);
                    }
                }
            }
            return Main.rand.Next(stats);
        }
        private void StatsCalculation(Player player, MysteriousPotionPlayer modplayer)
        {
            int potionPoint = modplayer.PotionPoint();
            if (potionPoint <= 0)
            {
                return;
            }
            for (int i = potionPoint; i != 0; i--)
            {
                if (i < 0)
                {
                    modplayer.Stats.Add(SetStatsToAdd(modplayer));
                    modplayer.StatsMulti.Add(i);
                    break;
                }
                int pointSubstract = i != 1 ? Main.rand.Next(1, i) : 1;
                modplayer.Stats.Add(SetStatsToAdd(modplayer));
                modplayer.StatsMulti.Add(pointSubstract);
                i -= pointSubstract;
            }
            for (int i = 0; i < modplayer.Stats.Count; i++)
            {
                Color textcolor = Color.White;
                if (modplayer.StatsMulti[i] < 0)
                {
                    textcolor = Color.Red;
                }
                BossRushUtils.CombatTextRevamp(player.Hitbox, textcolor, StatNumberAsText(modplayer, i), i * 20, 180);
            }
        }
        public static string StatNumberAsText(MysteriousPotionPlayer modplayer, int index)
        {
            string value = "";
            if (modplayer.StatsMulti[index] > 0)
            {
                value = "+";
            }
            if (BossRushUtils.DoesStatsRequiredWholeNumber(modplayer.Stats[index]))
            {
                return value + $"{modplayer.ToStatsNumInt(modplayer.Stats[index], modplayer.StatsMulti[index])} {modplayer.Stats[index]}";
            }
            return value + $"{modplayer.ToStatsNumInt(modplayer.Stats[index], modplayer.StatsMulti[index])}% {modplayer.Stats[index]}";
        }
    }
    public class MysteriousPotionBuff : ModBuff
    {
        public override string Texture => BossRushTexture.EMPTYBUFF;
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            MysteriousPotionPlayer modplayer = player.GetModPlayer<MysteriousPotionPlayer>();
            for (int i = 0; i < modplayer.Stats.Count; i++)
            {
                switch (modplayer.Stats[i])
                {
                    case PlayerStats.MeleeDMG:
                        player.GetDamage(DamageClass.Melee) += modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]);
                        break;
                    case PlayerStats.RangeDMG:
                        player.GetDamage(DamageClass.Ranged) += modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]);
                        break;
                    case PlayerStats.MagicDMG:
                        player.GetDamage(DamageClass.Magic) += modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]);
                        break;
                    case PlayerStats.SummonDMG:
                        player.GetDamage(DamageClass.Summon) += modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]);
                        break;
                    case PlayerStats.MovementSpeed:
                        player.moveSpeed += modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]);
                        break;
                    case PlayerStats.JumpBoost:
                        player.jumpSpeedBoost += modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]);
                        break;
                    case PlayerStats.MaxHP:
                        player.statLifeMax2 += modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i]);
                        break;
                    case PlayerStats.RegenHP:
                        player.lifeRegen += modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i]);
                        break;
                    case PlayerStats.MaxMana:
                        player.statManaMax2 += modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i]);
                        break;
                    case PlayerStats.RegenMana:
                        player.manaRegen += modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i]);
                        break;
                    case PlayerStats.Defense:
                        player.statDefense += modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i]);
                        break;
                    case PlayerStats.DamageUniverse:
                        player.GetDamage(DamageClass.Generic) += modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]);
                        break;
                    case PlayerStats.CritChance:
                        player.GetCritChance(DamageClass.Generic) += modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]);
                        break;
                    case PlayerStats.CritDamage:
                        modplayer.CritDMG += modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]);
                        break;
                    case PlayerStats.DefenseEffectiveness:
                        player.DefenseEffectiveness *= modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]) + 1;
                        break;
                    case PlayerStats.Thorn:
                        player.thorns += modplayer.ToStatsNumFloat(modplayer.Stats[i], modplayer.StatsMulti[i]);
                        break;
                    case PlayerStats.MaxMinion:
                        player.maxMinions += modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i]);
                        break;
                    case PlayerStats.MaxSentry:
                        player.maxTurrets += modplayer.ToStatsNumInt(modplayer.Stats[i], modplayer.StatsMulti[i]);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public class MysteriousPotionPlayer : ModPlayer
    {
        public List<PlayerStats> Stats = new List<PlayerStats>();
        public List<int> StatsMulti = new List<int>();
        public int PotionPoint()
        {
            int point = 5;
            return point;
        }
        public float CritDMG = 0;
        public float ToStatsNumFloat(PlayerStats stats, int multi) => lookupDictionary[stats] * multi * .01f;

        public int ToStatsNumInt(PlayerStats stats, int multi) => lookupDictionary[stats] * multi;

        public override void ResetEffects()
        {
            base.ResetEffects();
            CritDMG = 0;
            if (!Player.HasBuff(ModContent.BuffType<MysteriousPotionBuff>()))
            {
                if (Stats.Count > 0)
                    Stats.Clear();
                if (StatsMulti.Count > 0)
                    StatsMulti.Clear();
            }
        }
        public Dictionary<PlayerStats, int> lookupDictionary = new Dictionary<PlayerStats, int>()
        {
            { PlayerStats.Thorn, 15 },
            { PlayerStats.MeleeDMG, 10 },
            { PlayerStats.RangeDMG, 10 },
            { PlayerStats.MagicDMG, 10 },
            { PlayerStats.SummonDMG, 10 },
            { PlayerStats.MovementSpeed,5 },
            { PlayerStats.DefenseEffectiveness, 5 },
            { PlayerStats.CritChance, 5 },
            { PlayerStats.CritDamage, 5 },
            { PlayerStats.JumpBoost, 5 },
            { PlayerStats.MaxMana, 5 },
            { PlayerStats.MaxHP, 5 },
            { PlayerStats.DamageUniverse, 4 },
            { PlayerStats.Defense, 4 },
            { PlayerStats.RegenHP, 1 },
            { PlayerStats.RegenMana, 1 },
            { PlayerStats.ChestLootDropIncrease, 1 },
            { PlayerStats.MaxMinion, 1 },
            { PlayerStats.MaxSentry, 1 },
        };
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.CritDamage += Math.Clamp(CritDMG * .01f, 0, 999999);
        }
    }
}
