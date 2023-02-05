using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.DeathBySpark
{
    internal class DeathBySpark : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("let hope those weird creature isn't present in this game");
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 24;

            Item.damage = 24;
            Item.knockBack = 1f;
            Item.value = Item.buyPrice(gold: 50);
            Item.shootSpeed = 12;
            Item.useTime = 100;
            Item.useAnimation = 100;

            Item.autoReuse = false;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<SparkFlare>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAmmo = AmmoID.Flare;
            Item.rare = 3;
            Item.DamageType = DamageClass.Ranged;

            Item.UseSound = SoundID.Item11;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SparkFlare>(), damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FlareGun)
                .AddIngredient(ItemID.WandofSparking)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
}
