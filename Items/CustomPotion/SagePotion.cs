using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.BuffAndDebuff;

namespace BossRush.Items.CustomPotion
{
    internal class SagePotion : ModItem
    {
        public override string Texture => "BossRush/MissingTexturePotion";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Sage's Exilir");
            Tooltip.SetDefault("Increases maximum mana by 50" +
            "\n400% increased mana regeneration" +
            "\n45% decreased mana cost" +
            "\n90% decreased melee, ranged and summon damage") +
            "\n'Side effects include becoming an explosive hazard.'";
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = ItemRarityID.Orange;
            Item.buffType = ModContent.BuffType<SageBuff>();
            Item.buffTime = 12000;
        }
    }
}
