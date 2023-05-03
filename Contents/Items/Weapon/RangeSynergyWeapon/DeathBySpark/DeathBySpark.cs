using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.DeathBySpark
{
    internal class DeathBySpark : ModItem, ISynergyItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(34, 24, 15, 1f, 84, 84, ItemUseStyleID.Shoot, ModContent.ProjectileType<SparkFlare>(), 12, false, AmmoID.Flare);
            Item.rare = 3;
            Item.UseSound = SoundID.Item11;
            Item.value = Item.buyPrice(gold: 50);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<SparkFlare>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FlareGun)
                .AddIngredient(ItemID.WandofSparking)
                .Register();
        }
    }
}