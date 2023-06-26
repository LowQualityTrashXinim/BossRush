using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon
{
    internal class SwotaffGemProjectile : SynergyModProjectile
    {
        public virtual void PreSetDefault() { }
        public override void SetDefaults()
        {
            PreSetDefault();
            Projectile.friendly = true;
            base.SetDefaults();
        }
    }
}
