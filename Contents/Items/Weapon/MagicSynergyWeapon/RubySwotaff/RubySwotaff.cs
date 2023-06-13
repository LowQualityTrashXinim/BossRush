using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.RubySwotaff
{
    internal class RubySwotaff : ModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Really Fancy Sword and staff");
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 58;

            Item.damage = 32;
            Item.crit = 10;
            Item.knockBack = 3f;

            Item.useTime = 1;
            Item.useAnimation = 10;
            Item.reuseDelay = 20;

            Item.shootSpeed = 7;
            Item.mana = 20;

            Item.value = Item.buyPrice(gold: 50);
            Item.shoot = ProjectileID.RubyBolt;
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
                mult = 7.5f;
            }
        }
        public override bool AltFunctionUse(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<GiantRubyBolt>()] < 1;
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
                    Projectile.NewProjectile(source, position, velocity * 5, ModContent.ProjectileType<GiantRubyBolt>(), damage, knockback, player.whoAmI);
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
                .AddIngredient(ItemID.RubyStaff)
                .AddIngredient(ItemID.GoldBroadsword)
                .Register();
        }
    }
    internal class GiantRubyBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Magic;
        }
        float count = 0;
        int counttime = 0;
        int permaCount = 0;
        public override void AI()
        {
            SelectFrame();
            Player player = Main.player[Projectile.owner];
            if (player.velocity != Vector2.Zero)
            {
                Projectile.timeLeft--;
            }
            Projectile.velocity -= Projectile.velocity * 0.1f;
            EntitySource_ItemUse source = new EntitySource_ItemUse(player, new Item(ModContent.ItemType<RubySwotaff>()));
            if (Projectile.velocity.X < 1 && Projectile.velocity.X > -1 && Projectile.velocity.Y < 1 && Projectile.velocity.Y > -1)
            {
                Projectile.velocity = Vector2.Zero;
                for (int i = 0; i < 30; i++)
                {
                    Vector2 Rotate = Main.rand.NextVector2CircularEdge(5f, 5f);
                    int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, Rotate.X, Rotate.Y, 100, default, Main.rand.NextFloat(0.75f, 2f));
                    Main.dust[dustnumber].noGravity = true;
                }
            }
            if (Projectile.velocity == Vector2.Zero)
            {
                if (permaCount == 0)
                {
                    for (int i = 0; i < 150; i++)
                    {
                        Vector2 Rotate = Main.rand.NextVector2CircularEdge(20f, 20f);
                        int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, Rotate.X, Rotate.Y, default, default, Main.rand.NextFloat(1.5f, 2.5f));
                        Main.dust[dustnumber].noGravity = true;
                        permaCount++;
                    }
                }
                Projectile.ai[0]++;
                if (Projectile.ai[0] >= 10)
                {
                    Projectile.netUpdate = true;
                    for (int i = 0; i < 12; i++)
                    {
                        Vector2 Rotate = Main.rand.NextVector2Circular(5f, 5f);
                        int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, Rotate.X, Rotate.Y, 100, default, Main.rand.NextFloat(0.75f, 2f));
                        Main.dust[dustnumber].noGravity = true;
                    }
                    if (counttime == 0)
                    {
                        count += 2f;
                        for (int i = 0; i < 4; i++)
                        {
                            Projectile.NewProjectile(source, Projectile.Center, Vector2.One.RotatedBy(MathHelper.ToRadians(count + i * 90)) * 5, ModContent.ProjectileType<SmallerRubyBolt>(), (int)(Projectile.damage * 0.35f), 0, Projectile.owner);
                            Projectile.NewProjectile(source, Projectile.Center, Vector2.One.RotatedBy(MathHelper.ToRadians(-count + i * 90)) * 5, ModContent.ProjectileType<SmallerRubyBolt>(), (int)(Projectile.damage * 0.35f), 0, Projectile.owner);
                        }
                    }
                    counttime++;
                    if (counttime >= 1)
                    {
                        counttime = 0;
                    }
                }
            }
        }

        public void SelectFrame()
        {
            if (++Projectile.frameCounter >= 10)
            {
                Projectile.frameCounter = 0;
                Projectile.frame += 1;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }
    }
    internal class SmallerRubyBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.timeLeft = 50;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
        }
        int count = 0;
        public override void AI()
        {
            Projectile.alpha += 5;
            Projectile.scale -= 0.015f;
            if (count >= 3)
            {
                for (int i = 0; i < 2; i++)
                {
                    int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, -Projectile.velocity.X + Main.rand.Next(-5, 5), -Projectile.velocity.Y + Main.rand.Next(-5, 5), 100, default, Projectile.scale);
                    Main.dust[dustnumber].noGravity = true;
                }
                count = 0;
            }
            count++;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 1;
        }
    }
}
