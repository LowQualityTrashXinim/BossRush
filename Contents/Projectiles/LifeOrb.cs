using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Projectiles
{
    internal class LifeOrb : ModProjectile
    {
        public override string Texture => BossRushTexture.WHITEBALL;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.timeLeft = 999;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.damage = 0;
        }
        public override bool? CanDamage() => false;
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 255, 0);
        }
        Player player;
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemEmerald);
            Main.dust[dust].velocity = Main.rand.NextVector2Circular(3, 3);
            Main.dust[dust].noGravity = true;
            if (Projectile.timeLeft == 999)
                player = Main.player[Projectile.owner];
            if (player is not null & Projectile.Center.IsCloseToPosition(player.Center, 10))
            {
                player.Heal(10);
                Projectile.Kill();
            }
        }
    }
}
