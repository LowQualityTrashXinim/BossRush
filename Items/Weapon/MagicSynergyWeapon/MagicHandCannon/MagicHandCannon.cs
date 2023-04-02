using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Texture;

namespace BossRush.Items.Weapon.MagicSynergyWeapon.MagicHandCannon
{
    internal class MagicHandCannon : ModItem
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Item.BossRushDefaultMagic(0, 0, 30, 5f, 30, 30, ItemUseStyleID.Shoot, ModContent.ProjectileType<MagicHandCannonProjectile>(), 20, 30, false);
        }
        public override void HoldItem(Player player)
        {
            for (int i = 0; i < 150; i++)
            {
                Vector2 SquarePosition = player.Center + Main.rand.NextVector2RectangleEdge(400, 400);
                int dust = Dust.NewDust(SquarePosition, 0, 0, DustID.WhiteTorch, 0, 0, 0, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].noLight = true;
                Main.dust[dust].velocity = Vector2.Zero;
            }
        }
    }
    class MagicHandCannonProjectile : ModProjectile
    {
        public override string Texture => BossRushTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.wet = false;
            Projectile.timeLeft = 300;
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
            Projectile.position += player.velocity;
        }
    }
}
