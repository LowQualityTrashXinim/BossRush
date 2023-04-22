using Terraria;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.YinYang
{
    internal class YinLight : BaseBoltProjectile
    {
        public override void AI()
        {
            if (Projectile.ai[0] != 1)
            {
                return;
            }
        }
    }

    internal class YangDark : BaseBoltProjectile
    {
        public override void AI()
        {
            base.AI();
        }
    }
}
