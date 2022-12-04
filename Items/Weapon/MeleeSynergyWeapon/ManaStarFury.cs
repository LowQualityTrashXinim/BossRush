using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon
{
    internal class ManaStarFury : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("don't actually give you more mana or consume mana when use");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.useTurn = true;

            Item.width = 34;
            Item.height = 40;

            Item.damage = 35;
            Item.knockBack = 5f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.rare = 3;
            Item.useStyle = 1;

            Item.shoot = ProjectileID.Starfury;
            Item.shootSpeed = 15;
            Item.value = Item.buyPrice(gold: 50);

            Item.UseSound = SoundID.Item1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 SkyPos = new Vector2(Main.MouseWorld.X + Main.rand.Next(-200, 200), Main.MouseWorld.Y - 800 + Main.rand.Next(-300, 100));
            Vector2 Aimto = Main.MouseWorld - SkyPos;
            Vector2 safeAim = Aimto.SafeNormalize(Vector2.UnitX);
            Projectile.NewProjectile(source, SkyPos, safeAim * 20, ProjectileID.Starfury, (int)(damage * 0.45f), knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity, ProjectileID.Starfury, (int)(damage * 0.75f), knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity, ProjectileID.StarCannonStar, damage, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.Starfury)
                .AddIngredient(ItemID.ManaCrystal)
                .AddIngredient(ItemID.BandofStarpower)
                .Register();
        }
    }
}
