using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.IO;

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
            for (int i = 0; i < CardStats.Count; i++)
            {
                for (int l = i + 1; l < CardStats.Count; l++)
                {
                    if (CardStats[i] == CardStats[l])
                    {
                        CardStatsNumber[i] += CardStatsNumber[l];
                        CardStats.RemoveAt(i);
                        CardStatsNumber.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Use this if <see cref="Tier"/> value set within the card item have value larger than 0
        /// </summary>
        public virtual void OnTierItemSpawn() { }
        public virtual List<PlayerStats> CardStats => new List<PlayerStats>();
        public virtual List<float> CardStatsNumber => new List<float>();
        protected float statsCalculator(PlayerStats stats)
        {
            float statsNum = (float)Math.Round(Main.rand.NextFloat(.01f, .04f), 2);
            if (DoesStatsRequiredWholeNumber(stats))
            {
                if (stats is PlayerStats.ChestLootDropIncrease
                    || stats is PlayerStats.MaxMinion
                    || stats is PlayerStats.MaxSentry)
                {
                    statsNum = Main.rand.Next(Tier) + 1;
                    return statsNum;
                }
                statsNum = (Main.rand.Next(Tier) + 1) * Tier;
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
                        modplayer.MeleeDMG += CardStatsNumber[i];
                        break;
                    case PlayerStats.RangeDMG:
                        modplayer.RangeDMG += CardStatsNumber[i];
                        break;
                    case PlayerStats.MagicDMG:
                        modplayer.MagicDMG += CardStatsNumber[i];
                        break;
                    case PlayerStats.SummonDMG:
                        modplayer.SummonDMG += CardStatsNumber[i];
                        break;
                    case PlayerStats.MovementSpeed:
                        modplayer.Movement += CardStatsNumber[i];
                        break;
                    case PlayerStats.MaxHP://This involve some special calculation, we can't use the normal calculation
                        modplayer.HPMax += (int)CardStatsNumber[i];
                        break;
                    case PlayerStats.RegenHP:
                        modplayer.HPRegen += CardStatsNumber[i];
                        break;
                    case PlayerStats.MaxMana://This involve some special calculation, we can't use the normal calculation
                        modplayer.ManaMax += (int)CardStatsNumber[i];
                        break;
                    case PlayerStats.RegenMana:
                        modplayer.ManaRegen += CardStatsNumber[i];
                        break;
                    case PlayerStats.Defense://This involve some special calculation, we can't use the normal calculation
                        modplayer.DefenseBase += (int)CardStatsNumber[i];
                        break;
                    case PlayerStats.DamageUniverse:
                        modplayer.DamagePure += CardStatsNumber[i];
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
                        modplayer.DropAmountIncrease += (int)CardStatsNumber[i];
                        break;
                    case PlayerStats.MaxMinion://This involve some special calculation, we can't use the normal calculation
                        modplayer.MinionSlot += (int)CardStatsNumber[i];
                        break;
                    case PlayerStats.MaxSentry://This involve some special calculation, we can't use the normal calculation
                        modplayer.SentrySlot += (int)CardStatsNumber[i];
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
        public float MeleeDMG = 0;
        public float RangeDMG = 0;
        public float MagicDMG = 0;
        public float SummonDMG = 0;
        public float Movement = 1;
        public int HPMax = 0;
        public float HPRegen = 0;
        public int ManaMax = 0;
        public float ManaRegen = 0;
        public int DefenseBase = 0;
        //Silver Tier
        public float DamagePure = 0;
        public int CritStrikeChance = 0;

        public float CritDamage = 1;
        public float DefenseEffectiveness = 1;
        //Gold
        public int DropAmountIncrease = 0;
        public int MinionSlot = 0;
        public int SentrySlot = 0;
        //Platinum
        //public float LuckIncrease = 0;
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (item.DamageType == DamageClass.Melee)
            {
                damage += MeleeDMG;
            }
            if (item.DamageType == DamageClass.Ranged)
            {
                damage += RangeDMG;
            }
            if (item.DamageType == DamageClass.Magic)
            {
                damage += MagicDMG;
            }
            if (item.DamageType == DamageClass.Summon)
            {
                damage += SummonDMG;
            }
            damage += DamagePure;
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

            health.Base = HPMax;
            mana.Base = ManaMax;
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
            Player.statDefense += DefenseBase;
            Player.accRunSpeed *= Movement;
            Player.lifeRegen = (int)(HPRegen * Player.lifeRegen);
            Player.manaRegen = (int)(ManaRegen * Player.manaRegen);
            Player.DefenseEffectiveness *= DefenseEffectiveness;
            Player.maxMinions += MinionSlot;
            Player.maxTurrets += SentrySlot;
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRushNetCodeHandle.MessageType.CardEffect);
            packet.Write((byte)Player.whoAmI);
            packet.Write(MeleeDMG);
            packet.Write(RangeDMG);
            packet.Write(MagicDMG);
            packet.Write(SummonDMG);
            packet.Write(Movement);
            packet.Write(HPMax);
            packet.Write(HPRegen);
            packet.Write(ManaMax);
            packet.Write(ManaRegen);
            packet.Write(DefenseBase);
            packet.Write(DamagePure);
            packet.Write(CritStrikeChance);
            packet.Write(CritDamage);
            packet.Write(DefenseEffectiveness);
            packet.Write(DropAmountIncrease);
            packet.Write(MinionSlot);
            packet.Write(SentrySlot);
            packet.Send(toWho, fromWho);
        }
        public override void SaveData(TagCompound tag)
        {
            tag["MeleeDMG"] = MeleeDMG;
            tag["RangeDMG"] = RangeDMG;
            tag["MagicDMG"] = MagicDMG;
            tag["SummonDMG"] = SummonDMG;
            tag["Movement"] = Movement;
            tag["HPMax"] = HPMax;
            tag["HPRegen"] = HPRegen;
            tag["ManaMax"] = ManaMax;
            tag["ManaRegen"] = ManaRegen;
            tag["DefenseBase"] = DefenseBase;
            tag["DamagePure"] = DamagePure;
            tag["CritStrikeChance"] = CritStrikeChance;
            tag["CritDamage"] = CritDamage;
            tag["DefenseEffectiveness"] = DefenseEffectiveness;
            tag["DropAmountIncrease"] = DropAmountIncrease;
            tag["MinionSlot"] = MinionSlot;
            tag["SentrySlot"] = SentrySlot;
        }

        public override void LoadData(TagCompound tag)
        {
            MeleeDMG = (float)tag["MeleeDMG"];
            RangeDMG = (float)tag["RangeDMG"];
            MagicDMG = (float)tag["MagicDMG"];
            SummonDMG = (float)tag["SummonDMG"];
            Movement = (float)tag["Movement"];
            HPMax = (int)tag["HPMax"];
            HPRegen = (float)tag["HPRegen"];
            ManaMax = (int)tag["ManaMax"];
            ManaRegen = (float)tag["ManaRegen"];
            DefenseBase = (int)tag["DefenseBase"];
            DamagePure = (float)tag["DamagePure"];
            CritStrikeChance = (int)tag["CritStrikeChance"];
            CritDamage = (float)tag["CritDamage"];
            DefenseEffectiveness = (float)tag["DefenseEffectiveness"];
            DropAmountIncrease = (int)tag["DropAmountIncrease"];
            MinionSlot = (int)tag["MinionSlot"];
            SentrySlot = (int)tag["SentrySlot"];
        }
    }
    class CardNPCdrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.boss)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<CardPacket>(), 3));
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<BigCardPacket>(), 7));
            }
            else
            {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(),ModContent.ItemType<CardPacket>(), 10));
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(),ModContent.ItemType<BigCardPacket>(), 20));
            }
        }
    }
}