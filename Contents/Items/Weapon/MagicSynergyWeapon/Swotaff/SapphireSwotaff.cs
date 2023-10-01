using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.Swotaff
{
    internal class SapphireSwotaff : SwotaffGemItem
    {
        public override void PreSetDefaults(out int damage, out int ProjectileType, out int ShootType)
        {
            damage = 16;
            ProjectileType = ModContent.ProjectileType<SapphireSwotaffP>();
            ShootType = ProjectileID.SapphireBolt;
        }
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
        public override void SwotaffCustomSetDefault(out float AltAttackAmountProjectile, out int AltAttackProjectileType, out int NormalBoltProjectile, out int DustType, out int ManaCost)
        {
            AltAttackAmountProjectile = 5;
            AltAttackProjectileType = ModContent.ProjectileType<SapphireSwotaffGemProjectile>();
            NormalBoltProjectile = ProjectileID.SapphireBolt;
            DustType = DustID.GemSapphire;
            ManaCost = 75;
        }
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
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemSapphire);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = Main.rand.NextFloat(1.25f, 1.75f);
            }
            base.OnKill(timeLeft);
        }
    }
}