using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Weapon.MeleeSynergyWeapon.Katana
{
    internal class PlatinumKatana : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The best katana there is, yet");
        }
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 52;

            Item.damage = 34;
            Item.knockBack = 4f;
            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.shoot = ModContent.ProjectileType<PlatinumSlash>();
            Item.DamageType = DamageClass.Melee;
            Item.shootSpeed = 5;
            Item.rare = 1;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.value = Item.buyPrice(gold: 50);
        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * -200f;
            position += muzzleOffset;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.Katana)
                .AddIngredient(ItemID.PlatinumBroadsword)
                .Register();
        }
    }
}
