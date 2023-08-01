using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Projectiles
{
    internal class TinOreProjectile : ModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.TinOre);
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[1] > 0)
                return false;
            return base.CanHitNPC(target);
        }
        public override void AI()
        {
            Projectile.ai[1] = BossRushUtils.CoolDown(Projectile.ai[1]);
            Projectile.rotation += MathHelper.ToRadians(20) * (Projectile.velocity.X > 0 ? 1 : -1);
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 20)
            {
                if (Projectile.velocity.Y < 30)
                    Projectile.velocity.Y++;
            }
            base.AI();
        }
    }
    class TinBarProjectile : ModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.TinBar);
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            Projectile.rotation += MathHelper.ToRadians(20) * (Projectile.velocity.X > 0 ? 1 : -1);
            if (Projectile.ai[0] > 20)
            {
                if (Projectile.velocity.Y < 30)
                    Projectile.velocity.Y += 1.5f;
            }
            base.AI();
        }
        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
            for (int i = 0; i < 4; i++)
            {
                Vector2 vel = -Vector2.UnitY.Vector2DistributeEvenly(4, 60, i) * 5f;
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vel, ModContent.ProjectileType<TinOreProjectile>(), (int)(Projectile.damage * .5f), Projectile.knockBack, Projectile.owner, 0, 20);
            }
        }
    }
}