using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using BossRush.Common.Utils;

namespace BossRush.Contents.Items.Chest
{
    class IronLootBox : LootBoxBase
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Good Luck!");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 1;
        }
        public override List<int> FlagNumber() => new List<int> { 0, 1, 2, 3 };
        public override List<int> FlagNumAcc() => new List<int> { 0, 1, 2 };
        public override void OnRightClick(Player player, ChestLootDropPlayer modplayer)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            if (player.IsDebugPlayer())
            {
                int[] fullbodyarmor = new int[]{
                Main.rand.Next(TerrariaArrayID.HeadArmorPreBoss),
                Main.rand.Next(TerrariaArrayID.BodyArmorPreBoss),
                Main.rand.Next(TerrariaArrayID.LegArmorPreBoss) };
                for (int i = 0; i < fullbodyarmor.Length; i++)
                {
                    player.QuickSpawnItem(entitySource, fullbodyarmor[i]);
                }
            }
            else
            {
                int RandomNumber = Main.rand.Next(6);
                switch (RandomNumber)
                {
                    case 0:
                        player.QuickSpawnItem(entitySource, ItemID.CopperHelmet);
                        player.QuickSpawnItem(entitySource, ItemID.CopperChainmail);
                        player.QuickSpawnItem(entitySource, ItemID.CopperGreaves);
                        break;
                    case 1:
                        player.QuickSpawnItem(entitySource, ItemID.TinHelmet);
                        player.QuickSpawnItem(entitySource, ItemID.TinChainmail);
                        player.QuickSpawnItem(entitySource, ItemID.TinGreaves);
                        break;
                    case 2:
                        player.QuickSpawnItem(entitySource, ItemID.IronHelmet);
                        player.QuickSpawnItem(entitySource, ItemID.IronChainmail);
                        player.QuickSpawnItem(entitySource, ItemID.IronGreaves);
                        break;
                    case 3:
                        player.QuickSpawnItem(entitySource, ItemID.LeadHelmet);
                        player.QuickSpawnItem(entitySource, ItemID.LeadChainmail);
                        player.QuickSpawnItem(entitySource, ItemID.LeadGreaves);
                        break;
                    case 4:
                        player.QuickSpawnItem(entitySource, ItemID.NinjaHood);
                        player.QuickSpawnItem(entitySource, ItemID.NinjaPants);
                        player.QuickSpawnItem(entitySource, ItemID.NinjaShirt);
                        break;
                    case 5:
                        player.QuickSpawnItem(entitySource, ItemID.JungleHat);
                        player.QuickSpawnItem(entitySource, ItemID.JungleShirt);
                        player.QuickSpawnItem(entitySource, ItemID.JunglePants);
                        break;
                }
            }
            modplayer.GetAmount();
            for (int i = 0; i < modplayer.weaponAmount; i++)
            {
                GetWeapon(player, out int ReturnWeapon, out int SpecialAmount);
                AmmoForWeapon(out int ammo, out int num, ReturnWeapon);
                player.QuickSpawnItem(entitySource, ReturnWeapon, SpecialAmount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            player.QuickSpawnItem(entitySource, GetAccessory());
            for (int i = 0; i < modplayer.potionTypeAmount; i++)
            {
                player.QuickSpawnItem(entitySource, GetPotion(), modplayer.potionNumAmount);
            }
            if (Main.rand.NextBool(5))
            {
                player.QuickSpawnItem(entitySource, ItemID.KingSlimeBossBag);
            }
            player.QuickSpawnItem(entitySource, ItemID.Campfire, 10);
            player.QuickSpawnItem(entitySource, ItemID.PlatinumPickaxe);
            player.QuickSpawnItem(entitySource, ItemID.PlatinumAxe);
        }
    }
}