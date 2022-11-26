using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace BossRush.Items.Weapon.MagicSynergyWeapon
{
    internal class DiamondBoltSpecial : ModProjectile
    {
        public override string Texture => "BossRush/Items/Weapon/MagicSynergyWeapon/DiamondOrb";
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.timeLeft = 70;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
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
    }
}
