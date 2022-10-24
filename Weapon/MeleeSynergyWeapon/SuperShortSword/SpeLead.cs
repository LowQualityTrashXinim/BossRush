using Terraria;
using Microsoft.Xna.Framework;
namespace BossRush.Weapon.MeleeSynergyWeapon.SuperShortSword
{
    class SpeLead : SpecialSwordTemplateProjectile
    {
        int Counter = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Counter == MathHelper.TwoPi * 100 || Counter == -MathHelper.TwoPi * 100) { Counter = 0; }
            if (player.direction == 1)
            {
                Counter++;
            }
            else
            {
                Counter--;
            }
            Behavior(player, 180, Counter);
        }
    }
}