using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Contents.Perks;
using BossRush.Common.Systems.Achievement;
using BossRush.Common.Systems;
using System.Collections.Generic;

namespace BossRush.Contents.Artifacts {
	internal class HeartOfEarthArtifact : Artifact {
		public override Color DisplayNameColor => Color.Red;
	}

	class HeartOfEarthPlayer : ModPlayer {
		bool Earth = false;
		public override void ResetEffects() {
			Earth = Player.ActiveArtifact() == Artifact.ArtifactType<HeartOfEarthArtifact>();
		}
		int ShortStanding = 0;
		int OnHitDelay = 0;
		public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
			base.ModifyMaxStats(out health, out mana);
			if (Earth) {
				health.Flat += 100;
				health += .1f;
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (Earth) {
				damage *= Player.statLife / (float)Player.statLifeMax2;
			}
		}
		public override void PostUpdate() {
			if (!Earth || OnHitDelay > 0) {
				OnHitDelay = BossRushUtils.CountDown(OnHitDelay);
				return;
			}
			if (Player.velocity == Vector2.Zero) {
				ShortStanding++;
				if (ShortStanding > 120) {//2s required
					if (ShortStanding % Math.Clamp(10 - ShortStanding / 100, 1, 10) == 0) {
						Player.statLife = Math.Clamp(Player.statLife + 1, 0, Player.statLifeMax2);
					}
				}
			}
			else {
				ShortStanding = 0;
			}
		}
		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
			if (!Earth) {
				return;
			}
			ShortStanding = (int)(ShortStanding * .75f);
			OnHitDelay = BossRushUtils.ToSecond(.5f);
		}
		public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
			if (!Earth) {
				return;
			}
			ShortStanding = (int)(ShortStanding * .75f);
			OnHitDelay = BossRushUtils.ToSecond(.5f);
		}
	}

	public class EarthlyWarm : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 3;
		}
		public override bool SelectChoosing() {
			return Artifact.PlayerCurrentArtifact<HeartOfEarthArtifact>() || AchievementSystem.IsAchieved("HeartOfEarth");
		}
		public override void UpdateEquip(Player player) {
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, Additive: 1f + 0.5f * StackAmount(player));
		}
		public override void OnHitByAnything(Player player) {
			if (Main.rand.NextBool(Main.rand.Next(3, Math.Clamp(11 - StackAmount(player), 4, 10)))) {
				player.Heal(Main.rand.Next((int)(player.statLife / (Math.Clamp(2f - .1f * StackAmount(player), .1f, 9)))));
			}
			if (Main.rand.NextBool(200)) {
				player.Heal(player.statLifeMax2);
			}
		}
	}
	public class NaturalThorn : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 3;
		}
		public override bool SelectChoosing() {
			return Artifact.PlayerCurrentArtifact<HeartOfEarthArtifact>() || AchievementSystem.IsAchieved("HeartOfEarth");
		}
		public override void UpdateEquip(Player player) {
			int stack = StackAmount(player);
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.MaxHP, 1 + .1f * stack);
			player.Center.LookForHostileNPC(out List<NPC> npclist, 200);
			foreach (NPC npc in npclist) {
				if (npc.immune[player.whoAmI] <= 0)
					player.StrikeNPCDirect(npc, npc.CalculateHitInfo((player.statLifeMax2 - player.statLife) * stack, 1));
			}
		}
		public override void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) {
			NPC.HitInfo hit = new();
			hit.Damage = (int)player.GetModPlayer<PlayerStatsHandle>().UpdateHPRegen.ApplyTo(hurtInfo.Damage * .25f * StackAmount(player)) + player.lifeRegen;
			hit.Knockback = 10;
			hit.HitDirection = BossRushUtils.DirectionFromPlayerToNPC(player.Center.X, npc.Center.X);
			player.StrikeNPCDirect(npc, hit);
		}
	}
}
