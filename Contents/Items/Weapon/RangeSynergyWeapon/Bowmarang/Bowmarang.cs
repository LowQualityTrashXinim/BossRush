using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Bowmarang
{
    internal class Bowmarang : SynergyModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(32, 64, 15, 3f, 30, 30, ItemUseStyleID.Shoot, ModContent.ProjectileType<BowmarangP>(), 20f, false);
            Item.crit = 10;
            Item.noUseGraphic = true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<BowmarangP>()] < 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WoodenBoomerang)
                .AddRecipeGroup("Wood Bow")
                .Register();
        }
    }
    public class BowmarangP : SynergyModProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<Bowmarang>();
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.DamageType = DamageClass.Melee;
        }
        int count = 0;
        int count2 = 0;
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            EntitySource_ItemUse source = new EntitySource_ItemUse(player, new Item(ModContent.ItemType<Bowmarang>()));
            Projectile.rotation += 0.4f;
            float offSetRotate = Projectile.rotation - MathHelper.PiOver4;

            Vector2 aimto = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.UnitX);
            if (count2 % 10 == 0)
            {
                if (Projectile.velocity != Vector2.Zero)
                {
                    Projectile.NewProjectile(source, Projectile.Center + offSetRotate.ToRotationVector2(), aimto * 9, ProjectileID.WoodenArrowFriendly, Projectile.damage, Projectile.knockBack * 0.5f, Projectile.owner);
                }
            }

            if (Projectile.timeLeft < 3)
            {
                Vector2 GoBack = player.Center - Projectile.position;
                Vector2 SafeGoBack = GoBack.SafeNormalize(Vector2.UnitY);
                if (count >= 30)
                {
                    Projectile.velocity = SafeGoBack * 20f;
                }
                Projectile.timeLeft = 2;
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
            count2++;
            Projectile.velocity = Projectile.velocity.LimitedVelocity(20);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 6;
        }
    }
}