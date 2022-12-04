using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon
{
    internal class EnchantedStarfury : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A truely unthinkable");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;

            Item.damage = 32;
            Item.knockBack = 4f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.rare = 4;
            Item.shoot = ProjectileID.EnchantedBeam;
            Item.shootSpeed = 20f;

            Item.width = 66;
            Item.height = 66;
            Item.value = Item.buyPrice(gold: 50);

            Item.UseSound = SoundID.Item1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 SkyPos = new Vector2(Main.MouseWorld.X + Main.rand.Next(-100, 100), Main.MouseWorld.Y - 700);
            Vector2 Aimto = Main.MouseWorld - SkyPos;
            Vector2 safeAim = Aimto.SafeNormalize(Vector2.UnitX);
            Projectile.NewProjectile(source, SkyPos, safeAim * 20, ProjectileID.Starfury, (int)(damage * 1.5f), knockback, player.whoAmI);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.EnchantedSword)
                .AddIngredient(ItemID.Starfury)
                .Register();
        }
    }
}
