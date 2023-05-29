using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.EnchantedOreSword
{
    abstract class EnchantedProjectileBase : SynergyModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.width = 32;
            Projectile.height = 32;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public virtual void PostSetDefault() { }
        int counter = 0;
        public override void SynergyPostAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            base.SynergyPostAI(player, modplayer);
            counter++;
            if (modplayer.EnchantedOreSword_StarFury && counter >= 10)
            {
                int projectile = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity * .001f, ProjectileID.Starfury, Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[projectile].tileCollide = false;
                Main.projectile[projectile].timeLeft = 90;
                counter = 0;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            if (Projectile.alpha >= 235)
            {
                Projectile.Kill();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrail(lightColor);
            return base.PreDraw(ref lightColor);
        }
    }
    internal class EnchantedCopperSwordP : EnchantedProjectileBase
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.CopperShortsword);
        public override void PostSetDefault()
        {
            Projectile.light = .45f;
        }
        public override void AI()
        {
            Projectile.alpha += 5;
        }
    }
    internal class EnchantedTinSwordP : EnchantedProjectileBase
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.TinShortsword);
        public override void PostSetDefault()
        {
            Projectile.light = .45f;
        }
        public override void AI()
        {
            Projectile.alpha += 5;
        }
    }
    internal class EnchantedIronSwordP : EnchantedProjectileBase
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.IronShortsword);
        public override void SetStaticDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }
        public override void PostSetDefault()
        {
            Projectile.light = .55f;
        }
        public override void AI()
        {
            Projectile.alpha += 4;
        }
    }
    internal class EnchantedLeadSwordP : EnchantedProjectileBase
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.LeadShortsword);
        public override void PostSetDefault()
        {
            Projectile.light = .55f;
        }
        int count = 0;
        public override void AI()
        {
            count++;
            if (count >= 20)
            {
                Projectile.velocity = Main.rand.NextVector2CircularEdge(10f, 10f);
                count = 0;
            }
            Projectile.alpha += 2;
        }
    }
    internal class EnchantedSilverSwordP : EnchantedProjectileBase
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.SilverShortsword);
        public override void PostSetDefault()
        {
            Projectile.light = 0.65f;
        }
        public override void AI()
        {
            Projectile.alpha += 3;
        }
    }
    internal class EnchantedTungstenSwordP : EnchantedProjectileBase
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.TungstenShortsword);
        public override void PostSetDefault()
        {
            Projectile.light = 0.65f;
        }
        public override void AI()
        {
            Projectile.alpha += 3;
        }
    }
    internal class EnchantedGoldSwordP : EnchantedProjectileBase
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.GoldShortsword);
        public override void PostSetDefault()
        {
            Projectile.light = .75f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 4;
        }
        public override void AI()
        {
            Projectile.velocity += (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.UnitX);
            Projectile.velocity = Projectile.velocity.LimitedVelocity(10);
            Projectile.alpha += 2;
        }
    }
    internal class EnchantedPlatinumSwordP : EnchantedProjectileBase
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.PlatinumShortsword);
        public override void PostSetDefault()
        {
            Projectile.light = .75f;
        }
        public override void AI()
        {
            Projectile.alpha += 2;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 3;
            Projectile.position += Main.rand.NextVector2CircularEdge(200, 200);
            Projectile.velocity = (target.Center - Projectile.position).SafeNormalize(Vector2.UnitX) * 20;
        }
    }
}