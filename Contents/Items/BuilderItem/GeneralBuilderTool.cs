using BossRush.Common.Systems;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BossRush.Contents.Items.BuilderItem;
public class GeneralBuilderToolSystem : ModSystem {
	public const byte None = 0;
	public const byte Fill = 1;
	public const byte Draw = 2;
	public static byte CurrentMode = None;
	public static bool DeleteMode = false;
	public static bool OverrideMode = false;
	public static bool Tile = false;
	public static bool Wall = false;
	public GeneralBuilderToolUI GeneralBuilderToolState;
	internal UserInterface userInterface;
	public override void Load() {
		if (!Main.dedServ) {
			userInterface = new();
			GeneralBuilderToolState = new();
		}
	}
	public override void Unload() {
		userInterface = null;
		GeneralBuilderToolState = null;
	}
	public override void UpdateUI(GameTime gameTime) {
		base.UpdateUI(gameTime);
		userInterface?.Update(gameTime);
	}
	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
		int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
		if (InventoryIndex != -1)
			layers.Insert(InventoryIndex, new LegacyGameInterfaceLayer(
				"BossRush: UI",
				delegate {
					GameTime gametime = new GameTime();
					userInterface.Draw(Main.spriteBatch, gametime);
					return true;
				},
				InterfaceScaleType.UI)
			);
	}
	public void ToggleUI() {
		if (userInterface.CurrentState != null) {
			DeactivateUI();
		}
		else {
			ActivateUI();
		}
	}
	public void DeactivateUI() {
		userInterface.SetState(null);
	}
	public void ActivateUI() {
		userInterface.SetState(GeneralBuilderToolState);
	}
}
public class GeneralBuilderToolUI : UIState {
	public GeneralBuilderToolUI() {
	}
	public Roguelike_UIPanel Panel;
	public Roguelike_UIImage FillMode;
	public Roguelike_UIImage DrawMode;
	public Roguelike_UIImage DeleteMode;
	public Roguelike_UIImage OverrideMode;
	public Roguelike_UIImage TileMode;
	public Roguelike_UIImage WallMode;
	public override void OnInitialize() {
		Asset<Texture2D> thesameuitextureasvanilla = TextureAssets.InventoryBack7;

		Panel = new();
		Panel.Width.Pixels = 350;
		Panel.Height.Pixels = 100;
		Panel.HAlign = .5f;
		Panel.VAlign = .4f;
		Append(Panel);

		DrawMode = new(thesameuitextureasvanilla);
		DrawMode.SetPostTex(ModContent.Request<Texture2D>(BossRushTexture.DrawBrush));
		DrawMode.VAlign = .5f;
		DrawMode.HighlightColor = DrawMode.OriginalColor * .5f;
		DrawMode.SwapHightlightColorWithOriginalColor();
		DrawMode.OnLeftClick += DrawMode_OnLeftClick;
		DrawMode.HoverText = "Draw mode";
		Panel.Append(DrawMode);

		FillMode = new(thesameuitextureasvanilla);
		FillMode.SetPostTex(ModContent.Request<Texture2D>(BossRushTexture.FillBucket));
		FillMode.VAlign = .5f;
		FillMode.HAlign = MathHelper.Lerp(0, 1f, 1 / 5f);
		FillMode.HighlightColor = FillMode.OriginalColor * .5f;
		FillMode.SwapHightlightColorWithOriginalColor();
		FillMode.OnLeftClick += FillMode_OnLeftClick;
		FillMode.HoverText = "Fill mode";
		Panel.Append(FillMode);

		DeleteMode = new(thesameuitextureasvanilla);
		DeleteMode.SetPostTex(TextureAssets.Trash);
		DeleteMode.VAlign = .5f;
		DeleteMode.HAlign = MathHelper.Lerp(0, 1f, 2 / 5f);
		DeleteMode.HighlightColor = DeleteMode.OriginalColor * .5f;
		DeleteMode.OnLeftClick += DeleteMode_OnLeftClick;
		DeleteMode.SwapHightlightColorWithOriginalColor();
		DeleteMode.HoverText = "Delete mode";
		Panel.Append(DeleteMode);

		OverrideMode = new(thesameuitextureasvanilla);
		OverrideMode.VAlign = .5f;
		OverrideMode.HAlign = MathHelper.Lerp(0, 1f, 3 / 5f);
		OverrideMode.HighlightColor = OverrideMode.OriginalColor * .5f;
		OverrideMode.OnLeftClick += OverrideMode_OnLeftClick;
		OverrideMode.SwapHightlightColorWithOriginalColor();
		OverrideMode.HoverText = "Override mode";
		Panel.Append(OverrideMode);

		TileMode = new(thesameuitextureasvanilla);
		TileMode.SetPostTex(TextureAssets.Item[ItemID.StoneBlock]);
		TileMode.VAlign = .5f;
		TileMode.HAlign = MathHelper.Lerp(0, 1f, 4 / 5f);
		TileMode.HighlightColor = TileMode.OriginalColor * .5f;
		TileMode.OnLeftClick += TileMode_OnLeftClick;
		TileMode.SwapHightlightColorWithOriginalColor();
		TileMode.HoverText = "Tile";
		Panel.Append(TileMode);

		WallMode = new(thesameuitextureasvanilla);
		WallMode.SetPostTex(TextureAssets.Item[ItemID.StoneWall], attemptToLoad: true);
		WallMode.VAlign = .5f;
		WallMode.HAlign = 1;
		WallMode.HighlightColor = WallMode.OriginalColor * .5f;
		WallMode.OnLeftClick += WallMode_OnLeftClick;
		WallMode.SwapHightlightColorWithOriginalColor();
		WallMode.HoverText = "Wall";
		Panel.Append(WallMode);
	}

