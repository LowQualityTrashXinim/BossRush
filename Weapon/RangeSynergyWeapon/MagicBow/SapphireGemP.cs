using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Weapon.RangeSynergyWeapon.MagicBow
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
        int seperateCount = 0;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 3;
        }
        public override void AI()
        {
            seperateCount++;
            if (seperateCount >= 5)
            {
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Terraria.ID.DustID.GemSapphire, Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f), Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f), 0, default, Main.rand.NextFloat(1f, 1.5f));
                Main.dust[dustnumber].noGravity = true;
                seperateCount = 0;
            }

            Player player = Main.player[Projectile.owner];
            if(player.dead || !player.active)
            {
                Projectile.Kill();
            }
            SearchForTargets(out bool fountTarget, out float distance, out Vector2 Target);
            count++;
            if (count < 30)
            {
                Projectile.velocity -= Projectile.velocity * 0.06f;
            }
            if (count >= 30 && setAi != 1)
            {
                Projectile.velocity += (player.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 3f;
                if (Vector2.Distance(Projectile.Center, player.Center) <= 100)
                {
                    Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 10f;
                    if (Vector2.Distance(Projectile.Center, player.Center) <= 20)
                    {
                        setAi = 1;
                        Projectile.netUpdate = true;
                        if (Main.rand.NextBool(5))
                        {
                            player.Heal(5);
                            int manaheal = 10;
                            player.statMana += manaheal;
                            if (player.statMana > player.statManaMax2) player.statMana = player.statManaMax2;
                            player.ManaEffect(manaheal);
                            Projectile.Kill();
                        }
                    }
                }
            }
            if (setAi == 1 && distance >= 20f && fountTarget)
            {
                Projectile.velocity += (Target - Projectile.Center).SafeNormalize(Vector2.UnitX) * 3f;
                Projectile.penetrate = 1;
                if (count % 70 == 0)
                {
                    Projectile.velocity = (Target - Projectile.Center).SafeNormalize(Vector2.UnitX) * 5f;
                }
            }
            else if (setAi == 1 && !fountTarget)
            {
                Projectile.velocity += (player.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 1.5f;
            }
            if (Projectile.velocity.X > 15)
            {
                Projectile.velocity.X = 15;
            }
            else if (Projectile.velocity.X < -15)
            {
                Projectile.velocity.X = -15;
            }
            if (Projectile.velocity.Y > 15)
            {
                Projectile.velocity.Y = 15;
            }
            else if (Projectile.velocity.Y < -15)
            {
                Projectile.velocity.Y = -15;
            }
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
                    if (npc.CanBeChasedBy())
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
