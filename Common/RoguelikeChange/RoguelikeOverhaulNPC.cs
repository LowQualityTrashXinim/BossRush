using Terraria;
using Terraria.ModLoader;
using BossRush.Contents.Items.Accessories.LostAccessories;
using BossRush.Common.Systems;
using Terraria.ID;
using System;
using Terraria.ModLoader.UI.ModBrowser;

namespace BossRush.Common.RoguelikeChange;
internal class RoguelikeOverhaulNPC : GlobalNPC {
	public override bool InstancePerEntity => true;
	public int HeatRay_Decay = 0;
	public int HeatRay_HitCount = 0;
	public StatModifier StatDefense = new StatModifier();
	public bool DRFromFatalAttack = false;
	public int DRTimer = 0;
	public override void SetDefaults(NPC entity) {
		StatDefense = new();
	}
	public override void ResetEffects(NPC npc) {
		StatDefense = new();
		if (--DRTimer <= 0) {
			DRFromFatalAttack = false;
		}
		else {
			DRFromFatalAttack = true;
		}
	}
	public override void PostAI(NPC npc) {
		if (HeatRay_HitCount > 0) {
			HeatRay_Decay = BossRushUtils.CountDown(HeatRay_Decay);
			if (HeatRay_Decay <= 0) {
				HeatRay_HitCount--;
			}
		}
	}
	public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers) {
		if (npc.boss) {
			modifiers.FinalDamage.Flat += (int)(target.statLifeMax2 * .1f);
		}
	}
	public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
		if (npc.boss) {
			if (Main.rand.NextBool(20)) {
				modifiers.SetMaxDamage(1);
			}
			if (DRFromFatalAttack) {
				modifiers.FinalDamage *= .35f;
			}
		}
		modifiers.Defense = modifiers.Defense.CombineWith(StatDefense);
	}
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		if (npc.boss) {
			if (Main.rand.NextBool(20)) {
				modifiers.SetMaxDamage(1);
			}
			if (DRFromFatalAttack) {
				modifiers.FinalDamage *= .35f;
			}
		}
		if (projectile.type == ProjectileID.HeatRay) {
			modifiers.SourceDamage += HeatRay_HitCount * .02f;
		}
		if ((projectile.minion || projectile.DamageType == DamageClass.Summon) && npc.HasBuff<Crystalized>() && Main.rand.NextBool(10)) {
			modifiers.SourceDamage += .55f;
		}
		modifiers.Defense = modifiers.Defense.CombineWith(StatDefense);
	}
	public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
		if (!npc.boss) {
			return;
		}
		if (npc.lifeMax / (float)hit.Damage >= npc.lifeMax * .1f) {
			if (!DRFromFatalAttack) {
				npc.life += npc.lifeMax / 10;
			}
			DRTimer = BossRushUtils.ToMinute(1);
			DRFromFatalAttack = true;
		}
		if (hit.Crit) {
			if (Main.rand.NextBool(10)) {
				npc.Heal(hit.Damage * 2);
			}
		}
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		if (projectile.type == ProjectileID.HeatRay) {
			HeatRay_HitCount = Math.Clamp(HeatRay_HitCount + 1, 0, 100);
			HeatRay_Decay = 30;
		}
		if (!npc.boss) {
			return;
		}
		if (npc.lifeMax / (float)hit.Damage >= npc.lifeMax * .1f) {
			if (!DRFromFatalAttack) {
				npc.life += npc.lifeMax / 10;
			}
			DRTimer = BossRushUtils.ToMinute(1);
			DRFromFatalAttack = true;
		}
		if (hit.Crit) {
			if (Main.rand.NextBool(10)) {
				npc.Heal(hit.Damage * 2);
			}
		}
	}
	public override void OnKill(NPC npc) {
		int playerIndex = npc.lastInteraction;
		if (!Main.player[playerIndex].active || Main.player[playerIndex].dead) {
			playerIndex = npc.FindClosestPlayer();
		}
		Player player = Main.player[playerIndex];
		player.GetModPlayer<PlayerStatsHandle>().successfullyKillNPCcount++;
		if (npc.boss && player.GetModPlayer<GamblePlayer>().GodDice) {
			player.GetModPlayer<GamblePlayer>().Roll++;
		}
		if (player.GetModPlayer<KillingThrillPlayer>().KillingThrill) {
			player.GetModPlayer<KillingThrillPlayer>().KillCount_Decay++;
		}
	}
}
