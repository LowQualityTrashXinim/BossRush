using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.Swotaff
{
    internal class DiamondSwotaff : SwotaffGemItem, ISynergyItem
    {
        public override int ProjectileType => ModContent.ProjectileType<DiamondSwotaffP>();
        public override int ShootType => ProjectileID.DiamondBolt;
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PlatinumBroadsword)
                .AddIngredient(ItemID.DiamondStaff)
                .Register();
        }
    }
    public class DiamondSwotaffP : SwotaffProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<DiamondSwotaff>();
        //protected override int AltAttackProjectileType() => ModContent.ProjectileType<DiamondSwotaffGemProjectile>();
        protected override int NormalBoltProjectile() => ProjectileID.DiamondBolt;
        protected override int AltAttackProjectileType() => ModContent.ProjectileType<DiamondSwotaffGemProjectile>();
        protected override float AltAttackAmountProjectile() => 9;
        protected override int DustType() => DustID.GemDiamond;
        protected override bool CanActivateSpecialAltAttack() =>
            Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<DiamondSwotaffGemProjectile>()] < AltAttackAmountProjectile();

    }
    public class DiamondSwotaffGemProjectile : SynergyModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Diamond);
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 1000;
            Projectile.penetrate = -1;
        }

        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemDiamond);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = Main.rand.NextFloat(1f);
            }
            Vector2 positionToGo = player.Center + Vector2.UnitY.RotatedBy(30 * (Projectile.ai[2] - 5)) * 50f;
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }
            Projectile.velocity = (positionToGo - Projectile.Center).SafeNormalize(Vector2.Zero) * (positionToGo - Projectile.Center).Length() * .25f;
            if (Projectile.ai[0] > 0)
            {
                Projectile.ai[0]--;
                return;
            }
            if (Projectile.position.LookForHostileNPC(out NPC npc, 600))
            {
                if (npc is null)
                {
                    return;
                }
                Vector2 toNPC = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 20f;
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, toNPC, ModContent.ProjectileType<DiamondGemProjectile>(), Projectile.damage, 0, Projectile.owner);
                Projectile.ai[0] = 60;
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemDiamond);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = Main.rand.NextFloat(1.25f, 1.75f);
            }
            base.Kill(timeLeft);
        }
    }
    class DiamondGemProjectile : SynergyModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Diamond);
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 16;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 6;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hide = true;
        }
        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemDiamond);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = Main.rand.NextFloat(1f, 1.25f);
            }
        }
    }
}
