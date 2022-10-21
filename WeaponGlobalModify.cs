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
        private Vector2 vec2ToRotate;
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

        public float ModifiedProjAmount(float NumAmount)
        {
            return NumAmount += 2;
        }
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

        public Vector2 RandomSpread(Vector2 ToRotateAgain, float Spread, float additionalMultiplier = 1)
        {
            ToRotateAgain.X += (Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier) * ModifySpread(1);
            ToRotateAgain.Y += (Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier) * ModifySpread(1);
            return ToRotateAgain;
        }

        public void GlobalRandomSpreadFiring(Player player, EntitySource_ItemUse_WithAmmo source,ref Vector2 position,ref Vector2 velocity,ref int type,ref int damage,ref float knockback, float SpreadAmount = 0, float AdditionalSpread = 0, float AdditionalMultiplier = 1, bool ItemISaShotgun = false)
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
            var source = new EntitySource_ItemUse_WithAmmo(player,item,item.ammo);
            if (item.type == ItemID.RedRyder && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 20);
                GlobalRandomSpreadFiring(player, source, ref position,ref velocity, ref type, ref damage, ref knockback, 6);

            }
            if (item.type == ItemID.Minishark && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 10);
                GlobalRandomSpreadFiring(player, source, ref position,ref velocity, ref type, ref damage, ref knockback, 10);

            }
            if (item.type == ItemID.Gatligator && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 20);
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 35, 10);

            }
            if (item.type == ItemID.Handgun && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 10);
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 15);

            }
            if (item.type == ItemID.PhoenixBlaster && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 10);
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 10);

            }
            if (item.type == ItemID.Musket && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 20);
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 5);

            }
            if (item.type == ItemID.TheUndertaker && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 5);
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 12);

            }
            if (item.type == ItemID.FlintlockPistol && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 5);
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 25);

            }
            if (item.type == ItemID.Revolver && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 5);
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 15);

            }
            if (item.type == ItemID.ClockworkAssaultRifle && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 15);
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 19);

            }
            if (item.type == ItemID.Megashark && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 25);
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 8);

            }
            if (item.type == ItemID.Uzi && AppliesToEntity(item, false))
            {
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 14);

            }
            if (item.type == ItemID.VenusMagnum && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 25);
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 14);

            }
            if (item.type == ItemID.SniperRifle && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 35);
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 2);

            }
            if (item.type == ItemID.ChainGun && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 35);
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 33);

            }
            if (item.type == ItemID.VortexBeater && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 35);
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 20);

            }
            if (item.type == ItemID.SDMG && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 35);
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 4);

            }
            if (item.type == ItemID.Boomstick && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 25);
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 18, 35, .04f, true);

            }
            if (item.type == ItemID.QuadBarrelShotgun && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 25);
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 65,default, default,true);

            }
            if (item.type == ItemID.Shotgun && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 35);
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 30, 10, .5f,true);

            }
            if (item.type == ItemID.OnyxBlaster && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 35);
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 15,default,default,true);

            }
            if (item.type == ItemID.TacticalShotgun && AppliesToEntity(item, false))
            {
                position = PositionOFFSET(position, velocity, 35);
                GlobalRandomSpreadFiring(player, source,ref position,ref velocity, ref type, ref damage, ref knockback, 18, 3, .076f,true);

            }
        }
    }
}