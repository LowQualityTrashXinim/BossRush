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
using BossRush.Contents.Items.BossRushItem;
using BossRush.Contents.Items.Accessories.LostAccessories;
using BossRush.Common.WorldGenOverhaul;
using Terraria.GameInput;
using BossRush.Contents.Items.Consumable.Potion;
using BossRush.Contents.Items.Consumable.Spawner;
using BossRush.Contents.Items.Toggle;
using System;
using BossRush.Contents.Items.RelicItem;
using BossRush.Contents.Items.Weapon;
using BossRush.Contents.Projectiles;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.ChaosMiniShark;
using Steamworks;

namespace BossRush.Common.General {
	class ModdedPlayer : ModPlayer {
		public int EnchantingEnable = 0;
		public int SkillEnable = 0;

		public int HowManyBossIsAlive = 0;
		public bool ItemIsUsedDuringBossFight = false;

		public bool Hold_Shift = false;
		private Item starterItem = null;
		public bool UseOnly1ItemSinceTheStartOfTheGame(int type = 0) {
			if (starterItem == null) {
				return false;
			}
			if (starterItem != Player.HeldItem) {
				return false;
			}
			if (type != 0) {
				if (starterItem.type == type) {
					return true;
				}
			}
			return true;
		}
		public override void ProcessTriggers(TriggersSet triggersSet) {
			Hold_Shift = triggersSet.SmartSelect;
		}
		public override void OnEnterWorld() {
			if (ModContent.GetInstance<RogueLikeConfig>().AutoHardCore) {
				Player.difficulty = PlayerDifficultyID.Hardcore;
			}
			Player.itemAnimation = 0;
			if (Player.HeldItem != null && Player.HeldItem.IsAWeapon()) {
				Player.itemAnimationMax = Player.HeldItem.useAnimation;
			}
			BossRushUtils.Reflesh_GlobalItem(Mod, Player);
			RogueLikeWorldGen.GridPart_X = Main.maxTilesX / 24;
			RogueLikeWorldGen.GridPart_Y = Main.maxTilesY / 24;
			if (Player.IsDebugPlayer()) {
				Main.NewText("You have enter debug mode", Color.Red);
			}
			//if (Main.ActiveWorldFileData.GameMode == 0) {
			//	Main.NewText("Yo this guys playing on classic mode lol, skill issues spotted !");
			//}
		}
		public override void PreUpdate() {
			if (starterItem == null) {
				Item item = Player.HeldItem;
				if (item.IsAWeapon()) {
					starterItem = item;
				}
			}
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
		public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (Secert_PapyroVer && item.type == ModContent.ItemType<ChaosMiniShark>()) {
				if (Main.rand.NextBool(10000)) {
					type = ModContent.ProjectileType<SpaceGuyProjectile>();
				}
			}
		}
		public bool Secert_PapyroVer => Player.name == "Papyro" || Player.name == "WhoAmI" || Player.name == "IdentityCrisis" || Player.name == "Dysmorphia";
		public bool Secert_PototoVer => Player.name == "Pototo" || Player.name == "eatpotato";
		public bool Secret_MrRakan => Player.name == "MrRakan" || Player.name == "sorrow994";
		public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath) {
			int LifeCrystal = 0;
			int ManaCrystal = 0;
			yield return new Item(ModContent.ItemType<WoodenLootBox>());
			if (UniversalSystem.CanAccessContent(Player, UniversalSystem.HARDCORE_MODE)) {
				yield return new Item(ModContent.ItemType<LunchBox>());
				if (UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE)) {
					LifeCrystal += 5;
					ManaCrystal += 4;
					if (!UniversalSystem.CheckLegacy(UniversalSystem.LEGACY_LOOTBOX)) {
						yield return new Item(ModContent.ItemType<ExoticTeleporter>());
					}
					else {
						yield return new Item(ModContent.ItemType<BuilderLootBox>());
					}
					if (UniversalSystem.CanAccessContent(UniversalSystem.CHAOS_MODE)) {
						yield return new Item(ModContent.ItemType<SkillLootBox>(), 3);
						yield return new Item(ModContent.ItemType<WorldEssence>());
					}
					for (int i = 0; i < 3; i++) {
						yield return new Item(ModContent.ItemType<Relic>());
					}
					if (UniversalSystem.LuckDepartment(UniversalSystem.CHECK_RARELOOTBOX)) {
						if (Main.rand.NextBool(10)) {
							yield return new Item(ModContent.ItemType<WorldEssence>());
						}
						else {
							yield return new Item(ModContent.ItemType<CelestialEssence>());
						}
					}
					else {
						yield return new Item(ModContent.ItemType<CelestialEssence>());
					}
					//yield return new Item(ModContent.ItemType<WindSlashPerk>());
					yield return new Item(ModContent.ItemType<SynergyEnergy>());
				}
				if (ModContent.GetInstance<RogueLikeConfig>().WorldGenRLSettingTest) {
					yield return new Item(ItemID.CopperPickaxe);
					yield return new Item(ItemID.CopperAxe);
				}

				if (ModContent.GetInstance<RogueLikeConfig>().Nightmare) {
					yield return new Item(ItemID.RedPotion, 10);
				}
				if (Player.name == "LQTXinim" || Player.name == "LowQualityTrashXinim") {
					yield return new Item(ModContent.ItemType<RainbowLootBox>());
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
				if (Secert_PototoVer) {
					yield return new Item(ModContent.ItemType<SlipGun>());
					yield return new Item(ModContent.ItemType<FairFrozen>());
					yield return new Item(ItemID.MusketBall, 350);
					yield return new Item(ItemID.PotatoChips, 99);
				}
				if (Player.name.Trim() == "hmdebug") {
					yield return new Item(ModContent.ItemType<LootboxLordSummon>());
					yield return new Item(ModContent.ItemType<IronLootBox>());
					yield return new Item(ModContent.ItemType<SilverLootBox>());
					yield return new Item(ModContent.ItemType<GoldLootBox>());
					yield return new Item(ModContent.ItemType<CorruptionLootBox>());
					yield return new Item(ModContent.ItemType<CrimsonLootBox>());
					yield return new Item(ModContent.ItemType<IceLootBox>());
					yield return new Item(ModContent.ItemType<HoneyLootBox>());
					yield return new Item(ModContent.ItemType<WorldEssence>(), 2);
					yield return new Item(ItemID.PlatinumCoin, 2);
					LifeCrystal = Math.Clamp(LifeCrystal + 15, 0, 15);
					ManaCrystal = Math.Clamp(ManaCrystal + 9, 0, 9);
					yield return new Item(ItemID.KingSlimeBossBag);
					yield return new Item(ItemID.EyeOfCthulhuBossBag);
					yield return new Item(ItemID.EaterOfWorldsBossBag);
					yield return new Item(ItemID.BrainOfCthulhuBossBag);
					yield return new Item(ItemID.SkeletronBossBag);
					yield return new Item(ItemID.QueenBeeBossBag);
					yield return new Item(ItemID.DeerclopsBossBag);
					yield return new Item(ItemID.GuideVoodooDoll);
				}
				yield return new Item(ItemID.LifeCrystal, LifeCrystal);
				yield return new Item(ItemID.ManaCrystal, ManaCrystal);
			}
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
		public override void ModifyStartingInventory(IReadOnlyDictionary<string, List<Item>> itemsByMod, bool mediumCoreDeath) {
			itemsByMod["Terraria"].Clear();
		}
		public int amountOfTimeGotHit = 0;
		public bool AllowForAchievement = true;
		public override void OnHurt(Player.HurtInfo info) {
			if (BossRushUtils.IsAnyVanillaBossAlive())
				amountOfTimeGotHit++;
		}

		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) {
			if (Player.difficulty != PlayerDifficultyID.Hardcore) {
				SpawnItem();
			}
		}
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)BossRush.MessageType.GodUltimateChallenge);
			packet.Write((byte)Player.whoAmI);
			packet.Write(EnchantingEnable);
			packet.Write(SkillEnable);
			packet.Write(AllowForAchievement);
			packet.Send(toWho, fromWho);
		}
		public override void Initialize() {
			EnchantingEnable = 0;
			SkillEnable = 0;
			AllowForAchievement = true;
		}
		public override void SaveData(TagCompound tag) {
			tag["EnchantingEnable"] = EnchantingEnable;
			tag["SkillEnable"] = SkillEnable;
			tag["AllowForAchievement"] = AllowForAchievement;
		}
		public override void LoadData(TagCompound tag) {
			if (tag.TryGet("EnchantingEnable", out int value1)) {
				EnchantingEnable = value1;
			}
			if (tag.TryGet("SkillEnable", out int value2)) {
				SkillEnable = value2;
			}
			if (tag.TryGet("AllowForAchievement", out bool all)) {
				AllowForAchievement = all;
			}
		}
		public void ReceivePlayerSync(BinaryReader reader) {
			EnchantingEnable = reader.ReadInt32();
			SkillEnable = reader.ReadInt32();
			AllowForAchievement = reader.ReadBoolean();
		}

		public override void CopyClientState(ModPlayer targetCopy) {
			ModdedPlayer clone = (ModdedPlayer)targetCopy;
			clone.EnchantingEnable = EnchantingEnable;
			clone.SkillEnable = SkillEnable;
			clone.AllowForAchievement = AllowForAchievement;
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
			ModdedPlayer clone = (ModdedPlayer)clientPlayer;
			if (EnchantingEnable != clone.EnchantingEnable
				|| SkillEnable != clone.SkillEnable
				|| AllowForAchievement != clone.AllowForAchievement) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}
	}
}
public class SimpleExtraJump : ExtraJump {
	public override Position GetDefaultPosition() => new After(BlizzardInABottle);

