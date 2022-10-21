using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Weapon.MeleeSynergyWeapon.SuperEnchantedSword
{
    internal class EnchantedLongSword : WeaponTemplate
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Long Enchanted Sword like in zelda");
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 62;

            Item.damage = 25;
            Item.knockBack = 5f;
            Item.useTime = 30;
            Item.useAnimation = 15;

            Item.shoot = ProjectileID.EnchantedBeam;
            Item.shootSpeed = 15;

            Item.rare = 3;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(gold: 50);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            NumOfProjectile = 5;
            for (int i = 0; i < NumOfProjectile; i++)
            {
                Vector2 rotate = RotateCode(20, i);
                Projectile.NewProjectile(source, position, rotate, ProjectileID.EnchantedBeam, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.EnchantedSword, 2)
                .Register();
        }
    }
}
