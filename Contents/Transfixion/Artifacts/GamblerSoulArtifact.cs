using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossRush.Contents.Perks;
using BossRush.Contents.Skill;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems.ArtifactSystem;
using System;
using BossRush.Common.Systems.Achievement;
using BossRush.Contents.Transfixion.WeaponEnchantment;
using BossRush.Common.Global;
using System.Linq;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ID;

namespace BossRush.Contents.Transfixion.Artifacts;
internal class GamblerSoulArtifact : Artifact {
	public override Color DisplayNameColor => Color.LightGoldenrodYellow;
	public override string TexturePath => BossRushTexture.Get_MissingTexture("Artifact");
}
public class GamblerSoulPlayer : ModPlayer {
	bool GamblerSoul = false;
	int AlreadyGotGivenItem = 0;
	int UnstableTimer = 0;
	public override void OnEnterWorld() {
		if (GamblerSoul && AlreadyGotGivenItem == 0) {
			Player.GetModPlayer<SkillHandlePlayer>().RequestAddSkill_Inventory(ModSkill.GetSkillType<CoinFlip>(), false);
			Player.GetModPlayer<SkillHandlePlayer>().RequestAddSkill_Inventory(ModSkill.GetSkillType<DiceRoll>(), false);
			AlreadyGotGivenItem++;
		}
	}
	public override void ResetEffects() {
		GamblerSoul = Player.HasArtifact<GamblerSoulArtifact>();
		if (!GamblerSoul) {
			return;
		}
		if (!Player.active) {
			return;
		}
		SkillHandlePlayer skillplayer = Player.GetModPlayer<SkillHandlePlayer>();
		if (skillplayer.Activate &&
			skillplayer.GetCurrentActiveSkillHolder().Contains(ModSkill.GetSkillType<CoinFlip>())
			|| skillplayer.GetCurrentActiveSkillHolder().Contains(ModSkill.GetSkillType<DiceRoll>())
			) {
			UnstableTimer = 0;
		}
		if (CheckCoolDown(0, 45)) {
			ConditionMet_OnHit = false;
		}
		if (CheckCoolDown(0, 20)) {
			ConditionMet_MaxHPOnHit = false;
		}
		UnstableTimer++;
		if (CheckUnstableness()) {
			ConditionMet_OnHit = true;
		}
		if (CheckUnstableness(0, 20)) {
			ConditionMet_MaxHPOnHit = true;
		}
		if (CheckUnstableness(0, 30)) {
			ConditionMet_ItemDup = true;
		}
		if (CheckUnstableness(0, 30)) {
			ConditionMet_EnchantmentRandom = true;
		}
		if (CheckUnstableness(2, 30)) {
			ConditionMet_PerkJumbo = true;
		}
		if (CheckUnstableness(3, 30)) {
			ConditionMet_ArtifactChange = true;
		}
		if (CheckUnstableness(0, 20)) {
			ConditionMet_ModifyHit = true;
		}
	}
	public bool CheckCoolDown(int minute = 1, int second = 30) {
		int timer = BossRushUtils.ToMinute(minute) + BossRushUtils.ToSecond(second);
		if (timer == 0) {
			return false;
		}
		return UnstableTimer % timer == 0;
	}
	public bool CheckUnstableness(int minute = 1, int second = 0) {
		int timer = BossRushUtils.ToMinute(minute) + BossRushUtils.ToSecond(second);
		if (timer == 0) {
			return UnstableTimer > 3601;
		}
		return UnstableTimer % timer == 0 && UnstableTimer > 3601;
	}
	bool ConditionMet_ItemDup = false;
	bool ConditionMet_OnHit = false;
	bool ConditionMet_ModifyHit = false;
	bool ConditionMet_MaxHPOnHit = false;
	bool ConditionMet_PerkJumbo = false;
	bool ConditionMet_ArtifactChange = false;
	int ConditionMet_PerkJumbo_Counter = 0;
	bool ConditionMet_EnchantmentRandom = false;
	public override void UpdateEquips() {
		if (!GamblerSoul) {
			return;
		}
		Item item = Player.HeldItem;
		if (ConditionMet_EnchantmentRandom) {
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem enchantitem)) {
				if (Main.rand.NextBool()) {
					for (int i = 0; i < enchantitem.EnchantmenStlot.Length; i++) {
						enchantitem.EnchantmenStlot[i] = Main.rand.Next(EnchantmentLoader.EnchantmentcacheID);
						if (Main.rand.NextBool()) {
							break;
						}
					}
				}
				else {
					for (int i = 0; i < enchantitem.EnchantmenStlot.Length; i++) {
						enchantitem.EnchantmenStlot[i] = ItemID.None;
						if (Main.rand.NextBool()) {
							break;
						}
					}
				}

			}
			ConditionMet_EnchantmentRandom = false;
		}
		if (ConditionMet_ArtifactChange) {
			if (Main.rand.NextBool()) {
				Player.GetModPlayer<ArtifactPlayer>().ActiveArtifact = Main.rand.Next(Artifact.ArtifactCount);
			}
			else {
				ConditionMet_ArtifactChange = false;
			}
		}
		Player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.LootDropIncrease, Base: 1);
		Player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MaxHP, Multiplicative: multiplier);
		if (ConditionMet_ItemDup) {
			if (Main.rand.NextFloat() <= .65f) {
				Player.QuickSpawnItem(Player.GetSource_FromThis(), item);
			}
			else if (Main.rand.NextFloat() <= .01f) {
				item.TurnToAir();
			}
			ConditionMet_ItemDup = false;
		}
		if (ConditionMet_PerkJumbo) {
			ConditionMet_PerkJumbo_Counter++;
			if (ConditionMet_PerkJumbo_Counter >= 180 && Main.rand.NextFloat() <= .1f) {
				ConditionMet_PerkJumbo_Counter = 0;
				PerkPlayer perkplayer = Player.GetModPlayer<PerkPlayer>();
				for (int i = perkplayer.perks.Keys.Count - 1; i >= 0; i--) {
					int perk = perkplayer.perks.Keys.ElementAt(i);
					if (Main.rand.NextBool() && perkplayer.perks.Keys.Count < ModPerkLoader.TotalCount / 2) {
						int value = perkplayer.perks[perk];
						int changePerk = Main.rand.Next(ModPerkLoader.TotalCount);
						while (perk == changePerk && perkplayer.perks.ContainsKey(changePerk)) {
							changePerk = Main.rand.Next(ModPerkLoader.TotalCount);
						}
						perkplayer.perks.Remove(perk);
						perkplayer.perks.Add(changePerk, value);
					}
					else {
						int value = perkplayer.perks[perk];
						perkplayer.perks[perk] += Main.rand.Next(-value, value + 1);
					}
				}
				ConditionMet_PerkJumbo = false;
			}
			else {
				ConditionMet_PerkJumbo_Counter = 0;
			}
		}
	}
	float multiplier = 0;
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (!GamblerSoul) {
			return;
		}
		if (ConditionMet_MaxHPOnHit) {
			multiplier = Main.rand.NextFloat(0, 2.001f);
			ConditionMet_MaxHPOnHit = false;
		}
		if (ConditionMet_OnHit) {
			OnHitNormal(hurtInfo);
			ConditionMet_OnHit = false;
		}
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (!GamblerSoul) {
			return;
		}
		if (ConditionMet_MaxHPOnHit) {
			multiplier = Main.rand.NextFloat(0, 2.001f);
			ConditionMet_MaxHPOnHit = false;
		}
		if (ConditionMet_OnHit) {
			OnHitNormal(hurtInfo);
			ConditionMet_OnHit = false;

		}
	}
	public void OnHitNormal(Player.HurtInfo hurtInfo) {
		if (Main.rand.NextFloat() <= .85f) {
			Player.Heal(hurtInfo.Damage * 2);
		}
		else {
			Player.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromLiteral($"{Player.name} has tasted luck")), 9999999999, hurtInfo.HitDirection);
		}
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (!GamblerSoul) {
			return;
		}
		modifiers.DamageVariationScale *= 1.65f;
		if (ConditionMet_ModifyHit) {
			if (Main.rand.NextFloat() <= .75f) {
				modifiers.FinalDamage += target.lifeMax / 10;
			}
			else {
				modifiers.SetMaxDamage(1);
			}
			ConditionMet_ModifyHit = false;
		}

	}
	public override void SaveData(TagCompound tag) {
		tag.Add("AlreadyGotGivenItem", AlreadyGotGivenItem);
	}
	public override void LoadData(TagCompound tag) {
		AlreadyGotGivenItem = (int)tag["AlreadyGotGivenItem"];
	}
}
public class StrokeOfLuck : Perk {
	public override bool SelectChoosing() {
		return Artifact.PlayerCurrentArtifact<GamblerSoulArtifact>() || AchievementSystem.IsAchieved("GamblerSoul");
	}
	public override void SetDefaults() {
		textureString = BossRushUtils.GetTheSameTextureAsEntity<StrokeOfLuck>();
		CanBeChoosen = false;
		CanBeStack = false;
	}
	public override void ModifyHitByNPC(Player player, NPC npc, ref Player.HurtModifiers modifiers) {
		if (Main.rand.NextFloat() <= .2f) {
			modifiers.FinalDamage.Flat -= Main.rand.Next(1, 1 + (int)Math.Ceiling(npc.damage * .85f));
		}
	}
	public override void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers) {
		if (Main.rand.NextFloat() <= .2f) {
			modifiers.FinalDamage.Flat -= Main.rand.Next(1, 1 + (int)Math.Ceiling(proj.damage * .85f));
		}
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextFloat() <= .2f) {
			modifiers.SourceDamage += Main.rand.NextFloat(.15f, 1f);
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextFloat() <= .2f) {
			modifiers.SourceDamage += Main.rand.NextFloat(.15f, 1f);
		}
	}
	public override bool FreeDodge(Player player, Player.HurtInfo hurtInfo) {
		if (!player.immune && Main.rand.NextFloat() <= .35f) {
			player.AddImmuneTime(hurtInfo.CooldownCounter, Main.rand.Next(44, 89));
			player.immune = true;
			return true;
		}
		return base.FreeDodge(player, hurtInfo);
	}
}
public class UncertainStrike : Perk {
	public override bool SelectChoosing() {
		return Artifact.PlayerCurrentArtifact<GamblerSoulArtifact>() || AchievementSystem.IsAchieved("GamblerSoul");
	}
	public override void SetDefaults() {
		textureString = BossRushUtils.GetTheSameTextureAsEntity<UncertainStrike>();
		CanBeChoosen = false;
		CanBeStack = false;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextFloat() <= .33f) {
			modifiers.SourceDamage += Main.rand.NextFloat(-.15f, .55f);
		}
		if (Main.rand.NextFloat() <= .05f) {
			modifiers.SourceDamage *= 2;
		}
		if (Main.rand.NextFloat() <= .15f) {
			modifiers.ArmorPenetration += 20;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextFloat() <= .33f) {
			modifiers.SourceDamage += Main.rand.NextFloat(-.15f, .55f);
		}
		if (Main.rand.NextFloat() <= .05f) {
			modifiers.SourceDamage *= 2;
		}
		if (Main.rand.NextFloat() <= .15f) {
			modifiers.ArmorPenetration += 20;
		}
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextFloat() <= .01f) {
			player.Heal(Main.rand.Next(hit.Damage));
		}
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextFloat() <= .01f) {
			player.Heal(Main.rand.Next(hit.Damage));
		}
	}
}
