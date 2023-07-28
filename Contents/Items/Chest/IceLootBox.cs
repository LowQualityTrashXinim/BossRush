using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using BossRush.Common.Utils;

namespace BossRush.Contents.Items.Chest
{
    internal class IceLootBox : LootBoxBase
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 30;
            Item.rare = 5;
        }
        public override bool CanLootRNGbeRandomize() => false;
        public override List<int> FlagNumber() => new List<int>() { 0, 1, 2, 3, 4, 5, 6 };
        public override List<int> FlagNumAcc()
        {
            List<int> list = new List<int>() { 0, 1, 2, 3, 4, 5 };
            if (NPC.downedQueenBee)
            {
                list.Add(6);
            }
            if (NPC.downedBoss3)
            {
                list.Add(7);
            }
            return list;
        }
        public override void OnRightClick(Player player, ChestLootDropPlayer modplayer)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            if (NPC.downedQueenBee)
            {
                int OneRareBeeItem = Main.rand.Next(new int[] { ItemID.BeeCloak, ItemID.QueenBeeBossBag, ItemID.HoneyBalloon, ItemID.SweetheartNecklace, ItemID.WaspGun });
                player.QuickSpawnItem(entitySource, OneRareBeeItem);
            }
            if(player.IsDebugPlayer())
            {
                List<int> armor = new List<int>();
                armor.AddRange(TerrariaArrayID.HeadArmorPostEvil);
                armor.AddRange(TerrariaArrayID.HeadArmorPreBoss);
                int[] fullbodyarmor = new int[3];
                fullbodyarmor[0] = Main.rand.Next(armor);
                armor.Clear();
                armor.AddRange(TerrariaArrayID.BodyArmorPostEvil);
                armor.AddRange(TerrariaArrayID.BodyArmorPreBoss);
                fullbodyarmor[1] = Main.rand.Next(armor);
                armor.Clear();
                armor.AddRange(TerrariaArrayID.LegArmorPostEvil);
                armor.AddRange(TerrariaArrayID.LegArmorPostEvil);
                fullbodyarmor[2] = Main.rand.Next(armor);
                for (int i = 0; i < fullbodyarmor.Length; i++)
                {
                    player.QuickSpawnItem(entitySource, fullbodyarmor[i]);
                }
            }
            player.QuickSpawnItem(entitySource, GetAccessory());
            for (int i = 0; i < 5; i++)
            {
                GetWeapon(player, out int weapon, out int specialAmount);
                AmmoForWeapon(out int ammo, out int num, weapon);
                player.QuickSpawnItem(entitySource, weapon, specialAmount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            for (int i = 0; i < 5; i++)
            {
                player.QuickSpawnItem(entitySource, GetPotion(), 3);
            }
        }
    }
}
