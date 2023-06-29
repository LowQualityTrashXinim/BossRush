using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.Swotaff
{
    internal class TopazSwotaff : SwotaffGemItem, ISynergyItem
    {
        public override void PreSetDefaults(out int damage, out int ProjectileType, out int ShootType)
        {
            damage = 20;
            ProjectileType = ModContent.ProjectileType<TopazSwotaffP>();
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
    public class TopazSwotaffP : SwotaffProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<TopazSwotaff>();
        protected override int AltAttackProjectileType() => ModContent.ProjectileType<TopazGemP>();
        protected override int NormalBoltProjectile() => ProjectileID.TopazBolt;
        protected override int DustType() => DustID.GemTopaz;
    }
}