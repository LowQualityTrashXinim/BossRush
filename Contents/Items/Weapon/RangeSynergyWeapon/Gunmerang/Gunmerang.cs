using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Gunmerang
{
    internal class Gunmerang : ModItem, ISynergyItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(34, 26, 34, 1f, 20, 20, ItemUseStyleID.Shoot, ProjectileID.WoodenBoomerang, 14, true);
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(platinum: 5);
            Item.UseSound = SoundID.Item11;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            int BoomerangCount = 0;
            if (player.HasItem(ItemID.EnchantedBoomerang))
            {
                tooltips.Add(new TooltipLine(Mod, "EnchantedBoomerang", $"[i:{ItemID.EnchantedBoomerang}]" + " Shoot out EnchantedBoomerang after 3 shots"));
                BoomerangCount++;
            }
            if (player.HasItem(ItemID.Shroomerang))
            {
                tooltips.Add(new TooltipLine(Mod, "Shroomerang", $"[i:{ItemID.Shroomerang}]" + " Shoot out Flamarang after 5 shots"));
                BoomerangCount++;
            }
            if (player.HasItem(ItemID.IceBoomerang))
            {
                tooltips.Add(new TooltipLine(Mod, "IceBoomerang", $"[i:{ItemID.IceBoomerang}]" + " Shoot out Ice Boomerang after 7 shots"));
                BoomerangCount++;
            }
            if (player.HasItem(ItemID.Flamarang))
            {
                tooltips.Add(new TooltipLine(Mod, "Flamarang", $"[i:{ItemID.Flamarang}]" + " Shoot out Flamarang after 10 shots"));
                BoomerangCount++;
            }
            if (BoomerangCount >= 2)
            {
                tooltips.Add(new TooltipLine(Mod, "BoomerangCount", "Your shot is set into array and the array reset after each 30 shots"));
            }
        }
        int count = 0;
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ProjectileID.WoodenBoomerang;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            count++;
            if (player.HasItem(ItemID.EnchantedBoomerang) && count == 3)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.EnchantedBoomerang, damage, knockback, player.whoAmI);
            }
            else if (player.HasItem(ItemID.Shroomerang) && count == 5)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.Shroomerang, damage, knockback, player.whoAmI);
            }
            else if (player.HasItem(ItemID.IceBoomerang) && count == 7)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.IceBoomerang, damage, knockback, player.whoAmI);
            }
            else if (player.HasItem(ItemID.Flamarang) && count == 10)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.Flamarang, damage, knockback, player.whoAmI);
            }
            if (count >= 10)
            {
                count = 0;
            }
            return true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 1);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WoodenBoomerang)
                .AddIngredient(ItemID.FlintlockPistol)
                .Register();
        }
    }
}
