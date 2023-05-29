using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Global;

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
            counter++;
            if (modplayer.EnchantedOreSword_StarFury && counter >= 20)
            {
                int projectile = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity * .001f, ProjectileID.Starfury, Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[projectile].tileCollide = false;
                Main.projectile[projectile].timeLeft = 120;
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
            if (modplayer.EnchantedOreSword_Musket && player.ownedProjectileCounts[ModContent.ProjectileType<MusketGunProjectile>()] < 3)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), npc.Center, Main.rand.NextVector2CircularEdge(10f, 10f), ModContent.ProjectileType<MusketGunProjectile>(), Projectile.damage, 0, Projectile.owner);
            }
            EnchantedProjectileOnHitNPC(npc, hit, damageDone);
        }
        public virtual void EnchantedProjectileOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) { }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrail(lightColor);
            return base.PreDraw(ref lightColor);
        }
    }
    public class MusketGunProjectile : ModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Musket);
        public override void SetDefaults()
        {
            Projectile.width = 56;
            Projectile.height = 18;
            Projectile.timeLeft = 250;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override bool? CanDamage() => false;
        int timer = 50;
        public override void AI()
        {
            Projectile.velocity -= Projectile.velocity * .05f;
            if (timer > 0)
            {
                timer = BossRushUtils.CoolDown(timer);
            }
            else
            {
                if (Projectile.Center.LookForHostileNPC(out NPC npc, 800) && npc != null)
                {
                    Vector2 velocityToNpc = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                    Projectile.spriteDirection = Projectile.Center.X < npc.Center.X ? 1 : -1;
                    Projectile.rotation = velocityToNpc.ToRotation();
                    Projectile.rotation += Projectile.spriteDirection == 1 ? 0 : MathHelper.Pi;
                    timer = 50;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.PositionOFFSET(velocityToNpc, 45f), velocityToNpc * 20f, ProjectileID.Bullet, Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Main.rand.NextVector2Circular(5f, 5f);
                Main.dust[dust].scale = Main.rand.NextFloat(.75f, 1.5f);
                Main.dust[dust].rotation = MathHelper.ToRadians(20f);
            }
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
        public override void EnchantedProjectileOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
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
        public override void EnchantedProjectileOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 3;
            Projectile.position += Main.rand.NextVector2CircularEdge(200, 200);
            Projectile.velocity = (target.Center - Projectile.position).SafeNormalize(Vector2.UnitX) * 20;
        }
    }
}