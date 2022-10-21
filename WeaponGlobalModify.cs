using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace BossRush
{
    public abstract class WeaponDataStoreValue : ModPlayer
    {
        private float numOfProjectile = 1;
        private Vector2 vec2ToRotate;
        private bool rotateByRandom;
        public float SpreadModify { get => SpreadModify1; set => SpreadModify1 = value; }
        public float NumOfProjectile { get => numOfProjectile; set => numOfProjectile = value; }
        public Vector2 Vec2ToRotate { get => vec2ToRotate; set => vec2ToRotate = value; }
        public bool RotateByRandom { get => rotateByRandom; set => rotateByRandom = value; }

        private float spreadModify = 1;
        public float SpreadModify1 { get => spreadModify; set => spreadModify = value; }
    }
    public abstract class WeaponTemplate : ModItem
    {
        protected WeaponDataStoreValue WeaponData;
        public float ModifySpread(float TakeFloat) => WeaponData.SpreadModify <= 0 ? 0 : TakeFloat += WeaponData.SpreadModify;

        public Vector2 RotateRandom(float ToRadians)
        {
            float rotation = MathHelper.ToRadians(ModifySpread(ToRadians));
            return WeaponData.Vec2ToRotate.RotatedByRandom(rotation);
        }

        public Vector2 RotateCode(float ToRadians, float time = 0)
        {
            float rotation = MathHelper.ToRadians(ModifySpread(ToRadians));
            if (WeaponData.NumOfProjectile > 1)
            {
                return WeaponData.Vec2ToRotate.RotatedBy(MathHelper.Lerp(rotation / 2f, -rotation / 2f, time / (WeaponData.NumOfProjectile - 1f)));
            }
            return WeaponData.Vec2ToRotate;
        }

        public Vector2 RandomSpread(Vector2 ToRotateAgain, int Spread, float additionalMultiplier = 1)
        {
            ToRotateAgain.X += (Main.rand.Next(-Spread, Spread) * additionalMultiplier) * ModifySpread(1);
            ToRotateAgain.Y += (Main.rand.Next(-Spread, Spread) * additionalMultiplier) * ModifySpread(1);
            return ToRotateAgain;
        }
    }

    abstract class GlobalWeaponModify : GlobalItem
    {
        protected WeaponDataStoreValue WeaponData;
        public float ModifiedProjAmount(float NumAmount)
        {
            return NumAmount += 5;
        }
        public float ModifySpread(float TakeFloat) => WeaponData.SpreadModify <= 0 ? 0 : TakeFloat += WeaponData.SpreadModify;

        public Vector2 RotateRandom(float ToRadians)
        {
            float rotation = MathHelper.ToRadians(ModifySpread(ToRadians));
            return WeaponData.Vec2ToRotate.RotatedByRandom(rotation);
        }

        public Vector2 RotateCode(float ToRadians, float time = 0)
        {
            float rotation = MathHelper.ToRadians(ModifySpread(ToRadians));
            if (WeaponData.NumOfProjectile > 1)
            {
                return WeaponData.Vec2ToRotate.RotatedBy(MathHelper.Lerp(rotation / 2f, -rotation / 2f, time / (WeaponData.NumOfProjectile - 1f)));
            }
            return WeaponData.Vec2ToRotate;
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

        public Vector2 RandomSpread(Vector2 ToRotateAgain, float Spread, float additionalMultiplier = 1)
        {
            ToRotateAgain.X += (Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier) * ModifySpread(1);
            ToRotateAgain.Y += (Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier) * ModifySpread(1);
            return ToRotateAgain;
        }

        public void GlobalRandomSpreadFiring(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockback, float SpreadAmount = 0, float AdditionalSpread = 1, float AdditionalMultiplier = 1)
        {
            for (int i = 0; i < ModifiedProjAmount(WeaponData.NumOfProjectile); i++)
            {
                Projectile.NewProjectile(source, position, RandomSpread(RotateRandom(SpreadAmount), AdditionalSpread, AdditionalMultiplier), type, damage, knockback, player.whoAmI);
            }
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.type == ItemID.RedRyder && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 20);
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 4);
                return false;
            }
            if (item.type == ItemID.Minishark && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 10);
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 10);
                return false;
            }
            if (item.type == ItemID.Gatligator && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 20);
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 35, 10);
                return false;
            }
            if (item.type == ItemID.Handgun && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 10);
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 15);
                return false;
            }
            if (item.type == ItemID.PhoenixBlaster && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 10);
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 10);
                return false;
            }
            if (item.type == ItemID.Musket && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 20);
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 5);
                return false;
            }
            if (item.type == ItemID.TheUndertaker && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 5);
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 12);
                return false;
            }
            if (item.type == ItemID.FlintlockPistol && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 5);
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 25);
                return false;
            }
            if (item.type == ItemID.Revolver && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 5);
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 15);
                return false;
            }
            if (item.type == ItemID.ClockworkAssaultRifle && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 15);
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 19);
                return false;
            }
            if (item.type == ItemID.Megashark && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 25);
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 8);
                return false;
            }
            if (item.type == ItemID.Uzi && AppliesToEntity(item, false))
            {
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 14);
                return false;
            }
            if (item.type == ItemID.VenusMagnum && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 25);
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 14);
                return false;
            }
            if (item.type == ItemID.SniperRifle && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 35);
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 2);
                return false;
            }
            if (item.type == ItemID.ChainGun && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 35);
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 33);
                return false;
            }
            if (item.type == ItemID.VortexBeater && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 35);
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 20);
                return false;
            }
            if (item.type == ItemID.SDMG && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 35);
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 4);
                return false;
            }
            if (item.type == ItemID.Boomstick && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 25);
                WeaponData.NumOfProjectile += Main.rand.Next(2, 5);
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 18, 35, .04f);
                return false;
            }
            if (item.type == ItemID.QuadBarrelShotgun && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 25);
                WeaponData.NumOfProjectile += 5;
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 65);
                return false;
            }
            if (item.type == ItemID.Shotgun && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 35);
                WeaponData.NumOfProjectile += Main.rand.Next(2, 5);
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 30, 10, .5f);
                return false;
            }
            if (item.type == ItemID.OnyxBlaster && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 35);
                WeaponData.NumOfProjectile += 3;
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 15, 5, .5f);
                Projectile.NewProjectile(source, position, velocity, ProjectileID.BlackBolt, damage * 2, knockback, player.whoAmI);
                return false;
            }
            if (item.type == ItemID.TacticalShotgun && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 35);
                WeaponData.NumOfProjectile += 5;
                GlobalRandomSpreadFiring(player, source, position, type, damage, knockback, 18, 3, .076f);
                return false;
            }
            return true;
        }
    }
}