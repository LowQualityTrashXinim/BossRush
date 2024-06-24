using System;
using Terraria;
using Terraria.UI;
using Terraria.ID;
using System.Linq;
using ReLogic.Content;
using BossRush.Texture;
using ReLogic.Graphics;
using Terraria.UI.Chat;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.GameContent;
using BossRush.Contents.Perks;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items;
using BossRush.Contents.Skill;
using System.Collections.Generic;
using BossRush.Contents.Items.Card;
using BossRush.Contents.Items.Chest;
using Terraria.GameContent.UI.States;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using BossRush.Contents.WeaponEnchantment;

namespace BossRush.Common.Systems;
/// <summary>
/// This not only include main stuff that make everything work but also contain some fixes to vanilla
/// </summary>
internal class UniversalSystem : ModSystem {
	public const string SYNERGY_MODE = "SynergyModeEnable";
	public const string BOSSRUSH_MODE = "ChallengeModeEnable";
	public const string NIGHTMARE_MODE = "NightmareEnable";
	public const string HARDCORE_MODE = "Hardcore";
	public const string TRUE_MODE = "TrueMode";
	/// <summary>
	/// Use this to lock content behind hardcore
	/// </summary>
	/// <param name="player"></param>
	/// <param name="context">Use <see cref="BOSSRUSH_MODE"/> or any kind of mode that seem fit</param>
	/// <returns></returns>
	public static bool CanAccessContent(Player player, string context) {
		BossRushModConfig config = ModContent.GetInstance<BossRushModConfig>();
		if (config.HardEnableFeature || player.IsDebugPlayer())
			return true;
		if (context == NIGHTMARE_MODE)
			return config.Nightmare;
		if (context == HARDCORE_MODE)
			return player.difficulty == PlayerDifficultyID.Hardcore || config.AutoHardCore;
		if (context == BOSSRUSH_MODE)
			return player.difficulty == PlayerDifficultyID.Hardcore && config.BossRushMode;
		if (context == SYNERGY_MODE)
			return player.difficulty == PlayerDifficultyID.Hardcore && config.SynergyMode;
		if (context == TRUE_MODE)
			return player.difficulty == PlayerDifficultyID.Hardcore && config.SynergyMode && config.BossRushMode;
		return false;
	}
	public static bool CanAccessContent(string context) {
		BossRushModConfig config = ModContent.GetInstance<BossRushModConfig>();
		if (context == BOSSRUSH_MODE)
			return config.BossRushMode;
		if (config.HardEnableFeature)
			return true;
		if (context == NIGHTMARE_MODE)
			return config.Nightmare;
		if (context == HARDCORE_MODE)
			return config.AutoHardCore;
		if (context == SYNERGY_MODE)
			return config.SynergyMode;
		if (context == TRUE_MODE)
			return config.SynergyMode && config.BossRushMode;
		return false;
	}
	internal UserInterface userInterface;
	public EnchantmentUIState Enchant_uiState;
	public PerkUIState perkUIstate;
	public SkillUI skillUIstate;
	public DefaultUI defaultUI;

	public static bool EnchantingState = false;
	public override void Load() {

		//UI stuff
		if (!Main.dedServ) {
			//Mod custom UI
			Enchant_uiState = new();
			perkUIstate = new();

			skillUIstate = new();
			defaultUI = new();

			userInterface = new();
		}
		On_UIElement.OnActivate += On_UIElement_OnActivate;
	}

