using Terraria;
using System.IO;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using BossRush.Contents.Items;
using BossRush.Contents.Perks;
using System.Collections.Generic;
using BossRush.Contents.Items.Card;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Potion;
using BossRush.Contents.Items.Spawner;
using BossRush.Contents.Items.Toggle;
using BossRush.Contents.BuffAndDebuff;
using BossRush.Contents.Items.aDebugItem;
using BossRush.Contents.Items.Accessories.SynergyAccessories.GuideToMasterNinja;

namespace BossRush.Common {
	class ModdedPlayer : ModPlayer {
		//NoHiter
		public int gitGud = 0;
		public int HowManyBossIsAlive = 0;
		public override void OnEnterWorld() {
			if (ModContent.GetInstance<BossRushModConfig>().AutoHardCore && !Player.IsDebugPlayer())
				Player.difficulty = PlayerDifficultyID.Hardcore;
			Main.NewText("Currently the mod are still lacking a lot of planned feature but we are focusing on pre hardmode content");
			Main.NewText("We are currently working hard on the mod, if you spotted any isssue such as bug please report them in our discord server");
			if (Player.difficulty != PlayerDifficultyID.Hardcore && !ModContent.GetInstance<BossRushModConfig>().HardEnableFeature)
				Main.NewText("Most of the mod content are locked behind hardcore, please play in hardcore or enable HardEnableFeature");
			if (Main.ActiveWorldFileData.GameMode == 0) {
				Main.NewText("Yo this guys playing on easy mode lol, skill issues spotted !");
			}
		}
		public override void PreUpdate() {
			CheckHowManyHit();
		}
		private void CheckHowManyHit() {
			HowManyBossIsAlive = 0;
			bool FoundEater = false;
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC npc = Main.npc[i];
				if ((npc.boss || (npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsTail) && !FoundEater) && npc.active) {
					HowManyBossIsAlive++;
					FoundEater = true;
				}
			}
			// What happen when boss is inactive
			if (HowManyBossIsAlive == 0) {
				amountOfTimeGotHit = 0;
			}
		}

