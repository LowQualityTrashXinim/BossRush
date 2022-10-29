using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Weapon.MeleeSynergyWeapon.EnchantedOreSword
{
    internal class EnchantedLeadSwordP : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.LeadShortsword;
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.alpha = 0;
            Projectile.light = 0.55f;
        }
        public int count = 0;
        public override void AI()
        {
            count++;
            if (count >= 40)
            {
                Projectile.velocity = Main.rand.NextVector2CircularEdge(10f, 10f);
                count = 0;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Projectile.alpha += 2;
            if (Projectile.alpha >= 235)
            {
                Projectile.Kill();
            }
        }
    }
}
