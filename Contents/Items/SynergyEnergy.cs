using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace BossRush.Contents.Items
{
    internal class SynergyEnergy : ModItem
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Red;
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
