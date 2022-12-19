using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Items.Accessories;
using BossRush.Items.CustomPotion;

namespace BossRush.Items.Chest
{
    class WoodenTreasureChest : ChestLootDrop
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Good Luck!");
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 38;
            Item.rare = 0;
        }

        public override bool CanRightClick()
        {
            return true;
        }
        public override List<int> FlagNumber()
        {
            return new List<int> { 0 };
        }
        public override List<int> FlagNumAcc() => new List<int> { 0,1,2 };
        public override void RightClick(Player player)
        {
            var entitySource = player.GetSource_OpenItem(Type);
            GetAmount(out int amount, out int amount2, out int amount3, player);
            for (int i = 0; i < amount; i++)
            {
                GetWeapon(player, out int ReturnWeapon, out int SpecialAmount);
                AmmoForWeapon(out int ammo, out int num, ReturnWeapon);
                player.QuickSpawnItem(entitySource, ReturnWeapon, SpecialAmount);
                player.QuickSpawnItem(entitySource, ammo, num);
            }
            GetWeapon(player, out int ReturnWeaponMelee, out _, 1);
            player.QuickSpawnItem(entitySource, ReturnWeaponMelee);
            for (int i = 0; i < 2; i++)
            {
                if (i == 1)
                {
                    FlagNumAcc(new List<int> {0});
                }
                player.QuickSpawnItem(entitySource, GetAccessory());
            }
            for (int i = 0; i < amount2; i++)
            {
                player.QuickSpawnItem(entitySource, GetPotion(true), amount3);
            }
            int RandomNumber = Main.rand.Next(7);
            switch (RandomNumber)
            {
                case 0:
                    player.QuickSpawnItem(entitySource, ItemID.WoodHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.WoodBreastplate);
                    player.QuickSpawnItem(entitySource, ItemID.WoodGreaves);
                    break;
                case 1:
                    player.QuickSpawnItem(entitySource, ItemID.BorealWoodHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.BorealWoodBreastplate);
                    player.QuickSpawnItem(entitySource, ItemID.BorealWoodGreaves);
                    break;
                case 2:
                    player.QuickSpawnItem(entitySource, ItemID.RichMahoganyHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.RichMahoganyBreastplate);
                    player.QuickSpawnItem(entitySource, ItemID.RichMahoganyGreaves);
                    break;
                case 3:
                    player.QuickSpawnItem(entitySource, ItemID.EbonwoodHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.EbonwoodBreastplate);
                    player.QuickSpawnItem(entitySource, ItemID.EbonwoodGreaves);
                    break;
                case 4:
                    player.QuickSpawnItem(entitySource, ItemID.PalmWoodHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.PalmWoodBreastplate);
                    player.QuickSpawnItem(entitySource, ItemID.PalmWoodGreaves);
                    break;
                case 5:
                    player.QuickSpawnItem(entitySource, ItemID.ShadewoodHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.ShadewoodBreastplate);
                    player.QuickSpawnItem(entitySource, ItemID.ShadewoodGreaves);
                    break;
                case 6:
                    player.QuickSpawnItem(entitySource, ItemID.CactusHelmet);
                    player.QuickSpawnItem(entitySource, ItemID.CactusBreastplate);
                    player.QuickSpawnItem(entitySource, ItemID.CactusLeggings);
                    break;
            }
            int RandomAssArmor = Main.rand.Next(new int[] { ItemID.FlinxFurCoat, ItemID.VikingHelmet, ItemID.EmptyBucket, ItemID.NightVisionHelmet, ItemID.DivingHelmet, ItemID.Goggles, ItemID.Gi });
            player.QuickSpawnItem(entitySource, RandomAssArmor);
            int SuperRare = Main.rand.Next(1000);
            if (SuperRare == 1 && ModContent.GetInstance<BossRushModConfig>().SynergyMode)
            {
                player.QuickSpawnItem(entitySource, ModContent.ItemType<UnlimitedProgress>());
            }
            if (ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode)
            {
                int RandomModdedBuff = Main.rand.Next(new int[] { ModContent.ItemType<BerserkPotion>(), ModContent.ItemType<PinPointAccuracyPotion>(), ModContent.ItemType<SagePotion>(), ModContent.ItemType<LeaderPotion>(), ModContent.ItemType<TankPotion>() });
                player.QuickSpawnItem(entitySource, RandomModdedBuff, 1);
            }
            player.QuickSpawnItem(entitySource, ModContent.ItemType<BuilderTreasureChest>());
            player.QuickSpawnItem(entitySource, ItemID.SlimeCrown);
            player.QuickSpawnItem(entitySource, ItemID.GrapplingHook);
            player.QuickSpawnItem(entitySource, ItemID.LesserHealingPotion, 5);
        }
    }
}