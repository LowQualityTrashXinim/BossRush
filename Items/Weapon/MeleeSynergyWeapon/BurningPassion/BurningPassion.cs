using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon.BurningPassion
{
    class BurningPassion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Brimsing with passion");
        }

        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 28;
            Item.useTime = 40;
            Item.shootSpeed = 3.7f;
            Item.knockBack = 6.5f;
            Item.width = 74;
            Item.height = 74;
            Item.rare = 3;
            Item.value = Item.sellPrice(silver: 1000);

            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;

            Item.shoot = ModContent.ProjectileType<BurningPassionP>();
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                player.velocity = Vector2.Zero;

                player.velocity.X = velocity.X * 5f;
                player.velocity.Y = velocity.Y * 5f;
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Spear, 2)
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .Register();
        }
    }
    class BurningPassionPlayer : ModPlayer
    {
        int check = 1;
        public override void PostUpdate()
        {
            if (Player.HeldItem.type == ModContent.ItemType<BurningPassion>())
            {
                if (!Player.ItemAnimationActive && check == 0)
                {
                    Player.velocity -= Player.velocity * 0.75f;
                    check++;
                }
                else if (Player.ItemAnimationActive && Main.mouseRight)
                {
                    Player.gravity = 0;
                    Player.velocity.Y -= 0.3f;
                    Player.ignoreWater = true;
                    check = 0;
                }
            }
        }
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            //BurningPassionItem
            if (Player.ItemAnimationActive && Player.HeldItem.type == ModContent.ItemType<BurningPassion>() && Player.ownedProjectileCounts[ModContent.ProjectileType<BurningPassionP>()] > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
