using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Weapon.MeleeSynergyWeapon.EnchantedOreSword
{
    internal class EnchantedGoldSwordP : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.GoldShortsword;
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.alpha = 0;
            Projectile.light = 0.75f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 4;
        }

        public override void AI()
        {
            Projectile.velocity += (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.UnitX);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Projectile.alpha += 2;
            if (Projectile.alpha >= 235)
            {
                Projectile.Kill();
            }
        }
    }
}
