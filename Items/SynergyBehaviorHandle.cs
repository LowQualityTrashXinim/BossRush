using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Items.Weapon.RangeSynergyWeapon.IceStorm;

namespace BossRush.Items
{
    class BaseSynergyHandleItem : GlobalItem
    {
        public virtual bool PreHoldItem(Item item, Player player)
        {
            return true;
        }
        public virtual void NormalHoldItem(Item item, Player player)
        {

        }
        public virtual void PostHoldItem(Item item, Player player)
        {

        }
        public override void HoldItem(Item item, Player player)
        {
            if (PreHoldItem(item, player))
            {
                NormalHoldItem(item, player);
            }
            PostHoldItem(item, player);
        }
        public virtual bool PreShoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }
        public virtual void ShootNormal(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

        }
        public virtual void PostShoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (PreShoot(item, player, source, position, velocity, type, damage, knockback)) ;
            {
                ShootNormal(item, player, source, position, velocity, type, damage, knockback);
            }
            PostShoot(item, player, source, position, velocity, type, damage, knockback);
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
    class SynergyBehaviorHandleItem : BaseSynergyHandleItem
    {
        public override void ShootNormal(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.type == ModContent.ItemType<IceStorm>())
            {
                IceStormSynergy(player, source, position, velocity, damage, knockback);
            }
        }
        public override void NormalHoldItem(Item item, Player player)
        {
            if (item.type == ModContent.ItemType<IceStorm>())
            {

                if (player.HasItem(ItemID.SnowballCannon))
                {
                    if (player.ownedProjectileCounts[ModContent.ProjectileType<IceStormSnowBallCannonMinion>()] < 1)
                    {
                        Projectile.NewProjectile(
                            item.GetSource_FromThis(),
                            player.Center,
                            Vector2.Zero,
                            ModContent.ProjectileType<IceStormSnowBallCannonMinion>(),
                            player.GetWeaponDamage(item),
                            player.GetWeaponKnockback(item),
                            player.whoAmI);
                    }
                }
                if (player.HasItem(ItemID.FlowerofFrost))
                {
                    if (player.ownedProjectileCounts[ModContent.ProjectileType<IceStormFrostFlowerMinion>()] < 1)
                    {
                        Projectile.NewProjectile(
                            item.GetSource_FromThis(),
                            player.Center,
                            Vector2.Zero,
                            ModContent.ProjectileType<IceStormFrostFlowerMinion>(),
                            player.GetWeaponDamage(item),
                            player.GetWeaponKnockback(item),
                            player.whoAmI);
                    }
                }
            }
        }
        private void HasSnowBall(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
        {
            int projectile4 = (int)(player.GetModPlayer<IceStormPlayer>().SpeedMultiplier * .5f);
            for (int i = 0; i < projectile4; i++)
            {
                float ToRa = 0;
                if (projectile4 != 1)
                {
                    ToRa = projectile4 * 7;
                }
                Projectile.NewProjectile(source, position, velocity.RotateRandom(ToRa).RandomSpread(7) * 1.5f, ProjectileID.SnowBallFriendly, damage, knockback, player.whoAmI);
            }
        }
        private void HasFrostFlower(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
        {
            int projectile5 = (int)(player.GetModPlayer<IceStormPlayer>().SpeedMultiplier * .1666667f);
            for (int i = 0; i < projectile5; i++)
            {
                float ToRa = 0;
                if (projectile5 != 1)
                {
                    ToRa = projectile5 * 5;
                }
                Projectile.NewProjectile(source, position, velocity.RotateRandom(ToRa).RandomSpread(12), ProjectileID.BallofFrost, damage, knockback, player.whoAmI);
            }
        }
        private void HasBlizzardStaff(Player player, EntitySource_ItemUse_WithAmmo source, int damage, float knockback)
        {
            if (player.GetModPlayer<IceStormPlayer>().SpeedMultiplier < 8)
            {
                return;
            }
            Vector2 SkyPos = new Vector2(player.Center.X, player.Center.Y - 800);
            Vector2 SkyVelocity = (Main.MouseWorld - SkyPos).SafeNormalize(Vector2.UnitX);
            for (int i = 0; i < 5; i++)
            {
                SkyPos += Main.rand.NextVector2Circular(200, 200);
                int FinalCharge = Projectile.NewProjectile(source, SkyPos, SkyVelocity * 30, ProjectileID.Blizzard, damage, knockback, player.whoAmI);
                Main.projectile[FinalCharge].tileCollide = false;
                Main.projectile[FinalCharge].timeLeft = 100;
            }
        }
        private void IceStormSynergy(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
        {
            if (player.HasItem(ItemID.SnowballCannon))
            {
                HasSnowBall(player, source, position, velocity, damage, knockback);
            }
            if (player.HasItem(ItemID.FlowerofFrost))
            {
                HasFrostFlower(player, source, position, velocity, damage, knockback);
            }
            if (player.HasItem(ItemID.BlizzardStaff))
            {
                HasBlizzardStaff(player, source, damage, knockback);
            }
        }
    }

    class SynergyBehaviorHandlePlayer : ModPlayer
    {

    }
}
