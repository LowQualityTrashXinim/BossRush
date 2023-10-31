using BossRush.Contents.Items.Accessories.EnragedBossAccessories.EvilEye;
using BossRush.Contents.Items.Accessories.EnragedBossAccessories.KingSlimeDelight;
using BossRush.Contents.Items.NohitReward;
using BossRush.Contents.Items.Spawner;
using BossRush.Contents.Items.Chest;
//EnragedStuff
using Terraria.GameContent.ItemDropRules;
using BossRush.Contents.Perks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace BossRush.Common {
	class GlobalNPCMod : GlobalNPC {
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
			int lifecrystal = 1;
			int manacrystal = 1;
			LeadingConditionRule ExpertVSnormal = new LeadingConditionRule(new Conditions.LegacyHack_IsBossAndNotExpert());
			LeadingConditionRule noHit = new LeadingConditionRule(new GitGudMode());
			LeadingConditionRule DropOnce = new LeadingConditionRule(new IsPlayerAlreadyHaveASpawner());
			LeadingConditionRule IsABoss = new(new Conditions.LegacyHack_IsABoss());
			if (npc.type == NPCID.KingSlime) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.ByCondition(new HardcoreExclusive(),ModContent.ItemType<KSNoHitReward>()));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<IronLootBox>()));
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ItemID.SuspiciousLookingEye));
				//Enraged boss drop
				npcLoot.Add(ItemDropRule.BossBagByCondition(new BossIsEnragedBySpecialSpawner(), ModContent.ItemType<KingSlimeDelight>()));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<IronLootBox>()));
			}
			if (npc.type == NPCID.EyeofCthulhu) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<EoCNoHitReward>()));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SilverLootBox>()));
				DropOnce.OnSuccess(ItemDropRule.ByCondition(new ChallengeModeException(), ItemID.WormFood));
				DropOnce.OnSuccess(ItemDropRule.ByCondition(new ChallengeModeException(), ItemID.BloodySpine));
				npcLoot.Add(DropOnce);
				//Enraged boss drop
				npcLoot.Add(ItemDropRule.BossBagByCondition(new BossIsEnragedBySpecialSpawner(), ItemID.TheEyeOfCthulhu).OnSuccess(ItemDropRule.Common(ModContent.ItemType<EvilEye>())));
				//Expert Mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<SilverLootBox>()));
			}
			if (System.Array.IndexOf(new int[] { NPCID.EaterofWorldsBody, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsTail }, npc.type) > -1) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<EoWNoHitReward>()));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBagByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<CorruptionLootBox>()));
				//normal drop
				lifecrystal += 2;
				IsABoss.OnSuccess(ItemDropRule.ByCondition(new EvilBossChallengeModeException(), ModContent.ItemType<PreHardmodeBossBundle>()));
				IsABoss.OnSuccess(ItemDropRule.ByCondition(new EvilBossChallengeModeException(), ModContent.ItemType<ItemBundle>()));
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<CorruptionLootBox>()));
			}
			if (npc.type == NPCID.BrainofCthulhu) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BoCNoHitReward>()));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<CrimsonLootBox>()));
				//Normal mode drop
				lifecrystal += 2;
				IsABoss.OnSuccess(ItemDropRule.ByCondition(new EvilBossChallengeModeException(), ModContent.ItemType<PreHardmodeBossBundle>()));
				IsABoss.OnSuccess(ItemDropRule.ByCondition(new EvilBossChallengeModeException(), ModContent.ItemType<ItemBundle>()));
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<CrimsonLootBox>()));
			}
			if (npc.type == NPCID.QueenBee) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<QueenBeeNoHitReward>()));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HoneyTreasureChest>()));
				//Enraged boss drop
				npcLoot.Add(ItemDropRule.BossBagByCondition(new QueenBeeEnranged(), ModContent.ItemType<HoneyTreasureChest>()));
				//Expert drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<HoneyTreasureChest>()));
			}
			if (npc.type == NPCID.SkeletronHead) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SkeletronNoHitReward>()));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<GoldLootBox>()));
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ItemID.GuideVoodooDoll));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<GoldLootBox>()));
			}
			if (npc.type == NPCID.Deerclops) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DeerclopNoHitReward>()));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<IceLootBox>()));
				//Enraged boss drop
				LeadingConditionRule hateyoulol = new LeadingConditionRule(new DeerclopHateYou());
				hateyoulol.OnSuccess(ItemDropRule.BossBag(ModContent.ItemType<DeerclopTreasureChest>()));
				npcLoot.Add(hateyoulol);
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<IceLootBox>()));
			}

			if (npc.type == NPCID.WallofFlesh) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<WallOfFleshNoHitReward>()));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ShadowTreasureChest>()));
				lifecrystal += 5;
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HardModeBossBundle>()));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<ShadowTreasureChest>()));
			}

			if (npc.type == NPCID.BloodNautilus) {
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BloodTreasureChest>()));
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<BloodTreasureChest>()));
			}

			if (npc.type == NPCID.QueenSlimeBoss) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<CrystalTreasureChest>(), 1, 2, 2));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<CrystalTreasureChest>()));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<CrystalTreasureChest>()));
			}

			if (npc.type == NPCID.TheDestroyer || npc.type == NPCID.SkeletronPrime) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<MechTreasureChest>(), 1, 2, 2));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<MechTreasureChest>()));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<MechTreasureChest>()));
			}
			if (npc.type == NPCID.Spazmatism || npc.type == NPCID.Retinazer) {
				LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.MissingTwin());
				//NoHit Mode drop
				leadingConditionRule.OnSuccess(ItemDropRule.ByCondition(new GitGudMode(), ModContent.ItemType<MechTreasureChest>(), 1, 2, 2));
				//Normal mode drop
				leadingConditionRule.OnSuccess(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsBossAndNotExpert(), ModContent.ItemType<MechTreasureChest>()));
				//Expert mode drop
				leadingConditionRule.OnSuccess(ItemDropRule.BossBag(ModContent.ItemType<MechTreasureChest>()));
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.Plantera) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<NatureTreasureChest>(), 1, 2, 2));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<NatureTreasureChest>()));
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ItemID.LihzahrdPowerCell));
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ItemID.LihzahrdAltar));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<NatureTreasureChest>()));
			}
			if (npc.type == NPCID.Golem) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<LihzahrdTreasureChest>(), 1, 2, 2));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<LihzahrdTreasureChest>()));
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ModContent.ItemType<LunaticCultistSpawner>()));
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ItemID.TruffleWorm));
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ItemID.EmpressButterfly, 1, 5, 5));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<LihzahrdTreasureChest>()));
			}
			if (npc.type == NPCID.HallowBoss) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BlackTreasureChest>(), 1, 2, 2));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BlackTreasureChest>()));
				//Enraged boss drop
				npcLoot.Add(ItemDropRule.BossBagByCondition(new Conditions.EmpressOfLightIsGenuinelyEnraged(), ModContent.ItemType<EmpressTreasureChest>()));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<BlackTreasureChest>()));
			}
			if (npc.type == NPCID.DukeFishron) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DukeTreasureChest>(), 1, 2, 2));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BlackTreasureChest>()));
				//Enraged boss drop
				LeadingConditionRule rule = new LeadingConditionRule(new DukeIsEnrage());
				rule.OnSuccess(ItemDropRule.BossBag(ModContent.ItemType<DukeTreasureChest>()));
				npcLoot.Add(rule);
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<BlackTreasureChest>()));
			}
			if (npc.type == NPCID.CultistBoss) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<LunaticLootBox>(), 1, 2, 2));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<LunaticLootBox>()));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<LunaticLootBox>()));
			}
			if (npc.type == NPCID.MoonLordCore) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BlackTreasureChest>(), 1, 2, 2));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ModContent.ItemType<MoonLordEnrage>()));
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<MoonTreasureChest>()));
			}
			IsABoss.OnSuccess(ItemDropRule.ByCondition(new LifeCrystalMax(), ItemID.LifeCrystal, 1, lifecrystal, lifecrystal));
			IsABoss.OnSuccess(ItemDropRule.ByCondition(new ManaCrystalMax(), ItemID.ManaCrystal, 1, manacrystal, manacrystal));
			IsABoss.OnSuccess(ItemDropRule.ByCondition(new SynergyDrop(), ModContent.ItemType<PerkChooser>()));
			npcLoot.Add(noHit);
			npcLoot.Add(ExpertVSnormal);
			npcLoot.Add(IsABoss);
		}
	}
}
