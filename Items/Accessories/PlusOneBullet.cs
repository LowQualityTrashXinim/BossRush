using BossRush.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Accessories
{
    internal class PlusOneBullet : ModItem, ISynergyItem
    {
        public override string Texture => ItemTexture.MISSINGTEXTURE;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Extra gun Barrel");
            Tooltip.SetDefault("\"why have more gun when you can tape extra barrel into your gun ?\"" +
                "\nIncrease amount of bullet you shoot by 1" +
                "\nIncrease weapon spread amount by 35%");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 28;
            Item.rare = 2;
            Item.value = 1000000;
        }
        public override void UpdateEquip(Player player)
        {
            RangeWeaponOverhaul.NumOfProModify += 1;
            RangeWeaponOverhaul.SpreadModify += .35f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Minishark)
                .AddIngredient(ItemID.CloudinaBottle)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
}
