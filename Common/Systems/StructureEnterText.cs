using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Input;

namespace BossRush.Common.Systems;

public class StructureEnterText : UIElement {

	public StructureEnterText() {
		Width.Precent = 1f;
		Height.Precent = 1f;
		Recalculate();
	}

	StructStructureEnterText_TextBox textBar;
	UITextBox SaveButton;
	UITextBox ExitButton;
	public bool focus = false;
	bool mousePressed = false;
	public override void OnInitialize() {
		textBar = new("");
		ExitButton = new("Exit");
		ExitButton.UISetPosition(new Vector2(10, 46 * 1.5f));
		SaveButton = new("Save");
		SaveButton.UISetPosition(new Vector2(10, 46));

		textBar.UISetWidthHeight(64, 16);
		textBar.SetTextMaxLength(75);
		textBar.OnLeftMouseDown += (a, b) => {
			focus = true;
			mousePressed = true;
		};

		textBar.OnLeftMouseUp += (a, b) => {
			mousePressed = false;
		};


		ExitButton.OnLeftClick += (a, b) => {
			ModContent.GetInstance<UniversalSystem>().DeactivateUI();
		};
		SaveButton.OnLeftClick += (a, b) => {
			// textBar.Text is the value you want to set structure name
			BossRushUtils.CombatTextRevamp(Main.LocalPlayer.Hitbox, Color.White, textBar.Text);
			ModContent.GetInstance<UniversalSystem>().DeactivateUI();
		};



		Append(textBar);
		Append(SaveButton);
		Append(ExitButton);

	}

	public override void Update(GameTime gameTime) {
		if (IgnoresMouseInteraction)
			return;

		if (mousePressed)
			this.UISetPosition(Vector2.Clamp(Main.MouseScreen - new Vector2(textBar.Width.Pixels / 2f, textBar.Height.Pixels / 2f), Vector2.Zero + new Vector2(60), Main.ScreenSize.ToVector2() - new Vector2(60) * Main.UIScale));
		this.UISetPosition(Vector2.Clamp(new Vector2(this.Left.Pixels, this.Top.Pixels), Vector2.Zero + new Vector2(60), Main.ScreenSize.ToVector2() - new Vector2(60) * Main.UIScale));

		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}

		if ((Main.mouseLeft && !textBar.IsMouseHovering) || Main.inputText.IsKeyDown(Keys.Escape))
			focus = false;

		Main.blockInput = focus;
		textBar.focused = focus;
	}

	public override void OnActivate() {
		this.UISetPosition(Main.ScreenSize.ToVector2() / 2f);
	}
	public override void OnDeactivate() {
		focus = false;
		Main.blockInput = false;
		PlayerInput.WritingText = false;
		textBar.SetText("");
	}
}

public class StructStructureEnterText_TextBox : UITextBox {
	public StructStructureEnterText_TextBox(string text, float textScale = 1, bool large = false) : base(text, textScale, large) {
	}
	public bool focused = false;
	// must be inside this drawself method for it to write text like this...
	protected override void DrawSelf(SpriteBatch spriteBatch) {
		PlayerInput.WritingText = focused;
		string text = Main.GetInputText(Text);

		if (!Text.Equals(text) && focused) {
			SetText(text);
		}

		this._color = focused ? Color.Yellow : Color.White;
		base.DrawSelf(spriteBatch);
	}
}
