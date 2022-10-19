using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Weapon.MeleeSynergyWeapon.FlameingWoodSword
{
    internal class FlamingWoodSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Funny how a wooden sword got fire aspect enchantment");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 36;
            Item.rare = 2;

            Item.damage = 16;
            Item.crit = 5;
            Item.knockBack = 1f;

            Item.useTime = 5;
            Item.useAnimation = 50;

            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(gold: 50);
            Item.autoReuse = true;
            Item.useTurn = false;

            Item.shoot = ProjectileID.WandOfSparkingSpark;
            Item.shootSpeed = 6;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 90);
        }
        int count = 0;
        float rotate = MathHelper.ToRadians(250);
        float rotate2 = MathHelper.ToRadians(470);
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.direction == 1)
            {
                if (count < 10)
                {
                    rotate += MathHelper.ToRadians(14);
                    Vector2 staticRotate = new Vector2(Item.shootSpeed + player.velocity.X, 0).RotatedBy(rotate);
                    Projectile.NewProjectile(source, position, staticRotate, type, (int)(damage * 0.75f), knockback, player.whoAmI);
                    count++;
                }
                if (count == 10)
                {
                    count = 0;
                    rotate = MathHelper.ToRadians(250);
                }
            }
            else
            {
                if (count < 10)
                {
                    rotate2 -= MathHelper.ToRadians(14);
                    Vector2 staticRotate = new Vector2(-Item.shootSpeed + player.velocity.X, 0).RotatedBy(rotate2);
                    Projectile.NewProjectile(source, position, staticRotate, type, (int)(damage * 0.75f), knockback, player.whoAmI);
                    count++;
                }
                if (count == 10)
                {
                    count = 0;
                    rotate2 = MathHelper.ToRadians(470);
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("Wood Sword")
                .AddIngredient(ItemID.WandofSparking)
                .Register();
        }
    }
}
