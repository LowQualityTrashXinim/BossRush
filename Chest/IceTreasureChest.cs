using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Chest
{
    internal class IceTreasureChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 30;
            Item.rare = 5;
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            if (NPC.downedQueenBee)
            {
                int OneRareBeeItem = Main.rand.Next(new int[] { ItemID.BeeCloak, ItemID.QueenBeeBossBag, ItemID.HoneyBalloon, ItemID.SweetheartNecklace, ItemID.WaspGun });
                player.QuickSpawnItem(entitySource, OneRareBeeItem);
            }
            int Accessory = Main.rand.Next(new int[] { ItemID.FlyingCarpet, ItemID.FrogLeg, ItemID.IceSkates, ItemID.ShoeSpikes, ItemID.ClimbingClaws, ItemID.BandofRegeneration, ItemID.BandofStarpower, ItemID.CelestialMagnet, ItemID.NaturesGift, ItemID.FeralClaws, ItemID.ObsidianSkull, ItemID.SharkToothNecklace, ItemID.WhiteString, ItemID.BlackCounterweight, ItemID.FlurryBoots, ItemID.CloudinaBottle, ItemID.Shackle, ItemID.SandstorminaBottle, ItemID.BlizzardinaBottle, ItemID.Flipper, ItemID.AnkletoftheWind, ItemID.BalloonPufferfish, ItemID.TsunamiInABottle, ItemID.LuckyHorseshoe, ItemID.ShinyRedBalloon });
            player.QuickSpawnItem(entitySource, Accessory);
            ChestLootDrop IceChest = new ChestLootDrop(player);
            for (int i = 0; i < 12; i++)
            {
                IceChest.GetWeapon(out int weapon, out int specialAmount);
                IceChest.AmmoForWeapon(out int ammo, out int num, weapon);
                player.QuickSpawnItem(entitySource, weapon, specialAmount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            for (int i = 0; i < 5; i++)
            {
                IceChest.GetPotion(out int potion);
                player.QuickSpawnItem(entitySource, potion, 3);
            }
        }
    }
}
