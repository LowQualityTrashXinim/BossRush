using BossRush.ExtraItem;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Chest
{
    class MechTreasureChest : ChestLootDrop
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Right click to open chest");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 7;
        }

        public override bool CanRightClick()
        {
            return true;
        }
        public override List<int> FlagNumber() => new List<int>() { 6, 7, 8, (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 ? 9 : 8) };
        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            int wing = Main.rand.Next(new int[] { ItemID.ButterflyWings, ItemID.FlameWings,ItemID.FrozenWings,ItemID.SteampunkWings,ItemID.Jetpack});
            player.QuickSpawnItem(entitySource,wing);
            GetAmount(out int amount, out int amount2, out int amount3, player);
            for (int i = 0; i < amount; i++)
            {
                GetWeapon(player,out int weapon, out int specialAmount);
                AmmoForWeapon(out int ammo, out int num, weapon);
                player.QuickSpawnItem(entitySource, weapon, specialAmount);
                player.QuickSpawnItem(entitySource,ammo, num);
            }
            for (int i = 0; i < 3; i++)
            {
                GetAccessory(out int Accessory, true, true, true, false, false);
                player.QuickSpawnItem(entitySource, Accessory);
            }
            if(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                player.QuickSpawnItem(entitySource, ItemID.ChlorophytePickaxe);
            }
            for (int i = 0; i < amount2; i++)
            {
                GetPotion(out int potion);
                player.QuickSpawnItem(entitySource, potion, amount3);
            }
            player.QuickSpawnItem(entitySource,ModContent.ItemType<PlanteraEssence>());
            player.QuickSpawnItem(entitySource, ItemID.LifeFruit, 5);
        }
    }
}
