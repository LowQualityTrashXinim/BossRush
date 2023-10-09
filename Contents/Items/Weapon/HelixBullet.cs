using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace BossRush.Contents.Items.Weapon
{
    class HelixBullet : ModProjectile
    {
        int count = 0;
        float rotate = -2.5f;
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.alpha = 0;
            Projectile.light = 0.7f;
            Projectile.timeLeft = 900;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;
            if (count == 2)
            {
                rotate = -5f;
            }
            if (count == 1)
            {
                rotate = 5f;
            }
            if (Projectile.ai[0] == 10)
            {
                Projectile.ai[0] = 0;
                count++;
            }
            if (count > 2)
            {
                count = 1;
            }
            Vector2 Helix = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy((double)MathHelper.ToRadians(rotate));
            Projectile.velocity = Helix;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[ModContent.ProjectileType<HelixFade>()].Value;

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
