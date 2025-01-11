using System;
using Terraria;
using System.Linq;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.General;
using BossRush.Contents.Artifacts;
using BossRush.Contents.Items.Weapon;
using BossRush.Common.Systems.ArtifactSystem;

namespace BossRush.Common.Systems.ArgumentsSystem;

public class FireI : ModAugments {
	public override void SetStaticDefaults() {
		Chance = .15f;
		tooltipColor = Color.Red;
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.OnFire, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type)
			npc.AddBuff(BuffID.OnFire, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
}
public class FireII : ModAugments {
	public override void SetStaticDefaults() {
		Chance = .05f;
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
public class FrostBurnI : ModAugments {
	public override void SetStaticDefaults() {
		Chance = .15f;
		tooltipColor = Color.Cyan;
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.Frostburn, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type)
			npc.AddBuff(BuffID.Frostburn, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
}
public class FrostBurnII : ModAugments {
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
public class BerserkI : ModAugments {
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

public class True : ModAugments {
	public override bool ConditionToBeApplied(Player player, Item item, out float Chance) {
		Chance = 0;
		if (player.HeldItem.type == ItemID.TrueExcalibur || player.HeldItem.type == ItemID.TrueNightsEdge) {
			Chance = .2f;
		}
		return true;
	}
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

public class Terra : ModAugments {
	public override bool ConditionToBeApplied(Player player, Item item, out float Chance) {
		Chance = 0;
		if (player.HeldItem.type == ItemID.TerraBlade) {
			Chance = .2f;
		}
		return true;
	}
	public override void SetStaticDefaults() {
		tooltipColor = Color.Green;
	}
	public override void OnHitNPC(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		if (Main.rand.NextFloat() > .05f) {
			return;
		}
		NPC.HitModifiers modifier = new NPC.HitModifiers();
		modifier.FinalDamage.Flat = player.GetWeaponDamage(item) * (hitInfo.Crit ? player.GetModPlayer<PlayerStatsHandle>().UpdateCritDamage.ApplyTo(2) : 1);
		modifier.FinalDamage *= 0;
		player.StrikeNPCDirect(npc, modifier.ToHitInfo(1, hitInfo.Crit, hitInfo.Knockback, true));
	}
}

public class TitanI : ModAugments {
	public override bool ConditionToBeApplied(Player player, Item item, out float Chance) {
		Chance = 0;
		if (player.HeldItem.knockBack > 10) {
			Chance = .2f;
		}
		return true;
	}
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

public class TitanII : ModAugments {
	public override bool ConditionToBeApplied(Player player, Item item, out float Chance) {
		Chance = 0;
		if (player.HeldItem.knockBack > 10) {
			Chance = .2f;
		}
		return true;
	}
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

public class CriticalI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Orange;
	}
	public override void OnHitNPC(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		if (hitInfo.Crit) {
			player.Heal(Math.Clamp((int)Math.Ceiling(player.statLifeMax2 * 0.01f), 1, player.statLifeMax2));
		}
	}
}
public class CriticalII : ModAugments {
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

public class CriticalIII : ModAugments {
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

public class CriticalIV : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Orange;
	}
	public override void UpdateAccessory(Player player, Item item) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 25);
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, .5f);
	}
}


public class VampireI : ModAugments {
	public override bool ConditionToBeApplied(Player player, Item item, out float Chance) {
		Chance = 0;
		if (!Main.IsItDay()) {
			Chance += .1f;
		}
		return true;
	}
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkRed;
	}
	public override void UpdateAccessory(Player player, Item item) {
		player.GetModPlayer<PlayerStatsHandle>().LifeSteal += 0.01f;
	}
}

public class VampireII : ModAugments {
	public override bool ConditionToBeApplied(Player player, Item item, out float Chance) {
		Chance = 0;
		if (!Main.IsItDay()) {
			Chance += .1f;
		}
		return true;
	}
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkRed;
	}
	public override void UpdateAccessory(Player player, Item item) {
		if (!Main.IsItDay())
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritDamage, Multiplicative: 1.5f);
	}
}

public class VampireIII : ModAugments {
	public override bool ConditionToBeApplied(Player player, Item item, out float Chance) {
		Chance = 0;
		if (!Main.IsItDay()) {
			Chance += .1f;
		}
		return true;
	}
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkRed;
	}
	public override void UpdateAccessory(Player player, Item item) {
		if (!Main.IsItDay()) {
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, 1.25f, Base: 1);
		}
	}
}


public class AlchemistI : ModAugments {
	public override bool ConditionToBeApplied(Player player, Item item, out float Chance) {
		Chance = 0;
		if (Artifact.PlayerCurrentArtifact<AlchemistKnowledgeArtifact>()) {
			Chance += .1f;
		}
		return true;
	}
	public override void SetStaticDefaults() {
		tooltipColor = Color.BlueViolet;
	}
	public override void UpdateAccessory(Player player, Item item) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.DebuffDamage, 1.06f);
	}
}

