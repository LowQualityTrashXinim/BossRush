using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Global;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.RectangleShotgun
{
    class RectangleShotgun : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("What the holy of this ?");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.damage = 75;
            Item.knockBack = 4f;
            Item.height = 12;
            Item.width = 74;

            Item.useAmmo = AmmoID.Bullet;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.buyPrice(gold: 50);
            Item.rare = 4;

            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.shoot = ModContent.ProjectileType<RectangleBullet>();
            Item.shootSpeed = 70f;
            Item.reuseDelay = 30;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
            Item.noMelee = true;

            Item.UseSound = SoundID.Item38;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<RectangleBullet>();
            position = position.PositionOFFSET(velocity, 40);
            velocity *= .1f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-19, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Boomstick, 2)
            .Register();
        }
    }
}
