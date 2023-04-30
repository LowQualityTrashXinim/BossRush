using Terraria;
using Terraria.ModLoader;
using BossRush.Contents.Items.Chest;

namespace BossRush.Contents.Items.Card
{
    abstract class Card : ModItem
    {   
        public override void SetDefaults()
        {
            Item.BossRushDefaultToConsume(0, 0);
        }
        public override bool? UseItem(Player player)
        {
            PlayerCardHandle modplayer = player.GetModPlayer<PlayerCardHandle>();
            OnUseItem(player, modplayer);
            return base.UseItem(player);
        }
        public virtual void OnUseItem(Player player, PlayerCardHandle modplayer)
        {

        }
    }

    class PlayerCardHandle : ModPlayer
    {
        public ChestLootDropPlayer ChestLoot => Player.GetModPlayer<ChestLootDropPlayer>();
    }
}
