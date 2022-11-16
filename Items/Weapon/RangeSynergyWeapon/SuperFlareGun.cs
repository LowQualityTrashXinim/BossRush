using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.RangeSynergyWeapon
{
    internal class SuperFlareGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The better flare gun");
        }
        public override void SetDefaults()
        {
            Item.width = 68;
            Item.height = 38;
            Item.rare = 4;

            Item.damage = 20;
            Item.crit = 5;
            Item.knockBack = 2f;

            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.useAmmo = AmmoID.Flare;
            Item.scale = 0.75f;

            Item.shoot = ModContent.ProjectileType<SuperFlareP>();
            Item.shootSpeed = 20;
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
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
}
