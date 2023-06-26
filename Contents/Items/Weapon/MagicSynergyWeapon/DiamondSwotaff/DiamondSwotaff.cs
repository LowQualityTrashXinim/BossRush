using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.DiamondSwotaff
{
    internal class DiamondSwotaff : SynergyModItem, ISynergyItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("ya know, despite it being a stupid design idea, it working quite well");
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.BossRushDefaultMagic(60, 58, 17, 3f, 1, 10, ItemUseStyleID.Shoot, ProjectileID.DiamondBolt, 7, 20, true);
            Item.rare = 2;
            Item.crit = 10;
            Item.reuseDelay = 20;
            Item.useTurn = false;
            Item.UseSound = SoundID.Item8;
            Item.value = Item.buyPrice(gold: 50);
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<DiamondSwotaffP>()] < 1;
        }
        public override void OnConsumeMana(Player player, int manaConsumed)
        {
            if (player.altFunctionUse == 2)
            {
                player.statMana += manaConsumed;
            }
        }
        public override void OnMissingMana(Player player, int neededMana)
        {
            if (player.statMana <= player.GetManaCost(Item))
            {
                CanShootProjectile = -1;
            }
            player.statMana += neededMana;
        }
        public override bool AltFunctionUse(Player player)
        {
            CanShootProjectile = -1;
            return true;
        }
        int CanShootProjectile = 1;
        int countIndex = 1;
        int time = 1;
        public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
        {
            if (player.statMana >= player.GetManaCost(Item) && player.altFunctionUse != 2)
            {
                CanShootProjectile = 1;
            }
            Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI, countIndex, CanShootProjectile);
            if (CanShootProjectile == 1)
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileID.DiamondBolt, damage, knockback, player.whoAmI);
            }
            if (player.altFunctionUse != 2)
            {
                SwingComboHandle();
            }
            CanShootItem = false;
        }
        private void SwingComboHandle()
        {
            countIndex = countIndex != 0 ? countIndex * -1 : 1;
            time++;
            if (time >= 3)
            {
                countIndex = 0;
                time = 0;
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
            if (player.velocity != Vector2.Zero)
            {
                Projectile.timeLeft--;
            }
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
        public override string Texture => BossRushUtils.GetTheSameTextureAs<DiamondSwotaff>("DiamondSwotaffOrb");
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 1;
        }
    }
    public class DiamondSwotaffP : SwotaffProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<DiamondSwotaff>();
        protected override int AltAttackProjectileType() => ModContent.ProjectileType<DiamondGemP>();
        protected override int NormalBoltProjectile() => ProjectileID.DiamondBolt;
        protected override int DustType() => DustID.GemDiamond;
    }
}
