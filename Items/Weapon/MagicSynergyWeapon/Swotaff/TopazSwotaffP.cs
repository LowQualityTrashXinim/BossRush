using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace BossRush.Items.Weapon.MagicSynergyWeapon.Swotaff
{
    public class TopazSwotaffP : ModProjectile
    {
        public override string Texture => "BossRush/Weapon/MagicSynergyWeapon/Swotaff/TopazSwotaff";
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 58;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.DamageType = DamageClass.Magic;
        }
        int count = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            EntitySource_ItemUse source = new EntitySource_ItemUse(player, new Item(ModContent.ItemType<TopazSwotaff>()));
            Projectile.rotation += 0.25f;
            float offSetRotate = Projectile.rotation - MathHelper.PiOver4;
            if (Projectile.velocity.X < 0)
            {
                Projectile.NewProjectile(source, Projectile.Center + offSetRotate.ToRotationVector2() * 30, Projectile.rotation.ToRotationVector2() * 9, ProjectileID.TopazBolt, (int)(Projectile.damage * 0.5f), Projectile.knockBack * 0.5f, Projectile.owner);
            }
            if (Projectile.velocity.X > 0)
            {
                Projectile.NewProjectile(source, Projectile.Center + offSetRotate.ToRotationVector2() * 30, Projectile.rotation.ToRotationVector2() * 9, ProjectileID.TopazBolt, (int)(Projectile.damage * 0.5f), Projectile.knockBack * 0.5f, Projectile.owner);
            }

            if (Projectile.timeLeft < 10)
            {
                Vector2 GoBack = player.Center - Projectile.position;
                Vector2 SafeGoBack = GoBack.SafeNormalize(Vector2.UnitY);

                if (count >= 30)
                {
                    Projectile.velocity = SafeGoBack * 20f;
                }
                Projectile.timeLeft = 8;
                Projectile.velocity += SafeGoBack * 2f;

                float distance = 60;
                Vector2 newMove = player.Center - Projectile.Center;
                float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                if (distanceTo < distance)
                {
                    Projectile.Kill();
                }
                count++;
            }

            if (Projectile.velocity.X > 20)
            {
                Projectile.velocity.X = 20;
            }
            else if (Projectile.velocity.X < -20)
            {
                Projectile.velocity.X = -20;
            }
            if (Projectile.velocity.Y > 20)
            {
                Projectile.velocity.Y = 20;
            }
            else if (Projectile.velocity.Y < -20)
            {
                Projectile.velocity.Y = -20;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 6;
        }
    }
}
