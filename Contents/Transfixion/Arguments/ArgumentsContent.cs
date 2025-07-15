using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.Weapon;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Contents.Transfixion.Artifacts;
using BossRush.Common.Global;

namespace BossRush.Contents.Transfixion.Arguments;

public class Fire : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Red;
	}
	public override TooltipLine ModifyDescription(Player player, AugmentsWeapon acc, int index, Item item, int stack) {
		string desc = Description;
		for (int i = 0; i < stack; i++) {
			switch (stack) {
				case 1:
					desc += "\n" + Description2("1");
					break;
				case 2:
				case 3:
				case 4:
				case 5:
					desc += "\n" + Description2("2");
					break;
			}
		}
		TooltipLine line = new(Mod, Name, desc);
		return line;
	}
	public override string ModifyName(Player player, AugmentsWeapon acc, int index, Item item, int stack) {
		string name = DisplayName;
		switch (stack) {
			case 1:
				name = DisplayName2("1");
				break;
			case 2:
			case 3:
			case 4:
			case 5:
				name = DisplayName2("2");
				break;
		}
		return ColorWrapper(name);
	}
	public override void OnHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.OnFire, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void OnHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion)
			npc.AddBuff(BuffID.OnFire, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int chargeNum = acc.Check_ChargeConvertToStackAmount(index);
		if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
			if (chargeNum >= 1) {
				modifiers.SourceDamage += .2f;
			}
			if (chargeNum >= 2) {
				modifiers.Knockback += .4f;
			}
		}
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		int chargeNum = acc.Check_ChargeConvertToStackAmount(index);
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion) {
			if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
				if (chargeNum >= 1) {
					modifiers.SourceDamage += .2f;
				}
				if (chargeNum >= 2) {
					modifiers.Knockback += .4f;
				}
			}
		}
	}
}
public class FrostBurn : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Cyan;
	}
	public override TooltipLine ModifyDescription(Player player, AugmentsWeapon acc, int index, Item item, int stack) {
		string desc = Description;
		for (int i = 0; i < stack; i++) {
			switch (stack) {
				case 1:
					desc += Description2("1");
					break;
				case 2:
				case 3:
				case 4:
				case 5:
					desc += Description2("2");
					break;
			}
		}
		TooltipLine line = new(Mod, Name, desc);
		return line;
	}
	public override string ModifyName(Player player, AugmentsWeapon acc, int index, Item item, int stack) {
		string name = DisplayName;
		switch (stack) {
			case 1:
				name = DisplayName2("1");
				break;
			case 2:
			case 3:
			case 4:
			case 5:
				name = DisplayName2("2");
				break;
		}
		return ColorWrapper(name);
	}
	public override void OnHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.Frostburn, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void OnHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion)
			npc.AddBuff(BuffID.Frostburn, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int chargeNum = acc.Check_ChargeConvertToStackAmount(index);
		if (target.HasBuff(BuffID.Frostburn) || target.HasBuff(BuffID.Frostburn2)) {
			if (chargeNum >= 1) {
				modifiers.SourceDamage += .2f;
			}
			if (chargeNum >= 2) {
				modifiers.Knockback += .4f;
			}
		}
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		int chargeNum = acc.Check_ChargeConvertToStackAmount(index);
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion) {
			if (target.HasBuff(BuffID.Frostburn) || target.HasBuff(BuffID.Frostburn2)) {
				if (chargeNum >= 1) {
					modifiers.SourceDamage += .2f;
				}
				if (chargeNum >= 2) {
					modifiers.Knockback += .4f;
				}
			}
		}
	}
}
public class BerserkI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.OrangeRed;
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		float percentage = player.statLife / (float)player.statLifeMax2;
		modifiers.SourceDamage += .5f * percentage;
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion) {
			float percentage = player.statLife / (float)player.statLifeMax2;
			modifiers.SourceDamage += .5f * percentage;
		}
	}
}

public class TrueStatus : ModAugments {
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
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int damage = (int)(player.GetWeaponDamage(item) * .1f);
		modifiers.FinalDamage.Flat += damage;
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			int damage = (int)(player.GetWeaponDamage(player.HeldItem) * .1f);
			modifiers.FinalDamage.Flat += damage;
		}
	}
}

