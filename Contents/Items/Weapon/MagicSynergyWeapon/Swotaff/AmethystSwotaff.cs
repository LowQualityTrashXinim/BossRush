using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.Swotaff
{
    internal class AmethystSwotaff : SwotaffGemItem, ISynergyItem
    {
        public override int ProjectileType => ModContent.ProjectileType<AmethystSwotaffP>();
        public override int ShootType => ProjectileID.AmethystBolt;
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CopperBroadsword)
                .AddIngredient(ItemID.AmethystStaff)
                .Register();
        }
    }
    public class AmethystSwotaffP : SwotaffProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<AmethystSwotaff>();
        protected override int AltAttackProjectileType() => ModContent.ProjectileType<AmethystSwotaffGemProjectile>();
        protected override int NormalBoltProjectile() => ProjectileID.AmethystBolt;
        protected override int DustType() => DustID.GemAmethyst;
        protected override bool CanActivateSpecialAltAttack(Player player)
        {
            return player.statMana >= ManaCostForAltSpecial() && player.ownedProjectileCounts[ModContent.ProjectileType<EmeraldSwotaffGemProjectile>()] < AltAttackAmountProjectile();
        }
        protected override int ManaCostForAltSpecial() => 50;
    }
    public class AmethystSwotaffGemProjectile : SynergyModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Amethyst);
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Magic;
        }
        Vector2 firstframePos = Vector2.Zero;
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (Projectile.timeLeft == 300)
            {
                Projectile.velocity = Vector2.Zero;
                firstframePos = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero);
            }
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemAmethyst);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = Main.rand.NextFloat(1f);
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 20)
            {
                Projectile.hide = true;
                for (int i = 0; i < 3; i++)
                {
                    int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemAmethyst);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity = Main.rand.NextVector2Circular(3f, 3f);
                    Main.dust[dust].scale = Main.rand.NextFloat(1.25f, 1.75f);
                }
                Projectile.velocity = firstframePos * 15f;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemAmethyst);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = Main.rand.NextFloat(1.25f, 1.75f);
            }
            base.Kill(timeLeft);
        }
    }
}