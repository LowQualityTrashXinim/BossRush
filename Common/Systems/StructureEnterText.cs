using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BossRush.Common.Systems;
public class Roguelike_TextBox : UITextBox {
	public Roguelike_TextBox(string text, float textScale = 1, bool large = false) : base(text, textScale, large) {
	}
	public bool Hide = false;
	public bool focus = false;
	bool mousePressed = false;
	public override void LeftClick(UIMouseEvent evt) {
	}
	public override void LeftMouseUp(UIMouseEvent evt) {
		mousePressed = false;
	}
	public override void LeftMouseDown(UIMouseEvent evt) {
		focus = true;
		mousePressed = true;
	}
	public override void OnDeactivate() {
		focus = false;
		Main.blockInput = false;
		PlayerInput.WritingText = false;
		SetText("");
	}
	public override void Update(GameTime gameTime) {
		if (IgnoresMouseInteraction || Hide)
			return;
		this.SetTextMaxLength(999);
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}

		if ((Main.mouseLeft && !IsMouseHovering) || Main.inputText.IsKeyDown(Keys.Escape))
			focus = false;

		Main.blockInput = focus;
		focused = focus;
	}
	public bool focused = false;
	// must be inside this drawself method for it to write text like this...
	protected override void DrawSelf(SpriteBatch spriteBatch) {
		if (Hide) {
			return;
		}
		PlayerInput.WritingText = focused;
		string text = Main.GetInputText(Text);

		if (!Text.Equals(text) && focused) {
			SetText(text);
		}

		this._color = focused ? Color.Yellow : Color.White;
		base.DrawSelf(spriteBatch);
	}
}
