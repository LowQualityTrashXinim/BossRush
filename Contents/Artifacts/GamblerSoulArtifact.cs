using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossRush.Common.Systems;
using BossRush.Contents.Perks;
using BossRush.Contents.Skill;
using Microsoft.Xna.Framework;
using BossRush.Contents.WeaponEnchantment;
using BossRush.Common.Systems.ArtifactSystem;
using System;
using BossRush.Common.Systems.Achievement;

namespace BossRush.Contents.Artifacts;
internal class GamblerSoulArtifact : Artifact {
	public override Color DisplayNameColor => Color.LightGoldenrodYellow;
	public override string TexturePath => BossRushTexture.Get_MissingTexture("Artifact");
}
public class GamblerSoulPlayer : ModPlayer {
	bool GamblerSoul = false;
	int AlreadyGotGivenItem = 0;
	public override void ResetEffects() {
		GamblerSoul = Player.HasArtifact<GamblerSoulArtifact>();
	}
	public override void OnEnterWorld() {
		if (GamblerSoul && AlreadyGotGivenItem == 0) {
			Player.GetModPlayer<SkillHandlePlayer>().RequestAddSkill_Inventory(ModSkill.GetSkillType<AllOrNothing>(), false);
			Player.QuickSpawnItem(null, ModContent.ItemType<LuckEssence>());
			AlreadyGotGivenItem++;
		}
	}
	public override bool CanUseItem(Item item) {
		if (GamblerSoul)
			return item.type != ModContent.ItemType<EnchantmentTablet>();
		return base.CanUseItem(item);
	}
	public override void UpdateEquips() {
		if (GamblerSoul)
			Player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.LootDropIncrease, Base: 1);
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (GamblerSoul)
			modifiers.DamageVariationScale *= 1.65f;
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (GamblerSoul) {
			modifiers.FinalDamage += .2f;
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (GamblerSoul) {
			modifiers.FinalDamage += .2f;
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
		if (Main.rand.NextFloat() <= Main.rand.NextFloat(.20f, .70f)) {
			modifiers.FinalDamage.Flat -= Main.rand.Next(1, 1 + (int)Math.Ceiling(npc.damage * .85f));
		}
	}
	public override void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers) {
		if (Main.rand.NextFloat() <= Main.rand.NextFloat(.20f, .70f)) {
			modifiers.FinalDamage.Flat -= Main.rand.Next(1, 1 + (int)Math.Ceiling(proj.damage * .85f));
		}
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextFloat() <= Main.rand.NextFloat(.20f, .70f)) {
			modifiers.SourceDamage += Main.rand.NextFloat(.15f, 1f);
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextFloat() <= Main.rand.NextFloat(.20f, .70f)) {
			modifiers.SourceDamage += Main.rand.NextFloat(.15f, 1f);
		}
	}
	public override bool FreeDodge(Player player, Player.HurtInfo hurtInfo) {
		if (!player.immune && Main.rand.NextFloat() <= Main.rand.NextFloat(.1f, .9f)) {
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
