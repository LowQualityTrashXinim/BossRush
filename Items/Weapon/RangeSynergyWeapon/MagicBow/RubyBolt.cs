using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class RubyBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Projectile.light = 1f;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            float ToRandom = Main.rand.Next(3);
            if (player.ownedProjectileCounts[ModContent.ProjectileType<RubyGemP>()] < 1)
            {
                for (int i = 0; i < 8; i++)
                {
                    Vector2 Rotate = Vector2.One.RotatedBy(MathHelper.ToRadians(45 * i + ToRandom * 30)) * 6f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Rotate, ModContent.ProjectileType<RubyGemP>(), Projectile.damage, 0, Projectile.owner);
                }
            }
        }

        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f), Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f), 0, default, Main.rand.NextFloat(1f, 1.5f));
                Main.dust[dustnumber].noGravity = true;
            }
            if (CheckNearByProjectile())
            {
                Projectile.damage += 3;
                Projectile.velocity *= 1.05f;
            }
        }

        public bool CheckNearByProjectile()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].type == ModContent.ProjectileType<RubyGemP>())
                {
                    float Distance = Vector2.Distance(Projectile.Center, Main.projectile[i].Center);
                    if (Distance <= 30)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 100; i++)
            {
                Vector2 RandomCir = Main.rand.NextVector2Circular(10f, 10f);
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, RandomCir.X, RandomCir.Y, 0, default, Main.rand.NextFloat(1f, 1.5f));
                Main.dust[dustnumber].noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale - k * 0.02f, SpriteEffects.None, 0);
            }

            return true;
        }
    }
}
