using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.EverlastingCold
{
    internal class EverlastingCold : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("This forever cold, so cold that it could sent the whole world back to ice age");
        }
        public override void SetDefaults()
        {
            Item.BossRushSetDefaultMelee(
                92,
                92,
                100,
                5f,
                20,
                20,
                ItemUseStyleID.Swing,
                true);
            Item.rare = 6;
            Item.value = Item.buyPrice(gold: 50);
            Item.UseSound = SoundID.Item1;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            for (int i = 0; i < 20; i++)
            {
                int RandomProjectile = Main.rand.Next(new int[] { ProjectileID.FrostBlastFriendly, ProjectileID.IceBolt });
                Vector2 GetPostDirect2 = new Vector2(player.Center.X + Main.rand.Next(300, 1000) * player.direction, player.Center.Y - Main.rand.Next(300, 700));
                Vector2 GoTo = (new Vector2(Main.MouseWorld.X + Main.rand.Next(-200, 200), Main.MouseWorld.Y + Main.rand.Next(-200, 200)) - GetPostDirect2).SafeNormalize(Vector2.UnitX);
                int proj = Projectile.NewProjectile(Item.GetSource_FromThis(), GetPostDirect2, GoTo * 20, RandomProjectile, (int)(damage * 0.65f), knockBack, player.whoAmI);
                Main.projectile[proj].tileCollide = false;
                Main.projectile[proj].timeLeft = 200;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IceBlade)
                .AddIngredient(ItemID.Frostbrand)
                .Register();
        }
    }
}
