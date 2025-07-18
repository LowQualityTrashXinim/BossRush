﻿using BossRush.Contents.Items.Chest;
//EnragedStuff
using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using BossRush.Contents.Perks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using System;
using BossRush.Common.Systems;
using BossRush.Contents.Items.Consumable.Spawner;
using BossRush.Contents.Items.Consumable.SpecialReward;
using BossRush.Contents.Transfixion.WeaponEnchantment;
using BossRush.Contents.Items;
using BossRush.Common.General;
using BossRush.Contents.Items.Consumable;

namespace BossRush.Common.Global {
	class GlobalNPCMod : GlobalNPC {
		public override void OnSpawn(NPC npc, IEntitySource source) {
			if (!npc.boss && Array.IndexOf(new int[] { NPCID.EaterofWorldsBody, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsTail }, npc.type) > -1 && npc.type != NPCID.Creeper) {
				npc.damage += Main.rand.Next((int)(npc.damage * .5f) + 1);
				npc.lifeMax += Main.rand.Next((int)(npc.lifeMax * .5f) + 1);
				npc.defense += Main.rand.Next((int)(npc.defense * .5f) + 1);
				npc.life = npc.lifeMax;
			}
		}
		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
			if (npc.HasBuff<Marked>()) {
				modifiers.CritDamage += 1;
			}
		}
		public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
			if (npc.HasBuff<Marked>()) {
				modifiers.CritDamage += 1;
			}
		}
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
			var ExpertVSnormal = new LeadingConditionRule(new Conditions.LegacyHack_IsBossAndNotExpert());
			var noHit = new LeadingConditionRule(new GitGudMode());
			var dontHit = new LeadingConditionRule(new DontHitBoss());
			LeadingConditionRule IsABoss = new(new Conditions.LegacyHack_IsABoss());
			LeadingConditionRule legacyLootboxCheck = new(new CheckLegacyLootboxBoss());
			if(npc.boss) {
				npcLoot.Add(ItemDropRule.BossBagByCondition(new LootBoxLordDrop(), ModContent.ItemType<LootboxLordSummon>()));
			}
			if (npc.type == NPCID.KingSlime) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<KSNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<KSDonHitReward>()));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<IronLootBox>()));
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ItemID.SuspiciousLookingEye));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<IronLootBox>()));
			}
			else if (npc.type == NPCID.EyeofCthulhu) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<EoCNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<EoCDonHitReward>()));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SilverLootBox>()));
				//Expert Mode drop
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ItemID.WormFood));
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ItemID.BloodySpine));
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<SilverLootBox>()));
			}
			else if (Array.IndexOf(new int[] { NPCID.EaterofWorldsBody, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsTail }, npc.type) > -1) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<EoWNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<EoWDonHitReward>()));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBagByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<CorruptionLootBox>()));
				//normal drop
				legacyLootboxCheck.OnSuccess(ItemDropRule.ByCondition(new EvilBossChallengeModeException(), ModContent.ItemType<PreHardmodeBossBundle>()));
				legacyLootboxCheck.OnSuccess(ItemDropRule.ByCondition(new EvilBossChallengeModeException(), ModContent.ItemType<ItemBundle>()));
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<CorruptionLootBox>()));
			}
			else if (npc.type == NPCID.BrainofCthulhu) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BoCNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BoCDonHitReward>()));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<CrimsonLootBox>()));
				//Normal mode drop
				legacyLootboxCheck.OnSuccess(ItemDropRule.ByCondition(new EvilBossChallengeModeException(), ModContent.ItemType<PreHardmodeBossBundle>()));
				legacyLootboxCheck.OnSuccess(ItemDropRule.ByCondition(new EvilBossChallengeModeException(), ModContent.ItemType<ItemBundle>()));
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<CrimsonLootBox>()));
			}
			else if (npc.type == NPCID.QueenBee) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<QueenBeeNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<QueenBeeDonHitReward>()));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HoneyLootBox>()));
				//Enraged boss drop
				npcLoot.Add(ItemDropRule.BossBagByCondition(new QueenBeeEnranged(), ModContent.ItemType<HoneyLootBox>()));
				//Expert drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<HoneyLootBox>()));
			}
			else if (npc.type == NPCID.SkeletronHead) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SkeletronNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SkeletronDonHitReward>()));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<GoldLootBox>()));
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ItemID.GuideVoodooDoll));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<GoldLootBox>()));
				npcLoot.Add(ItemDropRule.BossBagByCondition(new NoHitAndIsRakan(), ItemID.Handgun));
			}
			else if (npc.type == NPCID.Deerclops) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DeerclopNoHitReward>()));
				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DeerclopDonHitReward>()));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<IceLootBox>()));
				//Enraged boss drop
				var hateyoulol = new LeadingConditionRule(new DeerclopHateYou());
				hateyoulol.OnSuccess(ItemDropRule.BossBag(ModContent.ItemType<DeerclopTreasureChest>()));
				npcLoot.Add(hateyoulol);
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<IceLootBox>()));
			}
			else if (npc.type == NPCID.WallofFlesh) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<WallOfFleshNoHitReward>()));
				noHit.OnSuccess(ItemDropRule.ByCondition(new NoHitAndIsRakan(), ModContent.ItemType<WeaponBluePrint>())).OnFailedConditions(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<WeaponBluePrint>(), 100));

				dontHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<WallOfFleshDonHitReward>()));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ShadowLootBox>()));
				//npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HardModeBossBundle>()));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<ShadowLootBox>()));
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<WorldEssence>()));
			}
			else if (npc.type == NPCID.BloodNautilus) {
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BloodLootBox>()));
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<BloodLootBox>()));
			}
			else if (npc.type == NPCID.QueenSlimeBoss) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<CrystalLootBox>(), 1, 2, 2));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<CrystalLootBox>()));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<CrystalLootBox>()));
			}
			else if (npc.type == NPCID.TheDestroyer || npc.type == NPCID.SkeletronPrime) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<MechLootBox>(), 1, 2, 2));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<MechLootBox>()));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<MechLootBox>()));
			}
			else if (npc.type == NPCID.Spazmatism || npc.type == NPCID.Retinazer) {
				var leadingConditionRule = new LeadingConditionRule(new Conditions.MissingTwin());
				//NoHit Mode drop
				leadingConditionRule.OnSuccess(ItemDropRule.ByCondition(new GitGudMode(), ModContent.ItemType<MechLootBox>(), 1, 2, 2));
				//Normal mode drop
				leadingConditionRule.OnSuccess(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsBossAndNotExpert(), ModContent.ItemType<MechLootBox>()));
				//Expert mode drop
				leadingConditionRule.OnSuccess(ItemDropRule.BossBag(ModContent.ItemType<MechLootBox>()));
				npcLoot.Add(leadingConditionRule);
			}
			else if (npc.type == NPCID.Plantera) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<NatureLootBox>(), 1, 2, 2));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<NatureLootBox>()));
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ItemID.LihzahrdPowerCell));
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ItemID.LihzahrdAltar));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<NatureLootBox>()));
			}
			else if (npc.type == NPCID.Golem) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<LihzahrdLootBox>(), 1, 2, 2));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<LihzahrdLootBox>()));
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ModContent.ItemType<LunaticTablet>()));
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ItemID.TruffleWorm));
				npcLoot.Add(ItemDropRule.ByCondition(new ChallengeModeException(), ItemID.EmpressButterfly, 1, 5, 5));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<LihzahrdLootBox>()));
			}
			else if (npc.type == NPCID.HallowBoss) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BlackLootBox>(), 1, 2, 2));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BlackLootBox>()));
				//Enraged boss drop
				npcLoot.Add(ItemDropRule.BossBagByCondition(new Conditions.EmpressOfLightIsGenuinelyEnraged(), ModContent.ItemType<EmpressLootBox>()));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<BlackLootBox>()));
			}
			else if (npc.type == NPCID.DukeFishron) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DukeLootBox>(), 1, 2, 2));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BlackLootBox>()));
				//Enraged boss drop
				var rule = new LeadingConditionRule(new DukeIsEnrage());
				rule.OnSuccess(ItemDropRule.BossBag(ModContent.ItemType<DukeLootBox>()));
				npcLoot.Add(rule);
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<BlackLootBox>()));
			}
			else if (npc.type == NPCID.CultistBoss) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<LunaticLootBox>(), 1, 2, 2));
				//Normal mode drop
				ExpertVSnormal.OnSuccess(ItemDropRule.Common(ModContent.ItemType<LunaticLootBox>()));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<LunaticLootBox>()));
			}
			else if (npc.type == NPCID.MoonLordCore) {
				//NoHit mode drop
				noHit.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BlackLootBox>(), 1, 2, 2));
				//Expert mode drop
				npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<MoonLootBox>()));
			}
			IsABoss.OnSuccess(ItemDropRule.ByCondition(new LifeCrystalDrop(), ItemID.LifeCrystal));
			IsABoss.OnSuccess(ItemDropRule.ByCondition(new ManaCrystalDrop(), ItemID.ManaCrystal));
			LeadingConditionRule perkrule = new(new PerkDrop());
			perkrule.OnSuccess(ItemDropRule.ByCondition(new Conditions.LegacyHack_IsABoss(), ModContent.ItemType<WorldEssence>()));
			IsABoss.OnSuccess(ItemDropRule.ByCondition(new SkillUnlockRule(), ModContent.ItemType<SkillSlotUnlock>()));
			npcLoot.Add(perkrule);
			npcLoot.Add(noHit);
			npcLoot.Add(dontHit);
			npcLoot.Add(ExpertVSnormal);
			npcLoot.Add(IsABoss);
			npcLoot.Add(legacyLootboxCheck);
		}
		public override void OnKill(NPC npc) {
			if (npc.boss) {
				var system = ModContent.GetInstance<UniversalSystem>();
				system.ListOfBossKilled.Add(npc.type);
			}
		}
	}
}
