using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;
using System.Collections.Generic;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.MagicBow
{
    internal class DiamondBolt : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Projectile.light = 1f;
        }


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 2;
            Player player = Main.player[Projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<DiamondGemP>()] < 1)
            {
                int num = Main.rand.Next(9);
                for (int i = 0; i < 4; i++)
                {
                    Vector2 Rotate = Projectile.velocity.RotatedBy(MathHelper.ToRadians(90 * i + 10 * num)) * Main.rand.NextFloat(1.5f, 3f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Rotate, ModContent.ProjectileType<DiamondGemP>(), 0, 0, Projectile.owner);
                }
            }
        }

        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, Projectile.velocity.X * Main.rand.NextFloat(-1.25f, -0.5f), Projectile.velocity.Y * Main.rand.NextFloat(-1.25f, -0.5f), 0, default, Main.rand.NextFloat(1f, 1.5f));
                Main.dust[dustnumber].noGravity = true;
            }
            if (RicochetOff(out Vector2 pos2))
            {
                Projectile.netUpdate = true;
                Projectile.damage += 50;
                Projectile.velocity = (pos2 - Projectile.position).SafeNormalize(Vector2.UnitX) * 10;
                for (int i = 0; i < 50; i++)
                {
                    Vector2 ReverseVelSpread = -Projectile.velocity * 2 + Main.rand.NextVector2Circular(5f, 5f);
                    int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, ReverseVelSpread.X, ReverseVelSpread.Y, 0, default, Main.rand.NextFloat(1f, 1.5f));
                    Main.dust[dustnumber].noGravity = true;
                }
            }
        }

        public bool CheckActive(Projectile projectileThatNeedtoCheck)
        {
            Player player = Main.player[Projectile.owner];
            float Distance = 500f;
            if (projectileThatNeedtoCheck.type == ModContent.ProjectileType<DiamondGemP>() && projectileThatNeedtoCheck.active && projectileThatNeedtoCheck.velocity == Vector2.Zero)
            {
                if (Vector2.Distance(player.Center, projectileThatNeedtoCheck.Center) < Distance)
                {
                    return true;
                }
            }
            return false;
        }
        public List<Vector2> GetListOfActiveProj(out bool Check)
        {
            List<Vector2> list = new List<Vector2>();
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (CheckActive(Main.projectile[i]))
                {
                    list.Add(Main.projectile[i].Center);
                }
            }
            if (list.Count <= 1)
            {
                Check = false;
            }
            else
            {
                Check = true;
            }
            return list;
        }
        public bool RicochetOff(out Vector2 Pos2)
        {
            List<Vector2> list = GetListOfActiveProj(out bool Check);
            if (Check)
            {
                Vector2 Pos1 = Vector2.Zero;
                foreach (Vector2 pos in list)
                {
                    float Distance = Vector2.Distance(Projectile.Center, pos);
                    if (Distance <= 15)
                    {
                        Pos1 = pos;
                        do
                        {
                            Pos2 = Main.rand.Next(list);
                        }
                        while (Pos2 == Pos1);
                        return true;
                    }
                }
            }
            Pos2 = Vector2.Zero;
            return false;
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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale - k * 0.01f, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}