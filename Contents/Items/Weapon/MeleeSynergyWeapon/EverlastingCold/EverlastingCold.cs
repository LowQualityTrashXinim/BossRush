using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.EverlastingCold
{
    internal class EverlastingCold : SynergyModItem
    {
        public override void SetDefaults()
        {
            BossRushUtils.BossRushSetDefault(Item, 92, 92, 100, 5f, 20, 20, BossRushUseStyle.Swipe, true);
            Item.DamageType = DamageClass.Melee;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.buyPrice(gold: 50);
            Item.UseSound = SoundID.Item1;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 20; i++)
            {
                int RandomProjectile = Main.rand.Next(new int[] { ProjectileID.FrostBlastFriendly, ProjectileID.IceBolt });
                Vector2 GetPostDirect2 = new Vector2(player.Center.X + Main.rand.Next(300, 1000) * player.direction, player.Center.Y - Main.rand.Next(300, 700));
                Vector2 GoTo = (new Vector2(Main.MouseWorld.X + Main.rand.Next(-200, 200), Main.MouseWorld.Y + Main.rand.Next(-200, 200)) - GetPostDirect2).SafeNormalize(Vector2.UnitX);
                int proj = Projectile.NewProjectile(Item.GetSource_FromThis(), GetPostDirect2, GoTo * 20, RandomProjectile, (int)(hit.Damage * 0.65f), hit.Knockback, player.whoAmI);
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
