using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace BossRush.Common.Systems;
public class StructureEnterText_State : UIState {
	Vector2 position;
	StructStructureEnterText_TextBox textBar;
	UIText SaveButton;
	UIText ExitButton;

	public override void OnInitialize() {


		Main.blockInput = true;
		textBar = new("");
		SaveButton = new("Save");
		ExitButton = new("Exit");

		textBar.UISetWidthHeight(64, 16);
		textBar.SetTextMaxLength(75);
		SaveButton.UISetPosition(new Vector2(10, 46));
		ExitButton.UISetPosition(new Vector2(10, 46 * 1.5f)); ;

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

	public override void OnActivate() {
		position = Main.ScreenSize.ToVector2() / 2f * (1 / Main.UIScale);
		this.UISetPosition(position);
	}
	public override void OnDeactivate() {
		Main.blockInput = false;
		PlayerInput.WritingText = false;
	}
}

public class StructStructureEnterText_TextBox : UITextBox {
	public StructStructureEnterText_TextBox(string text, float textScale = 1, bool large = false) : base(text, textScale, large) {
	}

	// must be inside this drawself method for it to write text like this...
	protected override void DrawSelf(SpriteBatch spriteBatch) {
		PlayerInput.WritingText = true;
		Main.instance.HandleIME();
		string text = Main.GetInputText(Text);

		if (!Text.Equals(text)) {
			SetText(text);
		}
		base.DrawSelf(spriteBatch);
	}
}
