using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Weapon.RangeSynergyWeapon.DeathBySpark
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

        public override void AI()
        {
            Projectile.ai[0]++;
            if (!Collision.SolidCollision(Projectile.Center, Projectile.width, Projectile.height))
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                if(Projectile.ai[0] >= 40)
                {
                    if (Projectile.velocity.Y < 16) Projectile.velocity.Y += 0.1f;
                }
            }
            Player player = Main.player[Projectile.owner];
            EntitySource_ItemUse entity = new EntitySource_ItemUse(player, new Item(ModContent.ItemType<DeathBySpark>()));
            int rand = 1 + Main.rand.Next(2);
            for (int i = 0; i < rand; i++)
            {
                Vector2 newPos = new Vector2(Projectile.position.X + 11, Projectile.position.Y - 4);
                Vector2 OppositeVelocity = new Vector2(Projectile.velocity.X + Main.rand.Next(-5, 5), Projectile.velocity.Y + Main.rand.Next(-5, 5)).RotatedBy(MathHelper.ToRadians(Projectile.rotation + 180));
                Projectile.NewProjectile(entity, newPos, OppositeVelocity * 0.5f, ProjectileID.WandOfSparkingSpark, (int)(Projectile.damage * 0.45f), Projectile.owner, player.whoAmI);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;
            }
            return false;
        }
    }
}
