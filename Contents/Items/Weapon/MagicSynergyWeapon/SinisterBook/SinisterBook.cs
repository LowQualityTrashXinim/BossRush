using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.SinisterBook
{
    internal class SinisterBook : ModItem, ISynergyItem
    {
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;

            Item.damage = 43;
            Item.knockBack = 1f;
            Item.mana = 7;

            Item.useTime = 9;
            Item.useAnimation = 9;

            Item.shoot = ModContent.ProjectileType<SinisterBolt>();
            Item.shootSpeed = 2.5f;

            Item.autoReuse = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(platinum: 5);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;

            Item.UseSound = SoundID.Item8;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = position.PositionOFFSET(velocity, 30);
            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                velocity = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextBool(2) ? Main.rand.Next(40, 90) : -Main.rand.Next(40, 90)));
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
    internal class SinisterBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.light = 0.8f;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        }
        Vector2 MousePosFixed;
        bool DirectionFace;
        int count = 0;
        int CountMain = 0;
        int CountCount = 0;
        bool CheckRotation;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (CountMain == 0)
            {
                MousePosFixed = Main.MouseWorld;
                DirectionFace = player.direction == 1;
            }
            CountMain++;
            if (CountMain == 10)
            {
                CheckRotation = Projectile.velocity.Y < 0;
            }
            if (CountMain > 10) Projectile.velocity = !DirectionFace ? CheckRotation ? Projectile.velocity.RotatedBy(MathHelper.ToRadians(1f)) : Projectile.velocity.RotatedBy(MathHelper.ToRadians(-1f)) : CheckRotation ? Projectile.velocity.RotatedBy(MathHelper.ToRadians(-1f)) : Projectile.velocity.RotatedBy(MathHelper.ToRadians(1f));
            if (CountMain >= 60)
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
        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<GhostHitBox>(), Projectile.damage, 0, Projectile.owner);
            for (int i = 0; i < 25; i++)
            {
                Vector2 Rotate = Main.rand.NextVector2Circular(9f, 9f);
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, Rotate.X, Rotate.Y, 0, default, Main.rand.NextFloat(0.75f, 1f));
                Main.dust[dustnumber].noGravity = true;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawTrail(lightColor, .5f);
            return true;
        }
    }
}
