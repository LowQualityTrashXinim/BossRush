using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.YinYang
{
    internal class YinLight : BaseBoltProjectile
    {
        public override int Offset() => 0;
    }

    internal class YangDark : BaseBoltProjectile
    {
        public override int Offset() => 180;
    }
}
