using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class AmethystBow : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("well at least it don't consume arrow");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 32;

            Item.damage = 15;
            Item.knockBack = 1f;
            Item.mana = 12;

            Item.useTime = 45;
            Item.useAnimation = 45;

            Item.noMelee = true;
            Item.autoReuse = true;
            Item.rare = 2;
            Item.value = Item.buyPrice(gold: 50);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Ranged;

            Item.shoot = ModContent.ProjectileType<AmethystBolt>();
            Item.shootSpeed = 5f;

            Item.UseSound = SoundID.Item75;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 CircularRan = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
                CircularRan.X += Main.rand.Next(-3, 3);
                CircularRan.Y += Main.rand.Next(-3, 3);
                Dust.NewDustPerfect(position, DustID.GemAmethyst, CircularRan, 100, default, 0.5f);
            }
            position -= new Vector2(0, 5);
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AmethystBolt>(), damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CopperBow)
                .AddIngredient(ItemID.AmethystStaff)
                .Register();
        }
    }
}
