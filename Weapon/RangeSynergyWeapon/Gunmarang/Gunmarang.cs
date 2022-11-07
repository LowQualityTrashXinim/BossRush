using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace BossRush.Weapon.RangeSynergyWeapon.Gunmarang
{
    internal class Gunmarang : ModItem
    {
        public override string Texture => "BossRush/MissingTexture";
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Weird gun");
        }
        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.shoot = ProjectileID.WoodenBoomerang;
            Item.shootSpeed = 12;

            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.rare = ItemRarityID.Blue;
            Item.noMelee = true;
            Item.value = Item.buyPrice(platinum: 5);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            int BoomerangCount = 0;
            if(player.HeldItem.type == ItemID.IceBoomerang)
            {
                tooltips.Add(new TooltipLine(Mod, "IceBoomerang", $"[i:{ItemID.IceBoomerang}]" + " Shoot out Ice Boomerang for each 5 shots"));
                BoomerangCount++;
            }
            if (player.HeldItem.type == ItemID.Shroomerang)
            {
                tooltips.Add(new TooltipLine(Mod, "Flamarang", $"[i:{ItemID.Shroomerang}]" + " Shoot out Flamarang for each 7 shots"));
                BoomerangCount++;
            }
            if (player.HeldItem.type == ItemID.Flamarang)
            {
                tooltips.Add(new TooltipLine(Mod, "Flamarang", $"[i:{ItemID.Flamarang}]" +" Shoot out Flamarang for each 10 shots"));
                BoomerangCount++;
            }
            if(BoomerangCount >= 2)
            {
                tooltips.Add(new TooltipLine(Mod, "BoomerangCount", "Your shot is set into array and the array reset after each 30 shots"));
            }
        }
        int count = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            count++;
            if (player.HeldItem.type == ItemID.IceBoomerang)
            {
                if(count == 5)
                {
                    Projectile.NewProjectile(source, position, velocity * 2, ProjectileID.IceBoomerang, damage, knockback, player.whoAmI);
                    return false;
                }
            }
            if(player.HeldItem.type == ItemID.Shroomerang)
            {
                if (count == 7)
                {
                    Projectile.NewProjectile(source, position, velocity * 1.5f, ProjectileID.Shroomerang, damage, knockback, player.whoAmI);
                    return false;
                }
            }
            if(player.HeldItem.type == ItemID.Flamarang)
            {
                if (count == 10)
                {
                    Projectile.NewProjectile(source, position, velocity * 2, ProjectileID.Flamarang, damage, knockback, player.whoAmI);
                    return false;
                }
            }
            if(count >= 30)
            {
                count = 0;
            }
            return true;
        }
        public override void AddRecipes()
        {
            base.AddRecipes();
        }
    }
}
