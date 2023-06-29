using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

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
        protected override bool CanActivateSpecialAltAttack(Player player)
        {
            return player.statMana >= ManaCostForAltSpecial() && player.ownedProjectileCounts[ModContent.ProjectileType<EmeraldSwotaffGemProjectile>()] < AltAttackAmountProjectile();
        }
        protected override int ManaCostForAltSpecial() => 75;
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
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
        }
        Vector2 firstframePos = Vector2.Zero;
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (Projectile.timeLeft == 300)
            {
                firstframePos = Main.MouseWorld - Projectile.Center;
            }
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemSapphire);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = Main.rand.NextFloat(1f);
            }
            float counter = 300 - Projectile.timeLeft;
            Vector2 positionToGo = player.Center + Vector2.One.RotatedBy(MathHelper.ToRadians(72 * Projectile.ai[2] + counter) + firstframePos.ToRotation()) * Projectile.timeLeft;
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }
            Projectile.velocity = (positionToGo - Projectile.Center).SafeNormalize(Vector2.Zero) * (positionToGo - Projectile.Center).Length() * .1f;
        }
        public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone)
        {
            Projectile.Center.LookForHostileNPC(out List<NPC> npclist, 100);
            foreach (NPC npc1 in npclist)
            {
                npc1.StrikeNPC(npc1.CalculateHitInfo(Projectile.damage * 2, 1, false, 0, DamageClass.Magic, false));
            }
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