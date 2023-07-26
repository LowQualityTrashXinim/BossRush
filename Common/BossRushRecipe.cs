using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using BossRush.Contents.Items;
using System.Collections.Generic;
using BossRush.Contents.Items.Spawner;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Common
{
    internal class BossRushRecipe : ModSystem
    {
        List<int> list = new List<int>();
        public override void AddRecipes()
        {
            //QoL convert
            Recipe recipe = Recipe.Create(ItemID.FallenStar, 5);
            recipe.AddIngredient(ItemID.ManaCrystal);
            recipe.Register();

            //enraged covert to normal
            Recipe KingSlimeEnraged = Recipe.Create(ItemID.SlimeCrown);
            KingSlimeEnraged.AddIngredient(ModContent.ItemType<KingSlimeSpecialSpawner>());

            Recipe EoCEnraged = Recipe.Create(ItemID.SuspiciousLookingEye);
            EoCEnraged.AddIngredient(ModContent.ItemType<EyeOfCthulhuSpecialSpawner>());

            Recipe EoWEnraged = Recipe.Create(ItemID.WormFood);
            EoWEnraged.AddIngredient(ModContent.ItemType<EaterOfWorldSpecialSpawner>());

            Recipe BoWEnraged = Recipe.Create(ItemID.BloodySpine);
            BoWEnraged.AddIngredient(ModContent.ItemType<BrainOfCthulhuSpecialSpawner>());

            Recipe MoonLordEnraged = Recipe.Create(ItemID.CelestialSigil);
            MoonLordEnraged.AddIngredient(ModContent.ItemType<MoonLordEnrage>());

            KingSlimeEnraged.Register();
            EoCEnraged.Register();
            EoWEnraged.Register();
            BoWEnraged.Register();
            MoonLordEnraged.Register();
        }
        public override void AddRecipeGroups()
        {
            foreach (var item in ContentSamples.ItemsByType)
            {
                if (item.Value.ModItem is SynergyModItem)
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
            foreach (Recipe recipe in Main.recipe)
            {
                SynergyRecipe(recipe);
                EnragedBossSpawnerRecipe(recipe);
                if (config.EnableChallengeMode)
                {
                    continue;
                }
                ChallengeModeRecipe(recipe);
            }
        }
        private void SynergyRecipe(Recipe recipe)
        {
            if (recipe.createItem.ModItem is SynergyModItem)
            {
                recipe.AddIngredient(ModContent.ItemType<SynergyEnergy>());
            }
        }
        private void EnragedBossSpawnerRecipe(Recipe recipe)
        {
            if (recipe.createItem.ModItem is EnragedSpawner)
            {
                recipe.AddIngredient(ModContent.ItemType<PowerEnergy>());
            }
        }
        private void ChallengeModeRecipe(Recipe recipe)
        {
            if (recipe.HasResult(ItemID.FlamingArrow) ||
                recipe.HasResult(ItemID.FrostburnArrow))
            {
                recipe.DisableRecipe();
            }
        }
    }
}