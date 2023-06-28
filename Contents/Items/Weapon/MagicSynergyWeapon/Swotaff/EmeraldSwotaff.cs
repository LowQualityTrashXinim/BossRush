using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.Swotaff
{
    internal class EmeraldSwotaff : SwotaffGemItem, ISynergyItem
    {
        public override int ProjectileType => ModContent.ProjectileType<EmeraldSwotaffP>();
        public override int ShootType => ProjectileID.EmeraldBolt;
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TungstenBroadsword)
                .AddIngredient(ItemID.EmeraldStaff)
                .Register();
        }
    }
    public class EmeraldSwotaffP : SwotaffProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<EmeraldSwotaff>();
        protected override int AltAttackProjectileType() => ModContent.ProjectileType<EmeraldSwotaffGemProjectile>();
        protected override float AltAttackAmountProjectile() => 5;
        protected override bool CanActivateSpecialAltAttack() =>
            Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<EmeraldSwotaffGemProjectile>()] < AltAttackAmountProjectile();
        protected override int NormalBoltProjectile() => ProjectileID.EmeraldBolt;
        protected override int DustType() => DustID.GemEmerald;
    }
    public class EmeraldSwotaffGemProjectile : SynergyModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Emerald);
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            Projectile.velocity = (Projectile.Center - player.Center).SafeNormalize(Vector2.Zero) * 2f;

            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2CircularEdge(200f,200f), 0, 0, DustID.GemEmerald);
                Main.dust[dust].noGravity = true;
            }
            if (Projectile.ai[0] > 0)
            {
                Projectile.ai[0]--;
                return;
            }
            if (Projectile.position.LookForHostileNPC(out NPC npc, 200))
            {
                if (npc is null)
                {
                    return;
                }
                npc.StrikeNPC(npc.CalculateHitInfo((int)(Projectile.damage * .25f), 1, false, 0, DamageClass.Magic, false));
                Projectile.ai[0] = 20;
            }
        }
    }
}