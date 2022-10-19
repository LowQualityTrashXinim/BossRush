using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Weapon.MeleeSynergyWeapon.EnchantedOreSword
{
    class EnchantedLeadSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = 1;

            Item.height = 50;
            Item.width = 50;

            Item.useTime = 17;
            Item.useAnimation = 17;

            Item.damage = 30;
            Item.knockBack = 5.1f;

            Item.shoot = ModContent.ProjectileType<EnchantedLeadSwordP>();
            Item.shootSpeed = 10f;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(gold: 50);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 RandomCirclePos = position + Main.rand.NextVector2Circular(350f, 350f);
            Vector2 Aimto = (Main.MouseWorld - RandomCirclePos).SafeNormalize(Vector2.UnitX) * Item.shootSpeed;
            Projectile.NewProjectile(source, position, Aimto, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.LeadShortsword)
                .AddIngredient(ItemID.LeadBroadsword)
                .Register();
        }
    }
}
