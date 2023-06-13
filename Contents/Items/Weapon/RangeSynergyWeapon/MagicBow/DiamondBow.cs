using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class DiamondBow : ModItem, ISynergyItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(18, 32, 39, 1f, 24, 24, ItemUseStyleID.Shoot, ModContent.ProjectileType<DiamondBolt>(), 10, true);
            Item.crit = 10;
            Item.mana = 12;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(gold: 50);
            Item.UseSound = SoundID.Item75;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 CircularRan = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(20)) + Main.rand.NextVector2Circular(3, 3);
                Dust.NewDustPerfect(position, DustID.GemDiamond, CircularRan, 100, default, 0.5f);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PlatinumBow)
                .AddIngredient(ItemID.DiamondStaff)
                .Register();
        }
    }
}