		public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
			health = StatModifier.Default; mana = StatModifier.Default;
			if (Main.ActiveWorldFileData.GameMode == 0) {
				health.Base = 100;
			}
		}
		public override void ModifyItemScale(Item item, ref float scale) {
			if (Player.HasBuff(ModContent.BuffType<BerserkBuff>()) && item.DamageType == DamageClass.Melee) {
				scale += .3f;
			}
		}
		public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath) {
			yield return new Item(ModContent.ItemType<WoodenLootBox>());
			yield return new Item(ItemID.Safe);
			yield return new Item(ItemID.MoneyTrough);
			yield return new Item(ItemID.PlatinumPickaxe);
			yield return new Item(ItemID.PlatinumAxe);
			yield return new Item(ModContent.ItemType<BuilderLootBox>());
			if (Player.difficulty == PlayerDifficultyID.Hardcore || ModContent.GetInstance<BossRushModConfig>().AutoHardCore) {
				if (ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode) {
					yield return new Item(ModContent.ItemType<LunchBox>());
					yield return new Item(ItemID.ManaCrystal, 5);
					yield return new Item(ModContent.ItemType<DayTimeCycle>());
					yield return new Item(ModContent.ItemType<CursedSkull>());
					yield return new Item(ModContent.ItemType<BiomeToggle>());
				}
				if (ModContent.GetInstance<BossRushModConfig>().SynergyMode) {
					yield return new Item(ModContent.ItemType<StarterPerkChooser>());
					yield return new Item(ModContent.ItemType<SynergyEnergy>());
					//yield return new Item(ModContent.ItemType<PowerEnergy>());
					yield return new Item(ModContent.ItemType<CardSacrifice>());
				}
				if (ModContent.GetInstance<BossRushModConfig>().Nightmare) {
					yield return new Item(ItemID.RedPotion, 10);
				}
				if (Player.name == "LQTXinim" || Player.name == "LowQualityTrashXinim") {
					yield return new Item(ModContent.ItemType<RainbowTreasureChest>());
				}
				if (Player.name == "FeelingLucky") {
					yield return new Item(ModContent.ItemType<GodDice>());
				}
				if (Player.name.ToLower().Trim() == "drugaddict") {
					yield return new Item(ModContent.ItemType<WonderDrug>(), 99);
				}
				if (Player.name.Contains("Ninja")) {
					yield return new Item(ItemID.Katana);
					yield return new Item(ItemID.Shuriken, 100);
					yield return new Item(ItemID.ThrowingKnife, 100);
					yield return new Item(ItemID.PoisonedKnife, 100);
					yield return new Item(ItemID.BoneDagger, 100);
					yield return new Item(ItemID.FrostDaggerfish, 100);
					yield return new Item(ItemID.NinjaHood);
					yield return new Item(ItemID.NinjaShirt);
					yield return new Item(ItemID.NinjaPants);
					yield return new Item(ModContent.ItemType<GuideToMasterNinja>());
					yield return new Item(ModContent.ItemType<GuideToMasterNinja2>());
				}
				if (Player.name == "HMdebug") {
					yield return new Item(ModContent.ItemType<IronLootBox>());
					yield return new Item(ModContent.ItemType<SilverLootBox>());
					yield return new Item(ModContent.ItemType<GoldLootBox>());
					yield return new Item(ModContent.ItemType<CorruptionLootBox>());
					yield return new Item(ModContent.ItemType<CrimsonLootBox>());
					yield return new Item(ModContent.ItemType<IceLootBox>());
					yield return new Item(ModContent.ItemType<HoneyTreasureChest>());
					yield return new Item(ModContent.ItemType<LootboxLordSummon>());
					yield return new Item(ModContent.ItemType<PerkChooser>(), 8);
					yield return new Item(ItemID.LifeCrystal, 15);
					yield return new Item(ItemID.ManaCrystal, 4);
					yield return new Item(ItemID.KingSlimeBossBag);
					yield return new Item(ItemID.EyeOfCthulhuBossBag);
					yield return new Item(ItemID.EaterOfWorldsBossBag);
					yield return new Item(ItemID.BrainOfCthulhuBossBag);
					yield return new Item(ItemID.SkeletronBossBag);
					yield return new Item(ItemID.QueenBeeBossBag);
					yield return new Item(ItemID.DeerclopsBossBag);
					yield return new Item(ItemID.GuideVoodooDoll);
				}
				yield return new Item(ModContent.ItemType<ModStatsDebugger>());
				yield return new Item(ModContent.ItemType<ShowPlayerStats>());
			}
		}
		public override void ModifyStartingInventory(IReadOnlyDictionary<string, List<Item>> itemsByMod, bool mediumCoreDeath) {
			itemsByMod["Terraria"].Clear();
		}
		public int amountOfTimeGotHit = 0;
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
			if (Player.HasBuff(ModContent.BuffType<Protection>())) {
				Player.Heal(Player.statLifeMax2);
				Player.ClearBuff(ModContent.BuffType<Protection>());
				return false;
			}
			return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
		}
		public override void OnHurt(Player.HurtInfo info) {
			if (Player.HasBuff(ModContent.BuffType<GodVision>())) {
				Player.ClearBuff(ModContent.BuffType<GodVision>());
			}
			if (BossRushUtils.IsAnyVanillaBossAlive()) {
				if (gitGud > 0) {
					PlayerDeathReason reason = new PlayerDeathReason();
					reason.SourceCustomReason = $"{Player.name} has fail the challenge";
					Player.KillMe(reason, 9999999999, info.HitDirection);
					return;
				}
				else {
					amountOfTimeGotHit++;
				}
			}
		}

		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) {
			SpawnItem();
		}
		private void SpawnItem() {
			if (NPC.AnyNPCs(NPCID.KingSlime)) {
				Player.QuickSpawnItem(null, ItemID.SlimeCrown);
			}
			if (NPC.AnyNPCs(NPCID.EyeofCthulhu)) {
				Player.QuickSpawnItem(null, ItemID.SuspiciousLookingEye);
			}
			if (NPC.AnyNPCs(NPCID.BrainofCthulhu)) {
				Player.QuickSpawnItem(null, ItemID.BloodySpine);
			}
			if (NPC.AnyNPCs(NPCID.EaterofWorldsHead)) {
				Player.QuickSpawnItem(null, ItemID.WormFood);
			}
			if (NPC.AnyNPCs(NPCID.SkeletronHead)) {
				Player.QuickSpawnItem(null, ModContent.ItemType<CursedDoll>());
			}
			if (NPC.AnyNPCs(NPCID.QueenBee)) {
				Player.QuickSpawnItem(null, ItemID.Abeemination);
			}
			if (NPC.AnyNPCs(NPCID.WallofFlesh)) {
				Player.QuickSpawnItem(null, ItemID.GuideVoodooDoll);
			}
			if (NPC.AnyNPCs(NPCID.QueenSlimeBoss)) {
				Player.QuickSpawnItem(null, ItemID.QueenSlimeCrystal);
			}
			if (NPC.AnyNPCs(NPCID.Spazmatism) || NPC.AnyNPCs(NPCID.Retinazer)) {
				Player.QuickSpawnItem(null, ItemID.MechanicalEye);
			}
			if (NPC.AnyNPCs(NPCID.TheDestroyer)) {
				Player.QuickSpawnItem(null, ItemID.MechanicalWorm);
			}
			if (NPC.AnyNPCs(NPCID.SkeletronPrime)) {
				Player.QuickSpawnItem(null, ItemID.MechanicalSkull);
			}
			if (NPC.AnyNPCs(NPCID.Plantera)) {
				Player.QuickSpawnItem(null, ModContent.ItemType<PlanteraSpawn>());
			}
			if (NPC.AnyNPCs(NPCID.Golem)) {
				Player.QuickSpawnItem(null, ItemID.LihzahrdPowerCell);
			}
			if (NPC.AnyNPCs(NPCID.DukeFishron)) {
				Player.QuickSpawnItem(null, ItemID.TruffleWorm);
			}
			if (NPC.AnyNPCs(NPCID.HallowBoss)) {
				Player.QuickSpawnItem(null, ItemID.EmpressButterfly);
			}
			if (NPC.AnyNPCs(NPCID.CultistBoss)) {
				Player.QuickSpawnItem(null, ModContent.ItemType<LunaticCultistSpawner>());
			}
			if (NPC.AnyNPCs(NPCID.MoonLordCore)) {
				Player.QuickSpawnItem(null, ItemID.CelestialSigil);
			}
		}
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)BossRush.MessageType.GodUltimateChallenge);
			packet.Write((byte)Player.whoAmI);
			packet.Write(gitGud);
			packet.Send(toWho, fromWho);
		}
		public override void Initialize() {
			gitGud = 0;
		}
		public override void SaveData(TagCompound tag) {
			tag["gitgud"] = gitGud;
		}
		public override void LoadData(TagCompound tag) {
			gitGud = (int)tag["gitgud"];
		}
		public void ReceivePlayerSync(BinaryReader reader) {
			gitGud = reader.ReadInt32();
		}

		public override void CopyClientState(ModPlayer targetCopy) {
			ModdedPlayer clone = (ModdedPlayer)targetCopy;
			clone.gitGud = gitGud;
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
			ModdedPlayer clone = (ModdedPlayer)clientPlayer;
			if (gitGud != clone.gitGud) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}
	}
}
