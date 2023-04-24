using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class EmeraldBow : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("well at least it don't consume arrow");
        }
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(18, 32, 17, 1, 28, 28, ItemUseStyleID.Shoot, ModContent.ProjectileType<EmeraldBolt>(), 6f, true);

            Item.rare = 2;
            Item.mana = 12;
            Item.value = Item.buyPrice(gold: 50);
            Item.UseSound = SoundID.Item75;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 CircularRan = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
                CircularRan += Main.rand.NextVector2Circular(3, 3);
                Dust.NewDustPerfect(position, DustID.GemEmerald, CircularRan, 100, default, 0.5f);
            }
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<EmeraldBolt>(), damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TungstenBow)
                .AddIngredient(ItemID.EmeraldStaff)
                .Register();
        }
    }
}
