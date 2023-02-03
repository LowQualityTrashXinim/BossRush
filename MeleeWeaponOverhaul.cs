using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static BossRush.MeleeWeaponOverhaul;
using BossRush.Items.Weapon;

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
            if (item.type == ItemID.Starfury)
            {
                item.width = item.height = 42;
            }
            #endregion
            switch (item.type)
            {
                #region CustomUsestyleID.Swipe
                //WoodSword
                case ItemID.PearlwoodSword:
                case ItemID.BorealWoodSword:
                case ItemID.PalmWoodSword:
                case ItemID.ShadewoodSword:
                case ItemID.EbonwoodSword:
                //PreHMSword
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
                case ItemID.Katana:
                //preHMSaber
                case ItemID.Bladetongue:
                case ItemID.BeamSword:
                case ItemID.Starfury:
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
                    item.useTurn = false;
                    break;
                #endregion
                default:
                    break;
            }
            base.SetDefaults(item);
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.useStyle != CustomUsestyleID.Swipe)
            {
                return base.CanUseItem(item, player);
            }
            return player.GetModPlayer<MeleeOverhaulPlayer>().delaytimer <= 0;
        }
        public override float UseSpeedMultiplier(Item item, Player player)
        {
            float useTimeMultiplierOnCombo = 1;
            if (item.useStyle == CustomUsestyleID.Swipe)
            {
                MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
                if (modPlayer.count == 1)
                {
                    useTimeMultiplierOnCombo -= .5f;
                }
            }
            return useTimeMultiplierOnCombo;
        }
        public override void UseStyle(Item Item, Player player, Rectangle heldItemFrame)
        {
            if (Item.useStyle == CustomUsestyleID.Swipe)
            {
                MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
                Item.noUseGraphic = false;
                if (modPlayer.count == 2)
                {
                    CircleSwingAttack(player, modPlayer);
                    return;
                }
                SwipeAttack(player, modPlayer);
            }
        }
        public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            //this remain untouch cause idk what in the hell should i change here
            if (item.useStyle == CustomUsestyleID.Swipe)
            {
                //Get the direction of the weapon, and the distance from the player to the hilt
                float distance = player.itemWidth * player.itemHeight * .00625f;
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
                player.GetModPlayer<MeleeOverhaulPlayer>().SwordHitBox = hitbox;
                int damage = player.GetModPlayer<MeleeOverhaulPlayer>().count == 2 ? (int)(player.HeldItem.damage * 1.5f) : player.HeldItem.damage;
                int proj = Projectile.NewProjectile(item.GetSource_ItemUse(item), player.itemLocation, player.GetModPlayer<MeleeOverhaulPlayer>().data, ModContent.ProjectileType<GhostHitBox2>(), damage, player.HeldItem.knockBack, player.whoAmI);
                Main.projectile[proj].Hitbox = hitbox;
            }
        }

        private static (int, int) Order(float v1, float v2) => v1 < v2 ? ((int)v1, (int)v2) : ((int)v2, (int)v1);
        public override void ModifyHitNPC(Item Item, Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (Item.useStyle == CustomUsestyleID.Swipe)
            {
                MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
                float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
                float mult = MathHelper.Lerp(.85f, 1.2f, percentDone);
                damage = (int)(damage * mult);
                knockBack *= mult;
                modPlayer.critReference = crit;
                if (modPlayer.count == 2)
                {
                    damage = (int)(damage * 1.5f);
                }
                int proj = Projectile.NewProjectile(Item.GetSource_ItemUse(Item), player.itemLocation, Vector2.Zero, ModContent.ProjectileType<GhostHitBox2>(), damage, knockBack, player.whoAmI);
                Main.projectile[proj].Hitbox = modPlayer.SwordHitBox;
            }
            base.ModifyHitNPC(Item, player, target, ref damage, ref knockBack, ref crit);
        }
        private void SwipeAttack(Player player, MeleeOverhaulPlayer modPlayer)
        {
            int VerticleDirectionSwipe = modPlayer.count == 0 ? -1 : 1;
            float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
            float baseAngle = modPlayer.data.ToRotation();
            float angle = MathHelper.ToRadians(baseAngle + 90) * player.direction;
            float start = baseAngle + angle * VerticleDirectionSwipe;
            float end = baseAngle - angle * VerticleDirectionSwipe;
            float currentAngle = MathHelper.SmoothStep(start, end, BossRushUtils.InExpo(percentDone));
            player.itemRotation = currentAngle;
            player.itemRotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f;
            player.compositeFrontArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Full, currentAngle - MathHelper.PiOver2);
            float distance = (player.itemWidth * player.itemHeight) * .00625f;
            player.itemLocation = player.MountedCenter + Vector2.UnitX.RotatedBy(currentAngle) * distance;
        }

        private void CircleSwingAttack(Player player, MeleeOverhaulPlayer modPlayer)
        {
            float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
            float baseAngle = modPlayer.data.ToRotation();
            float start = baseAngle + MathHelper.PiOver2 * player.direction;
            float end = baseAngle - (MathHelper.TwoPi + MathHelper.PiOver2) * player.direction;
            float currentAngle = MathHelper.SmoothStep(start, end, BossRushUtils.InExpo(percentDone));
            player.itemRotation = currentAngle;
            player.itemRotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f;
            player.compositeFrontArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Full, currentAngle - MathHelper.PiOver2);
            float distance = (player.itemWidth * player.itemHeight) * .00625f;
            player.itemLocation = player.MountedCenter + Vector2.UnitX.RotatedBy(currentAngle) * distance;
        }
    }
    public class MeleeOverhaulPlayer : ModPlayer
    {
        private bool CanAttack(NPC npc)
        {
            if (!npc.active || npc.immune[Player.whoAmI] != 0)
            {
                return true;
            }
            return false;
        }
        public Vector2 data;
        public int count = -1;
        public Rectangle SwordHitBox;
        public bool critReference;
        int iframeCounter = 0;
        public int delaytimer = 0;
        public override void PostUpdate()
        {
            delaytimer = delaytimer > 0 ? delaytimer - 1 : 0;
            iframeCounter -= iframeCounter > 0 ? 1 : 0;
            if (Player.HeldItem.useStyle != CustomUsestyleID.Swipe)
            {
                return;
            }
            Player.HeldItem.noUseGraphic = true;
            if (Player.ItemAnimationJustStarted && delaytimer == 0)
            {
                delaytimer = Player.itemAnimationMax + (int)(Player.itemAnimationMax * .34f);
                data = (Main.MouseWorld - Player.MountedCenter).SafeNormalize(Vector2.Zero);
                count++;
                if (count >= 3)
                {
                    count = 0;
                }
            }
            if (Player.ItemAnimationActive)
            {
                Player.direction = data.X > 0 ? 1 : -1;
                Item item = Player.HeldItem;
                //MeleeHit();
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.Hitbox.Intersects(SwordHitBox) && CanAttack(npc) && npc.immune[Player.whoAmI] > 0 && iframeCounter == 0 && count == 2)
                    {
                        npc.StrikeNPC((int)(item.damage * 1.5f), item.knockBack, Player.direction, critReference);
                        iframeCounter = (int)(Player.itemAnimationMax * .333f);
                        Player.dpsDamage += (int)(item.damage * 1.5f);
                    }
                }
            }
        }
    }
}