public class AlchemistII : ModAugments {
	public override bool ConditionToBeApplied(Player player, Item item, out float Chance) {
		Chance = 0;
		if (Artifact.PlayerCurrentArtifact<AlchemistKnowledgeArtifact>()) {
			Chance += .1f;
		}
		return true;
	}
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

public class Light : ModAugments {
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

public class Dark : ModAugments {
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

public class Union : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Bisque;
	}
	public override void UpdateAccessory(Player player, Item item) {
		float damageIncreasement = 0;
		for (int i = 0; player.inventory.Length > 0; i++) {
			if (i > 50) {
				break;
			}
			Item invitem = player.inventory[i];
			if (!invitem.IsAWeapon() || invitem == item || item.ModItem is SynergyModItem) {
				continue;
			}
			damageIncreasement += .5f;
		}
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, Flat: damageIncreasement);
	}
}

public class ShadowFlameI : ModAugments {
	public override void SetStaticDefaults() {
		Chance = .07f;
		tooltipColor = Color.MediumPurple;
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.ShadowFlame, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type)
			npc.AddBuff(BuffID.ShadowFlame, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
}
public class ShadowFlameII : ModAugments {
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

public class CursedFlameI : ModAugments {
	public override void SetStaticDefaults() {
		Chance = .07f;
		tooltipColor = Color.ForestGreen;
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.CursedInferno, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type)
			npc.AddBuff(BuffID.CursedInferno, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
}
public class CursedFlameII : ModAugments {
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
public class PoisonI : ModAugments {
	public override void SetStaticDefaults() {
		Chance = .15f;
		tooltipColor = Color.PaleGreen;
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.Poisoned, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type)
			npc.AddBuff(BuffID.Poisoned, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
}
public class PoisonII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.PaleGreen;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.Poisoned)) {
			modifiers.SourceDamage += .2f;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type)
			if (target.HasBuff(BuffID.Poisoned)) {
				modifiers.SourceDamage += .2f;
			}
	}
}

public class VenomI : ModAugments {
	public override void SetStaticDefaults() {
		Chance = .07f;
		tooltipColor = Color.Purple;
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.Venom, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type)
			npc.AddBuff(BuffID.Venom, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
}
public class VenomII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Purple;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.Venom)) {
			modifiers.SourceDamage += .2f;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type)
			if (target.HasBuff(BuffID.Venom)) {
				modifiers.SourceDamage += .2f;
			}
	}
}
public class StrengthenI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.IndianRed;
	}
	public override void UpdateAccessory(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, 1.25f);
	}
}
public class StrengthenII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.IndianRed;
	}
	public override void UpdateAccessory(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.CritDamage, 1.25f);
	}
}
public class StrengthenIII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.IndianRed;
	}
	public override void UpdateAccessory(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.Defense, Base: 10);
	}
}
public class StrengthenIV : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.IndianRed;
	}
	public override void UpdateAccessory(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.MaxHP, Base: 50);
	}
}
public class StrengthenV : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.IndianRed;
	}
	public override void UpdateAccessory(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.CritChance, Base: 10);
	}
}

public class DarkSoul : ModAugments {
	public override bool ConditionToBeApplied(Player player, Item item, out float Chance) {
		Chance = 0;
		return !player.HeldItem.noMelee && !player.HeldItem.noUseGraphic;
	}
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkBlue;
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) {
		player.AddImmuneTime(-1, 12);
	}
}

public class ExtraLife : ModAugments {
	public override bool ConditionToBeApplied(Player player, Item item, out float Chance) {
		Chance = -.1f;
		return true;
	}
	public override void SetStaticDefaults() {
		tooltipColor = Color.White;
	}
	public override void UpdateAccessory(Player player, Item item) {
		player.GetModPlayer<PlayerStatsHandle>().Add_ExtraLifeWeapon(item);
	}
}

public class IntoxicateI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.GreenYellow;
	}
	public override void UpdateAccessory(Player player, Item item) {
		for (int i = 0; i < player.buffType.Length; i++) {
			if (player.buffType[i] == 0) continue;
			if (Main.debuff[player.buffType[i]]) {
				player.endurance += .1f;
			}
		}
	}
}