public class Terra : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Green;
	}
	public override void OnHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC npc, NPC.HitInfo hitInfo) {
		TerraStrike(player, npc, player.HeldItem, hitInfo);
	}
	public override void OnHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.Check_ItemTypeSource(player.HeldItem.type) && !proj.minion) {
			TerraStrike(player, npc, player.HeldItem, hitInfo);
		}
	}
	private static void TerraStrike(Player player, NPC npc, Item item, NPC.HitInfo hitInfo) {
		if (Main.rand.NextFloat() > .05f) {
			return;
		}
		float randomrotation = Main.rand.NextFloat(90);
		Vector2 randomPosOffset = Main.rand.NextVector2Circular(20f, 20f);
		for (int i = 0; i < 4; i++) {
			Vector2 Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i + randomrotation)) * (3 + Main.rand.NextFloat());
			for (int l = 0; l < 8; l++) {
				float multiplier = Main.rand.NextFloat();
				float scale = MathHelper.Lerp(1.5f, .1f, multiplier);
				int dust = Dust.NewDust(npc.Center + randomPosOffset, 0, 0, DustID.Terra, 0, 0, 0, default, scale);
				Main.dust[dust].velocity = Toward * multiplier;
				Main.dust[dust].noGravity = true;
			}
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
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int damage = (int)player.GetWeaponKnockback(item);
		modifiers.SourceDamage.Base += damage;
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
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
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int knockbackStrength = (int)(player.GetWeaponDamage(item) * .05f);
		modifiers.Knockback += knockbackStrength;
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
			int knockbackStrength = (int)(player.GetWeaponDamage(player.HeldItem) * .05f);
			modifiers.Knockback += knockbackStrength;
		}
	}
}

public class Critical : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Orange;
	}
	public override TooltipLine ModifyDescription(Player player, AugmentsWeapon acc, int index, Item item, int stack) {
		string desc = Description;
		for (int i = 0; i < stack; i++) {
			switch (stack) {
				case 1:
					desc += "\n" + Description2("1");
					break;
				case 2:
					desc += "\n" + Description2("2");
					break;
				case 3:
					desc += "\n" + Description2("3");
					break;
				case 4:
					desc += "\n" + Description2("4");
					break;
				case 5:
					break;
			}
		}
		TooltipLine line = new(Mod, Name, desc);
		return line;
	}
	public override string ModifyName(Player player, AugmentsWeapon acc, int index, Item item, int stack) {
		string name = DisplayName;
		switch (stack) {
			case 1:
				name = DisplayName2("1");
				break;
			case 2:
				name = DisplayName2("2");
				break;
			case 3:
				name = DisplayName2("3");
				break;
			case 4:
				name = DisplayName2("4");
				break;
			case 5:
				name = DisplayName2("5");
				break;
		}
		return ColorWrapper(name);
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 5);
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 1.1f);
	}
	public override void OnHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC npc, NPC.HitInfo hitInfo) {
		if (acc.Check_ChargeConvertToStackAmount(index) >= 2) {
			if (hitInfo.Crit) {
				player.Heal(Math.Clamp((int)Math.Ceiling(player.statLifeMax2 * 0.01f), 1, player.statLifeMax2));
			}
		}
	}
	public override void OnHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (acc.Check_ChargeConvertToStackAmount(index) >= 2) {
			if (hitInfo.Crit && !proj.minion && proj.Check_ItemTypeSource(player.HeldItem.type)) {
				player.Heal(Math.Clamp((int)Math.Ceiling(player.statLifeMax2 * 0.01f), 1, player.statLifeMax2));
			}
		}
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (acc.Check_ChargeConvertToStackAmount(index) >= 1) {
			if (player.GetModPlayer<PlayerStatsHandle>().ModifyHit_Before_Crit) {
				modifiers.ScalingArmorPenetration += .5f;
			}
		}
		if (acc.Check_ChargeConvertToStackAmount(index) >= 3) {
			int critchanceReroll = player.GetWeaponCrit(item);
			if (Main.rand.Next(1, 101) < critchanceReroll) {
				modifiers.CritDamage += 1;
			}
		}
	}

	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (acc.Check_ChargeConvertToStackAmount(index) >= 1) {
			if (player.GetModPlayer<PlayerStatsHandle>().ModifyHit_Before_Crit && !proj.minion && proj.Check_ItemTypeSource(player.HeldItem.type)) {
				modifiers.ScalingArmorPenetration += .5f;
			}
		}
		if (acc.Check_ChargeConvertToStackAmount(index) >= 3) {
			int critchanceReroll = proj.CritChance;
			if (Main.rand.Next(1, 101) < critchanceReroll) {
				modifiers.CritDamage += 1;
			}
		}
	}
}

