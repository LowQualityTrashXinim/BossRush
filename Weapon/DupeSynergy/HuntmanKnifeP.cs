using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Weapon.DupeSynergy
{
	public class HuntmanKnifeP : ModProjectile
	{
        public override string Texture => "BossRush/Weapon/DupeSynergy/HuntmanKnife";

        public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.penetrate = 1;
			Projectile.alpha = 0;

			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.timeLeft = 250;
			Projectile.scale -= 0.35f;
		}

        public override void AI()
        {
			Projectile.rotation = Projectile.velocity.ToRotation()+MathHelper.PiOver2;
        }
    }
}
