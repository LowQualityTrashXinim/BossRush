using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class DiamondBow : MagicBow, ISynergyItem
    {
        public override void MagicBowSetDefault(out int mana, out int shoot, out float shootspeed, out int damage, out int useTime, out int dustType)
        {
            mana = 12;
            shoot = ModContent.ProjectileType<DiamondBolt>();
            shootspeed = 10f;
            damage = 39;
            useTime = 24;
            dustType = DustID.GemDiamond;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PlatinumBow)
                .AddIngredient(ItemID.DiamondStaff)
                .Register();
        }
    }
}