public class VampireI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkRed;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		player.GetModPlayer<PlayerStatsHandle>().LifeSteal += 0.01f;
	}
}

public class VampireII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkRed;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		if (!Main.IsItDay())
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritDamage, Multiplicative: 1.5f);
	}
}

public class VampireIII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkRed;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
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
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
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
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, Base: player.BuffAmount());
	}
}

public class Light : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Pink;
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.GetLifePercent() > .8f)
			modifiers.SourceDamage += 1.5f;
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.GetLifePercent() > .8f)
			modifiers.SourceDamage += 1.5f;
	}
}

public class Dark : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Purple;
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.GetLifePercent() < .4f)
			modifiers.SourceDamage += 1.5f;
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.GetLifePercent() < .4f)
			modifiers.SourceDamage += 1.5f;
	}
}

public class Union : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Bisque;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
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

public class ShadowFlame : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.MediumPurple;
	}
	public override TooltipLine ModifyDescription(Player player, AugmentsWeapon acc, int index, Item item, int stack) {
		string desc = Description;
		for (int i = 0; i < stack; i++) {
			switch (stack) {
				case 1:
					desc += Description2("1");
					break;
				case 2:
				case 3:
				case 4:
				case 5:
					desc += Description2("2");
					break;
			}
		}
		TooltipLine line = new(Mod, Name, desc);
		return line;
	}
	public override string ModifyName(Player player, AugmentsWeapon acc, int index, Item item, int stack) {
		string name = DisplayName;
		switch (stack) {
			case 1:
				name = DisplayName2("1");
				break;
			case 2:
			case 3:
			case 4:
			case 5:
				name = DisplayName2("2");
				break;
		}
		return ColorWrapper(name);
	}
	public override void OnHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.ShadowFlame, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void OnHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion)
			npc.AddBuff(BuffID.ShadowFlame, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int chargeNum = acc.Check_ChargeConvertToStackAmount(index);
		if (target.HasBuff(BuffID.ShadowFlame)) {
			if (chargeNum >= 1) {
				modifiers.SourceDamage += .2f;
			}
			if (chargeNum >= 2) {
				modifiers.Knockback += .4f;
			}
		}
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		int chargeNum = acc.Check_ChargeConvertToStackAmount(index);
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion) {
			if (target.HasBuff(BuffID.Frostburn)) {
				if (chargeNum >= 1) {
					modifiers.SourceDamage += .2f;
				}
				if (chargeNum >= 2) {
					modifiers.Knockback += .4f;
				}
			}

		}
	}
}

