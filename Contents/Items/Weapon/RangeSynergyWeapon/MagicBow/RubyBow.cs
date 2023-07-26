using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class RubyBow : MagicBow
    {
        public override void MagicBowSetDefault(out int mana, out int shoot, out float shootspeed, out int damage, out int useTime, out int dustType)
        {
            mana = 12;
            shoot = ModContent.ProjectileType<RubyBolt>();
            shootspeed = 4f;
            damage = 17;
            useTime = 30;
            dustType = DustID.GemRuby;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GoldBow)
                .AddIngredient(ItemID.RubyStaff)
                .Register();
        }
    }
}