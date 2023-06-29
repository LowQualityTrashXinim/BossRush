using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.Swotaff
{
    internal class RubySwotaff : SwotaffGemItem, ISynergyItem
    {
        public override void PreSetDefaults(out int damage, out int ProjectileType, out int ShootType)
        {
            damage = 20;
            ProjectileType = ModContent.ProjectileType<RubySwotaffProjectile>();
            ShootType = ProjectileID.RubyBolt;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.RubyStaff)
                .AddIngredient(ItemID.GoldBroadsword)
                .Register();
        }
    }
    class RubySwotaffProjectile : SwotaffProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<RubySwotaff>();
        public override void SwotaffCustomSetDefault(out float AltAttackAmountProjectile, out int AltAttackProjectileType, out int NormalBoltProjectile, out int DustType, out int ManaCost)
        {
            AltAttackAmountProjectile = 9;
            AltAttackProjectileType = ModContent.ProjectileType<RubySwotaffGemProjectile>();
            NormalBoltProjectile = ProjectileID.RubyBolt;
            DustType = DustID.GemRuby;
            ManaCost = 100;
        }
    }
    class RubySwotaffGemProjectile : SynergyModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Ruby);
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
        }
    }
}