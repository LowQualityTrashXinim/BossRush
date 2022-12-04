using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon
{
    internal class BloodStar : ModItem, ISynergyItem
    {
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;

            Item.damage = 40;
            Item.knockBack = 4;

            Item.shoot = ProjectileID.StarCannonStar;
            Item.shootSpeed = 20;

            Item.useTime = 15;
            Item.useAnimation = 15;

            Item.channel = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(gold: 50);
            Item.rare = 3;

            Item.UseSound = SoundID.Item1;
        }

        int count = 0;
        public override void HoldItem(Player player)
        {
            if (Main.mouseRight)
            {
                count++;
                if (count >= 4 && player.GetModPlayer<PlayerCharge>().ChargePower <= 50)
                {
                    player.GetModPlayer<PlayerCharge>().ChargePower++;
                    count = 0;
                }
            }
            if (Main.mouseRightRelease)
            {
                count = 0;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 newPos = new Vector2(Main.MouseWorld.X, player.Center.Y - 800);
            Vector2 FallVelocity = (Main.MouseWorld - newPos).SafeNormalize(Vector2.UnitX) * 20;
            for (int i = 0; i < 10 + player.GetModPlayer<PlayerCharge>().ChargePower; i++)
            {
                Projectile.NewProjectile(source, newPos.X + Main.rand.Next(-100, 100), newPos.Y + Main.rand.Next(-10, 10), FallVelocity.X, FallVelocity.Y, ProjectileID.BloodArrow, damage, knockback, player.whoAmI);
            }
            for (int i = 0; i < 3 + player.GetModPlayer<PlayerCharge>().ChargePower; i++)
            {
                Projectile.NewProjectile(source, newPos.X + Main.rand.Next(-100, 100), newPos.Y + Main.rand.Next(-10, 10), FallVelocity.X, FallVelocity.Y, ProjectileID.StarCannonStar, damage, knockback, player.whoAmI);
            }
            if (player.GetModPlayer<PlayerCharge>().ChargePower >= 25)
            {
                int SuperStar = Projectile.NewProjectile(source, newPos, FallVelocity, ProjectileID.SuperStar, damage, knockback, player.whoAmI);
                Projectile Proj = Main.projectile[SuperStar];
                Proj.scale += 10;
            }
            player.GetModPlayer<PlayerCharge>().ChargePower = 0;

            return false;
        }
    }
    class PlayerCharge : ModPlayer
    {
        public float ChargePower = 0;
    }
}
