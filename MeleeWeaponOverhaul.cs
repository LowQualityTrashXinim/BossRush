using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace BossRush
{
    internal class MeleeWeaponOverhaul : GlobalItem
    {
        public class CustomUsestyleID
        {
            public const int Swipe = 16;
        }

        public override void SetDefaults(Item item)
        {
            if (ModContent.GetInstance<BossRushModConfig>().DisableWeaponOverhaul)
            {
                return;
            }
            #region Vanilla Fixes
            if (item.type == ItemID.FieryGreatsword)
            {
                item.width = 54;
                item.height = 54;
            }
            if (item.type == ItemID.Frostbrand)
            {
                item.width = 46;
                item.height = 50;
            }
            if (item.type == ItemID.CactusSword)
            {
                item.width = 48;
                item.height = 48;
            }
            if (item.type == ItemID.TerraBlade)
            {
                item.width = 46;
                item.height = 54;
            }
            if (item.type == ItemID.Meowmere)
            {
                item.width = 50;
                item.height = 58;
            }
            #endregion
            switch (item.type)
            {
                #region CustomUsestyleID.Swipe
                case ItemID.PearlwoodSword:
                case ItemID.BorealWoodSword:
                case ItemID.PalmWoodSword:
                case ItemID.ShadewoodSword:
                case ItemID.EbonwoodSword:
                case ItemID.BladeofGrass:
                case ItemID.FieryGreatsword:
                case ItemID.LightsBane:
                case ItemID.EnchantedSword:
                case ItemID.BeeKeeper:
                case ItemID.Muramasa:
                case ItemID.PurplePhaseblade:
                case ItemID.BluePhaseblade:
                case ItemID.GreenPhaseblade:
                case ItemID.YellowPhaseblade:
                case ItemID.OrangePhaseblade:
                case ItemID.RedPhaseblade:
                case ItemID.WhitePhaseblade:
                case ItemID.TinBroadsword:
                case ItemID.SilverBroadsword:
                case ItemID.LeadBroadsword:
                case ItemID.GoldBroadsword:

                case ItemID.Bladetongue:
                case ItemID.BeamSword:
                case ItemID.PurplePhasesaber:
                case ItemID.BluePhasesaber:
                case ItemID.GreenPhasesaber:
                case ItemID.YellowPhasesaber:
                case ItemID.OrangePhasesaber:
                case ItemID.RedPhasesaber:
                case ItemID.WhitePhasesaber:

                case ItemID.CobaltSword:
                case ItemID.MythrilSword:
                case ItemID.TitaniumSword:

                case ItemID.Excalibur:
                case ItemID.TrueExcalibur:
                case ItemID.ChlorophyteClaymore:

                case ItemID.TerraBlade:
                case ItemID.TheHorsemansBlade:

                case ItemID.InfluxWaver:

                case ItemID.Meowmere:
                    item.useStyle = CustomUsestyleID.Swipe;
                    item.reuseDelay = item.useAnimation / 3;
                    break;
                #endregion
                default:
                    break;
            }
            if (item.useStyle == CustomUsestyleID.Swipe)
            {
                if (item.reuseDelay < 2)
                    item.reuseDelay = 2; //Minumum of 2 reuseDelay
            }
            base.SetDefaults(item);
        }
        public override bool CanShoot(Item item, Player player)
        {
            if (item.useStyle == CustomUsestyleID.Swipe)
            {
                return base.CanShoot(item, player) && player.GetModPlayer<MeleeOverhaulPlayer>().useStyleData == player.direction;
            }
            return base.CanShoot(item, player);
        }
        public override bool? UseItem(Item Item, Player player)
        {
            if (Item.useStyle == CustomUsestyleID.Swipe)
            {
                if (player.ItemAnimationJustStarted)
                {
                    MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
                    modPlayer.useStyleData = Math.Abs(modPlayer.useStyleData) != 1 ? Main.rand.Next(new int[] { -1, 1 }) : modPlayer.useStyleData * -1;
                    modPlayer.data = (Main.MouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero);
                    player.direction = ((Vector2)modPlayer.data).X > 0 ? 1 : -1;

                    #region GhostItem Fix
                    //Easing function
                    float percentDone = MathHelper.Clamp(((player.itemAnimationMax * .225f) / ((float)player.itemAnimation) - .225f), 0f, 1f);
                    if (modPlayer.isItemDelay)
                        percentDone = 1f;

                    //This value represets the angle that the item will be swung at.
                    float angle = MathHelper.Clamp(player.GetAdjustedItemScale(Item) * (Item.width * Item.height) * .0056f * Item.useAnimation, 30f, 450f);
                    angle = MathHelper.ToRadians(angle); //Convert to radians

                    //Run various calculations:
                    //1 ... Get the baseline rotation (player -> cursor)
                    if (modPlayer.data == null)
                        modPlayer.data = (Main.MouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero);
                    float baseAngle = ((Vector2)(modPlayer.data)).ToRotation();
                    player.direction = ((Vector2)modPlayer.data).X > 0 ? 1 : -1;

                    //2 ... Get the start and end rotational values
                    float start = baseAngle + (modPlayer.useStyleData * (angle * .5f));
                    float end = baseAngle - (modPlayer.useStyleData * (angle * .5f));

                    //3 ... Given the previous 2 values, and the percent way through the animation...
                    //Get the current rotational value
                    float currentAngle = MathHelper.Lerp(start, end, percentDone);

                    //4 ... Finally, apply these maths to the item rotation and player arm
                    player.itemRotation = currentAngle;
                    if (player.direction > 0)
                    {
                        player.itemRotation += MathHelper.PiOver4;
                    }
                    else
                    {
                        player.itemRotation += MathHelper.PiOver4 * 3f;
                    }

                    player.compositeFrontArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Full, currentAngle - MathHelper.PiOver2);

                    //5 ... Move the back arm, so it does not look like the player's other arm is completely still
                    player.compositeBackArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Quarter, 3 * MathHelper.PiOver2);

                    //6 ... Show item at correct location
                    float distance = (player.itemWidth * player.itemHeight) * .00625f;
                    player.itemLocation = player.MountedCenter + Vector2.UnitX.RotatedBy(currentAngle) * distance;
                    #endregion
                    return true;
                }
            }
            return base.UseItem(Item, player);
        }
        public override void UseStyle(Item Item, Player player, Rectangle heldItemFrame)
        {
            if (Item.useStyle == CustomUsestyleID.Swipe)
            {
                //Get the modplayer responsible for handling this custom usestyle
                MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();

                //Easing function
                float percentDone = MathHelper.Clamp(((player.itemAnimationMax * .225f) / ((float)player.itemAnimation) - .225f), 0f, 1f);
                if (modPlayer.isItemDelay)
                    percentDone = 1f;

                //This value represets the angle that the item will be swung at.
                float angle = MathHelper.Clamp(player.GetAdjustedItemScale(Item) * (Item.width * Item.height) * .0056f * Item.useAnimation, 30f, 450f);
                angle = MathHelper.ToRadians(angle); //Convert to radians

                //Run various calculations:
                //1 ... Get the baseline rotation (player -> cursor)
                if (modPlayer.data == null)
                    modPlayer.data = (Main.MouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero);
                float baseAngle = ((Vector2)(modPlayer.data)).ToRotation();
                player.direction = ((Vector2)modPlayer.data).X > 0 ? 1 : -1;

                //2 ... Get the start and end rotational values
                float start = baseAngle + (modPlayer.useStyleData * (angle * .5f));
                float end = baseAngle - (modPlayer.useStyleData * (angle * .5f));

                //3 ... Given the previous 2 values, and the percent way through the animation...
                //Get the current rotational value
                float currentAngle = MathHelper.Lerp(start, end, percentDone);

                //4 ... Finally, apply these maths to the item rotation and player arm
                player.itemRotation = currentAngle;
                if (player.direction > 0)
                {
                    player.itemRotation += MathHelper.PiOver4;
                }
                else
                {
                    player.itemRotation += MathHelper.PiOver4 * 3f;
                }

                player.compositeFrontArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Full, currentAngle - MathHelper.PiOver2);

                //5 ... Move the back arm, so it does not look like the player's other arm is completely still
                player.compositeBackArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Quarter, 3 * MathHelper.PiOver2);

                //6 ... Show item at correct location
                float distance = (player.itemWidth * player.itemHeight) * .00625f;
                player.itemLocation = player.MountedCenter + Vector2.UnitX.RotatedBy(currentAngle) * distance;
            }
        }
        public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            if (item.useStyle == CustomUsestyleID.Swipe)
            {
                //Helper method
                (int, int) Order(float v1, float v2) => v1 < v2 ? ((int)v1, (int)v2) : ((int)v2, (int)v1);

                if (player.GetModPlayer<MeleeOverhaulPlayer>().isItemDelay || player.ItemAnimationJustStarted)
                {
                    noHitbox = true;
                }

                //Get the direction of the weapon, and the distance from the player to the hilt
                float distance = (player.itemWidth * player.itemHeight) * .00625f;
                Vector2 handPos = Vector2.UnitY.RotatedBy(player.compositeFrontArm.rotation);

                //Use afforementioned direction, and get the distance from the player to the tip of the weapon
                float length = (item.width + item.height) * player.GetAdjustedItemScale(item);
                Vector2 endPos = handPos;

                //Use values obtained above to construct an approximation of the two most important points
                handPos *= distance;
                endPos *= length;
                handPos += player.MountedCenter;
                endPos += player.MountedCenter;

                //Use helper method to get coordinates and size for the rectangle
                (int X1, int X2) XVals = Order(handPos.X, endPos.X);
                (int Y1, int Y2) YVals = Order(handPos.Y, endPos.Y);

                //Create the new bounds of the hitbox
                hitbox = new Rectangle(XVals.X1 - 2, YVals.Y1 - 2, XVals.X2 - XVals.X1 + 2, YVals.Y2 - YVals.Y1 + 2);
            }
        }
        public override void ModifyHitNPC(Item Item, Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (Item.useStyle == CustomUsestyleID.Swipe)
            {
                //Get the modplayer responsible for handling this custom usestyle
                MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();

                //Easing function
                float percentDone = MathHelper.Clamp(((player.itemAnimationMax * .2f) / ((float)player.itemAnimation) - .2f), 0f, 1f);
                if (modPlayer.isItemDelay)
                    percentDone = 1f;

                //Get a multiplier based on the swing progress
                float mult = MathHelper.Lerp(.85f, 1.2f, percentDone);

                //Apply multiplier to the relevant fields, and floor it
                damage = (int)(damage * mult);
                knockBack *= mult;
            }
            base.ModifyHitNPC(Item, player, target, ref damage, ref knockBack, ref crit);
        }
    }
    public class MeleeOverhaulPlayer : ModPlayer
    {
        public float useStyleData;
        public bool isItemDelay;
        public object data;
    }
}