	private void On_UIElement_OnActivate(On_UIElement.orig_OnActivate orig, UIElement self) {
		try {
			if (ModContent.GetInstance<BossRushModConfig>().AutoRandomizeCharacter) {
				if (self is UICharacterCreation el && Main.MenuUI.CurrentState is UICharacterCreation) {
					MethodInfo method = typeof(UICharacterCreation).GetMethod("Click_RandomizePlayer", BindingFlags.NonPublic | BindingFlags.Instance);
					method.Invoke(el, new object[] { null, null });
				}
			}
		}
		finally {
			orig(self);
		}
	}
	public override void Unload() {
	}
	public override void UpdateUI(GameTime gameTime) {
		userInterface?.Update(gameTime);
	}
	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
		int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
		if (InventoryIndex != -1)
			layers.Insert(InventoryIndex, new LegacyGameInterfaceLayer(
				"BossRush: UI",
				delegate {
					userInterface.Draw(Main.spriteBatch, new GameTime());
					return true;
				},
				InterfaceScaleType.UI)
			);
	}
	public void SetState(UIState state) {
		if (userInterface.CurrentState == null || userInterface.CurrentState == defaultUI) {
			userInterface.SetState(state);
		}
	}
	public void DeactivateState() {
		if (userInterface.CurrentState != null) {
			userInterface.SetState(defaultUI);
		}
	}
}
public class UniversalGlobalBuff : GlobalBuff {
	public override void SetStaticDefaults() {
		//I am unsure why this is set to true
		Main.debuff[BuffID.Campfire] = false;
		Main.debuff[BuffID.Honey] = false;
		Main.debuff[BuffID.StarInBottle] = false;
		Main.debuff[BuffID.HeartLamp] = false;
		Main.debuff[BuffID.CatBast] = false;
		Main.debuff[BuffID.Sunflower] = false;
	}
}
public class UniversalGlobalItem : GlobalItem {
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (!UniversalSystem.EnchantingState)
			return;
		for (int i = 0; i < tooltips.Count; i++) {
			if (tooltips[i].Name == "ItemName") {
				string tooltipText = "No enchantment can be found";
				if (EnchantmentLoader.GetEnchantmentItemID(item.type) != null) {
					tooltipText = EnchantmentLoader.GetEnchantmentItemID(item.type).Description;
				}
				tooltips[i].Text = tooltipText;
				continue;
			}
			tooltips[i].Hide();
		}
	}
}
public class UniversalModPlayer : ModPlayer {
	public override void OnEnterWorld() {
		var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		uiSystemInstance.SetState(uiSystemInstance.defaultUI);
	}
	public override bool CanUseItem(Item item) {
		var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		if (uiSystemInstance.userInterface.CurrentState != null && uiSystemInstance.userInterface.CurrentState != uiSystemInstance.defaultUI) {
			return false;
		}
		return base.CanUseItem(item);
	}
}

class DefaultUI : UIState {
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

	private List<UIImage> perkShowcase;
	public override void OnInitialize() {
		CreateEnergyBar();
		CreateCoolDownBar();
		//CreateDefaultSkillBTN();
	}
	//private void CreateDefaultSkillBTN() {
	//	Player player = Main.LocalPlayer;
	//	if (player.TryGetModPlayer(out SkillHandlePlayer modplayer)) {

