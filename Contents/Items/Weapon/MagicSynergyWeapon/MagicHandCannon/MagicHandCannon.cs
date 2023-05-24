using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.MagicHandCannon
{
    internal class MagicHandCannon : ModItem, ISynergyItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultMagic(54, 32, 30, 5f, 30, 30, ItemUseStyleID.Shoot, ModContent.ProjectileType<MagicHandCannonProjectile>(), 10, 30, false);
            Item.scale = .75f;
        }
        public override void HoldItem(Player player)
        {
            for (int i = 0; i < 150; i++)
            {
                Vector2 SquarePosition = player.Center + Main.rand.NextVector2RectangleEdge(400, 400);
                int dust = Dust.NewDust(SquarePosition, 0, 0, DustID.Shadowflame, 0, 0, 0, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].noLight = true;
                Main.dust[dust].velocity = Vector2.Zero;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Handgun)
                .AddIngredient(ItemID.WaterBolt)
                .AddIngredient(ItemID.ShadowFlameHexDoll)
                .Register();
        }
    }
    class MagicHandCannonProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 11;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.wet = false;
            Projectile.timeLeft = 600;
            Projectile.light = 1f;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        bool isAlreadyOutX = false;
        bool isAlreadyOutY = false;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.direction == 1)
            {
                DrawOffsetX = -5;
            }
            else
            {
                DrawOffsetX = 0;
            }
            SelectFrame();
            if (!BossRushUtils.Vector2TouchLine(Projectile.Center.X, 400, player.Center.X))
            {
                if (!isAlreadyOutX)
                {
                    Projectile.velocity.X = -Projectile.velocity.X;
                    isAlreadyOutX = true;
                }
            }
            else
            {
                isAlreadyOutX = false;
            }
            if (!BossRushUtils.Vector2TouchLine(Projectile.Center.Y, 400, player.Center.Y))
            {
                if (!isAlreadyOutY)
                {
                    Projectile.velocity.Y = -Projectile.velocity.Y;
                    isAlreadyOutY = true;
                }
            }
            else
            {
                isAlreadyOutY = false;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.position += player.velocity;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 180);
        }
        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
            for (int i = 0; i < 20; i++)
            {
                int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2CircularEdge(5f, 5f), 0, 0, DustID.Shadowflame);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Vector2.Zero + Projectile.velocity + Main.rand.NextVector2CircularEdge(5f, 5f);
                Main.dust[dust].scale = Main.rand.NextFloat(1f, 1.5f);
            }
        }
        public void SelectFrame()
        {
            if (++Projectile.frameCounter >= 4)
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
}