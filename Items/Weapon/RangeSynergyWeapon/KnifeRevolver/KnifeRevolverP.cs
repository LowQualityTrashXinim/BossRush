using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using BossRush.Items.Weapon;

namespace BossRush.Items.Weapon.RangeSynergyWeapon.KnifeRevolver
{
    internal class KnifeRevolverP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(Projectile.gfxOffY);
                Main.EntitySpriteDraw(texture, drawPos, null, Color.LightYellow, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
        public override void Kill(int timeLeft)
        {
            Vector2 Rotate;
            float randomRotation = Main.rand.NextFloat(90);
            for (int i = 0; i < 150; i++)
            {
                Rotate = Main.rand.NextVector2CircularEdge(12.5f, 12.5f);
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemRuby, Rotate.X, Rotate.Y, 0, default, Main.rand.NextFloat(1f, 2.5f));
                Main.dust[dustnumber].noGravity = true;
            }
            for (int i = 0; i < 100; i++)
            {
                Rotate = Main.rand.NextVector2CircularEdge(10f, 10f);
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, Rotate.X, Rotate.Y, 0, default, Main.rand.NextFloat(1.25f, 1.5f));
                Main.dust[dustnumber].noGravity = true;
            }
            for (int i = 0; i < 250; i++)
            {

                if (i % 2 == 0)
                {
                    Rotate = Main.rand.NextVector2CircularEdge(1.5f, 20f).RotatedBy(MathHelper.ToRadians(randomRotation)) * 2;
                }
                else
                {
                    Rotate = Main.rand.NextVector2CircularEdge(20f, 1.5f).RotatedBy(MathHelper.ToRadians(randomRotation)) * 2;
                }
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemDiamond, Rotate.X, Rotate.Y, 0, default, Main.rand.NextFloat(1.25f, 1.5f));
                Main.dust[dustnumber].noGravity = true;
            }
            Item item = new Item(ModContent.ItemType<KnifeRevolver>());
            Projectile.NewProjectile(Entity.GetSource_ItemUse_WithPotentialAmmo(new Item(ModContent.ItemType<KnifeRevolver>()), item.useAmmo), Projectile.position, Vector2.Zero, ModContent.ProjectileType<GhostHitBox>(), Projectile.damage, 5f, Projectile.owner);
        }
    }
}
