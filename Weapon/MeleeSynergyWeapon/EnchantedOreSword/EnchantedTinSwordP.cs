using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;


namespace BossRush.Weapon.MeleeSynergyWeapon.EnchantedOreSword
{
    internal class EnchantedTinSwordP : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.TinShortsword;
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.alpha = 0;
            Projectile.light = 0.45f;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Projectile.alpha += 5;
            if (Projectile.alpha >= 235)
            {
                Projectile.Kill();
            }
        }
    }
}
