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
        public float NumOfProjectile { get => numOfProjectile; set => numOfProjectile = value; }
        public Vector2 Vec2ToRotate { get => vec2ToRotate; set => vec2ToRotate = value; }

        private float spreadModify = 1;
        public float SpreadModify1 { get => spreadModify; set => spreadModify = value; }
        public float ModifySpread(float TakeFloat) => SpreadModify <= 0 ? 0 : TakeFloat += SpreadModify;

        public Vector2 RotateRandom(float ToRadians)
        {
            float rotation = MathHelper.ToRadians(ModifySpread(ToRadians));
            return Vec2ToRotate.RotatedByRandom(rotation);
        }

        public Vector2 RotateCode(float ToRadians, float time = 0)
        {
            float rotation = MathHelper.ToRadians(ModifySpread(ToRadians));
            if (NumOfProjectile > 1)
            {
                return Vec2ToRotate.RotatedBy(MathHelper.Lerp(rotation / 2f, -rotation / 2f, time / (NumOfProjectile - 1f)));
            }
            return Vec2ToRotate;
        }
        public Vector2 PositionOFFSET(Vector2 position, Vector2 ProjectileVelocity, float offSetBy)
        {
            Vector2 OFFSET = ProjectileVelocity.SafeNormalize(Vector2.UnitX) * offSetBy;
            if (Collision.CanHitLine(position, 0, 0, position + OFFSET, 0, 0))
            {
                return position += OFFSET;
            }
            return position;
        }
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
        public static float SpreadModify = 1;

        /// <summary>
        /// Modify the ammount of projectile to be shoot
        /// </summary>
        /// <param name="NumAmount">the original amount</param>
        /// <returns></returns>
        public float ModifiedProjAmount(float NumAmount)
        {
            return NumAmount;
        }
        /// <summary>
        /// Modify the spread of a weapon
        /// </summary>
        /// <param name="TakeFloat">the amount to be change</param>
        /// <returns></returns>
        public float ModifySpread(float TakeFloat) => SpreadModify <= 0 ? 0 : TakeFloat += SpreadModify;

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
            return Main.rand.NextFloat() < 1/ChanceNotToConsume;
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
                Projectile.NewProjectile(source, position, RandomSpread(RotateRandom(SpreadAmount), AdditionalSpread, AdditionalMultiplier), type, damage, knockback, player.whoAmI);
            }
            NumOfProjectile = 0;
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
                        OffSetPost = 20;
                        SpreadAmount = 6;
                        break;
                    case ItemID.Minishark:
                        OffSetPost = 10;
                        SpreadAmount = 10;
                        AdditionalSpread = 2;
                        break;
                    case ItemID.Gatligator:
                        OffSetPost = 20;
                        SpreadAmount = 30;
                        AdditionalSpread = 3;
                        break;
                    case ItemID.Handgun:
                        OffSetPost = 10;
                        SpreadAmount = 15;
                        AdditionalSpread = 2;
                        break;
                    case ItemID.PhoenixBlaster:
                        OffSetPost = 10;
                        SpreadAmount = 12;
                        AdditionalSpread = 2;
                        break;
                    case ItemID.Musket:
                        OffSetPost = 35;
                        SpreadAmount = 5;
                        break;
                    case ItemID.TheUndertaker:
                        OffSetPost = 20;
                        SpreadAmount = 12;
                        break;
                    case ItemID.FlintlockPistol:
                        OffSetPost = 10;
                        SpreadAmount = 25;
                        AdditionalSpread = 4;
                        break;
                    case ItemID.Revolver:
                        OffSetPost = 10;
                        SpreadAmount = 17;
                        break;
                    case ItemID.ClockworkAssaultRifle:
                        OffSetPost = 15;
                        SpreadAmount = 19;
                        AdditionalSpread = 1;
                        break;
                    case ItemID.Megashark:
                        OffSetPost = 30;
                        SpreadAmount = 9;
                        AdditionalSpread = 2;
                        break;
                    case ItemID.Uzi:
                        SpreadAmount = 14;
                        AdditionalSpread = 1;
                        break;
                    case ItemID.VenusMagnum:
                        OffSetPost = 25;
                        SpreadAmount = 14;
                        AdditionalSpread = 2;
                        break;
                    case ItemID.SniperRifle:
                        OffSetPost = 35;
                        SpreadAmount = 2;
                        break;
                    case ItemID.ChainGun:
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
                        OffSetPost = 35;
                        SpreadAmount = 4;
                        AdditionalSpread = 2;
                        break;
                    case ItemID.Boomstick:
                        OffSetPost = 25;
                        SpreadAmount = 18;
                        AdditionalSpread = 35;
                        AdditionalMulti = .4f;
                        ShotguntType = true;
                        break;
                    case ItemID.QuadBarrelShotgun:
                        OffSetPost = 25;
                        SpreadAmount = 65;
                        ShotguntType = true;
                        break;
                    case ItemID.Shotgun:
                        OffSetPost = 35;
                        SpreadAmount = 24;
                        AdditionalSpread = 6;
                        AdditionalMulti = .5f;
                        ShotguntType = true;
                        break;
                    case ItemID.OnyxBlaster:
                        OffSetPost = 35;
                        SpreadAmount = 15;
                        ShotguntType = true;
                        break;
                    case ItemID.TacticalShotgun:
                        OffSetPost = 35;
                        SpreadAmount = 18;
                        AdditionalSpread = 3;
                        AdditionalMulti = .76f;
                        ShotguntType = true;
                        break;
                }
                position = PositionOFFSET(position, velocity, OffSetPost);
                GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, SpreadAmount, AdditionalSpread, AdditionalMulti, ShotguntType);
            }
        }
    }
}