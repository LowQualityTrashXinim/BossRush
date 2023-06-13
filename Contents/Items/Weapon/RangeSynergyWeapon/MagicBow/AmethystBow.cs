using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class AmethystBow : ModItem, ISynergyItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(18, 32, 15, 1f, 45, 45, ItemUseStyleID.Shoot, ModContent.ProjectileType<AmethystBolt>(), 5f, true);
            Item.mana = 12;
            Item.rare = 2;
            Item.value = Item.buyPrice(gold: 50);
            Item.UseSound = SoundID.Item75;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 CircularRan = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(20)) + Main.rand.NextVector2Circular(3f, 3f);
                Dust.NewDustPerfect(position, DustID.GemAmethyst, CircularRan, 100, default, 0.5f);
            }
            position -= new Vector2(0, 5);
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