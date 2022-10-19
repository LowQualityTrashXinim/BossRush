using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class DiamondGemP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 16;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 120;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            if (Projectile.velocity != Vector2.Zero) { Projectile.velocity -= Projectile.velocity * 0.05f; Projectile.timeLeft = 120; }
            if(Projectile.velocity.X < .1f && Projectile.velocity.X > -.1f && Projectile.velocity.Y < .1f && Projectile.velocity.Y > -.1f) Projectile.velocity = Vector2.Zero;
            if(Main.rand.NextBool(5))
            {
                Vector2 RandomSpread = Main.rand.NextVector2Circular(4f, 4f);
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, RandomSpread.X, RandomSpread.Y, 0, default, Main.rand.NextFloat(1f, 1.5f));
                Main.dust[dustnumber].noGravity = true;
            }
        }
    }
}
