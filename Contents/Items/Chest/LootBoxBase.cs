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
using Terraria.DataStructures;
using System.Collections.Generic;
using BossRush.Contents.Items.Card;
using BossRush.Contents.Items.Potion;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Contents.Items.NohitReward;
using BossRush.Common.Systems;

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
				chestplayer.GetAmount();
				List<int> potiontotal = new List<int>();
				potiontotal.AddRange(TerrariaArrayID.NonMovementPotion);
				potiontotal.AddRange(TerrariaArrayID.MovementPotion);
				TooltipLine chestline = new TooltipLine(Mod, "ChestLoot",
					$"Weapon : [i:{Main.rand.NextFromHashSet(LootboxSystem.GetItemPool(Type).AllItemPool())}] x {chestplayer.weaponAmount}\n" +
					$"Potion type : [i:{Main.rand.Next(potiontotal)}] x {chestplayer.potionTypeAmount}\n" +
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
		//Note this method stillsuffer from slight performance problem
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
			if (ItemGraveYard.Count > 0) {
				int RemoveAmount = 1 + ItemGraveYard.Count / 10;
				for (int i = 0; i < RemoveAmount; i++) {
					int index = Main.rand.Next(ItemGraveYard.Count);
					Main.NewText($"[DEBUG] Removing [i:{ItemGraveYard.ElementAt(index)}] from item graveyard");
					ItemGraveYard.Remove(ItemGraveYard.ElementAt(index));
				}
			}
			OnRightClick(player, modplayer);
			if (!UniversalSystem.CanAccessContent(player, UniversalSystem.SYNERGY_MODE)) {
				return;
			}
			var entitySource = player.GetSource_OpenItem(Type);
			if (modplayer.CanDropSynergyEnergy)
				player.QuickSpawnItem(entitySource, ModContent.ItemType<SynergyEnergy>());

			//This is very bulky but gotta make it work
			if (Main.rand.NextBool(20)) {
				player.QuickSpawnItem(entitySource, Main.rand.Next(TerrariaArrayID.Trinket));
			}
			PlayerStatsHandle cardplayer = player.GetModPlayer<PlayerStatsHandle>();
			int cardReRoll = (int)Math.Round(cardplayer.CardLuck * .1f, 2);
			for (int i = 0; i < cardReRoll; i++) {
				if (Main.rand.NextFloat() * 100 < cardplayer.CardLuck) {
					player.QuickSpawnItem(entitySource, ModContent.ItemType<BoxOfCard>());
					continue;
				}
				if (Main.rand.NextFloat() * 100 < cardplayer.CardLuck * 2) {
					player.QuickSpawnItem(entitySource, ModContent.ItemType<BigCardPacket>());
					continue;
				}
				player.QuickSpawnItem(entitySource, ModContent.ItemType<CardPacket>());
			}
		}
		public virtual void OnRightClick(Player player, ChestLootDropPlayer modplayer) { }
		private static HashSet<int> ItemGraveYard = new HashSet<int>();
		/// <summary>
		/// Return a random weapon and if the weapon consumable then return many ammount
		/// </summary>
		/// <param name="player">Player player</param>
		/// <param name="ReturnWeapon">Weapon get return</param>
		/// <param name="specialAmount">Ammount of that weapon get return</param>
		/// <param name="rng">Set the rng number to return a specific type of weapon
		/// <br/>1 : Melee weapon
		/// <br/>2 : Range weapon
		/// <br/>3 : Magic weapon
		/// <br/> 4 : Summon weapon
		/// <br/>5 : Misc weapon
		/// <br/> 6 : Drug
		/// <br/> 7 : Rainbow Chest</param>
		public void GetWeapon(Player player, out int ReturnWeapon, out int specialAmount, int rng = 0) {
			specialAmount = 1;
			ReturnWeapon = ItemID.None;
			if (rng == 0) {
				rng = RNGManage(player);
			}
			rng = ModifyRNG(rng, player);
			//adding stuff here
			if (rng < 6 && rng > 0) {
				ModifyLootAdd(player);
			}
			//actual choosing item
			ChooseWeapon(rng, ref ReturnWeapon, ref specialAmount);
		}
		public void ChooseWeapon(int rng, ref int weapon, ref int amount) {
			weapon = ItemID.None;
			amount = 1;
			HashSet<int> DummyMeleeData = LootboxSystem.GetItemPool(Type).DropItemMelee.Where(x => !ItemGraveYard.Contains(x)).ToHashSet();
			HashSet<int> DummyRangeData = LootboxSystem.GetItemPool(Type).DropItemRange.Where(x => !ItemGraveYard.Contains(x)).ToHashSet();
			HashSet<int> DummyMagicData = LootboxSystem.GetItemPool(Type).DropItemMagic.Where(x => !ItemGraveYard.Contains(x)).ToHashSet();
			HashSet<int> DummySummonData = LootboxSystem.GetItemPool(Type).DropItemSummon.Where(x => !ItemGraveYard.Contains(x)).ToHashSet();
			HashSet<int> DummyMiscsData = LootboxSystem.GetItemPool(Type).DropItemMisc;
			switch (rng) {
				case 0:
					weapon = ItemID.None;
					return;
				case 1:
					if (DummyMeleeData.Count <= 0) {
						GetWeapon(out int Weapon, out int Amount, rng);
						weapon = Weapon;
						amount = Amount;
						return;
					}
					weapon = DummyMeleeData.ElementAt(Main.rand.Next(DummyMeleeData.Count));
					ItemGraveYard.Add(weapon);
					return;
				case 2:
					if (DummyRangeData.Count <= 0) {
						GetWeapon(out int Weapon, out int Amount, rng);
						weapon = Weapon;
						amount = Amount;
						return;
					}
					weapon = DummyRangeData.ElementAt(Main.rand.Next(DummyRangeData.Count));
					ItemGraveYard.Add(weapon);
					return;
				case 3:
					if (DummyMagicData.Count <= 0) {
						GetWeapon(out int Weapon, out int Amount, rng);
						weapon = Weapon;
						amount = Amount;
						return;
					}
					weapon = DummyMagicData.ElementAt(Main.rand.Next(DummyMagicData.Count));
					ItemGraveYard.Add(weapon);
					return;
				case 4:
					if (DummySummonData.Count <= 0) {
						GetWeapon(out int Weapon, out int Amount, rng);
						weapon = Weapon;
						amount = Amount;
						return;
					}
					weapon = DummySummonData.ElementAt(Main.rand.Next(DummySummonData.Count));
					ItemGraveYard.Add(weapon);
					return;
				case 5:
					if (DummyMiscsData.Count < 1) {
						ChooseWeapon(Main.rand.Next(1, 5), ref weapon, ref amount);
						break;
					}
					amount += 199;
					if (Main.masterMode) {
						amount += 150;
					}
					weapon = DummyMiscsData.ElementAt(Main.rand.Next(DummyMiscsData.Count));
					return;
				case 6:
					weapon = ModContent.ItemType<WonderDrug>();
					return;
			}
		}
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
			if (NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3) {
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
		/// Return ammo of weapon accordingly to weapon parameter
		/// </summary>
		/// <param name="Ammo">Return ammo type accordingly to weapon type</param>
		/// <param name="Amount">Return the ammount of ammo</param>
		/// <param name="weapon">Weapon need to be checked</param>
		/// <param name="AmountModifier">Modify the ammount of ammo will be given</param>
		public void AmmoForWeapon(out int Ammo, out int Amount, int weapon, float AmountModifier = 1) {
			Amount = (int)(200 * AmountModifier);
			Item weapontoCheck = new Item(weapon);
			if (Main.masterMode) {
				Amount += 150;
			}
			if (weapontoCheck.consumable) {
				Ammo = ItemID.WoodenArrow;
				return;
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
				Ammo = ItemID.RocketI;
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
		public override void Unload() {
			ItemGraveYard = null;
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
		public bool CanDropSynergyEnergy = true;
		public int CurrentSectionAmountOfChestOpen = 0;
		public override void OnEnterWorld() {
		}
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
		/// <summary>
		/// Use this if it is a consumable item
		/// </summary>
		public int ModifyWeaponAmountAddition = 0;
		/// <summary>
		/// Use this if it is a consumable item
		/// </summary>
		public int ModifyPotionTypeAmountAddition = 0;
		/// <summary>
		/// Use this if it is a consumable item
		/// </summary>
		public int ModifyPotionNumberAmountAddition = 0;
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
			weaponAmount = Math.Clamp(ModifyGetAmount(weaponAmount + ModifyWeaponAmountAddition + WeaponAmountAddition), 1, 999999);
			potionTypeAmount = ModifyGetAmount(potionTypeAmount + ModifyPotionTypeAmountAddition + PotionTypeAmountAddition);
			potionNumAmount = ModifyGetAmount(potionNumAmount + ModifyPotionNumberAmountAddition + PotionNumberAmountAddition);
		}
		public override void ResetEffects() {
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
		public override void Initialize() {
			MeleeChanceMutilplier = 1f;
			RangeChanceMutilplier = 1f;
			MagicChanceMutilplier = 1f;
			SummonChanceMutilplier = 1f;
			ModifyWeaponAmountAddition = 0;
			ModifyPotionTypeAmountAddition = 0;
			ModifyPotionNumberAmountAddition = 0;
		}
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)BossRush.MessageType.ChanceMultiplayer);
			packet.Write((byte)Player.whoAmI);
			packet.Write(MeleeChanceMutilplier);
			packet.Write(RangeChanceMutilplier);
			packet.Write(MagicChanceMutilplier);
			packet.Write(SummonChanceMutilplier);
			packet.Write(ModifyWeaponAmountAddition);
			packet.Write(ModifyPotionTypeAmountAddition);
			packet.Write(ModifyPotionNumberAmountAddition);
			packet.Send(toWho, fromWho);
		}
		public override void SaveData(TagCompound tag) {
			tag["MeleeChanceMulti"] = MeleeChanceMutilplier;
			tag["RangeChanceMulti"] = RangeChanceMutilplier;
			tag["MagicChanceMulti"] = MagicChanceMutilplier;
			tag["SummonChanceMulti"] = SummonChanceMutilplier;
			tag["ModifyWeaponAmountAddition"] = ModifyWeaponAmountAddition;
			tag["ModifyPotionTypeAmountAddition"] = ModifyPotionTypeAmountAddition;
			tag["ModifyPotionNumberAmountAddition"] = ModifyPotionNumberAmountAddition;
		}
		public override void LoadData(TagCompound tag) {
			MeleeChanceMutilplier = (float)tag["MeleeChanceMulti"];
			RangeChanceMutilplier = (float)tag["RangeChanceMulti"];
			MagicChanceMutilplier = (float)tag["MagicChanceMulti"];
			SummonChanceMutilplier = (float)tag["SummonChanceMulti"];
			try {
				ModifyWeaponAmountAddition = (int)tag["ModifyWeaponAmountAddition"];
				ModifyPotionTypeAmountAddition = (int)tag["ModifyPotionTypeAmountAddition"];
				ModifyPotionNumberAmountAddition = (int)tag["ModifyPotionNumberAmountAddition"];
			}
			catch (Exception ex) {
				Main.NewText(ex.Message, Color.Red);
			}
		}
		public void ReceivePlayerSync(BinaryReader reader) {
			MeleeChanceMutilplier = reader.ReadSingle();
			RangeChanceMutilplier = reader.ReadSingle();
			MagicChanceMutilplier = reader.ReadSingle();
			SummonChanceMutilplier = reader.ReadSingle();
			ModifyWeaponAmountAddition = reader.ReadInt32();
			ModifyPotionTypeAmountAddition = reader.ReadInt32();
			ModifyPotionNumberAmountAddition = reader.ReadInt32();
		}

		public override void CopyClientState(ModPlayer targetCopy) {
			ChestLootDropPlayer clone = (ChestLootDropPlayer)targetCopy;
			clone.MeleeChanceMutilplier = MeleeChanceMutilplier;
			clone.RangeChanceMutilplier = RangeChanceMutilplier;
			clone.MagicChanceMutilplier = MagicChanceMutilplier;
			clone.SummonChanceMutilplier = SummonChanceMutilplier;
			clone.ModifyWeaponAmountAddition = ModifyWeaponAmountAddition;
			clone.ModifyPotionTypeAmountAddition = ModifyPotionTypeAmountAddition;
			clone.ModifyPotionNumberAmountAddition = ModifyPotionNumberAmountAddition;
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
			ChestLootDropPlayer clone = (ChestLootDropPlayer)clientPlayer;
			if (MeleeChanceMutilplier != clone.MeleeChanceMutilplier
			|| RangeChanceMutilplier != clone.RangeChanceMutilplier
			 || MagicChanceMutilplier != clone.MagicChanceMutilplier
			 || SummonChanceMutilplier != clone.SummonChanceMutilplier
			 || ModifyWeaponAmountAddition != clone.ModifyWeaponAmountAddition
			 || ModifyPotionTypeAmountAddition != clone.ModifyPotionTypeAmountAddition
			 || ModifyPotionNumberAmountAddition != clone.ModifyPotionNumberAmountAddition
			 )
				SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}
	}
}
