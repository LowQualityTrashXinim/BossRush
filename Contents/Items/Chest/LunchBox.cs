using BossRush.Common.Utils;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Chest {
	internal class LunchBox : ModItem {
		public override void SetStaticDefaults() {
			// Tooltip.SetDefault("Made with love \n-From your mom and your crush " + $"[i:{58}]");
		}
		public override void SetDefaults() {
			Item.width = 38;
			Item.height = 46;
			Item.rare = 5;
		}

		public override bool CanRightClick() {
			return true;
		}

		public override void RightClick(Player player) {
			var entitysource = player.GetSource_OpenItem(Type);

			for (int i = 0; i < 4; i++) {
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