using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Artifact;

namespace BossRush.Common
{
    internal class BossRushRecipe : ModSystem
    {
        List<int> list = new List<int>();
        public override void AddRecipeGroups()
        {
            foreach (var item in ContentSamples.ItemsByType)
            {
                if (item.Value.ModItem is ISynergyItem)
                {
                    list.Add(item.Key);
                }
            }
            RecipeGroup SynergyItem = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<SynergyEnergy>())}", list.ToArray());
            RecipeGroup.RegisterGroup("Synergy Item", SynergyItem);

            RecipeGroup WoodSword = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.WoodenSword)}", new int[]
            {
                ItemID.WoodenSword,
                ItemID.BorealWoodSword,
                ItemID.RichMahoganySword,
                ItemID.ShadewoodSword,
                ItemID.EbonwoodSword,
                ItemID.PalmWoodSword,
                ItemID.PearlwoodSword,
            });
            RecipeGroup.RegisterGroup("Wood Sword", WoodSword);

            RecipeGroup WoodBow = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.WoodenBow)}", new int[]
            {
                ItemID.WoodenBow,
                ItemID.BorealWoodBow,
                ItemID.RichMahoganyBow,
                ItemID.ShadewoodBow,
                ItemID.EbonwoodBow,
                ItemID.PalmWoodBow,
                ItemID.PearlwoodBow,
            });
            RecipeGroup.RegisterGroup("Wood Bow", WoodBow);

            RecipeGroup OreShortSword = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CopperShortsword)}", new int[]
            {
                ItemID.CopperShortsword,
                ItemID.TinShortsword,
                ItemID.IronShortsword,
                ItemID.LeadShortsword,
                ItemID.SilverShortsword,
                ItemID.TungstenShortsword,
                ItemID.GoldShortsword,
                ItemID.PlatinumShortsword,
            });
            RecipeGroup.RegisterGroup("OreShortSword", OreShortSword);

            RecipeGroup OreBroadSword = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CopperBroadsword)}", new int[]
            {
                ItemID.CopperBroadsword,
                ItemID.TinBroadsword,
                ItemID.IronBroadsword,
                ItemID.LeadBroadsword,
                ItemID.SilverBroadsword,
                ItemID.TungstenBroadsword,
                ItemID.GoldBroadsword,
                ItemID.PlatinumBroadsword,
            });
            RecipeGroup.RegisterGroup("OreBroadSword", OreBroadSword);

            RecipeGroup OreBow = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CopperBow)}", new int[]
            {
                ItemID.CopperBow,
                ItemID.TinBow,
                ItemID.IronBow,
                ItemID.LeadBow,
                ItemID.SilverBow,
                ItemID.TungstenBow,
                ItemID.GoldBow,
                ItemID.PlatinumBow,
            });
            RecipeGroup.RegisterGroup("OreBow", OreBow);
        }
        public override void PostAddRecipes()
        {
            BossRushModConfig config = ModContent.GetInstance<BossRushModConfig>();
            SynergyRecipe();
            ChallengeModeRecipe(config);
        }
        private static void SynergyRecipe()
        {
            foreach (Recipe recipe in Main.recipe)
            {
                if (recipe.createItem.ModItem is ISynergyItem)
                {
                    recipe.AddIngredient(ModContent.ItemType<SynergyEnergy>());
                }
            }
        }
        private static void ChallengeModeRecipe(BossRushModConfig config)
        {
            if (!config.EnableChallengeMode)
            {
                return;
            }
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                if (Main.recipe[i].HasResult(ItemID.FlamingArrow) ||
                    Main.recipe[i].HasResult(ItemID.FrostburnArrow))
                {
                    Main.recipe[i].DisableRecipe();
                }
            }
        }
    }
}