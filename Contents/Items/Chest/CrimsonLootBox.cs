using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Contents.Items.Spawner;
using BossRush.Common.Utils;
using BossRush.Contents.Items.BuilderItem;

namespace BossRush.Contents.Items.Chest
{
    class CrimsonLootBox : LootBoxBase
    {
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 3;
        }
        public override List<int> FlagNumber() => new List<int>() { 1, 2, 3, 5 };
        public override List<int> FlagNumAcc() => new List<int> { 0, 1, 2, 3, 4, 5 };
        public override void OnRightClick(Player player,ChestLootDropPlayer modplayer)
        {
            var entitySource = player.GetSource_OpenItem(Type);
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
                switch (Main.rand.Next(5))
                {
                    case 0:
                        player.QuickSpawnItem(entitySource, ItemID.CrimsonHelmet);
                        player.QuickSpawnItem(entitySource, ItemID.CrimsonScalemail);
                        player.QuickSpawnItem(entitySource, ItemID.CrimsonGreaves);
                        break;
                    case 1:
                        player.QuickSpawnItem(entitySource, ItemID.MeteorHelmet);
                        player.QuickSpawnItem(entitySource, ItemID.MeteorSuit);
                        player.QuickSpawnItem(entitySource, ItemID.MeteorLeggings);
                        break;
                    case 2:
                        player.QuickSpawnItem(entitySource, ItemID.FossilHelm);
                        player.QuickSpawnItem(entitySource, ItemID.FossilShirt);
                        player.QuickSpawnItem(entitySource, ItemID.FossilPants);
                        break;
                    case 3:
                        player.QuickSpawnItem(entitySource, ItemID.MoltenHelmet);
                        player.QuickSpawnItem(entitySource, ItemID.MoltenBreastplate);
                        player.QuickSpawnItem(entitySource, ItemID.MoltenGreaves);
                        break;
                    case 4:
                        player.QuickSpawnItem(entitySource, ItemID.ObsidianHelm);
                        player.QuickSpawnItem(entitySource, ItemID.ObsidianShirt);
                        player.QuickSpawnItem(entitySource, ItemID.ObsidianPants);
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
            player.QuickSpawnItem(entitySource, ItemID.TinkerersWorkshop);
            player.QuickSpawnItem(entitySource, ItemID.Hellforge);
            player.QuickSpawnItem(entitySource, ItemID.DD2ElderCrystalStand);
            player.QuickSpawnItem(entitySource, ItemID.DD2ElderCrystal);
            player.QuickSpawnItem(entitySource, ModContent.ItemType<PocketPortal>());
            player.QuickSpawnItem(entitySource, ModContent.ItemType<NeoDynamite>());
            player.QuickSpawnItem(entitySource, ModContent.ItemType<TowerDestruction>());
        }
    }
}