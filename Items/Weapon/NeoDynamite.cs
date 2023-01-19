using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Items.Weapon
{
    internal class NeoDynamite : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Neo Dynamite");
            Tooltip.SetDefault("Mordern dynamite, never running out!" +
                "\nThrow out a un-consumable dynamite that explode like normal dynamite on tile touch");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 56;

            Item.useAnimation = 20;
            Item.useTime = 20;

            Item.shoot = ModContent.ProjectileType<NeoDynamiteExplosion>();
            Item.shootSpeed = 15f;

            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Blue;
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
        public override string Texture => "BossRush/Items/Weapon/NeoDynamite";
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
            Vector2 Head = Projectile.Center + (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2() *11;
            Vector2 End = Projectile.Center - (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2() * 11;
            for (int i = 0; i < 3; i++)
            {
                int dust = Dust.NewDust(Head, 0, 0, 229, 0, 0, 0, default, Main.rand.NextFloat(.9f, 1.1f));
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(2f, 2f);
                int dust2 = Dust.NewDust(End, 0, 0, 229, 0, 0, 0, default, Main.rand.NextFloat(.9f, 1.1f));
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].velocity = Main.rand.NextVector2CircularEdge(2f, 2f);
            }
        }

        public bool cankillWalls(int i, int j, double distance)
        {
            if (distance < explosionRadius * explosionRadius && Main.tile[i, j] != null && Main.tile[i, j].WallType == 0)
            {
                return true;
            }
            return false;
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
                    if (Main.tile[x, y] != null && Main.tile[x, y].WallType > 0 && cankillWalls(i, j, distanceToTile) && WallLoader.CanExplode(x, y, Main.tile[x, y].WallType))
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
            for (int i = 0; i < 200; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, 226, 0, 0, 0, default, Main.rand.NextFloat(.9f, 1.1f));
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Main.rand.NextVector2Circular(23f, 18f);
            }
            for (int i = 0; i < 200; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, 229, 0, 0, 0, default, Main.rand.NextFloat(1.35f, 1.5f));
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(19f, 19f);
            }
        }

        public override void Kill(int timeLeft)
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
                    Vector2 diff = new Vector2(i - Projectile.position.X / 16f, j - Projectile.position.Y / 16f);
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
    }
}
