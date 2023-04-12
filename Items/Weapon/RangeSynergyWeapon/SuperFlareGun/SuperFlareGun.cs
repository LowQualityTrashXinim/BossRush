using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.SuperFlareGun
{
    internal class SuperFlareGun : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The better flare gun");
        }
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(68, 38, 20, 2f, 20, 20, ItemUseStyleID.Shoot, ModContent.ProjectileType<SuperFlareP>(), 20, false, AmmoID.Flare);
            Item.rare = 4;
            Item.crit = 5;
            Item.scale = 0.75f;
            Item.UseSound = SoundID.Item11;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(3, 0);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<SuperFlareP>();
            Vector2 Offset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 40f;
            if (Collision.CanHit(position, 0, 0, position + Offset, 0, 0))
            {
                position += Offset;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FlareGun)
                .AddIngredient(ItemID.MoltenFury)
                .Register();
        }
    }
}
