using Mono.Cecil;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon
{
    internal class GhostHitBox : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2;
            Projectile.alpha = 255;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 1;
        }
    }
    internal class GhostHitBox2 : ModProjectile
    {
        public override string Texture => "BossRush/Items/Weapon/GhostHitBox";
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 1;
            Projectile.alpha = 255;
        }
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }
    }
}