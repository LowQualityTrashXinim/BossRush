using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.ShadowTrick
{
    internal class ShadowTrickP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 15f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 400f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 12;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 99;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 24;
            Projectile.height = 24;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            float Num = 3;
            float Rotation = MathHelper.ToRadians(180);
            for (int i = 0; i < Num; i++)
            {
                Vector2 rotateVecloity = Vector2.One.RotatedBy(MathHelper.Lerp(Rotation, -Rotation, i / Num)) * 5;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, rotateVecloity, ModContent.ProjectileType<ShadowSpike>(), Projectile.damage / 5, 0.2f, Projectile.owner);
            }
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(150)), ModContent.ProjectileType<ShadowTrickP2>(), Projectile.damage, 0.2f, Projectile.owner);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
    internal class ShadowTrickP2 : ModProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<ShadowTrick>();
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 3f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 1000f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 15;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 99;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 24;
            Projectile.height = 24;
        }
    }
}

