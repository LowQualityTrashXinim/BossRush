using Terraria;
using Terraria.ModLoader;
using BossRush.Contents.Items.Accessories.LostAccessories;
using BossRush.Common.Systems;
using Terraria.ID;
using System;

namespace BossRush.Common.RoguelikeChange;
internal class RoguelikeOverhaulNPC : GlobalNPC {
	public override bool InstancePerEntity => true;
	public int HeatRay_Decay = 0;
	public int HeatRay_HitCount = 0;
	public StatModifier StatDefense = new StatModifier();
	public bool DRFromFatalAttack = false;
	public int DRTimer = 0;
	public const int BossHP = 4000;
	public const int BossDMG = 40;
	public const int BossDef = 5;
	/// <summary>
	/// Set this to true if you don't want the mod to apply boss NPC fixed boss's stats
	/// </summary>
	public bool NPC_SpecialException = false;
	public override void SetDefaults(NPC entity) {
		StatDefense = new();
		if (!entity.boss || entity.type == NPCID.WallofFlesh || entity.type == NPCID.WallofFleshEye || !Main.LocalPlayer.IsDebugPlayer() || NPC_SpecialException) {
			return;
		}
		entity.lifeMax = (int)(BossHP * GetValueMulti());
		entity.damage = (int)(BossDMG * GetValueMulti());
		entity.defense = (int)(BossDef * GetValueMulti(.5f));
	}
	public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment) {
		if (!npc.boss || npc.type == NPCID.WallofFlesh || npc.type == NPCID.WallofFleshEye || !Main.LocalPlayer.IsDebugPlayer() ||
			NPC_SpecialException) {
			return;
		}
		float adjustment;
		if (Main.expertMode)
			adjustment = 2;
		else if (Main.masterMode)
			adjustment = 3;
		else
			adjustment = 1;

		npc.lifeMax = (int)(BossHP / adjustment * GetValueMulti());
		npc.life = npc.lifeMax;
		npc.damage = (int)(BossDMG / adjustment * GetValueMulti());
		npc.defense = (int)(BossDef / adjustment * GetValueMulti(.5f));
	}
	public float GetValueMulti(float scale = 1) {
		float extraMultiply = 0;
		if (Main.expertMode) {
			extraMultiply += .25f;
		}
		if (Main.masterMode) {
			extraMultiply += .75f;
		}
		if (Main.getGoodWorld) {
			extraMultiply += 1;
		}
		return (1 + ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Count * .5f + extraMultiply) * scale;
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
		if (hit.Damage >= npc.lifeMax * .1f) {
			if (!DRFromFatalAttack) {
				npc.Heal(npc.lifeMax / 10);
			}
			DRTimer = BossRushUtils.ToMinute(1);
			DRFromFatalAttack = true;
		}
		if (hit.Crit) {
			if (Main.rand.NextBool(10)) {
				npc.Heal(Main.rand.Next(hit.Damage));
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
		if (hit.Damage >= npc.lifeMax * .1f) {
			if (!DRFromFatalAttack) {
				npc.Heal(npc.lifeMax / 10);
			}
			DRTimer = BossRushUtils.ToMinute(1);
			DRFromFatalAttack = true;
		}
		if (hit.Crit) {
			if (Main.rand.NextBool(10)) {
				npc.Heal(Main.rand.Next(hit.Damage));
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
