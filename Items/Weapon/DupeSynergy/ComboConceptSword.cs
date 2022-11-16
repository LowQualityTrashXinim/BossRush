using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.DupeSynergy
{
    internal class ComboConceptSword : ModItem
    {
        public override string Texture => "BossRush/Items/Weapon/DupeSynergy/GenericDupeSword";
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 44;

            Item.damage = 20;
            Item.knockBack = 3;
            Item.crit = 10;

            Item.useTime = 15;
            Item.useAnimation = 15;

            Item.rare = 1;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAmmo = AmmoID.Arrow;
            Item.shootSpeed = 10;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.mouseLeft)
            {
                player.GetModPlayer<ComboPlayer>().a++;
            }
            if (Main.mouseRight)
            {
                player.GetModPlayer<ComboPlayer>().b++;
            }
            switch (player.GetModPlayer<ComboPlayer>().ReturnComboType())
            {
                case 1:
                    Projectile.NewProjectile(source, position, velocity, ProjectileID.EnchantedBeam, damage, knockback, player.whoAmI);
                    break;
                case 2:
                    Projectile.NewProjectile(source, position, velocity, ProjectileID.Fireball, damage, knockback, player.whoAmI);
                    break;
                case 3:
                    Projectile.NewProjectile(source, position, velocity, ProjectileID.UnholyArrow, damage, knockback, player.whoAmI);
                    break;
                case 4:
                    Projectile.NewProjectile(source, position, velocity, ProjectileID.LaserMachinegun, damage, knockback, player.whoAmI);
                    break;
                default:
                    break;
            }
            return false;
        }
    }
    public class ComboPlayer : ModPlayer
    {
        public int a = 0;
        public int b = 0;

        public int ReturnComboType()
        {
            if (a - b == 2)
            {
                a = 0;
                b = 0;
                return 1;
            }
            if (b - a == 2)
            {
                a = 0;
                b = 0;
                return 2;
            }
            if (a - b == 1 && a != b)
            {
                a = 0;
                b = 0;
                return a > b ? 3 : 4;
            }
            return 0;
        }
    }
}
