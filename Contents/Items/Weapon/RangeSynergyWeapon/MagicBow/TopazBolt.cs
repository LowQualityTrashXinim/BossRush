using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class TopazBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 100;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Projectile.light = 1f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            int amountrand = Main.rand.Next(1, 3);
            for (int i = 0; i < amountrand; i++)
            {
                Vector2 RandomCircular = Main.rand.NextVector2Circular(10f, 10f);
                Vector2 newVelocity = new Vector2(RandomCircular.X, -10 + RandomCircular.Y);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, newVelocity, ModContent.ProjectileType<TopazGemP>(), Projectile.damage, 0, Projectile.owner);
            }
            return true;
        }
        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemTopaz, Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f), Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f), 0, default, Main.rand.NextFloat(1f, 1.5f));
                Main.dust[dustnumber].noGravity = true;
            }
            if (Projectile.timeLeft <= 2)
            {
                Projectile.timeLeft = 2;
                if (Projectile.velocity.Y < 10) Projectile.velocity.Y += 0.0167f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage -= 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrail(lightColor, .02f);
            return true;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 45; i++)
            {
                Vector2 RandomCircular = Main.rand.NextVector2Circular(4.5f, 4.5f);
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemTopaz, RandomCircular.X, RandomCircular.Y, 0, default, Main.rand.NextFloat(1.25f, 2.25f));
                Main.dust[dustnumber].noGravity = true;
            }
        }
    }
}
