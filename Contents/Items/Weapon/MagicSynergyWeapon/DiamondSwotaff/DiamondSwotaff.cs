using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.DiamondSwotaff
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
        protected override int DustType() => DustID.GemDiamond;
        protected override float AltAttackAmountProjectile() => 9;
    }
    public class DiamondSwotaffGemProjectile : SwotaffGemProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Diamond);
        public override void PreSetDefault()
        {
            Projectile.width = 18;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1000;
            Projectile.penetrate = -1;
        }
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            Projectile.velocity = (Projectile.Center - player.Center).SafeNormalize(Vector2.Zero);
            if (Projectile.position.LookForHostileNPC(out NPC npc, 600))
            {
                if (npc is null)
                {
                    return;
                }

            }
        }
    }
    class DiamondGemProjectile : SynergyModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Diamond);
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 100;
            Projectile.penetrate = -1;
            Projectile.hide = true;
        }
    }
}
