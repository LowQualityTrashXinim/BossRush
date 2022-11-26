using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.BuffAndDebuff;

namespace BossRush.Items.CustomPotion
{
    internal class LeaderPotion : ModItem
    {
        public override string Texture => "BossRush/MissingTexturePotion";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Commander's Exilir");
            Tooltip.SetDefault("Increases your max number of minions by 5" +
            "\n50% increased minion attack speed" +
            "\n25% increased whip range"
            "\n90% decreased melee, ranged and magic damage" +
            "\n'A must for CEOs in the making!'");
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
            Item.buffType = ModContent.BuffType<LeaderShip>();
            Item.buffTime = 12000;
        }
    }
}
