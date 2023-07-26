using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Global;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.RectangleShotgun
{
    class RectangleShotgun : SynergyModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(12, 74, 75, 4f, 10, 10, ItemUseStyleID.Shoot, ModContent.ProjectileType<RectangleBullet>(), 100f, true, AmmoID.Bullet);
            Item.value = Item.buyPrice(gold: 50);
            Item.rare = ItemRarityID.LightRed;
            Item.reuseDelay = 30;
            Item.UseSound = SoundID.Item38;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<RectangleBullet>();
            position = position.PositionOFFSET(velocity, 40);
            velocity *= .1f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-19, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Boomstick, 2)
            .Register();
        }
    }
}