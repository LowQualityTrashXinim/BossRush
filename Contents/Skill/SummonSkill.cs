using BossRush.Common.Utils;
using BossRush.Contents.Projectiles;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Skill;

public class BroadSwordSpirit : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 145;
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
		Skill_EnergyRequire = 145;
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
