using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Utils;

namespace BossRush.Contents.Items.BuilderItem {
	internal class TowerDestruction : ModItem {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 56;

			Item.useAnimation = 20;
			Item.useTime = 20;

			Item.shoot = ModContent.ProjectileType<TowerDestructionProjectile>();
			Item.shootSpeed = 11f;

			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.value = Item.buyPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.StickyDynamite, 99)
				.Register();
		}
	}
	class TowerDestructionProjectile : ModProjectile {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Projectile.width = 14;
			Projectile.height = 14;
			DrawOffsetX = -2;
			DrawOriginOffsetY = -22;
			Projectile.penetrate = -2;
			Projectile.scale = .5f;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
		}
		int firstframe = 0;
		bool directionOfMoving;
		public override void AI() {
			if (firstframe == 0) {
				directionOfMoving = Projectile.velocity.X > 0;
				firstframe++;
			}
			Projectile.rotation += MathHelper.ToRadians(20);
		}
		int explosionRadiusX = 300;
		int explosionRadiusY = 10;
		public override void OnKill(int timeLeft) {
			SpawnExplosionDust();
			int tileX = (int)(Projectile.position.X * .0625f);
			int tileY = (int)(Projectile.position.Y * .0625f);
			int minX = directionOfMoving ? -3 : -explosionRadiusX;
			int maxX = directionOfMoving ? explosionRadiusX : 3;
			for (int x = minX; x < maxX; x++) {
				int xPos = x + tileX;
				for (int y = -explosionRadiusY; y < explosionRadiusY; y++) {
					int yPos = y + tileY;
					if (WorldGen.CanKillTile(xPos, yPos, WorldGen.SpecialKillTileContext.None)) {
						//GenerationHelper.FastRemoveTile(xPos, yPos);
						WorldGen.KillTile(xPos, yPos, false, false, true);
						if (Main.tile[xPos, yPos] != null && Main.netMode != NetmodeID.SinglePlayer) {
							NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, xPos, yPos, 0f, 0, 0, 0);
						}
						killWall(xPos, yPos);
					}
				}
			}
		}
		private void SpawnExplosionDust() {
			int offsethalf = (int)(explosionRadiusY * .5f);
			for (int i = -offsethalf; i < offsethalf; i++) {
				Vector2 pos = Projectile.Center + new Vector2(0, i * 16);
				for (int l = 0; l < 25; l++) {
					int dust = Dust.NewDust(pos + new Vector2(Main.rand.NextFloat(20), Main.rand.NextFloat(20)), 0, 0, DustID.Torch);
					Main.dust[dust].velocity = new Vector2(Main.rand.NextFloat(8, 25), 0) * directionOfMoving.ToDirectionInt();
					Main.dust[dust].noGravity = true;
					Main.dust[dust].scale = Main.rand.NextFloat(3f, 5f);
					Main.dust[dust].fadeIn = Main.rand.NextFloat(1f, 2f);
					int dust2 = Dust.NewDust(pos + new Vector2(Main.rand.NextFloat(20), Main.rand.NextFloat(20)), 0, 0, DustID.Obsidian);
					Main.dust[dust2].velocity = new Vector2(Main.rand.NextFloat(5, 9), 0) * directionOfMoving.ToDirectionInt();
					Main.dust[dust2].noGravity = true;
					Main.dust[dust2].scale = Main.rand.NextFloat(3f, 5f);
					Main.dust[dust].fadeIn = Main.rand.NextFloat(1f, 2f);
				}
			}
		}
		//public bool canKillTiles(int i, int j) {
		//	if (Main.tile[i, j] != null) {
		//		if (Main.tileDungeon[Main.tile[i, j].TileType]
		//			|| Main.tile[i, j].TileType == TileID.Dressers
		//			|| Main.tile[i, j].TileType == TileID.Containers
		//			|| Main.tile[i, j].TileType == TileID.DemonAltar
		//			|| Main.tile[i, j].TileType == 107
		//			|| Main.tile[i, j].TileType == 108
		//			|| Main.tile[i, j].TileType == 111
		//			|| Main.tile[i, j].TileType == 226
		//			|| Main.tile[i, j].TileType == 237
		//			|| Main.tile[i, j].TileType == 221
		//			|| Main.tile[i, j].TileType == 222
		//			|| Main.tile[i, j].TileType == 223
		//			|| Main.tile[i, j].TileType == 211
		//			|| Main.tile[i, j].TileType == 404) {
		//			return false;
		//		}
		//		if (!TileLoader.CanExplode(i, j)) {
		//			return false;
		//		}
		//	}
		//	return true;
		//}
		public void killWall(int i, int j) {
			for (int x = i - 1; x <= i + 1; x++) {
				for (int y = j - 1; y <= j + 1; y++) {
					if (Main.tile[x, y] != null && Main.tile[x, y].WallType > 0 && Main.tile[i, j].WallType == 0 && Main.netMode != NetmodeID.SinglePlayer) {
						WorldGen.KillWall(x, y, false);
						if (Main.tile[x, y].WallType == 0 && Main.netMode != NetmodeID.SinglePlayer) {
							NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 2, x, y, 0f, 0, 0, 0);
						}
					}
				}
			}
		}
		//public override void PostDraw(Color lightColor)
		//{
		//    Texture2D texture = ModContent.Request<Texture2D>(BossRushUtils.GetTheSameTextureAs<NeoDynamite>("NeoDynamiteGlowMask"), AssetRequestMode.ImmediateLoad).Value;
		//    Main.EntitySpriteDraw(
		//        texture,
		//        new Vector2
		//        (
		//            Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f + 2,
		//            Projectile.position.Y - Main.screenPosition.Y + Projectile.height - texture.Height * 0.5f + 2f + 22
		//        ),
		//        null,
		//        Color.White,
		//        Projectile.rotation,
		//        texture.Size() * 0.5f,
		//        Projectile.scale,
		//        SpriteEffects.None,
		//        0
		//    );
		//}
	}
}