	public override float GetDurationMultiplier(Player player) {
		return 1.25f;
	}

	public override void UpdateHorizontalSpeeds(Player player) {
		player.runAcceleration *= 1.2f;
		player.maxRunSpeed *= 2f;
	}

	public override void OnStarted(Player player, ref bool playSound) {
		// Use this hook to trigger effects that should appear at the start of the extra jump
		// This example mimics the logic for spawning the puff of smoke from the Cloud in a Bottle
		int offsetY = player.height;
		if (player.gravDir == -1f)
			offsetY = 0;

		offsetY -= 16;

		SpawnCloudPoof(player, player.Top + new Vector2(-16f, offsetY));
		SpawnCloudPoof(player, player.position + new Vector2(-36f, offsetY));
		SpawnCloudPoof(player, player.TopRight + new Vector2(4f, offsetY));
	}

	private static void SpawnCloudPoof(Player player, Vector2 position) {
		Gore gore = Gore.NewGoreDirect(player.GetSource_FromThis(), position, -player.velocity, Main.rand.Next(11, 14));
		gore.velocity.X = gore.velocity.X * 0.1f - player.velocity.X * 0.1f;
		gore.velocity.Y = gore.velocity.Y * 0.1f - player.velocity.Y * 0.05f;
	}

	public override void ShowVisuals(Player player) {
		// Use this hook to trigger effects that should appear throughout the duration of the extra jump
		// This example mimics the logic for spawning the dust from the Blizzard in a Bottle
		int offsetY = player.height - 6;
		if (player.gravDir == -1f)
			offsetY = 6;

		Vector2 spawnPos = new Vector2(player.position.X, player.position.Y + offsetY);

		for (int i = 0; i < 2; i++) {
			SpawnBlizzardDust(player, spawnPos, 0.1f, i == 0 ? -0.07f : -0.13f);
		}
	}

	private static void SpawnBlizzardDust(Player player, Vector2 spawnPos, float dustVelocityMultiplier, float playerVelocityMultiplier) {
		Dust dust = Dust.NewDustDirect(spawnPos, player.width, 12, DustID.Snow, player.velocity.X * 0.3f, player.velocity.Y * 0.3f, newColor: Color.White);
		dust.fadeIn = 1.5f;
		dust.velocity *= dustVelocityMultiplier;
		dust.velocity += player.velocity * playerVelocityMultiplier;
		dust.noGravity = true;
		dust.noLight = true;
	}
}
