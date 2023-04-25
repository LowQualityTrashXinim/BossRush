using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System.Collections.Generic;
using System.Linq;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class DiamondBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            Projectile.light = 1f;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<DiamondGemP>()] < 25)
            {
                Vector2 Rotate = Main.rand.NextVector2CircularEdge(15, 15);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Rotate, ModContent.ProjectileType<DiamondGemP>(), 0, 0, Projectile.owner);
            }
        }
        public override void AI()
        {
            for (int i = 0; i < 1; i++)
            {
                int dustnumber = Dust.NewDust(Projectile.position, 0, 0, DustID.GemDiamond, Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f), Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f));
                Main.dust[dustnumber].noGravity = true;
                Main.dust[dustnumber].fadeIn = 1f;
            }
            if (RicochetOff(out Vector2 pos2))
            {
                Projectile.netUpdate = true;
                Projectile.damage += 3;
                Projectile.velocity = (pos2 - Projectile.position).SafeNormalize(Vector2.UnitX) * 10;
                for (int i = 0; i < 15; i++)
                {
                    Vector2 ReverseVelSpread = -Projectile.velocity * 2 + Main.rand.NextVector2Circular(5f, 5f);
                    int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, ReverseVelSpread.X, ReverseVelSpread.Y, 0, default, Main.rand.NextFloat(1f, 1.5f));
                    Main.dust[dustnumber].noGravity = true;
                }
            }
        }
        private readonly float Distance = 2250000;
        public bool CheckActiveAndCon(Projectile projectileThatNeedtoCheck)
        {
            Player player = Main.player[Projectile.owner];
            if (projectileThatNeedtoCheck.ModProjectile is DiamondGemP && projectileThatNeedtoCheck.active && !projectileThatNeedtoCheck.velocity.IsLimitReached(2))
            {
                if (Vector2.DistanceSquared(player.Center, projectileThatNeedtoCheck.Center) < Distance)
                {
                    return true;
                }
            }
            return false;
        }
        public List<Vector2> GetListOfActiveProj(out bool Check)
        {
            List<Vector2> list = new List<Vector2>();
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (CheckActiveAndCon(Main.projectile[i]))
                {
                    list.Add(Main.projectile[i].Center);
                }
            }
            Check = list.Count > 1;
            return list;
        }
        public bool RicochetOff(out Vector2 Pos2)
        {
            List<Vector2> list = GetListOfActiveProj(out bool Check);
            if (Check)
            {
                Vector2 Pos1;
                foreach (Vector2 pos in list.Where(x => Vector2.Distance(Projectile.Center, x) <= 20))
                {
                    Pos1 = pos;
                    do
                    {
                        Pos2 = Main.rand.Next(list);
                    }
                    while (Pos2 == Pos1);
                    return true;
                }
            }
            Pos2 = Vector2.Zero;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrail(lightColor, 0.01f);
            return true;
        }
    }
}