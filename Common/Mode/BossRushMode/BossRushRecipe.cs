﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using BossRush.Contents.Items;
using BossRush.Common.Systems;
using System.Collections.Generic;
using BossRush.Contents.Items.Weapon;
using BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.FlamingWoodSword;
using BossRush.Common.General;

namespace BossRush.Common.Mode.BossRushMode {
	internal class BossRushRecipe : ModSystem {
		List<int> list = new List<int>();
		public override void AddRecipes() {
			//QoL convert
			Recipe recipe = Recipe.Create(ItemID.FallenStar, 5);
			recipe.AddIngredient(ItemID.ManaCrystal);
			recipe.Register();
		}
		public override void AddRecipeGroups() {
			//foreach (var item in ContentSamples.ItemsByType) {
			//	if (item.Value.ModItem is SynergyModItem) {
			//		list.Add(item.Key);
			//	}
			//}
			//RecipeGroup SynergyItem = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<SynergyEnergy>())}", list.ToArray());
			//RecipeGroup.RegisterGroup("Synergy Item", SynergyItem);

			RecipeGroup WoodSword = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} Wood sword", new int[]
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

			RecipeGroup WoodBow = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} Wood bow", new int[]
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

			RecipeGroup OreShortSword = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} Ore short sword", new int[]
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

			RecipeGroup OreBroadSword = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} Ore broad sword", new int[]
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

			RecipeGroup OreBow = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} Ore Bow", new int[]
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
		public override void PostAddRecipes() {
			RogueLikeConfig config = ModContent.GetInstance<RogueLikeConfig>();
			foreach (Recipe recipe in Main.recipe) {
				SynergyRecipe(recipe);
				if (UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE)) {
					ChallengeModeRecipe(recipe);
				}
			}
		}
		private void SynergyRecipe(Recipe recipe) {
			if (recipe.createItem.ModItem is SynergyModItem) {
				recipe.AddIngredient(ModContent.ItemType<SynergyEnergy>());
			}
		}
		private void ChallengeModeRecipe(Recipe recipe) {
			if (recipe.HasResult(ItemID.FlamingArrow) ||
				recipe.HasResult(ItemID.FrostburnArrow) ||
				recipe.HasResult(ItemID.CursedArrow)) {
				recipe.DisableRecipe();
			}
		}
	}
}
