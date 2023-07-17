using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Projectiles
{
    class AcornRainCloud : ModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Acorn);
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 380;
            Projectile.hide = true;
        }
        public override void AI()
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2 positionAbove = Projectile.Center + Main.rand.NextVector2Circular(1200f, 100f) - new Vector2(0, 1000);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), positionAbove, Vector2.UnitY * 10, ModContent.ProjectileType<AcornProjectile>(), 10, 1f, Projectile.owner);
            }
        }
    }

    internal class AcornProjectile : ModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Acorn);
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 900;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4 + MathHelper.PiOver2;
            if (Projectile.ai[0] <= 30)
            {
                Projectile.ai[0]++;
                return;
            }
            if (Projectile.velocity.Y < 20)
                Projectile.velocity.Y += .1f;
        }
    }
}
