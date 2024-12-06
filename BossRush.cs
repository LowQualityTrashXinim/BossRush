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
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using BossRush.Common.Utils;

namespace BossRush {
	public partial class BossRush : Mod {
		public static BossRush Instance { get; private set; }
		public override void Load() {
			Instance = this;
			base.Load();
			loadShaders();
		}
		public void loadShaders() 
		{
			if (Main.netMode != NetmodeID.Server) {
				Asset<Effect> trailEffect = Assets.Request<Effect>("Contents/Shaders/TrailEffect");
				GameShaders.Misc["TrailEffect"] = new MiscShaderData(trailEffect, "FadeTrail");
				Asset<Effect> flameEffect = Assets.Request<Effect>("Contents/Shaders/FlameEffect");
				GameShaders.Misc["FlameEffect"] = new MiscShaderData(flameEffect, "FlamethrowerFlame");
			}
		}
	}
	public class BossRushModSystem : ModSystem {
		public static bool[] IsFireBuff;
		public static bool[] IsPoisonBuff;
		public static bool[] CanBeAffectByLastingVile;
		public static bool[] AdvancedRPGItem;
		public static Dictionary<int, List<int>> WeaponRarityDB { get; private set; }
		public static Dictionary<int, List<int>> AccRarityDB { get; private set; }
		public static Dictionary<int, List<int>> HeadArmorRarityDB { get; private set; }
		public static Dictionary<int, List<int>> BodyArmorRarityDB { get; private set; }
		public static Dictionary<int, List<int>> LegsArmorRarityDB { get; private set; }
		public static List<Item> SynergyItem { get; private set; }
		public static List<Item> LostAccessories { get; private set; }
		public static List<Item> TrinketAccessories { get; private set; }
		public static List<Item> RPGItem { get; private set; }
		public static List<int> ListLootboxType { get; private set; }
		public static HashSet<Item> List_Weapon { get; private set; }
		public static int Safe_GetWeaponRarity(int rare) {
			if (WeaponRarityDB.ContainsKey(rare)) {
				return Main.rand.Next(WeaponRarityDB[rare]);
			}
			return ItemID.None;
		}
		public static int Safe_GetAccRarity(int rare) {
			if (AccRarityDB.ContainsKey(rare)) {
				return Main.rand.Next(AccRarityDB[rare]);
			}
			return ItemID.None;
		}
		public static int Safe_GetHeadRarity(int rare) {
			if (HeadArmorRarityDB.ContainsKey(rare)) {
				return Main.rand.Next(HeadArmorRarityDB[rare]);
			}
			return ItemID.None;
		}
		public static int Safe_GetBodyRarity(int rare) {
			if (BodyArmorRarityDB.ContainsKey(rare)) {
				return Main.rand.Next(BodyArmorRarityDB[rare]);
			}
			return ItemID.None;
		}
		public static int Safe_GetLegsRarity(int rare) {
			if (LegsArmorRarityDB.ContainsKey(rare)) {
				return Main.rand.Next(LegsArmorRarityDB[rare]);
			}
			return ItemID.None;
		}
		public override void OnModLoad() {
			TrinketAccessories = new();
			LostAccessories = new();
			SynergyItem = new();
			RPGItem = new();
			WeaponRarityDB = new();
			ListLootboxType = new();
			HeadArmorRarityDB = new();
			BodyArmorRarityDB = new();
			LegsArmorRarityDB = new();
			AccRarityDB = new();
			List_Weapon = new();
		}
		public override void OnModUnload() {
			SynergyItem = null;
			LostAccessories = null;
			RPGItem = null;
			TrinketAccessories = null;
			ListLootboxType = null;
			WeaponRarityDB = null;
			IsFireBuff = null;
			IsPoisonBuff = null;
			HeadArmorRarityDB = null;
			BodyArmorRarityDB = null;
			LegsArmorRarityDB = null;
			AccRarityDB = null;
			List_Weapon = null;
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
					SynergyItem.Add(item);
					continue;
				}
				if (!item.vanity)
					if (item.headSlot > 0) {
						if (!HeadArmorRarityDB.ContainsKey(item.rare)) {
							HeadArmorRarityDB.Add(item.rare, new List<int> { item.type });
						}
						else {
							HeadArmorRarityDB[item.rare].Add(item.type);
						}
						continue;
					}
					else if (item.bodySlot > 0) {
						if (!BodyArmorRarityDB.ContainsKey(item.rare)) {
							BodyArmorRarityDB.Add(item.rare, new List<int> { item.type });
						}
						else {
							BodyArmorRarityDB[item.rare].Add(item.type);
						}
						continue;
					}
					else if (item.legSlot > 0) {
						if (!LegsArmorRarityDB.ContainsKey(item.rare)) {
							LegsArmorRarityDB.Add(item.rare, new List<int> { item.type });
						}
						else {
							LegsArmorRarityDB[item.rare].Add(item.type);
						}
						continue;
					}
				if (item.TryGetGlobalItem(out GlobalItemHandle globalitem)) {
					if (globalitem.LostAccessories) {
						LostAccessories.Add(item);
						continue;
					}
					if (globalitem.RPGItem) {
						if (globalitem.AdvancedBuffItem) {
							AdvancedRPGItem[item.type] = true;
						}
						RPGItem.Add(item);
						continue;
					}
				}
				if (item.accessory && !item.vanity && item.createTile == -1 
					&& item.type != ItemID.ClothierVoodooDoll
					&& item.type != ItemID.GuideVoodooDoll
					&& item.type != ItemID.TreasureMagnet
					&& item.type != ItemID.DontStarveShaderItem
					&& item.type != ItemID.JellyfishNecklace
					&& item.type != ItemID.JellyfishDivingGear
					&& item.type != ItemID.GreedyRing
					&& item.type != ItemID.GoldRing
					&& item.type != ItemID.LuckyCoin
					&& item.type != ItemID.DiscountCard
					&& item.type != ItemID.CoinRing
					&& item.type != ItemID.ShimmerCloak
					&& item.type != ItemID.SpectreGoggles
					&& item.type != ItemID.FlowerBoots
					&& item.type != ItemID.CordageGuide
					&& !TerrariaArrayID.IsFishingBobber.Contains(item.type)
					&& !TerrariaArrayID.NonHelpfulCombatAcc.Contains(item.type)
					&& !TerrariaArrayID.IsInfoAcc.Contains(item.type)
					&& !TerrariaArrayID.FishingAcc.Contains(item.type)) {
					if (item.ModItem is BaseTrinket) {
						TrinketAccessories.Add(item);
						continue;
					}
					if (!AccRarityDB.ContainsKey(item.rare)) {
						AccRarityDB.Add(item.rare, new List<int> { item.type });
					}
					else {
						AccRarityDB[item.rare].Add(item.type);
					}
					continue;
				}
				if (!item.IsAWeapon()) {
					continue;
				}
				List_Weapon.Add(item);
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
				PrefixImproved.Call("AddValueToModdedPrefix", PrefixLoader.GetPrefix(ModContent.PrefixType<Holy>()).Name, (byte)4);
			}
		}
	}
}

