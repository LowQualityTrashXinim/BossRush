using BossRush.Texture;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace BossRush.Contents.Projectiles
{
    class ArtifactSwordSlashProjectile : ModProjectile
    {
        public override string Texture => BossRushTexture.SMALLWHITEBALL;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 50;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 15;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 50;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 10;
        }
        public override void AI()
        {
            Projectile.alpha = (int)MathHelper.Lerp(0, 255, (50 - Projectile.timeLeft) / 50);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrail(lightColor, .02f);
            return base.PreDraw(ref lightColor);
        }
    }
}
