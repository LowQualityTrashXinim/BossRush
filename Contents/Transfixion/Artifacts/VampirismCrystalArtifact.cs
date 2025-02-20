using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Contents.BuffAndDebuff;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Common.Systems;
using BossRush.Contents.Perks;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.BloodyShot;
using BossRush.Contents.Items.Accessories.LostAccessories;
using System;
using BossRush.Common.RoguelikeChange;

namespace BossRush.Contents.Transfixion.Artifacts {
	internal class VampirismCrystalArtifact : Artifact {
		public override int Frames => 7;
		public override Color DisplayNameColor => Color.MediumVioletRed;
	}

	public class VampirePlayer : ModPlayer {
		bool Vampire = false;
		int cooldown = 0;
		public override void ResetEffects() {
			Vampire = Player.HasArtifact<VampirismCrystalArtifact>();
			cooldown = BossRushUtils.CountDown(cooldown);
			PlayerStatsHandle.SetSecondLifeCondition(Player, "VC", !Player.HasBuff(ModContent.BuffType<SecondChance>()) && Vampire);
		}
		public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
			base.ModifyMaxStats(out health, out mana);
			if (Vampire) {
				health -= .55f;
				PerkPlayer perkplayer = Player.GetModPlayer<PerkPlayer>();
				if (perkplayer.perks.ContainsKey(Perk.GetPerkType<VampirismCrystal_Upgrade1>())) {
					health += .4f;
				}
				if (perkplayer.perks.ContainsKey(Perk.GetPerkType<VampirismCrystal_Upgrade2>())) {
					health -= .1f;
				}
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
				LifeSteal(target, 3, 6, Main.rand.NextFloat(1, 3));
			}
		}
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
			if (PlayerStatsHandle.GetSecondLife(Player, "VC")) {
				Player.Heal(Player.statLifeMax2);
				Player.AddBuff(ModContent.BuffType<SecondChance>(), 18000);
				return false;
			}
			return true;
		}
		private void LifeSteal(NPC target, int rangeMin = 1, int rangeMax = 3, float multiplier = 1) {
			if (cooldown > 0) {
				return;
			}
			if (target.lifeMax > 5 && !target.friendly && target.type != NPCID.TargetDummy) {
				cooldown = BossRushUtils.ToSecond(.25f);
				int HP = (int)(Main.rand.Next(rangeMin, rangeMax) * multiplier);
				if (Player.HasPerk<VampirismCrystal_Upgrade2>()) {
					cooldown /= 2;
					HP += Main.rand.Next(1, 10);
				}
				Player.Heal(HP);
			}
		}
	}

	public class VampirismCrystal_Upgrade1 : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
			list_category.Add(PerkCategory.ArtifactExclusive);
		}
		public override bool SelectChoosing() {
			return !Main.LocalPlayer.HasPerk<VampirismCrystal_Upgrade2>() && Main.LocalPlayer.HasArtifact<VampirismCrystalArtifact>();
		}
	}
	public class VampirismCrystal_Upgrade2 : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
			list_category.Add(PerkCategory.ArtifactExclusive);
		}
		public override bool SelectChoosing() {
			return !Main.LocalPlayer.HasPerk<VampirismCrystal_Upgrade1>() && Main.LocalPlayer.HasArtifact<VampirismCrystalArtifact>();
		}
	}
	public class BloodVoodoo : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 5;
		}
		public override void UpdateEquip(Player player) {
			player.GetModPlayer<PlayerStatsHandle>().Set_TemporaryLife(100 * StackAmount(player), 30);
		}
		public override void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) {
			int damage = player.GetModPlayer<PlayerStatsHandle>().TemporaryLife; Vector2 vecR;
			if (Main.rand.NextBool(10)) {
				vecR = (npc.Center - player.Center).SafeNormalize(Vector2.Zero);
				for (int i = 0; i < 6; i++) {
					Vector2 vec = vecR.Vector2DistributeEvenlyPlus(6, 60, i);
					Projectile.NewProjectile(player.GetSource_ItemUse(ContentSamples.ItemsByType[ModContent.ItemType<HeartOfBloodThorn>()].Clone()), player.Center, vec, ProjectileID.SharpTears, damage, 3f, player.whoAmI, 0, Main.rand.NextFloat(.9f, 1.1f));
				}
			}
			vecR = Vector2.One.Vector2RotateByRandom(90);
			for (int i = 0; i < 6; i++) {
				Vector2 vec = vecR.Vector2DistributeEvenly(6, 360, i);
				Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, vec, ModContent.ProjectileType<BloodWater>(), Math.Max(damage / 6, 1), 3f, player.whoAmI);
			}
		}
		public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			Healing(player, hit.Damage);
		}
		public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type)
				Healing(player, hit.Damage);
		}
		public void Healing(Player player, int damage) {
			damage = damage / 4;
			player.GetModPlayer<PlayerStatsHandle>().TemporaryLife += damage;
		}
	}
	public class NightTerror : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void UpdateEquip(Player player) {
			if (Main.IsItDay()) {
				return;
			}
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.RegenHP, 1.25f, Flat: 10 * StackAmount(player));
			modplayer.AddStatsToPlayer(PlayerStats.MeleeDMG, Multiplicative: 1.3f + .1f * StackAmount(player));
			modplayer.AddStatsToPlayer(PlayerStats.CritDamage, Multiplicative: 1.4f + .1f * StackAmount(player));
			modplayer.AddStatsToPlayer(PlayerStats.MaxHP, 1.45f);
			modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1.55f + .1f * StackAmount(player));
			modplayer.AddStatsToPlayer(PlayerStats.JumpBoost, 1.55f + .1f * StackAmount(player));
		}
	}
}
