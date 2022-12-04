using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Items.Weapon.MeleeSynergyWeapon
{
    internal class PlatinumSlash : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 6;
            Projectile.light = 0.5f;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 150; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            Projectile.scale = 0.5f;
            Projectile.Size -= new Vector2(10, 10);
        }
        public override void AI()
        {
            Player player = Main.LocalPlayer;
            int MaxAnimation = player.itemAnimationMax;
            if (Projectile.timeLeft > MaxAnimation * 4)
            {
                Projectile.timeLeft = MaxAnimation * 4;
            }
            for (int i = 0; i < 3; i++)
            {
                Vector2 RandomSpread = Main.rand.NextVector2Circular(1f, 1f);
                int dustnumber = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemDiamond, RandomSpread.X, RandomSpread.Y, 0, default, Main.rand.NextFloat(.75f, 1f));
                Main.dust[dustnumber].noGravity = true;
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 1; i <= 10; i++)
            {
                for (int l = 0; l < 10; l++)
                {
                    Vector2 RandomSpread = Main.rand.NextVector2CircularEdge(1f, 1f) * 10 / i + Projectile.velocity * 1 / i;
                    int dustnumber = Dust.NewDust(Projectile.Center, Projectile.width / 2, Projectile.height / 2, DustID.GemDiamond, -RandomSpread.X, -RandomSpread.Y, 0, default, Main.rand.NextFloat(.75f, 1f));
                    Main.dust[dustnumber].noGravity = true;
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 5;
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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale - k * .006f, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}
