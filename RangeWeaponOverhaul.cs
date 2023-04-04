using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace BossRush
{
    /// <summary>
    /// This is for specific gun that deal range damage only
    /// </summary>
    public static class RangeWeaponOverhaul
    {
        /// <summary>
        /// Use this to change how much weapon spread should be modify<br/>
        /// -For global modify use multiplication<br/>
        /// -For general modify use addictive<br/>
        /// Do not use SpreadModify = 0 as it will fuck the other stuff<br/>
        /// Best practice for this is to use + operator as it is what i use<br/>
        /// Using * is for globally and very hard to balance
        /// </summary>
        public static float SpreadModify = 1;
        /// <summary>
        /// This is use to set how many projectile you can shoot in actual weapon class<br/>
        /// Do not use NumOfProjectile = 0 as it will make gun unable to shoot<br/>
        /// Use this when making a range modder weapon
        /// </summary>
        public static int NumOfProjectile = 1;
        /// <summary>
        /// This is to modify the amount of projectiles you gonna shoot from weapon that got assigned with NumOfProjectile<br/>
        /// This is the safe way to modify the amount of projectile you can modify, do keep in mind to not go below -1 as it is very dangerous<br/>
        /// Use this if you are making a accessory
        /// </summary>
        public static float NumOfProModify = 0;
        /// <summary>
        /// Modify the ammount of projectile to be shoot
        /// </summary>
        /// <param name="TakeNumAmount">the original amount</param>
        /// <returns></returns>
        public static float ModifiedProjAmount(float TakeNumAmount)
        {
            return NumOfProModify + TakeNumAmount;
        }
        /// <summary>
        /// Modify the spread of a weapon
        /// </summary>
        /// <param name="TakeFloat">the amount to be change</param>
        /// <returns></returns>
        private static float ModifySpread(float TakeFloat) => SpreadModify <= 0 ? 0 : TakeFloat * SpreadModify;

        /// <summary>
        /// Return a Vector that got evenly distribute
        /// </summary>
        /// <param name="ToRadians">The radius that it get distribute</param>
        /// <param name="time">the current progress</param>
        /// <returns></returns>
        public static Vector2 RotateCode(this Vector2 Vec2ToRotate, float ToRadians, float i = 0)
        {
            float modifyradian = ModifySpread(ToRadians);
            float rotation = MathHelper.ToRadians(modifyradian) * .5f;
            if (NumOfProjectile > 1)
            {
                float RotateValue = MathHelper.Lerp(-rotation , rotation , i / (float)NumOfProjectile);
                return Vec2ToRotate.RotatedBy(RotateValue);
            }
            return Vec2ToRotate;
        }
        /// <summary>
        /// Return a random vector that got rotate randomly
        /// </summary>
        /// <param name="ToRadians">Rotate radius</param>
        /// <returns></returns>
        public static Vector2 RotateRandom(this Vector2 Vec2ToRotate, float ToRadians)
        {
            float rotation = MathHelper.ToRadians(ModifySpread(ToRadians));
            return Vec2ToRotate.RotatedByRandom(rotation);
        }
        /// <summary>
        /// Return a position Vector that got offset
        /// </summary>
        /// <param name="position">Original position</param>
        /// <param name="ProjectileVelocity">Current projectile velocity </param>
        /// <param name="offSetBy">Offset amount</param>
        /// <returns></returns>
        public static Vector2 PositionOFFSET(this Vector2 position, Vector2 ProjectileVelocity, float offSetBy)
        {
            Vector2 OFFSET = ProjectileVelocity.SafeNormalize(Vector2.UnitX) * offSetBy;
            if (Collision.CanHitLine(position, 0, 0, position + OFFSET, 0, 0))
            {
                return position += OFFSET;
            }
            return position;
        }

        /// <summary>
        /// Return a vector that got its X parameter and Y parameter change randomely
        /// </summary>
        /// <param name="ToRotateAgain">The original Vector</param>
        /// <param name="Spread">Value to change speed</param>
        /// <param name="additionalMultiplier">Multiplier for final speed change</param>
        /// <returns></returns>
        public static Vector2 RandomSpread(this Vector2 ToRotateAgain, float Spread, float additionalMultiplier = 1)
        {
            ToRotateAgain.X += (Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier) * ModifySpread(1);
            ToRotateAgain.Y += (Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier) * ModifySpread(1);
            return ToRotateAgain;
        }

        public static int[] GunType = {
            ItemID.RedRyder,
            ItemID.Minishark,
            ItemID.Gatligator,
            ItemID.Handgun,
            ItemID.PhoenixBlaster,
            ItemID.Musket,
            ItemID.TheUndertaker,
            ItemID.FlintlockPistol,
            ItemID.Revolver,
            ItemID.ClockworkAssaultRifle,
            ItemID.Megashark,
            ItemID.Uzi,
            ItemID.VenusMagnum,
            ItemID.SniperRifle,
            ItemID.ChainGun,
            ItemID.SDMG,
            ItemID.Boomstick,
            ItemID.QuadBarrelShotgun,
            ItemID.Shotgun,
            ItemID.OnyxBlaster,
            ItemID.TacticalShotgun
        };
        /// <summary>
        /// Method that make the item currently in use can be shoot by many amount at a random spread<br/>
        /// It is better to use this method if you want to make your weapon affected by spread in ModifyShootStats
        /// </summary>
        /// <param name="player"></param>
        /// <param name="source"></param>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="type"></param>
        /// <param name="damage"></param>
        /// <param name="knockback"></param>
        /// <param name="SpreadAmount">Rotation radius</param>
        /// <param name="AdditionalSpread">Addition X and Y modifier</param>
        /// <param name="AdditionalMultiplier">Multiplier for final speed change</param>
        /// </param>
        public static void NewGunShotProjectile(
            Player player,
            EntitySource_ItemUse_WithAmmo source,
            ref Vector2 position,
            ref Vector2 velocity,
            ref int type,
            ref int damage,
            ref float knockback,
            int NumOfProjectile = 1,
            float SpreadAmount = 0,
            float AdditionalSpread = 0,
            float AdditionalMultiplier = 1)
        {
            int ProjectileAmount = (int)ModifiedProjAmount(NumOfProjectile);
            if (ProjectileAmount == 1)
            {
                velocity = velocity.RotateRandom(SpreadAmount).RandomSpread(AdditionalSpread, AdditionalMultiplier);
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                return;
            }
            for (int i = 0; i < ProjectileAmount; i++)
            {
                Vector2 velocity2 = velocity.RotateRandom(SpreadAmount).RandomSpread(AdditionalSpread, AdditionalMultiplier);
                Projectile.NewProjectile(source, position, velocity2, type, damage, knockback, player.whoAmI);
            }
        }
    }
    public class GlobalHandleSystem : ModSystem
    {
        public override void PostUpdateItems()
        {
            RangeWeaponOverhaul.NumOfProModify = 0;
            RangeWeaponOverhaul.SpreadModify = 1;
            RangeWeaponOverhaul.NumOfProjectile = 1;
        }
    }
    public class GlobalWeaponModify : GlobalItem
    {
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (ModContent.GetInstance<BossRushModConfig>().DisableWeaponOverhaul)
            {
                return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
            }
            //if (!player.GetModPlayer<OverhaulWeaponPlayer>().OverhaulWeapon)
            //{
            //    return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
            //}
            if (item.type == ItemID.VortexBeater)
            {
                return true;
            }
            if (item.type == ItemID.OnyxBlaster)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.BlackBolt, damage * 3, knockback, player.whoAmI);
            }
            for (int i = 0; i < RangeWeaponOverhaul.GunType.Length; i++)
            {
                if (item.type == RangeWeaponOverhaul.GunType[i] && AppliesToEntity(item, true))
                {
                    return false;
                }
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (ModContent.GetInstance<BossRushModConfig>().DisableWeaponOverhaul)
            {
                return;
            }
            //if(!player.GetModPlayer<OverhaulWeaponPlayer>().OverhaulWeapon)
            //{
            //    return;
            //}
            var source = new EntitySource_ItemUse_WithAmmo(player, item, item.ammo);
            if (AppliesToEntity(item, false))
            {
                float OffSetPost = 0;
                float SpreadAmount = 0;
                float AdditionalSpread = 0;
                float AdditionalMulti = 1;
                int NumOfProjectile = 0;
                switch (item.type)
                {
                    case ItemID.RedRyder:
                        NumOfProjectile = 1;
                        OffSetPost = 20;
                        SpreadAmount = 6;
                        break;
                    case ItemID.Minishark:
                        NumOfProjectile = 1;
                        OffSetPost = 20;
                        SpreadAmount = 7;
                        AdditionalSpread = 2;
                        break;
                    case ItemID.Gatligator:
                        NumOfProjectile = 1;
                        OffSetPost = 20;
                        SpreadAmount = 30;
                        AdditionalSpread = 3;
                        break;
                    case ItemID.Handgun:
                        NumOfProjectile = 1;
                        OffSetPost = 10;
                        SpreadAmount = 15;
                        AdditionalSpread = 2;
                        break;
                    case ItemID.PhoenixBlaster:
                        NumOfProjectile = 1;
                        OffSetPost = 10;
                        SpreadAmount = 12;
                        AdditionalSpread = 2;
                        break;
                    case ItemID.Musket:
                        NumOfProjectile = 1;
                        OffSetPost = 35;
                        SpreadAmount = 5;
                        break;
                    case ItemID.TheUndertaker:
                        NumOfProjectile = 1;
                        OffSetPost = 20;
                        SpreadAmount = 12;
                        break;
                    case ItemID.FlintlockPistol:
                        NumOfProjectile = 1;
                        OffSetPost = 10;
                        SpreadAmount = 25;
                        AdditionalSpread = 4;
                        break;
                    case ItemID.Revolver:
                        NumOfProjectile = 1;
                        OffSetPost = 10;
                        SpreadAmount = 17;
                        break;
                    case ItemID.ClockworkAssaultRifle:
                        NumOfProjectile = 1;
                        OffSetPost = 15;
                        SpreadAmount = 19;
                        AdditionalSpread = 1;
                        break;
                    case ItemID.Megashark:
                        NumOfProjectile = 1;
                        OffSetPost = 30;
                        SpreadAmount = 9;
                        AdditionalSpread = 2;
                        break;
                    case ItemID.Uzi:
                        NumOfProjectile = 1;
                        SpreadAmount = 14;
                        AdditionalSpread = 1;
                        break;
                    case ItemID.VenusMagnum:
                        NumOfProjectile = 1;
                        OffSetPost = 25;
                        SpreadAmount = 14;
                        AdditionalSpread = 2;
                        break;
                    case ItemID.SniperRifle:
                        NumOfProjectile = 1;
                        OffSetPost = 35;
                        SpreadAmount = 2;
                        break;
                    case ItemID.ChainGun:
                        NumOfProjectile = 1;
                        OffSetPost = 35;
                        SpreadAmount = 33;
                        AdditionalSpread = 3;
                        break;
                    case ItemID.VortexBeater:
                        OffSetPost = 35;
                        SpreadAmount = 20;
                        AdditionalSpread = 2;
                        break;
                    case ItemID.SDMG:
                        NumOfProjectile = 1;
                        OffSetPost = 35;
                        SpreadAmount = 4;
                        AdditionalSpread = 2;
                        break;
                    case ItemID.Boomstick:
                        OffSetPost = 25;
                        SpreadAmount = 18;
                        AdditionalSpread = 4;
                        AdditionalMulti = .4f;
                        NumOfProjectile += Main.rand.Next(4, 6);
                        break;
                    case ItemID.QuadBarrelShotgun:
                        OffSetPost = 25;
                        SpreadAmount = 45;
                        AdditionalSpread = 6;
                        NumOfProjectile += 6;
                        break;
                    case ItemID.Shotgun:
                        OffSetPost = 35;
                        SpreadAmount = 24;
                        AdditionalSpread = 6;
                        AdditionalMulti = .5f;
                        NumOfProjectile += Main.rand.Next(4, 6);
                        break;
                    case ItemID.OnyxBlaster:
                        OffSetPost = 35;
                        SpreadAmount = 15;
                        AdditionalSpread = 6;
                        NumOfProjectile += Main.rand.Next(4, 6);
                        break;
                    case ItemID.TacticalShotgun:
                        OffSetPost = 35;
                        SpreadAmount = 18;
                        AdditionalSpread = 3;
                        AdditionalMulti = .76f;
                        NumOfProjectile += 6;
                        break;
                }
                position = position.PositionOFFSET(velocity, OffSetPost);
                RangeWeaponOverhaul.NewGunShotProjectile(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, NumOfProjectile, SpreadAmount, AdditionalSpread, AdditionalMulti);
            }
        }
    }
}