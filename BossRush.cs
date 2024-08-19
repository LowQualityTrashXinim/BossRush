using Terraria;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Weapon;

namespace BossRush {
	public partial class BossRush : Mod {
	}
	public class BossRushModSystem : ModSystem {
		public static Dictionary<int, List<int>> WeaponRarityDB;
		private static List<Item> _synergyitem;
		private static List<Item> _lostAccs;

		public static List<int> ListLootboxType;
		public static List<Item> SynergyItem => _synergyitem;
		public static List<Item> LostAccessories => _lostAccs;
		public override void OnModLoad() {
			_synergyitem = new List<Item>();
			_lostAccs = new List<Item>();
			WeaponRarityDB = new Dictionary<int, List<int>>();
			ListLootboxType = new List<int>();
		}
		public override void OnModUnload() {
			_synergyitem = null;
			_lostAccs = null;
			ListLootboxType = null;
			WeaponRarityDB = null;
		}
		public int AmountOfLootboxOpenInCurrentSection() {
			if (Main.netMode == NetmodeID.SinglePlayer) {
				return Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().CurrentSectionAmountOfChestOpen;
			}
			return -1;
		}
		public override void PostSetupContent() {
			List<Item> cacheitemList = ContentSamples.ItemsByType.Values.ToList();
			for (int i = 0; i < cacheitemList.Count; i++) {
				Item item = cacheitemList[i];
				if(item.ModItem is LootBoxBase) {
					ListLootboxType.Add(item.type);
					continue;
				}
				if (item.ModItem is SynergyModItem) {
					_synergyitem.Add(item);
					continue;
				}
				if (item.TryGetGlobalItem(out GlobalItemHandle globalItem)) {
					if (globalItem.LostAccessories) {
						_lostAccs.Add(item);
						continue;
					}
				}
				if (!item.IsAWeapon()) {
					continue;
				}
				if (!WeaponRarityDB.ContainsKey(item.rare)) {
					WeaponRarityDB.Add(item.rare, new List<int> { item.type });
				}
				else {
					WeaponRarityDB[item.rare].Add(item.type);
				}
			}
		}
	}
}

