using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class AmethystBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            Projectile.light = 1f;
        }

        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemAmethyst, Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f), Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f), 0, default, Main.rand.NextFloat(1f, 1.5f));
                Main.dust[dustnumber].noGravity = true;
            }
            if (Projectile.timeLeft <= 2)
            {
                Projectile.timeLeft = 2;
                if (Projectile.velocity.Y < 10) Projectile.velocity.Y += 0.0167f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Player player = Main.player[Projectile.owner];
            float speedMultipler = 1;
            if (player.ZoneOverworldHeight)
            {
                speedMultipler += 3.5f;
            }
            int damage = (int)(Projectile.damage * 1.25f);
            for (int i = 0; i < 5; i++)
            {
                Vector2 RandomCircular = Main.rand.NextVector2Circular(5 + speedMultipler, 5 + speedMultipler);
                Vector2 TemporaryVector = RandomCircular - oldVelocity * speedMultipler;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, TemporaryVector, ModContent.ProjectileType<AmethystGemP>(), damage, 0, Projectile.owner);
            }
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage -= 1;
            target.immune[Projectile.owner] = 3;
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            float speedMultipler = 1;
            if (player.ZoneOverworldHeight)
            {
                speedMultipler += 3.5f;
            }
            for (int i = 0; i < 75; i++)
            {
                Vector2 RandomCircular = Main.rand.NextVector2Circular(5 + speedMultipler, 5 + speedMultipler);
                Vector2 TemporaryVector = RandomCircular + -Projectile.oldVelocity * speedMultipler;
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemAmethyst, TemporaryVector.X, TemporaryVector.Y, 0, default, Main.rand.NextFloat(1.25f, 2.25f));
                Main.dust[dustnumber].noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale - k * 0.02f, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}
