using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.Chest {
	internal class BloodTreasureChest : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 38;
			Item.height = 30;
			Item.rare = ItemRarityID.LightPurple;
		}
		//public override List<int> FlagNumber() => new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
		public override List<int> FlagNumAcc() => new List<int>() { 8, 9, 10 };
		public override void OnRightClick(Player player, ChestLootDropPlayer modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			modplayer.GetAmount();
			GetWeapon(entitySource, player, modplayer.weaponAmount);
			for (int i = 0; i < modplayer.potionTypeAmount; i++) {
				player.QuickSpawnItem(entitySource, GetPotion(), modplayer.potionNumAmount);
			}
			player.QuickSpawnItem(entitySource, GetAccessory());
		}
	}
}
