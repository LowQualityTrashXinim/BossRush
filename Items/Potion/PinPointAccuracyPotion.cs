using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.BuffAndDebuff;
using BossRush.Texture;

namespace BossRush.Items.Potion
{
    internal class PinPointAccuracyPotion : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTUREPOTION;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Sniper's Exilir");
            Tooltip.SetDefault("'Aim Assist is bought separately!'");
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
