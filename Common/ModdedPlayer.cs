using Terraria;
using System.IO;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossRush.Common.Systems;
using Terraria.DataStructures;
using BossRush.Contents.Items;
using BossRush.Contents.Perks;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Potion;
using BossRush.Contents.Items.Spawner;
using BossRush.Contents.Items.aDebugItem;
using BossRush.Contents.Items.Accessories.LostAccessories;

namespace BossRush.Common {
	class ModdedPlayer : ModPlayer {
		//NoHiter
		public int gitGud = 0;
		public int EnchantingEnable = 0;
		public int SkillEnable = 0;

		public int HowManyBossIsAlive = 0;
		public bool ItemIsUsedDuringBossFight = false;
		public override void OnEnterWorld() {
			if (Player.IsDebugPlayer()) {
				Main.NewText("You have enter debug mode", Color.Red);
				return;
			}
			if (Main.ActiveWorldFileData.GameMode == 0) {
				Main.NewText("Yo this guys playing on classic mode lol, skill issues spotted !");
			}
		}
		public override void PreUpdate() {
			CheckHowManyHit();
			if (Player.ItemAnimationActive && HowManyBossIsAlive > 0 && Player.HeldItem.damage > 0) {
				ItemIsUsedDuringBossFight = true;
			}
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
				ItemIsUsedDuringBossFight = false;
				amountOfTimeGotHit = 0;
			}
		}
		public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath) {
			yield return new Item(ModContent.ItemType<WoodenLootBox>());
			yield return new Item(ItemID.PlatinumPickaxe);
			yield return new Item(ItemID.PlatinumAxe);
			yield return new Item(ModContent.ItemType<BuilderLootBox>());
			if (UniversalSystem.CanAccessContent(Player, UniversalSystem.HARDCORE_MODE)) {
				yield return new Item(ModContent.ItemType<LunchBox>());
				if (UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE)) {
					yield return new Item(ItemID.ManaCrystal, 5);
					yield return new Item(ModContent.ItemType<DayTimeCycle>());
					yield return new Item(ModContent.ItemType<BiomeToggle>());
				}
				if (ModContent.GetInstance<BossRushModConfig>().SynergyMode) {
					//yield return new Item(ModContent.ItemType<CursedSkull>());
					yield return new Item(ModContent.ItemType<StarterPerkChooser>());
					yield return new Item(ModContent.ItemType<SynergyEnergy>());
					//yield return new Item(ModContent.ItemType<ConfrontTrueGod>());
					//yield return new Item(ModContent.ItemType<PowerEnergy>());
					if (UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE)) {
						yield return new Item(ModContent.ItemType<SpawnMerchant>());
					}
				}
				if (ModContent.GetInstance<BossRushModConfig>().Nightmare) {
					yield return new Item(ItemID.RedPotion, 10);
					yield return new Item(ItemID.LifeCrystal, 15);
					yield return new Item(ItemID.ManaCrystal, 4);
				}
				if (Player.name == "LQTXinim" || Player.name == "LowQualityTrashXinim") {
					yield return new Item(ModContent.ItemType<RainbowLootBox>());
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
				}
				if (Player.name == "HMdebug") {
					yield return new Item(ModContent.ItemType<LootboxLordSummon>());
					yield return new Item(ModContent.ItemType<IronLootBox>());
					yield return new Item(ModContent.ItemType<SilverLootBox>());
					yield return new Item(ModContent.ItemType<GoldLootBox>());
					yield return new Item(ModContent.ItemType<CorruptionLootBox>());
					yield return new Item(ModContent.ItemType<CrimsonLootBox>());
					yield return new Item(ModContent.ItemType<IceLootBox>());
					yield return new Item(ModContent.ItemType<HoneyTreasureChest>());
					yield return new Item(ModContent.ItemType<StarterPerkChooser>());
					yield return new Item(ModContent.ItemType<PerkChooser>());
					yield return new Item(ItemID.PlatinumCoin, 2);
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
			}
		}
		public override void ModifyStartingInventory(IReadOnlyDictionary<string, List<Item>> itemsByMod, bool mediumCoreDeath) {
			itemsByMod["Terraria"].Clear();
		}
		public int amountOfTimeGotHit = 0;
		public override void OnHurt(Player.HurtInfo info) {
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
				Player.QuickSpawnItem(null, ModContent.ItemType<LunaticTablet>());
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
			packet.Write(EnchantingEnable);
			packet.Write(SkillEnable);
			packet.Send(toWho, fromWho);
		}
		public override void Initialize() {
			gitGud = 0;
			EnchantingEnable = 0;
			SkillEnable = 0;
		}
		public override void SaveData(TagCompound tag) {
			tag["gitgud"] = gitGud;
			tag["EnchantingEnable"] = EnchantingEnable;
			tag["SkillEnable"] = SkillEnable;
		}
		public override void LoadData(TagCompound tag) {
			gitGud = (int)tag["gitgud"];
			if(tag.TryGet<int>("EnchantingEnable", out int value1)) {
				EnchantingEnable = value1;
			}
			if (tag.TryGet<int>("SkillEnable", out int value2)) {
				SkillEnable = value2;
			}
		}
		public void ReceivePlayerSync(BinaryReader reader) {
			gitGud = reader.ReadInt32();
			EnchantingEnable = reader.ReadInt32();
			SkillEnable = reader.ReadInt32();
		}

		public override void CopyClientState(ModPlayer targetCopy) {
			ModdedPlayer clone = (ModdedPlayer)targetCopy;
			clone.gitGud = gitGud;
			clone.EnchantingEnable = EnchantingEnable;
			clone.SkillEnable = SkillEnable;
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
			ModdedPlayer clone = (ModdedPlayer)clientPlayer;
			if (gitGud != clone.gitGud
				|| EnchantingEnable != clone.EnchantingEnable
				|| SkillEnable != clone.SkillEnable) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}
	}
}
