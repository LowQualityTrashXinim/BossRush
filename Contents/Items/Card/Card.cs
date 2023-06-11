using System;
using Terraria;
using Terraria.ID;
using BossRush.Common;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;
using System.IO;

namespace BossRush.Contents.Items.Card
{
    enum PlayerStats
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
        //Luck
    }
    abstract class CardItem : ModItem
    {
        public const int PlatinumCardDropChance = 40;
        public const int GoldCardDropChance = 20;
        public const int SilverCardDropChance = 10;

        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(30, 24);
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
                tooltips.Add(new TooltipLine(Mod, "statsBugged", "It is appear that the card stats got corrupted or won't save !"));
                return;
            }
            if (CardStats.Count < 1)
            {
                tooltips.Add(new TooltipLine(Mod, "statsBugged", "It is appear that the card stats got corrupted or won't save !"));
                return;
            }
            for (int i = 0; i < CardStats.Count; i++)
            {
                if (CardStatsNumber[i] == 0)
                {
                    continue;
                }
                if (CardStatsNumber[i] < 0)
                {
                    TooltipLine badline = new TooltipLine(Mod, "stats", StatNumberAsText(CardStats[i], CardStatsNumber[i]));
                    badline.OverrideColor = BossRushModSystem.RedToBlack;
                    tooltips.Add(badline);
                    continue;
                }
                TooltipLine line = new TooltipLine(Mod, "stats", StatNumberAsText(CardStats[i], CardStatsNumber[i]));
                tooltips.Add(line);
            }
        }
        private string StatNumberAsText(PlayerStats stat, float number)
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
                list.Add(PlayerStats.JumpBoost);
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
            AddBadStatsBaseOnTier();
            for (int i = 0; i < CardStats.Count; i++)
            {
                for (int l = i + 1; l < CardStats.Count; l++)
                {
                    if (CardStats[i] == CardStats[l])
                    {
                        CardStatsNumber[i] += CardStatsNumber[l];
                        CardStats.RemoveAt(l);
                        CardStatsNumber.RemoveAt(l);
                        i = 0;
                        l = 0;
                    }
                }
            }
        }
        //This one is automatically add in bad stats and is handled by themselves, no need to interfere in this if not neccessary
        private void AddBadStatsBaseOnTier()
        {
            if (Tier <= 1)
            {
                return;
            }
            if (Main.rand.NextFloat() < .1f * Tier)
            {
                PlayerStats badstat = SetStatsToAddBaseOnTier();
                CardStats.Add(badstat);
                CardStatsNumber.Add(statsCalculator(badstat, -2));
            }
        }
        /// <summary>
        /// Use this if <see cref="Tier"/> value set within the card item have value larger than 0
        /// </summary>
        public virtual void OnTierItemSpawn() { }
        public virtual List<PlayerStats> CardStats => new List<PlayerStats>();
        public virtual List<float> CardStatsNumber => new List<float>();
        protected float statsCalculator(PlayerStats stats, float multiplier = 1)
        {
            float statsNum = (float)Math.Round(Main.rand.NextFloat(.01f, .04f), 2);
            if (DoesStatsRequiredWholeNumber(stats))
            {
                if (SpecialCheckPlayerStats(stats))
                {
                    statsNum = Main.rand.Next((int)(Tier * .5f)) + 1;
                }
                else
                {
                    statsNum = (Main.rand.Next(Tier) + 1) * Tier;
                }
                return statsNum * multiplier;
            }
            switch (Tier)
            {
                case 1:
                    return statsNum * multiplier;
                case 2:
                    return (statsNum + (float)Math.Round(Main.rand.NextFloat(.02f)) + .01f) * Tier * multiplier;
                case 3:
                    return (statsNum + (float)Math.Round(Main.rand.NextFloat(.05f)) + .01f) * Tier * multiplier;
                case 4:
                    return (statsNum + (float)Math.Round(Main.rand.NextFloat(.07f)) + .01f) * Tier * multiplier;
                default:
                    return (statsNum + (float)Math.Round(Main.rand.NextFloat(.01f, .1f))) * Tier * multiplier;
            }
        }
        public bool SpecialCheckPlayerStats(PlayerStats stats) =>
            stats is PlayerStats.ChestLootDropIncrease
            || stats is PlayerStats.MaxMinion
            || stats is PlayerStats.MaxSentry;
        /// <summary>
        /// 1 = Copper<br/>
        /// 2 = Silver<br/>
        /// 3 = Gold<br/>
        /// 4 = Platinum<br/>
        /// </summary>
        public virtual int Tier => 0;
        public override bool CanUseItem(Player player)
        {
            return !BossRushUtils.IsAnyVanillaBossAlive();
        }
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
                int offset = i * 20;
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
                    case PlayerStats.JumpBoost:
                        modplayer.JumpBoost += CardStatsNumber[i];
                        break;
                    case PlayerStats.MaxHP:
                        modplayer.HPMax += (int)CardStatsNumber[i];
                        break;
                    case PlayerStats.RegenHP:
                        modplayer.HPRegen += CardStatsNumber[i];
                        break;
                    case PlayerStats.MaxMana:
                        modplayer.ManaMax += (int)CardStatsNumber[i];
                        break;
                    case PlayerStats.RegenMana:
                        modplayer.ManaRegen += CardStatsNumber[i];
                        break;
                    case PlayerStats.Defense:
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
                    case PlayerStats.ChestLootDropIncrease:
                        modplayer.DropAmountIncrease += (int)CardStatsNumber[i];
                        break;
                    case PlayerStats.MaxMinion:
                        modplayer.MinionSlot += (int)CardStatsNumber[i];
                        break;
                    case PlayerStats.MaxSentry:
                        modplayer.SentrySlot += (int)CardStatsNumber[i];
                        break;
                    default:
                        break;
                }
                BossRushUtils.CombatTextRevamp(player.Hitbox, Color.White, StatNumberAsText(CardStats[i], CardStatsNumber[i]), offset);
            }
            return true;
        }
        private int countX = 0;
        private float positionRotateX = 0;
        private void PositionHandle()
        {
            if (positionRotateX < 3.5f && countX == 1)
            {
                positionRotateX += .2f;
            }
            else
            {
                countX = -1;
            }
            if (positionRotateX > 0 && countX == -1)
            {
                positionRotateX -= .2f;
            }
            else
            {
                countX = 1;
            }
        }
        Color auraColor;
        private void ColorHandle()
        {
            switch (Tier)
            {
                case 1:
                    auraColor = new Color(255, 100, 0, 30);
                    break;
                case 2:
                    auraColor = new Color(200, 200, 200, 30);
                    break;
                case 3:
                    auraColor = new Color(255, 255, 0, 30);
                    break;
                default:
                    auraColor = new Color(255, 255, 255, 30);
                    break;
            }
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            PositionHandle();
            ColorHandle();
            Main.instance.LoadItem(Item.type);
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            for (int i = 0; i < 3; i++)
            {
                spriteBatch.Draw(texture, position + new Vector2(positionRotateX, positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, position + new Vector2(positionRotateX, -positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, position + new Vector2(-positionRotateX, positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, position + new Vector2(-positionRotateX, -positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
            }
            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            //if (Item.whoAmI != whoAmI)
            //{
            //    return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
            //}
            Main.instance.LoadItem(Item.type);
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 drawPos = Item.position - Main.screenPosition + origin;
            for (int i = 0; i < 3; i++)
            {
                spriteBatch.Draw(texture, drawPos + new Vector2(positionRotateX, positionRotateX), null, auraColor, rotation, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, drawPos + new Vector2(positionRotateX, -positionRotateX), null, auraColor, rotation, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, drawPos + new Vector2(-positionRotateX, positionRotateX), null, auraColor, rotation, origin, scale, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, drawPos + new Vector2(-positionRotateX, -positionRotateX), null, auraColor, rotation, origin, scale, SpriteEffects.None, 0);
            }
            return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
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
        public const int maxStatCanBeAchieved = 9999;
        //Copper tier
        public float MeleeDMG = 0;
        public float RangeDMG = 0;
        public float MagicDMG = 0;
        public float SummonDMG = 0;
        public float Movement = 0;
        public float JumpBoost = 0;
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
                damage.Base = Math.Clamp(MeleeDMG + damage.Base, 0, maxStatCanBeAchieved);
            }
            if (item.DamageType == DamageClass.Ranged)
            {
                damage.Base = Math.Clamp(RangeDMG + damage.Base, 0, maxStatCanBeAchieved);
            }
            if (item.DamageType == DamageClass.Magic)
            {
                damage.Base = Math.Clamp(MagicDMG + damage.Base, 0, maxStatCanBeAchieved);
            }
            if (item.DamageType == DamageClass.Summon)
            {
                damage.Base = Math.Clamp(SummonDMG + damage.Base, 0, maxStatCanBeAchieved);
            }
            damage.Base = Math.Clamp(DamagePure + damage.Base, 0, maxStatCanBeAchieved);
        }
        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            crit = Math.Clamp(CritStrikeChance + crit, 0, maxStatCanBeAchieved);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                hit.Damage = (int)(Math.Clamp(CritDamage, 0, 999999) * hit.Damage);
            }
        }
        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            health = StatModifier.Default;
            mana = StatModifier.Default;

            health.Base = Math.Clamp(HPMax + health.Base, 100, maxStatCanBeAchieved);
            mana.Base = Math.Clamp(ManaMax + mana.Base, 20, maxStatCanBeAchieved);
        }
        public override void PreUpdate()
        {
            base.PreUpdate();
        }
        public override void PostUpdate()
        {
            base.PostUpdate();
            ChestLoot.amountModifier = Math.Clamp(DropAmountIncrease + ChestLoot.amountModifier, 0, maxStatCanBeAchieved);
        }
        public override void ResetEffects()
        {
            Player.statDefense += Math.Clamp(DefenseBase, -(maxStatCanBeAchieved + Player.statDefense), maxStatCanBeAchieved);
            Player.moveSpeed = Math.Clamp(Movement + Player.moveSpeed, 0, maxStatCanBeAchieved);
            Player.jumpSpeedBoost = Math.Clamp(JumpBoost + Player.jumpSpeedBoost, 0, maxStatCanBeAchieved);
            Player.lifeRegen = (int)Math.Clamp(HPRegen * Player.lifeRegen, 0, maxStatCanBeAchieved);
            Player.manaRegen = (int)Math.Clamp(ManaRegen * Player.manaRegen, 0, maxStatCanBeAchieved);
            Player.DefenseEffectiveness *= Math.Clamp(DefenseEffectiveness, 0, maxStatCanBeAchieved);
            Player.maxMinions = Math.Clamp(MinionSlot + Player.maxMinions, 0, maxStatCanBeAchieved);
            Player.maxTurrets = Math.Clamp(SentrySlot + Player.maxTurrets, 0, maxStatCanBeAchieved);
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRush.MessageType.CardEffect);
            packet.Write((byte)Player.whoAmI);
            packet.Write(MeleeDMG);
            packet.Write(RangeDMG);
            packet.Write(MagicDMG);
            packet.Write(SummonDMG);
            packet.Write(Movement);
            packet.Write(JumpBoost);
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
            tag["JumpBoost"] = JumpBoost;
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
            JumpBoost = (float)tag["JumpBoost"];
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
        public void ReceivePlayerSync(BinaryReader reader)
        {
            MeleeDMG = reader.ReadSingle();
            RangeDMG = reader.ReadSingle();
            MagicDMG = reader.ReadSingle();
            SummonDMG = reader.ReadSingle();
            Movement = reader.ReadSingle();
            JumpBoost = reader.ReadSingle();
            HPMax = reader.ReadInt32();
            HPRegen = reader.ReadSingle();
            ManaMax = reader.ReadInt32();
            ManaRegen = reader.ReadSingle();
            DefenseBase = reader.ReadInt32();
            DamagePure = reader.ReadSingle();
            CritStrikeChance = reader.ReadInt32();
            CritDamage = reader.ReadSingle();
            DefenseEffectiveness = reader.ReadSingle();
            DropAmountIncrease = reader.ReadInt32();
            MinionSlot = reader.ReadInt32();
            SentrySlot = reader.ReadInt32();
        }

        public override void CopyClientState(ModPlayer targetCopy)
        {
            PlayerCardHandle clone = (PlayerCardHandle)targetCopy;
            clone.MeleeDMG = MeleeDMG;
            clone.RangeDMG = RangeDMG;
            clone.MagicDMG = MagicDMG;
            clone.SummonDMG = SummonDMG;
            clone.Movement = Movement;
            clone.JumpBoost = JumpBoost;
            clone.HPMax = HPMax;
            clone.HPRegen = HPRegen;
            clone.ManaMax = ManaMax;
            clone.ManaRegen = ManaRegen;
            clone.DefenseBase = DefenseBase;
            clone.DamagePure = DamagePure;
            clone.CritStrikeChance = CritStrikeChance;
            clone.CritDamage = CritDamage;
            clone.DefenseEffectiveness = DefenseEffectiveness;
            clone.DropAmountIncrease = DropAmountIncrease;
            clone.MinionSlot = MinionSlot;
            clone.SentrySlot = SentrySlot;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            PlayerCardHandle clone = (PlayerCardHandle)clientPlayer;
            if (MeleeDMG != clone.MeleeDMG) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (RangeDMG != clone.RangeDMG) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (MagicDMG != clone.MagicDMG) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (SummonDMG != clone.SummonDMG) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (Movement != clone.Movement) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (JumpBoost != clone.JumpBoost) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (HPMax != clone.HPMax) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (HPRegen != clone.HPRegen) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (ManaMax != clone.ManaMax) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (ManaRegen != clone.ManaRegen) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (DefenseBase != clone.DefenseBase) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (DamagePure != clone.DamagePure) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (CritStrikeChance != clone.CritStrikeChance) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (CritDamage != clone.CritDamage) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (DefenseEffectiveness != clone.DefenseEffectiveness) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (DropAmountIncrease != clone.DropAmountIncrease) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (MinionSlot != clone.MinionSlot) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (SentrySlot != clone.SentrySlot) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
        }
    }
    class CardNPCdrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.boss)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<CardPacket>(), 5));
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<BigCardPacket>(), 10));
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<BoxOfCard>(), 50));
            }
            else
            {
                if (npc.friendly || npc.lifeMax <= 5)
                {
                    return;
                }
                npcLoot.Add(ItemDropRule.ByCondition(new IsNotABossAndBossIsAlive(), ModContent.ItemType<CardPacket>(), 100));
                npcLoot.Add(ItemDropRule.ByCondition(new IsNotABossAndBossIsAlive(), ModContent.ItemType<BigCardPacket>(), 200));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CardPacket>(), 200));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BigCardPacket>(), 400));
            }
        }
    }
}