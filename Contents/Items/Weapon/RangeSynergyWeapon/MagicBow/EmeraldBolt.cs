using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class EmeraldBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 100;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Projectile.light = 1f;
        }

        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemEmerald, Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f), Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f), 0, default, Main.rand.NextFloat(1f, 1.5f));
                Main.dust[dustnumber].noGravity = true;
            }
            if (Projectile.timeLeft <= 2)
            {
                Projectile.timeLeft = 2;
                if (Projectile.velocity.Y < 10) Projectile.velocity.Y += 0.0167f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            float ToRandom = Main.rand.Next(90);
            for (int i = 0; i < 3; i++)
            {
                Vector2 Rotate = Vector2.One.RotatedBy(MathHelper.ToRadians(120 * i + ToRandom)) * 20f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Rotate, ModContent.ProjectileType<EmeraldGemP>(), Projectile.damage, 0, Projectile.owner);
            }
            target.immune[Projectile.owner] = 3;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrail(lightColor);
            return true;
        }
    }
}
