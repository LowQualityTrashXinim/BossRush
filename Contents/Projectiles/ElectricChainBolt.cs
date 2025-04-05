using Microsoft.Xna.Framework;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using BossRush.Common.Global;
using BossRush.Contents.Skill;

namespace BossRush.Contents.Projectiles;
internal class ElectricChainBolt : ModProjectile {
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 2000;
		Projectile.penetrate = 10;
		Projectile.extraUpdates = 100;
	}
	NPC npc = null;
	public override void AI() {
		int electic = Dust.NewDust(Projectile.Center, 0, 0, DustID.Electric);
		Main.dust[electic].noGravity = true;
		Main.dust[electic].velocity = Vector2.Zero;
		if (npc == null) {
			float distance = 1000 * 1000;
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC mainnpc = Main.npc[i];
				if (mainnpc.active
					&& BossRushUtils.CompareSquareFloatValue(Projectile.Center, mainnpc.Center, distance, out float dis)
					&& mainnpc.CanBeChasedBy()
					&& !mainnpc.friendly
					&& Collision.CanHitLine(Projectile.position, 10, 10, mainnpc.position, mainnpc.width, mainnpc.height)
					&& mainnpc.immune[Projectile.owner] <= 0
					) {
					distance = dis;
					npc = mainnpc;
				}
			}
			return;
		}
		Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 10;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.Electrified, BossRushUtils.ToSecond(Main.rand.Next(13, 17)));
		Main.player[Projectile.owner].GetModPlayer<SkillHandlePlayer>().Modify_EnergyAmount(5);
		npc = null;
	}
}
