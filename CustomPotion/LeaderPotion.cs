using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.BuffAndDebuff;

namespace BossRush.CustomPotion
{
	internal class LeaderPotion : ModItem
	{
        public override string Texture => "BossRush/CustomPotion/MissingTexturePotion";
        public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("Lower other ability in exchange to lead more minion");
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
