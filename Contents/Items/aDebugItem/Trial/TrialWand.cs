using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.UI;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using BossRush.Texture;
using Terraria.GameContent.UI.Elements;

namespace BossRush.Contents.Items.aDebugItem.Trial;
internal class TrialWand : ModItem {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.CelestialWand);
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.useAnimation = Item.useTime = 1;
		Item.noUseGraphic = true;
		Item.consumable = false;
		Item.autoReuse = true;
	}
	public override bool? UseItem(Player player) {
		return true;
	}
}
public class Roguelike_TrialUI : UIState {
	public UIPanel Panel;
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
		Panel.Left.Percent = .5f;
		Panel.Top.Percent = .5f;
		Panel.BackgroundColor.A = 255;
		Panel.OnLeftMouseDown += Panel_OnLeftMouseDown;
		Panel.OnLeftMouseUp += Panel_OnLeftMouseUp;
		Panel.OnUpdate += Panel_OnUpdate;
		Append(Panel);

		DrawMode = new(thesameuitextureasvanilla);
		DrawMode.SetPostTex(ModContent.Request<Texture2D>(BossRushTexture.DrawBrush));
		DrawMode.VAlign = .5f;
		DrawMode.HighlightColor = DrawMode.OriginalColor * .5f;
		DrawMode.SwapHightlightColorWithOriginalColor();
		DrawMode.OnLeftClick += null;
		DrawMode.HoverText = "Draw mode";
		Panel.Append(DrawMode);

		FillMode = new(thesameuitextureasvanilla);
		FillMode.SetPostTex(ModContent.Request<Texture2D>(BossRushTexture.FillBucket));
		FillMode.VAlign = .5f;
		FillMode.HAlign = MathHelper.Lerp(0, 1f, 1 / 5f);
		FillMode.HighlightColor = FillMode.OriginalColor * .5f;
		FillMode.SwapHightlightColorWithOriginalColor();
		FillMode.OnLeftClick += null;
		FillMode.HoverText = "Fill mode";
		Panel.Append(FillMode);

		DeleteMode = new(thesameuitextureasvanilla);
		DeleteMode.SetPostTex(TextureAssets.Trash);
		DeleteMode.VAlign = .5f;
		DeleteMode.HAlign = MathHelper.Lerp(0, 1f, 2 / 5f);
		DeleteMode.HighlightColor = DeleteMode.OriginalColor * .5f;
		DeleteMode.OnLeftClick += null;
		DeleteMode.SwapHightlightColorWithOriginalColor();
		DeleteMode.HoverText = "Delete mode";
		Panel.Append(DeleteMode);

		OverrideMode = new(thesameuitextureasvanilla);
		OverrideMode.VAlign = .5f;
		OverrideMode.HAlign = MathHelper.Lerp(0, 1f, 3 / 5f);
		OverrideMode.HighlightColor = OverrideMode.OriginalColor * .5f;
		OverrideMode.OnLeftClick += null;
		OverrideMode.SwapHightlightColorWithOriginalColor();
		OverrideMode.HoverText = "Override mode";
		Panel.Append(OverrideMode);

		TileMode = new(thesameuitextureasvanilla);
		TileMode.SetPostTex(TextureAssets.Item[ItemID.StoneBlock], attemptToLoad: true);
		TileMode.VAlign = .5f;
		TileMode.HAlign = MathHelper.Lerp(0, 1f, 4 / 5f);
		TileMode.HighlightColor = TileMode.OriginalColor * .5f;
		TileMode.OnLeftClick += null;
		TileMode.SwapHightlightColorWithOriginalColor();
		TileMode.HoverText = "Tile";
		Panel.Append(TileMode);

		WallMode = new(thesameuitextureasvanilla);
		WallMode.SetPostTex(TextureAssets.Item[ItemID.StoneWall], attemptToLoad: true);
		WallMode.VAlign = .5f;
		WallMode.HAlign = 1;
		WallMode.HighlightColor = WallMode.OriginalColor * .5f;
		WallMode.OnLeftClick += null;
		WallMode.SwapHightlightColorWithOriginalColor();
		WallMode.HoverText = "Wall";
		Panel.Append(WallMode);
	}

	public override void LeftClick(UIMouseEvent evt) {
	}
	Vector2 offsetExtra = Vector2.Zero;
	Vector2 lastPressedPositionDistance = Vector2.Zero;
	private void Panel_OnLeftMouseUp(UIMouseEvent evt, UIElement listeningElement) {
		if (evt.Target != listeningElement) {
			return;
		}
		dragging = false;
	}

	private void Panel_OnLeftMouseDown(UIMouseEvent evt, UIElement listeningElement) {
		if (evt.Target != listeningElement) {
			return;
		}
		if (!dragging) {
			offsetExtra = lastPressedPositionDistance - evt.MousePosition;
		}
		dragging = true;
		Vector2 pos = Panel.GetInnerDimensions().Position();
		Vector2 distanceFromWhereTopLeftToMouse = pos - evt.MousePosition;
		offset = distanceFromWhereTopLeftToMouse;
	}

	private void Panel_OnUpdate(UIElement affectedElement) {

		if (dragging) {
			Panel.Left.Set(Main.mouseX - offset.X + offsetExtra.X * 2, 0f); // Main.MouseScreen.X and Main.mouseX are the same
			Panel.Top.Set(Main.mouseY - offset.Y + offsetExtra.Y * 2, 0f);
			Panel.Recalculate();
		}
		else {
			lastPressedPositionDistance = Panel.GetOuterDimensions().Position();
		}
	}

	private Vector2 offset;
	private bool dragging;
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);

	}
}

public class TrialWandSystem : ModSystem {
	public static TrialWandSystem instance => ModContent.GetInstance<TrialWandSystem>();
	public bool CheckTrialSizeSet => trialSize.X != 0 && trialSize.Y != 0 && trialSize.Width != 0 && trialSize.Height != 0;
	public Rectangle trialSize = new Rectangle();
	public List<TrialNPCCord> cords = new();
}
public class TrialNPCCord {
	public int NPCType;
	public Point TilePosition;
}
