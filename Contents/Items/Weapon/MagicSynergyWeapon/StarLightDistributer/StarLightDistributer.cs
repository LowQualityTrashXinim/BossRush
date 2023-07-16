using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.StarLightDistributer
{
    internal class StarLightDistributer : SynergyModItem, ISynergyItem
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
            {
                tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.MeteorHelmet}][i:{ItemID.MeteorSuit}][i:{ItemID.MeteorLeggings}]Attack now cost 0 mana"));
            }
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = position.PositionOFFSET(velocity, 30);
            float num = 5;
            if (Main.dayTime)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.ThunderStaffShot, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ProjectileID.GreenLaser, damage, knockback, player.whoAmI);
                if (player.ZoneNormalSpace)
                {
                    for (int i = 0; i < num; i++)
                    {
                        Vector2 EvenSpread = velocity.Vector2DistributeEvenly(num, 30, i);
                        Projectile.NewProjectile(source, position, EvenSpread, ProjectileID.GreenLaser, (int)(damage * 1.25f), knockback, player.whoAmI);
                    }
                }
                return false;
            }
            if (player.ZoneNormalSpace)
            {
                num += 5;
                for (int i = 0; i < num; i++)
                {
                    Vector2 EvenSpread = velocity.Vector2DistributeEvenly(num, 30, i);
                    int proj = Projectile.NewProjectile(source, position, EvenSpread, ProjectileID.GreenLaser, (int)(damage * 1.25f), knockback, player.whoAmI);
                    Main.projectile[proj].usesLocalNPCImmunity = true;
                }
            }
            Projectile.NewProjectile(source, position, velocity, ProjectileID.GreenLaser, damage, knockback, player.whoAmI);
            for (int i = 0; i < num; i++)
            {
                Vector2 EvenSpread = velocity.Vector2DistributeEvenly(num, 30, i);
                Projectile.NewProjectile(source, position, EvenSpread, ProjectileID.ThunderStaffShot, (int)(damage * 1.25f), knockback, player.whoAmI);
            }
            return false;
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