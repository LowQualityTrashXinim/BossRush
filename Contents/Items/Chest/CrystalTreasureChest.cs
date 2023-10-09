using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.Chest
{
    class CrystalTreasureChest : LootBoxBase
    {
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 6;
        }
        public override List<int> FlagNumber() => new List<int>() { 7, 8};
        public override List<int> FlagNumAcc() => new List<int>() { 8, 9, 10 };
        public override void OnRightClick(Player player, ChestLootDropPlayer modplayer)
         {
            var entitySource = player.GetSource_OpenItem(Type);
            int wing = Main.rand.Next(new int[] { ItemID.AngelWings, ItemID.DemonWings, ItemID.LeafWings, ItemID.FairyWings, ItemID.HarpyWings });
            player.QuickSpawnItem(entitySource, wing);
            switch (Main.rand.Next(3))
            {
                case 0:
                    player.QuickSpawnItem(entitySource, ItemID.FrostHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.FrostBreastplate);
                    player.QuickSpawnItem(entitySource, ItemID.FrostLeggings);
                    break;
                case 1:
                    player.QuickSpawnItem(entitySource, ItemID.AncientBattleArmorHat);
                    player.QuickSpawnItem(entitySource, ItemID.AncientBattleArmorShirt);
                    player.QuickSpawnItem(entitySource, ItemID.AncientBattleArmorPants);
                    break;
                case 2:
                    player.QuickSpawnItem(entitySource, ItemID.CrystalNinjaHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.CrystalNinjaChestplate);
                    player.QuickSpawnItem(entitySource, ItemID.CrystalNinjaLeggings);
                    break;
            }
            modplayer.GetAmount();
            for (int i = 0; i < modplayer.weaponAmount; i++)
            {
                GetWeapon(player, out int weapon, out int specialAmount);
                AmmoForWeapon(out int ammo, out int num, weapon);
                player.QuickSpawnItem(entitySource, weapon, specialAmount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            for (int i = 0; i < 2; i++)
            {
                player.QuickSpawnItem(entitySource, GetAccessory());
            }
            for (int i = 0; i < modplayer.potionTypeAmount; i++)
            {
                player.QuickSpawnItem(entitySource, GetPotion(), modplayer.potionNumAmount);
            }
            if (Main.rand.NextBool(5))
            {
                player.QuickSpawnItem(entitySource, ItemID.QueenSlimeBossBag);
            }
            if (Main.rand.NextBool(20))
            {
                player.QuickSpawnItem(entitySource, ItemID.RodofDiscord);
            }
        }
    }
}