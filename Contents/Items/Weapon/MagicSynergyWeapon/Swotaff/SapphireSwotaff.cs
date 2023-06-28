using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.Swotaff
{
    internal class SapphireSwotaff : SwotaffGemItem, ISynergyItem
    {
        public override int ProjectileType => ModContent.ProjectileType<SapphireSwotaffP>();
        public override int ShootType => ProjectileID.SapphireBolt;
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SilverBroadsword)
                .AddIngredient(ItemID.SapphireStaff)
                .Register();
        }
    }

    public class SapphireSwotaffP : SwotaffProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<SapphireSwotaff>();
        protected override int AltAttackProjectileType() => ModContent.ProjectileType<SapphireSwotaffGemProjectile>();
        protected override float AltAttackAmountProjectile() => 5;
        protected override bool CanActivateSpecialAltAttack() =>
            Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<SapphireSwotaffGemProjectile>()] < AltAttackAmountProjectile();
        protected override int NormalBoltProjectile() => ProjectileID.SapphireBolt;
        protected override int DustType() => DustID.GemSapphire;
    }
    public class SapphireSwotaffGemProjectile : SynergyModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Sapphire);
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemSapphire);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = Main.rand.NextFloat(1f);
            }
            float counter = (300 - Projectile.timeLeft);
            Vector2 positionToGo = player.Center + Vector2.One.RotatedBy(MathHelper.ToRadians(72 * Projectile.ai[2] + counter)) * Projectile.timeLeft;
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }
            Projectile.velocity = (positionToGo - Projectile.Center).SafeNormalize(Vector2.Zero) * (positionToGo - Projectile.Center).Length() * .1f;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemSapphire);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = Main.rand.NextFloat(1.25f, 1.75f);
            }
            base.Kill(timeLeft);
        }
    }
}