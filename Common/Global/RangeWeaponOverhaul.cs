using Terraria;
using System.IO;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace BossRush.Common.Global
{
    //public readonly static int[] GunType = {
    //    ItemID.RedRyder,
    //    ItemID.Minishark,
    //    ItemID.Gatligator,
    //    ItemID.Handgun,
    //    ItemID.PhoenixBlaster,
    //    ItemID.Musket,
    //    ItemID.TheUndertaker,
    //    ItemID.FlintlockPistol,
    //    ItemID.Revolver,
    //    ItemID.ClockworkAssaultRifle,
    //    ItemID.Megashark,
    //    ItemID.Uzi,
    //    ItemID.VenusMagnum,
    //    ItemID.SniperRifle,
    //    ItemID.ChainGun,
    //    ItemID.SDMG,
    //    ItemID.Boomstick,
    //    ItemID.QuadBarrelShotgun,
    //    ItemID.Shotgun,
    //    ItemID.OnyxBlaster,
    //    ItemID.TacticalShotgun
    //};
    public class GlobalWeaponModify : GlobalItem
    {
        public override bool InstancePerEntity => true;
        float OffSetPost = 0;
        float SpreadAmount = 0;
        float AdditionalSpread = 0;
        float AdditionalMulti = 1;
        int NumOfProjectile = 1;
        bool itemIsAShotgun = false;
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
                    SpreadAmount = 5;
                    AdditionalSpread = 2;
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
                    itemIsAShotgun = true;
                    break;
                case ItemID.QuadBarrelShotgun:
                    OffSetPost = 25;
                    SpreadAmount = 45;
                    AdditionalSpread = 6;
                    NumOfProjectile += 6;
                    itemIsAShotgun = true;
                    break;
                case ItemID.Shotgun:
                    OffSetPost = 35;
                    SpreadAmount = 24;
                    AdditionalSpread = 6;
                    AdditionalMulti = .5f;
                    NumOfProjectile += Main.rand.Next(4, 6);
                    itemIsAShotgun = true;
                    break;
                case ItemID.OnyxBlaster:
                    OffSetPost = 35;
                    SpreadAmount = 15;
                    AdditionalSpread = 6;
                    NumOfProjectile += Main.rand.Next(4, 6);
                    itemIsAShotgun = true;
                    break;
                case ItemID.TacticalShotgun:
                    OffSetPost = 35;
                    SpreadAmount = 18;
                    AdditionalSpread = 3;
                    AdditionalMulti = .76f;
                    NumOfProjectile += 6;
                    itemIsAShotgun = true;
                    break;
            }
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul)
            {
                return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
            }
            RangerOverhaulPlayer modplayer = player.GetModPlayer<RangerOverhaulPlayer>();
            int amount = NumOfProjectile + modplayer.ProjectileAmountModify;
            if (itemIsAShotgun)
            {
                return true;
            }
            if (amount >= 2)
            {
                amount--;
                for (int i = 0; i < amount; i++)
                {
                    Vector2 velocity2 = velocity = velocity.Vector2RotateByRandom(SpreadAmount * modplayer.SpreadModify).Vector2RandomSpread(AdditionalSpread * modplayer.SpreadModify, AdditionalMulti * modplayer.SpreadModify);
                    Projectile.NewProjectile(new EntitySource_ItemUse_WithAmmo(player, item, item.ammo), position, velocity2, type, damage, knockback, player.whoAmI);
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
            position = position.PositionOFFSET(velocity, OffSetPost);
            if (!itemIsAShotgun)
            {
                velocity = velocity.Vector2RotateByRandom(SpreadAmount * modplayer.SpreadModify).Vector2RandomSpread(AdditionalSpread * modplayer.SpreadModify, AdditionalMulti * modplayer.SpreadModify);
            }
        }
    }
    /// <summary>
    /// This will auto handle the value for you, but if you want to shoot custom projectile<br/>
    /// you must to it in <see cref="ModItem.Shoot(Player, EntitySource_ItemUse_WithAmmo, Vector2, Vector2, int, int, float)"/>
    /// </summary>
    public interface IBossRushRangeGun
    {
        public float OffSetPosition { get; }
        public int NumOfProjectile { get; }
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
            SpreadModify = 1;
            ProjectileAmountModify = 0;
            base.ResetEffects();
        }
        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            return;
            if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul)
            {
                return;
            }
            if (item is IBossRushRangeGun brItem)
            {
                position = position.PositionOFFSET(velocity, brItem.OffSetPosition);
                if (brItem.NumOfProjectile == 1)
                {
                    velocity = velocity.NextVector2RotatedByRandom(BaseSpreadModifier * SpreadModify);
                }
            }
        }
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
            if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul)
            {
                return base.Shoot(item, source, position, velocity, type, damage, knockback);
            }
            if (item is IBossRushRangeGun brItem)
            {
                if (brItem.NumOfProjectile > 1)
                {
                    for (int i = 0; i < brItem.NumOfProjectile; i++)
                    {
                        Vector2 velocity2 = velocity.NextVector2RotatedByRandom(BaseSpreadModifier * SpreadModify);
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
            BaseSpreadModifier = (float)tag["BaseSpreadModifier"];
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
            if (BaseSpreadModifier != clone.BaseSpreadModifier || BaseProjectileAmountModifier != clone.BaseProjectileAmountModifier)
                SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
        }
    }
}