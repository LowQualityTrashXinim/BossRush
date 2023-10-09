using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace BossRush.Contents.Items.BuilderItem
{
    internal class NeoDynamite : ModItem
    {
        public override void SetDefaults()
        {
            Item.BossRushDefaultRange(18, 56, 0, 0, 20, 20, ItemUseStyleID.Swing, ModContent.ProjectileType<NeoDynamiteExplosion>(), 11f, false);
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.buyPrice(0, 0, 20, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.StickyDynamite, 99)
                .Register();
        }
    }

    class NeoDynamiteExplosion : ModProjectile
    {
        public override string Texture => BossRushUtils.GetTheSameTextureAs<NeoDynamite>();
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            DrawOffsetX = -2;
            DrawOriginOffsetY = -22;
            Projectile.penetrate = -2;
            Projectile.scale = .5f;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
        }

        int explosionRadius = 14;

        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(20);
            Vector2 Head = Projectile.Center + (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2() * 13;
            Vector2 End = Projectile.Center - (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2() * 13;
            for (int i = 0; i < 3; i++)
            {
                int dust = Dust.NewDust(Head, 0, 0, DustID.Vortex, 0, 0, 0, default, Main.rand.NextFloat(.9f, 1.1f));
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Vector2.Zero;
                Main.dust[dust].fadeIn = 1f;
                int dust2 = Dust.NewDust(End, 0, 0, DustID.Vortex, 0, 0, 0, default, Main.rand.NextFloat(.9f, 1.1f));
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].velocity = Vector2.Zero;
                Main.dust[dust2].fadeIn = 1f;
                int dust3 = Dust.NewDust(Projectile.Center, 0, 0, DustID.Vortex, 0, 0, 0, default, Main.rand.NextFloat(.9f, 1.1f));
                Main.dust[dust3].noGravity = true;
                Main.dust[dust3].velocity = Main.rand.NextVector2Circular(3f, 3f) - Projectile.velocity;
                Main.dust[dust3].fadeIn = 1f;
            }
        }
        public bool canKillTiles(int i, int j)
        {
            if (Main.tile[i, j] != null)
            {
                if (Main.tileDungeon[Main.tile[i, j].TileType]
                    || Main.tile[i, j].TileType == 88
                    || Main.tile[i, j].TileType == 21
                    || Main.tile[i, j].TileType == 26
                    || Main.tile[i, j].TileType == 107
                    || Main.tile[i, j].TileType == 108
                    || Main.tile[i, j].TileType == 111
                    || Main.tile[i, j].TileType == 226
                    || Main.tile[i, j].TileType == 237
                    || Main.tile[i, j].TileType == 221
                    || Main.tile[i, j].TileType == 222
                    || Main.tile[i, j].TileType == 223
                    || Main.tile[i, j].TileType == 211
                    || Main.tile[i, j].TileType == 404)
                {
                    return false;
                }
                if (!Main.hardMode && Main.tile[i, j].TileType == 58)
                {
                    return false;
                }
                if (!TileLoader.CanExplode(i, j))
                {
                    return false;
                }
            }
            return true;
        }

        public void killWall(int i, int j, double distanceToTile)
        {
            for (int x = i - 1; x <= i + 1; x++)
            {
                for (int y = j - 1; y <= j + 1; y++)
                {
                    if (Main.tile[x, y] != null
                        && distanceToTile < explosionRadius * explosionRadius
                        && Main.tile[x, y].WallType > 0
                        && WallLoader.CanExplode(x, y, Main.tile[x, y].WallType))
                    {
                        WorldGen.KillWall(x, y, false);
                        if (Main.tile[x, y].WallType == 0 && Main.netMode != NetmodeID.SinglePlayer)
                        {
                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 2, x, y, 0f, 0, 0, 0);
                        }
                    }
                }
            }
        }

        public void SpawnExplosiveDust()
        {
            int count;
            float rngRotate = Main.rand.NextFloat(180);
            for (int i = 0; i < 200; i++)
            {
                count = i / 100;
                float ToRotation = MathHelper.ToRadians(90 * count + rngRotate);
                Vector2 circle = Main.rand.NextVector2CircularEdge(4f, 22.5f).RotatedBy(ToRotation);
                int dust = Dust.NewDust(Projectile.Center, 0, 0, 229, 0, 0, 0, default, Main.rand.NextFloat(.9f, 1.2f));
                Main.dust[dust].noGravity = true;
                Main.dust[dust].fadeIn = 1.5f;
                Main.dust[dust].velocity = circle;
            }
            for (int i = 0; i < 200; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, Type: DustID.Vortex, 0, 0, 0, default, Main.rand.NextFloat(1.25f, 1.5f));
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(19f, 19f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            SpawnExplosiveDust();
            int minTileX = Projectile.position.X > 0 ? (int)(Projectile.position.X / 16f - explosionRadius) : 0;
            int maxTileX = Projectile.position.X < Main.maxTilesX ? (int)(Projectile.position.X / 16f + explosionRadius) : Main.maxTilesX;
            int minTileY = Projectile.position.Y > 0 ? (int)(Projectile.position.Y / 16f - explosionRadius) : 0;
            int maxTileY = Projectile.position.Y < Main.maxTilesY ? (int)(Projectile.position.Y / 16f + explosionRadius) : Main.maxTilesY;

            for (int i = minTileX; i <= maxTileX; i++)
            {
                for (int j = minTileY; j <= maxTileY; j++)
                {
                    Vector2 diff = new Vector2(i - Projectile.position.X / 16, j - Projectile.position.Y / 16);
                    double distanceToTile = diff.LengthSquared();
                    if (distanceToTile < explosionRadius * explosionRadius)
                    {
                        if (canKillTiles(i, j))
                        {
                            WorldGen.KillTile(i, j, false, false, false);
                            if (Main.tile[i, j] != null && Main.netMode != NetmodeID.SinglePlayer)
                            {
                                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
                            }
                        }
                        if (canKillTiles(i, j))
                        {
                            killWall(i, j, distanceToTile);
                        }
                    }
                }
            }
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(BossRushUtils.GetTheSameTextureAs<NeoDynamite>("NeoDynamiteGlowMask"), AssetRequestMode.ImmediateLoad).Value;
            Main.EntitySpriteDraw(
                texture,
                new Vector2
                (
                    Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f + 2,
                    Projectile.position.Y - Main.screenPosition.Y + Projectile.height - texture.Height * 0.5f + 2f + 22
                ),
                null,
                Color.White,
                Projectile.rotation,
                texture.Size() * 0.5f,
                Projectile.scale,
                SpriteEffects.None,
                0
            );
        }
    }
}