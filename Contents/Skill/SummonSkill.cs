using BossRush.Common.Utils;
using BossRush.Contents.Projectiles;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.BuffAndDebuff.PlayerDebuff;

namespace BossRush.Contents.Skill;

public class BroadSwordSpirit : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 545;
		Skill_Duration = BossRushUtils.ToSecond(1);
		Skill_CoolDown = BossRushUtils.ToSecond(3);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void Update(Player player) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<SwordProjectile3>()] < 1) {
			for (int i = 0; i < 3; i++) {
				SummonSword(player, i);
			}
		}
	}
	private void SummonSword(Player player, int index) {
		int weapondamage = (int)(player.GetWeaponDamage(player.HeldItem) * .75f);
		int damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(34) + weapondamage;
		float knockback = (int)player.GetTotalKnockback(DamageClass.Melee).ApplyTo(3);
		int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<SwordProjectile3>(), damage, knockback, player.whoAmI, 0, 0, index);
		if (Main.projectile[proj].ModProjectile is SwordProjectile3 woodproj)
			woodproj.ItemIDtextureValue = Main.rand.Next(TerrariaArrayID.AllOreBroadSword);
	}
}
public class WoodSwordSpirit : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 645;
		Skill_Duration = BossRushUtils.ToSecond(3);
		Skill_CoolDown = BossRushUtils.ToSecond(6);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void Update(Player player) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<SoulWoodSword>()] < 1) {
			int damage = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(24);
			float knockback = (int)player.GetTotalKnockback(DamageClass.Melee).ApplyTo(5);
			int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<SoulWoodSword>(), damage, knockback, player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile2 woodproj)
				woodproj.ItemIDtextureValue = Main.rand.Next(TerrariaArrayID.AllWoodSword);
		}
	}
}

public class WilloFreeze : ModSkill {
	public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<WilloFreeze>();
	public override void SetDefault() {
		Skill_EnergyRequire = 485;
		Skill_Duration = BossRushUtils.ToSecond(4);
		Skill_CoolDown = BossRushUtils.ToSecond(12);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void Update(Player player) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<WilloFreezeProjectile>()] < 1) {
			int damage = (int)player.GetTotalDamage(DamageClass.Magic).ApplyTo(36);
			float knockback = (int)player.GetTotalKnockback(DamageClass.Magic).ApplyTo(5);
			for (int i = 0; i < 4; i++) {
				Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<WilloFreezeProjectile>(), damage, knockback, player.whoAmI, 75, i, 4);
			}
		}
	}
}

public class PowerPlant : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 525;
		Skill_Duration = BossRushUtils.ToSecond(4);
		Skill_CoolDown = BossRushUtils.ToSecond(12);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void Update(Player player) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<PowerPlantProjectile>()] < 1) {
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<PowerPlantProjectile>(), 0, 0, player.whoAmI);
		}
	}
}
public class TransferStation : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 525;
		Skill_Duration = BossRushUtils.ToSecond(4);
		Skill_CoolDown = BossRushUtils.ToSecond(12);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void Update(Player player) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<TransferStationProjectile>()] < 1) {
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<TransferStationProjectile>(), 0, 0, player.whoAmI);
		}
	}
}
public class OrbOfPurity : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 325;
		Skill_Duration = BossRushUtils.ToSecond(4);
		Skill_CoolDown = BossRushUtils.ToSecond(12);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void Update(Player player) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<DiamondSwotaffOrb>()] < 1) {
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<DiamondSwotaffOrb>(), 10, 0, player.whoAmI);
		}
	}
}

public class PhoenixBlazingTornado : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 325;
		Skill_Duration = BossRushUtils.ToSecond(5);
		Skill_CoolDown = BossRushUtils.ToSecond(12);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void Update(Player player) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<BlazingTornado>()] < 1) {
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<BlazingTornado>(), 120, 0, player.whoAmI);
		}
	}
}

public class DebugCommand : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 777;
		Skill_Duration = BossRushUtils.ToSecond(10);
		Skill_CoolDown = BossRushUtils.ToSecond(60);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer modplayer) {
		player.Center.LookForHostileNPC(out List<NPC> npclist, 2500);
		foreach (NPC npc in npclist) {
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo(2000, BossRushUtils.DirectionFromPlayerToNPC(player.Center.X, npc.Center.X)));
		}
	}
}

public class LucidNightmares : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 450;
		Skill_Duration = BossRushUtils.ToSecond(4);
		Skill_CoolDown = BossRushUtils.ToSecond(12);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer modplayer) {
		for (int i = 0; i < 3; i++) {
			Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.UnitX.Vector2DistributeEvenly(3, 360, i) * Main.rand.NextFloat(4, 7), ModContent.ProjectileType<NightmaresProjectile>(), 53, 0, player.whoAmI);
		}
	}
}

public class SacrificialWormhole : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 150;
		Skill_Duration = BossRushUtils.ToSecond(14);
		Skill_CoolDown = BossRushUtils.ToSecond(30);
		Skill_Type = SkillTypeID.Skill_Summon;
	}
	public override void OnTrigger(Player player, SkillHandlePlayer modplayer) {
		Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero,
			ModContent.ProjectileType<SacrificialWormholeProjectile>(), 53, 0, player.whoAmI);
		player.AddBuff<LifeLoss>(BossRushUtils.ToSecond(60));
	}
}
