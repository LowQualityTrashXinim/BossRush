using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Weapon.MeleeSynergyWeapon.SuperShortSword
{
    class SpeTin : ModProjectile
    {
        static int Counter = 0;
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
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.damage = (int)(player.GetWeaponDamage(player.HeldItem) * 0.25f * player.GetTotalDamage(DamageClass.Melee).Additive);
            Projectile.CritChance = (int)(player.GetCritChance(DamageClass.Melee) + player.GetCritChance(DamageClass.Generic));
            if (player.direction == 1)
            {
                Counter++;
            }
            else
            {
                Counter--;
            }
            if (Counter == MathHelper.TwoPi * 100) { Counter = 0; }
            if (Counter == -MathHelper.TwoPi * 100) { Counter = 0; }
            if (player.dead || !player.active || !player.HasBuff(ModContent.BuffType<SuperShortSwordPower>()))
            {
                Projectile.Kill();
            }
            Vector2 DegreeToAim = Main.MouseWorld - Projectile.position;
            Vector2 SafeDegree = DegreeToAim.SafeNormalize(Vector2.UnitX);
            float SaferRotate = SafeDegree.ToRotation();

            Projectile.rotation = SaferRotate + MathHelper.PiOver4;
            Vector2 Rotate = new Vector2(1, 1).RotatedBy(MathHelper.ToRadians(270));
            Projectile.Center = player.Center + Rotate.RotatedBy(Counter * 0.1f) * 150;
            if (Counter == MathHelper.TwoPi * 100) { Counter = 0; }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 5;
        }
    }
}
