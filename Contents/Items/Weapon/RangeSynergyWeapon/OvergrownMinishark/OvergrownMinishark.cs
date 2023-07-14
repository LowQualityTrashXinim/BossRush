using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Global;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.OvergrownMinishark
{
    internal class OvergrownMinishark : SynergyModItem, ISynergyItem
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
        public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer)
        {
            if (modplayer.OvergrownMinishark_CrimsonRod)
            {
                tooltips.Add(new TooltipLine(Mod, "OvergrownMinishark_CrimsonRod", $"[i:{ItemID.CrimsonRod}] When shooting, you summon blood rain at cursor"));
            }
        }
        public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (player.HasItem(ItemID.CrimsonRod))
            {
                modplayer.OvergrownMinishark_CrimsonRod = true;
            }
            if (player.HasItem(ItemID.PlatinumShortsword))
            {
                modplayer.OvergrownMinishark_PlatinumShortSword = true;
            }
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
        public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
        {
            if (modplayer.OvergrownMinishark_CrimsonRod)
            {
                Vector2 pos = Main.MouseWorld + new Vector2(Main.rand.NextFloat(-30, 30), -1000);
                int proj = Projectile.NewProjectile(source, pos, Vector2.UnitY * Main.rand.NextFloat(9, 11), ProjectileID.BloodRain, damage, knockback, player.whoAmI);
                Main.projectile[proj].penetrate = 1;
            }
            CanShootItem = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Minishark)
                .AddIngredient(ItemID.Vilethorn)
                .Register();
        }
    }
    class OvergrownMiniSharkPlatinumShortSwordProjectile : SynergyModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.PlatinumShortsword);
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
        }
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            base.SynergyAI(player, modplayer);
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
            if (Main.rand.NextBool(10))
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