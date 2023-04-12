using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.MagicSynergyWeapon.DiamondSwotaff
{
    internal class DiamondSwotaff : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("ya know, despite it being a stupid design idea, it working quite well");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 58;

            Item.damage = 17;
            Item.crit = 10;
            Item.knockBack = 3f;

            Item.useTime = 1;
            Item.useAnimation = 10;
            Item.reuseDelay = 20;

            Item.shootSpeed = 7;
            Item.mana = 20;

            Item.value = Item.buyPrice(gold: 50);
            Item.shoot = ProjectileID.DiamondBolt;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
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
                mult = 8.5f;
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<DiamondSwotaffOrb>()] < 1;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse != 2)
            {
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
                    Projectile.NewProjectile(source, position, velocity * 5, ModContent.ProjectileType<DiamondSwotaffOrb>(), damage, knockback, player.whoAmI);
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
                .AddIngredient(ItemID.PlatinumBroadsword)
                .AddIngredient(ItemID.DiamondStaff)
                .Register();
        }
    }
    internal class DiamondSwotaffOrb : ModProjectile
    {
        float rotateto = 0;
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 400;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.velocity -= Projectile.velocity * 0.05f;
            }
            if (Projectile.velocity.X < 1 && Projectile.velocity.X > -1 && Projectile.velocity.Y < 1 && Projectile.velocity.Y > -1)
            {
                for (int i = 0; i < 30; i++)
                {
                    Vector2 Rotate = Main.rand.NextVector2CircularEdge(5f, 5f);
                    int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, Rotate.X, Rotate.Y, 100, default, Main.rand.NextFloat(0.75f, 2f));
                    Main.dust[dustnumber].noGravity = true;
                }
                var source = new EntitySource_ItemUse(player, new Item(ModContent.ItemType<DiamondSwotaff>()));
                Projectile.velocity = Vector2.Zero;
                Projectile.ai[0]++;
                if (Projectile.ai[0] >= 30)
                {
                    Projectile.netUpdate = true;
                    for (int i = 0; i < 10; i++)
                    {
                        Vector2 RandomRotatePos = Projectile.Center + Main.rand.NextVector2CircularEdge(330f, 330f);
                        Vector2 velocityCustom = (Projectile.Center - RandomRotatePos).SafeNormalize(Vector2.UnitX) * 17;
                        int dustnumber = Dust.NewDust(RandomRotatePos, 0, 0, DustID.GemDiamond, velocityCustom.X, velocityCustom.Y, default, default, Main.rand.NextFloat(1.5f, 2.5f));
                        Main.dust[dustnumber].noGravity = true;
                    }
                    for (int i = 0; i < 25; i++)
                    {
                        Vector2 RandomRotatePos = Projectile.Center + Main.rand.NextVector2CircularEdge(330f, 330f);
                        int dustnumber = Dust.NewDust(RandomRotatePos, 0, 0, DustID.GemDiamond, 0, 0, default, default, Main.rand.NextFloat(1.5f, 2.5f));
                        Main.dust[dustnumber].noGravity = true;
                    }
                    Vector2 newPos = Projectile.Center + (Projectile.Center + Vector2.One).SafeNormalize(Vector2.UnitX) * 300;
                    Vector2 newVelo = (Projectile.Center - newPos.RotatedBy(MathHelper.ToRadians(rotateto), Projectile.Center)).SafeNormalize(Vector2.UnitX);
                    Vector2 newVelo2 = (Projectile.Center - newPos.RotatedBy(MathHelper.ToRadians(rotateto + 90), Projectile.Center)).SafeNormalize(Vector2.UnitX);
                    Vector2 newVelo3 = (Projectile.Center - newPos.RotatedBy(MathHelper.ToRadians(rotateto + 180), Projectile.Center)).SafeNormalize(Vector2.UnitX);
                    Vector2 newVelo4 = (Projectile.Center - newPos.RotatedBy(MathHelper.ToRadians(rotateto + 270), Projectile.Center)).SafeNormalize(Vector2.UnitX);
                    Projectile.NewProjectile(source, newPos.RotatedBy(MathHelper.ToRadians(rotateto), Projectile.Center), newVelo * 5, ModContent.ProjectileType<DiamondBoltSpecial>(), (int)(Projectile.damage * 0.35), 0, Projectile.owner);
                    Projectile.NewProjectile(source, newPos.RotatedBy(MathHelper.ToRadians(rotateto + 90), Projectile.Center), newVelo2 * 5, ModContent.ProjectileType<DiamondBoltSpecial>(), (int)(Projectile.damage * 0.35), 0, Projectile.owner);
                    Projectile.NewProjectile(source, newPos.RotatedBy(MathHelper.ToRadians(rotateto + 180), Projectile.Center), newVelo3 * 5, ModContent.ProjectileType<DiamondBoltSpecial>(), (int)(Projectile.damage * 0.35), 0, Projectile.owner);
                    Projectile.NewProjectile(source, newPos.RotatedBy(MathHelper.ToRadians(rotateto + 270), Projectile.Center), newVelo4 * 5, ModContent.ProjectileType<DiamondBoltSpecial>(), (int)(Projectile.damage * 0.35), 0, Projectile.owner);
                    rotateto += 5;
                }
            }
        }
    }
    internal class DiamondBoltSpecial : ModProjectile
    {
        public override string Texture => "BossRush/Items/Weapon/MagicSynergyWeapon/DiamondSwotaff/DiamondSwotaffOrb";
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.timeLeft = 70;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.alpha = 255;
        }
        int count = 0;
        public override void AI()
        {
            if (Projectile.timeLeft > 55)
            {
                Projectile.alpha -= 10;
            }
            else
            {
                Projectile.alpha += 5;
                Projectile.scale -= 0.025f;
            }
            if (count >= 3)
            {
                for (int i = 0; i < 2; i++)
                {
                    int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, 0, 0, 100, default, Projectile.scale);
                    Main.dust[dustnumber].noGravity = true;
                }
                count = 0;
            }
            count++;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 1;
        }
    }
}
