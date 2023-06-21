using System;
using Terraria;
using System.IO;
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
using BossRush.Contents.Items.Artifact;

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
        public float Multiplier = 1f;
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
            PlayerCardHandle modplayer = Main.LocalPlayer.GetModPlayer<PlayerCardHandle>();
            TooltipLine badstatsline = new(Mod, "stats", "");
            for (int i = 0; i < CardStats.Count; i++)
            {
                if (CardStatsNumber[i] == 0)
                {
                    continue;
                }
                if (CardStatsNumber[i] < 0)
                {
                    badstatsline.Text = StatNumberAsText(CardStats[i], CardStatsNumber[i]);
                    badstatsline.OverrideColor = BossRushColor.MultiColor(new List<Color> { Color.Red, Color.Black }, 1);
                    continue;
                }
                float statsNum = CardStatsNumber[i];
                if (CardStatsNumber[i] > 0)
                {
                    if (modplayer.ReducePositiveCardStat)
                    {
                        statsNum *= PlayerCardHandle.ReducePositiveCardStatByHalf;
                    }
                }
                tooltips.Add(new TooltipLine(Mod, "stats", StatNumberAsText(CardStats[i], statsNum)));
            }
            if (!string.IsNullOrEmpty(badstatsline.Text))
            {
                tooltips.Add(badstatsline);
            }
            if (CursedID != -1)
            {
                TooltipLine badline = new TooltipLine(Mod, "stats", modplayer.CursedStringStats(CursedID));
                badline.OverrideColor = BossRushColor.MultiColor(new List<Color> { Color.Red, Color.Black }, 1);
                tooltips.Add(badline);
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
        public List<PlayerStats> CardStats = new List<PlayerStats>();
        public List<float> CardStatsNumber = new List<float>();
        public override void OnSpawn(IEntitySource source)
        {
            if (Tier <= 0)
            {
                return;
            }
            SetBadStatsBaseOnTier();
            int offset = 0;
            if (CardStats.Count > 0)
            {
                offset++;
            }
            for (int i = offset; i < Tier + offset; i++)
            {
                CardStats.Add(SetStatsToAddBaseOnTier());
                CardStatsNumber.Add(statsCalculator(CardStats[i], Multiplier));
            }
            OnTierItemSpawn();
            for (int i = 0; i < CardStats.Count; i++)
            {
                for (int l = i + 1; l < CardStats.Count; l++)
                {
                    if (CardStats[i] != CardStats[l])
                    {
                        continue;
                    }
                    CardStatsNumber[i] += CardStatsNumber[l];
                    CardStats.RemoveAt(l);
                    CardStatsNumber.RemoveAt(l);
                    i = 0;
                    l = 0;
                }
            }
        }
        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);
            if (CursedID == -1)
            {
                return;
            }
            PlayerCardHandle modplayer = player.GetModPlayer<PlayerCardHandle>();
            if (modplayer.listCursesID.Count >= 12)
            {
                CursedID = -1;
                return;
            }
            CursedID = Main.rand.Next(12) + 1;
        }
        //since we only add 1 curse per card, this don't need to be a list
        public int CursedID = -1;
        protected PlayerStats SetStatsToAddBaseOnTier()
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
            if (CardStats.Count > 0)
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
        private void SetBadStatsBaseOnTier()
        {
            if (Tier <= 1)
            {
                return;
            }
            bool hasMagicDeck = Main.LocalPlayer.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID == 7;
            if (Main.rand.NextFloat() < .15f * Tier || (hasMagicDeck && Main.rand.NextFloat() < .25f * Tier))
            {
                if (Main.rand.NextBool(10) || (hasMagicDeck && Main.rand.NextBool(3)))
                {
                    CursedID = 0;
                }
                PlayerStats badstat = SetStatsToAddBaseOnTier();
                CardStats.Add(badstat);
                CardStatsNumber.Add(statsCalculator(badstat, -3));
            }
        }
        /// <summary>
        /// Use this if <see cref="Tier"/> value set within the card item have value larger than 0
        /// </summary>
        public virtual void OnTierItemSpawn() { }
        public virtual float CursedStat => -1;
        protected float statsCalculator(PlayerStats stats, float multiplier)
        {
            if (CursedID != -1 && multiplier > 0)
            {
                multiplier *= 3;
            }
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
                return (int)(statsNum * multiplier);
            }
            switch (Tier)
            {
                case 1:
                    return statsNum * multiplier;
                case 2:
                    return (float)Math.Round((statsNum + Main.rand.NextFloat(.01f, .03f) + .01f) * Tier * multiplier, 2);
                case 3:
                    return (float)Math.Round((statsNum + Main.rand.NextFloat(.02f, .05f) + .01f) * Tier * multiplier, 2);
                case 4:
                    return (float)Math.Round((statsNum + Main.rand.NextFloat(.04f, .07f) + .01f) * Tier * multiplier, 2);
                default:
                    return (float)Math.Round((statsNum + Main.rand.NextFloat(.01f, .1f)) * Tier * multiplier, 2);
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
                int offset = (i + 1) * 20;
                if (modplayer.ReducePositiveCardStat && CardStatsNumber[i] > 0)
                {
                    CardStatsNumber[i] *= PlayerCardHandle.ReducePositiveCardStatByHalf;
                }
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
                Color textcolor = Color.White;
                if (CardStatsNumber[i] < 0)
                {
                    textcolor = Color.Red;
                }
                BossRushUtils.CombatTextRevamp(player.Hitbox, textcolor, StatNumberAsText(CardStats[i], CardStatsNumber[i]), offset);
            }
            if (CursedID != -1)
            {
                while (modplayer.listCursesID.Contains(CursedID))
                {
                    CursedID = Main.rand.Next(12) + 1;
                }
                modplayer.listCursesID.Add(CursedID);
                BossRushUtils.CombatTextRevamp(player.Hitbox, Color.DarkRed, modplayer.CursedStringStats(CursedID));
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
        public bool DecreaseRateOfFire = false;// ID 1
        public bool NoHealing = false;// ID 2
        public bool SluggishDamage = false;// ID 3
        public bool FiveTimeDamageTaken = false;// ID 4
        public bool LimitedResource = false;// ID 5
        public bool PlayWithConstantLifeLost = false;// ID 6
        public bool ReduceIframe = false;// ID 7
        public bool WeaponCanJammed = false; // ID 8 sometime can't use weapon
        public bool WeaponCanKick = false; // ID 9 lose life on use weapon
        public bool NegativeDamageRandomize = false;// ID 10
        public bool CritDealNoDamage = false;// ID 11
        public bool ReducePositiveCardStat = false;// ID 12
        public const float ReducePositiveCardStatByHalf = .5f;
        public bool AccessoriesDisable = false; // Will be implement much later
        public List<int> listCursesID = new List<int>();
        //We handle no dupe curses in here
        public override void PreUpdate()
        {
            base.PreUpdate();
            if (item != Player.HeldItem)
            {
                SlowDown = 0;
                CoolDown = 0;
            }
            DecreaseRateOfFire = false;
            NoHealing = false;
            SluggishDamage = false;
            FiveTimeDamageTaken = false;
            LimitedResource = false;
            PlayWithConstantLifeLost = false;
            ReduceIframe = false;
            WeaponCanJammed = false;
            WeaponCanKick = false;
            NegativeDamageRandomize = false;
            CritDealNoDamage = false;
            ReducePositiveCardStat = false;
            AccessoriesDisable = false;
            for (int i = 0; i < listCursesID.Count; i++)
            {
                switch (listCursesID[i])
                {
                    case 1:
                        DecreaseRateOfFire = true;
                        break;
                    case 2:
                        NoHealing = true;
                        break;
                    case 3:
                        SluggishDamage = true;
                        break;
                    case 4:
                        FiveTimeDamageTaken = true;

                        break;
                    case 5:
                        LimitedResource = true;
                        break;
                    case 6:
                        PlayWithConstantLifeLost = true;
                        break;
                    case 7:
                        ReduceIframe = true;
                        break;
                    case 8:
                        WeaponCanJammed = true;
                        break;
                    case 9:
                        WeaponCanKick = true;
                        break;
                    case 10:
                        NegativeDamageRandomize = true;
                        break;
                    case 11:
                        CritDealNoDamage = true;
                        break;
                    case 12:
                        ReducePositiveCardStat = true;
                        break;
                    default:
                        break;
                }
            }
        }
        public string CursedStringStats(int curseID)
        {
            string CursedString = "";
            switch (curseID)
            {
                case 1:
                    CursedString = "Weapon have decrease fire rate";
                    break;
                case 2:
                    CursedString = "You can't heal using potion";
                    break;
                case 3:
                    CursedString = "Decrease weapon damage severely";
                    break;
                case 4:
                    CursedString = "Getting hit is much more fatal";
                    break;
                case 5:
                    CursedString = "You can't regenarate mana nor hp";
                    break;
                case 6:
                    CursedString = "You always lose life leaving you with 1 hp left";
                    break;
                case 7:
                    CursedString = "Your lost some immunity frame";
                    break;
                case 8:
                    CursedString = "Your weapon will jammed if you use the same weapon too much";
                    break;
                case 9:
                    CursedString = "Your weapon use your life to work";
                    break;
                case 10:
                    CursedString = "Damage have been ramdomize to be worse";
                    break;
                case 11:
                    CursedString = "Critical damage deal next to no damage";
                    break;
                case 12:
                    CursedString = "Cards stats is reduce by half";
                    break;
                default:
                    CursedString = "Error ! You shouldn't be getting this tho unless you done something horribly wrong";
                    break;
            }
            return CursedString;
        }
        public override float UseSpeedMultiplier(Item item)
        {
            if (DecreaseRateOfFire)
            {
                return .35f;
            }
            return base.UseSpeedMultiplier(item);
        }
        float SlowDown = 1;
        int CoolDown = 0;
        public override bool CanUseItem(Item item)
        {
            if (WeaponCanJammed && CoolDown != 0)
            {
                return SlowDown <= 1;
            }
            return base.CanUseItem(item);
        }
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (WeaponCanKick)
            {
                Player.statLife -= Player.GetWeaponDamage(item);
            }
            if (WeaponCanJammed && Main.mouseLeft)
            {
                SlowDown += .2f;
            }
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
        }
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
        Item item;
        //Platinum
        //public float LuckIncrease = 0;
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (item.DamageType == DamageClass.Melee)
            {
                damage.Base = Math.Clamp(MeleeDMG + damage.Base, -damage.Base + .1f, maxStatCanBeAchieved);
            }
            if (item.DamageType == DamageClass.Ranged)
            {
                damage.Base = Math.Clamp(RangeDMG + damage.Base, -damage.Base + .1f, maxStatCanBeAchieved);
            }
            if (item.DamageType == DamageClass.Magic)
            {
                damage.Base = Math.Clamp(MagicDMG + damage.Base, -damage.Base + .1f, maxStatCanBeAchieved);
            }
            if (item.DamageType == DamageClass.Summon)
            {
                damage.Base = Math.Clamp(SummonDMG + damage.Base, -damage.Base + .1f, maxStatCanBeAchieved);
            }
            damage.Base = Math.Clamp(DamagePure + damage.Base, -damage.Base + .1f, maxStatCanBeAchieved);
            if (SluggishDamage)
            {
                damage *= .5f;
            }
            if (NegativeDamageRandomize)
            {
                damage *= Main.rand.NextFloat();
            }
        }
        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            crit = Math.Clamp(CritStrikeChance + crit, 0, maxStatCanBeAchieved);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.CritDamage.Flat = Math.Clamp(CritDamage, -modifiers.CritDamage.Base + 1, 999999) * modifiers.CritDamage.Base;
            if (CritDealNoDamage)
            {
                modifiers.CritDamage *= 0;
            }
        }
        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            health = StatModifier.Default;
            mana = StatModifier.Default;

            health.Base = Math.Clamp(HPMax + health.Base, -Player.statLifeMax + 100, maxStatCanBeAchieved);
            mana.Base = Math.Clamp(ManaMax + mana.Base, -Player.statManaMax + 20, maxStatCanBeAchieved);
        }
        public override void PostUpdate()
        {
            base.PostUpdate();
            item = Player.HeldItem;
            ChestLoot.amountModifier = Math.Clamp(DropAmountIncrease + ChestLoot.amountModifier, 0, maxStatCanBeAchieved);
            if (NoHealing)
            {
                Player.AddBuff(BuffID.PotionSickness, 999);
            }
            if (PlayWithConstantLifeLost)
            {
                Player.statLife = Math.Clamp(Player.statLife - 1, 1, Player.statLifeMax2);
            }
            if (WeaponCanJammed)
            {
                if (SlowDown >= 6)
                {
                    SlowDown = 6;
                    CoolDown = Player.itemAnimationMax * 10;
                }
                if (!Player.ItemAnimationActive)
                {
                    CoolDown = BossRushUtils.CoolDown(CoolDown);
                    SlowDown = Math.Clamp(SlowDown - .05f, 1, 6);
                }
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            base.ModifyHurt(ref modifiers);
            if (FiveTimeDamageTaken)
            {
                modifiers.FinalDamage.Base *= 5;
            }
        }
        public override void PostHurt(Player.HurtInfo info)
        {
            base.PostHurt(info);
            if (info.PvP)
            {
                return;
            }
            if (ReduceIframe)
            {
                Player.AddImmuneTime(info.CooldownCounter, -30);
            }
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
            if (LimitedResource)
            {
                Player.lifeRegen = 0;
                Player.manaRegen = 0;
            }
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
            foreach (int item in listCursesID)
            {
                packet.Write(item);
            }
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
            tag["CursesID"] = listCursesID;
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
            listCursesID = tag.Get<List<int>>("CursesID");
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
            for (int i = 0; i < listCursesID.Count; i++)
            {
                listCursesID[i] = reader.ReadByte();
            }
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
            clone.listCursesID = listCursesID;
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
            if (listCursesID != clone.listCursesID) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
        }
    }
    class CardNPCdrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (!ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode)
            {
                return;
            }
            if (npc.boss)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<CardPacket>(), 3));
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<BigCardPacket>(), 7));
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<BoxOfCard>(), 25));
            }
            else
            {
                if (npc.friendly || npc.lifeMax <= 5)
                {
                    return;
                }
                npcLoot.Add(ItemDropRule.ByCondition(new MagicalCardDeckException(), ModContent.ItemType<CardPacket>(), 10));
                npcLoot.Add(ItemDropRule.ByCondition(new MagicalCardDeckException(), ModContent.ItemType<BigCardPacket>(), 20));
                npcLoot.Add(ItemDropRule.ByCondition(new MagicalCardDeckException(), ModContent.ItemType<BoxOfCard>(), 50));
                npcLoot.Add(ItemDropRule.ByCondition(new IsNotABossAndBossIsAlive(), ModContent.ItemType<CardPacket>(), 100));
                npcLoot.Add(ItemDropRule.ByCondition(new IsNotABossAndBossIsAlive(), ModContent.ItemType<BigCardPacket>(), 200));
                npcLoot.Add(ItemDropRule.ByCondition(new IsNotABossAndBossIsAlive(), ModContent.ItemType<BoxOfCard>(), 500));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CardPacket>(), 400));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BigCardPacket>(), 600));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BoxOfCard>(), 700));
            }
        }
    }
}