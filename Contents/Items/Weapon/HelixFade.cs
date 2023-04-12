using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace BossRush.Contents.Items.Weapon
{
    class HelixFade : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.alpha = 100;
            Projectile.light = 0.7f;
            Projectile.timeLeft = 20;
        }

        public override void AI()
        {
            Projectile.scale -= 0.05f;
            if (Projectile.scale <= 0)
            {
                Projectile.scale = 0;
            }
        }
    }
}
