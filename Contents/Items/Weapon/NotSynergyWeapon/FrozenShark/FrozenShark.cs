using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Global;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Weapon.NotSynergyWeapon.FrozenShark
{
    internal class FrozenShark : ModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(64, 20, 16, 1f, 9, 9, ItemUseStyleID.Shoot, ProjectileID.IceBolt, 12, true);
            Item.rare = 3;
            Item.value = Item.buyPrice(gold: 50);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.rand.NextBool(5))
            {
                Projectile.NewProjectile(source, position, velocity.NextVector2RotatedByRandom(3), type, damage, knockback, player.whoAmI);
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2f, -3f);
        }
    }
}