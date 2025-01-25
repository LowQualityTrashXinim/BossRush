using BossRush.Common.Systems.ArtifactSystem;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using BossRush.Texture;
using Terraria;
using System;
using BossRush.Contents.Perks;

namespace BossRush.Contents.Transfixion.Artifacts;
internal class TokenOfGluttonyArtifact : Artifact {
	public override string TexturePath => BossRushTexture.Get_MissingTexture("Artifact");
	public override Color DisplayNameColor => Color.LimeGreen;
}
public class TokenOfGluttonyPlayer : ModPlayer {
	bool TokenOfGluttony = false;
	public override void ResetEffects() {
		TokenOfGluttony = Player.HasArtifact<TokenOfGluttonyArtifact>();
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (!TokenOfGluttony) {
			return;
		}
		float defensesValue = MathF.Round(Player.statDefense.Positive * (1 + Player.statDefense.AdditiveBonus.Value) - Player.statDefense.Negative, 2);
		float DRvalue = defensesValue / 2f;
		modifiers.ScalingArmorPenetration += 1;
		modifiers.FinalDamage *= Math.Clamp(1 - DRvalue * .01f, .01f, 1f);
		if (Main.rand.NextBool(5)) {
			Player.Heal((int)modifiers.FinalDamage.ApplyTo(npc.damage * .5f));
			modifiers.SetMaxDamage(1);
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (!TokenOfGluttony) {
			return;
		}
		float defensesValue = MathF.Round(Player.statDefense.Positive * (1 + Player.statDefense.AdditiveBonus.Value) - Player.statDefense.Negative, 2);
		float DRvalue = defensesValue / 2f;
		modifiers.ScalingArmorPenetration += 1;
		modifiers.FinalDamage *= Math.Clamp(1 - DRvalue * .01f, .01f, 1f);
		if (Main.rand.NextBool(5)) {
			Player.Heal((int)modifiers.FinalDamage.ApplyTo(proj.damage * .5f));
			modifiers.SetMaxDamage(1);
		}
	}
}
public class EndlessHunger : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 3;
	}
	public override bool SelectChoosing() {
		return Artifact.PlayerCurrentArtifact<TokenOfGluttonyArtifact>();
	}
	public override void UpdateEquip(Player player) {
		player.endurance += .05f * StackAmount(player);
	}
	public override void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) {
		if (player.endurance >= .2f && player.statLife <= 50) {
			player.Heal((int)(hurtInfo.SourceDamage * Safe_MultiScale(player.endurance)));
		}
	}
	public override void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) {
		if (player.endurance >= .2f && player.statLife <= 50) {
			player.Heal((int)(hurtInfo.SourceDamage * Safe_MultiScale(player.endurance)));
		}
	}
	private float Safe_MultiScale(float endurance) => Main.rand.NextFloat(Math.Min(endurance, 1f), Math.Max(endurance, 1f));
}
public class Satisfaction : Perk {
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 3;
	}
	public override bool SelectChoosing() {
		return Artifact.PlayerCurrentArtifact<TokenOfGluttonyArtifact>();
	}
	public override void UpdateEquip(Player player) {
		if (!player.ComparePlayerHealthInPercentage(.4f)) {
			player.endurance += 0.1f * StackAmount(player);
		}
		else if (player.statLife >= player.statLifeMax2) {
			player.endurance += 1f;
		}
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!player.ComparePlayerHealthInPercentage(.6f)) {
			player.Heal(Math.Clamp((int)(hit.Damage * .1f), 0, 100));
		}
	}
}
