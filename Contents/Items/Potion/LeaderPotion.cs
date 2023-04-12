using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Texture;
using BossRush.Contents.BuffAndDebuff;

namespace BossRush.Contents.Items.Potion
{
    internal class LeaderPotion : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTUREPOTION;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Commander's Exilir");
            Tooltip.SetDefault("'A must for CEOs in the making!'");
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
