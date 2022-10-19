using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Weapon.MeleeSynergyWeapon.Katana
{
    internal class LeadKatana : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Be careful, you could get lead posion");
        }
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 52;

            Item.damage = 26;
            Item.knockBack = 4f;
            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.shoot = ModContent.ProjectileType<LeadSlash>();
            Item.DamageType = DamageClass.Melee;
            Item.shootSpeed = 3;
            Item.rare = 1;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.value = Item.buyPrice(gold: 50);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.direction == 1)
            {
                position += velocity.SafeNormalize(Vector2.UnitX).RotatedBy(MathHelper.ToRadians(-90)) * 50f;
            }
            else
            {
                position += velocity.SafeNormalize(Vector2.UnitX).RotatedBy(MathHelper.ToRadians(90)) * 50f;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.Katana)
                .AddIngredient(ItemID.LeadBroadsword)
                .Register();
        }
    }
}
