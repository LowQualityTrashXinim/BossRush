using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.DeathBySpark
{
    internal class SparkFlare : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.light = 2f;
        }
        bool hittile = false;
        public override void AI()
        {
            Projectile.ai[0]++;
            if (!hittile)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                if (Projectile.ai[0] >= 40)
                {
                    if (Projectile.velocity.Y < 16) Projectile.velocity.Y += 0.1f;
                }
            }
            Player player = Main.player[Projectile.owner];
            Vector2 OppositeVelocity = Projectile.rotation.ToRotationVector2() * -4.5f;
            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, OppositeVelocity + Main.rand.NextVector2Circular(1f, 1f), ProjectileID.WandOfSparkingSpark, (int)(Projectile.damage * 0.45f), Projectile.owner, player.whoAmI);
            Main.projectile[proj].usesLocalNPCImmunity = true;
            Main.projectile[proj].localNPCHitCooldown = 20;
            Main.projectile[proj].timeLeft = 10;

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!hittile)
            {
                Projectile.position += Projectile.velocity;
            }
            hittile = true;
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            Projectile.velocity.X = 0;
            Projectile.velocity.Y = 0;
            return false;
        }
    }
}
