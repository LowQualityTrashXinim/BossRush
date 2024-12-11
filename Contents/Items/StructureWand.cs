using BossRush.Common.Systems;
using BossRush.Common.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items {
	class StructureWand : ModItem {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Swordfish);
		public bool secondPoint;

		public Point16 point1;
		public Point16 point2;

		public bool movePoint1;
		public bool movePoint2;

		public Point16 TopLeft => new Point16(point1.X < point2.X ? point1.X : point2.X, point1.Y < point2.Y ? point1.Y : point2.Y);
		public Point16 BottomRight => new Point16(point1.X > point2.X ? point1.X : point2.X, point1.Y > point2.Y ? point1.Y : point2.Y);
		public int Width => BottomRight.X - TopLeft.X;
		public int Height => BottomRight.Y - TopLeft.Y;

		public bool Ready => !secondPoint && point1 != default;

		public override void SetDefaults() {
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 20;
			Item.rare = ItemRarityID.Blue;
			Item.noMelee = true;
			Item.noUseGraphic = true;
		}

		public override bool AltFunctionUse(Player player) {
			return true;
		}

		public override void HoldItem(Player player) {
			if (movePoint1)
				point1 = (Main.MouseWorld / 16).ToPoint16();

			if (movePoint2)
				point2 = (Main.MouseWorld / 16).ToPoint16();

			if (!Main.mouseLeft) {
				movePoint1 = false;
				movePoint2 = false;
			}
		}
		public override bool? UseItem(Player player) {
			if (player.altFunctionUse == 2 && Ready && player.ItemAnimationJustStarted) {
				GenerationHelper.SaveToFile(new(TopLeft.X, TopLeft.Y, Width - 1, Height - 1));
				return true;
			}

			if (Ready) {
				if (Vector2.Distance(Main.MouseWorld, point1.ToVector2() * 16) <= 32) {
					movePoint1 = true;
					return true;
				}

				if (Vector2.Distance(Main.MouseWorld, point2.ToVector2() * 16) <= 32) {
					movePoint2 = true;
					return true;
				}
			}

			if (!secondPoint) {
				point1 = (Main.MouseWorld / 16).ToPoint16();
				point2 = default;

				Main.NewText("Select Second Point");
				secondPoint = true;
			}
			else {
				point2 = (Main.MouseWorld / 16).ToPoint16();

				Main.NewText("Ready to save! Right click to save this structure...");
				secondPoint = false;
			}

			return true;
		}
	}
}
