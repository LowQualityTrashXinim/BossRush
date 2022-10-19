using Terraria;
using Terraria.ModLoader;

namespace BossRush.Weapon
{
    internal class TestSword : ModItem
    {
        public override string Texture => "BossRush/Weapon/TestProjectileRotate";
        public override void SetDefaults()
        {
            Item.noUseGraphic = true;
            Item.damage = 10;
            Item.DamageType = DamageClass.Melee;
            Item.width = 109;
            Item.height = 109;
            Item.useStyle = 1;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TestProjectileRotate>();
            Item.shootSpeed = 10;
        }
    }
}
