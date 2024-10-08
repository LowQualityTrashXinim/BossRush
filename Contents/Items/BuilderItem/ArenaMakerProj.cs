﻿using BossRush.Texture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.BuilderItem {
	internal class ArenaMakerProj : ModProjectile {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Projectile.width = 20;
			Projectile.height = 36;
			Projectile.friendly = true;
			Projectile.aiStyle = 16;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 1;
		}
		public override bool? CanDamage() {
			return false;
		}
		public override void OnKill(int timeLeft) {
			Vector2 position = Projectile.Center;
			const int length = 300;
			bool goLeft = Projectile.Center.X < Main.player[Projectile.owner].Center.X;
			int min = goLeft ? -length : 0;
			int max = goLeft ? 0 : length;
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}
			for (int x = min; x <= max; x++) {
				int xPos = (int)(x + position.X * .0625f);
				int yPos = (int)(position.Y * .0625f);
				if (!WorldGen.InWorld(xPos, yPos))
					continue;

				Tile tile = Main.tile[xPos, yPos];

				if (tile == null)
					continue;

				if (Main.player[Projectile.owner].ZoneUnderworldHeight)
					WorldGen.PlaceTile(xPos, yPos, TileID.Platforms, false, false, -1, 13);
				else
					WorldGen.PlaceTile(xPos, yPos, TileID.Platforms);

				if (Main.netMode == NetmodeID.Server)
					NetMessage.SendTileSquare(-1, xPos, yPos, 1);

				if (x % 25 != 0 || yPos - 1 < 0)
					continue;

				Tile tileAbove = Main.tile[xPos, yPos - 1];
				if (tileAbove.HasTile)
					continue;

				WorldGen.PlaceTile(xPos, yPos - 1, TileID.Torches);
				if (Main.netMode == NetmodeID.Server)
					NetMessage.SendTileSquare(-1, xPos, yPos - 1, 1);
			}
		}
	}
}
