using Terraria;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Weapon;
using BossRush.Common.RoguelikeChange.Prefixes;
using BossRush.Contents.Items.Accessories.TrinketAccessories;
using BossRush.Contents.Items.Consumable.Potion;

namespace BossRush {
	public partial class BossRush : Mod {
		public override void Load() {
			base.Load();
		}
	}
	public class BossRushModSystem : ModSystem {
		public static bool[] IsFireBuff;
		public static bool[] IsPoisonBuff;
		public static bool[] CanBeAffectByLastingVile;
		public static Dictionary<int, List<int>> WeaponRarityDB;
		private static List<Item> _synergyitem;
		private static List<Item> _lostAccs;
		private static List<Item> _trinket;
		public static List<int> ListLootboxType;
		public static List<Item> SynergyItem => _synergyitem;
		public static List<Item> LostAccessories => _lostAccs;
		public static List<Item> TrinketAccessories => _trinket;
		public override void OnModLoad() {
			_trinket = new();
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
			IsFireBuff = null;
			IsPoisonBuff = null;
		}
		public override void PostSetupContent() {
			IsFireBuff = BuffID.Sets.Factory.CreateBoolSet(BuffID.OnFire, BuffID.OnFire3, BuffID.ShadowFlame, BuffID.Frostburn, BuffID.Frostburn2, BuffID.CursedInferno);
			IsPoisonBuff = BuffID.Sets.Factory.CreateBoolSet(BuffID.Poisoned, BuffID.Venom);
			CanBeAffectByLastingVile = BuffID.Sets.Factory.CreateBoolSet(true, ModContent.BuffType<LastingVileBuff>());

			List<Item> cacheitemList = ContentSamples.ItemsByType.Values.ToList();
			for (int i = 0; i < cacheitemList.Count; i++) {
				Item item = cacheitemList[i];
				if (item.ModItem is LootBoxBase) {
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
				if (item.ModItem is BaseTrinket) {
					_trinket.Add(item);
					continue;
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
		public override void PostAddRecipes() {
			if (ModLoader.TryGetMod("PrefixImproved", out Mod PrefixImproved)) {
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Vampiric>()).Name, (byte)4);
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Evasive>()).Name, (byte)4);
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Cunning>()).Name, (byte)2);
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Stealthy>()).Name, (byte)2);
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Savage>()).Name, (byte)2);
			}
		}
	}
}

