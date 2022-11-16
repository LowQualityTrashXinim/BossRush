using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.HeartPistol
{
    class HeartP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.alpha = 0;
            Projectile.light = 0.65f;
            Projectile.timeLeft = 90;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            // prevent heal from applying when damaging critters or target dummy
            Player player = Main.player[Projectile.owner];
            if (target.lifeMax > 5 && !target.friendly && target.type != NPCID.TargetDummy)
            {
                int healAmount = Main.rand.Next(1, 5);

                player.statLife += healAmount;
                // this part here prevents health from going above max
                if (player.statLife > player.statLifeMax2)
                {
                    player.statLife = player.statLifeMax2;
                }

                // the heal popup text
                player.HealEffect(healAmount, true);
            }
        }

        public override void Kill(int timeLeft)
        {
            Projectile.position += new Vector2(11, 11);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, -5, 0, ModContent.ProjectileType<smallerHeart>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, -5, -2.5f, ModContent.ProjectileType<smallerHeart>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, -2.5f, -5, ModContent.ProjectileType<smallerHeart>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, -2.5f, 2.5f, ModContent.ProjectileType<smallerHeart>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 0, 5, ModContent.ProjectileType<smallerHeart>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 0, -2.5f, ModContent.ProjectileType<smallerHeart>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 5, 0, ModContent.ProjectileType<smallerHeart>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 5, -2.5f, ModContent.ProjectileType<smallerHeart>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 2.5f, -5f, ModContent.ProjectileType<smallerHeart>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, 2.5f, 2.5f, ModContent.ProjectileType<smallerHeart>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner);

        }
    }
}
