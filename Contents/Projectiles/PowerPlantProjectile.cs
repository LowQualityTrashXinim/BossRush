using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using BossRush.Contents.Skill;
using System.Collections.Generic;
using System;

namespace BossRush.Contents.Projectiles;
internal class PowerPlantProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.GlassKiln);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 16;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 600;
	}
	public override bool? CanDamage() {
		return false;
	}
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		if (Projectile.timeLeft % 60 == 0) {
			if (player.Center.IsCloseToPosition(Projectile.Center, 300)) {
				player.statLife = Math.Clamp(player.statLife - 10, 1, int.MaxValue);
			}
			Projectile.Center.LookForHostileNPC(out List<NPC> npclist, 300);
			foreach (var npc in npclist) {
				npc.life = Math.Clamp(npc.life - 10, 1, int.MaxValue);
				Projectile.ai[0]++;
			}
		}
		for (int i = 0; i < 10; i++) {
			Dust d = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2CircularEdge(300, 300), 0, 0, DustID.Glass);
			d.noGravity = true;
			d.velocity = Vector2.Zero;
		}
	}
	public override void OnKill(int timeLeft) {
		Player player = Main.player[Projectile.owner];
		player.GetModPlayer<SkillHandlePlayer>().Modify_EnergyAmount((int)(Projectile.ai[0] / 10));
	}
}
internal class TransferStationProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.RepairedLifeCrystal);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 16;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 600;
	}
	public override bool? CanDamage() {
		return false;
	}
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		if (Projectile.timeLeft % 60 == 0) {
			if (player.Center.IsCloseToPosition(Projectile.Center, 300)) {
				player.Heal(5);
			}
			Projectile.Center.LookForHostileNPC(out List<NPC> npclist, 300);
			foreach (var npc in npclist) {
				npc.life = Math.Clamp(npc.life - 10, 1, int.MaxValue);
			}
		}
		for (int i = 0; i < 10; i++) {
			Dust d = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2CircularEdge(300, 300), 0, 0, DustID.LifeCrystal);
			d.noGravity = true;
			d.velocity = Vector2.Zero;
		}
	}
}
