using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class EmeraldBow : MagicBow, ISynergyItem
    {
        public override void MagicBowSetDefault(out int mana, out int shoot, out float shootspeed, out int damage, out int useTime, out int dustType)
        {
            mana = 12;
            damage = 17;
            useTime = 28;
            shoot = ModContent.ProjectileType<EmeraldBolt>();
            shootspeed = 6f;
            dustType = DustID.GemEmerald;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TungstenBow)
                .AddIngredient(ItemID.EmeraldStaff)
                .Register();
        }
    }
}