public class CursedFlame : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.ForestGreen;
	}
	public override TooltipLine ModifyDescription(Player player, AugmentsWeapon acc, int index, Item item, int stack) {
		string desc = Description;
		for (int i = 0; i < stack; i++) {
			switch (stack) {
				case 1:
					desc += Description2("1");
					break;
				case 2:
				case 3:
				case 4:
				case 5:
					desc += Description2("2");
					break;
			}
		}
		TooltipLine line = new(Mod, Name, desc);
		return line;
	}
	public override string ModifyName(Player player, AugmentsWeapon acc, int index, Item item, int stack) {
		string name = DisplayName;
		switch (stack) {
			case 1:
				name = DisplayName2("1");
				break;
			case 2:
			case 3:
			case 4:
			case 5:
				name = DisplayName2("2");
				break;
		}
		return ColorWrapper(name);
	}
	public override void OnHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.CursedInferno, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void OnHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion)
			npc.AddBuff(BuffID.CursedInferno, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int chargeNum = acc.Check_ChargeConvertToStackAmount(index);
		if (target.HasBuff(BuffID.CursedInferno)) {
			if (chargeNum >= 1) {

				modifiers.SourceDamage += .2f;
			}
			if (chargeNum >= 2) {
				modifiers.Knockback += .4f;
			}
		}
		if (target.HasBuff(BuffID.CursedInferno)) {
		}
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		int chargeNum = acc.Check_ChargeConvertToStackAmount(index);
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion) {
			if (target.HasBuff(BuffID.CursedInferno)) {
				if (chargeNum >= 1) {
					modifiers.SourceDamage += .2f;
				}
				if (chargeNum >= 2) {
					modifiers.Knockback += .4f;
				}
			}
		}
	}
}
public class Poison : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.PaleGreen;
	}
	public override TooltipLine ModifyDescription(Player player, AugmentsWeapon acc, int index, Item item, int stack) {
		string desc = Description;
		for (int i = 0; i < stack; i++) {
			switch (stack) {
				case 1:
					desc += Description2("1");
					break;
				case 2:
				case 3:
				case 4:
				case 5:
					desc += Description2("2");
					break;
			}
		}
		TooltipLine line = new(Mod, Name, desc);
		return line;
	}
	public override string ModifyName(Player player, AugmentsWeapon acc, int index, Item item, int stack) {
		string name = DisplayName;
		switch (stack) {
			case 1:
				name = DisplayName2("1");
				break;
			case 2:
			case 3:
			case 4:
			case 5:
				name = DisplayName2("2");
				break;
		}
		return ColorWrapper(name);
	}
	public override void OnHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.Poisoned, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void OnHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion)
			npc.AddBuff(BuffID.Poisoned, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int chargeNum = acc.Check_ChargeConvertToStackAmount(index);
		if (target.HasBuff(BuffID.Poisoned)) {
			if (chargeNum >= 1) {
				modifiers.SourceDamage += .2f;
			}
			if (chargeNum >= 2) {
				modifiers.Knockback += .4f;
			}
		}
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type! && proj.minion) {
			int chargeNum = acc.Check_ChargeConvertToStackAmount(index);
			if (target.HasBuff(BuffID.Poisoned)) {
				if (chargeNum >= 1) {
					modifiers.SourceDamage += .2f;
				}
				if (chargeNum >= 2) {
					modifiers.Knockback += .4f;
				}
			}
		}
	}
}
public class Venom : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Purple;
	}
	public override TooltipLine ModifyDescription(Player player, AugmentsWeapon acc, int index, Item item, int stack) {
		string desc = Description;
		for (int i = 0; i < stack; i++) {
			switch (stack) {
				case 1:
					desc += Description2("1");
					break;
				case 2:
				case 3:
				case 4:
				case 5:
					desc += Description2("2");
					break;
			}
		}
		TooltipLine line = new(Mod, Name, desc);
		return line;
	}
	public override string ModifyName(Player player, AugmentsWeapon acc, int index, Item item, int stack) {
		string name = DisplayName;
		switch (stack) {
			case 1:
				name = DisplayName2("1");
				break;
			case 2:
			case 3:
			case 4:
			case 5:
				name = DisplayName2("2");
				break;
		}
		return ColorWrapper(name);
	}
	public override void OnHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC npc, NPC.HitInfo hitInfo) {
		npc.AddBuff(BuffID.Venom, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void OnHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC npc, NPC.HitInfo hitInfo) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type)
			npc.AddBuff(BuffID.Venom, BossRushUtils.ToSecond(Main.rand.Next(1, 3)));
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int chargeNum = acc.Check_ChargeConvertToStackAmount(index);
		if (target.HasBuff(BuffID.Venom)) {
			if (chargeNum >= 1) {
				modifiers.SourceDamage += .2f;
			}
			if (chargeNum >= 2) {
				modifiers.Knockback += .4f;
			}
		}
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type && !proj.minion) {
			int chargeNum = acc.Check_ChargeConvertToStackAmount(index);
			if (target.HasBuff(BuffID.Venom)) {
				if (chargeNum >= 1) {
					modifiers.SourceDamage += .2f;
				}
				if (chargeNum >= 2) {
					modifiers.Knockback += .4f;
				}
			}
		}
	}
}
public class Strengthen : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.IndianRed;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle stathandle = player.ModPlayerStats();
		stathandle.AddStatsToPlayer(PlayerStats.PureDamage, 1.03f);
		stathandle.AddStatsToPlayer(PlayerStats.CritDamage, 1.06f);
		stathandle.AddStatsToPlayer(PlayerStats.Defense, Base: 2);
		stathandle.AddStatsToPlayer(PlayerStats.MaxHP, Base: 5);
		stathandle.AddStatsToPlayer(PlayerStats.MaxMana, Base: 5);
		stathandle.AddStatsToPlayer(PlayerStats.RegenHP, Base: 1);
		stathandle.AddStatsToPlayer(PlayerStats.CritChance, Base: 1);
	}
}

