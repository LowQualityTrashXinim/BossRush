using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TMod = Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
namespace BossRush
{
    public class GlobalHandleSystem : ModSystem
    {
        public override void PostUpdateItems()
        {
            BossRushWeaponSpreadUtils.NumOfProModify = 0;
            BossRushWeaponSpreadUtils.SpreadModify = 1;
            BossRushWeaponSpreadUtils.NumOfProjectile = 1;
        }
    }
    public class GlobalWeaponModify : GlobalItem
    {
        /// <summary>
        /// Method that make the item currently in use can be shoot by many amount at a random spread<br/>
        /// It is better to use this method if you want to make your weapon affected by spread
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
        public void GlobalRandomSpreadFiring(
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
            float ProjectileAmount = BossRushWeaponSpreadUtils.ModifiedProjAmount(NumOfProjectile);
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
        public override bool InstancePerEntity => true;
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.type == ItemID.VortexBeater)
            {
                return true;
            }
            if (item.type == ItemID.OnyxBlaster)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.BlackBolt, damage * 3, knockback, player.whoAmI);
            }
            for (int i = 0; i < GunType.Length; i++)
            {
                if (item.type == GunType[i] && AppliesToEntity(item, true))
                {
                    return false;
                }
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
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
                GlobalRandomSpreadFiring(player, source, ref position, ref velocity, ref type, ref damage, ref knockback, NumOfProjectile, SpreadAmount, AdditionalSpread, AdditionalMulti);
            }
        }
    }
}