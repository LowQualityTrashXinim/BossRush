using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using static Terraria.ModLoader.PlayerDrawLayer;
using System.IO;
using Terraria.ModLoader.IO;

namespace BossRush.Common.Global
{
    /// <summary>
    /// This is for specific gun that deal range damage only
    /// </summary>
    public static class RangeWeaponOverhaulUtils
    {
        public static Vector2 RotateCode(this Vector2 Vec2ToRotate, float NumOfProjectile, float ToRadians, float i = 0)
        {
            float rotation = MathHelper.ToRadians(ToRadians) * .5f;
            if (NumOfProjectile > 1)
            {
                float RotateValue = MathHelper.Lerp(-rotation, rotation, i / NumOfProjectile);
                return Vec2ToRotate.RotatedBy(RotateValue);
            }
            return Vec2ToRotate;
        }
        public static Vector2 RotateRandom(this Vector2 Vec2ToRotate, float ToRadians)
        {
            float rotation = MathHelper.ToRadians(ToRadians);
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
        public static Vector2 RandomSpread(this Vector2 ToRotateAgain, float Spread, float additionalMultiplier = 1)
        {
            ToRotateAgain.X += Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier;
            ToRotateAgain.Y += Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier;
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
    }
    public class GlobalWeaponModify : GlobalItem
    {
        public override bool InstancePerEntity => true;
        float OffSetPost = 0;
        float SpreadAmount = 0;
        float AdditionalSpread = 0;
        float AdditionalMulti = 1;
        int NumOfProjectile = 0;
        public override void SetDefaults(Item entity)
        {
            if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul)
            {
                return;
            }
            switch (entity.type)
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
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul)
            {
                return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
            }
            if(NumOfProjectile > 1)
            {
                for (int i = 0; i < NumOfProjectile; i++)
                {
                    Vector2 velocity2 = velocity.RotateRandom(SpreadAmount).RandomSpread(AdditionalSpread, AdditionalMulti);
                    Projectile.NewProjectile(source, position, velocity2, type, damage, knockback, player.whoAmI);
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
            RangerOverhaulPlayer modplayer = player.GetModPlayer<RangerOverhaulPlayer>();
            SpreadAmount *= modplayer.SpreadModify;
            AdditionalSpread *= modplayer.SpreadModify;
            AdditionalMulti *= modplayer.SpreadModify;
            NumOfProjectile += modplayer.ProjectileAmountModify;
            position = position.PositionOFFSET(velocity, OffSetPost);
            if (NumOfProjectile == 1)
            {
                velocity = velocity.RotateRandom(SpreadAmount).RandomSpread(AdditionalSpread, AdditionalMulti);
            }
        }
    }
    /// <summary>
    /// This will auto handle the value for you, but if you want to shoot custom projectile<br/>
    /// you must to it in <see cref="ModItem.Shoot(Player, EntitySource_ItemUse_WithAmmo, Vector2, Vector2, int, int, float)"/>
    /// </summary>
    public interface IBossRushRangeGun
    {
        public float OffSetPost { get; set; }
        public float SpreadAmount { get; set; }
        public float AdditionalSpread { get; set; }
        public float AdditionalMulti { get; set; }
        public int NumOfProjectile { get; set; }
    }
    /// <summary>
    /// This will auto handle the value for you, but if you want to shoot custom projectile<br/>
    /// you must to it in <see cref="ModItem.Shoot(Player, EntitySource_ItemUse_WithAmmo, Vector2, Vector2, int, int, float)"/>
    /// </summary>
    public class RangerOverhaulPlayer : ModPlayer
    {
        /// <summary>
        /// Use this to change spread value of gun type weapon via consumable
        /// </summary>
        public float BaseSpreadModifier = 0;
        /// <summary>
        /// Use this to change amount of projectile to be shoot out
        /// </summary>
        public int BaseProjectileAmountModifier = 0;
        /// <summary>
        /// Use this to change globaly, do not use "=" as that set and is not the correct way to use
        /// </summary>
        public float SpreadModify = 1;
        /// <summary>
        /// Use this to change globaly, do not use "=" as that set and is not the correct way to use
        /// </summary>
        public int ProjectileAmountModify = 0;
        public override void ResetEffects()
        {
            base.ResetEffects();
            SpreadModify = 1 + BaseSpreadModifier;
            ProjectileAmountModify = 0 + BaseProjectileAmountModifier;
        }
        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul)
            {
                return;
            }
            if (item.ModItem is IBossRushRangeGun brItem)
            {
                if (brItem.NumOfProjectile == 1)
                {
                    velocity = velocity.NextVector2RotatedByRandom(brItem.SpreadAmount).Vector2RandomSpread(brItem.AdditionalSpread, brItem.AdditionalMulti);
                }
            }
        }
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul)
            {
                return base.Shoot(item, source, position, velocity, type, damage, knockback);
            }
            if (item.ModItem is IBossRushRangeGun brItem)
            {
                if (brItem.NumOfProjectile > 1)
                {
                    for (int i = 0; i < brItem.NumOfProjectile; i++)
                    {
                        Vector2 velocity2 = velocity.NextVector2RotatedByRandom(brItem.SpreadAmount).Vector2RandomSpread(brItem.AdditionalSpread, brItem.AdditionalMulti);
                        Projectile.NewProjectile(source, position, velocity2, type, damage, knockback, Player.whoAmI);
                    }
                }
            }
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)BossRush.MessageType.RangerOverhaul);
            packet.Write((byte)Player.whoAmI);
            packet.Write(BaseSpreadModifier);
            packet.Write(BaseProjectileAmountModifier);
            packet.Send(toWho, fromWho);
        }
        public override void SaveData(TagCompound tag)
        {
            tag["BaseSpreadModifier"] = BaseSpreadModifier;
            tag["BaseProjectileAmountModifier"] = BaseProjectileAmountModifier;
        }
        public override void LoadData(TagCompound tag)
        {
            BaseSpreadModifier = (int)tag["BaseSpreadModifier"];
            BaseProjectileAmountModifier = (int)tag["BaseProjectileAmountModifier"];
        }
        public void ReceivePlayerSync(BinaryReader reader)
        {
            BaseSpreadModifier = reader.ReadSingle();
            BaseProjectileAmountModifier = reader.ReadInt32();
        }

        public override void CopyClientState(ModPlayer targetCopy)
        {
            RangerOverhaulPlayer clone = (RangerOverhaulPlayer)targetCopy;
            clone.BaseSpreadModifier = BaseSpreadModifier;
            clone.BaseProjectileAmountModifier = BaseProjectileAmountModifier;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            RangerOverhaulPlayer clone = (RangerOverhaulPlayer)clientPlayer;
            if (BaseSpreadModifier != clone.BaseSpreadModifier) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
            if (BaseProjectileAmountModifier != clone.BaseProjectileAmountModifier) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
        }
    }
}