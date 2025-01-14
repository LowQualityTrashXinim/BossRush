﻿using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.Chest;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Contents.WeaponEnchantment;
using Terraria.ID;
using BossRush.Contents.Perks;
using BossRush.Common.Systems.ArgumentsSystem;
using BossRush.Common.Systems;
using BossRush.Common.Systems.Achievement;
using System;

namespace BossRush.Contents.Artifacts {
	internal class TokenOfPrideArtifact : Artifact {
		public override int Frames => 10;
		public override Color DisplayNameColor => Color.PaleGreen;
	}
	public class PridePlayer : ModPlayer {
		bool Pride = false;
		public override void ResetEffects() {
			Pride = Player.HasArtifact<TokenOfPrideArtifact>();
		}
		public override void UpdateEquips() {
			if (Pride) {
				Player.GetModPlayer<ChestLootDropPlayer>().DropModifier *= 0;
				Player.GetModPlayer<AugmentsPlayer>().IncreasesChance += .65f;
			}
		}
		public override void PreUpdate() {
			if (!Pride) {
				return;
			}
			Item item = Player.HeldItem;
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalitem)) {
				if (globalitem.EnchantmenStlot == null || globalitem.EnchantmenStlot.Length < 1 && EnchantmentGlobalItem.CanBeEnchanted(item)
					|| globalitem.EnchantmenStlot[3] != ItemID.None || globalitem.EnchantmenStlot[3] == -1) {
					return;
				}
				EnchantmentSystem.EnchantItem(ref item, 3);
			}
		}
	}
	public class BlindPride : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 3;
			DataStorer.AddContext("Perk_BlindPride", new(400, Vector2.Zero, false, Color.Yellow));
		}
		public override bool SelectChoosing() {
			return Artifact.PlayerCurrentArtifact<TokenOfPrideArtifact>() || AchievementSystem.IsAchieved("TokenOfPride");
		}
		public override void Update(Player player) {
			DataStorer.ActivateContext(player, "Perk_BlindPride");
			DataStorer.ModifyContextDistance("Perk_BlindPride", 300 + 100 * StackAmount(player));
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .24f + .1f * StackAmount(player);
		}
		public override void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) {
			crit += 5 + 5 * StackAmount(player);
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			if (Vector2.DistanceSquared(player.Center, target.Center) <= MathF.Pow(300f + 100 * StackAmount(player), 2)) {
				return;
			}
			modifiers.FinalDamage *= .5f;
		}
	}
	public class PridefulPossession : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 3;
		}
		public override bool SelectChoosing() {
			return Artifact.PlayerCurrentArtifact<TokenOfPrideArtifact>() || AchievementSystem.IsAchieved("TokenOfPride");
		}
		public override void UpdateEquip(Player player) {
			player.GetModPlayer<AugmentsPlayer>().IncreasesChance += .1f;
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.Defense, Base: player.GetModPlayer<AugmentsPlayer>().valid * StackAmount(player));
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalitem)) {
				int power = globalitem.GetValidNumberOfEnchantment();
				damage += power * .1f * StackAmount(player);
			}
		}
		public override void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) {
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalitem)) {
				int power = globalitem.GetValidNumberOfEnchantment();
				crit += power * StackAmount(player);
			}
		}
	}
}
