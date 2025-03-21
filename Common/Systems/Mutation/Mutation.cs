using BossRush.Common.Global;
using BossRush.Texture;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BossRush.Common.Systems.Mutation;
public class Vampirism : ModMutation {
	public override void SetDefaults(NPC npc) {
		NewGamePlus = true;
	}
	public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo) {
		npc.life += (int)Math.Round(hurtInfo.Damage * .25f);
	}
}
public class ChaoticTransportation : ModMutation {
	public override void SetDefaults(NPC npc) {
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
	public override void SetDefaults(NPC npc) {
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
	public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams) {
		return false;
	}
}
public class ProjectileResistance : ModMutation {
	public override void SetDefaults(NPC npc) {
		NewGamePlus = true;
	}
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage -= .35f;
	}
}
public class ItemResistance : ModMutation {
	public override void SetDefaults(NPC npc) {
		NewGamePlus = true;
	}
	public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
		modifiers.SourceDamage -= .35f;
	}
}
public class BreakItem : ModMutation {
	public override void SetDefaults(NPC npc) {
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
	public override void SetDefaults(NPC npc) {
		NewGamePlus = true;
		npc.lifeMax *= 3;
		npc.life = npc.lifeMax;
		npc.defense *= 3;
	}
}
