using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class TopazBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 100;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Projectile.light = 1f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Player player = Main.player[Projectile.owner];
            var Source = new EntitySource_ItemUse(player, new Item(ModContent.ItemType<TopazBow>()));
            for (int i = 0; i < 5; i++)
            {
                Vector2 RandomCircular = Main.rand.NextVector2Circular(10f, 10f);
                Vector2 newVelocity = new Vector2(RandomCircular.X, -10 + RandomCircular.Y);
                Projectile.NewProjectile(Source, Projectile.Center, newVelocity, ModContent.ProjectileType<TopazGemP>(), Projectile.damage, 0, Projectile.owner);
            }
            return true;
        }
        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemTopaz, Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f), Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f), 0, default, Main.rand.NextFloat(1f, 1.5f));
                Main.dust[dustnumber].noGravity = true;
            }
            if (Projectile.timeLeft <= 2)
            {
                Projectile.timeLeft = 2;
                if (Projectile.velocity.Y < 10) Projectile.velocity.Y += 0.0167f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.damage -= 1;
            target.immune[Projectile.owner] = 3;
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
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 75; i++)
            {
                Vector2 RandomCircular = Main.rand.NextVector2Circular(10f, 10f);
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemTopaz, RandomCircular.X, RandomCircular.Y, 0, default, Main.rand.NextFloat(1.25f, 2.25f));
                Main.dust[dustnumber].noGravity = true;
            }
        }
    }
}
