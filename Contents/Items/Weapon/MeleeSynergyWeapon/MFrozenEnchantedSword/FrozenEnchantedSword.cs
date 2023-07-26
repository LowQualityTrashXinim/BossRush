using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.MFrozenEnchantedSword
{
    public class FrozenEnchantedSword : SynergyModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushSetDefault(34, 40, 29, 7f, 15, 15, BossRushUseStyle.GenericSwingDownImprove, true);

            Item.DamageType = DamageClass.Melee;
            Item.rare = 3;
            Item.shoot = ProjectileID.EnchantedBeam;
            Item.shootSpeed = 15;
            Item.value = Item.buyPrice(gold: 50);
            Item.UseSound = SoundID.Item1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ProjectileID.IceBolt, damage, knockback, player.whoAmI);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.EnchantedSword)
                .AddIngredient(ItemID.IceBlade)
                .Register();
        }
    }
}
