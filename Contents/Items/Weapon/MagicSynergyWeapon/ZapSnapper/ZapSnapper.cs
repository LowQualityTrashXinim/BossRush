using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.ZapSnapper
{
    internal class ZapSnapper : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Zap Rifle");
            // Tooltip.SetDefault("quite a zapping");
        }
        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 16;

            Item.damage = 12;
            Item.knockBack = 2f;
            Item.shoot = ProjectileID.ThunderSpearShot;
            Item.shootSpeed = 22;
            Item.useTime = 5;
            Item.useAnimation = 5;

            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.mana = 4;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(gold: 50);

            Item.UseSound = SoundID.Item9;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 30f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
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
