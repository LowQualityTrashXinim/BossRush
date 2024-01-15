using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Skill;
public class ArrowRain : ModSkill {
	public override void SetDefault() {
		Skill_EnergyRequire = 150;
		Skill_Duration = BossRushUtils.ToSecond(5);
		Skill_CoolDown = BossRushUtils.ToSecond(6);
	}
	public override void Update(Player player) {
		if (Main.rand.NextBool()) {
			return;
		}
		int damage = (int)player.GetDamage(DamageClass.Ranged).ApplyTo(20);
		float knockback = (int)player.GetKnockback(DamageClass.Ranged).ApplyTo(2);
		Vector2 position = Main.MouseWorld;
		position.Y -= 500;
		position.X += Main.rand.NextFloat(-50, 50);
		Projectile.NewProjectile(player.GetSource_FromThis(), position, Vector2.UnitY * Main.rand.NextFloat(11, 15), ProjectileID.WoodenArrowFriendly, damage, knockback, player.whoAmI);
		for (int l = 0; l < 2; l++) {
			int dust = Dust.NewDust(position, 0, 0, DustID.Smoke, Scale: Main.rand.NextFloat(3, 4));
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(2, 2);
		}
	}
}
