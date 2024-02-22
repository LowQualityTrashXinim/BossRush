using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Projectiles;

namespace BossRush.Contents.Skill;
public class HellFireArrowRain : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 75;
		Skill_Duration = BossRushUtils.ToSecond(1);
		Skill_CoolDown = BossRushUtils.ToSecond(5);
	}
	public override void Update(Player player) {
		if (!Main.rand.NextBool(3)) {
			return;
		}
		int damage = (int)player.GetDamage(DamageClass.Ranged).ApplyTo(20);
		float knockback = (int)player.GetKnockback(DamageClass.Ranged).ApplyTo(2);
		Vector2 position = Main.MouseWorld;
		position.Y -= 500;
		position.X += Main.rand.NextFloat(-75, 75);
		Projectile.NewProjectile(player.GetSource_FromThis(), position, Vector2.UnitY * Main.rand.NextFloat(20, 24), ProjectileID.HellfireArrow, damage, knockback, player.whoAmI);
		for (int l = 0; l < 2; l++) {
			int dust = Dust.NewDust(position, 0, 0, DustID.Smoke, Scale: Main.rand.NextFloat(3, 4));
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(2, 2);
		}
	}
}
public class Increases_3xDamage : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 230;
		Skill_Duration = 8;
		Skill_CoolDown = BossRushUtils.ToSecond(15);
	}
	public override void ResetEffect(Player player) {
		player.GetDamage(DamageClass.Generic) += 3f;
	}
}
public class SpiritBurst : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 110;
		Skill_Duration = BossRushUtils.ToSecond(.5f);
		Skill_CoolDown = BossRushUtils.ToSecond(7);
	}
	public override void Update(Player player) {
		if (!Main.rand.NextBool(2)) {
			return;
		}
		int damage = (int)player.GetDamage(DamageClass.Magic).ApplyTo(40);
		float knockback = (int)player.GetKnockback(DamageClass.Magic).ApplyTo(2);
		Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Main.rand.NextVector2CircularEdge(5, 5), ModContent.ProjectileType<SpiritProjectile>(), damage, knockback, player.whoAmI);
	}
}
public class InfiniteManaSupply : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 60;
		Skill_Duration = BossRushUtils.ToSecond(.5f);
		Skill_CoolDown = BossRushUtils.ToSecond(6);
	}
	public override void Update(Player player) {
		if (player.statMana < player.statManaMax2) {
			player.statMana++;
		}
	}
	public override void OnMissingMana(Player player, Item item, int neededMana) {
		player.statMana += neededMana;
	}
}
public class GuaranteedCrit : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 125;
		Skill_Duration = BossRushUtils.ToSecond(.5f);
		Skill_CoolDown = BossRushUtils.ToSecond(5);
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.SetCrit();
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.SetCrit();
	}
}
public class FireBall : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 45;
		Skill_Duration = BossRushUtils.ToSecond(.12f);
		Skill_CoolDown = BossRushUtils.ToSecond(2);
	}
	public override void Update(Player player) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if(modplayer.Duration % 10 != 0) {
			return;
		}
		int damage = (int)player.GetDamage(DamageClass.Magic).ApplyTo(70);
		float knockback = (int)player.GetKnockback(DamageClass.Magic).ApplyTo(4);
		Vector2 velocity = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 15f;
		int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, velocity, ProjectileID.Fireball, damage, knockback, player.whoAmI);
		Main.projectile[proj].friendly = true;
		Main.projectile[proj].hostile = false;
	}
}
public class RapidHealing : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 145;
		Skill_Duration = BossRushUtils.ToSecond(2);
		Skill_CoolDown = BossRushUtils.ToSecond(30);
	}
	public override void Update(Player player) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (modplayer.Duration % 6 != 0) {
			return;
		}
		player.Heal(6);
	}
}
