using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon
{
    internal class FlamingWoodSword : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Funny how a wooden sword got fire aspect enchantment");
        }

        public override void SetDefaults()
        {
            Item.BossRushSetDefaultMelee(32, 36, 36, 5f, 4, 40, ItemUseStyleID.Swing, false);
            Item.crit = 5;
            Item.rare = 2;
            Item.value = Item.buyPrice(gold: 50);
            Item.useTurn = false;
            Item.shoot = ProjectileID.WandOfSparkingSpark;
            Item.shootSpeed = 6;
            Item.UseSound = SoundID.Item1;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 90);
        }
        int count = 0;
        float rotate;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (count == 0)
            {
                rotate = player.direction == 1 ? MathHelper.ToRadians(470) : MathHelper.ToRadians(250);
            }
            if (count < 10)
            {
                rotate += MathHelper.ToRadians(14) * player.direction;
                Vector2 staticRotate = new Vector2(Item.shootSpeed + player.velocity.X, 0).RotatedBy(rotate);
                Projectile.NewProjectile(source, position, staticRotate, type, (int)(damage * 0.75f), knockback, player.whoAmI);
                count++;
            }
            if (count == 10)
            {
                count = 0;
                rotate = MathHelper.ToRadians(250);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("Wood Sword")
                .AddIngredient(ItemID.WandofSparking)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
}
