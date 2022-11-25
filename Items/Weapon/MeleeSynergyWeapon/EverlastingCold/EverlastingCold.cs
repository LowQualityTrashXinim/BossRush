using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon.EverlastingCold
{
    internal class EverlastingCold : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("This forever cold, so cold that it could sent the whole world back to ice age");
        }
        public override void SetDefaults()
        {
            Item.width = 92;
            Item.height = 92;
            Item.rare = 6;

            Item.damage = 100;
            Item.knockBack = 3;

            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Melee;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(gold: 50);

            Item.UseSound = SoundID.Item1;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            EntitySource_ItemUse source = new EntitySource_ItemUse(player, Item);
            if (player.direction == 1)
            {
                for (int i = 0; i < 20; i++)
                {
                    int RandomProjectile = Main.rand.Next(new int[] { ProjectileID.FrostBlastFriendly, ProjectileID.IceBolt });
                    Vector2 GetPostDirect1 = new Vector2(player.Center.X + Main.rand.Next(-1000, -300), player.Center.Y - Main.rand.Next(300, 700));
                    Vector2 GoTo = (new Vector2(Main.MouseWorld.X + Main.rand.Next(-200, 200), Main.MouseWorld.Y + Main.rand.Next(-200, 200)) - GetPostDirect1).SafeNormalize(Vector2.UnitX);
                    Projectile.NewProjectile(source, GetPostDirect1, GoTo * 20, RandomProjectile, (int)(damage * 0.65f), knockBack, player.whoAmI);
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    int RandomProjectile = Main.rand.Next(new int[] { ProjectileID.FrostBlastFriendly, ProjectileID.IceBolt });
                    Vector2 GetPostDirect2 = new Vector2(player.Center.X + Main.rand.Next(300, 1000), player.Center.Y - Main.rand.Next(300, 700));
                    Vector2 GoTo = (new Vector2(Main.MouseWorld.X + Main.rand.Next(-200, 200), Main.MouseWorld.Y + Main.rand.Next(-200, 200)) - GetPostDirect2).SafeNormalize(Vector2.UnitX);
                    Projectile.NewProjectile(source, GetPostDirect2, GoTo * 20, RandomProjectile, (int)(damage * 0.65f), knockBack, player.whoAmI);
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IceBlade)
                .AddIngredient(ItemID.Frostbrand)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
}
