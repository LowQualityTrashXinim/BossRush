using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Weapon.BasicWeapon
{
    internal class FrozenShark : WeaponTemplate
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Functional surprisingly enough");
        }
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 20;

            Item.damage = 10;
            Item.knockBack = 1f;
            Item.shootSpeed = 10;
            Item.useTime = 9;
            Item.useAnimation = 9;

            Item.noMelee = true;
            Item.shoot = ProjectileID.IceBolt;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = 3;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(gold: 50);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vec2ToRotate = velocity;
            position = PositionOFFSET(position, velocity, 50);
            velocity = RotateRandom(5);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2f, -2f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Minishark)
                .AddIngredient(ItemID.IceBlade)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
}
