using System;
using Terraria;
using Terraria.UI;
using ReLogic.Content;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;

namespace BossRush.Contents.Skill;
class SkillBarUI : UIState {
	// For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
	// Once this is all set up make sure to go and do the required stuff for most UI's in the ModSystem class.
	private UIText text;
	private UIElement area;
	private UIImage barFrame;
	private Color gradientA;
	private Color gradientB;
	private UIText text2;
	private UIElement area2;
	private UIImage barFrame2;
	private Color gradientA2;
	private Color gradientB2;
	public override void OnInitialize() {
		// Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
		// UIElement is invisible and has no padding.
		area = new UIElement();
		area.Left.Set(-area.Width.Pixels - 600, 1f); // Place the resource bar to the left of the hearts.
		area.Top.Set(30, 0f); // Placing it just a bit below the top of the screen.
		area.Width.Set(182, 0f); // We will be placing the following 2 UIElements within this 182x60 area.
		area.Height.Set(60, 0f);

		barFrame = new UIImage(ModContent.Request<Texture2D>(BossRushTexture.EXAMPLEUI)); // Frame of our resource bar
		barFrame.Left.Set(22, 0f);
		barFrame.Top.Set(0, 0f);
		barFrame.Width.Set(138, 0f);
		barFrame.Height.Set(34, 0f);

		text = new UIText("0/0", 0.8f); // text to show stat
		text.Width.Set(138, 0f);
		text.Top.Set(40, 0f);
		text.Left.Set(0, 0f);

		gradientA = new Color(123, 25, 138); // A dark purple
		gradientB = new Color(207, 111, 221); // A light purple

		area.Append(text);
		area.Append(barFrame);
		Append(area);

		area2 = new UIElement();
		area2.Left.Set(-area2.Width.Pixels - 600, 1f); // Place the resource bar to the left of the hearts.
		area2.Top.Set(80, 0f); // Placing it just a bit below the top of the screen.
		area2.Width.Set(182, 0f); // We will be placing the following 2 UIElements within this 182x60 area.
		area2.Height.Set(60, 0f);

		barFrame2 = new UIImage(ModContent.Request<Texture2D>(BossRushTexture.EXAMPLEUI)); // Frame of our resource bar
		barFrame2.Left.Set(22, 0f);
		barFrame2.Top.Set(0, 0f);
		barFrame2.Width.Set(138, 0f);
		barFrame2.Height.Set(34, 0f);

		text2 = new UIText("0/0", 0.8f); // text to show stat
		text2.Width.Set(138, 0f);
		text2.Height.Set(34, 0f);
		text2.Top.Set(40, 0f);
		text2.Left.Set(0, 0f);

		gradientA2 = new Color(123, 25, 138); // A dark purple
		gradientB2 = new Color(207, 111, 221); // A light purple

		area2.Append(text2);
		area2.Append(barFrame2);
		Append(area2);
	}

	// Here we draw our UI
	protected override void DrawSelf(SpriteBatch spriteBatch) {
		base.DrawSelf(spriteBatch);

		var modPlayer = Main.LocalPlayer.GetModPlayer<SkillHandlePlayer>();
		// Calculate quotient
		float quotient = (float)modPlayer.Energy / modPlayer.EnergyCap; // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.
		quotient = Utils.Clamp(quotient, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

		// Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
		Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
		hitbox.X += 12;
		hitbox.Width -= 24;
		hitbox.Y += 8;
		hitbox.Height -= 16;

		// Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
		int left = hitbox.Left;
		int right = hitbox.Right;
		int steps = (int)((right - left) * quotient);
		for (int i = 0; i < steps; i += 1) {
			// float percent = (float)i / steps; // Alternate Gradient Approach
			float percent = (float)i / (right - left);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
		}

		if (modPlayer.CoolDown > 0 && modPlayer.MaximumCoolDown > 0) {
			float quotient2 = modPlayer.CoolDown / (float)modPlayer.MaximumCoolDown; // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.
			quotient2 = Utils.Clamp(quotient2, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

			// Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
			Rectangle hitbox2 = barFrame2.GetInnerDimensions().ToRectangle();
			hitbox2.X += 12;
			hitbox2.Width -= 24;
			hitbox2.Y += 8;
			hitbox2.Height -= 16;

			// Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
			int left2 = hitbox2.Left;
			int right2 = hitbox2.Right;
			int steps2 = (int)((right2 - left2) * quotient2);
			for (int i = 0; i < steps2; i += 1) {
				// float percent = (float)i / steps; // Alternate Gradient Approach
				float percent = (float)i / (right2 - left2);
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left2 + i, hitbox2.Y, 1, hitbox2.Height), Color.Lerp(gradientA2, gradientB2, percent));
			}
		}
	}

