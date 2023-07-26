using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.NoneSynergyWeapon.Resolve
{
    internal class Resolve : SynergyModItem, ISynergyItem
    {
        public override void SetDefaults()
        {
            Item.BossRushSetDefault(34, 54, 20, 7f, 20, 20, ItemUseStyleID.Shoot, true);
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 5);
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<ResolveProjectile>();
            Item.shootSpeed = 30;
        }
        public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer)
        {
            base.ModifySynergyToolTips(ref tooltips, modplayer);
        }
        public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer)
        {
            base.HoldSynergyItem(player, modplayer);
        }
        public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = position.PositionOFFSET(velocity, 30);
        }
        public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ResolveBroadSword>()] < 1)
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ResolveBroadSword>(), damage, knockback, player.whoAmI);
            }

            base.SynergyShoot(player, modplayer, source, position, velocity * .01f, type, damage, knockback, out CanShootItem);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("OreShortSword")
                .AddRecipeGroup("OreBroadSword")
                .AddRecipeGroup("OreBow")
                .Register();
            base.AddRecipes();
        }
    }
    class ResolveProjectile : SynergyModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 14;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
        }
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            Projectile.alpha = (int)MathHelper.Lerp(0, 255, (180 - Projectile.timeLeft) / 180f);
            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity -= Projectile.velocity * .001f;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.position += Projectile.velocity;
                Projectile.velocity = Vector2.Zero;
            }
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return base.GetAlpha(lightColor);
        }
        public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (int)Math.Clamp(Projectile.damage - Projectile.damage * .1f, 1, Projectile.damage);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrail(lightColor);
            return base.PreDraw(ref lightColor);
        }
    }
    class ResolveBroadSword : SynergyModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.CopperBroadsword);
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 999;
        }
        int MouseXPosDirection;
        float MaxLengthX = 0;
        float MaxLengthY = 0;
        public override void SynergyPreAI(Player player, PlayerSynergyItemHandle modplayer, out bool runAI)
        {
            if (Projectile.timeLeft == 999)
            {
                MouseXPosDirection = Main.MouseWorld.X - player.Center.X > 0 ? 1 : -1;
                MaxLengthX = (Main.MouseWorld - player.Center).Length();
                MaxLengthY = -MaxLengthX * .25f * MouseXPosDirection;
            }
            base.SynergyPreAI(player, modplayer, out runAI);
        }
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            int maxProgress = 50;
            if (Projectile.timeLeft > maxProgress)
            {
                Projectile.timeLeft = maxProgress;
            }
            int halfmaxProgress = (int)(maxProgress * .5f);
            int quadmaxProgress = (int)(maxProgress * .25f);
            float progress;
            if (Projectile.timeLeft > halfmaxProgress)
            {
                progress = (maxProgress - Projectile.timeLeft) / (float)halfmaxProgress;
            }
            else
            {
                progress = Projectile.timeLeft / (float)halfmaxProgress;
            }
            float X = MathHelper.SmoothStep(-60, MaxLengthX, progress);
            ProgressYHandle(Projectile.timeLeft, halfmaxProgress, quadmaxProgress, out float Y);
            Vector2 VelocityPosition = new Vector2(X, Y).RotatedBy(Projectile.velocity.ToRotation());
            Projectile.Center = player.Center + VelocityPosition;
            float rotation = MathHelper.SmoothStep(0, 360, 1 - Projectile.timeLeft / (float)maxProgress) * MouseXPosDirection;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4 + MathHelper.Pi + MathHelper.ToRadians(rotation);
        }
        private void ProgressYHandle(int timeleft, float progressMaxHalf, float progressMaxQuad, out float Y)
        {
            if (timeleft > progressMaxHalf + progressMaxQuad)
            {
                float progressY = 1 - (timeleft - (progressMaxHalf + progressMaxQuad)) / progressMaxQuad;
                Y = MathHelper.SmoothStep(0, MaxLengthY, progressY);
                return;
            }
            if (timeleft > progressMaxQuad)
            {
                float progressY = 1 - (timeleft - progressMaxQuad) / progressMaxHalf;
                Y = MathHelper.SmoothStep(MaxLengthY, -MaxLengthY, progressY);
                return;
            }
            else
            {
                float progressY = 1 - timeleft / progressMaxQuad;
                Y = MathHelper.SmoothStep(-MaxLengthY, 0, progressY);
                return;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrail(lightColor);
            return base.PreDraw(ref lightColor);
        }
    }
}