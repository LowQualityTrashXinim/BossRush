using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items;
using BossRush.Contents.Items.Chest;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Contents.Perks;
using BossRush.Common.Systems;
using BossRush.Contents.WeaponEnchantment;
using BossRush.Contents.Arguments;

namespace BossRush.Contents.Transfixion.Artifacts {
	internal class TokenOfGreedArtifact : Artifact {
		public override Color DisplayNameColor => Color.Navy;
	}

	public class GreedPlayer : ModPlayer {
		bool Greed = false;
		protected ChestLootDropPlayer chestmodplayer => Player.GetModPlayer<ChestLootDropPlayer>();
		public override void ResetEffects() {
			Greed = Player.HasArtifact<TokenOfGreedArtifact>();
		}
		public override void PostUpdate() {
			if (Greed) {
				chestmodplayer.DropModifier += 1;
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (!Greed) {
				return;
			}
			if (Player.GetModPlayer<SynergyModPlayer>().CompareOldvsNewItemType) {
				damage *= .85f;
			}
		}
	}
	public class GreedPrivilage : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 2;
		}
		public override bool SelectChoosing() {
			return Artifact.PlayerCurrentArtifact<TokenOfGreedArtifact>();
		}
		public override void UpdateEquip(Player player) {
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.LootDropIncrease, Base: 1 * StackAmount(player));
			PlayerStatsHandle handle = player.GetModPlayer<PlayerStatsHandle>();
			handle.AugmentationChance += .2f * StackAmount(player);
			handle.RandomizeChanceEnchantment += .05f * StackAmount(player);
		}
	}
	public class TheBigMoment : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 5;
		}
		public override bool SelectChoosing() {
			return Artifact.PlayerCurrentArtifact<TokenOfGreedArtifact>();
		}
		public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
			float chance = player.GetModPlayer<PlayerStatsHandle>().ChestLoot.DropModifier.ApplyTo(1);
			if (Main.rand.NextFloat() <= chance * .01f * StackAmount(player)) {
				modifiers.SourceDamage *= 2;
			}
			else if (Main.rand.NextFloat() <= chance * .025f * StackAmount(player)) {
				modifiers.SourceDamage += 1;
			}
			else if (Main.rand.NextFloat() <= chance * .05f * StackAmount(player)) {
				player.Heal((int)chance);
			}
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			float chance = player.GetModPlayer<PlayerStatsHandle>().ChestLoot.DropModifier.ApplyTo(1);
			if (Main.rand.NextFloat() <= chance * .01f * StackAmount(player)) {
				modifiers.SourceDamage *= 2;
			}
			else if (Main.rand.NextFloat() <= chance * .025f * StackAmount(player)) {
				modifiers.SourceDamage += 1;
			}
			else if (Main.rand.NextFloat() <= chance * .05f * StackAmount(player)) {
				player.Heal((int)chance);
			}
		}
	}
}
