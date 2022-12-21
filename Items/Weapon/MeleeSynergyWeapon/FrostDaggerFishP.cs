using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon
{
    class FrostDaggerFishP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Generic;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.timeLeft = 500;
            Projectile.penetrate = -1;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Frostburn, 210);
        }
        int count = 19;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.timeLeft == 499)
            {
                for (int i = 0; i < 50; i++)
                {
                    Vector2 Circle = Main.rand.NextVector2CircularEdge(7f, 7f);
                    int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.IceRod, Circle.X, Circle.Y, 0, Color.Aqua, 1.5f);
                    Main.dust[dust].noGravity = true;
                }
            }
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] <= 30)
            {
                Projectile.rotation += MathHelper.ToRadians(495 / 30);
                return;
            }
            Projectile.netUpdate = true;

            for (int i = 0; i < 2; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 
                    (int)(Projectile.width * 0.5f), 
                    (int)(Projectile.height * 0.5f), 
                    DustID.IceTorch, 
                    Main.rand.Next(-5, 5) + Projectile.velocity.X * -0.25f, 
                    Main.rand.Next(-5, 5) + Projectile.velocity.Y * -0.25f, 
                    0, default, Main.rand.NextFloat(0.5f, 1.2f));
                Main.dust[dust].noGravity = true;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            NPC closestNPC = FindClosestNPC(900);
            if (closestNPC == null)
            {
                Projectile.velocity.Y += 0.3f;
                return;
            }
            Projectile.penetrate = 1;
            Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 7.5f;
            if (count >= 30)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), 
                    Projectile.Center, 
                    Projectile.velocity * 2.25f, 
                    ProjectileID.IceBolt, 
                    (int)(Projectile.damage * 0.75f), 
                    Projectile.knockBack * 0.65f, 
                    player.whoAmI);
                count = 0;
            }
            count++;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 Circle = Main.rand.NextVector2CircularEdge(5f, 5f);
                Dust.NewDust(Projectile.Center, (int)(Projectile.width * 0.5f), (int)(Projectile.height * 0.5f), DustID.IceRod, Circle.X, Circle.Y, 0, Color.Aqua, Main.rand.NextFloat(1, 1.25f));
            }
        }

        public NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                if (target.CanBeChasedBy())
                {
                    // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

                    // Check if it is within the radius
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }

                }
            }
            return closestNPC;
        }
    }
}
