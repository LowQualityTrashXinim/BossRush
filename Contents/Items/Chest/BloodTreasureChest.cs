using System.Collections.Generic;
using Terraria;

namespace BossRush.Contents.Items.Chest {
	internal class BloodTreasureChest : LootBoxBase {
		public override void SetDefaults() {
			Item.width = 38;
			Item.height = 30;
			Item.rare = 6;
		}
		public override List<int> FlagNumber() => new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
		public override List<int> FlagNumAcc() => new List<int>() { 8, 9, 10 };
		public override void OnRightClick(Player player, ChestLootDropPlayer modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			modplayer.GetAmount();
			for (int i = 0; i < modplayer.weaponAmount; i++) {
				GetWeapon(player, out int weapon, out int specialAmount);
				AmmoForWeapon(out int ammo, out int num, weapon);
				player.QuickSpawnItem(entitySource, weapon, specialAmount);
				player.QuickSpawnItem(entitySource, ammo, num);
			}
			for (int i = 0; i < modplayer.potionTypeAmount; i++) {
				player.QuickSpawnItem(entitySource, GetPotion(), modplayer.potionNumAmount);
			}
			player.QuickSpawnItem(entitySource, GetAccessory());
		}
	}
}