using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Contents.Perks;
using BossRush.Common.Systems.Achievement;

namespace BossRush.Contents.Transfixion.Artifacts {
	internal class AlchemistKnowledgeArtifact : Artifact {
		public override string TexturePath => BossRushTexture.Get_MissingTexture("Artifact");
		public override Color DisplayNameColor => Color.PaleVioletRed;
	}
	class AlchemistKnowledgePlayer : ModPlayer {
		bool Alchemist = false;
		public override void ResetEffects() {
			Alchemist = Player.HasArtifact<AlchemistKnowledgeArtifact>();
		}
		public override void UpdateEquips() {
			if (!Alchemist) {
				return;
			}
			int lengthpositive = 0;
			for (int i = 0; i < Player.buffType.Length; i++) {
				int buffType = Player.buffType[i];
				if (buffType == 0) {
					continue;
				}
				if (!Main.debuff[buffType]) {
					lengthpositive++;
				}
				else {
					lengthpositive -= 2;
				}
			}
			PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
			modplayer.BuffTime += .35f;
			modplayer.DebuffBuffTime += 1f;
			modplayer.Transmutation_SuccessChance += .2f;
			if (lengthpositive != 0) {
				if (lengthpositive > 0) {
					modplayer.AddStatsToPlayer(PlayerStats.MaxMinion, Base: lengthpositive * .25f);
					modplayer.AddStatsToPlayer(PlayerStats.MaxSentry, Base: lengthpositive * .25f);
				}
				modplayer.AddStatsToPlayer(PlayerStats.RegenHP, Base: lengthpositive * 1.5f);
				modplayer.AddStatsToPlayer(PlayerStats.RegenMana, Base: lengthpositive * 2);
				modplayer.AddStatsToPlayer(PlayerStats.Defense, Base: lengthpositive);
				modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: lengthpositive * 0.5f);
				modplayer.AddStatsToPlayer(PlayerStats.PureDamage, Additive: lengthpositive * .06f + 1);
				modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, Additive: lengthpositive * 0.085f + 1);
				modplayer.AddStatsToPlayer(PlayerStats.CritDamage, Additive: lengthpositive * 0.15f + 1);
			}
		}
		public override void GetHealLife(Item item, bool quickHeal, ref int healValue) {
			if (!Alchemist) {
				return;
			}
			healValue *= 2;
		}
		public override void GetHealMana(Item item, bool quickHeal, ref int healValue) {
			if (!Alchemist) {
				return;
			}
			healValue *= 2;
		}
	}
	public class BlessedKnowledge : Perk {
		public override void SetDefaults() {
			StackLimit = 3;
			CanBeStack = true;
		}
		public override bool SelectChoosing() {
			return Artifact.PlayerCurrentArtifact<AlchemistKnowledgeArtifact>() || AchievementSystem.IsAchieved("AlchemistKnowledge");
		}
		public override void UpdateEquip(Player player) {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();

			modplayer.AddStatsToPlayer(PlayerStats.HealEffectiveness, 1 + .2f * StackAmount(player));
			modplayer.AddStatsToPlayer(PlayerStats.DebuffDamage, 1 + .12f * StackAmount(player));
		}
		public override void OnUseItem(Player player, Item item) {
			if (item.buffType != 0) {
				PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
				//This is a hacky way to ensure the heal amount is not affected
				modplayer.HealEffectiveness -= .2f * StackAmount(player);
				player.Heal(10 * StackAmount(player));
				modplayer.HealEffectiveness += .2f * StackAmount(player);
			}
		}
	}
	public class AncientKnowledge : Perk {
		public override void SetDefaults() {
			StackLimit = 3;
			CanBeStack = true;
		}
		public override bool SelectChoosing() {
			return Artifact.PlayerCurrentArtifact<AlchemistKnowledgeArtifact>() || AchievementSystem.IsAchieved("AlchemistKnowledge");
		}
		public override void UpdateEquip(Player player) {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			modplayer.Transmutation_SuccessChance += .2f;
			modplayer.DebuffTime += .1f * StackAmount(player);
			modplayer.DebuffBuffTime -= .1f * StackAmount(player);
		}
	}
}