	public override void Update(GameTime gameTime) {

		var modPlayer = Main.LocalPlayer.GetModPlayer<SkillHandlePlayer>();
		// Setting the text per tick to update and show our resource values.
		text.SetText($"Energy : {modPlayer.Energy}/{modPlayer.EnergyCap}");
		if (modPlayer.CoolDown > 0) {
			text2.SetText($"CoolDown : {MathF.Round(modPlayer.CoolDown / 60f, 2)}");
		}
		else {
			text2.SetText("");
		}
		base.Update(gameTime);
	}
}
internal class SkillUI : UIState {
	public List<btn_SkillSlotHolder> skill = new List<btn_SkillSlotHolder>();
	public List<btn_SkillSlotHolder> inventory = new List<btn_SkillSlotHolder>();
	public ExitUI exitUI;
	public btn_SkillDeletion btn_delete;
	public const string UItype_SKILL = "skill";
	public const string UIType_INVENTORY = "inventory";

	public override void OnActivate() {
		Player player = Main.LocalPlayer;
		if (player.TryGetModPlayer(out SkillHandlePlayer modplayer)) {
			//Explain : since most likely in the future we aren't gonna expand the skill slot, we just hard set it to 10
			//We are also pre render these UI first
			int[] SkillHolder = modplayer.GetCurrentActiveSkillHolder();
			Vector2 textureSize = new Vector2(52, 52);
			Vector2 OffSetPosition_Skill = player.Center;
			OffSetPosition_Skill.X -= textureSize.X * 5;
			if (skill.Count < 1) {
				Vector2 customOffSet = OffSetPosition_Skill;
				customOffSet.Y -= 60;
				for (int i = 0; i < 3; i++) {
					btn_SkillSlotSelection btn_Selection = new btn_SkillSlotSelection(TextureAssets.InventoryBack7, modplayer, i + 1);
					btn_Selection.UISetPosition(customOffSet + new Vector2(52, 0) * i, textureSize);
					Append(btn_Selection);
				}
				for (int i = 0; i < 10; i++) {
					btn_SkillSlotHolder skillslot = new btn_SkillSlotHolder(TextureAssets.InventoryBack, player, i, SkillHolder[i], UItype_SKILL);
					skillslot.UISetPosition(OffSetPosition_Skill + new Vector2(52, 0) * i, textureSize);
					skill.Add(skillslot);
					Append(skill[i]);
				}
			}
			Vector2 InvOffSet = new Vector2(520, -55);
			if (inventory.Count < 1) {
				for (int i = 0; i < 30; i++) {
					btn_SkillSlotHolder skillslot = new btn_SkillSlotHolder(TextureAssets.InventoryBack, player, i, modplayer.SkillInventory[i], UIType_INVENTORY);
					Vector2 InvPos = OffSetPosition_Skill + new Vector2(0, 72);
					if (i >= 10) {
						InvPos -= InvOffSet;
					}
					if (i >= 20) {
						InvPos -= InvOffSet;
					}
					skillslot.UISetPosition(InvPos + new Vector2(52, 0) * i, textureSize);
					inventory.Add(skillslot);
					Append(inventory[i]);
				}
			}
			if (exitUI == null) {
				exitUI = new ExitUI(TextureAssets.InventoryBack10);
				exitUI.UISetPosition(player.Center + new Vector2(300, 0), textureSize);
				Append(exitUI);
			}
			if (btn_delete == null) {
				btn_delete = new btn_SkillDeletion(TextureAssets.InventoryBack, modplayer);
				btn_delete.UISetPosition(player.Center - new Vector2(330, 0), textureSize);
				Append(btn_delete);
			}
		}
	}
	public override void OnDeactivate() {
		SkillModSystem.SelectInventoryIndex = -1;
		SkillModSystem.SelectSkillIndex = -1;
	}
}
class btn_SkillSlotSelection : UIImage {
	int SelectionIndex = 0;
	SkillHandlePlayer modplayer;
	public btn_SkillSlotSelection(Asset<Texture2D> texture, SkillHandlePlayer modplayer, int selection) : base(texture) {
		SelectionIndex = selection;
		this.modplayer = modplayer;
	}
	public override void LeftClick(UIMouseEvent evt) {
		base.LeftClick(evt);
		if (SelectionIndex == 0) {
			return;
		}
		modplayer.ChangeHolder(SelectionIndex);
	}
	public override void Draw(SpriteBatch spriteBatch) {
		if (SelectionIndex != modplayer.CurrentActiveIndex) {
			Color = new Color(255, 255, 255, 100);
		}
		else {
			Color = Color.White;
		}
		base.Draw(spriteBatch);
	}
}
class btn_SkillDeletion : UIImage {
	SkillHandlePlayer modplayer;
	Vector2 size;
	public btn_SkillDeletion(Asset<Texture2D> texture, SkillHandlePlayer modplayer) : base(texture) {
		this.modplayer = modplayer;
		size = texture.Size();
	}
	public override void LeftClick(UIMouseEvent evt) {
		if (SkillModSystem.SelectInventoryIndex != -1) {
			modplayer.RequestSkillRemoval_SkillInventory(SkillModSystem.SelectInventoryIndex);
			SkillModSystem.SelectInventoryIndex = -1;
		}
		if (SkillModSystem.SelectSkillIndex != -1) {
			modplayer.RequestSkillRemoval_SkillHolder(SkillModSystem.SelectSkillIndex);
			SkillModSystem.SelectSkillIndex = -1;
		}
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		Vector2 drawpos = new Vector2(Left.Pixels, Top.Pixels) + size * .5f;
		Texture2D trashbin = TextureAssets.Trash.Value;
		float scaling = ScaleCalculation(size, trashbin.Size());
		Vector2 origin = trashbin.Size() * .5f;
		spriteBatch.Draw(trashbin, drawpos, null, Color.White, 0, origin, scaling, SpriteEffects.None, 0);
	}
	private float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / (textureSize.Length() * 1.5f);
}
class btn_SkillSlotHolder : UIImageButton {
	public int whoAmI = -1;
	public int sKillID = -1;
	public string uitype = "";
	Player player;
	Texture2D Texture;
	SkillHandlePlayer modplayer;
	public btn_SkillSlotHolder(Asset<Texture2D> texture, Player Tplayer, int WhoAmI, int SkillID, string UItype) : base(texture) {
		player = Tplayer;
		whoAmI = WhoAmI;
		sKillID = SkillID;
		Texture = texture.Value;
		uitype = UItype;
		modplayer = player.GetModPlayer<SkillHandlePlayer>();
		SetVisibility(1, .67f);
	}
	public override void LeftClick(UIMouseEvent evt) {
		//Moving skill around in inventory
		if (uitype == SkillUI.UIType_INVENTORY) {
			if (SkillModSystem.SelectInventoryIndex == -1) {
				if (SkillModSystem.SelectSkillIndex == -1) {
					//This mean the player haven't select anything
					SkillModSystem.SelectInventoryIndex = whoAmI;
				}
				else {
					//Player are Attempting to move a skill from their skill slot back to inventory
					modplayer.ReplaceSkillFromSkillHolderToInv(SkillModSystem.SelectSkillIndex, whoAmI);
					SkillModSystem.SelectSkillIndex = -1;
				}
			}
			else {
				//Player are moving skill around their inventory
				int cache = modplayer.SkillInventory[whoAmI];
				modplayer.SkillInventory[whoAmI] = modplayer.SkillInventory[SkillModSystem.SelectInventoryIndex];
				modplayer.SkillInventory[SkillModSystem.SelectInventoryIndex] = cache;
				SkillModSystem.SelectInventoryIndex = -1;
				//It is impossible where SelectSkillIndex can't be equal to -1
			}
		}
		else if (uitype == SkillUI.UItype_SKILL) {
			if (SkillModSystem.SelectSkillIndex == -1) {
				if (SkillModSystem.SelectInventoryIndex == -1) {
					//This mean the player haven't select anything
					SkillModSystem.SelectSkillIndex = whoAmI;
				}
				else {
					//Player are Attempting to move a skill from their inventory into a skill holder
					modplayer.ReplaceSkillFromInvToSkillHolder(whoAmI, SkillModSystem.SelectInventoryIndex);
					SkillModSystem.SelectInventoryIndex = -1;
				}
			}
			else {
				//Player are moving skill around their skill holder
				modplayer.SwitchSkill(whoAmI, SkillModSystem.SelectSkillIndex);
				SkillModSystem.SelectSkillIndex = -1;
			}
		}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (uitype == SkillUI.UIType_INVENTORY) {
			if (modplayer.SkillInventory[whoAmI] != sKillID) {
				sKillID = modplayer.SkillInventory[whoAmI];
			}
		}
		else if (uitype == SkillUI.UItype_SKILL) {
			int[] skillholder = modplayer.GetCurrentActiveSkillHolder();
			if (skillholder[whoAmI] != sKillID) {
				sKillID = skillholder[whoAmI];
			}
		}
		if (IsMouseHovering) {
			string tooltipText = "";
			string Name = "";
			if (SkillLoader.GetSkill(sKillID) != null) {
				Name = SkillLoader.GetSkill(sKillID).DisplayName;
				tooltipText = SkillLoader.GetSkill(sKillID).Description;
				tooltipText +=
					$"\n[c/{Color.Yellow.Hex3()}:Skill duration] : {Math.Round(SkillLoader.GetSkill(sKillID).Duration / 60f, 2)}" +
					$"\n[c/{Color.DodgerBlue.Hex3()}:Energy require] : {SkillLoader.GetSkill(sKillID).EnergyRequire}" +
					$"\n[c/{Color.OrangeRed.Hex3()}:Skill cooldown] : {Math.Round(SkillLoader.GetSkill(sKillID).CoolDown /60f, 2)}";
			}
			Main.instance.MouseText(Name + "\n" + tooltipText);
		}
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		if (sKillID < 0 || sKillID >= SkillLoader.TotalCount) {
			return;
		}
		Vector2 drawpos = new Vector2(Left.Pixels, Top.Pixels) + Texture.Size() * .5f;
		Texture2D skilltexture = ModContent.Request<Texture2D>(SkillLoader.GetSkill(sKillID).Texture).Value;
		Vector2 origin = skilltexture.Size() * .5f;
		float scaling = ScaleCalculation(Texture.Size(), skilltexture.Size());
		spriteBatch.Draw(skilltexture, drawpos, null, Color.White, 0, origin, scaling, SpriteEffects.None, 0);
	}
	private float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / (textureSize.Length() * 1.5f);
}