	//		Vector2 textureSize = new Vector2(52, 52);
	//		Vector2 OffSetPosition_Skill = player.Center;
	//		OffSetPosition_Skill.X -= textureSize.X * 5;
	//		if (skill.Count < 1) {
	//			Vector2 customOffSet = OffSetPosition_Skill;
	//			customOffSet.Y -= 60;
	//			for (int i = 0; i < 3; i++) {
	//				btn_SkillSlotSelection btn_Selection = new btn_SkillSlotSelection(TextureAssets.InventoryBack7, modplayer, i + 1);
	//				btn_Selection.UISetPosition(customOffSet + new Vector2(52, 0) * i, textureSize);
	//				Append(btn_Selection);
	//			}
	//			for (int i = 0; i < 10; i++) {
	//				btn_SkillSlotHolder skillslot = new btn_SkillSlotHolder(TextureAssets.InventoryBack, player, i, SkillHolder[i], UItype_SKILL);
	//				skillslot.UISetPosition(OffSetPosition_Skill + new Vector2(52, 0) * i, textureSize);
	//				skill.Add(skillslot);
	//				Append(skill[i]);
	//			}
	//		}
	//		Vector2 InvOffSet = new Vector2(520, -55);
	//		if (inventory.Count < 1) {
	//			for (int i = 0; i < 30; i++) {
	//				btn_SkillSlotHolder skillslot = new btn_SkillSlotHolder(TextureAssets.InventoryBack, player, i, modplayer.SkillInventory[i], UIType_INVENTORY);
	//				Vector2 InvPos = OffSetPosition_Skill + new Vector2(0, 72);
	//				if (i >= 10) {
	//					InvPos -= InvOffSet;
	//				}
	//				if (i >= 20) {
	//					InvPos -= InvOffSet;
	//				}
	//				skillslot.UISetPosition(InvPos + new Vector2(52, 0) * i, textureSize);
	//				inventory.Add(skillslot);
	//				Append(inventory[i]);
	//			}
	//		}
	//	}
	//}
	private void CreateEnergyBar() {
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
	}
	private void CreateCoolDownBar() {
		area2 = new UIElement();
		area2.Left.Set(-area2.Width.Pixels - 600, 1f);
		area2.Top.Set(80, 0f);
		area2.Width.Set(182, 0f);
		area2.Height.Set(60, 0f);

		barFrame2 = new UIImage(ModContent.Request<Texture2D>(BossRushTexture.EXAMPLEUI));
		barFrame2.Left.Set(22, 0f);
		barFrame2.Top.Set(0, 0f);
		barFrame2.Width.Set(138, 0f);
		barFrame2.Height.Set(34, 0f);

		text2 = new UIText("0/0", 0.8f);
		text2.Width.Set(138, 0f);
		text2.Height.Set(34, 0f);
		text2.Top.Set(40, 0f);
		text2.Left.Set(0, 0f);

		gradientA2 = new Color(123, 25, 138);
		gradientB2 = new Color(207, 111, 221);

		area2.Append(text2);
		area2.Append(barFrame2);
		Append(area2);
	}
	protected override void DrawSelf(SpriteBatch spriteBatch) {
		base.DrawSelf(spriteBatch);
		DrawSkillProgressBarUI(spriteBatch);
	}
	private void DrawSkillProgressBarUI(SpriteBatch spriteBatch) {
		var modPlayer = Main.LocalPlayer.GetModPlayer<SkillHandlePlayer>();
		// Calculate quotient
		float quotient = (float)modPlayer.Energy / modPlayer.EnergyCap; // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.
		quotient = Math.Clamp(quotient, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

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
			quotient2 = Math.Clamp(quotient2, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

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


	//public List<btn_SkillSlotHolder> skill = new List<btn_SkillSlotHolder>();
	//public List<btn_SkillSlotHolder> inventory = new List<btn_SkillSlotHolder>();
	//private ExitUI exitUI;
	//private btn_SkillDeletion btn_delete;
	//public const string UItype_SKILL = "skill";
	//public const string UIType_INVENTORY = "inventory";
	//private void ActivateSkillUI(Player player) {
	//	if (player.TryGetModPlayer(out SkillHandlePlayer modplayer)) {
	//		//Explain : since most likely in the future we aren't gonna expand the skill slot, we just hard set it to 10
	//		//We are also pre render these UI first
	//		int[] SkillHolder = modplayer.GetCurrentActiveSkillHolder();
	//		Vector2 textureSize = new Vector2(52, 52);
	//		Vector2 OffSetPosition_Skill = player.Center;
	//		OffSetPosition_Skill.X -= textureSize.X * 5;
	//		if (skill.Count < 1) {
	//			Vector2 customOffSet = OffSetPosition_Skill;
	//			customOffSet.Y -= 60;
	//			for (int i = 0; i < 3; i++) {
	//				btn_SkillSlotSelection btn_Selection = new btn_SkillSlotSelection(TextureAssets.InventoryBack7, modplayer, i + 1);
	//				btn_Selection.UISetPosition(customOffSet + new Vector2(52, 0) * i, textureSize);
	//				Append(btn_Selection);
	//			}
	//			for (int i = 0; i < 10; i++) {
	//				btn_SkillSlotHolder skillslot = new btn_SkillSlotHolder(TextureAssets.InventoryBack, player, i, SkillHolder[i], UItype_SKILL);
	//				skillslot.UISetPosition(OffSetPosition_Skill + new Vector2(52, 0) * i, textureSize);
	//				skill.Add(skillslot);
	//				Append(skill[i]);
	//			}
	//		}
	//		Vector2 InvOffSet = new Vector2(520, -55);
	//		if (inventory.Count < 1) {
	//			for (int i = 0; i < 30; i++) {
	//				btn_SkillSlotHolder skillslot = new btn_SkillSlotHolder(TextureAssets.InventoryBack, player, i, modplayer.SkillInventory[i], UIType_INVENTORY);
	//				Vector2 InvPos = OffSetPosition_Skill + new Vector2(0, 72);
	//				if (i >= 10) {
	//					InvPos -= InvOffSet;
	//				}
	//				if (i >= 20) {
	//					InvPos -= InvOffSet;
	//				}
	//				skillslot.UISetPosition(InvPos + new Vector2(52, 0) * i, textureSize);
	//				inventory.Add(skillslot);
	//				Append(inventory[i]);
	//			}
	//		}
	//		if (exitUI == null) {
	//			exitUI = new ExitUI(TextureAssets.InventoryBack10);
	//			exitUI.UISetPosition(player.Center + new Vector2(300, 0), textureSize);
	//			Append(exitUI);
	//		}
	//		if (btn_delete == null) {
	//			btn_delete = new btn_SkillDeletion(TextureAssets.InventoryBack, modplayer);
	//			btn_delete.UISetPosition(player.Center - new Vector2(330, 0), textureSize);
	//			Append(btn_delete);
	//		}
	//	}
	//}
	//public override void OnActivate() {
	//	Player player = Main.LocalPlayer;
	//	ActivateSkillUI(player);
	//}
	//public override void OnDeactivate() {
	//	SkillModSystem.SelectInventoryIndex = -1;
	//	SkillModSystem.SelectSkillIndex = -1;
	//}
}
internal class SkillUI : UIState {
	public List<btn_SkillSlotHolder> skill = new List<btn_SkillSlotHolder>();
	public List<btn_SkillSlotHolder> inventory = new List<btn_SkillSlotHolder>();
	public ExitUI exitUI;
	public btn_SkillDeletion btn_delete;
	public const string UItype_SKILL = "skill";
	public const string UIType_INVENTORY = "inventory";
	private void ActivateSkillUI(Player player) {
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
	public override void OnActivate() {
		Player player = Main.LocalPlayer;
		ActivateSkillUI(player);
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
					$"\n[c/{Color.Yellow.Hex3()}:Skill duration] : {Math.Round(SkillLoader.GetSkill(sKillID).Duration / 60f, 2)}s" +
					$"\n[c/{Color.DodgerBlue.Hex3()}:Energy require] : {SkillLoader.GetSkill(sKillID).EnergyRequire}" +
					$"\n[c/{Color.OrangeRed.Hex3()}:Skill cooldown] : {Math.Round(SkillLoader.GetSkill(sKillID).CoolDown / 60f, 2)}s";
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
internal class PerkUIState : UIState {
	public const short DefaultState = 0;
	public const short StarterPerkState = 1;
	public const short DebugState = 2;
	public int whoAmI = -1;
	public short StateofState = 0;
	public UIText toolTip;
	public override void OnActivate() {
		Elements.Clear();
		if (whoAmI == -1)
			return;
		Player player = Main.player[whoAmI];
		if (player.TryGetModPlayer(out PerkPlayer modplayer)) {
			if (StateofState == DefaultState) {
				ActivateNormalPerkUI(modplayer, player);
			}
			if (StateofState == StarterPerkState) {
				ActivateStarterPerkUI(modplayer, player);
			}
			if (StateofState == DebugState) {
				ActivateDebugPerkUI(modplayer, player);
			}
		}
		toolTip = new UIText("");
		Append(toolTip);
	}
	private void ActivateDebugPerkUI(PerkPlayer modplayer, Player player) {
		int amount = ModPerkLoader.TotalCount;
		Vector2 originDefault = new Vector2(26, 26);
		for (int i = 0; i < amount + 1; i++) {
			Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(amount + 1, 360, i) * Math.Clamp(amount * 20, 0, 260);
			if (i >= ModPerkLoader.TotalCount) {
				UIImageButton weapon =
						 new MaterialCardUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT));
				weapon.UISetWidthHeight(52, 52);
				weapon.UISetPosition(player.Center + offsetPos, originDefault);
				Append(weapon);
				i++;
				offsetPos = Vector2.UnitY.Vector2DistributeEvenly(amount + 2, 360, i) * Math.Clamp(amount * 20, 0, 260);
				UIImageButton weapon2 =
						 new MaterialWeaponUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.SUPPILESDROP));
				weapon2.UISetWidthHeight(52, 52);
				weapon2.UISetPosition(player.Center + offsetPos, originDefault);
				Append(weapon2);
				break;
			}
			Asset<Texture2D> texture;
			if (ModPerkLoader.GetPerk(i).textureString is not null)
				texture = ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(i).textureString);
			else
				texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
			//After that we assign perk
			PerkUIImageButton btn = new PerkUIImageButton(texture, modplayer);
			btn.UISetWidthHeight(52, 52);
			btn.UISetPosition(player.Center + offsetPos, originDefault);
			btn.perkType = i;
			Append(btn);
		}
	}
	private void ActivateNormalPerkUI(PerkPlayer modplayer, Player player) {
		List<int> listOfPerk = new List<int>();
		for (int i = 0; i < ModPerkLoader.TotalCount; i++) {
			if (modplayer.perks.ContainsKey(i)) {
				if (!ModPerkLoader.GetPerk(i).CanBeStack && modplayer.perks[i] > 0
					|| modplayer.perks[i] >= ModPerkLoader.GetPerk(i).StackLimit) {
					continue;
				}
			}
			if (!ModPerkLoader.GetPerk(i).CanBeChoosen) {
				continue;
			}
			listOfPerk.Add(i);
		}
		int amount = listOfPerk.Count;
		Vector2 originDefault = new Vector2(26, 26);
		int perkamount = modplayer.PerkAmountModified();
		for (int i = 0; i < perkamount; i++) {
			Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(perkamount, 360, i) * Math.Clamp(perkamount * 20, 0, 200);
			if (i >= amount || i >= perkamount - 1) {
				UIImageButton buttonWeapon = Main.rand.Next(new UIImageButton[]
				{
						 new MaterialCardUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT)),
						 new MaterialWeaponUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.SUPPILESDROP))
				});
				buttonWeapon.UISetWidthHeight(52, 52);
				buttonWeapon.UISetPosition(player.Center + offsetPos, originDefault);
				Append(buttonWeapon);
				continue;
			}
			int newperk = Main.rand.Next(listOfPerk);
			Asset<Texture2D> texture;
			if (ModPerkLoader.GetPerk(newperk).textureString is not null)
				texture = ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(newperk).textureString);
			else
				texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
			listOfPerk.Remove(newperk);
			//After that we assign perk
			PerkUIImageButton btn = new PerkUIImageButton(texture, modplayer);
			btn.UISetWidthHeight(52, 52);
			btn.UISetPosition(player.Center + offsetPos, originDefault);
			btn.perkType = newperk;
			Append(btn);
		}
	}
	private void ActivateStarterPerkUI(PerkPlayer modplayer, Player player) {
		Vector2 originDefault = new Vector2(26, 26);
		int[] starterPerk = new int[]
		{ Perk.GetPerkType<BlessingOfSolar>(),
			Perk.GetPerkType<BlessingOfVortex>(),
			Perk.GetPerkType<BlessingOfNebula>(),
			Perk.GetPerkType<BlessingOfStarDust>(),
			Perk.GetPerkType<BlessingOfPerk>()
		};
		for (int i = 0; i < starterPerk.Length; i++) {
			Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(starterPerk.Length, 360, i) * starterPerk.Length * 20;
			//After that we assign perk
			if (modplayer.perks.ContainsKey(starterPerk[i])) {
				if (modplayer.perks[starterPerk[i]] >= ModPerkLoader.GetPerk(starterPerk[i]).StackLimit) {
					continue;
				}
			}
			PerkUIImageButton btn = new PerkUIImageButton(ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(starterPerk[i]).textureString), modplayer);
			btn.UISetWidthHeight(52, 52);
			btn.UISetPosition(player.Center + offsetPos, originDefault);
			btn.perkType = starterPerk[i];
			Append(btn);
		}
	}
}
//Do all the check in UI state since that is where the perk actually get create and choose
class PerkUIImageButton : UIImageButton {
	PerkPlayer perkplayer;
	public int perkType;
	public PerkUIImageButton(Asset<Texture2D> texture, PerkPlayer perkPlayer) : base(texture) {
		perkplayer = perkPlayer;
	}
	public override void LeftClick(UIMouseEvent evt) {
		if (perkplayer.perks.Count < 0 || !perkplayer.perks.ContainsKey(perkType))
			perkplayer.perks.Add(perkType, 1);
		else
			if (perkplayer.perks.ContainsKey(perkType) && ModPerkLoader.GetPerk(perkType).CanBeStack)
			perkplayer.perks[perkType]++;
		ModPerkLoader.GetPerk(perkType).OnChoose(perkplayer.Player);
		UniversalSystem uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		uiSystemInstance.DeactivateState();
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		try {
			foreach (var el in Parent.Children) {
				if (el is UIText toolTip) {
					if (toolTip is null) {
						return;
					}
					if (IsMouseHovering) {
						FieldInfo textIsLarge = typeof(UIText).GetField("_isLarge", BindingFlags.NonPublic | BindingFlags.Instance);
						DynamicSpriteFont font = ((bool)textIsLarge.GetValue(el) ? FontAssets.DeathText : FontAssets.MouseText).Value;
						string tooltipText = ModPerkLoader.GetPerk(perkType).PerkNameToolTip;
						Vector2 size = ChatManager.GetStringSize(font, tooltipText, Vector2.One);
						size.X *= .5f;
						toolTip.UISetPosition(new Vector2(Left.Pixels, Top.Pixels) - size);
						toolTip.SetText(tooltipText);
					}
				}
			}
		}
		catch (Exception ex) {
			Main.NewText(ex.Message);
		}
	}
}
abstract class SpecialPerkUIImageButton : UIImageButton {
	protected SpecialPerkUIImageButton(Asset<Texture2D> texture) : base(texture) {
	}
	public new virtual void OnLeftClick(Player player) {
	}
	public override void LeftClick(UIMouseEvent evt) {
		base.LeftClick(evt);
		OnLeftClick(Main.LocalPlayer);
		UniversalSystem uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		uiSystemInstance.DeactivateState();
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		try {
			foreach (var el in Parent.Children) {
				if (el is UIText toolTip) {
					if (toolTip is null) {
						return;
					}
					if (IsMouseHovering) {
						FieldInfo textIsLarge = typeof(UIText).GetField("_isLarge", BindingFlags.NonPublic | BindingFlags.Instance);
						DynamicSpriteFont font = ((bool)textIsLarge.GetValue(el) ? FontAssets.DeathText : FontAssets.MouseText).Value;
						string tooltipText = TooltipText();
						Vector2 size = ChatManager.GetStringSize(font, tooltipText, Vector2.One);
						size.X *= .5f;
						toolTip.UISetPosition(new Vector2(Left.Pixels, Top.Pixels) - size);
						toolTip.SetText(tooltipText);
					}
				}
			}
		}
		catch (Exception ex) {
			Main.NewText(ex.Message);
		}
	}
	public virtual string TooltipText() => "";
}
class MaterialWeaponUIImageButton : SpecialPerkUIImageButton {
	public MaterialWeaponUIImageButton(Asset<Texture2D> texture) : base(texture) {
	}
	public override void OnLeftClick(Player player) {
		LootBoxBase.GetWeapon(out int weapon, out int amount);
		player.QuickSpawnItem(player.GetSource_FromThis(), weapon, amount);
	}
	public override string TooltipText() => "Give you 1 randomize weapon based on progression";
}
class MaterialCardUIImageButton : SpecialPerkUIImageButton {
	public MaterialCardUIImageButton(Asset<Texture2D> texture) : base(texture) {
	}
	public override void OnLeftClick(Player player) {
		player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<CardPacket>(), 3);
	}
	public override string TooltipText() => "Give you 3 card packets";
}
internal class EnchantmentUIState : UIState {
	public int WhoAmI = -1;
	WeaponEnchantmentUIslot weaponEnchantmentUIslot;
	ExitUI weaponEnchantmentUIExit;
	public override void OnActivate() {
		Elements.Clear();
		if (WhoAmI == -1)
			return;
		Player player = Main.player[WhoAmI];
		WeaponEnchantmentUIslot slot = new WeaponEnchantmentUIslot(TextureAssets.InventoryBack, player);
		slot.UISetWidthHeight(52, 52);
		slot.UISetPosition(player.Center + Vector2.UnitX * 120, new Vector2(26, 26));
		Append(slot);
		var exitUI = new ExitUI(TextureAssets.InventoryBack13);
		exitUI.UISetWidthHeight(52, 52);
		exitUI.UISetPosition(player.Center + Vector2.UnitX * 178, new Vector2(26, 26));
		Append(exitUI);
	}
}
public class WeaponEnchantmentUIslot : UIImage {
	public int WhoAmI = -1;
	public Texture2D textureDraw;
	public Item item;

