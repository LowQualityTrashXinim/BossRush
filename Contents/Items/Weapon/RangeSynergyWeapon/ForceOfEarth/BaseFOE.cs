﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.ForceOfEarth {
	abstract class BaseFOE : ModProjectile {
		public override void SetDefaults() {
			Projectile.tileCollide = false;
			Projectile.width = 16;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.light = 0.5f;
			Projectile.DamageType = DamageClass.Ranged;
		}
		public virtual float OffsetBehavior => 0;
		public override void AI() {
			Behavior(OffsetBehavior);
		}
		public void Behavior(float OFFSet, float position = 40) {
			Player player = Main.player[Projectile.owner];
			if (player.dead || !player.active || !player.HasBuff(ModContent.BuffType<EarthPower>())) {
				Projectile.Kill();
			}
			Vector2 Rotate = new Vector2(1, 1).RotatedBy(MathHelper.ToRadians(OFFSet));
			Projectile.Center = player.Center + Rotate * position;

			Vector2 aimto = Main.MouseWorld - Projectile.position;
			Vector2 safeAimto = aimto.SafeNormalize(Vector2.UnitX);
			Projectile.rotation = safeAimto.ToRotation();
		}
		public override bool? CanDamage() {
			return false;
		}
	}
}
