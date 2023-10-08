using BossRush.Texture;
using System.Collections.Generic;
using Terraria;

namespace BossRush.Contents.Items.Chest {
	internal class UnderworldLootBox : LootBoxBase {
		public override string Texture => BossRushTexture.PLACEHOLDERCHEST;
		public override void SetDefaults() {
			Item.width = 54;
			Item.height = 38;
			Item.rare = 0;
		}
		public override bool CanRightClick() {
			return true;
		}
		public override List<int> FlagNumber() {
			return new List<int> { 0 };
		}
		public override List<int> FlagNumAcc() => new List<int>() { 0, 1, 2, 3 };
		public override void OnRightClick(Player player, ChestLootDropPlayer modplayer) {
			var entitySource = player.GetSource_OpenItem(Type);
			modplayer.GetAmount();
			for (int i = 0; i < modplayer.weaponAmount; i++) {
				GetWeapon(player, out int weapon, out int specialAmount);
				player.QuickSpawnItem(entitySource, weapon, specialAmount);
			}
			for (int i = 0; i < modplayer.potionTypeAmount; i++) {
				player.QuickSpawnItem(entitySource, GetPotion(), modplayer.potionNumAmount);
			}
			player.QuickSpawnItem(entitySource, GetAccessory());
		}
	}
}