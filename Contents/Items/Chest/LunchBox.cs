﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Utils;

namespace BossRush.Contents.Items.Chest {
	internal class LunchBox : ModItem {
		public override void SetDefaults() {
			Item.width = 38;
			Item.height = 46;
			Item.rare = ItemRarityID.Pink;
		}

		public override bool CanRightClick() {
			return true;
		}

		public override void RightClick(Player player) {
			var entitysource = player.GetSource_OpenItem(Type);

			for (int i = 0; i < 2; i++) {
				int Chooser = Main.rand.Next(new int[] { Main.rand.Next(TerrariaArrayID.WeakDrink), Main.rand.Next(TerrariaArrayID.Smallmeal), Main.rand.Next(TerrariaArrayID.fruit) });
				if (Main.hardMode) {
					Chooser = Main.rand.Next(new int[] { Main.rand.Next(TerrariaArrayID.MediumDrink), Main.rand.Next(TerrariaArrayID.MediumMeal) });
				}
				if (NPC.downedPlantBoss) {
					Chooser = Main.rand.Next(new int[] { Main.rand.Next(TerrariaArrayID.BigMeal), Main.rand.Next(TerrariaArrayID.StrongDrink) });
				}
				int amount = Main.rand.Next(7, 10);
				player.QuickSpawnItem(entitysource, Chooser, amount);
			}
		}
	}
}
