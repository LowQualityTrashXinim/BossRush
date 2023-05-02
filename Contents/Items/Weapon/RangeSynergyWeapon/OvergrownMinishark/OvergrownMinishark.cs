﻿using BossRush.Common.Global;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.OvergrownMinishark
{
    internal class OvergrownMinishark : ModItem, ISynergyItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(54, 24, 24, 2f, 11, 11, ItemUseStyleID.Shoot, ProjectileID.Bullet, 15, true, AmmoID.Bullet);

            Item.rare = 2;
            Item.value = Item.sellPrice(gold: 50);
            Item.UseSound = SoundID.Item11;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 0);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 offset = velocity.SafeNormalize(Vector2.UnitX) * 40;
            if (Collision.CanHit(position, 0, 0, position * offset, 0, 0))
            {
                position += offset;
            }
            velocity = velocity.RotateRandom(7);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Minishark)
                .AddIngredient(ItemID.Vilethorn)
                .Register();
        }
    }
    class OvergrownMinisharkProjectileModify : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo entity && entity.Item.ModItem is OvergrownMinishark)
            {
                IsTruelyFromSource = true;
            }
        }
        public override bool InstancePerEntity => true;
        bool IsTruelyFromSource = false;
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!IsTruelyFromSource)
            {
                return;
            }
            target.AddBuff(BuffID.Poisoned, 420);
            if(Main.rand.NextBool(10))
            {
                float randomRotation = Main.rand.Next(90);
                Vector2 velocity;
                for (int i = 0; i < 6; i++)
                {
                    velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(i * 60 + randomRotation));
                    Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, velocity, ProjectileID.VilethornTip, hit.Damage, hit.Knockback, projectile.owner);
                    Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, velocity, ProjectileID.VilethornBase, hit.Damage, hit.Knockback, projectile.owner);
                }
            }
        }
    }
}
