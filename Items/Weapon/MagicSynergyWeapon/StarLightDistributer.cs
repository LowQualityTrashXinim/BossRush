using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Items.Weapon.MagicSynergyWeapon
{
    internal class StarLightDistributer : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("StarLight Distributer");
            Tooltip.SetDefault("It is beautiful to look at");
        }
        public override void SetDefaults()
        {
            Item.width = 45;
            Item.height = 24;

            Item.damage = 16;
            Item.knockBack = 2f;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.mana = 8;
            Item.shootSpeed = 10;

            Item.noMelee = true;
            Item.shoot = ProjectileID.GreenLaser;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.rare = 3;
            Item.value = Item.buyPrice(gold: 50);

            Item.UseSound = SoundID.Item12;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.player[Main.myPlayer];
            if (player.GetModPlayer<StarLightDistributerPlayer>().StarLightDistributer)
            {
                tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.MeteorHelmet}][i:{ItemID.MeteorSuit}][i:{ItemID.MeteorLeggings}]Attack now cost 0 mana"));
            }
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float rotation = MathHelper.ToRadians(5);
            float num = 5;
            if (player.ZoneNormalSpace)
            {
                if (!Main.dayTime)
                {
                    num += 5;
                    for (int i = 0; i < num; i++)
                    {
                        Vector2 rotate = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (num - 1)));
                        Projectile.NewProjectile(source, position, rotate, ProjectileID.GreenLaser, (int)(damage * 1.25f), knockback, player.whoAmI);
                    }
                }
                else
                {
                    for (int i = 0; i < num; i++)
                    {
                        Vector2 rotate = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (num - 1)));
                        Projectile.NewProjectile(source, position, rotate, ProjectileID.GreenLaser, (int)(damage * 1.25f), knockback, player.whoAmI);
                    }
                }
            }

            if (Main.dayTime)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.ThunderStaffShot, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ProjectileID.GreenLaser, damage, knockback, player.whoAmI);
            }
            else if (player.ZoneNormalSpace)
            {
                num += 5;
                for (int i = 0; i < num; i++)
                {
                    Vector2 rotate = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (num - 1)));
                    Projectile.NewProjectile(source, position, rotate * 1.5f, ProjectileID.ThunderStaffShot, (int)(damage * 1.25f), knockback, player.whoAmI);
                }
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.GreenLaser, damage, knockback, player.whoAmI);
                for (int i = 0; i < num; i++)
                {
                    Vector2 rotate = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(rotation, -rotation, i / (num - 1)));
                    Projectile.NewProjectile(source, position, rotate, ProjectileID.ThunderStaffShot, (int)(damage * 1.25f), knockback, player.whoAmI);
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SynergyEnergy>())
                .AddIngredient(ItemID.SpaceGun)
                .AddIngredient(ItemID.ThunderStaff)
                .Register();
        }
    }
    class StarLightDistributerPlayer : ModPlayer
    {
        public bool StarLightDistributer;
        public override void ResetEffects()
        {
            StarLightDistributer = false;
        }

        public override void UpdateEquips()
        {
            if (Player.head == 6 && Player.body == 6 && Player.legs == 6)
            {
                StarLightDistributer = true;
            }
        }
        public override void ModifyManaCost(Item item, ref float reduce, ref float mult)
        {
            if (StarLightDistributer && item.type == ModContent.ItemType<StarLightDistributer>())
            {
                mult = 0f;
            }
        }
    }
}
