using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.Bowmarang
{
    public class BowmarangP : ModProjectile
    {
        public override string Texture => "BossRush/Items/Weapon/RangeSynergyWeapon/Bowmarang/Bowmarang";
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.DamageType = DamageClass.Melee;
        }
        int count = 0;
        int count2 = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            EntitySource_ItemUse source = new EntitySource_ItemUse(player, new Item(ModContent.ItemType<Bowmarang>()));
            Projectile.rotation += 0.4f;
            float offSetRotate = Projectile.rotation - MathHelper.PiOver4;

            Vector2 aimto = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.UnitX);
            if (count2 % 10 == 0)
            {
                if (Projectile.velocity != Vector2.Zero)
                {
                    Projectile.NewProjectile(source, Projectile.Center + offSetRotate.ToRotationVector2(), aimto * 9, ProjectileID.WoodenArrowFriendly, Projectile.damage, Projectile.knockBack * 0.5f, Projectile.owner);
                }
                if (count2 == 30)
                {
                    float rotation = MathHelper.ToRadians(180);
                    for (int i = 0; i < 10; i++)
                    {
                        Vector2 rotate = Vector2.One.RotatedBy(MathHelper.Lerp(rotation, -rotation, i / 10f));
                        int FieryArrow = Main.rand.Next(new int[] { ProjectileID.FrostburnArrow, ProjectileID.FireArrow });
                        Projectile.NewProjectile(source, Projectile.Center, rotate * 4, FieryArrow, Projectile.damage, Projectile.knockBack * 0.5f, Projectile.owner);
                    }
                    count2 = 0;
                }
            }
            count2++;

            if (Projectile.timeLeft < 3)
            {
                Vector2 GoBack = player.Center - Projectile.position;
                Vector2 SafeGoBack = GoBack.SafeNormalize(Vector2.UnitY);
                if (count >= 30)
                {
                    Projectile.velocity = SafeGoBack * 20f;
                }
                Projectile.timeLeft = 2;
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
