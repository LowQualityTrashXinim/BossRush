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
        MaxSentry,
        //Luck
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
            if (CardStats is null)
            {
                return;
            }
            if (CardStats.Count < 1)
            {
                return;
            }
            for (int i = 0; i < CardStats.Count; i++)
            {
                if (DoesStatsRequiredWholeNumber(CardStats[i]))
                {
                    TooltipLine line1 = new TooltipLine(Mod, "stats", $"Increase {CardStatsNumber[i]} {CardStats[i]}");
                    tooltips.Add(line1);
                    continue;
                }
                int Fullnum = (int)(CardStatsNumber[i] * 100);
                TooltipLine line = new TooltipLine(Mod, "stats", $"Increase {Fullnum}% {CardStats[i]}");
                tooltips.Add(line);
            }
        }
        protected PlayerStats SetStatsToAddBaseOnTier()
        {
            List<PlayerStats> list = new List<PlayerStats>();
            if (Tier >= 4)
            {
                //list.Add(PlayerStats.Luck);
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
            return Main.rand.Next(list);
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (Tier <= 0)
            {
                return;
            }
            OnTierItemSpawn();
        }
        public virtual void OnTierItemSpawn() { }
        public virtual List<PlayerStats> CardStats => new List<PlayerStats>();
        public virtual List<float> CardStatsNumber => new List<float>();
        protected float statsCalculator(PlayerStats stats)
        {
            float statsNum = (float)Math.Round(Main.rand.NextFloat(.01f, .04f), 2);
            if (DoesStatsRequiredWholeNumber(stats))
            {
                if (stats is PlayerStats.ChestLootDropIncrease)
                {
                    statsNum = Main.rand.Next(Tier) + 1;
                    return statsNum;
                }
                statsNum = (Main.rand.Next(Tier) + Main.rand.Next(Tier) + 1) * Tier;
                return statsNum;
            }
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
            if (Tier == 0)
            {
                return true;
            }
            for (int i = 0; i < CardStats.Count; i++)
            {
                switch (CardStats[i])
                {
                    case PlayerStats.MeleeDMG:
                        modplayer.MeleeDamageMultiply += CardStatsNumber[i];
                        break;
                    case PlayerStats.RangeDMG:
                        modplayer.RangeDamageMultiply += CardStatsNumber[i];
                        break;
                    case PlayerStats.MagicDMG:
                        modplayer.MagicDamageMultiply += CardStatsNumber[i];
                        break;
                    case PlayerStats.SummonDMG:
                        modplayer.SummonDamageMultiply += CardStatsNumber[i];
                        break;
                    case PlayerStats.MovementSpeed:
                        modplayer.MovementMulti += CardStatsNumber[i];
                        break;
                    case PlayerStats.MaxHP://This involve some special calculation, we can't use the normal calculation
                        modplayer.HPMaxMulti += CardStatsNumber[i];
                        break;
                    case PlayerStats.RegenHP:
                        modplayer.LifeRegenMulti += CardStatsNumber[i];
                        break;
                    case PlayerStats.MaxMana://This involve some special calculation, we can't use the normal calculation
                        modplayer.ManaMaxMulti += CardStatsNumber[i];
                        break;
                    case PlayerStats.RegenMana:
                        modplayer.ManaRegenMulti += CardStatsNumber[i];
                        break;
                    case PlayerStats.Defense://This involve some special calculation, we can't use the normal calculation
                        modplayer.DefenseBase += CardStatsNumber[i];
                        break;
                    case PlayerStats.DamageUniverse:
                        modplayer.DamageMultiply += CardStatsNumber[i];
                        break;
                    case PlayerStats.CritChance:
                        modplayer.CritStrikeChance += (int)CardStatsNumber[i];
                        break;
                    case PlayerStats.CritDamage:
                        modplayer.CritDamage += CardStatsNumber[i];
                        break;
                    case PlayerStats.DefenseEffectiveness:
                        modplayer.DefenseEffectiveness += CardStatsNumber[i];
                        break;
                    case PlayerStats.ChestLootDropIncrease://This involve some special calculation, we can't use the normal calculation
                        modplayer.DropAmountIncrease += CardStatsNumber[i];
                        break;
                    case PlayerStats.MaxMinion://This involve some special calculation, we can't use the normal calculation
                        modplayer.MinionSlot += CardStatsNumber[i];
                        break;
                    case PlayerStats.MaxSentry://This involve some special calculation, we can't use the normal calculation
                        modplayer.SentrySlot += CardStatsNumber[i];
                        break;
                    default:
                        break;
                }
                //CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, (int)player.Center.X, (int)player.Center.Y), Color.White, $"+{CardStatsNumber[i]} {CardStats[i]}");
            }
            return true;
        }
        private bool DoesStatsRequiredWholeNumber(PlayerStats stats) =>
                    stats is PlayerStats.Defense
                    || stats is PlayerStats.MaxMinion
                    || stats is PlayerStats.MaxSentry
                    || stats is PlayerStats.MaxHP
                    || stats is PlayerStats.MaxMana
                    || stats is PlayerStats.CritChance
                    || stats is PlayerStats.ChestLootDropIncrease;
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
        public float DefenseBase = 0;
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
        public float DropAmountIncrease = 0;
        public float MinionSlot = 0;
        public float SentrySlot = 0;
        //Platinum
        //public float LuckIncrease = 0;
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                hit.Damage = (int)(CritDamage * hit.Damage);
            }
        }
        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            health = StatModifier.Default;
            mana = StatModifier.Default;

            health.Base = HPMaxMulti;
            mana.Base = ManaMaxMulti;
        }
        public override void PreUpdate()
        {
            base.PreUpdate();
        }
        public override void PostUpdate()
        {
            base.PostUpdate();
            ChestLoot.amountModifier += DropAmountIncrease;
        }
        public override void ResetEffects()
        {
            Player.statDefense += (int)DefenseBase;
            Player.accRunSpeed *= MovementMulti;
            Player.lifeRegen = (int)(LifeRegenMulti * Player.lifeRegen);
            Player.manaRegen = (int)(ManaRegenMulti * Player.manaRegen);
            Player.DefenseEffectiveness *= DefenseEffectiveness;
            Player.maxMinions = (int)(MinionSlot + Player.maxMinions);
            Player.maxTurrets = (int)(SentrySlot + Player.maxTurrets);
        }
    }
    class CardNPCdrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {

        }
    }
}