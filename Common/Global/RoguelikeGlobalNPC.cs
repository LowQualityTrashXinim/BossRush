﻿using Terraria;
using Terraria.ModLoader;
using BossRush.Contents.Items.Accessories.LostAccessories;
using BossRush.Common.Systems;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent.ItemDropRules;
using BossRush.Common.General;
using BossRush.Contents.Transfixion.Artifacts;
using System.Collections.Generic;
using Terraria.Audio;
using BossRush.Contents.Perks.BlessingPerk;
using BossRush.Common.Systems.IOhandle;
using BossRush.Contents.Items.Consumable.Throwable;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.SkullRevolver;

namespace BossRush.Common.Global;
internal class RoguelikeGlobalNPC : GlobalNPC {
	public override bool InstancePerEntity => true;
	public int HeatRay_Decay = 0;
	public int HeatRay_HitCount = 0;

	public int GolemFist_HitCount = 0;

	public StatModifier StatDefense = new StatModifier();
	public float Endurance = 0;
	public bool DRFromFatalAttack = false;
	public bool OneTimeDR = false;
	public int DRTimer = 0;
	public const int BossHP = 4000;
	public const int BossDMG = 40;
	public const int BossDef = 5;
	public bool EliteBoss = false;
	/// <summary>
	/// Use this for always update velocity
	/// </summary>
	public float VelocityMultiplier = 1;
	/// <summary>
	/// Use this for permanent effect
	/// </summary>
	public float static_velocityMultiplier = 1;
	/// <summary>
	/// Set this to true if your NPC is a ghost NPC which can't be kill<br/>
	/// Uses this along with <see cref="BelongToWho"/> to make it so that this NPC will die when the parent NPC is killed
	/// </summary>
	public bool IsAGhostEnemy = false;
	public int BelongToWho = -1;
	public bool CanDenyYouFromLoot = false;
	public int PositiveLifeRegen = 0;
	public int PositiveLifeRegenCount = 0;
	/// <summary>
	/// Set this to true if you don't want the mod to apply boss NPC fixed boss's stats
	/// </summary>
	public bool NPC_SpecialException = false;
	public override void SetDefaults(NPC entity) {
		StatDefense = new();
		if (!UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE)) {
			return;
		}
		if (entity.boss && entity.type != NPCID.WallofFlesh && entity.type != NPCID.WallofFleshEye) {
			if (!NPC_SpecialException) {
				entity.lifeMax = (int)(BossHP * GetValueMulti());
				entity.damage = (int)(BossDMG * GetValueMulti());
				entity.defense = (int)(BossDef * GetValueMulti(.5f));
			}
		}
		else {
			float adjustment = 1;
			if (Main.expertMode)
				adjustment = 1.5f;
			else if (Main.masterMode)
				adjustment = 2;

			entity.lifeMax += (int)(entity.lifeMax / adjustment * GetValueMulti() * .1f);
			entity.life = entity.lifeMax;
			entity.damage += (int)(entity.damage / adjustment * GetValueMulti() * .1f);
			entity.defense += (int)(entity.defense / adjustment * GetValueMulti(.5f) * .1f);
		}
	}
	public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment) {
		if (!UniversalSystem.CanAccessContent(UniversalSystem.BOSSRUSH_MODE)) {
			return;
		}
		if (npc.boss && npc.type != NPCID.WallofFlesh && npc.type != NPCID.WallofFleshEye) {
			if (!NPC_SpecialException) {
				float adjustment = 1;
				if (Main.expertMode)
					adjustment = 1.5f;
				else if (Main.masterMode)
					adjustment = 2;

				npc.lifeMax = (int)(BossHP / adjustment * GetValueMulti());
				npc.life = npc.lifeMax;
				npc.damage = (int)(BossDMG / adjustment * GetValueMulti());
				npc.defense = (int)(BossDef / adjustment * GetValueMulti(.5f));
			}
		}
		else {
			float adjustment = 1;
			if (Main.expertMode)
				adjustment = 1.5f;
			else if (Main.masterMode)
				adjustment = 2;
			npc.lifeMax += (int)(npc.lifeMax / adjustment * GetValueMulti() * .1f);
			npc.life = npc.lifeMax;
			npc.damage += (int)(npc.damage / adjustment * GetValueMulti() * .1f);
			npc.defense += (int)(npc.defense / adjustment * GetValueMulti(.5f) * .1f);
		}
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
		if (EliteBoss) {
			extraMultiply += 2;
		}
		int counter = ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Count;
		return (1 + counter * .5f + extraMultiply) * scale;
	}
	public override void ResetEffects(NPC npc) {
		StatDefense = new();
		if (IsAGhostEnemy) {
			npc.dontTakeDamage = true;
		}
		if (!npc.boss) {
			EliteBoss = false;
		}
		if (--DRTimer <= 0) {
			DRFromFatalAttack = false;
		}
		else {
			DRFromFatalAttack = true;
		}
		Endurance = 0;
	}
	public override void ModifyTypeName(NPC npc, ref string typeName) {
		if (EliteBoss) {
			typeName = "Elite " + typeName;
		}
	}
	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		LeadingConditionRule rule = new(new DenyYouFromLoot());
		foreach (var item in npcLoot.Get()) {
			item.OnSuccess(rule);
		}
	}
	public override Color? GetAlpha(NPC npc, Color drawColor) {
		if (npc.HasBuff<Urine_Debuff>()) {
			drawColor.R = 255;
			drawColor.G = 255;
			drawColor.B = 90;
			return drawColor;
		}
		return base.GetAlpha(npc, drawColor);
	}
	public override bool PreAI(NPC npc) {
		if (VelocityMultiplier != 0) {
			npc.velocity /= VelocityMultiplier + static_velocityMultiplier - 1;
		}
		else {
			npc.velocity /= .001f;
		}
		return base.PreAI(npc);
	}
	public override void PostAI(NPC npc) {
		if (VelocityMultiplier != 0) {
			npc.velocity *= VelocityMultiplier + static_velocityMultiplier - 1;
		}
		else {
			npc.velocity *= .001f;
		}
		VelocityMultiplier = 1;
		if (HeatRay_HitCount > 0) {
			HeatRay_Decay = BossRushUtils.CountDown(HeatRay_Decay);
			if (HeatRay_Decay <= 0) {
				HeatRay_HitCount--;
			}
		}
		if (BelongToWho >= 0 && BelongToWho < Main.maxNPCs) {
			var parent = Main.npc[BelongToWho];
			if (parent != null) {
				if (!parent.active || parent.life <= 0) {
					npc.StrikeInstantKill();
				}
			}
			else {
				BelongToWho = -1;
			}
		}
		if (++PositiveLifeRegenCount >= 60) {
			PositiveLifeRegenCount = 0;
			npc.life = Math.Clamp(npc.life + PositiveLifeRegen, 0, npc.lifeMax);
		}
	}
	public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers) {
		if (npc.HasBuff<NPC_Weakness>()) {
			modifiers.SourceDamage -= .5f;
		}
		if (npc.boss) {
			if (EliteBoss) {
				modifiers.FinalDamage.Flat += (int)(target.statLifeMax2 * .15f);
			}
		}
	}
	public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
		if (npc.boss) {
			if (Main.rand.NextBool(20) || EliteBoss && Main.rand.NextBool(10)) {
				modifiers.SetMaxDamage(1);
			}
			if (DRFromFatalAttack) {
				modifiers.FinalDamage *= .35f;
			}
		}
		modifiers.Defense = modifiers.Defense.CombineWith(StatDefense);
		modifiers.FinalDamage *= 1 - Endurance;
	}
	public int CursedSkullStatus = 0;
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		if (!projectile.npcProj && !projectile.trap && projectile.IsMinionOrSentryRelated) {
			var projTagMultiplier = ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type];
			if (npc.HasBuff<StarRay>()) {
				// Apply a flat bonus to every hit
				modifiers.FlatBonusDamage += StarRay.TagDamage * projTagMultiplier;
			}
		}
		if (projectile.Check_ItemTypeSource(ModContent.ItemType<SkullRevolver>())) {
			if (npc.HasBuff<CursedStatus>()) {
				if (++CursedSkullStatus >= 3) {
					CursedSkullStatus = 3;
				}
				if (projectile.type == ProjectileID.BookOfSkullsSkull) {
					modifiers.SourceDamage += 1;
				}
			}
		}


		if (npc.boss) {
			if (Main.rand.NextBool(20) || EliteBoss && Main.rand.NextBool(10)) {
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
		modifiers.FinalDamage *= 1 - Endurance;
		if (projectile.type == ProjectileID.GolemFist) {
			if (++GolemFist_HitCount % 3 == 0) {
				modifiers.SourceDamage += 1.5f;
			}
		}
	}
	public int HitCount = 0;
	public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
		HitCount++;
		if (!npc.boss) {
			return;
		}
		if (hit.Damage >= npc.lifeMax * .1f && !OneTimeDR) {
			if (EliteBoss) {
				npc.Heal(npc.lifeMax);
			}
			else {
				npc.Heal(npc.lifeMax / 10);
			}
			OneTimeDR = true;
			DRTimer = BossRushUtils.ToMinute(1);
			DRFromFatalAttack = true;
		}
		if (hit.Crit || EliteBoss) {
			if (Main.rand.NextBool(10)) {
				npc.Heal(Main.rand.Next(hit.Damage));
			}
		}
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		HitCount++;
		if (projectile.type == ProjectileID.HeatRay) {
			HeatRay_HitCount = Math.Clamp(HeatRay_HitCount + 1, 0, 100);
			HeatRay_Decay = 30;
		}
		else if (projectile.type == ProjectileID.GolemFist) {
			if (GolemFist_HitCount % 3 == 0) {
				for (int i = 0; i < 100; i++) {
					Dust dust = Dust.NewDustDirect(npc.Center, 0, 0, DustID.HeatRay);
					dust.noGravity = true;
					dust.velocity = Main.rand.NextVector2Circular(20, 20);
					dust.scale += Main.rand.NextFloat();
				}
				for (int i = 0; i < 100; i++) {
					Dust dust = Dust.NewDustDirect(npc.Center, 0, 0, DustID.HeatRay);
					dust.noGravity = true;
					dust.velocity = Main.rand.NextVector2CircularEdge(25, 25);
					dust.scale += Main.rand.NextFloat();
				}
				SoundEngine.PlaySound(SoundID.Item14, npc.Center);
				npc.Center.LookForHostileNPC(out List<NPC> npclist, 150);
				npc.TargetClosest();
				Player player = Main.player[npc.target];
				foreach (var target in npclist) {
					if (target.whoAmI != npc.whoAmI) {
						player.StrikeNPCDirect(target, target.CalculateHitInfo(hit.Damage, -1));
					}
				}
			}
		}
		if (!npc.boss) {
			return;
		}
		if (hit.Damage >= npc.lifeMax * .1f && !OneTimeDR) {
			if (EliteBoss) {
				npc.Heal(npc.lifeMax);
			}
			else {
				npc.Heal(npc.lifeMax / 10);
			}
			OneTimeDR = true;
			DRTimer = BossRushUtils.ToMinute(1);
			DRFromFatalAttack = true;
		}
		if (hit.Crit || EliteBoss) {
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
		var player = Main.player[playerIndex];
		player.GetModPlayer<PlayerStatsHandle>().successfullyKillNPCcount++;
		player.GetModPlayer<PlayerStatsHandle>().NPC_HitCount = HitCount;
		if (EliteBoss) {
			player.GetModPlayer<PlayerStatsHandle>().EliteKillCount++;
			RoguelikeData.EliteKill++;
		}
		if (npc.boss && player.GetModPlayer<GamblePlayer>().GodDice) {
			player.GetModPlayer<GamblePlayer>().Roll++;
		}
		if (player.GetModPlayer<KillingThrillPlayer>().KillingThrill) {
			player.GetModPlayer<KillingThrillPlayer>().KillCount_Decay++;
		}
	}
	public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
		//TODO : this is very broken, I couldn't get the outline to work so I gave up
		//if (npc.boss) {
		//	Main.instance.LoadNPC(npc.type);
		//	Texture2D texture = TextureAssets.Npc[npc.type].Value;
		//	SpriteEffects effect = SpriteEffects.None;
		//	Vector2 origin = npc.frame.Size() * .5f;
		//	Vector2 drawpos = npc.position - Main.screenPosition;
		//	spriteBatch.Draw(texture, drawpos + Vector2.One * 3, npc.frame, Color.Red * .25f, npc.rotation, origin, npc.scale, effect, 0);
		//	spriteBatch.Draw(texture, drawpos - Vector2.One * 3, npc.frame, Color.Red * .25f, npc.rotation, origin, npc.scale, effect, 0);
		//	spriteBatch.Draw(texture, drawpos + Vector2.One.Add(-2, 0) * 3, npc.frame, Color.Red * .25f, npc.rotation, origin, npc.scale, effect, 0);
		//	spriteBatch.Draw(texture, drawpos + Vector2.One.Add(0, -2) * 3, npc.frame, Color.Red * .25f, npc.rotation, origin, npc.scale, effect, 0);
		//}
		return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
	}
}
