using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon
{
    internal class PlatinumKatana : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enhanced Katana");
            Tooltip.SetDefault("The best katana there is, yet");
        }
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 52;

            Item.damage = 43;
            Item.knockBack = 4f;
            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.shoot = ModContent.ProjectileType<KatanaSlash>();
            Item.DamageType = DamageClass.Melee;
            Item.shootSpeed = 3;
            Item.rare = 1;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.value = Item.buyPrice(gold: 50);

            Item.UseSound = SoundID.Item1;
        }
        int count = 0;
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (count % 2 == 0)
            {
                type = ModContent.ProjectileType<KatanaSlash>();
            }
            else
            {
                type = ModContent.ProjectileType<KatanaSlashUpsideDown>();
            }
            count++;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.Katana)
                .AddRecipeGroup("OreBroadSword")
                .Register();
        }
    }
}
