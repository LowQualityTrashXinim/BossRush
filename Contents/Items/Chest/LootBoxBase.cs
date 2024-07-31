using System;
using Terraria;
using System.IO;
using Terraria.ID;
using System.Linq;
using Terraria.ModLoader;
using Terraria.GameContent;
using BossRush.Common.Utils;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using Terraria.DataStructures;
using System.Collections.Generic;
using BossRush.Contents.Items.Potion;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Contents.Items.RelicItem;
using BossRush.Common;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.NatureSelection;
using BossRush.Contents.Items.Weapon;
using BossRush.Contents.WeaponEnchantment;

namespace BossRush.Contents.Items.Chest {
	public abstract class LootBoxBase : ModItem {
		public override void SetStaticDefaults() {
			LootPoolSetStaticDefaults();
		}
		/// <summary>
		/// Set your lootbox loot pool here<br/>
		/// Add loot pool into this <see cref="LootboxSystem.LootBoxDropPool"/> and use <see cref="LootBoxItemPool"/> to add your pool in, all of them is hashset
		/// </summary>
		public virtual void LootPoolSetStaticDefaults() {

		}
		public virtual bool ChestUseOwnLogic => false;
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
			if (!ChestUseOwnLogic) {
				if (LootboxSystem.GetItemPool(Type) == null)
					return;
				if (LootboxSystem.GetItemPool(Type).AllItemPool().Count <= 0)
					return;
				//absolutely not recommend to do this
				List<int> potiontotal = [.. TerrariaArrayID.NonMovementPotion, .. TerrariaArrayID.MovementPotion];
				if (chestplayer.weaponShowID == 0 || --chestplayer.counterShow <= 0) {
					chestplayer.weaponShowID = Main.rand.NextFromHashSet(LootboxSystem.GetItemPool(Type).AllItemPool());
					chestplayer.potionShowID = Main.rand.Next(potiontotal);
					chestplayer.counterShow = 6;
				}
				chestplayer.GetAmount();
				TooltipLine chestline = new TooltipLine(Mod, "ChestLoot",
					$"Weapon : [i:{chestplayer.weaponShowID}] x {chestplayer.weaponAmount}\n" +
					$"Potion type : [i:{chestplayer.potionShowID}] x {chestplayer.potionTypeAmount}\n" +
					$"Amount of potion : [i:{ItemID.RegenerationPotion}][i:{ItemID.SwiftnessPotion}][i:{ItemID.IronskinPotion}] x {chestplayer.potionNumAmount}");
				tooltips.Add(chestline);
			}
			PostModifyTooltips(ref tooltips);
		}
		public virtual void PostModifyTooltips(ref List<TooltipLine> tooltips) { }
		private int ModifyRNG(int rng, Player player) {
			int DrugValue = player.GetModPlayer<WonderDrugPlayer>().DrugDealer;
			if (DrugValue > 0) {
				if (Main.rand.Next(100 + DrugValue * 5) <= DrugValue * 10) {
					return 6;
				}
			}
			return rng;
		}
		protected int RNGManage(Player player, int meleeChance = 20, int rangeChance = 25, int magicChance = 25, int summonChance = 15, int specialChance = 15) {
			ChestLootDropPlayer modPlayer = player.GetModPlayer<ChestLootDropPlayer>();
			meleeChance = (int)((modPlayer.MeleeChanceMutilplier + modPlayer.UpdateMeleeChanceMutilplier) * meleeChance);
			rangeChance = (int)((modPlayer.RangeChanceMutilplier + modPlayer.UpdateRangeChanceMutilplier) * rangeChance);
			magicChance = (int)((modPlayer.MagicChanceMutilplier + modPlayer.UpdateMagicChanceMutilplier) * magicChance);
			summonChance = (int)((modPlayer.SummonChanceMutilplier + modPlayer.UpdateSummonChanceMutilplier) * summonChance);
			rangeChance += meleeChance;
			magicChance += rangeChance;
			summonChance += magicChance;
			specialChance += summonChance;
			int chooser = Main.rand.Next(specialChance);
			if (chooser <= meleeChance) {
				return 1;
			}
			else if (chooser > meleeChance && chooser <= rangeChance) {
				return 2;
			}
			else if (chooser > rangeChance && chooser <= magicChance) {
				return 3;
			}
			else if (chooser > magicChance && chooser <= summonChance) {
				return 4;
			}
			else if (chooser > summonChance && chooser <= specialChance) {
				return 5;
			}
			return 0;
		}
		/// <summary>
		/// Use this to modify the item pool before the process of choosing weapon is proceed<br/>
		/// Use <see cref="LootboxSystem.GetItemPool"/> to modify loot pool
		/// </summary>
		public virtual void ModifyLootAdd(Player player) { }
		/// <summary>
		/// Use this to add condition to chest
		/// </summary>
		/// <returns></returns>
		public virtual bool CanBeRightClick() => false;
		public override bool CanRightClick() => true;
		public override void RightClick(Player player) {
			ChestLootDropPlayer modplayer = player.GetModPlayer<ChestLootDropPlayer>();
			modplayer.CurrentSectionAmountOfChestOpen++;
			base.RightClick(player);
			if (modplayer.ItemGraveYard.Count > 0) {
				int RemoveAmount = 1 + modplayer.ItemGraveYard.Count / 10;
				for (int i = 0; i < RemoveAmount; i++) {
					int index = Main.rand.Next(modplayer.ItemGraveYard.Count);
					modplayer.ItemGraveYard.Remove(modplayer.ItemGraveYard.ElementAt(index));
				}
			}
			OnRightClick(player, modplayer);
			if (!UniversalSystem.CanAccessContent(player, UniversalSystem.TRUE_MODE)) {
				return;
			}
			var entitySource = player.GetSource_OpenItem(Type);
			if (UniversalSystem.CanAccessContent(player, UniversalSystem.SYNERGYFEVER_MODE)) {
				Item relicitem = player.QuickSpawnItemDirect(entitySource, ModContent.ItemType<Relic>());
				if (relicitem.ModItem is Relic relic) {
					relic.AddRelicTemplate(player, RelicTemplate.GetRelicType<SynergyTemplate>());
				}
			}
			else {
				player.QuickSpawnItem(entitySource, ModContent.ItemType<Relic>());
			}
			player.QuickSpawnItem(entitySource, ModContent.ItemType<SkillLootBox>());
			if (modplayer.LootboxCanDropSpecialPotion) {
				player.QuickSpawnItem(entitySource, Main.rand.Next(TerrariaArrayID.SpecialPotion));
			}
			if (modplayer.CanDropSynergyEnergy) {
				player.QuickSpawnItem(entitySource, ModContent.ItemType<SynergyEnergy>());
			}
		}
		public virtual void OnRightClick(Player player, ChestLootDropPlayer modplayer) { }
		/// <summary>
		/// Return weapon
		/// </summary>
		/// <param name="entitySource"></param>
		/// <param name="player"></param>
		/// <param name="LoopAmount"></param>
		/// <param name="rng"></param>
		public void GetWeapon(IEntitySource entitySource, Player player, int LoopAmount, int rng = 0) {
			int SpecialAmount = 200;
			int ReturnWeapon = ItemID.None;
			//adding stuff here
			if (Main.masterMode) {
				SpecialAmount += 150;
			}
			if (UniversalSystem.CanAccessContent(player, UniversalSystem.SYNERGYFEVER_MODE)) {
				int weapon = Main.rand.Next(BossRushModSystem.SynergyItem).type;
				player.QuickSpawnItemDirect(entitySource, weapon);
				AmmoForWeapon(entitySource, player, weapon);
				return;
			}
			ModifyLootAdd(player);
			//actual choosing item
			ChestLootDropPlayer modplayer = player.GetModPlayer<ChestLootDropPlayer>();
			HashSet<int> DummyMeleeData = LootboxSystem.GetItemPool(Type).DropItemMelee.Where(x => !modplayer.ItemGraveYard.Contains(x)).ToHashSet();
			HashSet<int> DummyRangeData = LootboxSystem.GetItemPool(Type).DropItemRange.Where(x => !modplayer.ItemGraveYard.Contains(x)).ToHashSet();
			HashSet<int> DummyMagicData = LootboxSystem.GetItemPool(Type).DropItemMagic.Where(x => !modplayer.ItemGraveYard.Contains(x)).ToHashSet();
			HashSet<int> DummySummonData = LootboxSystem.GetItemPool(Type).DropItemSummon.Where(x => !modplayer.ItemGraveYard.Contains(x)).ToHashSet();
			HashSet<int> DummyMiscsData = LootboxSystem.GetItemPool(Type).DropItemMisc;
			for (int i = 0; i < LoopAmount; i++) {
				rng = RNGManage(player);
				rng = ModifyRNG(rng, player);
				switch (rng) {
					case 0:
						break;
					case 1:
						if (DummyMeleeData.Count <= 0) {
							GetWeapon(out int Weapon, out int Amount, rng);
							player.QuickSpawnItem(entitySource, Weapon, Amount);
							break;
						}
						ReturnWeapon = Main.rand.NextFromHashSet(DummyMeleeData);
						player.QuickSpawnItem(entitySource, ReturnWeapon);
						modplayer.ItemGraveYard.Add(ReturnWeapon);
						DummyMeleeData.Remove(ReturnWeapon);
						break;
					case 2:
						if (DummyRangeData.Count <= 0) {
							GetWeapon(out int Weapon, out int Amount, rng);
							player.QuickSpawnItem(entitySource, Weapon, Amount);
							AmmoForWeapon(entitySource, player, Weapon);
							break;
						}
						ReturnWeapon = Main.rand.NextFromHashSet(DummyRangeData);
						player.QuickSpawnItem(entitySource, ReturnWeapon);
						AmmoForWeapon(entitySource, player, ReturnWeapon);
						modplayer.ItemGraveYard.Add(ReturnWeapon);
						DummyRangeData.Remove(ReturnWeapon);
						break;
					case 3:
						if (DummyMagicData.Count <= 0) {
							GetWeapon(out int Weapon, out int Amount, rng);
							player.QuickSpawnItem(entitySource, Weapon, Amount);
							AmmoForWeapon(entitySource, player, Weapon);
							break;
						}
						ReturnWeapon = Main.rand.NextFromHashSet(DummyMagicData);
						player.QuickSpawnItem(entitySource, ReturnWeapon);
						modplayer.ItemGraveYard.Add(ReturnWeapon);
						DummyMagicData.Remove(ReturnWeapon);
						AmmoForWeapon(entitySource, player, ReturnWeapon);
						break;
					case 4:
						if (DummySummonData.Count <= 0) {
							GetWeapon(out int Weapon, out int Amount, rng);
							player.QuickSpawnItem(entitySource, Weapon, Amount);
							AmmoForWeapon(entitySource, player, Weapon);
							break;
						}
						ReturnWeapon = Main.rand.NextFromHashSet(DummyMagicData);
						player.QuickSpawnItem(entitySource, ReturnWeapon);
						AmmoForWeapon(entitySource, player, ReturnWeapon);
						modplayer.ItemGraveYard.Add(ReturnWeapon);
						DummySummonData.Remove(ReturnWeapon);
						break;
					case 5:
						if (DummyMiscsData.Count < 1) {
							ChooseWeapon(Main.rand.Next(1, 5), ref ReturnWeapon, ref SpecialAmount, DummyMeleeData.ToList(), DummyRangeData.ToList(), DummyMagicData.ToList(), DummySummonData.ToList(), DummyMiscsData.ToList());
							break;
						}
						ReturnWeapon = Main.rand.NextFromHashSet(DummyMiscsData);
						player.QuickSpawnItem(entitySource, ReturnWeapon, SpecialAmount);
						modplayer.ItemGraveYard.Add(ReturnWeapon);
						DummyMiscsData.Remove(ReturnWeapon);
						break;
					case 6:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<WonderDrug>());
						break;
				}
			}
		}
		/// <summary>
		/// Return weapon base on progression
		/// </summary>
		/// <param name="ReturnWeapon"></param>
		/// <param name="Amount"></param>
		/// <param name="rng"></param>
		public static void GetWeapon(out int ReturnWeapon, out int Amount, int rng = 0) {
			if (rng > 6 || rng <= 0) {
				rng = Main.rand.Next(1, 6);
			}
			ReturnWeapon = 0;
			Amount = 1;
			List<int> DropItemMelee = new List<int>();
			List<int> DropItemRange = new List<int>();
			List<int> DropItemMagic = new List<int>();
			List<int> DropItemSummon = new List<int>();
			List<int> DropItemMisc = new List<int>();
			DropItemMelee.AddRange(TerrariaArrayID.MeleePreBoss);
			DropItemRange.AddRange(TerrariaArrayID.RangePreBoss);
			DropItemMagic.AddRange(TerrariaArrayID.MagicPreBoss);
			DropItemSummon.AddRange(TerrariaArrayID.SummonPreBoss);
			DropItemMisc.AddRange(TerrariaArrayID.SpecialPreBoss);
			if (NPC.downedSlimeKing) {
				DropItemMelee.AddRange(TerrariaArrayID.MeleePreEoC);
				DropItemRange.AddRange(TerrariaArrayID.RangePreEoC);
				DropItemMagic.AddRange(TerrariaArrayID.MagicPreEoC);
				DropItemSummon.AddRange(TerrariaArrayID.SummonerPreEoC);
				DropItemMisc.AddRange(TerrariaArrayID.Special);
			}
			if (NPC.downedBoss1) {
				DropItemMelee.Add(ItemID.Code1);
				DropItemMagic.Add(ItemID.ZapinatorGray);
			}
			if (NPC.downedBoss2) {
				DropItemMelee.AddRange(TerrariaArrayID.MeleeEvilBoss);
				DropItemRange.Add(ItemID.MoltenFury);
				DropItemRange.Add(ItemID.StarCannon);
				DropItemRange.Add(ItemID.AleThrowingGlove);
				DropItemRange.Add(ItemID.Harpoon);
				DropItemMagic.AddRange(TerrariaArrayID.MagicEvilBoss);
				DropItemSummon.Add(ItemID.ImpStaff);
			}
			if (NPC.downedBoss3) {
				DropItemMelee.AddRange(TerrariaArrayID.MeleeSkel);
				DropItemRange.AddRange(TerrariaArrayID.RangeSkele);
				DropItemMagic.AddRange(TerrariaArrayID.MagicSkele);
				DropItemSummon.AddRange(TerrariaArrayID.SummonSkele);
			}
			if (NPC.downedQueenBee) {
				DropItemMelee.Add(ItemID.BeeKeeper);
				DropItemRange.Add(ItemID.BeesKnees); DropItemRange.Add(ItemID.Blowgun);
				DropItemMagic.Add(ItemID.BeeGun);
				DropItemSummon.Add(ItemID.HornetStaff);
				DropItemMisc.Add(ItemID.Beenade);
			}
			if (NPC.downedDeerclops) {
				DropItemRange.Add(ItemID.PewMaticHorn);
				DropItemMagic.Add(ItemID.WeatherPain);
				DropItemSummon.Add(ItemID.HoundiusShootius);
			}
			if (Main.hardMode) {
				DropItemMelee.AddRange(TerrariaArrayID.MeleeHM);
				DropItemRange.AddRange(TerrariaArrayID.RangeHM);
				DropItemMagic.AddRange(TerrariaArrayID.MagicHM);
				DropItemSummon.AddRange(TerrariaArrayID.SummonHM);
			}
			if (NPC.downedQueenSlime) {
				DropItemMelee.AddRange(TerrariaArrayID.MeleeQS);
				DropItemSummon.Add(ItemID.Smolstar);
			}
			if (NPC.downedMechBossAny) {
				DropItemMelee.AddRange(TerrariaArrayID.MeleeMech);
				DropItemRange.Add(ItemID.SuperStarCannon);
				DropItemRange.Add(ItemID.DD2PhoenixBow);
				DropItemMagic.Add(ItemID.UnholyTrident);
			}
			if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) {
				DropItemMelee.AddRange(TerrariaArrayID.MeleePostAllMechs);
				DropItemRange.AddRange(TerrariaArrayID.RangePostAllMech);
				DropItemMagic.AddRange(TerrariaArrayID.MagicPostAllMech);
				DropItemSummon.AddRange(TerrariaArrayID.SummonPostAllMech);
			}
			if (NPC.downedPlantBoss) {
				DropItemMelee.AddRange(TerrariaArrayID.MeleePostPlant);
				DropItemRange.AddRange(TerrariaArrayID.RangePostPlant);
				DropItemMagic.AddRange(TerrariaArrayID.MagicPostPlant);
				DropItemSummon.AddRange(TerrariaArrayID.SummonPostPlant);
			}
			if (NPC.downedGolemBoss) {
				DropItemMelee.AddRange(TerrariaArrayID.MeleePostGolem);
				DropItemRange.AddRange(TerrariaArrayID.RangePostGolem);
				DropItemMagic.AddRange(TerrariaArrayID.MagicPostGolem);
				DropItemSummon.AddRange(TerrariaArrayID.SummonPostGolem);
			}
			if (NPC.downedEmpressOfLight) {
				DropItemMelee.AddRange(TerrariaArrayID.MeleePreLuna);
				DropItemRange.AddRange(TerrariaArrayID.RangePreLuna);
				DropItemMagic.AddRange(TerrariaArrayID.MagicPreLuna);
				DropItemSummon.AddRange(TerrariaArrayID.SummonPreLuna);
			}
			if (NPC.downedAncientCultist) {
				DropItemMelee.Add(ItemID.DayBreak);
				DropItemMelee.Add(ItemID.SolarEruption);
				DropItemRange.Add(ItemID.Phantasm);
				DropItemRange.Add(ItemID.VortexBeater);
				DropItemMagic.Add(ItemID.NebulaArcanum);
				DropItemMagic.Add(ItemID.NebulaBlaze);
				DropItemSummon.Add(ItemID.StardustCellStaff);
				DropItemSummon.Add(ItemID.StardustDragonStaff);
			}
			if (NPC.downedMoonlord) {
				DropItemMelee.Add(ItemID.StarWrath);
				DropItemMelee.Add(ItemID.Meowmere);
				DropItemMelee.Add(ItemID.Terrarian);
				DropItemRange.Add(ItemID.SDMG);
				DropItemRange.Add(ItemID.Celeb2);
				DropItemMagic.Add(ItemID.LunarFlareBook);
				DropItemMagic.Add(ItemID.LastPrism);
				DropItemSummon.Add(ItemID.RainbowCrystalStaff);
				DropItemSummon.Add(ItemID.MoonlordTurretStaff);
			}
			ChooseWeapon(rng, ref ReturnWeapon, ref Amount, DropItemMelee, DropItemRange, DropItemMagic, DropItemSummon, DropItemMisc);
		}
		private static void ChooseWeapon(int rng, ref int weapon, ref int amount, List<int> DropItemMelee, List<int> DropItemRange, List<int> DropItemMagic, List<int> DropItemSummon, List<int> DropItemMisc) {
			switch (rng) {
				case 0:
					weapon = ItemID.None;
					break;
				case 1:
					weapon = Main.rand.NextFromCollection(DropItemMelee);
					break;
				case 2:
					weapon = Main.rand.NextFromCollection(DropItemRange);
					break;
				case 3:
					weapon = Main.rand.NextFromCollection(DropItemMagic);
					break;
				case 4:
					weapon = Main.rand.NextFromCollection(DropItemSummon);
					break;
				case 5:
					if (DropItemMisc.Count < 1) {
						int rngM = Main.rand.Next(1, 5);
						ChooseWeapon(rngM, ref weapon, ref amount, DropItemMelee, DropItemRange, DropItemMagic, DropItemSummon, DropItemMisc);
						break;
					}
					amount += 199;
					weapon = Main.rand.NextFromCollection(DropItemMisc);
					break;
			}
		}
		List<int> DropArrowAmmo = new List<int>();
		List<int> DropBulletAmmo = new List<int>();
		List<int> DropDartAmmo = new List<int>();

		/// <summary>
		/// Automatically quick drop player ammo item accordingly to weapon ammo type
		/// </summary>
		/// /// <param name="player">The player</param>
		/// <param name="weapon">Weapon need to be checked</param>
		/// <param name="AmountModifier">Modify the ammount of ammo will be given</param>
		public void AmmoForWeapon(IEntitySource source, Player player, int weapon, float AmountModifier = 1) {
			Item weapontoCheck = ContentSamples.ItemsByType[weapon];
			if (weapontoCheck.consumable || weapontoCheck.useAmmo == AmmoID.None) {
				if (weapontoCheck.mana > 0) {
					player.QuickSpawnItem(source, ItemID.LesserManaPotion, 5);
				}
				return;
			}
			//The most ugly code
			int Amount = (int)(200 * AmountModifier);
			int Ammo;
			if (Main.masterMode) {
				Amount += 150;
			}
			DropArrowAmmo.Clear();
			DropBulletAmmo.Clear();
			DropDartAmmo.Clear();

			DropArrowAmmo.AddRange(TerrariaArrayID.defaultArrow);
			DropBulletAmmo.AddRange(TerrariaArrayID.defaultBullet);
			DropDartAmmo.AddRange(TerrariaArrayID.defaultDart);

			if (Main.hardMode) {
				DropArrowAmmo.AddRange(TerrariaArrayID.ArrowHM);
				DropBulletAmmo.AddRange(TerrariaArrayID.BulletHM);
				DropDartAmmo.AddRange(TerrariaArrayID.DartHM);
			}
			if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) {
				DropArrowAmmo.Add(ItemID.ChlorophyteArrow);
				DropBulletAmmo.Add(ItemID.ChlorophyteBullet);
			}
			if (NPC.downedPlantBoss) {
				DropArrowAmmo.Add(ItemID.VenomArrow);
				DropBulletAmmo.Add(ItemID.NanoBullet);
				DropBulletAmmo.Add(ItemID.VenomBullet);
			}
			if (weapontoCheck.useAmmo == AmmoID.Arrow) {
				Ammo = Main.rand.NextFromCollection(DropArrowAmmo);
			}
			else if (weapontoCheck.useAmmo == AmmoID.Bullet) {
				Ammo = Main.rand.NextFromCollection(DropBulletAmmo);
			}
			else if (weapontoCheck.useAmmo == AmmoID.Dart) {
				Ammo = Main.rand.NextFromCollection(DropDartAmmo);
			}
			else if (weapontoCheck.mana > 0) {
				Ammo = ItemID.LesserManaPotion;
				Amount = (int)(10 * AmountModifier);
			}
			else if (weapontoCheck.useAmmo == AmmoID.Rocket) {
				Ammo = Main.rand.Next(new int[] { ItemID.RocketI,ItemID.RocketII, ItemID.RocketIII, ItemID.RocketIV });
			}
			else if (weapontoCheck.useAmmo == AmmoID.Snowball) {
				Ammo = ItemID.Snowball;
			}
			else if (weapontoCheck.useAmmo == AmmoID.CandyCorn) {
				Ammo = ItemID.CandyCorn;
			}
			else if (weapontoCheck.useAmmo == AmmoID.JackOLantern) {
				Ammo = ItemID.JackOLantern;
			}
			else if (weapontoCheck.useAmmo == AmmoID.Flare) {
				Ammo = ItemID.Flare;
			}
			else if (weapontoCheck.useAmmo == AmmoID.Stake) {
				Ammo = ItemID.Stake;
			}
			else if (weapontoCheck.useAmmo == AmmoID.StyngerBolt) {
				Ammo = ItemID.StyngerBolt;
			}
			else if (weapontoCheck.useAmmo == AmmoID.NailFriendly) {
				Ammo = ItemID.Nail;
			}
			else if (weapontoCheck.useAmmo == AmmoID.Gel) {
				Ammo = ItemID.Gel;
			}
			else if (weapontoCheck.useAmmo == AmmoID.FallenStar) {
				Ammo = ModContent.ItemType<StarTreasureChest>();
				if (Amount < 1) {
					Amount = 1;
				}
				else if (Amount > 1) {
					Amount = (int)(1 * AmountModifier);
				}
			}
			else if (weapontoCheck.useAmmo == AmmoID.Sand) {
				Ammo = ItemID.SandBlock;
			}
			else {
				Ammo = ItemID.WoodenArrow;
			}
			player.QuickSpawnItem(source, Ammo, Amount);
		}

		List<int> Accessories = new List<int>();
		/// <summary>
		///      Allow user to return a list of number that contain different data to insert into chest <br/>
		///      0 : Tier 1 Combat acc <br/>
		///      1 : Tier 1 Health and Mana acc<br/>
		///      2 : Tier 1 Movement acc<br/>
		///      3 : Post evil Combat acc<br/>
		///      4 : Post evil Health and Mana acc<br/>
		///      5 : Post evil Movement acc<br/>
		///      6 : Queen bee acc<br/>
		///      7 : Cobalt Shield<br/>
		///      8 : Anhk shield sub acc (not include the shield itself)<br/>
		///      9 : Hardmode acc<br/>
		///      10 : PhilosophersStone<br/>
		/// </summary>
		public virtual List<int> FlagNumAcc() => new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

		/// <summary>
		///      Allow user to return a list of number that contain different data to insert into chest <br/>
		///      0 : Tier 1 Combat acc <br/>
		///      1 : Tier 1 Health and Mana acc<br/>
		///      2 : Tier 1 Movement acc<br/>
		///      3 : Post evil Combat acc<br/>
		///      4 : Post evil Health and Mana acc<br/>
		///      5 : Post evil Movement acc<br/>
		///      6 : Queen bee acc<br/>
		///      7 : Cobalt Shield<br/>
		///      8 : Anhk shield sub acc (not include the shield itself)<br/>
		///      9 : Hardmode acc<br/>
		///      10 : PhilosophersStone<br/>
		/// </summary>
		public virtual List<int> FlagNumAcc(List<int> listofNum) => listofNum;

		/// <summary>
		/// Allow for safely add in a list of accessory that specific to a situation
		/// </summary>
		/// <returns></returns>
		public virtual List<int> SafePostAddAcc() => new List<int>() { };

		private void AddAcc(List<int> flag) {
			Accessories.AddRange(BossRushModSystem.LostAccessories.Select(i => i.type));
			for (int i = 0; i < flag.Count; i++) {
				switch (flag[i]) {
					case 0:
						Accessories.AddRange(TerrariaArrayID.T1CombatAccessory);
						break;
					case 1:
						Accessories.AddRange(TerrariaArrayID.T1HealthAndManaAccessory);
						break;
					case 2:
						Accessories.AddRange(TerrariaArrayID.T1MovementAccessory);
						break;
					case 3:
						Accessories.AddRange(TerrariaArrayID.PostEvilCombatAccessory);
						break;
					case 4:
						Accessories.AddRange(TerrariaArrayID.PostEvilHealthManaAccessory);
						break;
					case 5:
						Accessories.AddRange(TerrariaArrayID.PostEvilMovementAccessory);
						break;
					case 6:
						Accessories.AddRange(TerrariaArrayID.QueenBeeCombatAccessory);
						break;
					case 7:
						Accessories.Add(ItemID.CobaltShield);
						break;
					case 8:
						Accessories.AddRange(TerrariaArrayID.AnhkCharm);
						break;
					case 9:
						Accessories.AddRange(TerrariaArrayID.HMAccessory);
						break;
					case 10:
						Accessories.Add(ItemID.PhilosophersStone);
						break;
				}
				if (SafePostAddAcc().Count > 0) Accessories.AddRange(SafePostAddAcc());
			}
		}
		/// <summary>
		/// This happen automatically for the sake of prototype
		/// </summary>
		public void GetArmorForPlayer(IEntitySource entitySource, Player player) {
			List<int> HeadArmor = new List<int>();
			List<int> BodyArmor = new List<int>();
			List<int> LegArmor = new List<int>();
			HeadArmor.AddRange(TerrariaArrayID.HeadArmorPreBoss);
			BodyArmor.AddRange(TerrariaArrayID.BodyArmorPreBoss);
			LegArmor.AddRange(TerrariaArrayID.LegArmorPreBoss);
			if (NPC.downedBoss2) {
				HeadArmor.AddRange(TerrariaArrayID.HeadArmorPostEvil);
				BodyArmor.AddRange(TerrariaArrayID.BodyArmorPostEvil);
				LegArmor.AddRange(TerrariaArrayID.LegArmorPostEvil);
			}
			if (NPC.downedBoss3) {
				HeadArmor.Add(ItemID.NecroHelmet);
				BodyArmor.Add(ItemID.NecroBreastplate);
				LegArmor.Add(ItemID.NecroGreaves);
			}
			if (NPC.downedQueenBee) {
				HeadArmor.Add(ItemID.BeeHeadgear);
				BodyArmor.Add(ItemID.BeeBreastplate);
				LegArmor.Add(ItemID.BeeGreaves);
			}
			int[] fullbodyarmor = new int[]{
				Main.rand.Next(HeadArmor),
				Main.rand.Next(BodyArmor),
				Main.rand.Next(LegArmor) };
			for (int i = 0; i < fullbodyarmor.Length; i++) {
				player.QuickSpawnItem(entitySource, fullbodyarmor[i]);
			}
		}
		/// <summary>
		/// Return a random accessory 
		/// </summary>
		public int GetAccessory() {
			AddAcc(FlagNumAcc());
			return Main.rand.NextFromCollection(Accessories);
		}
		List<int> DropItemPotion = new List<int>();
		/// <summary>
		/// Return random potion
		/// </summary>
		/// <param name="MovementPotionOnly">Allow potion that enhance movement to be drop</param>
		public int GetPotion(bool MovementPotionOnly = false) {
			DropItemPotion.AddRange(TerrariaArrayID.NonMovementPotion);
			if (Main.hardMode) {
				DropItemPotion.Add(ItemID.LifeforcePotion);
				DropItemPotion.Add(ItemID.InfernoPotion);
			}
			if (MovementPotionOnly) {
				DropItemPotion.Clear();
			}
			DropItemPotion.AddRange(TerrariaArrayID.MovementPotion);
			return Main.rand.NextFromCollection(DropItemPotion);
		}
		Color color1, color2, color3, color4;
		private void ColorHandle() {
			color1 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 20);
			color2 = new Color(Main.DiscoG, Main.DiscoB, Main.DiscoR, 20);
			color3 = new Color(Main.DiscoB, Main.DiscoR, Main.DiscoG, 20);
			color4 = new Color(Main.DiscoG, Main.DiscoR, Main.DiscoB, 20);
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
			ColorHandle();
			Main.instance.LoadItem(Item.type);
			Texture2D texture = TextureAssets.Item[Item.type].Value;
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, position + new Vector2(2, 2), null, color1, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-2, 2), null, color2, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(2, -2), null, color3, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-2, -2), null, color4, 0, origin, scale, SpriteEffects.None, 0);
			}
			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
			ColorHandle();
			//if (Item.whoAmI != whoAmI)
			//{
			//    return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
			//}
			Main.instance.LoadItem(Item.type);
			Texture2D texture = TextureAssets.Item[Item.type].Value;
			Vector2 origin = new Vector2(38, 30);
			Vector2 drawPos = Item.Center - Main.screenPosition - origin * .5f;
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, drawPos + new Vector2(2, 2), null, color1, rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos + new Vector2(-2, 2), null, color2, rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos + new Vector2(2, -2), null, color3, rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos + new Vector2(-2, -2), null, color4, rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
			}
			return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
		}
	}
	public class LootboxSystem : ModSystem {
		private static List<LootBoxItemPool> LootBoxDropPool = new List<LootBoxItemPool>();
		/// <summary>
		/// Direct modify maybe unstable, unsure how this will work <br/>
		/// For a more direct way of modify, please refer to <see cref="ReplaceItemPool(LootBoxItemPool)"/>
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static LootBoxItemPool GetItemPool(int type) => LootBoxDropPool.Where(i => i.PoolID == type).FirstOrDefault();
		public static void AddItemPool(LootBoxItemPool pool) {
			pool.IndexID = LootBoxDropPool.Count;
			LootBoxDropPool.Add(pool);
		}
		public static void ReplaceItemPool(LootBoxItemPool pool) {
			LootBoxItemPool itempool = LootBoxDropPool.Where(i => i.PoolID == pool.PoolID).FirstOrDefault();
			int index = itempool.IndexID;
			LootBoxDropPool[index] = pool;
			LootBoxDropPool[index].IndexID = index;
		}
		public override void Unload() {
			LootBoxDropPool = null;
		}
	}
	public class LootBoxItemPool {
		public LootBoxItemPool(int ID) {
			_poolid = ID;
		}
		int _poolid = 0;
		/// <summary>
		/// Return the Item ID that own this <see cref="LootBoxItemPool"/>
		/// </summary>
		public int PoolID { get => _poolid; }
		/// <summary>
		/// Return the index of <see cref="LootBoxItemPool"/> that current at
		/// </summary>
		public int IndexID = -1;
		public HashSet<int> DropItemMelee = new HashSet<int>();
		public HashSet<int> DropItemRange = new HashSet<int>();
		public HashSet<int> DropItemMagic = new HashSet<int>();
		public HashSet<int> DropItemSummon = new HashSet<int>();
		public HashSet<int> DropItemMisc = new HashSet<int>();

		private int _cachedAllItemCount => _cachedAllItems.Count;
		private HashSet<int> _cachedAllItems = null;
		/// <summary>
		/// Call this when you know it will get update 
		/// </summary>
		public void UpdateAllItemPool() {
			_cachedAllItems = new HashSet<int>();
			_cachedAllItems.UnionWith(DropItemMelee);
			_cachedAllItems.UnionWith(DropItemRange);
			_cachedAllItems.UnionWith(DropItemMagic);
			_cachedAllItems.UnionWith(DropItemSummon);
			_cachedAllItems.UnionWith(DropItemMisc);
		}

		public HashSet<int> AllItemPool() {
			if (_cachedAllItems == null) {
				UpdateAllItemPool();
			}
			else if (DropItemMagic.Count + DropItemRange.Count + DropItemMagic.Count + DropItemSummon.Count + DropItemMisc.Count != _cachedAllItemCount) {
				UpdateAllItemPool();
			}

			return _cachedAllItems;
		}
	}
	public class ChestLootDropPlayer : ModPlayer {
		public int counterShow = 0;
		public int weaponShowID = 0, potionShowID = 0;

		public HashSet<int> ItemGraveYard = new HashSet<int>();
		public bool CanDropSynergyEnergy = true;
		public int CurrentSectionAmountOfChestOpen = 0;
		//To ensure this is save and predictable and more easily customizable, create your own modplayer class and save this data itself
		//Alternatively we can use this to handle all the data itself

		//This is global modifier ( aka amount modifier to all )
		public float finalMultiplier = 1f;
		public int amountModifier = 0;

		//This is inner modifier ( aka amount modifier to x stuff )
		/// <summary>
		/// Use this if it is a always update item
		/// </summary>
		public int WeaponAmountAddition = 0;
		/// <summary>
		/// Use this if it is a always update item
		/// </summary>
		public int PotionTypeAmountAddition = 0;
		/// <summary>
		/// Use this if it is a always update item
		/// </summary>
		public int PotionNumberAmountAddition = 0;
		//Do not touch this
		public int weaponAmount;
		public int potionTypeAmount;
		public int potionNumAmount;
		/// <summary>
		/// Use this if it is consumable
		/// </summary>
		public float MeleeChanceMutilplier = 1f;
		/// <summary>
		/// Use this if it is consumable
		/// </summary>
		public float RangeChanceMutilplier = 1f;
		/// <summary>
		/// Use this if it is consumable
		/// </summary>
		public float MagicChanceMutilplier = 1f;
		/// <summary>
		/// Use this if it is consumable
		/// </summary>
		public float SummonChanceMutilplier = 1f;
		/// <summary>
		/// Use this if you gonna always update it
		/// </summary>
		public float UpdateMeleeChanceMutilplier = 0;
		/// <summary>
		/// Use this if you gonna always update it
		/// </summary>
		public float UpdateRangeChanceMutilplier = 0;
		/// <summary>
		/// Use this if you gonna always update it
		/// </summary>
		public float UpdateMagicChanceMutilplier = 0;
		/// <summary>
		/// Use this if you gonna always update it
		/// </summary>
		public float UpdateSummonChanceMutilplier = 0;
		public bool LootboxCanDropSpecialPotion = false;
		private int ModifyGetAmount(int ValueToModify) => finalMultiplier > 0 ? (int)Math.Ceiling(finalMultiplier * (ValueToModify + amountModifier)) : 1;
		/// <summary>
		/// This must be called before using
		/// <br/><see cref="weaponAmount"/>
		/// <br/><see cref="potionTypeAmount"/>
		/// <br/><see cref="potionNumAmount"/>
		/// </summary>
		public void GetAmount() {
			weaponAmount = 5;
			potionTypeAmount = 3;
			potionNumAmount = 4;
			if (Main.ActiveWorldFileData.GameMode != 0) {
				weaponAmount -= 2;
				potionTypeAmount -= 2;
				potionNumAmount -= 2;
			}
			if (Main.getGoodWorld) {
				weaponAmount = 2;
				potionTypeAmount = 1;
				potionNumAmount = 1;
			}
			weaponAmount = Math.Clamp(ModifyGetAmount(weaponAmount  + WeaponAmountAddition), 1, 999999);
			potionTypeAmount = ModifyGetAmount(potionTypeAmount  + PotionTypeAmountAddition);
			potionNumAmount = ModifyGetAmount(potionNumAmount  + PotionNumberAmountAddition);
			if (ModContent.GetInstance<BossRushModConfig>().SynergyFeverMode && ModContent.GetInstance<BossRushModConfig>().SynergyMode) {
				weaponAmount = 1;
			}
		}
		public override void ResetEffects() {
			LootboxCanDropSpecialPotion = false;
			CanDropSynergyEnergy = false;
			amountModifier = 0;
			finalMultiplier = 1f;
			WeaponAmountAddition = 0;
			PotionTypeAmountAddition = 0;
			PotionNumberAmountAddition = 0;
			UpdateMeleeChanceMutilplier = 0;
			UpdateRangeChanceMutilplier = 0;
			UpdateMagicChanceMutilplier = 0;
			UpdateSummonChanceMutilplier = 0;
		}
		public override void Unload() {
			ItemGraveYard = null;
		}
		public override void Initialize() {
			MeleeChanceMutilplier = 1f;
			RangeChanceMutilplier = 1f;
			MagicChanceMutilplier = 1f;
			SummonChanceMutilplier = 1f;
		}
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)BossRush.MessageType.ChanceMultiplayer);
			packet.Write((byte)Player.whoAmI);
			packet.Write(MeleeChanceMutilplier);
			packet.Write(RangeChanceMutilplier);
			packet.Write(MagicChanceMutilplier);
			packet.Write(SummonChanceMutilplier);
			packet.Send(toWho, fromWho);
		}
		public override void SaveData(TagCompound tag) {
			tag["MeleeChanceMulti"] = MeleeChanceMutilplier;
			tag["RangeChanceMulti"] = RangeChanceMutilplier;
			tag["MagicChanceMulti"] = MagicChanceMutilplier;
			tag["SummonChanceMulti"] = SummonChanceMutilplier;
		}
		public override void LoadData(TagCompound tag) {
			MeleeChanceMutilplier = (float)tag["MeleeChanceMulti"];
			RangeChanceMutilplier = (float)tag["RangeChanceMulti"];
			MagicChanceMutilplier = (float)tag["MagicChanceMulti"];
			SummonChanceMutilplier = (float)tag["SummonChanceMulti"];
		}
		public void ReceivePlayerSync(BinaryReader reader) {
			MeleeChanceMutilplier = reader.ReadSingle();
			RangeChanceMutilplier = reader.ReadSingle();
			MagicChanceMutilplier = reader.ReadSingle();
			SummonChanceMutilplier = reader.ReadSingle();
		}

		public override void CopyClientState(ModPlayer targetCopy) {
			ChestLootDropPlayer clone = (ChestLootDropPlayer)targetCopy;
			clone.MeleeChanceMutilplier = MeleeChanceMutilplier;
			clone.RangeChanceMutilplier = RangeChanceMutilplier;
			clone.MagicChanceMutilplier = MagicChanceMutilplier;
			clone.SummonChanceMutilplier = SummonChanceMutilplier;
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
			ChestLootDropPlayer clone = (ChestLootDropPlayer)clientPlayer;
			if (MeleeChanceMutilplier != clone.MeleeChanceMutilplier
			|| RangeChanceMutilplier != clone.RangeChanceMutilplier
			 || MagicChanceMutilplier != clone.MagicChanceMutilplier
			 || SummonChanceMutilplier != clone.SummonChanceMutilplier
			 )
				SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}
	}
}
