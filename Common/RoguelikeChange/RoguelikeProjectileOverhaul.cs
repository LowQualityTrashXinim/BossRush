﻿using BossRush.Common.General;
using BossRush.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange;
internal class RoguelikeProjectileOverhaul : GlobalProjectile {
	public override void SetDefaults(Projectile entity) {
		if (!UniversalSystem.Check_RLOH()) {
			return;
		}
		if (entity.type == ProjectileID.ChlorophyteOrb) {
			entity.penetrate = 1;
		}
		if (Main.LocalPlayer.strongBees) {
			if (entity.type == ProjectileID.BeeArrow) {
				entity.extraUpdates += 1;
			}
		}
	}
	public override bool PreAI(Projectile projectile) {
		if (!UniversalSystem.Check_RLOH()) {
			return base.PreAI(projectile);
		}
		Player player = Main.player[projectile.owner];
		if (player.strongBees) {
			if (projectile.type == ProjectileID.Bee || projectile.type == ProjectileID.GiantBee) {
				projectile.velocity /= 1.25f;
			}
		}
		if (projectile.type == ProjectileID.InfluxWaver && projectile.Check_ItemTypeSource(ItemID.InfluxWaver)) {
			if (projectile.Center.LookForHostileNPC(out NPC npc, 300)) {
				projectile.velocity += (npc.Center - projectile.Center).SafeNormalize(Vector2.Zero) * .5f;
				projectile.timeLeft = 90;
			}
		}
		return base.PreAI(projectile);
	}
	public override void PostAI(Projectile projectile) {
		if (!UniversalSystem.Check_RLOH()) {
			return;
		}
		Player player = Main.player[projectile.owner];
		if (projectile.type == ProjectileID.ChlorophyteOrb) {
			Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.ChlorophyteWeapon);
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;

			Vector2 pos = projectile.Center.LookForHostileNPCPositionClosest(500);
			if (pos != Vector2.Zero) {
				float rotateTo = (pos - projectile.Center).SafeNormalize(Vector2.Zero).ToRotation();
				float currentRotation = projectile.velocity.ToRotation();

				projectile.velocity = projectile.velocity.RotatedBy(rotateTo - currentRotation);
			}
			if (player.strongBees) {
				if (projectile.type == ProjectileID.Bee || projectile.type == ProjectileID.GiantBee) {
					projectile.velocity *= 1.25f;
				}
			}
		}
	}
}
