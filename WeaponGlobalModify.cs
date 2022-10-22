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
            return NumAmount;
        }
        public float ModifySpread(float TakeFloat) => SpreadModify <= 0 ? 0 : TakeFloat += SpreadModify;

        public Vector2 RotateRandom(float ToRadians)
        {
            float rotation = MathHelper.ToRadians(ModifySpread(ToRadians));
            return Vec2ToRotate.RotatedByRandom(rotation);
        }
        public override bool CanConsumeAmmo(Item weapon, Item ammo, Player player)
        {
            int ChanceNotToConsume = weapon.useTime;
            return !Main.rand.NextBool(ChanceNotToConsume);
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
                switch (item.type)
                {
                    case ItemID.RedRyder:
                        position = PositionOFFSET(position, velocity, 20);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 6);
                        break;
                    case ItemID.Minishark:
                        position = PositionOFFSET(position, velocity, 10);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 10);
                        break;
                    case ItemID.Gatligator:
                        position = PositionOFFSET(position, velocity, 20);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 30, 3);
                        break;
                    case ItemID.Handgun:
                        position = PositionOFFSET(position, velocity, 10);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 15);
                        break;
                    case ItemID.PhoenixBlaster:
                        position = PositionOFFSET(position, velocity, 10);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 10);
                        break;
                    case ItemID.Musket:
                        position = PositionOFFSET(position, velocity, 20);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 5);
                        break;
                    case ItemID.TheUndertaker:
                        position = PositionOFFSET(position, velocity, 5);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 12);
                        break;
                    case ItemID.FlintlockPistol:
                        position = PositionOFFSET(position, velocity, 5);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 25);
                        break;
                    case ItemID.Revolver:
                        position = PositionOFFSET(position, velocity, 5);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 15);
                        break;
                    case ItemID.ClockworkAssaultRifle:
                        position = PositionOFFSET(position, velocity, 15);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 19);
                        break;
                    case ItemID.Megashark:
                        position = PositionOFFSET(position, velocity, 25);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 8);
                        break;
                    case ItemID.Uzi:
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 14);
                        break;
                    case ItemID.VenusMagnum:
                        position = PositionOFFSET(position, velocity, 25);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 14);
                        break;
                    case ItemID.SniperRifle:
                        position = PositionOFFSET(position, velocity, 35);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 2);
                        break;
                    case ItemID.ChainGun:
                        position = PositionOFFSET(position, velocity, 35);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 33);
                        break;
                    case ItemID.VortexBeater:
                        position = PositionOFFSET(position, velocity, 35);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 20);
                        break;
                    case ItemID.SDMG:
                        position = PositionOFFSET(position, velocity, 35);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 4);
                        break;
                    case ItemID.Boomstick:
                        position = PositionOFFSET(position, velocity, 25);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 18, 35, .04f, true);
                        break;
                    case ItemID.QuadBarrelShotgun:
                        position = PositionOFFSET(position, velocity, 25);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 65, default, default, true);
                        break;
                    case ItemID.Shotgun:
                        position = PositionOFFSET(position, velocity, 35);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 30, 10, .5f, true);
                        break;
                    case ItemID.OnyxBlaster:
                        position = PositionOFFSET(position, velocity, 35);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 15, default, default, true);
                        break;
                    case ItemID.TacticalShotgun:
                        position = PositionOFFSET(position, velocity, 35);
                        GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, 18, 3, .076f, true);
                        break;
                }
            }
        }
    }
}