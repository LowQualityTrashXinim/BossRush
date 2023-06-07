using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Common
{
    internal class GlobalProjectileMod : GlobalProjectile
    {
        public override void SetDefaults(Projectile entity)
        {
            if (!ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode)
            {
                return;
            }
            base.SetDefaults(entity);
        }
        public override void AI(Projectile projectile)
        {
            if (!ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode)
            {
                return;
            }
        }
        public override void PostAI(Projectile projectile)
        {
            base.PostAI(projectile);
            if (!ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode)
            {
                return;
            }
            if (projectile.type == ProjectileID.CultistRitual)
            {
                CultistRitualAI(projectile);
                return;
            }
        }
        private void CultistRitualAI(Projectile projectile)
        {
            if (projectile.ai[1] != -1f && Main.netMode != 1)
            {
                if (projectile.ai[0] == 100f)
                {
                    if (!NPC.AnyNPCs(454))
                        projectile.ai[1] = NPC.NewNPC(NPC.InheritSource(projectile), (int)projectile.Center.X, (int)projectile.Center.Y, 454);
                    else
                        projectile.ai[1] = NPC.NewNPC(NPC.InheritSource(projectile), (int)projectile.Center.X, (int)projectile.Center.Y, 521);
                }
                if (projectile.ai[0] == 110f)
                {
                    if (!NPC.AnyNPCs(454))
                        projectile.ai[1] = NPC.NewNPC(NPC.InheritSource(projectile), (int)projectile.Center.X, (int)projectile.Center.Y, 454);
                    else
                        projectile.ai[1] = NPC.NewNPC(NPC.InheritSource(projectile), (int)projectile.Center.X, (int)projectile.Center.Y, 521);
                }
            }
            if (projectile.ai[0] == 120f)
            {
                projectile.Kill();
            }
        }
    }
}