	private Texture2D texture;
	private Player player;
	public WeaponEnchantmentUIslot(Asset<Texture2D> texture, Player player) : base(texture) {
		this.player = player;
		this.texture = texture.Value;
	}
	List<int> textUqID = new List<int>();
	public override void LeftClick(UIMouseEvent evt) {
		if (Main.mouseItem.type != ItemID.None) {
			if (Main.mouseItem.consumable)
				return;
			Item itemcached;
			if (item != null && item.type != ItemID.None) {
				itemcached = item.Clone();
				item = Main.mouseItem.Clone();
				Main.mouseItem = itemcached.Clone();
				player.inventory[58] = itemcached.Clone();
			}
			else {
				item = Main.mouseItem.Clone();
				Main.mouseItem.TurnToAir();
				player.inventory[58].TurnToAir();
				UniversalSystem.EnchantingState = true;
			}
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalItem)) {
				int length = globalItem.EnchantmenStlot.Length;
				for (int i = 0; i < length; i++) {
					Vector2 pos = player.Center + Vector2.UnitY * 60 + Vector2.UnitX * 60 * i;
					EnchantmentUIslot slot = new EnchantmentUIslot(TextureAssets.InventoryBack, player);
					slot.UISetWidthHeight(52, 52);
					slot.UISetPosition(pos, new Vector2(26, 26));
					slot.WhoAmI = i;
					slot.itemOwner = item;
					slot.itemType = globalItem.EnchantmenStlot[i];
					Parent.Append(slot);
					UIText text = new UIText($"{i + 1}");
					text.UISetPosition(pos + Vector2.UnitY * 56, new Vector2(26, 26));
					textUqID.Add(text.UniqueId);
					Parent.Append(text);
				}
			}
		}
		else {
			if (item == null)
				return;
			UniversalSystem.EnchantingState = false;
			Main.mouseItem = item;
			item = null;
			int count = Parent.Children.Count();
			for (int i = count - 1; i >= 0; i--) {
				UIElement child = Parent.Children.ElementAt(i);
				if (child is EnchantmentUIslot wmslot) {
					if (wmslot.itemOwner == null)
						continue;
				}
				if (child is EnchantmentUIslot { itemOwner: not null }) {
					child.Deactivate();
					child.Remove();
				}
				if (child is UIText text && textUqID.Contains(text.UniqueId)) {
					textUqID.Remove(text.UniqueId);
					child.Deactivate();
					child.Remove();
				}
			}
		}
	}
	public override void OnDeactivate() {
		UniversalSystem.EnchantingState = false;
		if (item == null)
			return;
		for (int i = 0; i < 50; i++) {
			if (player.CanItemSlotAccept(player.inventory[i], item)) {
				player.inventory[i] = item;
				return;
			}
		}
		player.DropItem(player.GetSource_DropAsItem(), player.Center, ref item);
	}
	public override void Draw(SpriteBatch spriteBatch) {
		Vector2 drawpos = new Vector2(Left.Pixels, Top.Pixels) + texture.Size() * .5f;
		base.Draw(spriteBatch);
		try {
			if (item != null) {
				Main.instance.LoadItem(item.type);
				Texture2D texture = TextureAssets.Item[item.type].Value;
				Vector2 origin = texture.Size() * .5f;
				float scaling = ScaleCalculation(texture.Size());
				spriteBatch.Draw(texture, drawpos, null, Color.White, 0, origin, scaling, SpriteEffects.None, 0);
			}
		}
		catch (Exception ex) {
			Main.NewText(ex.Message);
		}
	}
	private float ScaleCalculation(Vector2 textureSize) => texture.Size().Length() / (textureSize.Length() * 1.5f);
}
public class EnchantmentUIslot : UIImage {
	public int itemType = 0;
	public int WhoAmI = -1;

