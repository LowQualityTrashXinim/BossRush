using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow;
using Terraria;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.Swotaff
{
    internal class TopazSwotaff : SwotaffGemItem, ISynergyItem
    {
        public override void PreSetDefaults(out int damage, out int ProjectileType, out int ShootType)
        {
            damage = 20;
            ProjectileType = ModContent.ProjectileType<TopazSwotaffProjectile>();
            ShootType = ProjectileID.TopazBolt;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TinBroadsword)
                .AddIngredient(ItemID.TopazStaff)
                .Register();
        }
    }
    public class TopazSwotaffProjectile : SwotaffProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<TopazSwotaff>();
        public override void SwotaffCustomSetDefault(out float AltAttackAmountProjectile, out int AltAttackProjectileType, out int NormalBoltProjectile, out int DustType, out int ManaCost)
        {
            AltAttackAmountProjectile = 4;
            AltAttackProjectileType = ModContent.ProjectileType<TopazGemP>();
            NormalBoltProjectile = ProjectileID.TopazBolt;
            DustType = DustID.GemTopaz;
            ManaCost = 50;
        }
    }
    public class TopazGemProjectile : SynergyModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Topaz);
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 500;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {

        }
    }
}