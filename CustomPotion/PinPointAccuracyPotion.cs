using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.BuffAndDebuff;

namespace BossRush.CustomPotion
{
	internal class PinPointAccuracyPotion : ModItem
	{
        public override string Texture => "BossRush/CustomPotion/MissingTexturePotion";
        public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("Increase your crits ability\nGetting hit will instantly cancel the buff");
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
			Item.buffType = ModContent.BuffType<GodVision>();
			Item.buffTime = 12000;
		}
	}
}
