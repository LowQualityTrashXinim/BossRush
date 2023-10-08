using BossRush.Contents.BuffAndDebuff;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Artifact {
	internal class VampirismCrystal : ArtifactModItem {
		public override void SetStaticDefaults() {
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 7));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}
		public override void ArtifactSetDefault() {
			width = 32; height = 58;
			Item.rare = ItemRarityID.Cyan;
		}
	}
	public class VampirePlayer : ModPlayer {
		bool Vampire = false;
		int vampirecountRange = 0;
		public override void ResetEffects() {
			Vampire = Player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID == ModContent.ItemType<VampirismCrystal>();
		}
		public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
			base.ModifyMaxStats(out health, out mana);
			if (Vampire) {
				health.Base = -(Player.statLifeMax * 0.55f);
			}
		}
		public override void PostUpdate() {
			if (Vampire) {
				Player.AddBuff(BuffID.PotionSickness, 600);
			}
		}
		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			if (Vampire) {
				LifeSteal(target, 3, 6, Main.rand.NextFloat(1, 3));
			}
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (Vampire) {
				vampirecountRange++;
				if (vampirecountRange >= 3) {
					LifeSteal(target, 1, 5);
					vampirecountRange = 0;
				}
			}
		}
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
			if (!Player.HasBuff(ModContent.BuffType<SecondChance>()) && Vampire) {
				Player.Heal(Player.statLifeMax2);
				Player.AddBuff(ModContent.BuffType<SecondChance>(), 18000);
				return false;
			}
			return true;
		}
		private void LifeSteal(NPC target, int rangeMin = 1, int rangeMax = 3, float multiplier = 1) {
			if (target.lifeMax > 5 && !target.friendly && target.type != NPCID.TargetDummy) {
				int HP = (int)(Main.rand.Next(rangeMin, rangeMax) * multiplier);
				int HPsimulation = Player.statLife + HP;
				if (HPsimulation < Player.statLifeMax2) {
					Player.Heal(HP);
				}
				else {
					Player.statLife = Player.statLifeMax2;
				}
			}
		}
	}
}