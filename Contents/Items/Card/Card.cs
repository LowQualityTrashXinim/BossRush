using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using Microsoft.Xna.Framework;
using Terraria.GameContent.ItemDropRules;

namespace BossRush.Contents.Items.Card
{
    enum PlayerStats
    {
        MeleeDMG,
        RangeDMG,
        MagicDMG,
        SummonDMG,
        MovementSpeed,
        MaxHP,
        RegenHP,
        MaxMana,
        RegenMana,
        Defense,
        DamageUniverse,
        CritChance,
        CritDamage,
        ChanceToNotConsumeAmmo,
        DefenseEffectiveness,
        ChestLootDropIncrease,
        MaxMinion,
        MaxSentry
    }
    abstract class Card : ModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(1, 1);
            Item.maxStack = 1;
            Item.UseSound = SoundID.Item35;
            PostCardSetDefault();
        }
        public virtual void PostCardSetDefault() { }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Tier <= 0)
            {
                return;
            }
            for (int i = 0; i < stats.Count; i++)
            {
                if (DoesStatsRequiredWholeNumber(i))
                {
                    TooltipLine line1 = new TooltipLine(Mod, "stats", $"Increase {statsNumber[i]} {stats[i]}");
                    tooltips.Add(line1);
                    continue;
                }
                int Fullnum = (int)(statsNumber[i] * 100);
                TooltipLine line = new TooltipLine(Mod, "stats", $"Increase {Fullnum}% {stats[i]}");
                tooltips.Add(line);
            }
        }
        private void SetStatsToAddBaseOnTier(ref List<PlayerStats> list)
        {
            if (Tier >= 4)
            {

            }
            if (Tier >= 3)
            {
                list.Add(PlayerStats.MaxSentry);
                list.Add(PlayerStats.MaxMinion);
                list.Add(PlayerStats.ChestLootDropIncrease);
            }
            if (Tier >= 2)
            {
                list.Add(PlayerStats.DefenseEffectiveness);
                //list.Add(PlayerStats.ChanceToNotConsumeAmmo);
                list.Add(PlayerStats.CritDamage);
                list.Add(PlayerStats.CritChance);
                list.Add(PlayerStats.DamageUniverse);
            }
            if (Tier >= 1)
            {
                list.Add(PlayerStats.Defense);
                list.Add(PlayerStats.RegenMana);
                list.Add(PlayerStats.MaxMana);
                list.Add(PlayerStats.RegenHP);
                list.Add(PlayerStats.MaxHP);
                list.Add(PlayerStats.MovementSpeed);
                list.Add(PlayerStats.SummonDMG);
                list.Add(PlayerStats.MagicDMG);
                list.Add(PlayerStats.RangeDMG);
                list.Add(PlayerStats.MeleeDMG);
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            if(Tier <= 0)
            {
                return;
            }
            List<PlayerStats> list = new List<PlayerStats>();
            SetStatsToAddBaseOnTier(ref list);
            for (int i = 0; i < Tier; i++)
            {
                stats.Add(Main.rand.Next(list));
                statsNumber.Add(statsCalculator());
            }
        }
        private float statsCalculator()
        {
            float statsNum = (float)Math.Round(Main.rand.NextFloat(.01f, .04f), 2);
            switch (Tier)
            {
                case 1:
                    return statsNum;
                case 2:
                    return (statsNum + (float)Math.Round(Main.rand.NextFloat(.02f)) + .01f) * Tier;
                case 3:
                    return (statsNum + (float)Math.Round(Main.rand.NextFloat(.05f)) + .01f) * Tier;
                case 4:
                    return (statsNum + (float)Math.Round(Main.rand.NextFloat(.07f)) + .01f) * Tier;
                default:
                    return (statsNum + (float)Math.Round(Main.rand.NextFloat(.01f, .1f))) * Tier;
            }
        }
        private List<PlayerStats> stats;
        private List<float> statsNumber;
        /// <summary>
        /// 1 = Copper<br/>
        /// 2 = Silver<br/>
        /// 3 = Gold<br/>
        /// 4 = Platinum<br/>
        /// </summary>
        public virtual int Tier => 0;
        public virtual void OnUseItem(Player player, PlayerCardHandle modplayer) { }
        public override bool? UseItem(Player player)
        {
            PlayerCardHandle modplayer = player.GetModPlayer<PlayerCardHandle>();
            OnUseItem(player, modplayer);
            for (int i = 0; i < stats.Count; i++)
            {
                switch (stats[i])
                {
                    case PlayerStats.MeleeDMG:
                        modplayer.MeleeDamageMultiply += statsNumber[i];
                        break;
                    case PlayerStats.RangeDMG:
                        modplayer.RangeDamageMultiply += statsNumber[i];
                        break;
                    case PlayerStats.MagicDMG:
                        modplayer.MagicDamageMultiply += statsNumber[i];
                        break;
                    case PlayerStats.SummonDMG:
                        modplayer.SummonDamageMultiply += statsNumber[i];
                        break;
                    case PlayerStats.MovementSpeed:
                        modplayer.MovementMulti += statsNumber[i];
                        break;
                    case PlayerStats.MaxHP://This involve some special calculation, we can't use the normal calculation
                        break;
                    case PlayerStats.RegenHP:
                        modplayer.LifeRegenMulti += statsNumber[i];
                        break;
                    case PlayerStats.MaxMana://This involve some special calculation, we can't use the normal calculation
                        break;
                    case PlayerStats.RegenMana:
                        modplayer.ManaRegenMulti += statsNumber[i];
                        break;
                    case PlayerStats.Defense://This involve some special calculation, we can't use the normal calculation
                        break;
                    case PlayerStats.DamageUniverse:
                        modplayer.DamageMultiply += statsNumber[i];
                        break;
                    case PlayerStats.CritChance:
                        modplayer.CritDamage += statsNumber[i];
                        break;
                    case PlayerStats.CritDamage:
                        modplayer.CritDamage += statsNumber[i];
                        break;
                    case PlayerStats.DefenseEffectiveness:
                        modplayer.DefenseEffectiveness += statsNumber[i];
                        break;
                    case PlayerStats.ChestLootDropIncrease://This involve some special calculation, we can't use the normal calculation
                        break;
                    case PlayerStats.MaxMinion://This involve some special calculation, we can't use the normal calculation
                        break;
                    case PlayerStats.MaxSentry://This involve some special calculation, we can't use the normal calculation
                        break;
                    default:
                        break;
                }
            }
            return true;
        }
        private bool DoesStatsRequiredWholeNumber(int i) => 
                    stats[i] is PlayerStats.Defense
                    || stats[i] is PlayerStats.MaxMinion
                    || stats[i] is PlayerStats.MaxSentry
                    || stats[i] is PlayerStats.MaxHP
                    || stats[i] is PlayerStats.MaxMana
                    || stats[i] is PlayerStats.ChestLootDropIncrease;
        public virtual bool CanBeCraft => true;
        public override void AddRecipes()
        {
            if (CanBeCraft)
            {
                CreateRecipe()
                    .AddIngredient(ModContent.ItemType<EmptyCard>())
                    .Register();
            }
        }
    }
    class PlayerCardHandle : ModPlayer
    {
        public ChestLootDropPlayer ChestLoot => Player.GetModPlayer<ChestLootDropPlayer>();

        //Copper tier
        public float MeleeDamageMultiply = 0;
        public float RangeDamageMultiply = 0;
        public float MagicDamageMultiply = 0;
        public float SummonDamageMultiply = 0;

        public float MovementMulti = 1;
        public float HPMaxMulti = 0;
        public float LifeRegenMulti = 0;
        public float ManaMaxMulti = 0;
        public float ManaRegenMulti = 0;
        public int DefenseBase = 0;
        //Silver Tier
        public float DamageMultiply = 0;
        public int CritStrikeChance = 0;
        /// <summary>
        /// Not implemented
        /// </summary>
        public float CritDamage = 1;
        /// <summary>
        /// Not implemented
        /// </summary>
        public float ChanceToNoConsumeAmmo = 0;
        public float DefenseEffectiveness = 1;
        //Gold
        /// <summary>
        /// Not implemented
        /// </summary>
        public int DropAmountIncrease = 0;
        public int MinionSlot = 0;
        public int SentrySlot = 0;
        //Platinum
        //public float LuckIncrease = 0;
        public int ID;
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            Player.GetDamage(DamageClass.Melee) += MeleeDamageMultiply;
            Player.GetDamage(DamageClass.Ranged) += RangeDamageMultiply;
            Player.GetDamage(DamageClass.Magic) += MagicDamageMultiply;
            Player.GetDamage(DamageClass.Summon) += SummonDamageMultiply;

            Player.GetDamage(DamageClass.Generic) += DamageMultiply;
        }
        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            crit += CritStrikeChance;
        }
        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            health = StatModifier.Default;
            mana = StatModifier.Default;

            health.Base = HPMaxMulti * Player.statLifeMax;
            mana.Base = ManaMaxMulti * Player.statManaMax;
        }
        public override void ResetEffects()
        {
            Player.accRunSpeed *= MovementMulti;
            Player.lifeRegen = (int)(LifeRegenMulti * Player.lifeRegen);
            Player.manaRegen = (int)(ManaRegenMulti * Player.manaRegen);
            Player.statDefense += DefenseBase;
            Player.DefenseEffectiveness *= DefenseEffectiveness;
            Player.maxMinions += MinionSlot;
            Player.maxTurrets += SentrySlot;
        }
    }
    class CardNPCdrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {

        }
    }
}