using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon.SuperShortSword
{
    abstract class SpecialSwordTemplateProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 32;
            Projectile.width = 32;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.light = 0.7f;
        }
        public void Behavior(Player player, float offSet, int Counter, float Distance = 250)
        {
            Projectile.damage = (int)(player.GetWeaponDamage(player.HeldItem) * 0.25f * player.GetTotalDamage(DamageClass.Melee).Additive);
            Projectile.CritChance = (int)(player.GetCritChance(DamageClass.Melee) + player.GetCritChance(DamageClass.Generic));
            if (player.dead || !player.active || !player.HasBuff(ModContent.BuffType<SuperShortSwordPower>()))
            {
                Projectile.Kill();
            }
            Vector2 DegreeToAim = Main.MouseWorld - Projectile.position;
            Vector2 SafeDegree = DegreeToAim.SafeNormalize(Vector2.UnitX);
            float SaferRotate = SafeDegree.ToRotation();

            Projectile.rotation = SaferRotate + MathHelper.PiOver4;

            Vector2 Rotate = new Vector2(1, 1).RotatedBy(MathHelper.ToRadians(offSet));
            Projectile.Center = player.Center + Rotate.RotatedBy(Counter * 0.05f) * Distance;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 5;
        }
    }
}
