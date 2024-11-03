using BossRush.Common.General;
using BossRush.Contents.Items.Weapon;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace BossRush.Common.Systems.ArgumentsSystem;

public class FireI : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Red;
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.OnFire, BossRushUtils.ToSecond(Main.rand.Next(7, 10)));
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type)
			npc.AddBuff(BuffID.OnFire, BossRushUtils.ToSecond(Main.rand.Next(7, 10)));
	}
}
public class FireII : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Red;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
			modifiers.SourceDamage += .2f;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type)
			if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
				modifiers.SourceDamage += .2f;
			}
	}
}
public class FrostBurnI : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Cyan;
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.Frostburn, BossRushUtils.ToSecond(Main.rand.Next(7, 10)));
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type)
			npc.AddBuff(BuffID.Frostburn, BossRushUtils.ToSecond(Main.rand.Next(7, 10)));
	}
}
public class FrostBurnII : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Cyan;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.Frostburn) || target.HasBuff(BuffID.Frostburn2)) {
			modifiers.SourceDamage += .2f;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type)
			if (target.HasBuff(BuffID.Frostburn) || target.HasBuff(BuffID.Frostburn2)) {
				modifiers.SourceDamage += .2f;
			}
	}
}
public class BerserkI : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.OrangeRed;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		float percentage = player.statLife / (float)player.statLifeMax2;
		modifiers.SourceDamage += .5f * percentage;
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			float percentage = player.statLife / (float)player.statLifeMax2;
			modifiers.SourceDamage += .5f * percentage;
		}
	}
}

public class True : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Yellow;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int damage = (int)(player.GetWeaponDamage(item) * .1f);
		modifiers.FinalDamage.Flat += damage;
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			int damage = (int)(player.GetWeaponDamage(player.HeldItem) * .1f);
			modifiers.FinalDamage.Flat += damage;
		}
	}
}

public class Terra : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Green;
	}
	public override void OnHitNPC(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		NPC.HitModifiers modifier = new NPC.HitModifiers();
		modifier.FinalDamage.Flat = player.GetWeaponDamage(item) * (hitInfo.Crit ? player.GetModPlayer<PlayerStatsHandle>().UpdateCritDamage.ApplyTo(2) : 1);
		modifier.FinalDamage *= 0;
		player.StrikeNPCDirect(npc, modifier.ToHitInfo(1, hitInfo.Crit, hitInfo.Knockback, true));
	}
}

public class TitanI : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Blue;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int damage = (int)player.GetWeaponKnockback(item);
		modifiers.SourceDamage.Base += damage;
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			int damage = (int)player.GetWeaponKnockback(player.HeldItem);
			modifiers.SourceDamage.Base += damage;
		}
	}
}

public class TitanII : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Blue;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int knockbackStrength = (int)(player.GetWeaponDamage(item) * .05f);
		modifiers.Knockback += knockbackStrength;
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			int knockbackStrength = (int)(player.GetWeaponDamage(player.HeldItem) * .05f);
			modifiers.Knockback += knockbackStrength;
		}
	}
}

public class CriticalI : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Orange;
	}
	public override void OnHitNPC(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		if (hitInfo.Crit) {
			player.Heal(Math.Clamp((int)Math.Ceiling(player.statLifeMax2 * 0.01f), 1, player.statLifeMax2));
		}
	}
}
public class CriticalII : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Orange;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (player.GetModPlayer<PlayerStatsHandle>().ModifyHit_Before_Crit) {
			modifiers.ScalingArmorPenetration += .5f;
		}
	}

	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (player.GetModPlayer<PlayerStatsHandle>().ModifyHit_Before_Crit) {
			modifiers.ScalingArmorPenetration += .5f;
		}
	}
}

public class CriticalIII : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Orange;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int critchanceReroll = player.GetWeaponCrit(item);
		if (Main.rand.Next(1, 101) < critchanceReroll) {
			modifiers.CritDamage += 1;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		int critchanceReroll = proj.CritChance;
		if (Main.rand.Next(1, 101) < critchanceReroll) {
			modifiers.CritDamage += 1;
		}
	}
}

public class VampireI : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkRed;
	}
	public override void UpdateHeld(Player player, Item item) {
		player.GetModPlayer<PlayerStatsHandle>().LifeSteal += 0.01f;
	}
}

public class VampireII : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkRed;
	}
	public override void UpdateHeld(Player player, Item item) {
		if (!Main.IsItDay())
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritDamage, Multiplicative: 1.5f);
	}
}

public class AlchemistI : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.BlueViolet;
	}
	public override void UpdateHeld(Player player, Item item) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.DebuffDamage, 1.06f);
	}
}

public class AlchemistII : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.BlueViolet;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int buffamount = target.buffType.Where(b => b != 0 && b != -1).Count();
		modifiers.FinalDamage += 0.06f * buffamount;
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			int buffamount = target.buffType.Where(b => b != 0 && b != -1).Count();
			modifiers.FinalDamage += 0.06f * buffamount;
		}
	}
}

public class Light : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Pink;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.GetLifePercent() > .8f)
			modifiers.SourceDamage += 1.5f;
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.GetLifePercent() > .8f)
			modifiers.SourceDamage += 1.5f;
	}
}

public class Dark : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Purple;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.GetLifePercent() < .4f)
			modifiers.SourceDamage += 1.5f;
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.GetLifePercent() < .4f)
			modifiers.SourceDamage += 1.5f;
	}
}

public class Union : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Bisque;
	}
	public override void UpdateHeld(Player player, Item item) {
		float damageIncreasement = 0;
		for (int i = 0; player.inventory.Length > 0; i++) {
			if (i > 50) {
				break;
			}
			Item invitem = player.inventory[i];
			if (!item.IsAWeapon() || invitem == item || item.ModItem is SynergyModItem) {
				continue;
			}
			damageIncreasement += .5f;
		}
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, Flat: damageIncreasement);
	}
}

public class ShadowFlameI : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.MediumPurple;
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.ShadowFlame, BossRushUtils.ToSecond(Main.rand.Next(7, 10)));
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type)
			npc.AddBuff(BuffID.ShadowFlame, BossRushUtils.ToSecond(Main.rand.Next(7, 10)));
	}
}
public class ShadowFlameII : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.MediumPurple;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.ShadowFlame)) {
			modifiers.SourceDamage += .2f;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type)
			if (target.HasBuff(BuffID.Frostburn)) {
				modifiers.SourceDamage += .2f;
			}
	}
}

public class CursedFlameI : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.ForestGreen;
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.CursedInferno, BossRushUtils.ToSecond(Main.rand.Next(7, 10)));
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type)
			npc.AddBuff(BuffID.CursedInferno, BossRushUtils.ToSecond(Main.rand.Next(7, 10)));
	}
}
public class CursedFlameII : ModArgument {
	public override void SetStaticDefaults() {
		tooltipColor = Color.ForestGreen;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.CursedInferno)) {
			modifiers.SourceDamage += .2f;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type)
			if (target.HasBuff(BuffID.CursedInferno)) {
				modifiers.SourceDamage += .2f;
			}
	}
}
