using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.BuffAndDebuff;

namespace BossRush.Items.CustomPotion
{
    internal class BerserkPotion : ModItem
    {
        public override string Texture => "BossRush/MissingTexturePotion";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Berserker's Elixir");
            Tooltip.SetDefault("'Smells like bloodshed...'");
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
            Item.buffType = ModContent.BuffType<BerserkBuff>();
            Item.buffTime = 12000;
        }
    }
}
