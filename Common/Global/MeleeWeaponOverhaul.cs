﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Common.Global
{
    public class MathH
    {
        public const float ToRadByFiveTeen = 0.2617994f;
    }
    public class BossRushUseStyle
    {
        public const int Swipe = 999;
        public const int Poke = 998;
        public const int GenericSwingDownImprove = 990;
    }
    internal class MeleeWeaponOverhaul : GlobalItem
    {

        public const float PLAYERARMLENGTH = 12f;
        public override void SetDefaults(Item item)
        {
            if (ModContent.GetInstance<BossRushModConfig>().DisableWeaponOverhaul)
            {
                return;
            }
            if (item.noMelee)
            {
                return;
            }
            #region Vanilla Fixes
            switch (item.type)
            {
                case ItemID.WoodenSword:
                case ItemID.BorealWoodSword:
                case ItemID.RichMahoganySword:
                case ItemID.PalmWoodSword:
                case ItemID.EbonwoodSword:
                case ItemID.ShadewoodSword:
                case ItemID.PearlwoodSword:
                    item.width = item.height = 32;
                    break;
                case ItemID.BluePhaseblade:
                case ItemID.RedPhaseblade:
                case ItemID.GreenPhaseblade:
                case ItemID.PurplePhaseblade:
                case ItemID.OrangePhaseblade:
                case ItemID.YellowPhaseblade:
                case ItemID.WhitePhaseblade:
                    item.width = item.height = 48;
                    break;
                case ItemID.BluePhasesaber:
                case ItemID.RedPhasesaber:
                case ItemID.GreenPhasesaber:
                case ItemID.PurplePhasesaber:
                case ItemID.OrangePhasesaber:
                case ItemID.YellowPhasesaber:
                case ItemID.WhitePhasesaber:
                    item.width = item.height = 56;
                    break;
                case ItemID.CopperBroadsword:
                case ItemID.TinBroadsword:
                case ItemID.LeadBroadsword:
                case ItemID.IronBroadsword:
                    item.width = item.height = 38;
                    break;
                case ItemID.SilverBroadsword:
                case ItemID.TungstenBroadsword:
                    item.width = item.height = 39;
                    break;
                case ItemID.GoldBroadsword:
                case ItemID.PlatinumBroadsword:
                    item.width = item.height = 42;
                    break;
                case ItemID.CobaltSword:
                    item.width = 56;
                    item.height = 58;
                    break;
                case ItemID.PalladiumSword:
                    item.width = 50;
                    item.height = 60;
                    break;
                case ItemID.MythrilSword:
                    item.width = item.height = 58;
                    break;
                case ItemID.OrichalcumSword:
                    item.width = item.height = 54;
                    break;
                case ItemID.AdamantiteSword:
                case ItemID.TitaniumSword:
                    item.width = item.height = 60;
                    break;
                case ItemID.Muramasa:
                    item.width = 50;
                    item.height = 64;
                    break;
                case ItemID.LightsBane:
                    item.width = item.height = 37;
                    break;
                case ItemID.BloodButcherer:
                    item.width = 50;
                    item.height = 58;
                    break;
                case ItemID.BladeofGrass:
                    item.width = item.height = 50;
                    break;
                case ItemID.FieryGreatsword:
                    item.width = 54;
                    item.height = 54;
                    break;
                case ItemID.TheHorsemansBlade:
                    item.width = item.height = 54;
                    break;
                case ItemID.Frostbrand:
                    item.width = 50;
                    item.height = 58;
                    break;
                case ItemID.CactusSword:
                    item.width = item.height = 48;
                    break;
                case ItemID.TerraBlade:
                    item.width = 48;
                    item.height = 52;
                    break;
                case ItemID.BeamSword:
                    item.width = item.height = 52;
                    break;
                case ItemID.Meowmere:
                    item.width = 50;
                    item.height = 58;
                    break;
                case ItemID.Starfury:
                    item.width = item.height = 42;
                    break;
                case ItemID.StarWrath:
                    item.width = 46;
                    item.height = 54;
                    break;
                case ItemID.BatBat:
                    item.width = item.height = 52;
                    break;
                case ItemID.TentacleSpike:
                    item.width = 44;
                    item.height = 40;
                    break;
                case ItemID.NightsEdge:
                    item.width = 50;
                    item.height = 54;
                    break;
                case ItemID.TrueNightsEdge:
                    item.width = 48;
                    item.height = 56;
                    break;
                case ItemID.Excalibur:
                    item.width = item.height = 48;
                    break;
                case ItemID.TrueExcalibur:
                    item.width = item.height = 52;
                    break;
                case ItemID.InfluxWaver:
                    item.width = item.height = 50;
                    break;
                case ItemID.Seedler:
                    item.width = 48;
                    item.height = 68;
                    break;
                case ItemID.Keybrand:
                    item.width = 58;
                    item.height = 62;
                    break;
                case ItemID.ChlorophyteSaber:
                    item.width += 10;
                    item.height += 10;
                    break;
                case ItemID.BreakerBlade:
                    item.width = 80;
                    item.height = 92;
                    break;
                case ItemID.BoneSword:
                    item.width = item.height = 50;
                    break;
                case ItemID.ChlorophyteClaymore:
                    item.width = item.height = 68;
                    break;
                case ItemID.Bladetongue:
                    item.width = item.height = 50;
                    break;
                case ItemID.DyeTradersScimitar:
                    item.width = 40;
                    item.height = 48;
                    break;
                case ItemID.BeeKeeper:
                    item.width = item.height = 44;
                    break;
                case ItemID.EnchantedSword:
                    item.width = item.height = 34;
                    break;
                case ItemID.ZombieArm:
                    item.width = 38;
                    item.height = 40;
                    break;
                case ItemID.FalconBlade:
                    item.width = 36;
                    item.height = 40;
                    break;
                case ItemID.Cutlass:
                    item.width = 40;
                    item.height = 48;
                    break;
                case ItemID.CandyCaneSword:
                    item.width = 44;
                    item.height = 75;
                    break;
                case ItemID.IceBlade:
                    item.width = 38;
                    item.height = 34;
                    break;
                case ItemID.HamBat:
                    item.width = 44;
                    item.height = 40;
                    break;
                case ItemID.DD2SquireBetsySword:
                    item.width = 66; item.height = 66;
                    break;
                case ItemID.PurpleClubberfish:
                    item.width = item.height = 50;
                    break;
                case ItemID.AntlionClaw:
                    item.width = item.height = 32;
                    break;
                case ItemID.Katana:
                    item.width = 48;
                    item.height = 54;
                    break;
                case ItemID.ChristmasTreeSword:
                    item.width = item.height = 60;
                    break;
            }
            #endregion
            switch (item.type)
            {
                #region BossRushUseStyle.Swipe
                //Sword that have even end
                //WoodSword
                case ItemID.PearlwoodSword:
                case ItemID.BorealWoodSword:
                case ItemID.PalmWoodSword:
                case ItemID.ShadewoodSword:
                case ItemID.EbonwoodSword:
                case ItemID.RichMahoganySword:
                case ItemID.WoodenSword:
                case ItemID.CactusSword:
                //OrebroadSword
                case ItemID.BeeKeeper:
                case ItemID.CopperBroadsword:
                case ItemID.TinBroadsword:
                case ItemID.IronBroadsword:
                case ItemID.LeadBroadsword:
                case ItemID.SilverBroadsword:
                case ItemID.TungstenBroadsword:
                case ItemID.GoldBroadsword:
                case ItemID.PlatinumBroadsword:
                //LightSaber
                case ItemID.PurplePhaseblade:
                case ItemID.BluePhaseblade:
                case ItemID.GreenPhaseblade:
                case ItemID.YellowPhaseblade:
                case ItemID.OrangePhaseblade:
                case ItemID.RedPhaseblade:
                case ItemID.WhitePhaseblade:
                //Saber
                case ItemID.PurplePhasesaber:
                case ItemID.BluePhasesaber:
                case ItemID.GreenPhasesaber:
                case ItemID.YellowPhasesaber:
                case ItemID.OrangePhasesaber:
                case ItemID.RedPhasesaber:
                case ItemID.WhitePhasesaber:
                //Misc PreHM sword
                case ItemID.PurpleClubberfish:
                case ItemID.StylistKilLaKillScissorsIWish:
                case ItemID.BladeofGrass:
                case ItemID.FieryGreatsword:
                case ItemID.LightsBane:
                //HardmodeSword
                case ItemID.CobaltSword:
                case ItemID.MythrilSword:
                case ItemID.AdamantiteSword:
                case ItemID.PalladiumSword:
                case ItemID.OrichalcumSword:
                case ItemID.TitaniumSword:
                case ItemID.Excalibur:
                case ItemID.TheHorsemansBlade:
                case ItemID.Bladetongue:
                case ItemID.DD2SquireDemonSword:
                //Sword That shoot projectile
                case ItemID.BeamSword:
                case ItemID.EnchantedSword:
                case ItemID.Starfury:
                case ItemID.InfluxWaver:
                case ItemID.ChlorophyteClaymore:
                case ItemID.ChlorophyteSaber:
                case ItemID.ChristmasTreeSword:
                case ItemID.TrueExcalibur:
                    item.useStyle = BossRushUseStyle.Swipe;
                    item.useTurn = false;
                    break;
                //Poke Sword
                //Pre HM Sword
                case ItemID.DyeTradersScimitar:
                case ItemID.CandyCaneSword:
                case ItemID.Muramasa:
                case ItemID.BloodButcherer:
                case ItemID.NightsEdge:
                case ItemID.Katana:
                case ItemID.FalconBlade:
                case ItemID.BoneSword:
                //HM sword
                case ItemID.IceBlade:
                case ItemID.BreakerBlade:
                case ItemID.Frostbrand:
                case ItemID.Cutlass:
                case ItemID.Seedler:
                case ItemID.TrueNightsEdge:
                case ItemID.TerraBlade:
                case ItemID.DD2SquireBetsySword:
                case ItemID.Meowmere:
                case ItemID.StarWrath:
                    item.useStyle = BossRushUseStyle.Poke;
                    item.useTurn = false;
                    break;
                case ItemID.ZombieArm:
                case ItemID.BatBat:
                case ItemID.TentacleSpike:
                case ItemID.SlapHand:
                case ItemID.Keybrand:
                case ItemID.AntlionClaw:
                case ItemID.HamBat:
                case ItemID.PsychoKnife:
                    item.useStyle = BossRushUseStyle.GenericSwingDownImprove;
                    item.useTurn = false;
                    break;
                #endregion
                default:
                    break;
            }
            base.SetDefaults(item);
        }
        /// <summary>
        /// Use to order 2 values from smallest to biggest
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        private static (int, int) Order(float v1, float v2) => v1 < v2 ? ((int)v1, (int)v2) : ((int)v2, (int)v1);
        public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            if (item.useStyle == BossRushUseStyle.Swipe || item.useStyle == BossRushUseStyle.Poke || item.useStyle == BossRushUseStyle.GenericSwingDownImprove)
            {
                MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
                Vector2 handPos = Vector2.UnitY.RotatedBy(player.compositeFrontArm.rotation);
                float length = new Vector2(item.width, item.height).Length() * player.GetAdjustedItemScale(item);
                Vector2 endPos = handPos;
                endPos *= length + PLAYERARMLENGTH;
                if (modPlayer.ComboNumber == 2 && item.useStyle == BossRushUseStyle.Poke)
                {
                    handPos.Y += 20;
                    endPos.Y -= handPos.Y;
                }
                handPos += player.MountedCenter;
                endPos += player.MountedCenter;
                (int X1, int X2) XVals = Order(handPos.X, endPos.X);
                (int Y1, int Y2) YVals = Order(handPos.Y, endPos.Y);
                hitbox = new Rectangle(XVals.X1 - 2, YVals.Y1 - 2, XVals.X2 - XVals.X1 + 2, YVals.Y2 - YVals.Y1 + 2);
                modPlayer.SwordHitBox = hitbox;
            }
        }
        private int DamageHandleSystem(MeleeOverhaulPlayer modPlayer, int damage)
        {
            Item item = modPlayer.Player.HeldItem;
            if (item.useStyle == BossRushUseStyle.Swipe && modPlayer.ComboNumber == 2)
            {
                return (int)(damage * 1.5f);
            }
            if (item.useStyle == BossRushUseStyle.Poke)
            {
                if (modPlayer.ComboNumber == 1)
                {
                    return (int)(damage * 1.75f);
                }
                if (modPlayer.ComboNumber == 2)
                {
                    return (int)(damage * 1.25f);
                }
            }
            return damage;
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.useStyle != BossRushUseStyle.Swipe &&
                item.useStyle != BossRushUseStyle.Poke)
            {
                return base.CanUseItem(item, player);
            }
            return player.GetModPlayer<MeleeOverhaulPlayer>().delaytimer <= 0;
        }
        public override float UseSpeedMultiplier(Item item, Player player)
        {
            if (item.useStyle != BossRushUseStyle.Swipe
                && item.useStyle != BossRushUseStyle.Poke)
            {
                return base.UseAnimationMultiplier(item, player);
            }
            float useTimeMultiplierOnCombo = 1;
            MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
            if (item.useStyle == BossRushUseStyle.Swipe)
            {
                if (modPlayer.ComboNumber == 2)
                {
                    useTimeMultiplierOnCombo -= .5f;
                }
            }
            if (item.useStyle == BossRushUseStyle.Poke)
            {
                if (modPlayer.ComboNumber == 1)
                {
                    useTimeMultiplierOnCombo -= .5f;
                }
                if (modPlayer.ComboNumber == 2)
                {
                    useTimeMultiplierOnCombo -= .25f;
                }
            }
            return useTimeMultiplierOnCombo;
        }
        public override void UseStyle(Item Item, Player player, Rectangle heldItemFrame)
        {
            MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
            if (Item.noMelee)
            {
                return;
            }
            switch (Item.useStyle)
            {
                case BossRushUseStyle.Swipe:
                    switch (modPlayer.ComboNumber)
                    {
                        case 0:
                            SwipeAttack(player, modPlayer, 1);
                            break;
                        case 1:
                            SwipeAttack(player, modPlayer, -1);
                            break;
                        case 2:
                            CircleSwingAttack(player, modPlayer);
                            break;
                    }
                    Item.noUseGraphic = false;
                    break;
                case BossRushUseStyle.Poke:
                    switch (modPlayer.ComboNumber)
                    {
                        case 0:
                            SwipeAttack(player, modPlayer, 1);
                            break;
                        case 1:
                            WideSwingAttack(player, modPlayer);
                            break;
                        case 2:
                            StrongThrust(player, modPlayer);
                            break;
                    }
                    Item.noUseGraphic = false;
                    break;
                case BossRushUseStyle.GenericSwingDownImprove:
                    SwipeAttack(player, modPlayer, 1);
                    Item.noUseGraphic = false;
                    break;
                default:
                    break;
            }
        }
        public override void ModifyHitNPC(Item Item, Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (Item.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckVanillaSwingWithModded))
            {
                MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
                modPlayer.critReference = crit;
                if (crit)
                {
                    damage += (int)(damage * .5f);
                }
                damage = DamageHandleSystem(modPlayer, damage);
                modPlayer.CountDownToResetCombo = (int)(player.itemAnimationMax * 2.35f);
            }
            base.ModifyHitNPC(Item, player, target, ref damage, ref knockBack, ref crit);
        }
        private void StrongThrust(Player player, MeleeOverhaulPlayer modPlayer)
        {
            float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
            Poke2(player, modPlayer, percentDone);
        }
        private void FastThurst(Player player, MeleeOverhaulPlayer modPlayer)
        {
            float percentDone = player.itemAnimation / (float)player.itemAnimationMax * .33333f;
            float firstThrust = player.itemAnimationMax / 3f;
            float secondThrust = player.itemAnimationMax * 2 / 3f;
            modPlayer.CountAmountOfThrustDid = 1;
            if (player.itemAnimation >= firstThrust)
            {
                modPlayer.CountAmountOfThrustDid = 2;
                --percentDone;
            }
            if (player.itemAnimation >= secondThrust)
            {
                modPlayer.CountAmountOfThrustDid = 3;
                --percentDone;
            }
            percentDone = MathHelper.Clamp(percentDone, 0f, 1f);
            Poke(player, modPlayer, percentDone);
        }
        private void Poke2(Player player, MeleeOverhaulPlayer modPlayer, float percentDone)
        {
            int direction = modPlayer.data.X > 0 ? 1 : -1;
            Vector2 toThrust = Vector2.UnitX * direction;
            Vector2 poke = Vector2.SmoothStep(toThrust * 30f, toThrust, percentDone);
            player.itemRotation = poke.ToRotation();
            player.itemRotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f;
            player.compositeFrontArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Full, poke.ToRotation() - MathHelper.PiOver2);
            player.itemLocation = player.MountedCenter + poke - poke.SafeNormalize(Vector2.Zero) * 20f;
        }
        private void Poke(Player player, MeleeOverhaulPlayer modPlayer, float percentDone)
        {
            Vector2 poke = Vector2.SmoothStep(modPlayer.data * 30f, modPlayer.data, percentDone).RotatedBy(modPlayer.RotateThurst * player.direction);
            player.itemRotation = poke.ToRotation();
            player.itemRotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f;
            player.compositeFrontArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Full, poke.ToRotation() - MathHelper.PiOver2);
            player.itemLocation = player.MountedCenter + poke - poke.SafeNormalize(Vector2.Zero) * 20f;
        }
        private void WideSwingAttack(Player player, MeleeOverhaulPlayer modPlayer)
        {
            float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
            percentDone = BossRushUtils.InOutBack(percentDone);
            float baseAngle = modPlayer.data.ToRotation();
            float angle = MathHelper.ToRadians(baseAngle + 125) * player.direction;
            float start = baseAngle + angle;
            float end = baseAngle - angle;
            Swipe(start, end, percentDone, player);
        }
        private void SwipeAttack(Player player, MeleeOverhaulPlayer modPlayer, int direct)
        {
            float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
            percentDone = BossRushUtils.InExpo(percentDone);
            float baseAngle = modPlayer.data.ToRotation();
            float angle = MathHelper.ToRadians(baseAngle + 90) * player.direction;
            float start = baseAngle + angle * direct;
            float end = baseAngle - angle * direct;
            Swipe(start, end, percentDone, player);
        }
        private void CircleSwingAttack(Player player, MeleeOverhaulPlayer modPlayer)
        {
            float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
            percentDone = BossRushUtils.InExpo(percentDone);
            float baseAngle = modPlayer.data.ToRotation();
            float start = baseAngle + MathHelper.PiOver2 * player.direction;
            float end = baseAngle - (MathHelper.TwoPi + MathHelper.PiOver2) * player.direction;
            Swipe(start, end, percentDone, player);
        }
        private void Swipe(float start, float end, float percentDone, Player player)
        {
            float currentAngle = MathHelper.SmoothStep(start, end, percentDone);
            player.itemRotation = currentAngle;
            player.itemRotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f;
            player.compositeFrontArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Full, currentAngle - MathHelper.PiOver2);
            player.itemLocation = player.MountedCenter + Vector2.UnitX.RotatedBy(currentAngle) * PLAYERARMLENGTH;
        }
    }
    public class MeleeOverhaulPlayer : ModPlayer
    {
        public Vector2 data;
        public int ComboNumber = 0;
        public Rectangle SwordHitBox;
        public int delaytimer = 10;
        public int CountAmountOfThrustDid = 1;
        public int oldHeldItem;
        public int CountDownToResetCombo = 0;
        public float RotateThurst;
        public int MouseXPosDirection = 1;
        public override void PreUpdate()
        {
            Item item = Player.HeldItem;
            if (item.type != oldHeldItem)
            {
                ComboNumber = 0;
            }
            //if (item.useStyle == BossRushUseStyle.Poke && count == 1)
            //{
            //    switch (CountAmountOfThrustDid)
            //    {
            //        case 1:
            //            RotateThurst = MathH.ToRadByFiveTeen;
            //            break;
            //        case 2:
            //            RotateThurst = 0;
            //            break;
            //        case 3:
            //            RotateThurst = MathHelper.TwoPi - MathH.ToRadByFiveTeen;
            //            break;
            //    }
            //}
            //else
            //{
            //    RotateThurst = 0;
            //}
            //useStyle system handle
            delaytimer -= delaytimer > 0 ? 1 : 0;
            CountDownToResetCombo -= CountDownToResetCombo > 0 ? 1 : 0;
            iframeCounter -= iframeCounter > 0 ? 1 : 0;
            if (!item.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModded) ||
                item.noMelee
                )
            {
                return;
            }
            MouseXPosDirection = Main.MouseWorld.X - Player.Center.X > 0 ? 1 : -1;
            if (Player.ItemAnimationJustStarted && delaytimer == 0)
            {
                delaytimer = (int)(Player.itemAnimationMax * 1.2f);
                ExecuteSpecialComboOnStart(item);
            }
            if (Player.ItemAnimationActive)
            {
                ExecuteSpecialComboOnActive(item);
            }
            else
            {
                if (AlreadyHitNPC)
                {
                    ++ComboNumber;
                }
                if (delaytimer != 0 && ComboNumber == 3 && item.useStyle == BossRushUseStyle.Poke)
                {
                    Player.velocity *= .1f;
                }
                ComboHandleSystem();
                AlreadyHitNPC = false;
                CanPlayerBeDamage = true;
            }
        }
        private void ExecuteSpecialComboOnActive(Item item)
        {
            if (ComboConditionChecking())
            {
                return;
            }
            CanPlayerBeDamage = false;
            switch (item.useStyle)
            {
                case BossRushUseStyle.Poke:
                    Player.gravity = 0;
                    break;
                case BossRushUseStyle.Swipe:
                    SpinAttackExtraHit(item);
                    break;
            }
        }
        private bool IsWallBossAlive()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.type == NPCID.WallofFlesh)
                {
                    return true;
                }
            }
            return false;
        }
        private bool ComboConditionChecking() =>
            Player.mount.Active
            || IsWallBossAlive()
            || ComboNumber != 2
            || (Player.velocity.X > 2 && MouseXPosDirection == -1)
            || (Player.velocity.X < -2 && MouseXPosDirection == 1)
            || Player.velocity == Vector2.Zero;
        private void ExecuteSpecialComboOnStart(Item item)
        {
            if (ComboConditionChecking())
            {
                return;
            }
            switch (item.useStyle)
            {
                case BossRushUseStyle.Poke:
                    Player.velocity.X += Vector2.UnitX.SafeNormalize(Vector2.Zero).X * 25f * Player.direction;
                    Player.velocity.Y = 0;
                    break;
                case BossRushUseStyle.Swipe:
                    Player.velocity.X += Vector2.UnitX.SafeNormalize(Vector2.Zero).X * 15f * Player.direction;
                    break;
            }
        }
        private bool CanAttack(NPC npc)
        {
            if (!npc.active || npc.immune[Player.whoAmI] > 0)
            {
                return true;
            }
            return false;
        }
        private void ComboHandleSystem()
        {
            if (ComboNumber >= 3 || CountDownToResetCombo == 0)
            {
                ComboNumber = 0;
            }
        }
        public bool critReference;
        int iframeCounter = 0;
        private void SpinAttackExtraHit(Item item)
        {
            NPC npclaststrike = null;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.Hitbox.Intersects(SwordHitBox) && CanAttack(npc) && (iframeCounter == 0 || npclaststrike != null && npclaststrike != npc))
                {
                    npclaststrike = npc;
                    int damage = (int)(item.damage * 1.5f);
                    npc.StrikeNPC(damage, item.knockBack, Player.direction, critReference);
                    iframeCounter = (int)(Player.itemAnimationMax * .25f);
                    Player.dpsDamage += damage;
                }
            }
        }
        bool CanPlayerBeDamage = true;
        public override void PostUpdate()
        {
            Item item = Player.HeldItem;
            if (!item.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModded) ||
                item.noMelee
                )
            {
                return;
            }
            if (Player.ItemAnimationJustStarted && Player.ItemAnimationActive && delaytimer == 0)
            {
                data = (Main.MouseWorld - Player.MountedCenter).SafeNormalize(Vector2.Zero);
                oldHeldItem = item.type;
            }
            if (Player.ItemAnimationActive)
            {
                Player.direction = data.X > 0 ? 1 : -1;
            }
            item.noUseGraphic = true;
            Player.attackCD = 0;
        }
        bool AlreadyHitNPC = false;
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (!AlreadyHitNPC)
            {
                AlreadyHitNPC = true;
            }
        }
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            return CanPlayerBeDamage;
        }
    }
}