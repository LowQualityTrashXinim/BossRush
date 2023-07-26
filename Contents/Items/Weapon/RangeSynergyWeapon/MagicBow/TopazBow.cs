using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class TopazBow : MagicBow
    {
        public override void MagicBowSetDefault(out int mana, out int shoot, out float shootspeed, out int damage, out int useTime, out int dustType)
        {
            useTime = 40;
            mana = 12;
            damage = 21;
            shootspeed = 5f;
            shoot = ModContent.ProjectileType<TopazBolt>();
            dustType = DustID.GemTopaz;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TinBow)
                .AddIngredient(ItemID.TopazStaff)
                .Register();
        }
    }
}