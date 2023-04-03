using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class SapphireGemP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.light = 1f;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
        }
        int count = 0;
        int setAi = 0;
        float speedextra = .1f;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 3;
        }
        public override void AI()
        {
            if (Main.rand.NextBool(10))
            {
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Terraria.ID.DustID.GemSapphire, Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f), Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f), 0, default, Main.rand.NextFloat(1f, 1.5f));
                Main.dust[dustnumber].noGravity = true;
            }
            Player player = Main.player[Projectile.owner];
            if (player.dead || !player.active)
            {
                Projectile.Kill();
            }
            SearchForTargets(out bool fountTarget, out float distance, out Vector2 Target);
            count++;
            if (count < 30)
            {
                Projectile.velocity -= Projectile.velocity * 0.06f;
            }
            if (count >= 30)
            {
                if (setAi != 1)
                {
                    Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * speedextra;
                    if (speedextra <= 10f) speedextra += .1f;
                    if (player.statLife <= player.statLifeMax / 3)
                    {
                        Projectile.netUpdate = true;
                        if (Main.rand.NextBool())
                        {
                            player.Heal(1);
                        }
                        else
                        {
                        player.statMana += 10;
                        if (player.statMana > player.statManaMax2) player.statMana = player.statManaMax2;
                        player.ManaEffect(10);
                        }
                        Projectile.Kill();
                    }
                    else
                    {
                        setAi = 1;
                        Projectile.netUpdate = true;
                    }
                }
                else
                {
                    if (distance >= 20f && fountTarget)
                    {
                        Projectile.velocity += (Target - Projectile.Center).SafeNormalize(Vector2.UnitX) * 3f;
                        Projectile.penetrate = 1;
                        if (count % 70 == 0)
                        {
                            Projectile.velocity = (Target - Projectile.Center).SafeNormalize(Vector2.UnitX) * 5f;
                        }
                    }
                    else if (!fountTarget)
                    {
                        Projectile.velocity += (player.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 1.5f;
                    }
                }
            }
            Projectile.velocity = Projectile.velocity.LimitedVelocity(15);
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 25; i++)
            {
                Vector2 RandomCircular = Main.rand.NextVector2Circular(10f, 10f);
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Terraria.ID.DustID.GemSapphire, RandomCircular.X, RandomCircular.Y, 0, default, Main.rand.NextFloat(1.5f, 2.25f));
                Main.dust[dustnumber].noGravity = true;
            }
        }
        private void SearchForTargets(out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter)
        {
            // Starting search distance
            distanceFromTarget = 2000f;
            targetCenter = Projectile.Center;
            foundTarget = false;

            if (!foundTarget)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy() && npc.active)
                    {
                        float between = Vector2.Distance(npc.Center, Projectile.Center);
                        bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;

                        if (closest && inRange || !foundTarget)
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }
        }
    }
}
