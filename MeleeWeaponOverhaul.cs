using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Items.Weapon;
using Microsoft.Xna.Framework;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BossRush
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

        const float PLAYERARMLENGTH = 12f;
        public override void SetDefaults(Item item)
        {
            if (ModContent.GetInstance<BossRushModConfig>().DisableWeaponOverhaul)
            {
                return;
            }
            #region Vanilla Fixes
            switch (item.type)
            {
                case ItemID.FieryGreatsword:
                    item.width = 54;
                    item.height = 54;
                    break;
                case ItemID.Frostbrand:
                    item.width = 46;
                    item.height = 50;
                    break;
                case ItemID.CactusSword:
                    item.width = 48;
                    item.height = 48;
                    break;
                case ItemID.TerraBlade:
                    item.width = 46;
                    item.height = 54;
                    break;
                case ItemID.Meowmere:
                    item.width = 50;
                    item.height = 58;
                    break;
                case ItemID.Starfury:
                    item.width = item.height = 42;
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
                case ItemID.TitaniumSword:
                //Sword That shoot projectile
                case ItemID.BeamSword:
                case ItemID.EnchantedSword:
                case ItemID.Starfury:
                case ItemID.InfluxWaver:
                case ItemID.ChlorophyteClaymore:
                case ItemID.TrueExcalibur:
                //Plantera Sword
                case ItemID.Excalibur:
                case ItemID.TheHorsemansBlade:
                case ItemID.Bladetongue:
                    item.useStyle = BossRushUseStyle.Swipe;
                    item.useTurn = false;
                    break;
                //Poke Sword
                //Pre HM Sword
                case ItemID.DyeTradersScimitar:
                case ItemID.ZombieArm:
                case ItemID.CandyCaneSword:
                case ItemID.Muramasa:
                case ItemID.BloodButcherer:
                case ItemID.NightsEdge:
                //HM sword
                case ItemID.TerraBlade:
                case ItemID.Meowmere:
                case ItemID.Katana:
                    item.useStyle = BossRushUseStyle.Poke;
                    item.useTurn = false;
                    break;
                #endregion
                default:
                    if (item.useStyle == ItemUseStyleID.Swing)
                    {
                        item.useStyle = BossRushUseStyle.GenericSwingDownImprove;
                        item.useTurn = false;
                    }
                    break;
            }
            base.SetDefaults(item);
        }
        public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            //this remain untouch cause idk what in the hell should i change here
            if (item.useStyle == BossRushUseStyle.Swipe || item.useStyle == BossRushUseStyle.Poke || item.useStyle == BossRushUseStyle.GenericSwingDownImprove)
            {
                //Get the direction of the weapon, and the distance from the player to the hilt
                Vector2 handPos = Vector2.UnitY.RotatedBy(player.compositeFrontArm.rotation);

                //Use afforementioned direction, and get the distance from the player to the tip of the weapon
                float length = (item.width + item.height) * player.GetAdjustedItemScale(item);
                Vector2 endPos = handPos;

                //Use values obtained above to construct an approximation of the two most important points
                handPos *= PLAYERARMLENGTH;
                endPos *= length;
                handPos += player.MountedCenter;
                endPos += player.MountedCenter;

                //Use helper method to get coordinates and size for the rectangle
                (int X1, int X2) XVals = Order(handPos.X, endPos.X);
                (int Y1, int Y2) YVals = Order(handPos.Y, endPos.Y);

                //Create the new bounds of the hitbox
                hitbox = new Rectangle(XVals.X1 - 2, YVals.Y1 - 2, XVals.X2 - XVals.X1 + 2, YVals.Y2 - YVals.Y1 + 2);
                player.GetModPlayer<MeleeOverhaulPlayer>().SwordHitBox = hitbox;
                int damage = player.GetModPlayer<MeleeOverhaulPlayer>().count == 2 ? (int)(player.HeldItem.damage * 1.5f) : player.HeldItem.damage;
                int proj = Projectile.NewProjectile(item.GetSource_ItemUse(item), player.itemLocation, player.GetModPlayer<MeleeOverhaulPlayer>().data, ModContent.ProjectileType<GhostHitBox2>(), damage, player.HeldItem.knockBack, player.whoAmI);
                Main.projectile[proj].Hitbox = hitbox;
            }
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
            float useTimeMultiplierOnCombo = 1;
            MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
            if (item.useStyle == BossRushUseStyle.Swipe)
            {
                if (modPlayer.count == 1)
                {
                    useTimeMultiplierOnCombo -= .5f;
                }
            }
            if (item.useStyle == BossRushUseStyle.Poke)
            {
                if (modPlayer.count == 1)
                {
                    useTimeMultiplierOnCombo -= .25f;
                }
            }
            return useTimeMultiplierOnCombo;
        }
        public override void UseStyle(Item Item, Player player, Rectangle heldItemFrame)
        {
            MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
            Item.noUseGraphic = false;
            switch (Item.useStyle)
            {
                case BossRushUseStyle.Swipe:
                    switch (modPlayer.count)
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
                    break;
                case BossRushUseStyle.Poke:
                    switch (modPlayer.count)
                    {
                        case 0:
                            SwipeAttack(player, modPlayer, 1);
                            break;
                        case 1:
                            FastThurst(player, modPlayer);
                            break;
                        case 2:
                            StrongThrust(player, modPlayer);
                            break;
                    }
                    break;
                case BossRushUseStyle.GenericSwingDownImprove:
                    SwipeAttack(player, modPlayer, 1);
                    break;
                default:
                    break;
            }
        }
        private static (int, int) Order(float v1, float v2) => v1 < v2 ? ((int)v1, (int)v2) : ((int)v2, (int)v1);
        public override void ModifyHitNPC(Item Item, Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
            float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
            float mult = MathHelper.Lerp(.85f, 1.2f, percentDone);
            damage = (int)(damage * mult);
            knockBack *= mult;
            modPlayer.critReference = crit;
            switch (Item.useStyle)
            {
                case BossRushUseStyle.Swipe:
                    if (modPlayer.count == 2)
                    {
                        damage = (int)(damage * 1.5f);
                    }
                    break;
                case BossRushUseStyle.Poke:
                    if (modPlayer.count == 2)
                    {
                        damage = (int)(damage * 1.75f);
                    }
                    break;
            }
            int proj = Projectile.NewProjectile(Item.GetSource_ItemUse(Item), player.itemLocation, Vector2.Zero, ModContent.ProjectileType<GhostHitBox2>(), damage, knockBack, player.whoAmI);
            Main.projectile[proj].Hitbox = modPlayer.SwordHitBox;
            base.ModifyHitNPC(Item, player, target, ref damage, ref knockBack, ref crit);
        }
        private void StrongThrust(Player player, MeleeOverhaulPlayer modPlayer)
        {
            float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
            Poke(player, modPlayer, percentDone);
        }
        private void FastThurst(Player player, MeleeOverhaulPlayer modPlayer)
        {
            float percentDone = player.itemAnimation / (float)player.itemAnimationMax * .33333f;
            float firstThrust = player.itemAnimationMax/3f;
            float secondThrust = player.itemAnimationMax*2/3f;
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
        private void Poke(Player player, MeleeOverhaulPlayer modPlayer, float percentDone)
        {
            Vector2 poke = Vector2.SmoothStep(modPlayer.data* 20f, modPlayer.data, percentDone).RotatedBy(modPlayer.RotateThurst * player.direction);
            player.itemRotation = poke.ToRotation();
            player.itemRotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f;
            player.compositeFrontArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Full, poke.ToRotation() - MathHelper.PiOver2);
            player.itemLocation = player.MountedCenter + poke;
        }
        private void SwipeAttack(Player player, MeleeOverhaulPlayer modPlayer, int direct)
        {
            float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
            float baseAngle = modPlayer.data.ToRotation();
            float angle = MathHelper.ToRadians(baseAngle + 90) * player.direction;
            float start = baseAngle + angle * direct;
            float end = baseAngle - angle * direct;
            Swipe(start, end, percentDone, player);
        }
        private void CircleSwingAttack(Player player, MeleeOverhaulPlayer modPlayer)
        {
            float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
            float baseAngle = modPlayer.data.ToRotation();
            float start = baseAngle + MathHelper.PiOver2 * player.direction;
            float end = baseAngle - (MathHelper.TwoPi + MathHelper.PiOver2) * player.direction;
            Swipe(start, end, percentDone, player);
        }
        private void Swipe(float start, float end, float percentDone, Player player)
        {
            float currentAngle = MathHelper.SmoothStep(start, end, BossRushUtils.InExpo(percentDone));
            player.itemRotation = currentAngle;
            player.itemRotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f;
            player.compositeFrontArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Full, currentAngle - MathHelper.PiOver2);
            player.itemLocation = player.MountedCenter + Vector2.UnitX.RotatedBy(currentAngle) * PLAYERARMLENGTH;
        }
    }
    public class MeleeOverhaulPlayer : ModPlayer
    {
        public Vector2 data;
        public int count = -1;
        public Rectangle SwordHitBox;
        public bool critReference;
        int iframeCounter = 0;
        public int delaytimer = 10;
        public int CountAmountOfThrustDid = 1;
        public int oldHeldItem;
        public float RotateThurst;
        public override void PreUpdate()
        {
            if (Player.HeldItem.type != oldHeldItem)
            {
                count = -1;
            }
            if (Player.HeldItem.useStyle == BossRushUseStyle.Poke && count == 1)
            {
                switch (CountAmountOfThrustDid)
                {
                    case 1:
                        RotateThurst = MathH.ToRadByFiveTeen;
                        break;
                    case 2:
                        RotateThurst = 0;
                        break;
                    case 3:
                        RotateThurst = MathHelper.TwoPi - MathH.ToRadByFiveTeen;
                        break;
                }
            }
            else
            {
                RotateThurst = 0;
            }

        }
        private bool CanAttack(NPC npc)
        {
            if (!npc.active || npc.immune[Player.whoAmI] != 0)
            {
                return true;
            }
            return false;
        }
        private void ComboHandleSystem()
        {
            count++;
            if (count >= 3)
            {
                count = 0;
            }
        }
        private void SpinAttackExtraHit()
        {
            if (count != 2)
            {
                return;
            }
            Item item = Player.HeldItem;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.Hitbox.Intersects(SwordHitBox) && CanAttack(npc) && npc.immune[Player.whoAmI] > 0 && iframeCounter == 0)
                {
                    npc.StrikeNPC((int)(item.damage * 1.5f), item.knockBack, Player.direction, critReference);
                    iframeCounter = (int)(Player.itemAnimationMax * .333f);
                    Player.dpsDamage += (int)(item.damage * 1.5f);
                }
            }
        }
        public override void PostUpdate()
        {
            delaytimer = delaytimer > 0 ? delaytimer - 1 : 0;
            iframeCounter -= iframeCounter > 0 ? 1 : 0;
            Item item = Player.HeldItem;
            if (item.useStyle != BossRushUseStyle.Swipe &&
                item.useStyle != BossRushUseStyle.Poke &&
                item.useStyle != BossRushUseStyle.GenericSwingDownImprove
                )
            {
                return;
            }
            Player.HeldItem.noUseGraphic = true;
            if (Player.ItemAnimationJustStarted && Player.ItemAnimationActive && delaytimer == 0)
            {
                delaytimer = Player.itemAnimationMax + (int)(Player.itemAnimationMax * .34f);
                data = (Main.MouseWorld - Player.MountedCenter).SafeNormalize(Vector2.Zero);
                ComboHandleSystem();
            }
            if (Player.ItemAnimationActive)
            {
                Player.direction = data.X > 0 ? 1 : -1;
                if (item.useStyle == BossRushUseStyle.Swipe)
                {
                    SpinAttackExtraHit();
                }
            }
            oldHeldItem = item.type;
        }
    }
}