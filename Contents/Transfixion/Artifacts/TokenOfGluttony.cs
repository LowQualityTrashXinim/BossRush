using BossRush.Common.Systems.ArtifactSystem;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using BossRush.Texture;
using Terraria;
using System;
using BossRush.Contents.Perks;
using BossRush.Common.Systems.Achievement;
using BossRush.Common.Systems;
using System.Linq;

namespace BossRush.Contents.Transfixion.Artifacts;
internal class TokenOfGluttonyArtifact : Artifact {
	public override string TexturePath => BossRushTexture.Get_MissingTexture("Artifact");
	public override Color DisplayNameColor => Color.LimeGreen;
}
public class TokenOfGluttonyPlayer : ModPlayer {
	bool TokenOfGluttony = false;
	public override void SetStaticDefaults() {
		DataStorer.AddContext("Artifact_ToG", new(300, Vector2.Zero, false, Color.Brown));
	}
	public override void ResetEffects() {
		TokenOfGluttony = Player.HasArtifact<TokenOfGluttonyArtifact>();
		if (TokenOfGluttony) {
			DataStorer.ActivateContext(Player, "Artifact_ToG");
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!TokenOfGluttony) {
			return;
		}
		Player.Heal(Main.rand.Next(1, 5));
	}
	public bool IsWithinTheRadius(Vector2 player, Vector2 entity) => player.DistanceSQ(entity) <= 300;
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (!TokenOfGluttony) {
			return;
		}
		if (!IsWithinTheRadius(Player.Center, target.Center)) {
			modifiers.SourceDamage -= .45f;
		}
	}
	public override void UpdateEquips() {
		if (TokenOfGluttony) {
			Player.endurance += (1 - PercentageRatio) / 2f;
			Player.GetModPlayer<PlayerStatsHandle>().BuffTime -= .9f;
			if (Player.buffType.Where(b => b != 0 && !Main.buffNoTimeDisplay[b]).Any()) {
				Player.endurance += .1f;
			}
		}
	}
	public float PercentageRatio => Player.statLife / (float)Player.statLifeMax2;
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (!TokenOfGluttony) {
			return;
		}
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
		return Artifact.PlayerCurrentArtifact<TokenOfGluttonyArtifact>() || AchievementSystem.IsAchieved("TokenOfGluttony");
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
		return Artifact.PlayerCurrentArtifact<TokenOfGluttonyArtifact>() || AchievementSystem.IsAchieved("TokenOfGluttony");
	}
	public override void UpdateEquip(Player player) {
		if (!player.IsHealthAbovePercentage(.4f)) {
			player.endurance += 0.1f * StackAmount(player);
		}
		else if (player.statLife >= player.statLifeMax2) {
			player.endurance += 1f;
		}
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!player.IsHealthAbovePercentage(.6f)) {
			player.Heal(Math.Clamp((int)(hit.Damage * .1f), 0, 100));
		}
	}
}
