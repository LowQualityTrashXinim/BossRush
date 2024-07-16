using BossRush.Contents.Items.NohitReward;
using BossRush.Contents.Items.Spawner;
using BossRush.Contents.Items.Chest;
//EnragedStuff
using Terraria.GameContent.ItemDropRules;
using BossRush.Contents.Items.Toggle;
using Terraria.DataStructures;
using BossRush.Contents.Perks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace BossRush.Common {
	class GlobalNPCMod : GlobalNPC {
		public override void OnSpawn(NPC npc, IEntitySource source) {
			if (!npc.boss) {
				npc.damage += Main.rand.Next((int)(npc.damage * .5f) + 1);
				npc.lifeMax += Main.rand.Next((int)(npc.lifeMax * .5f) + 1);
				npc.defense += Main.rand.Next((int)(npc.defense * .5f) + 1);
				npc.life = npc.lifeMax;
			}
		}
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
			int lifecrystal = 1;
			int manacrystal = 1;
			LeadingConditionRule ExpertVSnormal = new LeadingConditionRule(new Conditions.LegacyHack_IsBossAndNotExpert());
			LeadingConditionRule noHit = new LeadingConditionRule(new GitGudMode());
			LeadingConditionRule DropOnce = new LeadingConditionRule(new IsPlayerAlreadyHaveASpawner());
			LeadingConditionRule IsABoss = new(new Conditions.LegacyHack_IsABoss());
			LeadingConditionRule SynergyRule = new(new SynergyDrop());
			if (npc.type == NPCID.KingSlime) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<KSNoHitReward>()));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<IronLootBox>()));
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ItemID.SuspiciousLookingEye));
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
				SynergyRule.OnSuccess(ItemDropRule.ByCondition(new EvilBossChallengeModeException(), ModContent.ItemType<PerkChooser>()));
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
				SynergyRule.OnSuccess(ItemDropRule.ByCondition(new EvilBossChallengeModeException(), ModContent.ItemType<PerkChooser>()));
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
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BloodLootBox>()));
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<BloodLootBox>()));
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
				SynergyRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<PerkChooser>()));
			}
			if (npc.type == NPCID.Golem) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<LihzahrdTreasureChest>(), 1, 2, 2));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<LihzahrdTreasureChest>()));
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ModContent.ItemType<LunaticTablet>()));
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ItemID.TruffleWorm));
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ItemID.EmpressButterfly, 1, 5, 5));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<LihzahrdTreasureChest>()));
			}
			if (npc.type == NPCID.HallowBoss) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BlackLootBox>(), 1, 2, 2));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BlackLootBox>()));
				//Enraged boss drop
				npcLoot.Add(ItemDropRule.BossBagByCondition(new Conditions.EmpressOfLightIsGenuinelyEnraged(), ModContent.ItemType<EmpressTreasureChest>()));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<BlackLootBox>()));
			}
			if (npc.type == NPCID.DukeFishron) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DukeTreasureChest>(), 1, 2, 2));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BlackLootBox>()));
				//Enraged boss drop
				LeadingConditionRule rule = new LeadingConditionRule(new DukeIsEnrage());
				rule.OnSuccess(ItemDropRule.BossBag(ModContent.ItemType<DukeTreasureChest>()));
				npcLoot.Add(rule);
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<BlackLootBox>()));
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
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BlackLootBox>(), 1, 2, 2));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<MoonTreasureChest>()));
			}
			IsABoss.OnSuccess(ItemDropRule.ByCondition(new LifeCrystalMax(), ItemID.LifeCrystal, 1, lifecrystal, lifecrystal));
			IsABoss.OnSuccess(ItemDropRule.ByCondition(new ManaCrystalMax(), ItemID.ManaCrystal, 1, manacrystal, manacrystal));
			npcLoot.Add(noHit);
			npcLoot.Add(ExpertVSnormal);
			npcLoot.Add(IsABoss);
		}
		public override void OnKill(NPC npc) {
			if (npc.boss) {
				int playerIndex = npc.lastInteraction;
				if (!Main.player[playerIndex].active || Main.player[playerIndex].dead) {
					playerIndex = npc.FindClosestPlayer();
				}
				Player player = Main.player[playerIndex];
				player.GetModPlayer<GamblePlayer>().Roll++;
			}
		}
	}
}
