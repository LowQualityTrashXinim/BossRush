using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Items.Accessories.EnragedBossAccessories.EvilEye;
using BossRush.Items.Spawner;
using BossRush.Items;
using System.Collections.Generic;

namespace BossRush.Common.Global
{
    class GlobalItemMod : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.EoCShield)
            {
                player.GetModPlayer<EvilEyePlayer>().EoCShieldUpgrade = true;
            }
        }

        public override void AddRecipes()
        {
            //QoL convert
            Recipe recipe = Recipe.Create(ItemID.FallenStar, 5);
            recipe.AddIngredient(ItemID.ManaCrystal);
            recipe.Register();

            //enraged covert to normal
            Recipe KingSlimeEnraged = Recipe.Create(ItemID.SlimeCrown);
            KingSlimeEnraged.AddIngredient(ModContent.ItemType<KingSlimeCall>());

            Recipe EoCEnraged = Recipe.Create(ItemID.SuspiciousLookingEye);
            EoCEnraged.AddIngredient(ModContent.ItemType<BloodyEye>());

            Recipe EoWEnraged = Recipe.Create(ItemID.WormFood);
            EoWEnraged.AddIngredient(ModContent.ItemType<WormFeast>());

            Recipe BoWEnraged = Recipe.Create(ItemID.BloodySpine);
            BoWEnraged.AddIngredient(ModContent.ItemType<AttackOnTheMind>());

            Recipe MoonLordEnraged = Recipe.Create(ItemID.CelestialSigil);
            MoonLordEnraged.AddIngredient(ModContent.ItemType<MoonLordEnrage>());

            KingSlimeEnraged.Register();
            EoCEnraged.Register();
            EoWEnraged.Register();
            BoWEnraged.Register();
            MoonLordEnraged.Register();
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (ModContent.GetInstance<BossRushModConfig>().Enraged && player.whoAmI == Main.myPlayer && item.type == ItemID.SuspiciousLookingEye)
            {
                player.GetModPlayer<ModdedPlayer>().EoCEnraged = true;
                int type = NPCID.EyeofCthulhu;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // If the player is not in multiplayer, spawn directly
                    for (int i = 0; i < 2; i++)
                    {
                        NPC.SpawnOnPlayer(player.whoAmI, type);
                    }
                    Main.bloodMoon = true;
                }
                else
                {
                    NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type);
                    NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type);
                }
                return true;
            }
            return default;
        }

    }
}
