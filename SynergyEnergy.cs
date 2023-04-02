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
                "\nIncrease speed by 10%" +
                "\nIncrease jump speed by 10%" +
                "\nIncrease damage flat by 5" +
                "\nIncrease max mana by 20" +
                "\nIncrease health regen by 1");
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
            player.accRunSpeed += 0.1f;
            player.jumpSpeedBoost += .1f;
            player.statManaMax2 += 20;
            player.lifeRegen += 1;
            player.GetDamage(DamageClass.Generic).Flat += 5f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("Synergy Item")
                .Register();
        }
    }
}
