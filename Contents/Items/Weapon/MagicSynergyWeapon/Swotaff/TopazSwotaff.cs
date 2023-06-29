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
        public override void SwotaffCustomSetDefault(out float AltAttackAmountProjectile, out int AltAttackProjectileType, out int NormalBoltProjectile, out int DustType, out int ManaCost)
        {
            AltAttackAmountProjectile = 4;
            AltAttackProjectileType = ModContent.ProjectileType<TopazGemP>();
            NormalBoltProjectile = ProjectileID.TopazBolt;
            DustType = DustID.GemTopaz;
            ManaCost = 50;
        }
    }
}