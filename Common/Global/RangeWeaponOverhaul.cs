using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace BossRush.Common.Global
{
    /// <summary>
    /// This is for specific gun that deal range damage only
    /// </summary>
    public static class RangeWeaponOverhaulUtils
    {
        public static Vector2 RotateCode(this Vector2 Vec2ToRotate,RangerOverhaulPlayer modplayer, float ToRadians, float i = 0)
        {
            float modifyradian = modplayer.ModifySpread(ToRadians);
            float rotation = MathHelper.ToRadians(modifyradian) * .5f;
            if (modplayer.NumOfProjectile > 1)
            {
                float RotateValue = MathHelper.Lerp(-rotation, rotation, i / modplayer.NumOfProjectile);
                return Vec2ToRotate.RotatedBy(RotateValue);
            }
            return Vec2ToRotate;
        }
        public static Vector2 RotateRandom(this Vector2 Vec2ToRotate,RangerOverhaulPlayer modplayer, float ToRadians)
        {
            float rotation = MathHelper.ToRadians(modplayer.ModifySpread(ToRadians));
            return Vec2ToRotate.RotatedByRandom(rotation);
        }
        public static Vector2 PositionOFFSET(this Vector2 position, Vector2 ProjectileVelocity, float offSetBy)
        {
            Vector2 OFFSET = ProjectileVelocity.SafeNormalize(Vector2.Zero) * offSetBy;
            if (Collision.CanHitLine(position, 0, 0, position + OFFSET, 0, 0))
            {
                return position += OFFSET;
            }
            return position;
        }
        public static Vector2 IgnoreTilePositionOFFSET(this Vector2 position, Vector2 ProjectileVelocity, float offSetBy)
        {
            Vector2 OFFSET = ProjectileVelocity.SafeNormalize(Vector2.Zero) * offSetBy;
            return position += OFFSET;
        }
        public static Vector2 RandomSpread(this Vector2 ToRotateAgain, RangerOverhaulPlayer modplayer,float Spread, float additionalMultiplier = 1)
        {
            ToRotateAgain.X += Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier * modplayer.ModifySpread(1);
            ToRotateAgain.Y += Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier * modplayer.ModifySpread(1);
            return ToRotateAgain;
        }

        public readonly static int[] GunType = {
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
            RangerOverhaulPlayer modplayer = player.GetModPlayer<RangerOverhaulPlayer>();
            int ProjectileAmount = (int)modplayer.ModifiedProjAmount(NumOfProjectile);
            if (ProjectileAmount == 1)
            {
                velocity = velocity.RotateRandom(SpreadAmount).RandomSpread(modplayer,AdditionalSpread, AdditionalMultiplier);
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                return;
            }
            for (int i = 0; i < ProjectileAmount; i++)
            {
                Vector2 velocity2 = velocity.RotateRandom(SpreadAmount).RandomSpread(modplayer,AdditionalSpread, AdditionalMultiplier);
                Projectile.NewProjectile(source, position, velocity2, type, damage, knockback, player.whoAmI);
            }
        }
    }
    public class GlobalWeaponModify : GlobalItem
    {
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul)
            {
                return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
            }
            if (item.type == ItemID.VortexBeater)
            {
                return true;
            }
            if (item.type == ItemID.OnyxBlaster)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.BlackBolt, damage * 3, knockback, player.whoAmI);
            }
            for (int i = 0; i < RangeWeaponOverhaulUtils.GunType.Length; i++)
            {
                if (item.type == RangeWeaponOverhaulUtils.GunType[i] && AppliesToEntity(item, true))
                {
                    return false;
                }
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul)
            {
                return;
            }
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
                RangerOverhaulPlayer modplayer = player.GetModPlayer<RangerOverhaulPlayer>();
                modplayer.SpreadAmount = SpreadAmount;
                modplayer.AdditionalSpread = AdditionalSpread;
                modplayer.AdditionalMultiSpread = AdditionalMulti;
                modplayer.NumOfProjectile = NumOfProjectile;
                position = position.PositionOFFSET(velocity, OffSetPost);
                RangeWeaponOverhaulUtils.NewGunShotProjectile(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, NumOfProjectile, SpreadAmount, AdditionalSpread, AdditionalMulti);
            }
        }
    }
    public class RangerOverhaulPlayer: ModPlayer
    {
        public float SpreadAmount = 0, AdditionalSpread = 0, AdditionalMultiSpread = 0, NumOfProjectile = 1;
        public float SpreadModify = 1, ProjectileAmountModify = 0;
        public override void ResetEffects()
        {
            base.ResetEffects();
            SpreadAmount = 0;
            AdditionalSpread = 0;
            AdditionalMultiSpread = 0;
            NumOfProjectile = 1;
        }
        public float ModifiedProjAmount(float TakeNumAmount) => ProjectileAmountModify + TakeNumAmount;
        
        public float ModifySpread(float TakeFloat) => SpreadModify <= 0 ? 0 : TakeFloat * SpreadModify;

    }
}