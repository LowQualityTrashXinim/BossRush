﻿using BossRush.Common.Global;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.Systems.Mutation;
public class Vampirism : ModMutation {
	public override void SetStaticDefaults() {
		NewGamePlus = true;
	}
	public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo) {
		npc.life += (int)Math.Round(hurtInfo.Damage * .25f);
	}
}
public class ChaoticTransportation : ModMutation {
	public override void SetStaticDefaults() {
		NewGamePlus = true;
	}
	public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo) {
		if (Main.rand.NextBool(10)) {
			target.TeleportationPotion();
			npc.position = target.position;
		}
	}
}
public class TouchOfGrim : ModMutation {
	public override void SetStaticDefaults() {
		NewGamePlus = true;
	}
	public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo) {
		if (target.HasBuff<GrimTouch>()) {
			return;
		}
		target.statLife = 1;
		target.AddBuff(ModContent.BuffType<GrimTouch>(), BossRushUtils.ToSecond(Main.rand.Next(3, 9)));
	}
}
public class GrimTouch : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
}
public class ProjectileResistance : ModMutation {
	public override void SetStaticDefaults() {
		NewGamePlus = true;
	}
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage -= .35f;
	}
}
public class ItemResistance : ModMutation {
	public override void SetStaticDefaults() {
		NewGamePlus = true;
	}
	public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage -= .35f;
	}
}
public class BreakItem : ModMutation {
	public override void SetStaticDefaults() {
		NewGamePlus = true;
	}
	public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo) {
		target.HeldItem.TurnToAir();
	}
}
public class LifeStruck : ModMutation {
	public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo) {
		target.AddBuff(ModContent.BuffType<LifeStruckDebuff>(), BossRushUtils.ToSecond(5));
	}
}
public class LifeStruckDebuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff(true);
	}
	public override bool ReApply(Player player, int time, int buffIndex) {
		player.buffTime[buffIndex] += time;
		return true;
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MaxHP, -.05f);
	}
}
public class Tanky : ModMutation {
	public override void SetStaticDefaults() {
		NewGamePlus = true;
	}
	public override void OnSpawn(NPC npc, IEntitySource source) {
		npc.lifeMax *= 3;
		npc.life = npc.lifeMax;
		npc.defense *= 3;
		base.OnSpawn(npc, source);
	}
}
public class Masochsit : ModMutation {
	public override void SetStaticDefaults() {
		NewGamePlus = true;
	}
	public override void OnSpawn(NPC npc, IEntitySource source) {
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().Endurance += .5f;
		npc.defense *= 2;
	}
	public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
		npc.Heal(hit.Damage * 2);
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		npc.Heal(hit.Damage * 2);
	}
}
public class Slimy : ModMutation {
	public override bool MutationCondition(NPC npc, Player player) {
		return false;
	}
	public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo) {
		if (Main.rand.NextBool(10)) {
			target.AddBuff<Slimy_Debuff>(BossRushUtils.ToSecond(Main.rand.Next(4, 7)));
		}
	}
	public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
		if (modifiers.DamageType == DamageClass.Melee && modifiers.DamageType == DamageClass.Ranged) {
			modifiers.SourceDamage -= .2f;
		}
	}
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		if (modifiers.DamageType == DamageClass.Melee && modifiers.DamageType == DamageClass.Ranged) {
			modifiers.SourceDamage -= .2f;
		}
	}
}
public class Slimy_Debuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle stathandle = player.GetModPlayer<PlayerStatsHandle>();
		stathandle.AddStatsToPlayer(PlayerStats.AttackSpeed, .75f);
		stathandle.AddStatsToPlayer(PlayerStats.CritDamage, .55f);
		stathandle.AddStatsToPlayer(PlayerStats.RegenHP, .55f);
		stathandle.AddStatsToPlayer(PlayerStats.Defense, Base: 2);
	}
}
public class FleshAutomaton : ModMutation {
	public override bool MutationCondition(NPC npc, Player player) {
		return false;
	}
	public override void OnSpawn(NPC npc, IEntitySource source) {
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().PositiveLifeRegen += 10;
		npc.defense += 10;
	}
	public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().PositiveLifeRegen += 1;
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().PositiveLifeRegen += 1;
	}
}
public class Elite : ModMutation {
	public override void OnSpawn(NPC npc, IEntitySource source) {
		npc.lifeMax = npc.lifeMax + (int)(npc.lifeMax * .5f);
		npc.life = npc.lifeMax;
		npc.damage = npc.damage + (int)(npc.damage * .2f);
		npc.defense += 10;
	}
	public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextFloat() <= .1f) {
			modifiers.FinalDamage *= 0;
		}
	}
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		if (Main.rand.NextFloat() <= .1f) {
			modifiers.FinalDamage *= 0;
		}
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextFloat() <= .01f) {
			npc.Heal((int)(npc.lifeMax * .2f));
		}
	}
	public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextFloat() <= .01f) {
			npc.Heal((int)(npc.lifeMax * .2f));
		}
	}
}
public class DarkTentacle : ModMutation {
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		for (int i = 0; i < 5; i++) {
			Vector2 vec2 = Vector2.One.Vector2DistributeEvenly(5, 360, i) * 10;
			Projectile proj = Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, vec2, ProjectileID.ShadowFlame, npc.damage * 2, 3f);
			proj.ai[0] = Main.rand.NextFloat(-1,1);
			proj.ai[1] = Main.rand.NextFloat(-1, 1);
			proj.ai[2] = Main.rand.NextFloat(-1, 1);
		}
	}
}
