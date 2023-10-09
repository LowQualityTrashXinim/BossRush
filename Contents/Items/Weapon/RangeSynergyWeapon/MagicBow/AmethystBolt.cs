using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class AmethystBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            Projectile.light = 1f;
        }

        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemAmethyst, Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f), Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f), 0, default, Main.rand.NextFloat(1f, 1.5f));
                Main.dust[dustnumber].noGravity = true;
            }
            if (Projectile.timeLeft <= 2)
            {
                Projectile.timeLeft = 2;
                if (Projectile.velocity.Y < 10) Projectile.velocity.Y += 0.0167f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Player player = Main.player[Projectile.owner];
            float speedMultipler = 1;
            if (player.ZoneOverworldHeight)
            {
                speedMultipler += 3.5f;
            }
            int damage = (int)(Projectile.damage * 1.25f);
            for (int i = 0; i < 5; i++)
            {
                Vector2 RandomCircular = Main.rand.NextVector2Circular(5 + speedMultipler, 5 + speedMultipler);
                Vector2 TemporaryVector = RandomCircular - oldVelocity * speedMultipler;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, TemporaryVector, ModContent.ProjectileType<AmethystGemP>(), damage, 0, Projectile.owner);
            }
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage -= 1;
            target.immune[Projectile.owner] = 3;
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            float speedMultipler = 1;
            if (player.ZoneOverworldHeight)
            {
                speedMultipler += 3.5f;
            }
            for (int i = 0; i < 75; i++)
            {
                Vector2 RandomCircular = Main.rand.NextVector2Circular(5 + speedMultipler, 5 + speedMultipler);
                Vector2 TemporaryVector = RandomCircular + -Projectile.oldVelocity * speedMultipler;
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemAmethyst, TemporaryVector.X, TemporaryVector.Y, 0, default, Main.rand.NextFloat(1.25f, 2.25f));
                Main.dust[dustnumber].noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrail(lightColor);
            return true;
        }
    }
}