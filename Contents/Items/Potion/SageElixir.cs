using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.BuffAndDebuff;

namespace BossRush.Contents.Items.Potion
{
    internal class SageElixir : ModItem
    {
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
