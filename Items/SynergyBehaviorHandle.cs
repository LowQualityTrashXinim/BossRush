using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.ComponentModel.Design;
using BossRush.Items.Weapon.RangeSynergyWeapon.IceStorm;
using Mono.Cecil;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace BossRush.Items
{
    class BaseSynergyHandleItem : GlobalItem
    {
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
            if(PreShoot(item, player, source, position, velocity, type, damage, knockback));
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
            if(item.type == ModContent.ItemType<IceStorm>())
            {
                IceStormSynergy(player, source, position, velocity, type, damage, knockback);
            }
        }

        private void IceStormSynergy(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.HasItem(ItemID.SnowballCannon))
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
            if (player.HasItem(ItemID.FlowerofFrost))
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
        }
    }

    class SynergyBehaviorHandlePlayer : ModPlayer
    {

    }
}
