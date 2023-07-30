using BossRush.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Chest
{
    class GoldLootBox : LootBoxBase
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Good Luck!");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 5;
        }
        public override List<int> FlagNumber() => new List<int>() { 3, 4, 5, 6 };
        public override List<int> FlagNumAcc()
        {
            List<int> list = new List<int>() { 0, 1, 2, 3, 4, 5, 7 };
            if (NPC.downedQueenBee)
            {
                list.Add(6);
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
            if (player.IsDebugPlayer())
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
            else
            {
                int RandomNumber = Main.rand.Next(3);
                switch (RandomNumber)
                {
                    case 0:
                        player.QuickSpawnItem(entitySource, ItemID.NecroHelmet);
                        player.QuickSpawnItem(entitySource, ItemID.NecroBreastplate);
                        player.QuickSpawnItem(entitySource, ItemID.NecroGreaves);
                        break;
                    case 1:
                        player.QuickSpawnItem(entitySource, ItemID.MeteorHelmet);
                        player.QuickSpawnItem(entitySource, ItemID.MeteorSuit);
                        player.QuickSpawnItem(entitySource, ItemID.MeteorLeggings);
                        break;
                    case 2:
                        player.QuickSpawnItem(entitySource, ItemID.MoltenHelmet);
                        player.QuickSpawnItem(entitySource, ItemID.MoltenBreastplate);
                        player.QuickSpawnItem(entitySource, ItemID.MoltenGreaves);
                        break;
                }
            }
            modplayer.GetAmount();
            for (int i = 0; i < modplayer.weaponAmount; i++)
            {
                GetWeapon(player, out int weapon, out int specialAmount);
                AmmoForWeapon(out int ammo, out int num, weapon);
                player.QuickSpawnItem(entitySource, weapon, specialAmount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            player.QuickSpawnItem(entitySource, GetAccessory());
            for (int i = 0; i < modplayer.potionTypeAmount; i++)
            {
                player.QuickSpawnItem(entitySource, GetPotion(), modplayer.potionNumAmount);
            }
            player.QuickSpawnItem(entitySource, ItemID.WaterWalkingPotion, 10);
            player.QuickSpawnItem(entitySource, ItemID.FeatherfallPotion, 10);
            player.QuickSpawnItem(entitySource, ItemID.GravitationPotion, 10);
            player.QuickSpawnItem(entitySource, ItemID.CalmingPotion, 10);
            player.QuickSpawnItem(entitySource, ItemID.DemonConch);
        }
    }
}
