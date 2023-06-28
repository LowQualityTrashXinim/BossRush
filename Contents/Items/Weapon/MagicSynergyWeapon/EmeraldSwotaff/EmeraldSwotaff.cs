using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.EmeraldSwotaff
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
        protected override int AltAttackProjectileType() => ModContent.ProjectileType<AmethystGemP>();
        protected override int NormalBoltProjectile() => ProjectileID.EmeraldBolt;
        protected override int DustType() => DustID.GemEmerald;
    }
    public class EmeraldSwotaffGemProjectile : SwotaffGemProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Emerald);
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
    class EmeraldGemProjectile : SynergyModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Emerald);
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
