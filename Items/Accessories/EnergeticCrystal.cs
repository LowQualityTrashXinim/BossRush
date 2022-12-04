using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Accessories
{
    class EnergeticCrystal : ModItem, ISynergyItem
    {
        public override string Texture => "BossRush/MissingTexture";
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Do not take it out of context\nIncrease both health and mana by 50\nIncrease regen mana and health rate by 5");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 28;
            Item.rare = 2;
            Item.value = 1000000;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 50;
            player.statManaMax2 += 50;
            player.lifeRegen += 5;
            player.lifeRegenTime += 5;
            player.lifeRegenCount += 5;
            player.manaRegen += 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
           .AddIngredient(ModContent.ItemType<NatureCrystal>(), 1)
           .AddIngredient(ItemID.ManaRegenerationBand, 1)
           .Register();
        }
    }
}
