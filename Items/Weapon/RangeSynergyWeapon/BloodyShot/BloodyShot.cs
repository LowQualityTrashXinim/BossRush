using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.BloodyShot
{
    internal class BloodyShot : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("shot can bleed ? what next ? you gonna tell me that bone can bleed ?");
        }
        public override void SetDefaults()
        {
            Item.scale = 0.7f;
            Item.width = 42;
            Item.height = 36;

            Item.damage = 47;
            Item.knockBack = 1f;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.shootSpeed = 5;

            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<BloodBullet>();
            Item.useAmmo = AmmoID.Bullet;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Ranged;
            Item.rare = 3;
            Item.value = Item.buyPrice(gold: 50);

            Item.UseSound = SoundID.Item11;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<BloodBullet>(), damage, knockback, player.whoAmI);
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(4, 2);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.Handgun)
                .AddIngredient(ItemID.BloodRainBow)
                .Register();
        }
    }
}
