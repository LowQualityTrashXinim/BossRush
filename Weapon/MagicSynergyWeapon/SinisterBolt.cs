using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Weapon.MagicSynergyWeapon
{
    internal class SinisterBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.light = 0.8f;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        }
        Vector2 MousePosFixed;
        int count = 0;
        int CountMain = 0;
        public override void AI()
        {
            Vector2 RandomFollowVel = Main.rand.NextVector2CircularEdge(1f, 1f) + Projectile.velocity;
            int dustnumber = Dust.NewDust(Projectile.Center, Projectile.width/2, Projectile.height/2, DustID.GemDiamond, RandomFollowVel.X, RandomFollowVel.Y, 0, default, Main.rand.NextFloat(0.75f, 1f));
            Main.dust[dustnumber].noGravity = true;
            if (CountMain == 0)
            {
                MousePosFixed = Main.MouseWorld;
            }
            CountMain++;
            if (CountMain >= 60)
            {
                if ((Projectile.velocity.X > 1 || Projectile.velocity.X < -1 || Projectile.velocity.Y > 1 || Projectile.velocity.Y < -1 )&& count == 0)
                {
                    Projectile.velocity -= Projectile.velocity * 0.01f;
                }
                else
                {
                    count++;
                }
                if (count >= 150)
                {
                    Projectile.velocity = (MousePosFixed - Projectile.Center).SafeNormalize(Vector2.UnitX) * (1 + CountMain * 0.02f);
                    if (Vector2.Distance(MousePosFixed, Projectile.Center) <= 10)
                    {
                        Projectile.Kill();
                    }
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector2 Rotate = Main.rand.NextVector2Circular(10f, 10f);
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, Rotate.X, Rotate.Y, 0, default, Main.rand.NextFloat(0.75f, 1f));
                Main.dust[dustnumber].noGravity = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(Projectile.gfxOffY);
                Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale - k*0.05f, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}
