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
            Item.BossRushSetDefault(34, 54, 16, 7f, 20, 20, ItemUseStyleID.Shoot, false);
            Item.DamageType = DamageClass.Generic;
            Item.crit = 15;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 5);
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<ResolveProjectile>();
            Item.shootSpeed = 20;
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
            type = Main.rand.Next(new int[] { ModContent.ProjectileType<GhostBroadsword>(), ModContent.ProjectileType<GhostShortsword>(), ModContent.ProjectileType<ResolveGhostArrow>(), ModContent.ProjectileType<ResolveProjectile>() });
            if (type == ModContent.ProjectileType<GhostShortsword>())
                damage *= 2;
            velocity = velocity.Vector2RotateByRandom(5);
        }
        int counter = 0;
        public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
        {
            counter++;
            if (counter >= 15)
            {
                int projAmount = 4;
                for (int i = 0; i < projAmount; i++)
                {
                    Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenly(projAmount, 40, i), type, damage, knockback, player.whoAmI);
                }
                counter = 0;
                CanShootItem = false;
                return;
            }
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
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 90;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.usesIDStaticNPCImmunity = true;
        }
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            Projectile.alpha = (int)MathHelper.Lerp(0, 255, (90 - Projectile.timeLeft) / 90f);
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
    class ResolveGhostArrow : SynergyModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.WoodenArrow);
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 32;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
        }
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemDiamond);
            Main.dust[dust].noGravity = true;
            Projectile.alpha = (int)MathHelper.Lerp(0, 255, (180 - Projectile.timeLeft) / 180f);
            Projectile.ai[0]++;
            if (Projectile.ai[0] <= 20)
                return;
            if (Projectile.Center.LookForHostileNPC(out NPC npc, 600))
            {
                Vector2 distance = npc.Center - Projectile.Center;
                float length = distance.Length();
                if (length > 5)
                {
                    length = 5;
                }
                Projectile.velocity -= Projectile.velocity * .08f;
                Projectile.velocity += distance.SafeNormalize(Vector2.Zero) * length;
                Projectile.velocity = Projectile.velocity.LimitedVelocity(20);
                return;
            }
            Projectile.ai[1]++;
            if (Projectile.ai[1] > 20)
                Projectile.velocity.Y += .25f;
        }
        public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPCSynergy(player, modplayer, npc, hit, damageDone);
        }
    }
    class GhostBroadsword : SynergyModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 36;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.usesIDStaticNPCImmunity = true;
        }
        public int ProjDirection = 0;
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            ProjDirection = Projectile.velocity.X > 0 ? 1 : -1;
            int dust = Dust.NewDust(Projectile.Center + Projectile.rotation.ToRotationVector2() * 15, 0, 0, DustID.GemDiamond);
            Main.dust[dust].noGravity = true;
            Projectile.alpha = (int)MathHelper.Lerp(0, 255, (180 - Projectile.timeLeft) / 180f);
            Projectile.rotation += MathHelper.ToRadians(20) * ProjDirection;
        }
        public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPCSynergy(player, modplayer, npc, hit, damageDone);
            npc.immune[Projectile.owner] = 3;
        }
    }
    class GhostShortsword : SynergyModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.usesIDStaticNPCImmunity = true;
        }
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemDiamond);
            Main.dust[dust].noGravity = true;
            Projectile.alpha = (int)MathHelper.Lerp(0, 255, (180 - Projectile.timeLeft) / 180f);
        }
        public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (int)Math.Clamp(Projectile.damage - Projectile.damage * .1f, 1, Projectile.damage);
        }
    }
}