public class IntoxicateII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.GreenYellow;
	}
	public override void UpdateAccessory(Player player, Item item) {
		for (int i = 0; i < player.buffType.Length; i++) {
			if (player.buffType[i] == 0) continue;
			if (Main.debuff[player.buffType[i]]) {
				PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, Additive: 1.15f, Flat: 5);
			}
		}
	}
}
public class ReactiveHealingI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.ForestGreen;
	}
	public override void OnHitByNPC(Player player, NPC npc, Player.HurtInfo info) {
		if (Main.rand.NextBool(3)) {
			player.Heal((int)Math.Ceiling(player.statLifeMax2 * .05f));
		}
	}
	public override void OnHitByProj(Player player, Projectile projectile, Player.HurtInfo info) {
		if (Main.rand.NextBool(3)) {
			player.Heal((int)Math.Ceiling(player.statLifeMax2 * .05f));
		}
	}
}
public class ReactiveHealingII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.ForestGreen;
	}
	public override void OnHitByNPC(Player player, NPC npc, Player.HurtInfo info) {
		if (Main.rand.NextBool(3)) {
			player.AddBuff(ModContent.BuffType<ReactiveHealingBuff>(), BossRushUtils.ToSecond(Main.rand.Next(4, 11)));
		}
	}
	public override void OnHitByProj(Player player, Projectile projectile, Player.HurtInfo info) {
		if (Main.rand.NextBool(3)) {
			player.AddBuff(ModContent.BuffType<ReactiveHealingBuff>(), BossRushUtils.ToSecond(Main.rand.Next(4, 11)));
		}
	}
}
public class ReactiveHealingBuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, Base: 10);
	}
}

public class ReactiveDefenseI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.MediumPurple;
	}
	public override void OnHitByNPC(Player player, NPC npc, Player.HurtInfo info) {
		if (Main.rand.NextBool(4)) {
			player.AddBuff(ModContent.BuffType<ReactiveDefenseBuff>(), BossRushUtils.ToSecond(Main.rand.Next(4, 11)));
		}
	}
	public override void OnHitByProj(Player player, Projectile projectile, Player.HurtInfo info) {
		if (Main.rand.NextBool(4)) {
			player.AddBuff(ModContent.BuffType<ReactiveDefenseBuff>(), BossRushUtils.ToSecond(Main.rand.Next(4, 11)));
		}
	}
}
public class ReactiveDefenseBuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.endurance += .1f;
	}
}
public class ReactiveDefenseII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.MediumPurple;
	}
	public override void OnHitByNPC(Player player, NPC npc, Player.HurtInfo info) {
		if (Main.rand.NextBool(4)) {
			player.AddBuff(ModContent.BuffType<ReactiveDefenseIIBuff>(), BossRushUtils.ToSecond(Main.rand.Next(4, 11)));
		}
	}
	public override void OnHitByProj(Player player, Projectile projectile, Player.HurtInfo info) {
		if (Main.rand.NextBool(4)) {
			player.AddBuff(ModContent.BuffType<ReactiveDefenseIIBuff>(), BossRushUtils.ToSecond(Main.rand.Next(4, 11)));
		}
	}
}
public class ReactiveDefenseIIBuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.Defense, 1.1f, Flat: 6);
	}
}

public class VitalityStrikeI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.PaleVioletRed;
	}
	public override void UpdateAccessory(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, 1 + player.statLifeMax2 * .0005f);
	}
}
public class VitalityStrikeII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.PaleVioletRed;
	}
	public override void UpdateAccessory(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.CritChance, Base: player.statLifeMax2 * .01f);
	}
}

public class ArcaneStrikeI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkBlue;
	}
	public override void UpdateAccessory(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, 1 + player.statManaMax2 * .0005f);
	}
}
public class ArcaneStrikeII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkBlue;
	}
	public override void UpdateAccessory(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.CritChance, Base: player.statManaMax2 * .01f);
	}
}

public class StealthStrikeI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkGray;
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (player.invis)
			modifiers.SourceDamage += .25f;
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.Check_ItemTypeSource(player.HeldItem.type)) {
			if (player.invis)
				modifiers.SourceDamage += .25f;
		}
	}
}
public class StealthStrikeII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkGray;
	}
	public override void UpdateAccessory(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.FullHPDamage, 2f);
	}
}
public class DryadBlessing : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.LimeGreen;
	}
	public override void UpdateAccessory(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, Base: 3);
	}
	public override void OnHitByProj(Player player, Projectile projectile, Player.HurtInfo info) {
		if (Main.rand.NextFloat() <= Main.rand.NextFloat(.1f, .4f) && !player.HasBuff<DryadBlessing_Buff>()) {
			player.AddBuff(ModContent.BuffType<DryadBlessing_Buff>(), BossRushUtils.ToSecond(Main.rand.Next(3, 8)));
		}
	}
	public override void OnHitByNPC(Player player, NPC npc, Player.HurtInfo info) {
		if (Main.rand.NextFloat() <= Main.rand.NextFloat(.1f,.4f) && !player.HasBuff<DryadBlessing_Buff>()) {
			player.AddBuff(ModContent.BuffType<DryadBlessing_Buff>(), BossRushUtils.ToSecond(Main.rand.Next(3, 8)));
		}
	}
}
public class DryadBlessing_Buff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle statplayer = player.GetModPlayer<PlayerStatsHandle>();
		statplayer.AddStatsToPlayer(PlayerStats.Defense, Base: 8);
		statplayer.AddStatsToPlayer(PlayerStats.RegenHP, Base: 5);
	}
}
