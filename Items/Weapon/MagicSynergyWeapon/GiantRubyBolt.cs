using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Items.Weapon.MagicSynergyWeapon
{
    internal class GiantRubyBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Magic;
        }
        float count = 0;
        int counttime = 0;
        int permaCount = 0;
        public override void AI()
        {
            SelectFrame();
            Player player = Main.player[Projectile.owner];
            Projectile.velocity -= Projectile.velocity * 0.1f;
            EntitySource_ItemUse source = new EntitySource_ItemUse(player, new Item(ModContent.ItemType<RubySwotaff>()));
            if (Projectile.velocity.X < 1 && Projectile.velocity.X > -1 && Projectile.velocity.Y < 1 && Projectile.velocity.Y > -1)
            {
                Projectile.velocity = Vector2.Zero;
                for (int i = 0; i < 30; i++)
                {
                    Vector2 Rotate = Main.rand.NextVector2CircularEdge(5f, 5f);
                    int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, Rotate.X, Rotate.Y, 100, default, Main.rand.NextFloat(0.75f, 2f));
                    Main.dust[dustnumber].noGravity = true;
                }
            }
            if (Projectile.velocity == Vector2.Zero)
            {
                if (permaCount == 0)
                {
                    for (int i = 0; i < 150; i++)
                    {
                        Vector2 Rotate = Main.rand.NextVector2CircularEdge(20f, 20f);
                        int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, Rotate.X, Rotate.Y, default, default, Main.rand.NextFloat(1.5f, 2.5f));
                        Main.dust[dustnumber].noGravity = true;
                        permaCount++;
                    }
                }
                Projectile.ai[0]++;
                if (Projectile.ai[0] >= 10)
                {
                    Projectile.netUpdate = true;
                    for (int i = 0; i < 12; i++)
                    {
                        Vector2 Rotate = Main.rand.NextVector2Circular(5f, 5f);
                        int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, Rotate.X, Rotate.Y, 100, default, Main.rand.NextFloat(0.75f, 2f));
                        Main.dust[dustnumber].noGravity = true;
                    }
                    if (counttime == 0)
                    {
                        count += 2f;
                        for (int i = 0; i < 4; i++)
                        {
                            Projectile.NewProjectile(source, Projectile.Center, Vector2.One.RotatedBy(MathHelper.ToRadians(count + i * 90)) * 5, ModContent.ProjectileType<SmallerRubyBolt>(), (int)(Projectile.damage * 0.35f), Projectile.knockBack * 0.5f, Projectile.owner);
                            Projectile.NewProjectile(source, Projectile.Center, Vector2.One.RotatedBy(MathHelper.ToRadians(-count + i * 90)) * 5, ModContent.ProjectileType<SmallerRubyBolt>(), (int)(Projectile.damage * 0.35f), Projectile.knockBack * 0.5f, Projectile.owner);
                        }
                    }
                    counttime++;
                    if (counttime >= 1)
                    {
                        counttime = 0;
                    }
                }
            }
        }

        public void SelectFrame()
        {
            if (++Projectile.frameCounter >= 10)
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