	private void TileMode_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		GeneralBuilderToolSystem.Tile = !GeneralBuilderToolSystem.Tile;
		TileMode.Highlight = GeneralBuilderToolSystem.Tile;
	}

	private void WallMode_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		GeneralBuilderToolSystem.Wall = !GeneralBuilderToolSystem.Wall;
		WallMode.Highlight = GeneralBuilderToolSystem.Wall;
	}

	private void OverrideMode_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		GeneralBuilderToolSystem.OverrideMode = !GeneralBuilderToolSystem.OverrideMode;
		GeneralBuilderToolSystem.DeleteMode = false;
		DeleteMode.Highlight = false;
		OverrideMode.Highlight = GeneralBuilderToolSystem.OverrideMode;
	}

	private void DeleteMode_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		GeneralBuilderToolSystem.DeleteMode = !GeneralBuilderToolSystem.DeleteMode;
		GeneralBuilderToolSystem.OverrideMode = false;
		OverrideMode.Highlight = false;
		DeleteMode.Highlight = GeneralBuilderToolSystem.DeleteMode;
	}

	private void DrawMode_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		GeneralBuilderToolSystem.CurrentMode = GeneralBuilderToolSystem.Draw;
		FillMode.Highlight = false;
		DrawMode.Highlight = true;
	}

	private void FillMode_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		GeneralBuilderToolSystem.CurrentMode = GeneralBuilderToolSystem.Fill;
		DrawMode.Highlight = false;
		FillMode.Highlight = true;
	}
}
internal class GeneralBuilderTool : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.useAnimation = Item.useTime = 1;
		Item.noUseGraphic = true;
		Item.consumable = false;
		Item.autoReuse = true;
	}
	public Point position1 = new();
	public Point position2 = new();
	public Point oldMousePosition = new();
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		string text = "";
		if (GeneralBuilderToolSystem.CurrentMode == GeneralBuilderToolSystem.Fill) {
			text = "Current Mode : Fill\n";
		}
		else if (GeneralBuilderToolSystem.CurrentMode == GeneralBuilderToolSystem.Draw) {
			text = "Current Mode : Draw\n";
		}
		else {
			text = "Alt click to select Fill or Draw mode";
		}
		if (GeneralBuilderToolSystem.DeleteMode) {
			text += "Delete Mode";
		}
		else if (GeneralBuilderToolSystem.OverrideMode) {
			text += "Override Mode";
		}
		else {
			text += "Normal Mode";
		}
		if (GeneralBuilderToolSystem.Tile) {
			text += "\nPlace tile";
		}
		if (GeneralBuilderToolSystem.Wall) {
			text += "\nPlace wall";
		}
		TooltipLine line = new(Mod, "Text", text);
		tooltips.Add(line);
	}
	public override bool AltFunctionUse(Player player) => true;
	public override void HoldItem(Player player) {
		GeneralBuilderToolSystem system = ModContent.GetInstance<GeneralBuilderToolSystem>();
		if (GeneralBuilderToolSystem.CurrentMode == GeneralBuilderToolSystem.Fill) {
			Item.autoReuse = false;
		}
		else {
			Item.autoReuse = true;
		}
	}
	public override bool? UseItem(Player player) {
		GeneralBuilderToolSystem system = ModContent.GetInstance<GeneralBuilderToolSystem>();
		if (player.altFunctionUse == 2 && player.ItemAnimationJustStarted) {
			system.ToggleUI();
		}
		else if (GeneralBuilderToolSystem.CurrentMode == GeneralBuilderToolSystem.Fill) {
			return FillFunction(player);
		}
		else if (GeneralBuilderToolSystem.CurrentMode == GeneralBuilderToolSystem.Draw) {
			return DrawFunction(player);
		}
		return base.UseItem(player);
	}
	public bool DrawFunction(Player player) {
		Point point = Main.MouseWorld.ToTileCoordinates();
		if (Main.mouseLeftRelease) {
			oldMousePosition = new();
		}
		if (Main.mouseLeft) {
			if (oldMousePosition.X == 0 && oldMousePosition.Y == 0) {
				oldMousePosition = point;
			}
			GeneralPlaceTileFunction(player, point, oldMousePosition);
		}
		oldMousePosition = point;
		return false;
	}
	public bool FillFunction(Player player) {
		if (Main.mouseLeft) {
			if (position1.X == 0 && position1.Y == 0) {
				position1 = Main.MouseWorld.ToTileCoordinates();
				Main.NewText("First position selected");
				return false;
			}
			if (position2.X == 0 && position2.Y == 0) {
				position2 = Main.MouseWorld.ToTileCoordinates();
				Main.NewText("Second position selected, ready to fill tile");
				return false;
			}
			GeneralPlaceTileFunction(player, position1, position2);
			position1 = new();
			position2 = new();
			Main.NewText("Resetted position");
			return false;
		}
		Main.NewText("Fail to find nearest favorited tile");
		return false;
	}
	private static void GeneralPlaceTileFunction(Player player, Point position1, Point position2) {
		int minX = Math.Min(position1.X, position2.X);
		int maxX = Math.Max(position1.X, position2.X);
		int minY = Math.Min(position1.Y, position2.Y);
		int maxY = Math.Max(position1.Y, position2.Y);
		bool SearchedForTile, SearchedForWall = SearchedForTile = false;
		for (int i = 0; i < player.inventory.Length; i++) {
			if (SearchedForTile && GeneralBuilderToolSystem.Tile && !GeneralBuilderToolSystem.Wall) {
				break;
			}
			else if (SearchedForWall && GeneralBuilderToolSystem.Wall && !GeneralBuilderToolSystem.Tile) {
				break;
			}
			else if (!GeneralBuilderToolSystem.Tile && !GeneralBuilderToolSystem.Wall) {
				break;
			}
			else if (SearchedForTile && SearchedForWall) {
				break;
			}
			Item item = player.inventory[i];
			if (GeneralBuilderToolSystem.Tile && !SearchedForTile && (item.favorited && item.createTile != -1 || GeneralBuilderToolSystem.DeleteMode)) {
				for (int x = minX; x <= maxX; x++) {
					for (int y = minY; y <= maxY; y++) {
						if (GeneralBuilderToolSystem.DeleteMode) {
							WorldGen.KillTile(x, y, noItem: true);
						}
						else if (GeneralBuilderToolSystem.OverrideMode) {
							WorldGen.KillTile(x, y, noItem: true);
							WorldGen.PlaceTile(x, y, item.createTile);
						}
						else {
							WorldGen.PlaceTile(x, y, item.createTile);
						}
					}
				}
				SearchedForTile = true;
				continue;
			}
			if (GeneralBuilderToolSystem.Wall && !SearchedForWall && (item.favorited && item.createWall != -1 || GeneralBuilderToolSystem.DeleteMode)) {
				for (int x = minX; x <= maxX; x++) {
					for (int y = minY; y <= maxY; y++) {
						if (GeneralBuilderToolSystem.DeleteMode) {
							WorldGen.KillWall(x, y);
						}
						else if (GeneralBuilderToolSystem.OverrideMode) {
							WorldGen.KillWall(x, y);
							WorldGen.PlaceWall(x, y, item.createWall);
						}
						else {
							WorldGen.PlaceWall(x, y, item.createWall);
						}
					}
				}
				SearchedForWall = true;
				continue;
			}
		}
	}
}
