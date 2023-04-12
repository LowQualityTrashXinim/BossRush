using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.SapphireSwotaff
{
    internal class SapphireSwotaff : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("so it is just a better staff but with a sword as tip ?");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 58;

            Item.damage = 15;
            Item.crit = 10;
            Item.knockBack = 3f;
            Item.useTime = 2;
            Item.useAnimation = 20;
            Item.reuseDelay = 20;
            Item.shoot = ProjectileID.SapphireBolt;
            Item.shootSpeed = 5;
            Item.mana = 18;

            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.rare = 2;
            Item.value = Item.buyPrice(gold: 50);

            Item.UseSound = SoundID.Item8;
        }
        int i = 0;
        int counterChooser = 0;

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            if (player.altFunctionUse == 2)
            {
                mult = 5.75f;
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                if (player.ItemAnimationJustStarted)
                {
                    Item.noUseGraphic = true;
                    Projectile.NewProjectile(source, position, velocity * 4, ModContent.ProjectileType<SapphireSwotaffP>(), damage * 8, knockback, player.whoAmI);
                }
            }
            else
            {
                Item.noUseGraphic = false;
                Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 50f;
                if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                {
                    position += muzzleOffset;
                }
                float rotation = MathHelper.ToRadians(10);
                Vector2 Rotate;
                Vector2 Rotate2;
                if (counterChooser == 0)
                {
                    Rotate = velocity.RotatedBy(MathHelper.Lerp(0, -rotation, i / 9f));
                    Rotate2 = velocity.RotatedBy(MathHelper.Lerp(0, rotation, i / 9f));
                    Projectile.NewProjectile(source, position, Rotate, type, damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(source, position, Rotate2, type, damage, knockback, player.whoAmI);
                }
                else
                {
                    Rotate = velocity.RotatedBy(MathHelper.Lerp(rotation, -rotation, i / 9f));
                    Rotate2 = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / 9f));
                    Projectile.NewProjectile(source, position, Rotate * (i * 0.1f + 1), type, damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(source, position, Rotate2 * (i * 0.1f + 1), type, damage, knockback, player.whoAmI);
                }
                i++;
                if (i > 4)
                {
                    i = 0;
                    counterChooser++;
                    if (counterChooser > 1)
                    {
                        counterChooser = 0;
                    }
                }
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SilverBroadsword)
                .AddIngredient(ItemID.SapphireStaff)
                .Register();
        }
    }

    public class SapphireSwotaffP : ModProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<SapphireSwotaff>();
        public override void SetDefaults()
        {
            Projectile.height = 29;
            Projectile.width = 30;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 900;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            if (Projectile.ai[0] >= 20)
            {
                if (Projectile.velocity.Y <= 16)
                {
                    Projectile.velocity.Y += .2f;
                }
            }
            for (int i = 0; i < 2; i++)
            {
                Vector2 Velocity180 = Projectile.velocity.RotatedBy(MathHelper.ToRadians(90) + MathHelper.Pi * i);
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Velocity180 * .5f, ProjectileID.SapphireBolt, (int)(Projectile.damage * .3f), Projectile.knockBack, Projectile.owner);
                Main.projectile[proj].timeLeft = 30;
            }
        }
    }
}
