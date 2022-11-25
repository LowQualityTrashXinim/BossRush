using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace BossRush
{
    public abstract class WeaponTemplate : ModItem
    {
        private float numOfProjectile = 1;
        protected Vector2 vec2ToRotate;
        public float SpreadModify { get => SpreadModify1; set => SpreadModify1 = value; }
        /// <summary>
        /// Base amount of projectile
        /// <br/>Safe way to use this is to just reset it back to 1 each time the code have to reuse the math here
        /// </summary>
        public float NumOfProjectile { get => numOfProjectile; set => numOfProjectile = value; }
        /// <summary>
        /// Base Vector2
        /// <br/>Safe way to use this is just to set the base velocity at the start of the code
        /// </summary>
        public Vector2 Vec2ToRotate { get => vec2ToRotate; set => vec2ToRotate = value; }

        private float spreadModify = 1;
        public float SpreadModify1 { get => spreadModify; set => spreadModify = value; }
        public float ModifySpread(float TakeFloat) => SpreadModify <= 0 ? 0 : TakeFloat += SpreadModify;
        /// <summary>
        /// Return a random vector that got rotate randomly<br/>
        /// remember to state the amount of projectiles using numOfPojectile !<br/>
        /// remember to set the base velocity using vec2ToRotate !
        /// </summary>
        /// <param name="ToRadians">Rotate radius</param>
        /// <returns></returns>
        public Vector2 RotateRandom(float ToRadians)
        {
            float rotation = MathHelper.ToRadians(ModifySpread(ToRadians));
            return Vec2ToRotate.RotatedByRandom(rotation);
        }
        /// <summary>
        /// Return a Vector that got evenly distribute<br/>
        /// remember to state the amount of projectiles using numOfPojectile !<br/>
        /// remember to set the base velocity using vec2ToRotate !
        /// </summary>
        /// <param name="ToRadians">The radius that it get distribute</param>
        /// <param name="time">the current progress</param>
        /// <returns></returns>
        public Vector2 RotateCode(float ToRadians, float time = 0)
        {
            float rotation = MathHelper.ToRadians(ModifySpread(ToRadians));
            if (NumOfProjectile > 1)
            {
                return Vec2ToRotate.RotatedBy(MathHelper.Lerp(rotation / 2f, -rotation / 2f, time / (NumOfProjectile - 1f)));
            }
            return Vec2ToRotate;
        }
        /// <summary>
        /// Return a position Vector that got offset
        /// </summary>
        /// <param name="position">Original position</param>
        /// <param name="ProjectileVelocity">Current projectile velocity </param>
        /// <param name="offSetBy">Offset amount</param>
        /// <returns></returns>
        public Vector2 PositionOFFSET(Vector2 position, Vector2 ProjectileVelocity, float offSetBy)
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
        public Vector2 RandomSpread(Vector2 ToRotateAgain, int Spread, float additionalMultiplier = 1)
        {
            ToRotateAgain.X += (Main.rand.Next(-Spread, Spread) * additionalMultiplier) * ModifySpread(1);
            ToRotateAgain.Y += (Main.rand.Next(-Spread, Spread) * additionalMultiplier) * ModifySpread(1);
            return ToRotateAgain;
        }
    }

    public class GlobalWeaponModify : GlobalItem
    {
        public static float NumOfProjectile = 0;
        public static Vector2 Vec2ToRotate = Vector2.Zero;
        /// <summary>
        /// Use this to change how much weapon spread should be modify
        /// For global modify use multiplication
        /// For general modify use addictive
        /// Do not use SpreadModify = 0 as it will fuck the other stuff
        /// </summary>
        public static float SpreadModify = 1;
        /// <summary>
        /// Modify the ammount of projectile to be shoot
        /// </summary>
        /// <param name="TakeNumAmount">the original amount</param>
        /// <returns></returns>
        public float ModifiedProjAmount(float TakeNumAmount)
        {
            return TakeNumAmount;
        }
        /// <summary>
        /// Modify the spread of a weapon
        /// </summary>
        /// <param name="TakeFloat">the amount to be change</param>
        /// <returns></returns>
        public static float ModifySpread(float TakeFloat) => SpreadModify <= 0 ? 0 : TakeFloat * SpreadModify;

        /// <summary>
        /// Return a random vector that got rotate randomly
        /// </summary>
        /// <param name="ToRadians">Rotate radius</param>
        /// <returns></returns>
        public Vector2 RotateRandom(float ToRadians)
        {
            float rotation = MathHelper.ToRadians(ModifySpread(ToRadians));
            return Vec2ToRotate.RotatedByRandom(rotation);
        }
        public override bool CanConsumeAmmo(Item weapon, Item ammo, Player player)
        {
            float ChanceNotToConsume = weapon.useTime <= 20 && weapon.useTime >= 7 ? weapon.useTime * .35f : weapon.useTime < 7 ? weapon.useTime * 1.4f : weapon.useTime;
            for (int i = 0; i < SpecialGunType.Length; i++)
            {
                if(weapon.type == SpecialGunType[i])
                {
                    return true;
                }
            }
            return Math.Round(Main.rand.NextFloat(),2) > 1/Math.Round(ChanceNotToConsume,2);
        }
        /// <summary>
        /// Return a Vector that got evenly distribute
        /// </summary>
        /// <param name="ToRadians">The radius that it get distribute</param>
        /// <param name="time">the current progress</param>
        /// <returns></returns>
        public Vector2 RotateCode(float ToRadians, float time = 0)
        {
            float rotation = MathHelper.ToRadians(ModifySpread(ToRadians));
            if (NumOfProjectile > 1)
            {
                return Vec2ToRotate.RotatedBy(MathHelper.Lerp(rotation / 2f, -rotation / 2f, time / (NumOfProjectile - 1f)));
            }
            return Vec2ToRotate;
        }
        /// <summary>
        /// Return a position Vector that got offset
        /// </summary>
        /// <param name="position">Original position</param>
        /// <param name="ProjectileVelocity">Current projectile velocity </param>
        /// <param name="offSetBy">Offset amount</param>
        /// <returns></returns>
        public Vector2 PositionOFFSET(Vector2 position, Vector2 ProjectileVelocity, float offSetBy)
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
        public Vector2 RandomSpread(Vector2 ToRotateAgain, float Spread, float additionalMultiplier = 1)
        {
            ToRotateAgain.X += (Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier) * ModifySpread(1);
            ToRotateAgain.Y += (Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier) * ModifySpread(1);
            return ToRotateAgain;
        }

        /// <summary>
        /// Method that make the item currently in use can be shoot by many amount at a random spread
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
        /// <param name="ItemISaShotgun">
        /// Set true if the Item is a shotgun to make it don't change the angle it aim at <br/>
        /// Set false if the Item is not a shotgun to make it emulate recoil<br/>
        /// </param>
        public void GlobalRandomSpreadFiring(Player player, EntitySource_ItemUse_WithAmmo source, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback, float SpreadAmount = 0, float AdditionalSpread = 0, float AdditionalMultiplier = 1, bool ItemISaShotgun = false)
        {
            Vec2ToRotate = velocity;
            if (!ItemISaShotgun)
            {
                velocity = RandomSpread(RotateRandom(SpreadAmount), AdditionalSpread, AdditionalMultiplier);
            }
            for (int i = 0; i < ModifiedProjAmount(NumOfProjectile); i++)
            {
                velocity = RandomSpread(RotateRandom(SpreadAmount), AdditionalSpread, AdditionalMultiplier);
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            }
            NumOfProjectile = 0;
        }
        int[] GunType = { 
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

        int[] SpecialGunType = {
            ItemID.Boomstick,
            ItemID.QuadBarrelShotgun,
            ItemID.Shotgun,
            ItemID.OnyxBlaster,
            ItemID.TacticalShotgun
        };
        public override bool InstancePerEntity => true;
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(item.type == ItemID.VortexBeater)
            {
                return true;
            }
            for (int i = 0; i < GunType.Length; i++)
            {
                if (player.HeldItem.type == GunType[i] && AppliesToEntity(item, true))
                { 
                    if (item.type == ItemID.OnyxBlaster)
                    {
                        Projectile.NewProjectile(source, position, velocity, ProjectileID.BlackBolt, damage * 3, knockback, player.whoAmI);
                    }
                    return false;
                }
            }
            return base.Shoot(item,player,source,position,velocity,type,damage,knockback);
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vec2ToRotate = velocity;
            var source = new EntitySource_ItemUse_WithAmmo(player, item, item.ammo);
            if (AppliesToEntity(item, false))
            {
                float OffSetPost = 0;
                float SpreadAmount = 0;
                float AdditionalSpread = 0;
                float AdditionalMulti = 1;
                bool ShotguntType = false;
                switch (item.type)
                {
                    case ItemID.RedRyder:
                        NumOfProjectile = 1;
                        OffSetPost = 20;
                        SpreadAmount = 6;
                        break;
                    case ItemID.Minishark:
                        NumOfProjectile = 1;
                        OffSetPost = 10;
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
                        ShotguntType = true;
                        NumOfProjectile += Main.rand.Next(4, 6);
                        break;
                    case ItemID.QuadBarrelShotgun:
                        OffSetPost = 25;
                        SpreadAmount = 45;
                        AdditionalSpread = 6;
                        ShotguntType = true;
                        NumOfProjectile += 6;
                        break;
                    case ItemID.Shotgun:
                        OffSetPost = 35;
                        SpreadAmount = 24;
                        AdditionalSpread = 6;
                        AdditionalMulti = .5f;
                        ShotguntType = true;
                        NumOfProjectile += Main.rand.Next(4, 6);
                        break;
                    case ItemID.OnyxBlaster:
                        OffSetPost = 35;
                        SpreadAmount = 15;
                        AdditionalSpread = 6;
                        ShotguntType = true;
                        NumOfProjectile += Main.rand.Next(4, 6);
                        break;
                    case ItemID.TacticalShotgun:
                        OffSetPost = 35;
                        SpreadAmount = 18;
                        AdditionalSpread = 3;
                        AdditionalMulti = .76f;
                        ShotguntType = true;
                        NumOfProjectile += 6;
                        break;
                }
                position = PositionOFFSET(position, velocity, OffSetPost);
                GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, SpreadAmount, AdditionalSpread, AdditionalMulti, ShotguntType);
                SpreadModify = 1;
            }
        }
    }
}