using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.MagicHandCannon
{
    internal class MagicHandCannon : SynergyModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultMagic(54, 32, 30, 5f, 30, 30, ItemUseStyleID.Shoot, ModContent.ProjectileType<MagicHandCannonProjectile>(), 20, 30, false);
            Item.scale = .75f;
        }
        public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
        {
            CanShootItem = true;
            for (int i = 0; i < 30; i++)
            {
                int dust = Dust.NewDust(position.PositionOFFSET(velocity, 60), 0, 0, DustID.Shadowflame);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].position += Main.rand.NextVector2CircularEdge(10, 3.5f).RotatedBy(velocity.ToRotation() + MathHelper.PiOver2) * 2;
                Main.dust[dust].velocity = velocity * .2f;
                Main.dust[dust].fadeIn = 1f;
                int dust1 = Dust.NewDust(position.PositionOFFSET(velocity, 50), 0, 0, DustID.Shadowflame);
                Main.dust[dust1].noGravity = true;
                Main.dust[dust1].position += Main.rand.NextVector2CircularEdge(12.5f, 4.5f).RotatedBy(velocity.ToRotation() + MathHelper.PiOver2) * 2;
                Main.dust[dust1].velocity = velocity * .2f;
                Main.dust[dust1].fadeIn = 1f;
                int dust2 = Dust.NewDust(position.PositionOFFSET(velocity, 40), 0, 0, DustID.Shadowflame);
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].position += Main.rand.NextVector2CircularEdge(15, 5.5f).RotatedBy(velocity.ToRotation() + MathHelper.PiOver2) * 2;
                Main.dust[dust2].velocity = velocity * .2f;
                Main.dust[dust2].fadeIn = 1f;
            }
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
    class MagicHandCannonProjectile : SynergyModProjectile
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
        public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer)
        {
            int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2CircularEdge(5f, 5f), 0, 0, DustID.Shadowflame);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Vector2.Zero;
            Main.dust[dust].scale = .25f;
            Main.dust[dust].fadeIn = 1;
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
        public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone)
        {
            npc.AddBuff(BuffID.ShadowFlame, 180);
            npc.immune[Projectile.owner] = 4;
        }
        public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft)
        {
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