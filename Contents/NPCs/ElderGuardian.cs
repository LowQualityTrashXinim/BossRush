using BossRush.Texture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.NPCs {
	internal class ElderGuardian : ModNPC {
		public override string Texture => BossRushTexture.DIAMONDSWOTAFFORB;
		public override void SetStaticDefaults() {
			NPCID.Sets.ImmuneToAllBuffs[Type] = true;
		}
		public override void SetDefaults() {
			NPC.lifeMax = 200000;
			NPC.damage = 1000;
			NPC.defense = 200;
			NPC.friendly = false;
			NPC.width = NPC.height = 30;
			NPC.lavaImmune = true;
			NPC.trapImmune = true;
			NPC.knockBackResist = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.scale = 5f;
			NPC.color = Color.Black;
		}
		public override void AI() {
			for (int i = 0; i < 10; i++) {
				int dust = Dust.NewDust(NPC.Center + Main.rand.NextVector2Circular(100, 100), 0, 0, DustID.Granite, 0, 0, 0, Color.Black, Main.rand.NextFloat(1.5f, 3f));
				Main.dust[dust].noGravity = true;
				Main.dust[dust].color = Color.Black;
				Main.dust[dust].fadeIn = 1f;
			}
			Player player = Main.player[NPC.target];
			NPC.velocity = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero) * 7;
			if (!player.active || player.dead) {
				NPC.FindClosestPlayer();
				return;
			}
			if (NPC.ai[0] >= 100) {
				for (int i = 0; i < 8; i++) {
					Vector2 vel = Vector2.One.Vector2DistributeEvenly(8, 360, i);
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vel, ModContent.ProjectileType<GuardianSmallerProjectile>(), (int)(NPC.damage * .25f), 40, -1, NPC.target);
				}
				NPC.ai[0] = 0;
			}
			if (NPC.ai[1] % 20 == 0) {
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * .1f, ModContent.ProjectileType<GuardianSmallerProjectile>(), (int)(NPC.damage * .25f), 40, -1, NPC.target);
			}
			if (NPC.ai[1] >= 100) {
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * .25f, ModContent.ProjectileType<GuardianProjectile>(), NPC.damage, 40, -1, NPC.target);
				NPC.ai[1] = 0;
			}
			NPC.ai[0]++;
			NPC.ai[1]++;
		}
	}
}
