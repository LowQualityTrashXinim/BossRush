using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class TopazGemP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 18;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.light = 1f;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }
        int bouncecount = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate = 1;

            if (bouncecount < 6)
            {
                Projectile.netUpdate = true;
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
                if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = (int)(-oldVelocity.X * 0.4f);
                if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = (int)(-oldVelocity.Y * 0.4f);
                bouncecount++;
            }
            else
            {
                if (Projectile.velocity.Y < 0.5f && Projectile.velocity.Y > -0.5 && Projectile.velocity.X < 1f && Projectile.velocity.X > -1f)
                {
                    Projectile.velocity = Vector2.Zero;
                }
            }
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X);

            if (Projectile.timeLeft % 5 == 0)
            {
                Projectile.damage += 1;
            }
            return false;
        }

        public override void AI()
        {
            if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            else
            {
                Projectile.timeLeft = 300;
            }
            if (Main.rand.NextBool(7))
            {
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemTopaz, Projectile.velocity.X + Main.rand.Next(-5, 5), Projectile.velocity.Y + Main.rand.Next(-5, 5), 0, default, Main.rand.NextFloat(0.75f, 1.25f));
                Main.dust[dustnumber].noGravity = true;
            }
            Projectile.velocity.X *= 0.98f;
            Projectile.velocity.Y += 0.5f;
        }
        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<GhostHitBox>(), (int)(Projectile.damage * 0.85f), 5f, Projectile.owner);
            for (int i = 0; i < 15; i++)
            {
                Vector2 RandomCircular = Main.rand.NextVector2Circular(10f, 10f);
                Vector2 newVelocity = new Vector2(RandomCircular.X, RandomCircular.Y);
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemTopaz, newVelocity.X, newVelocity.Y, 0, default, Main.rand.NextFloat(1.75f, 2.25f));
                Main.dust[dustnumber].noGravity = true;
            }
        }
    }
}
