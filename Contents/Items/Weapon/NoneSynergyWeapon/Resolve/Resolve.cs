using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.NoneSynergyWeapon.Resolve
{
    internal class Resolve : SynergyModItem
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
            CanShootItem = true;
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
            Projectile.usesIDStaticNPCImmunity = true;
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
}