using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using BossRush.Common.Global;
using System;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.StarLightDistributer
{
    internal class StarLightDistributer : SynergyModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultMagic(45, 24, 16, 2f, 16, 16, ItemUseStyleID.Shoot, ProjectileID.GreenLaser, 10, 8, true);
            Item.rare = 3;
            Item.value = Item.buyPrice(gold: 50);
            Item.UseSound = SoundID.Item12;
        }
        public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer)
        {
            if (modplayer.StarLightDistributer_MeteorArmor)
                tooltips.Add(new TooltipLine(Mod, "StarLightDistributer_MeteorArmor", $"[i:{ItemID.MeteorHelmet}][i:{ItemID.MeteorSuit}][i:{ItemID.MeteorLeggings}]Attack now cost 0 mana"));
            if (modplayer.StarLightDistributer_MagicMissile)
                tooltips.Add(new TooltipLine(Mod, "StarLightDistributer_MagicMissile", $"[i:{ItemID.MagicMissile}] Shoot out magic missle"));
        }
        public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (player.HasItem(ItemID.MagicMissile))
                modplayer.StarLightDistributer_MagicMissile = true;
        }
        public override Vector2? HoldoutOffset() => new Vector2(-2, 0);
        public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
        {
            position = position.PositionOFFSET(velocity, 30);
            float num = 1;
            if (Main.dayTime)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.ThunderStaffShot, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ProjectileID.GreenLaser, damage, knockback, player.whoAmI);
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.GreenLaser, damage, knockback, player.whoAmI);
                num = 5;
                for (int i = 0; i < num; i++)
                {
                    Vector2 EvenSpread = velocity.Vector2DistributeEvenly(num, 30, i);
                    Projectile.NewProjectile(source, position, EvenSpread, ProjectileID.ThunderStaffShot, (int)(damage * 1.25f), knockback, player.whoAmI);
                }
            }
            if (player.ZoneNormalSpace)
            {
                num = 5;
                for (int i = 0; i < num; i++)
                {
                    Vector2 EvenSpread = velocity.Vector2DistributeEvenly(num, 45, i);
                    Projectile.NewProjectile(source, position, EvenSpread, ProjectileID.GreenLaser, (int)(damage * 1.25f), knockback, player.whoAmI);
                }
            }
            if (modplayer.StarLightDistributer_MagicMissile)
                for (int i = 0; i < num; i++)
                {
                    Vector2 spread = velocity.Vector2DistributeEvenly(num, 60, i);
                    Projectile.NewProjectile(source, position, spread, ProjectileID.MagicMissile, (int)(damage * 1.5f), knockback, player.whoAmI);
                }
            CanShootItem = false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SpaceGun)
                .AddIngredient(ItemID.ThunderStaff)
                .Register();
        }
    }
}