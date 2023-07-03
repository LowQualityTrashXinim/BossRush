using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class SapphireBow : MagicBow, ISynergyItem
    {
        public override void MagicBowSetDefault(out int mana, out int shoot, out float shootspeed, out int damage, out int useTime, out int dustType)
        {
            mana = 12;
            damage = 23;
            shoot = ModContent.ProjectileType<SapphireBolt>();
            shootspeed = 6f;
            useTime = 48;
            dustType = DustID.GemSapphire;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SilverBow)
                .AddIngredient(ItemID.SapphireStaff)
                .Register();
        }
    }
}