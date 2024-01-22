using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

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
		Skill_EnergyRequire = 330;
		Skill_Duration = BossRushUtils.ToSecond(.1f);
		Skill_CoolDown = BossRushUtils.ToSecond(15);
	}
	public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
		damage += 3f;
	}
}
