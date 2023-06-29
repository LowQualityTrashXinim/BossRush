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
        protected override int AltAttackProjectileType() => ModContent.ProjectileType<RubySwotaffGemProjectile>();
        protected override float AltAttackAmountProjectile() => 9;
        protected override int ManaCostForAltSpecial() => 100;
        protected override int NormalBoltProjectile() => ProjectileID.RubyBolt;
        protected override int DustType() => DustID.GemRuby;
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