﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange;
internal class RoguelikeProjectileOverhaul : GlobalProjectile {
	public override void SetDefaults(Projectile entity) {
		if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul) {
			return;
		}
		if (Main.LocalPlayer.strongBees) {
			if (entity.type == ProjectileID.BeeArrow) {
				entity.extraUpdates += 1;
			}
		}
	}
	public override bool PreAI(Projectile projectile) {
		if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul) {
			return base.PreAI(projectile);
		}
		Player player = Main.player[projectile.owner];
		if (player.strongBees) {
			if (projectile.type == ProjectileID.Bee || projectile.type == ProjectileID.GiantBee) {
				projectile.velocity /= 1.25f;
			}
		}
		return base.PreAI(projectile);
	}
	public override void PostAI(Projectile projectile) {
		if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul) {
			return;
		}
		Player player = Main.player[projectile.owner];
		if (player.strongBees) {
			if (projectile.type == ProjectileID.Bee || projectile.type == ProjectileID.GiantBee) {
				projectile.velocity *= 1.25f;
			}
		}
	}
}
