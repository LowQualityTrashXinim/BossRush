using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.Projectiles {
	public class SacrificialWormholeProjectile : ModProjectile {
		public override string Texture => BossRushTexture.WHITEBALL;
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.timeLeft = 1800;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}
		public override Color? GetAlpha(Color lightColor) {
			return new Color(0, 0, 0);
		}
		float CoolDown { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
		public override void AI() {
			for (int i = 0; i < 50; i++) {
				Vector2 pos = Projectile.Center + Main.rand.NextVector2CircularEdge(600, 600);
				float multiplier = Main.rand.NextFloat(1, 1.2f);
				Dust dust = Dust.NewDustDirect(pos, 0, 0, DustID.Granite, newColor: Color.Black);
				dust.noGravity = true;
				dust.velocity = (Projectile.Center - pos).SafeNormalize(Vector2.Zero) * 20 * multiplier;
				dust.scale = multiplier;
			}
			for (int i = 0; i < 50; i++) {
				Vector2 pos = Projectile.Center + Main.rand.NextVector2CircularEdge(600, 600);
				float multiplier = Main.rand.NextFloat(1, 1.2f);
				Dust dust = Dust.NewDustDirect(pos, 0, 0, DustID.Blood, newColor: Color.Red);
				dust.noGravity = true;
				dust.velocity = (Projectile.Center - pos).SafeNormalize(Vector2.Zero) * 20 * multiplier;
				dust.scale = multiplier;
			}
			for (int i = 0; i < 3; i++) {
				Dust dustEff = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Blood, newColor: Color.Red);
				dustEff.noGravity = true;
				dustEff.velocity = Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(5, 13), Main.rand.NextFloat(5, 13));
				dustEff.scale = Main.rand.NextFloat(2, 2.2f);
				Dust dustEff2 = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Granite, newColor: Color.Black);
				dustEff2.noGravity = true;
				dustEff2.velocity = Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(5, 13), Main.rand.NextFloat(5, 13));
				dustEff2.scale = Main.rand.NextFloat(2, 2.2f);
			}
			Player player = Main.player[Projectile.owner];
			if (player.Center.IsCloseToPosition(Projectile.Center, 600)) {
				player.AddBuff(BuffID.Suffocation, 10);
				player.velocity += (Projectile.Center - player.Center).SafeNormalize(Vector2.Zero) * .2f;
			}
			if (--CoolDown <= 0) {
				Projectile.Center.LookForHostileNPC(out List<NPC> npclist, 600);
				if (npclist.Count <= 0) {
					return;
				}
				foreach (NPC npc in npclist) {
					player.StrikeNPCDirect(npc, npc.CalculateHitInfo(240, 1));
					npc.velocity += (Projectile.Center - npc.Center).SafeNormalize(Vector2.Zero);
				}
				CoolDown = 10;
			}
		}
	}
}
