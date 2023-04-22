using Terraria;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.YinYang
{
    internal class YinLight : BaseBoltProjectile
    {
        Player player;
        int Counter = 0;
        int firstframe = 0;
        const float FullCircle = MathHelper.TwoPi * 100;
        public override bool PreAI()
        {
            if (firstframe == 0)
            {
                player = Main.player[Projectile.owner];
                firstframe++;
            }
            return true;
        }
        public override void AI()
        {
            if (player.dead || !player.active)
            {
                Projectile.Kill();
            }
            if (Counter >= FullCircle)
            {
                Counter = 0;
            }
            Counter++;
            Projectile.Center = getPosToReturn(0, Counter);
        }
    }

    internal class YangDark : BaseBoltProjectile
    {
        Player player;
        int Counter = 0;
        int firstframe = 0;
        const float FullCircle = MathHelper.TwoPi * 100;
        public override bool PreAI()
        {
            if (firstframe == 0)
            {
                player = Main.player[Projectile.owner];
                firstframe++;
            }
            return true;
        }
        public override void AI()
        {
            if (player.dead || !player.active)
            {
                Projectile.Kill();
            }
            if (Counter >= FullCircle)
            {
                Counter = 0;
            }
            Counter++;
            Projectile.Center = getPosToReturn(180, Counter);
        }
    }
}
