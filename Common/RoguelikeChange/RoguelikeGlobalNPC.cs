using Terraria;
using Terraria.ModLoader;
using BossRush.Contents.Items.Accessories.LostAccessories;
using BossRush.Common.Systems;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent.ItemDropRules;
using BossRush.Common.General;

namespace BossRush.Common.RoguelikeChange;
internal class RoguelikeGlobalNPC : GlobalNPC {
	public override bool InstancePerEntity => true;
	public int HeatRay_Decay = 0;
	public int HeatRay_HitCount = 0;
	public StatModifier StatDefense = new StatModifier();
	public float Endurance = 0;
	public bool DRFromFatalAttack = false;
	public bool OneTimeDR = false;
	public int DRTimer = 0;
	public const int BossHP = 4000;
	public const int BossDMG = 40;
	public const int BossDef = 5;
	public bool EliteBoss = false;
	public float VelocityMultiplier = 1;
	public bool IsAGhostEnemy = false;
	public int BelongToWho = -1;
	public bool CanDenyYouFromLoot = false;
	public float static_velocityMultiplier = 1;
	/// <summary>
	/// Set this to true if you don't want the mod to apply boss NPC fixed boss's stats
	/// </summary>
	public bool NPC_SpecialException = false;
	public override void SetDefaults(NPC entity) {
		StatDefense = new();
		if (!UniversalSystem.Check_RLOH()) {
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
		if (!UniversalSystem.Check_RLOH()) {
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
		return (1 + ModContent.GetInstance<UniversalSystem>().ListOfBossKilled.Count * .5f + extraMultiply) * scale;
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
			NPC parent = Main.npc[BelongToWho];
			if (parent != null) {
				if (!parent.active || parent.life <= 0) {
					npc.StrikeInstantKill();
				}
			}
			else {
				BelongToWho = -1;
			}
		}
	}
	public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers) {
		if (npc.boss) {
			modifiers.FinalDamage.Flat += (int)(target.statLifeMax2 * .1f);
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
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
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
	}
	public int HitCount = 0;
	public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
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
		HitCount++;
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		if (projectile.type == ProjectileID.HeatRay) {
			HeatRay_HitCount = Math.Clamp(HeatRay_HitCount + 1, 0, 100);
			HeatRay_Decay = 30;
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
		HitCount++;
	}
	public override void OnKill(NPC npc) {
		int playerIndex = npc.lastInteraction;
		if (!Main.player[playerIndex].active || Main.player[playerIndex].dead) {
			playerIndex = npc.FindClosestPlayer();
		}
		Player player = Main.player[playerIndex];
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
