using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.AmethystSwotaff
{
    internal class AmethystSwotaff : SwotaffGemItem, ISynergyItem
    {
        public override int ProjectileType => ModContent.ProjectileType<AmethystSwotaffP>();
        public override int ShootType => ProjectileID.AmethystBolt;
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CopperBroadsword)
                .AddIngredient(ItemID.AmethystStaff)
                .Register();
        }
    }
    public class AmethystSwotaffP : SwotaffProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<AmethystSwotaff>();
        protected override int AltAttackProjectileType() => ModContent.ProjectileType<AmethystGemP>();
        protected override int NormalBoltProjectile() => ProjectileID.AmethystBolt;
        protected override int DustType() => DustID.GemAmethyst;
    }
}