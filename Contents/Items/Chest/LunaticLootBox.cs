using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Chest {
	internal class LunaticLootBox : ModItem {
		public override void SetDefaults() {
			Item.width = 38;
			Item.height = 30;
			Item.rare = ItemRarityID.Red;
		}

		public override bool CanRightClick() {
			return true;
		}

		public override void RightClick(Player player) {
			var entitySource = player.GetSource_OpenItem(Type);
			int LunarWeapon = Main.rand.Next(new int[] { ItemID.DayBreak, ItemID.SolarEruption, ItemID.NebulaBlaze, ItemID.NebulaArcanum, ItemID.VortexBeater, ItemID.Phantasm, ItemID.StardustDragonStaff, ItemID.StardustCellStaff });
			player.QuickSpawnItem(entitySource, LunarWeapon, 1);
		}

	}
}
