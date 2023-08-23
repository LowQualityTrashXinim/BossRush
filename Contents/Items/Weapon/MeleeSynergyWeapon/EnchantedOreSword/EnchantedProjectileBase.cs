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
            Projectile.width = Projectile.height = 32;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public virtual void PostSetDefault() { }
        int counter = 0;
        public override void SynergyPostAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            counter++;
            if (modplayer.EnchantedOreSword_StarFury && counter >= 20)
            {
                int projectile = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, -Projectile.velocity, ProjectileID.Starfury, Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[projectile].tileCollide = false;
                Main.projectile[projectile].timeLeft = 30;
                Main.projectile[projectile].penetrate = 1;
                counter = 0;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            if (Projectile.alpha >= 235)
            {
                Projectile.Kill();
            }
        }
        public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone)
        {
            EnchantedProjectileOnHitNPC(npc, hit, damageDone);
        }
        public virtual void EnchantedProjectileOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) { }
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
            Projectile.alpha += 2;
        }
    }
    internal class EnchantedTinSwordP : EnchantedProjectileBase
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.TinShortsword);
        public override void PostSetDefault()
        {
            Projectile.light = .45f;
        }
        int count = 0;
        float rotate = -2.5f;
        public override void AI()
        {
            Projectile.alpha += 2;
            Projectile.ai[0] += 1f;
            if (count == 2)
            {
                rotate = -5f;
            }
            if (count == 1)
            {
                rotate = 5f;
            }
            if (Projectile.ai[0] == 10)
            {
                Projectile.ai[0] = 0;
                count++;
            }
            if (count > 2)
            {
                count = 1;
            }
            Vector2 Helix = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(rotate));
            Projectile.velocity = Helix;
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
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.Next(-10, 10)));
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
        int direction = 0;
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (count == 0)
            {
                direction = Projectile.Center.X - player.Center.X > 0 ? 1 : -1;
            }
            count++;
            if (count >= 20)
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(3) * direction * 1.01f;
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
        int firstFrame = 0;
        int direction = 0;
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (firstFrame == 0)
            {
                direction = Projectile.Center.X - player.Center.X > 0 ? 1 : -1;
                firstFrame++;
            }
            Projectile.alpha += 3;
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(1 * direction)) * 1.05f;
        }
    }
    internal class EnchantedTungstenSwordP : EnchantedProjectileBase
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.TungstenShortsword);
        public override void PostSetDefault()
        {
            Projectile.light = 0.65f;
        }
        int firstFrame = 0;
        int direction = 0;
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (firstFrame == 0)
            {
                direction = Projectile.Center.X - player.Center.X > 0 ? 1 : -1;
                firstFrame++;
            }
            Projectile.alpha += 3;
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(-1 * direction)) * 1.05f;
        }
    }
    internal class EnchantedGoldSwordP : EnchantedProjectileBase
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.GoldShortsword);
        public override void PostSetDefault()
        {
            Projectile.light = .75f;
        }
        public override void EnchantedProjectileOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 4;
        }
        public override void AI()
        {
            Projectile.velocity += (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.UnitX);
            Projectile.velocity = Projectile.velocity.LimitedVelocity(20);
            Projectile.alpha += 2;
        }
    }
    internal class EnchantedPlatinumSwordP : EnchantedProjectileBase
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.PlatinumShortsword);
        public override void PostSetDefault()
        {
            Projectile.light = .75f;
            Projectile.knockBack = 0;
        }
        public override void AI()
        {
            Projectile.alpha += 2;
            Projectile.knockBack = 0;
        }
        public override void EnchantedProjectileOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 3;
            Projectile.Center += Main.rand.NextVector2CircularEdge(200, 200);
            Projectile.velocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 30f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Projectile.damage += 2;
        }
    }
}