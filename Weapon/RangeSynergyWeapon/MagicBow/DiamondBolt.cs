using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;
using System.Collections.Generic;

namespace BossRush.Weapon.RangeSynergyWeapon.MagicBow
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
            if (RicochetOff(out int activeInList))
            {
                if (activeInList != -1 || activeInList < count.Count)
                {
                    Projectile.netUpdate = true;
                    Projectile.damage += 50;
                    Projectile.velocity = (count[activeInList].Position - Projectile.position).SafeNormalize(Vector2.UnitX) * 10;
                }
                for (int i = 0; i < 75; i++)
                {
                    Vector2 ReverseVelSpread = -Projectile.velocity*2 + Main.rand.NextVector2Circular(5f,5f);
                    int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, ReverseVelSpread.X, ReverseVelSpread.Y, 0, default, Main.rand.NextFloat(1f, 1.5f));
                    Main.dust[dustnumber].noGravity = true;
                }
            }

        }

        List<CountProjectile> count = new List<CountProjectile>();

        public override void Kill(int timeLeft)
        {
            count.Clear();
        }

        public bool CheckActive(Projectile projectileThatNeedtoCheck)
        {
            Player player = Main.player[Projectile.owner];
            float Distance = 1500f;
            if (projectileThatNeedtoCheck.type == ModContent.ProjectileType<DiamondGemP>())
            {
                if (Vector2.Distance(player.Center, projectileThatNeedtoCheck.Center) < Distance && projectileThatNeedtoCheck.active)
                {
                    return true;
                }
            }
            return false;
        }

        public bool RicochetOff(out int ActiveinList)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].type == ModContent.ProjectileType<DiamondGemP>() && Main.projectile[i].velocity == Vector2.Zero && Main.projectile[i].active)
                {
                    CountProjectile countPro = new CountProjectile(Main.projectile[i].Center);
                    countPro.ProjectileHolder = Main.projectile[i];
                    if (GetCountProjectile(count, Main.projectile[i].Center) && CheckActive(Main.projectile[i]))
                    {
                        count.Add(countPro);
                    }
                    else if(!CheckActive(countPro.ProjectileHolder))
                    {
                        count.Clear();
                    }
                    for (int l = 0; l < count.Count; l++)
                    {
                        float Distance = Vector2.Distance(Projectile.Center, count[l].Position);
                        if (Distance <= 15 && !count[l].Deactivate)
                        {
                            for (int a = 0; a < count.Count; a++)
                            {
                                if (count[a].Deactivate)
                                {
                                    foreach (var item in count)
                                    {
                                        item.Deactivate = false;
                                    }
                                }
                            }
                            int preventLoopInfinite = 0;
                            count[l].Deactivate = true;
                            do
                            {
                                ActiveinList = Main.rand.Next(count.Count);
                                preventLoopInfinite++;
                            }
                            while (count[ActiveinList].Deactivate || preventLoopInfinite <= 50);
                            return true;
                        }
                    }
                }
            }
            ActiveinList = -1;
            return false;
        }

        public bool GetCountProjectile(List<CountProjectile> list, Vector2 position)
        {
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Position.Equals(position))
                    {
                        return false;
                    }
                }
            }
            return true;
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
    public class CountProjectile
    {
        public Vector2 Position;
        public bool Deactivate = false;
        public Projectile ProjectileHolder;

        public CountProjectile(Vector2 position)
        {
            Position = position;
        }
    }
}