using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Projectiles
{
    internal class pearlSwordProj : ModProjectile
    {
        public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.PearlwoodSword);
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;

            Projectile.width = Projectile.height = 24;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 80;
            Projectile.aiStyle = -1;
            Projectile.alpha = 250;
            Projectile.ArmorPenetration = 10;
        }

        float flare = 0;

        public override void PostDraw(Color lightColor)
        {
            Vector2 position = Projectile.Center - Main.screenPosition;
            DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, position, new Color(255, 255, 255, 0) * 125, new Color(255, 0, 122, 0) * 125, flare, 0f, 0.5f, 0.5f, 1f, 0f, Vector2.One * 2f, Vector2.One * 2f);
        }

        private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawpos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness)
        {
            Texture2D sparkleTexture = TextureAssets.Extra[98].Value;
            Color bigColor = shineColor * opacity * 0.5f;
            bigColor.A = 0;
            Vector2 origin = sparkleTexture.Size() / 2f;
            Color smallColor = drawColor * 0.5f;
            float lerpValue = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
            Vector2 scaleLeftRight = new Vector2(fatness.X * 0.5f, scale.X) * lerpValue;
            Vector2 scaleUpDown = new Vector2(fatness.Y * 0.5f, scale.Y) * lerpValue;
            bigColor *= lerpValue;
            smallColor *= lerpValue;
            // Bright, large part
            Main.EntitySpriteDraw(sparkleTexture, drawpos, null, bigColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight, dir);
            Main.EntitySpriteDraw(sparkleTexture, drawpos, null, bigColor, 0f + rotation, origin, scaleUpDown, dir);
            // Dim, small part
            Main.EntitySpriteDraw(sparkleTexture, drawpos, null, smallColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight * 0.6f, dir);
            Main.EntitySpriteDraw(sparkleTexture, drawpos, null, smallColor, 0f + rotation, origin, scaleUpDown * 0.6f, dir);
        }
        public override void AI()
        {

            if (flare < 1f)
                flare += 0.03f;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Projectile.ai[0]++;
            if(Projectile.alpha > 0)
                Projectile.alpha -= 10;

            if (Projectile.ai[0] >= 28)
            {
                Vector2 dir = Projectile.velocity.SafeNormalize(Vector2.UnitY);
                Projectile.velocity = dir * 45;
                for(int i = 0; i < 5; i++)
                {

                    var dust = Dust.NewDust(Projectile.Center + Projectile.velocity, 4, 4, DustID.PinkTorch);
                    Main.dust[dust].noGravity = true;

                }
                

            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TrueExcalibur,
            new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) },
            Projectile.owner);
            modifiers.DisableCrit();
        }
    }
}
