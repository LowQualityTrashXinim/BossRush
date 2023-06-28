using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.Swotaff
{
    internal class TopazSwotaff : SwotaffGemItem, ISynergyItem
    {
        public override int ProjectileType => ModContent.ProjectileType<TopazSwotaffP>();
        public override int ShootType => ProjectileID.TopazBolt;
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