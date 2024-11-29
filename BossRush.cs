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
		public static BossRush Instance { get; private set; }
		public override void Load() {
			Instance = this;
			base.Load();
		}
	}
	public class BossRushModSystem : ModSystem {
		public static bool[] IsFireBuff;
		public static bool[] IsPoisonBuff;
		public static bool[] CanBeAffectByLastingVile;
		public static bool[] AdvancedRPGItem;
		public static Dictionary<int, List<int>> WeaponRarityDB;
		private static List<Item> _synergyitem;
		private static List<Item> _lostAccs;
		private static List<Item> _trinket;
		private static List<Item> _rpgitem;

		public static List<int> ListLootboxType;
		public static List<Item> SynergyItem => _synergyitem;
		public static List<Item> LostAccessories => _lostAccs;
		public static List<Item> TrinketAccessories => _trinket;
		public static List<Item> RPGItem => _rpgitem;
		public override void OnModLoad() {
			_trinket = new();
			_rpgitem = new();
			_synergyitem = new();
			_lostAccs = new();
			WeaponRarityDB = new();
			ListLootboxType = new();
		}
		public override void OnModUnload() {
			_synergyitem = null;
			_lostAccs = null;
			_rpgitem = null;
			_trinket = null;
			ListLootboxType = null;
			WeaponRarityDB = null;
			IsFireBuff = null;
			IsPoisonBuff = null;
		}
		public override void PostSetupContent() {
			IsFireBuff = BuffID.Sets.Factory.CreateBoolSet(BuffID.OnFire, BuffID.OnFire3, BuffID.ShadowFlame, BuffID.Frostburn, BuffID.Frostburn2, BuffID.CursedInferno);
			IsPoisonBuff = BuffID.Sets.Factory.CreateBoolSet(BuffID.Poisoned, BuffID.Venom);
			CanBeAffectByLastingVile = BuffID.Sets.Factory.CreateBoolSet(true, ModContent.BuffType<LastingVileBuff>());
			AdvancedRPGItem = ItemID.Sets.Factory.CreateBoolSet();
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
					else if (globalItem.RPGItem) {
						if (globalItem.AdvancedBuffItem) {
							AdvancedRPGItem[item.type] = true;
						}
						_rpgitem.Add(item);
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
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Jumpy>()).Name, (byte)4);
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Evasive>()).Name, (byte)4);
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Cunning>()).Name, (byte)2);
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Stealthy>()).Name, (byte)2);
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Spiky>()).Name, (byte)1);
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Alchemic>()).Name, (byte)2);
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Energetic>()).Name, (byte)2);
			}
		}
	}
}

