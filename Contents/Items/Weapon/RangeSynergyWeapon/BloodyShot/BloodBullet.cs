using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.BloodyShot
{
    internal class BloodBullet : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            int ran = Main.rand.Next(7);
            if (ran == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 newPos = new Vector2(Projectile.position.X + Main.rand.Next(-500, 500) + 5, Projectile.position.Y - (600 + Main.rand.Next(1, 100)) + 5);
                    Vector2 safeAimto = (Projectile.position - newPos).SafeNormalize(Vector2.UnitX);
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), newPos, safeAimto * 5, ModContent.ProjectileType<BloodBullet>(), hit.Damage, hit.Knockback, player.whoAmI);
                }
            }
            int randNum = 1 + Main.rand.Next(3, 6);
            for (int i = 0; i < randNum; i++)
            {
                Vector2 newPos = new Vector2(Projectile.position.X + Main.rand.Next(-200, 200) + 5, Projectile.position.Y - (600 + Main.rand.Next(1, 200)) + 5);
                Projectile.position.X += Main.rand.Next(-50, 50);
                Vector2 safeAimto = (Projectile.position - newPos).SafeNormalize(Vector2.UnitX);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), newPos, safeAimto * 25, ProjectileID.BloodArrow, (int)(hit.Damage * 0.75f), hit.Knockback, player.whoAmI);
            }
            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Blood);
            Main.dust[dust].noGravity = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrail(lightColor);
            return true;
        }
    }
}
