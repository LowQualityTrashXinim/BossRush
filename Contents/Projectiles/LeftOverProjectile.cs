using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Projectiles
{
    internal class DiamondSwotaffOrb : ModProjectile
    {
        public override string Texture => BossRushTexture.DIAMONDSWOTAFFORB;
        float rotateto = 0;
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 400;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.velocity != Vector2.Zero)
            {
                Projectile.timeLeft--;
            }
            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.velocity -= Projectile.velocity * 0.05f;
            }
            if (Projectile.velocity.X < 1 && Projectile.velocity.X > -1 && Projectile.velocity.Y < 1 && Projectile.velocity.Y > -1)
            {
                for (int i = 0; i < 30; i++)
                {
                    Vector2 Rotate = Main.rand.NextVector2CircularEdge(5f, 5f);
                    int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, Rotate.X, Rotate.Y, 100, default, Main.rand.NextFloat(0.75f, 2f));
                    Main.dust[dustnumber].noGravity = true;
                }
                Projectile.velocity = Vector2.Zero;
                Projectile.ai[0]++;
                if (Projectile.ai[0] >= 30)
                {
                    Projectile.netUpdate = true;
                    for (int i = 0; i < 10; i++)
                    {
                        Vector2 RandomRotatePos = Projectile.Center + Main.rand.NextVector2CircularEdge(330f, 330f);
                        Vector2 velocityCustom = (Projectile.Center - RandomRotatePos).SafeNormalize(Vector2.UnitX) * 17;
                        int dustnumber = Dust.NewDust(RandomRotatePos, 0, 0, DustID.GemDiamond, velocityCustom.X, velocityCustom.Y, default, default, Main.rand.NextFloat(1.5f, 2.5f));
                        Main.dust[dustnumber].noGravity = true;
                    }
                    for (int i = 0; i < 25; i++)
                    {
                        Vector2 RandomRotatePos = Projectile.Center + Main.rand.NextVector2CircularEdge(330f, 330f);
                        int dustnumber = Dust.NewDust(RandomRotatePos, 0, 0, DustID.GemDiamond, 0, 0, default, default, Main.rand.NextFloat(1.5f, 2.5f));
                        Main.dust[dustnumber].noGravity = true;
                    }
                    Vector2 newPos = Projectile.Center + (Projectile.Center + Vector2.One).SafeNormalize(Vector2.UnitX) * 300;
                    Vector2 newVelo = (Projectile.Center - newPos.RotatedBy(MathHelper.ToRadians(rotateto), Projectile.Center)).SafeNormalize(Vector2.UnitX);
                    Vector2 newVelo2 = (Projectile.Center - newPos.RotatedBy(MathHelper.ToRadians(rotateto + 90), Projectile.Center)).SafeNormalize(Vector2.UnitX);
                    Vector2 newVelo3 = (Projectile.Center - newPos.RotatedBy(MathHelper.ToRadians(rotateto + 180), Projectile.Center)).SafeNormalize(Vector2.UnitX);
                    Vector2 newVelo4 = (Projectile.Center - newPos.RotatedBy(MathHelper.ToRadians(rotateto + 270), Projectile.Center)).SafeNormalize(Vector2.UnitX);
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), newPos.RotatedBy(MathHelper.ToRadians(rotateto), Projectile.Center), newVelo * 5, ModContent.ProjectileType<DiamondBoltSpecial>(), (int)(Projectile.damage * 0.35), 0, Projectile.owner);
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), newPos.RotatedBy(MathHelper.ToRadians(rotateto + 90), Projectile.Center), newVelo2 * 5, ModContent.ProjectileType<DiamondBoltSpecial>(), (int)(Projectile.damage * 0.35), 0, Projectile.owner);
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), newPos.RotatedBy(MathHelper.ToRadians(rotateto + 180), Projectile.Center), newVelo3 * 5, ModContent.ProjectileType<DiamondBoltSpecial>(), (int)(Projectile.damage * 0.35), 0, Projectile.owner);
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), newPos.RotatedBy(MathHelper.ToRadians(rotateto + 270), Projectile.Center), newVelo4 * 5, ModContent.ProjectileType<DiamondBoltSpecial>(), (int)(Projectile.damage * 0.35), 0, Projectile.owner);
                    rotateto += 5;
                }
            }
        }
    }
    internal class DiamondBoltSpecial : ModProjectile
    {
        public override string Texture => BossRushTexture.DIAMONDSWOTAFFORB;
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.timeLeft = 70;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.alpha = 255;
        }
        int count = 0;
        public override void AI()
        {
            if (Projectile.timeLeft > 55)
            {
                Projectile.alpha -= 10;
            }
            else
            {
                Projectile.alpha += 5;
                Projectile.scale -= 0.025f;
            }
            if (count >= 3)
            {
                for (int i = 0; i < 2; i++)
                {
                    int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, 0, 0, 100, default, Projectile.scale);
                    Main.dust[dustnumber].noGravity = true;
                }
                count = 0;
            }
            count++;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 1;
        }
    }
}
