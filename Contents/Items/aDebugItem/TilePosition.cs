using BossRush.Texture;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.aDebugItem {
	class TilePosition : ModItem {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.CelestialWand);
		public override void SetDefaults() {
			Item.width = Item.height = 32;
			Item.useTime = Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = SoundID.MaxMana with { Pitch = 1f };
			Item.noUseGraphic = true;
		}
		Point point1 = Point.Zero;
		Point point2 = Point.Zero;
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			if (!WorldGen.InWorld(point1.X, point1.Y)) {
				return;
			}
			if (!WorldGen.InWorld(point2.X, point2.Y)) {
				return;
			}

			if (point1 != Point.Zero && point2 != Point.Zero) {

				int farX = Math.Max(point1.X, point2.X);
				int shortX = Math.Min(point1.X, point2.X);
				int distanceX = farX - shortX;

				int farY = Math.Max(point1.Y, point2.Y);
				int shortY = Math.Min(point1.Y, point2.Y);
				int distanceY = farY - shortY;

				Tile tile1 = Framing.GetTileSafely(point1);
				string tileinfo =
					$"Tile type : {tile1.TileType}\n" +
					$"Tile Frame X : {tile1.TileFrameX}\n" +
					$"Tile Frame Y : {tile1.TileFrameY}\n" +
					$"Tile position in tile : {point1.ToString()}\n";

				Tile tile2 = Framing.GetTileSafely(point2);
				string tileinfo2 =
					$"Tile type : {tile2.TileType}\n" +
					$"Tile Frame X : {tile2.TileFrameX}\n" +
					$"Tile Frame Y : {tile2.TileFrameY}\n" +
					$"Tile position in tile : {point2.ToString()}\n";


				tooltips.Add(new(Mod, "info",
					$"{tileinfo}\n" +
					$"<------->\n" +
					$"{tileinfo2}\n" +
					$"<------->\n" +
					$"Distance X : {distanceX}\n" +
					$"Distance Y : {distanceY}\n"));
			}
		}
		public override bool? UseItem(Player player) {
			if (player.ItemAnimationJustStarted) {
				if (player.altFunctionUse != 2) {

					if (point1 == Point.Zero) {
						point1 = Main.MouseWorld.ToTileCoordinates();
						return base.UseItem(player);
					}
					if (point2 == Point.Zero) {
						point2 = Main.MouseWorld.ToTileCoordinates();
						return base.UseItem(player);
					}
				}
				else {
					point1 = Point.Zero;
					point2 = Point.Zero;
				}
			}
			return base.UseItem(player);
		}
		public override bool AltFunctionUse(Player player) => true;
		public override void HoldItem(Player player) {
			if (Main.playerInventory) {
				return;
			}
			string tileinfo;
			Vector2 pos = Main.MouseWorld;
			Point point = pos.ToTileCoordinates();
			Tile tile = Framing.GetTileSafely(point.X, point.Y);
			if (tile == new Tile()) {
				return;
			}
			tileinfo =
				$"Tile type : {tile.TileType}\n" +
				$"Tile Frame X : {tile.TileFrameX}\n" +
				$"Tile Frame Y : {tile.TileFrameY}\n" +
				$"Tile position in tile : {point.ToString()}\n";
			Main.instance.MouseText(tileinfo);
		}
	}
}