public class Ghost : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.Wheat;
	}
	public override void OnHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC npc, NPC.HitInfo hitInfo) {
		if (!player.immune) {
			player.AddImmuneTime(-1, 8);
		}
	}
}

public class ExtraLife : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.White;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		player.GetModPlayer<PlayerStatsHandle>().Add_ExtraLifeWeapon(item);
	}
}

public class IntoxicateI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.GreenYellow;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
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
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
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
	public override void OnHitByNPC(Player player, AugmentsWeapon acc, int index, NPC npc, Player.HurtInfo info) {
		if (Main.rand.NextBool(3)) {
			player.Heal((int)Math.Ceiling(player.statLifeMax2 * .05f));
		}
	}
	public override void OnHitByProj(Player player, AugmentsWeapon acc, int index, Projectile projectile, Player.HurtInfo info) {
		if (Main.rand.NextBool(3)) {
			player.Heal((int)Math.Ceiling(player.statLifeMax2 * .05f));
		}
	}
}
public class ReactiveHealingII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.ForestGreen;
	}
	public override void OnHitByNPC(Player player, AugmentsWeapon acc, int index, NPC npc, Player.HurtInfo info) {
		if (Main.rand.NextBool(3) && !player.HasBuff<ReactiveHealingBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveHealingBuff>(), BossRushUtils.ToSecond(Main.rand.Next(4, 11)));
		}
	}
	public override void OnHitByProj(Player player, AugmentsWeapon acc, int index, Projectile projectile, Player.HurtInfo info) {
		if (Main.rand.NextBool(3) && !player.HasBuff<ReactiveHealingBuff>()) {
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
	public override void OnHitByNPC(Player player, AugmentsWeapon acc, int index, NPC npc, Player.HurtInfo info) {
		if (Main.rand.NextBool(4) && !player.HasBuff<ReactiveDefenseBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveDefenseBuff>(), BossRushUtils.ToSecond(Main.rand.Next(4, 11)));
		}
	}
	public override void OnHitByProj(Player player, AugmentsWeapon acc, int index, Projectile projectile, Player.HurtInfo info) {
		if (Main.rand.NextBool(4) && !player.HasBuff<ReactiveDefenseBuff>()) {
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
	public override void OnHitByNPC(Player player, AugmentsWeapon acc, int index, NPC npc, Player.HurtInfo info) {
		if (Main.rand.NextBool(4) && !player.HasBuff<ReactiveDefenseIIBuff>()) {
			player.AddBuff(ModContent.BuffType<ReactiveDefenseIIBuff>(), BossRushUtils.ToSecond(Main.rand.Next(4, 11)));
		}
	}
	public override void OnHitByProj(Player player, AugmentsWeapon acc, int index, Projectile projectile, Player.HurtInfo info) {
		if (Main.rand.NextBool(4) && !player.HasBuff<ReactiveDefenseIIBuff>()) {
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
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, 1 + player.statLifeMax2 * .0005f);
	}
}
public class VitalityStrikeII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.PaleVioletRed;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.CritChance, Base: player.statLifeMax2 * .01f);
	}
}

public class ArcaneStrikeI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkBlue;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, 1 + player.statManaMax2 * .0005f);
	}
}
public class ArcaneStrikeII : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkBlue;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.CritChance, Base: player.statManaMax2 * .01f);
	}
}

public class StealthStrikeI : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.DarkGray;
	}
	public override void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (player.invis)
			modifiers.SourceDamage += .25f;
	}
	public override void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
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
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.FullHPDamage, 2f);
	}
}
public class DryadBlessing : ModAugments {
	public override void SetStaticDefaults() {
		tooltipColor = Color.LimeGreen;
	}
	public override void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenHP, Base: 3);
	}
	public override void OnHitByProj(Player player, AugmentsWeapon acc, int index, Projectile projectile, Player.HurtInfo info) {
		if (Main.rand.NextFloat() <= Main.rand.NextFloat(.1f, .4f) && !player.HasBuff<DryadBlessing_Buff>()) {
			player.AddBuff(ModContent.BuffType<DryadBlessing_Buff>(), BossRushUtils.ToSecond(Main.rand.Next(3, 8)));
		}
	}
	public override void OnHitByNPC(Player player, AugmentsWeapon acc, int index, NPC npc, Player.HurtInfo info) {
		if (Main.rand.NextFloat() <= Main.rand.NextFloat(.1f, .4f) && !player.HasBuff<DryadBlessing_Buff>()) {
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
