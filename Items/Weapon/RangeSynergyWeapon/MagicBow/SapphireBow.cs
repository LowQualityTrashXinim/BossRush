using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class SapphireBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("well at least it don't consume arrow");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 32;

            Item.damage = 25;
            Item.knockBack = 1f;

            Item.useTime = 18;
            Item.useAnimation = 18;

            Item.rare = 2;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Ranged;
            Item.mana = 5;

            Item.shoot = ModContent.ProjectileType<SapphireBolt>();
            Item.shootSpeed = 6f;
            Item.value = Item.buyPrice(gold: 50);

            Item.UseSound = SoundID.Item75;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 CircularRan = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
                CircularRan.X += Main.rand.Next(-3, 3);
                CircularRan.Y += Main.rand.Next(-3, 3);
                Dust.NewDustPerfect(position, DustID.GemSapphire, CircularRan, 100, default, 0.5f);
            }
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SapphireBolt>(), damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.SilverBow)
                .AddIngredient(ItemID.SapphireStaff)
                .Register();
        }
    }
}
