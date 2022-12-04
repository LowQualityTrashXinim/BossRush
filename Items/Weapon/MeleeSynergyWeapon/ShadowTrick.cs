using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon
{
    public class ShadowTrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Let them know the shadow trick that they can't see");
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 72;
            Item.height = 52;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.shootSpeed = 16f;
            Item.knockBack = 2.5f;
            Item.damage = 25;
            Item.rare = 3;

            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = false;

            Item.UseSound = SoundID.Item1;
            Item.value = Item.buyPrice(gold: 50);
            Item.shoot = ModContent.ProjectileType<ShadowTrickP>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CorruptYoyo)
                .AddIngredient(ItemID.BallOHurt)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
}
