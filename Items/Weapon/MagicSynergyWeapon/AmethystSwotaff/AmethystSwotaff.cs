using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;

namespace BossRush.Items.Weapon.MagicSynergyWeapon.AmethystSwotaff
{
    internal class AmethystSwotaff : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("You know, if this is all what it do\nthen it would be pretty disappointing\nluckily, you can yeet the thing");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.BossRushDefaultMagic(60, 58, 20, 3f, 4, 20, ItemUseStyleID.Shoot, ProjectileID.AmethystBolt, 7, 30, false);
            Item.crit = 10;
            Item.reuseDelay = 20;
            Item.value = Item.buyPrice(gold: 50);
            Item.useTurn = false;
            Item.rare = 2;
            Item.UseSound = SoundID.Item8;
        }

        int i = 0;
        int countChange = 0;

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            if (player.altFunctionUse == 2)
            {
                mult = 2.5f;
            }
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<AmethystSwotaffP>()] < 1;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                Item.noUseGraphic = false;
                i++;
                float rotation = MathHelper.ToRadians(30);
                if (countChange == 0)
                {
                    velocity = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)(i / 9f)));
                }
                else
                {
                    velocity = velocity.RotatedBy(MathHelper.Lerp(rotation, -rotation, (float)(i / 9f)));
                }
                if (i > 9)
                {
                    i = 0;
                    countChange++;
                    if (countChange > 1)
                    {
                        countChange = 0;
                    }
                }
                Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 50f;
                if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                {
                    position += muzzleOffset;
                }
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                if (player.ItemAnimationJustStarted)
                {
                    Item.noUseGraphic = true;
                    Projectile.NewProjectile(source, position, velocity * 4, ModContent.ProjectileType<AmethystSwotaffP>(), damage, knockback, player.whoAmI);
                }
                return false;
            }
            else
            {
                return true;
            }

        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CopperBroadsword)
                .AddIngredient(ItemID.AmethystStaff)
                .Register();
        }
    }
    public class AmethystSwotaffP : ModProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsItem<AmethystSwotaff>();
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 58;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.DamageType = DamageClass.Magic;
        }
        int count = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation += 0.25f;
            float offSetRotate = Projectile.rotation - MathHelper.PiOver4;
            if (Projectile.velocity.X != 0)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + offSetRotate.ToRotationVector2() * 30, Projectile.rotation.ToRotationVector2() * 9, ProjectileID.AmethystBolt, (int)(Projectile.damage * 0.67f), Projectile.knockBack * 0.5f, Projectile.owner);
                Main.projectile[proj].timeLeft = 30;
            }
            if (Projectile.timeLeft < 10)
            {
                Vector2 GoBack = player.Center - Projectile.position;
                Vector2 SafeGoBack = GoBack.SafeNormalize(Vector2.UnitY);

                if (count >= 30)
                {
                    Projectile.velocity = SafeGoBack * 20f;
                }
                Projectile.timeLeft = 8;
                Projectile.velocity += SafeGoBack * 2f;

                float distance = 60;
                Vector2 newMove = player.Center - Projectile.Center;
                float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                if (distanceTo < distance)
                {
                    Projectile.Kill();
                }
                count++;
            }
            Projectile.velocity = Projectile.velocity.LimitedVelocity(20);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 6;
        }
    }
}
