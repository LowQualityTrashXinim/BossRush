using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Global;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.ZapSnapper
{
    internal class ZapSnapper : ModItem, ISynergyItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultMagic(56, 16, 12, 2f, 5, 5, ItemUseStyleID.Shoot, ProjectileID.ThunderSpearShot, 22, 4, true);

            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(gold: 50);
            Item.UseSound = SoundID.Item9;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = position.PositionOFFSET(velocity, 30);
            for (int i = 0; i < 3; i++)
            {
                Vector2 newVec = velocity.RotatedByRandom(MathHelper.ToRadians(10));
                int proj = Projectile.NewProjectile(source, position, newVec, ProjectileID.ThunderSpearShot, damage, knockback, player.whoAmI);
                Main.projectile[proj].DamageType = DamageClass.Magic;
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 2);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.RedRyder)
                .AddIngredient(ItemID.ThunderSpear)
                .Register();
        }
    }
}