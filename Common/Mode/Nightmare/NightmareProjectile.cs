using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.Mode.Nightmare;
internal class NightmareProjectile : GlobalProjectile {
	public override void PostAI(Projectile projectile) {
		base.PostAI(projectile);
		if (!NightmareSystem.IsANightmareWorld()) {
			return;
		}
		if (projectile.type == ProjectileID.CultistRitual) {
			CultistRitualAI(projectile);
			return;
		}
	}
	private void CultistRitualAI(Projectile projectile) {
		if (projectile.ai[1] != -1f && Main.netMode != NetmodeID.MultiplayerClient) {
			if (projectile.ai[0] == 100f) {
				if (!NPC.AnyNPCs(454))
					projectile.ai[1] = NPC.NewNPC(Entity.InheritSource(projectile), (int)projectile.Center.X, (int)projectile.Center.Y, 454);
				else
					projectile.ai[1] = NPC.NewNPC(Entity.InheritSource(projectile), (int)projectile.Center.X, (int)projectile.Center.Y, 521);
			}
			if (projectile.ai[0] == 110f) {
				if (!NPC.AnyNPCs(454))
					projectile.ai[1] = NPC.NewNPC(Entity.InheritSource(projectile), (int)projectile.Center.X, (int)projectile.Center.Y, 454);
				else
					projectile.ai[1] = NPC.NewNPC(Entity.InheritSource(projectile), (int)projectile.Center.X, (int)projectile.Center.Y, 521);
			}
		}
		if (projectile.ai[0] == 120f) {
			projectile.Kill();
		}
	}
}
