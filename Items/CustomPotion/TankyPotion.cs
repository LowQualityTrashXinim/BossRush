using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.BuffAndDebuff;

namespace BossRush.Items.CustomPotion
{
    internal class TankPotion : ModItem
    {
        public override string Texture => "BossRush/CustomPotion/MissingTexturePotion";
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("You are hard as a rock, but your movement is as sloppy as a rock");
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
            Item.buffType = ModContent.BuffType<Protection>();
            Item.buffTime = 12000;
        }
    }
}
