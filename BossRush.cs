using Terraria;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Common.Achievement;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Weapon;

namespace BossRush {
	public partial class BossRush : Mod {
	}
	public class RogueLikeData {
		public int AmountOfRun = 0;
		public int AmountOfLootBoxOpen = 0;
		public List<int> SynergyItemTouch = new List<int>();
	}
	public class BossRushModSystem : ModSystem {
		public static RogueLikeData roguelikedata;
		public override void OnModLoad() {
			roguelikedata = new RogueLikeData();
			_synergyitem = new List<Item>();
			_lostAccs = new List<Item>();
		}
		public override void OnWorldUnload() {
			roguelikedata.AmountOfLootBoxOpen += Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().CurrentSectionAmountOfChestOpen;
		}
		public override void OnModUnload() {
			roguelikedata = null;
			_synergyitem = null;
			_lostAccs = null;
		}
		public override void PostUpdateEverything() {
			foreach (var achieve in AchievementLoader.Achievement.Values) {
				bool condition = achieve.ConditionCheck();
				if (condition) {
					achieve.Condition = true;
				}
			}
		}
		public int AmountOfLootboxOpenInCurrentSection() {
			if (Main.netMode == NetmodeID.SinglePlayer) {
				return Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>().CurrentSectionAmountOfChestOpen;
			}
			return -1;
		}
		public override void PostSetupContent() {
			_synergyitem = new List<Item>();
			_lostAccs = new List<Item>();
			List<Item> cacheitemList = ContentSamples.ItemsByType.Values.ToList();
			for (int i = 0; i < cacheitemList.Count; i++) {
				if (cacheitemList[i].ModItem is SynergyModItem) {
					_synergyitem.Add(cacheitemList[i]);
				}
				if (cacheitemList[i].TryGetGlobalItem<GlobalItemHandle>(out GlobalItemHandle globalItem)) {
					if (globalItem.LostAccessories) {
						_lostAccs.Add(cacheitemList[i]);
					}
				}
			}
		}
		private static List<Item> _synergyitem;
		public static List<Item> SynergyItem => _synergyitem;
		private static List<Item> _lostAccs;
		public static List<Item> LostAccessories => _lostAccs;
	}
}
