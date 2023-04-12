using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.MFrozenEnchantedSword
{
    public class FrozenEnchantedSword : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("prove to work despite it is just a ice blade on top of enchanted sword in photoshop");
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.useTurn = true;

            Item.width = 34;
            Item.height = 40;

            Item.damage = 29;
            Item.knockBack = 5f;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.rare = 3;
            Item.useStyle = 1;

            Item.shoot = ProjectileID.EnchantedBeam;
            Item.shootSpeed = 15;
            Item.value = Item.buyPrice(gold: 50);

            Item.UseSound = SoundID.Item1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ProjectileID.IceBolt, damage, knockback, player.whoAmI);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.EnchantedSword)
                .AddIngredient(ItemID.IceBlade)
                .Register();
        }
    }
}
