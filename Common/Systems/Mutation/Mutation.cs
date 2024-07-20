using System;
using Terraria;

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
		if(Main.rand.NextBool(10)) {
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
		target.statLife = 1;
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