	public Item itemOwner = null;
	private Texture2D texture;
	private Player player;
	public EnchantmentUIslot(Asset<Texture2D> texture, Player player) : base(texture) {
		this.player = player;
		this.texture = texture.Value;
	}
	public override void LeftClick(UIMouseEvent evt) {
		if (itemOwner == null)
			return;
		if (Main.mouseItem.type != ItemID.None) {
			if (Main.mouseItem.consumable)
				return;
			if (itemType != 0)
				return;
			if (EnchantmentLoader.GetEnchantmentItemID(Main.mouseItem.type) == null)
				return;
			itemType = Main.mouseItem.type;
			Main.mouseItem.TurnToAir();
			player.inventory[58].TurnToAir();
			if (itemOwner.TryGetGlobalItem(out EnchantmentGlobalItem globalItem)) {
				globalItem.EnchantmenStlot[WhoAmI] = itemType;
			}
		}
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		try {
			if (itemOwner == null)
				return;
			if (itemType != 0) {
				Vector2 drawpos = new Vector2(Left.Pixels, Top.Pixels) + texture.Size() * .5f;
				Main.instance.LoadItem(itemType);
				Texture2D texture1 = TextureAssets.Item[itemType].Value;
				Vector2 origin = texture1.Size() * .5f;
				spriteBatch.Draw(texture1, drawpos, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
			}
		}
		catch (Exception ex) {
			Main.NewText(ex.Message);
		}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (itemType == ItemID.None)
			return;
		if (IsMouseHovering) {
			string tooltipText = "No enchantment can be found";
			if (EnchantmentLoader.GetEnchantmentItemID(itemType) != null) {
				tooltipText = EnchantmentLoader.GetEnchantmentItemID(itemType).Description;
			}
			Main.instance.MouseText(tooltipText);
		}
		else {
			if (!Parent.Children.Where(e => e.IsMouseHovering).Any()) {
				Main.instance.MouseText("");
			}
		}
	}
}
