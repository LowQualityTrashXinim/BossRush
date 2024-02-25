using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Projectiles;
using BossRush.Common.Utils;

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
		if (modplayer.Duration % 10 != 0) {
			return;
		}
		int damage = (int)player.GetDamage(DamageClass.Magic).ApplyTo(70);
		float knockback = (int)player.GetKnockback(DamageClass.Magic).ApplyTo(4);
		Vector2 velocity = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 15f;
		int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, velocity, ProjectileID.Fireball, damage, knockback, player.whoAmI);
		Main.projectile[proj].friendly = true;
		Main.projectile[proj].hostile = false;
		Main.projectile[proj].timeLeft = 120;
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
public class StarFury : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 225;
		Skill_Duration = 6;
		Skill_CoolDown = BossRushUtils.ToSecond(20);
	}
	public override void Update(Player player) {
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (modplayer.Duration % 120 != 0) {
			return;
		}
		int damage = (int)player.GetDamage(DamageClass.Generic).ApplyTo(1000);
		float knockback = (int)player.GetKnockback(DamageClass.Generic).ApplyTo(10);
		Vector2 position = player.Center.Subtract(0, 1000);
		Vector2 velocity = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * 20f;
		int proj = Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, ProjectileID.StarWrath, damage, knockback, player.whoAmI);
		Main.projectile[proj].friendly = true;
		Main.projectile[proj].hostile = false;
		Main.projectile[proj].tileCollide = false;
		Main.projectile[proj].timeLeft = 600;
	}
}
public class WoodSwordSpirit : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 245;
		Skill_Duration = BossRushUtils.ToSecond(3);
		Skill_CoolDown = BossRushUtils.ToSecond(6);
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		ShootingSword(player);
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		ShootingSword(player);
	}
	private void ShootingSword(Player player) {
		Vector2 position = player.Center + Main.rand.NextVector2Circular(50, 50);
		Vector2 velocity = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero);
		int damage = (int)player.GetDamage(DamageClass.Melee).ApplyTo(24);
		float knockback = (int)player.GetKnockback(DamageClass.Melee).ApplyTo(10);
		int proj = Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<SwordProjectile2>(), damage, knockback, player.whoAmI);
		if (Main.projectile[proj].ModProjectile is SwordProjectile2 woodproj)
			woodproj.ItemIDtextureValue = Main.rand.Next(TerrariaArrayID.AllWoodSword);
	}
}
public class BroadSwordSpirit : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 145;
		Skill_Duration = BossRushUtils.ToSecond(1);
		Skill_CoolDown = BossRushUtils.ToSecond(3);
	}
	public override void Update(Player player) {
		if (player.ownedProjectileCounts[ModContent.ProjectileType<SwordProjectile3>()] < 1) {
			for (int i = 0; i < 3; i++) {
				SummonSword(player, i);
			}
		}
	}
	private void SummonSword(Player player, int index) {
		int damage = (int)player.GetDamage(DamageClass.Melee).ApplyTo(34);
		float knockback = (int)player.GetKnockback(DamageClass.Melee).ApplyTo(3);
		int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<SwordProjectile3>(), damage, knockback, player.whoAmI,0,0, index);
		if (Main.projectile[proj].ModProjectile is SwordProjectile3 woodproj)
			woodproj.ItemIDtextureValue = Main.rand.Next(TerrariaArrayID.AllOreBroadSword);
	}
}
