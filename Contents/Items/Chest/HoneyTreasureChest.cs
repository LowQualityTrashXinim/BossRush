using BossRush.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Chest
{
    class HoneyTreasureChest : LootBoxBase
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Good luck !");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 4;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (!NPC.downedBoss3)
            {
                tooltips.Add(new TooltipLine(Mod, "ItemName", $"Locked from being opening, that big head boney coward afraid of yous"));
            }
            else
            {
                tooltips.Add(new TooltipLine(Mod, "ItemName", $"It is now can be open, you free that poor old man from his eternal suffer!"));
            }
        }
        public override bool CanLootRNGbeRandomize() => false;
        public override List<int> FlagNumber() => new List<int>() { 0, 1, 2, 4, 5 };
        public override void OnRightClick(Player player, ChestLootDropPlayer modplayer)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            if(player.IsDebugPlayer())
            {
                GetArmorForPlayer(entitySource, player);
            }
            for (int i = 0; i < 3; i++)
            {
                switch (Main.rand.Next(30))
                {
                    case 0:
                        player.QuickSpawnItem(entitySource, ItemID.BeeCloak);
                        break;
                    case 1:
                        player.QuickSpawnItem(entitySource, ModContent.ItemType<HoneyTreasureChest>());
                        break;
                    case 2:
                        player.QuickSpawnItem(entitySource, ItemID.HoneyedGoggles);
                        break;
                    case 5:
                        player.QuickSpawnItem(entitySource, ItemID.QueenBeeBossBag);
                        break;
                    case 10:
                        player.QuickSpawnItem(entitySource, ItemID.HoneyBalloon);
                        break;
                    case 15:
                        player.QuickSpawnItem(entitySource, ItemID.SweetheartNecklace);
                        break;
                    case 19:
                        player.QuickSpawnItem(entitySource, ItemID.WaspGun);
                        break;
                }
            }
            for (int i = 0; i < 5; i++)
            {
                GetWeapon(player, out int weapon, out int specialAmount);
                AmmoForWeapon(out int ammo, out int num, weapon);
                player.QuickSpawnItem(entitySource, weapon, specialAmount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            player.QuickSpawnItem(entitySource, ItemID.Honeyfin, 10);
            player.QuickSpawnItem(entitySource, GetPotion(), 3);
        }
    }
}
