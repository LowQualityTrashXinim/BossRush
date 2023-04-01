using Terraria;
using Terraria.ModLoader;

namespace BossRush
{
    internal class SynergyEnergy : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("the possibility is only limited to our own mind" +
                "\nDue to how overpowering this energy is, equiping it will" +
                "\nIncrease speed by 15%");
        }
        public override void SetDefaults()
        {
            Item.rare = 10;
            Item.width = 54;
            Item.height = 20;
            Item.material = true;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.accRunSpeed += 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("Synergy Item")
                .Register();
        }
    }
}
