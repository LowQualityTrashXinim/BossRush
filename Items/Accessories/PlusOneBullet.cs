using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Accessories
{
    internal class PlusOneBullet : ModItem, ISynergyItem
    {
        public override string Texture => "BossRush/MissingTexture";
        public int ModifyAmountBullet => 1;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("\"Extra gun barrel\"" +
                "\nIncrease amount of bullet you shoot by 1");
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
            BossRushWeaponSpreadUtils.NumOfProModify += 1;
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
