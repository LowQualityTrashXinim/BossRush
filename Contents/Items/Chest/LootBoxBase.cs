using System;
using Terraria;
using Terraria.ID;
using System.Linq;
using Terraria.ModLoader;
using Terraria.GameContent;
using BossRush.Common.Utils;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using Terraria.DataStructures;
using BossRush.Common.General;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Contents.Items.RelicItem;
using BossRush.Contents.Items.Consumable.Potion;
using Steamworks;

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
			if (!ChestUseOwnLogic) {
				if (LootboxSystem.GetItemPool(Type) == null)
					return;
				if (LootboxSystem.GetItemPool(Type).AllItemPool().Count <= 0)
					return;
				if (!UniversalSystem.CheckLegacy(UniversalSystem.LEGACY_LOOTBOX))
					return;
				ChestLootDropPlayer chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
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
		private int ModifyRNG(int rng, Player player, float chance = 0, int influence = -1) {
			if (influence != -1) {
				if (Main.rand.NextFloat() <= chance) {
					return influence;
				}
			}
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
			meleeChance = (int)(modPlayer.UpdateMeleeChanceMutilplier * meleeChance);
			rangeChance = (int)(modPlayer.UpdateRangeChanceMutilplier * rangeChance);
			magicChance = (int)(modPlayer.UpdateMagicChanceMutilplier * magicChance);
			summonChance = (int)(modPlayer.UpdateSummonChanceMutilplier * summonChance);
			rangeChance += meleeChance;
			magicChance += rangeChance;
			summonChance += magicChance;
			specialChance += summonChance;
			int chooser = Main.rand.Next(specialChance) + 1;
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
		public override bool CanRightClick() => true;
		public virtual bool CanActivateSpoil => true;
		public sealed override void RightClick(Player player) {
			RoguelikeData.Lootbox_AmountOpen = Math.Clamp(RoguelikeData.Lootbox_AmountOpen + 1, 0, int.MaxValue);
			if (!ModContent.GetInstance<UniversalSystem>().LootBoxOpen.Contains(Type)) {
				ModContent.GetInstance<UniversalSystem>().LootBoxOpen.Add(Type);
			}
			ChestLootDropPlayer modplayer = player.GetModPlayer<ChestLootDropPlayer>();
			if (modplayer.ItemGraveYard.Count > 0) {
				int RemoveAmount = 1 + modplayer.ItemGraveYard.Count / 10;
				for (int i = 0; i < RemoveAmount; i++) {
					int index = Main.rand.Next(modplayer.ItemGraveYard.Count);
					modplayer.ItemGraveYard.Remove(modplayer.ItemGraveYard.ElementAt(index));
				}
			}
			for (int i = 0; i < player.inventory.Length; i++) {
				Item item = player.inventory[i];
				if (item.ammo == AmmoID.None
					|| item.type == ItemID.EndlessMusketPouch
					|| item.type == ItemID.EndlessQuiver
					|| (item.type == ItemID.CopperCoin || item.type == ItemID.GoldCoin || item.type == ItemID.SilverCoin || item.type == ItemID.PlatinumCoin && player.HasItem(ItemID.CoinGun))) {
					continue;
				}
				int stackCheck = 350;
				if (Main.masterMode) {
					stackCheck += 150;
				}
				if (item.stack < stackCheck) {
					item.stack = stackCheck;
				}
			}
			var entitySource = player.GetSource_OpenItem(Type);
			if (modplayer.LootboxCanDropSpecialPotion) {
				player.QuickSpawnItem(entitySource, Main.rand.Next(TerrariaArrayID.SpecialPotion));
			}
			if (UniversalSystem.CheckLegacy(UniversalSystem.LEGACY_LOOTBOX)) {
				OnRightClick(player, modplayer);
				if (UniversalSystem.CanAccessContent(player, UniversalSystem.HARDCORE_MODE)) {
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
					if (modplayer.CanDropSynergyEnergy) {
						player.QuickSpawnItem(entitySource, ModContent.ItemType<SynergyEnergy>());
					}
				}
			}
			else {
				if (CanActivateSpoil) {
					UniversalSystem system = ModContent.GetInstance<UniversalSystem>();
					system.ActivateSpoilsUI(Type);
				}
			}
			AbsoluteRightClick(player);
			if (UniversalSystem.LuckDepartment(UniversalSystem.CHECK_RARELOOTBOX)) {
				if (Main.rand.NextBool()) {
					Item item = player.QuickSpawnItemDirect(entitySource, ModContent.ItemType<WeaponTicket>());
					WeaponTicket ticket = item.ModItem as WeaponTicket;
					LootBoxItemPool pool = LootboxSystem.GetItemPool(Type);
					int amount = (int)modplayer.DropModifier.ApplyTo(Main.rand.Next(4, 11));
					HashSet<int> p = new(pool.AllItemPool());
					if (p.Count <= amount) {
						ticket.Add_HashSet(p);
					}
					else {
						for (int i = 0; i < amount; i++) {
							int type = Main.rand.NextFromHashSet(p);
							p.Remove(type);
							if (!ticket.Add_Item(type)) {
								i--;
							}
						}
					}
				}
				if (Main.rand.NextBool(1500)) {
					player.QuickSpawnItem(entitySource, ModContent.ItemType<RainbowLootBox>());
				}
			}
		}
		/// <summary>
		/// This won't be change no matter what
		/// </summary>
		/// <param name="player"></param>
		public virtual void AbsoluteRightClick(Player player) { }
		/// <summary>
		/// This only active if legacy setting is true
		/// </summary>
		/// <param name="player"></param>
		/// <param name="modplayer"></param>
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
			DummyMeleeData.UnionWith(modplayer.Request_AddMelee);
			DummyRangeData.UnionWith(modplayer.Request_AddRange);
			DummyMagicData.UnionWith(modplayer.Request_AddMagic);
			DummySummonData.UnionWith(modplayer.Request_AddSummon);
			DummyMiscsData.UnionWith(modplayer.Request_AddMisc);
			for (int i = 0; i < LoopAmount; i++) {
				rng = RNGManage(player);
				rng = ModifyRNG(rng, player, modplayer.Chance_4RNGselector, modplayer.InfluenceableRNGselector);
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
						ReturnWeapon = Main.rand.NextFromHashSet(DummySummonData);
						player.QuickSpawnItem(entitySource, ReturnWeapon);
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
			int Amount = (int)(350 * AmountModifier);
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
				Ammo = Main.rand.Next(new int[] { ItemID.RocketI, ItemID.RocketII, ItemID.RocketIII, ItemID.RocketIV });
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
		/// Allow for safely add in a list of accessory that specific to a situation
		/// </summary>
		/// <returns></returns>
		public virtual List<int> SafePostAddAcc() => new List<int>() { };

		private void AddAcc(List<int> flag) {
			if (UniversalSystem.LuckDepartment(UniversalSystem.CHECK_LOSTACC)) {
				Accessories.AddRange(BossRushModSystem.LostAccessories.Select(i => i.type));
			}
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
		/// This method return a set of armor with randomize piece of armor accordingly to progression
		/// </summary>
		public static void GetArmorForPlayer(IEntitySource entitySource, Player player) {
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
			if (Main.hardMode) {
				HeadArmor.AddRange(TerrariaArrayID.HeadArmorHardMode);
				BodyArmor.AddRange(TerrariaArrayID.BodyArmorHardMode);
				LegArmor.AddRange(TerrariaArrayID.LegArmorHardMode);
			}
			if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) {
				HeadArmor.AddRange(TerrariaArrayID.HeadArmorPostMech);
				BodyArmor.AddRange(TerrariaArrayID.BodyArmorPostMech);
				LegArmor.AddRange(TerrariaArrayID.LegArmorPostMech);
			}
			if (NPC.downedPlantBoss) {
				HeadArmor.AddRange(TerrariaArrayID.HeadArmorPostPlant);
				BodyArmor.AddRange(TerrariaArrayID.BodyArmorPostPlant);
				LegArmor.AddRange(TerrariaArrayID.LegArmorPostPlant);
			}
			if (NPC.downedGolemBoss) {
				HeadArmor.AddRange(TerrariaArrayID.HeadArmorPostGolem);
				BodyArmor.AddRange(TerrariaArrayID.BodyArmorPostGolem);
				LegArmor.AddRange(TerrariaArrayID.LegArmorPostGolem);
			}
			player.QuickSpawnItem(entitySource, Main.rand.Next(HeadArmor));
			player.QuickSpawnItem(entitySource, Main.rand.Next(BodyArmor));
			player.QuickSpawnItem(entitySource, Main.rand.Next(LegArmor));
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
		/// <summary>
		/// Return weapon base on world/player progression
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
			DropItemMelee.AddRange(TerrariaArrayID.MeleePreEoC);
			DropItemRange.AddRange(TerrariaArrayID.RangePreEoC);
			DropItemMagic.AddRange(TerrariaArrayID.MagicPreEoC);
			DropItemSummon.AddRange(TerrariaArrayID.SummonerPreEoC);
			DropItemMisc.AddRange(TerrariaArrayID.Special);
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
					amount += 199;
					weapon = Main.rand.NextFromCollection(DropItemMisc);
					break;
			}
		}
		/// <summary>
		/// This is a static function version of GetWeapon in LootBoxBase<br/>
		/// Lootbox check is already presented in this function and as such it is not needed to check if the item a lootboxbase or not<br/>
		/// it is also important to remember that this function will automatically handle dropping item
		/// </summary>
		/// <param name="lootbox">The lootbox item</param>
		/// <param name="player">The player</param>
		/// <param name="rng">rng number</param>
		/// <param name="additiveModify">additive direct modify to amount of weapons can be given</param>
		public static void GetWeapon(Item lootbox, Player player, int rng = 0, float additiveModify = 1) {
			if (lootbox.ModItem is not LootBoxBase item) {
				return;
			}
			int SpecialAmount = 350;
			int ReturnWeapon = ItemID.None;
			//adding stuff here
			if (Main.masterMode) {
				SpecialAmount += 150;
			}
			IEntitySource entitySource = player.GetSource_OpenItem(lootbox.type);
			if (UniversalSystem.CanAccessContent(player, UniversalSystem.SYNERGYFEVER_MODE)) {
				int weapon = Main.rand.Next(BossRushModSystem.SynergyItem).type;
				player.QuickSpawnItemDirect(entitySource, weapon);
				item.AmmoForWeapon(entitySource, player, weapon);
				return;
			}
			item.ModifyLootAdd(player);
			//actual choosing item
			ChestLootDropPlayer modplayer = player.GetModPlayer<ChestLootDropPlayer>();
			modplayer.GetAmount();
			HashSet<int> DummyMeleeData = LootboxSystem.GetItemPool(item.Type).DropItemMelee.Where(x => !modplayer.ItemGraveYard.Contains(x)).ToHashSet();
			HashSet<int> DummyRangeData = LootboxSystem.GetItemPool(item.Type).DropItemRange.Where(x => !modplayer.ItemGraveYard.Contains(x)).ToHashSet();
			HashSet<int> DummyMagicData = LootboxSystem.GetItemPool(item.Type).DropItemMagic.Where(x => !modplayer.ItemGraveYard.Contains(x)).ToHashSet();
			HashSet<int> DummySummonData = LootboxSystem.GetItemPool(item.Type).DropItemSummon.Where(x => !modplayer.ItemGraveYard.Contains(x)).ToHashSet();
			HashSet<int> DummyMiscsData = LootboxSystem.GetItemPool(item.Type).DropItemMisc;
			DummyMeleeData.UnionWith(modplayer.Request_AddMelee);
			DummyRangeData.UnionWith(modplayer.Request_AddRange);
			DummyMagicData.UnionWith(modplayer.Request_AddMagic);
			DummySummonData.UnionWith(modplayer.Request_AddSummon);
			DummyMiscsData.UnionWith(modplayer.Request_AddMisc);
			int weaponAmount = (int)Math.Clamp(MathF.Ceiling(modplayer.weaponAmount * additiveModify), 1, 999999);
			for (int i = 0; i < weaponAmount; i++) {
				rng = item.RNGManage(player);
				rng = item.ModifyRNG(rng, player, modplayer.Chance_4RNGselector, modplayer.InfluenceableRNGselector);
				switch (rng) {
					case 0:
						continue;
					case 1:
						if (DummyMeleeData.Count <= 0) {
							GetWeapon(out int Weapon, out int Amount, rng);
							player.QuickSpawnItem(entitySource, Weapon, Amount);
							continue;
						}
						ReturnWeapon = Main.rand.NextFromHashSet(DummyMeleeData);
						player.QuickSpawnItem(entitySource, ReturnWeapon);
						modplayer.ItemGraveYard.Add(ReturnWeapon);
						DummyMeleeData.Remove(ReturnWeapon);
						continue;
					case 2:
						if (DummyRangeData.Count <= 0) {
							GetWeapon(out int Weapon, out int Amount, rng);
							player.QuickSpawnItem(entitySource, Weapon, Amount);
							item.AmmoForWeapon(entitySource, player, Weapon);
							continue;
						}
						ReturnWeapon = Main.rand.NextFromHashSet(DummyRangeData);
						player.QuickSpawnItem(entitySource, ReturnWeapon);
						item.AmmoForWeapon(entitySource, player, ReturnWeapon);
						modplayer.ItemGraveYard.Add(ReturnWeapon);
						DummyRangeData.Remove(ReturnWeapon);
						continue;
					case 3:
						if (DummyMagicData.Count <= 0) {
							GetWeapon(out int Weapon, out int Amount, rng);
							player.QuickSpawnItem(entitySource, Weapon, Amount);
							item.AmmoForWeapon(entitySource, player, Weapon);
							continue;
						}
						ReturnWeapon = Main.rand.NextFromHashSet(DummyMagicData);
						player.QuickSpawnItem(entitySource, ReturnWeapon);
						modplayer.ItemGraveYard.Add(ReturnWeapon);
						DummyMagicData.Remove(ReturnWeapon);
						item.AmmoForWeapon(entitySource, player, ReturnWeapon);
						continue;
					case 4:
						if (DummySummonData.Count <= 0) {
							GetWeapon(out int Weapon, out int Amount, rng);
							player.QuickSpawnItem(entitySource, Weapon, Amount);
							item.AmmoForWeapon(entitySource, player, Weapon);
							continue;
						}
						ReturnWeapon = Main.rand.NextFromHashSet(DummySummonData);
						player.QuickSpawnItem(entitySource, ReturnWeapon);
						item.AmmoForWeapon(entitySource, player, ReturnWeapon);
						modplayer.ItemGraveYard.Add(ReturnWeapon);
						DummySummonData.Remove(ReturnWeapon);
						continue;
					case 5:
						if (DummyMiscsData.Count < 1) {
							ChooseWeapon(Main.rand.Next(1, 5), ref ReturnWeapon, ref SpecialAmount, DummyMeleeData.ToList(), DummyRangeData.ToList(), DummyMagicData.ToList(), DummySummonData.ToList(), DummyMiscsData.ToList());
							continue;
						}
						ReturnWeapon = Main.rand.NextFromHashSet(DummyMiscsData);
						player.QuickSpawnItem(entitySource, ReturnWeapon, SpecialAmount);
						modplayer.ItemGraveYard.Add(ReturnWeapon);
						DummyMiscsData.Remove(ReturnWeapon);
						continue;
					case 6:
						player.QuickSpawnItem(entitySource, ModContent.ItemType<WonderDrug>());
						continue;
				}
			}
		}
		/// <summary>
		/// Use this if you only care about getting ammo for weapon
		/// </summary>
		/// <param name="player"></param>
		/// <param name="weapon"></param>
		/// <param name="AmountModifier"></param>
		public static void AmmoForWeapon(Player player, int weapon, float AmountModifier = 1) {
			LootBoxBase.AmmoForWeapon(BossRushModSystem.ListLootboxType.FirstOrDefault(), player, weapon, AmountModifier);
		}
		/// <summary>
		/// Automatically quick drop player ammo item accordingly to weapon ammo type
		/// </summary>
		/// <param name="lootbox">The id of lootbox</param>
		/// <param name="player">The player</param>
		/// <param name="weapon">Weapon need to be checked</param>
		/// <param name="AmountModifier">Modify the ammount of ammo will be given</param>
		public static void AmmoForWeapon(int lootbox, Player player, int weapon, float AmountModifier = 1) {
			IEntitySource entitySource = player.GetSource_OpenItem(lootbox);
			Item weapontoCheck = ContentSamples.ItemsByType[weapon];
			if (weapontoCheck.consumable || weapontoCheck.useAmmo == AmmoID.None) {
				if (weapontoCheck.mana > 0) {
					player.QuickSpawnItem(entitySource, ItemID.LesserManaPotion, 5);
				}
				return;
			}
			//The most ugly code
			int Amount = (int)(350 * AmountModifier);
			int Ammo;
			if (Main.masterMode) {
				Amount += 150;
			}
			List<int> DropArrowAmmo = new();
			List<int> DropBulletAmmo = new();
			List<int> DropDartAmmo = new();

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
				Ammo = Main.rand.Next(new int[] { ItemID.RocketI, ItemID.RocketII, ItemID.RocketIII, ItemID.RocketIV });
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
			player.QuickSpawnItem(entitySource, Ammo, Amount);
		}
		/// <summary>
		/// This function will automatically handle drop for you so no need to do it yourself
		/// </summary>
		/// <param name="type"></param>
		/// <param name="player"></param>
		public static void GetAccessories(int type, Player player, bool LostAccIncluded = false) {
			List<int> acc = [.. TerrariaArrayID.EveryCombatHealtMovehAcc];
			if (UniversalSystem.LuckDepartment(UniversalSystem.CHECK_LOSTACC) && LostAccIncluded) {
				acc.AddRange(BossRushModSystem.LostAccessories.Select(i => i.type));
			}
			IEntitySource entitySource = player.GetSource_OpenItem(type);
			player.QuickSpawnItem(entitySource, Main.rand.Next(acc));
		}
		/// <summary>
		/// This function will automatically handle drop for you so no need to do it yourself
		/// </summary>
		/// <param name="type"></param>
		/// <param name="player"></param>
		public static void GetPotion(int type, Player player) {
			List<int> DropItemPotion = [.. TerrariaArrayID.NonMovementPotion, .. TerrariaArrayID.MovementPotion];
			DropItemPotion.Add(ItemID.LifeforcePotion);
			DropItemPotion.Add(ItemID.InfernoPotion);
			ChestLootDropPlayer modplayer = player.GetModPlayer<ChestLootDropPlayer>();
			modplayer.GetAmount();
			IEntitySource entitySource = player.GetSource_OpenItem(type);
			for (int i = 0; i < modplayer.potionNumAmount; i++) {
				player.QuickSpawnItem(entitySource, Main.rand.Next(DropItemPotion), modplayer.potionTypeAmount);
			}
		}
		public static void GetArmorPiece(int type, Player player, bool randomized = false) {
			IEntitySource entitySource = player.GetSource_OpenItem(type);
			if (randomized) {
				player.QuickSpawnItem(entitySource, Main.rand.Next(TerrariaArrayID.EveryArmorPiece));
				return;
			}
			player.QuickSpawnItem(entitySource, Main.rand.Next(TerrariaArrayID.HeadAllPiece));
			player.QuickSpawnItem(entitySource, Main.rand.Next(TerrariaArrayID.BodyAllPiece));
			player.QuickSpawnItem(entitySource, Main.rand.Next(TerrariaArrayID.LegsAllPiece));
		}
		public static void GetRelic(int type, Player player, int amount = 1) {
			IEntitySource entitySource = player.GetSource_OpenItem(type);
			amount = player.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(amount);

			for (int i = 0; i < amount; i++) {
				if (UniversalSystem.CanAccessContent(player, UniversalSystem.SYNERGYFEVER_MODE)) {
					Item relicitem = player.QuickSpawnItemDirect(entitySource, ModContent.ItemType<Relic>());
					if (Main.rand.NextBool(4)) {
						if (relicitem.ModItem is Relic relic) {
							relic.AddRelicTemplate(player, RelicTemplate.GetRelicType<SynergyTemplate>());
						}
					}
				}
				else {
					player.QuickSpawnItem(entitySource, ModContent.ItemType<Relic>());
				}
			}
		}
		public static void GetSkillLootbox(int type, Player player, int amount = 1) {
			IEntitySource entitySource = player.GetSource_OpenItem(type);
			amount = player.GetModPlayer<ChestLootDropPlayer>().ModifyGetAmount(amount);

			for (int i = 0; i < amount; i++) {
				player.QuickSpawnItem(entitySource, ModContent.ItemType<SkillLootBox>());
			}
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
			Main.instance.LoadItem(Type);
			Texture2D texture = TextureAssets.Item[Type].Value;
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, position + new Vector2(2, 2), null, color1, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-2, 2), null, color2, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(2, -2), null, color3, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-2, -2), null, color4, 0, origin, scale, SpriteEffects.None, 0);
			}
			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
	}
	public class LootboxSystem : ModSystem {
		protected static List<LootBoxItemPool> LootBoxDropPool { get; private set; } = new List<LootBoxItemPool>();
		/// <summary>
		/// Direct modify maybe unstable, unsure how this will work <br/>
		/// To safely modifying loot pool of said item, please refer to <see cref="ReplaceItemPool(LootBoxItemPool)"/>
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static LootBoxItemPool GetItemPool(int type) => LootBoxDropPool.Where(i => i.PoolID == type).FirstOrDefault();
		public static void AddItemPool(LootBoxItemPool pool) {
			pool.IndexID = LootBoxDropPool.Count;
			LootBoxDropPool.Add(pool);
		}
		/// <summary>
		/// Replace the item pool with a new item pool
		/// </summary>
		/// <param name="pool"></param>
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
		public HashSet<int> Request_AddMelee = new();
		public HashSet<int> Request_AddRange = new();
		public HashSet<int> Request_AddMagic = new();
		public HashSet<int> Request_AddSummon = new();
		public HashSet<int> Request_AddMisc = new();
		public int InfluenceableRNGselector = -1;
		public float Chance_4RNGselector = 0;

		public int counterShow = 0;
		public int weaponShowID = 0, potionShowID = 0, foodshowID = 0, accShowID = 0;

		public HashSet<int> ItemGraveYard = new HashSet<int>();
		public bool CanDropSynergyEnergy = true;

		public StatModifier DropModifier = new();

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
		public int ModifyGetAmount(int baseValue) {
			int amount = (int)DropModifier.ApplyTo(baseValue);
			if (amount <= 0) {
				return 1;
			}
			return amount;
		}
		/// <summary>
		/// This must be called before using
		/// <br/><see cref="weaponAmount"/>
		/// <br/><see cref="potionTypeAmount"/>
		/// <br/><see cref="potionNumAmount"/>
		/// </summary>
		public void GetAmount() {
			weaponAmount = 3;
			potionTypeAmount = 1;
			potionNumAmount = 2;
			if (Main.getGoodWorld) {
				weaponAmount = 2;
				potionTypeAmount = 1;
				potionNumAmount = 1;
			}
			weaponAmount = Math.Clamp(ModifyGetAmount(weaponAmount + WeaponAmountAddition), 1, 999999);
			potionTypeAmount = ModifyGetAmount(potionTypeAmount + PotionTypeAmountAddition);
			potionNumAmount = ModifyGetAmount(potionNumAmount + PotionNumberAmountAddition);
			if (ModContent.GetInstance<RogueLikeConfig>().SynergyFeverMode) {
				weaponAmount = 1;
			}
		}
		public override void ResetEffects() {
			Request_AddMelee.Clear();
			Request_AddRange.Clear();
			Request_AddMagic.Clear();
			Request_AddSummon.Clear();
			Request_AddMisc.Clear();
			InfluenceableRNGselector = -1;
			Chance_4RNGselector = 0;
			LootboxCanDropSpecialPotion = false;
			CanDropSynergyEnergy = false;
			DropModifier = StatModifier.Default;
			WeaponAmountAddition = 0;
			PotionTypeAmountAddition = 0;
			PotionNumberAmountAddition = 0;
			UpdateMeleeChanceMutilplier = 1;
			UpdateRangeChanceMutilplier = 1;
			UpdateMagicChanceMutilplier = 1;
			UpdateSummonChanceMutilplier = 1;
		}
		public override void Unload() {
			ItemGraveYard = null;
		}
	}
}
