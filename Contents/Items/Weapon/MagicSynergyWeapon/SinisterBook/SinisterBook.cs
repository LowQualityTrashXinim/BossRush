using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.SinisterBook
{
    internal class SinisterBook : SynergyModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultMagic(10, 10, 24, 2f, 9, 9, ItemUseStyleID.Shoot, ModContent.ProjectileType<SinisterBolt>(), 2.5f, 14, true);
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(platinum: 5);
            Item.UseSound = SoundID.Item8;
        }
        public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer)
        {
            base.ModifySynergyToolTips(ref tooltips, modplayer);
            if (modplayer.SinisterBook_DemonScythe)
                tooltips.Add(new TooltipLine(Mod, "SinisterBook_DemonScythe", $"[i:{ItemID.DemonScythe}] Where the sinister bolt explode will spawn a demon scythe that aim back to player"));
        }
        public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (player.HasItem(ItemID.DemonScythe))
            {
                modplayer.SinisterBook_DemonScythe = true;
                modplayer.SynergyBonus++;
            }
        }
        public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
        {
            base.SynergyShoot(player, modplayer, source, position, velocity, type, damage, knockback, out CanShootItem);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = position.PositionOFFSET(velocity, 30);
            for (int i = 0; i < Main.rand.Next(3, 5); i++)
            {
                velocity = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.Next(60, 90) * Main.rand.NextBool().BoolOne()));
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SinisterBolt>(), damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BookofSkulls)
                .AddIngredient(ItemID.WaterBolt)
                .Register();
        }
    }
    internal class SinisterBolt : SynergyModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.light = 0.8f;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        }
        Vector2 MousePosFixed;
        bool DirectionFace;
        int count = 0;
        int CountMain = 0;
        int CountCount = 0;
        bool CheckRotation;
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            if (CountMain == 0)
            {
                MousePosFixed = Main.MouseWorld;
                DirectionFace = player.direction == 1;
            }
            CountMain++;
            if (CountMain == 10)
                CheckRotation = Projectile.velocity.Y < 0;
            if (CountMain > 10)
                Projectile.velocity = !DirectionFace ? 
                    CheckRotation ? Projectile.velocity.RotatedBy(MathHelper.ToRadians(1f)) : Projectile.velocity.RotatedBy(MathHelper.ToRadians(-1f)) : 
                    CheckRotation ? Projectile.velocity.RotatedBy(MathHelper.ToRadians(-1f)) : Projectile.velocity.RotatedBy(MathHelper.ToRadians(1f));
            if (CountMain >= 120)
            {
                if (Math.Round(Projectile.velocity.X, 2) != 0 && Math.Round(Projectile.velocity.Y, 2) != 0 && count == 0)
                {
                    Projectile.velocity -= Projectile.velocity * 0.01f;
                }
                else
                {
                    count++;
                }
                if (count >= 12)
                {
                    CountCount++;
                    Projectile.velocity = (MousePosFixed - Projectile.Center).SafeNormalize(Vector2.UnitX) * CountCount * .033333f;
                    if (Vector2.Distance(MousePosFixed, Projectile.Center) <= 5)
                    {
                        Projectile.Kill();
                    }
                }
            }
        }
        public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<GhostHitBox>(), Projectile.damage, 0, Projectile.owner);
            for (int i = 0; i < 25; i++)
            {
                Vector2 Rotate = Main.rand.NextVector2Circular(9f, 9f);
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, Rotate.X, Rotate.Y, 0, default, Main.rand.NextFloat(0.75f, 1f));
                Main.dust[dustnumber].noGravity = true;
            }
            if (modplayer.SinisterBook_DemonScythe)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero), ProjectileID.DemonScythe, Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[proj].timeLeft = 100;
                Main.projectile[proj].usesLocalNPCImmunity = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrailWithoutColorAdjustment(lightColor, .02f);
            return true;
        }
    }
}