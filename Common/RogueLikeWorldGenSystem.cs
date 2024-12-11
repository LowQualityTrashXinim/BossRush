using System.IO;
using System.Text;
using Terraria.ModLoader;
using BossRush.Common.Utils;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using BossRush.Common.Systems;
using BossRush.Texture;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria;
using BossRush.Contents.Items;
using ReLogic.Content;

namespace BossRush.Common;
public enum StructureUI_State : byte {
	Default,
	HorizontalSelection,
	VerticalSelection,
	MultiStructureSelection,
	Saving,
}
public class StructureUI : UIState {
	public UIPanel panel;
	public Roguelike_UITextPanel listtext_panel;
	public Roguelike_UITextPanel textBox;
	public Roguelike_UIImageButton btn_confirm;
	public Roguelike_UIImageButton btn_cancel;
	public Roguelike_UITextPanel textPanel;
	public Point16 TopLeft = new Point16();
	public Point16 BottomRight = new Point16();
	public bool IsFocus = false;

	public List<Roguelike_UIImageButton> list_btn = new();
	public int WidthStruct => BottomRight.X - TopLeft.X;
	public int HeightStruct => BottomRight.Y - TopLeft.Y;
	Asset<Texture2D> defaultlookingassbtn = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
	public override void OnDeactivate() {
		VisibilityUI(true);
	}
	public override void OnInitialize() {
		panel = new();
		panel.HAlign = .5f;
		panel.VAlign = .5f;
		panel.UISetWidthHeight(450, 200);
		panel.OnUpdate += Panel_OnUpdate;
		Append(panel);

		for (int i = 0; i < 5; i++) {
			Roguelike_UIImageButton btn = new(defaultlookingassbtn);
			btn.UISetWidthHeight(52, 52);
			btn.HAlign = .5f;
			btn.VAlign = MathHelper.Lerp(0f, 1, i / 4);
			btn.OnUpdate += List_Btn_OnUpdate;
			btn.OnLeftClick += List_Btn_OnLeftClick;
			panel.Append(btn);
		}

		textPanel = new("Save this structure ? Please name the file");
		textPanel.UISetWidthHeight(400, 40);
		textPanel.HAlign = .5f;
		textPanel.VAlign = .1f;
		textPanel.Hide = true;
		panel.Append(textPanel);

		textBox = new("");
		textBox.HAlign = .5f;
		textBox.VAlign = .45f;
		textBox.UISetWidthHeight(400, 40);
		textBox.TextHAlign = 0f;
		textBox.Hide = true;
		textBox.OnLeftClick += TextBox_OnLeftClick;
		panel.Append(textBox);

		btn_cancel = new(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT));
		btn_cancel.HAlign = 0f;
		btn_cancel.VAlign = 1f;
		btn_cancel.OnLeftClick += Btn_cancel_OnLeftClick;
		btn_cancel.UISetWidthHeight(52, 52);
		btn_cancel.Hide = true;
		panel.Append(btn_cancel);

		btn_confirm = new(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT));
		btn_confirm.HAlign = 1f;
		btn_confirm.VAlign = 1f;
		btn_confirm.UISetWidthHeight(52, 52);
		btn_confirm.OnLeftClick += Btn_confirm_OnLeftClick;
		btn_confirm.Hide = true;
		panel.Append(btn_confirm);
	}
	StructureUI_State CurrentUI_State = 0;
	SaverOptimizedMethod method = SaverOptimizedMethod.Default;
	private void List_Btn_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (listeningElement.UniqueId == list_btn[0].UniqueId) {
			ModContent.GetInstance<UniversalSystem>().DeactivateUI();
		}
	}
	private void VisibilityUI(bool hide) {
		textPanel.Hide = hide;
		textBox.Hide = hide;
		btn_cancel.Hide = hide;
		btn_confirm.Hide = hide;
	}
	private void VisibilitySettingUI(bool hide) {
		list_btn.ForEach(b => b.Hide = hide);
	}
	private void List_Btn_OnUpdate(UIElement affectedElement) {
		if (!affectedElement.IsMouseHovering) {
			return;
		}

		if (affectedElement.UniqueId == list_btn[0].UniqueId) {
			Main.instance.MouseText("Close ?");
			CurrentUI_State = StructureUI_State.Default;
			method = SaverOptimizedMethod.Default;
		}
		if (affectedElement.UniqueId == list_btn[1].UniqueId) {
			Main.instance.MouseText("Horizontal saving");
			CurrentUI_State = StructureUI_State.HorizontalSelection;
			method = SaverOptimizedMethod.HorizontalDefault;
		}
		if (affectedElement.UniqueId == list_btn[2].UniqueId) {
			Main.instance.MouseText("Vertical saving");
			CurrentUI_State = StructureUI_State.VerticalSelection;
			method = SaverOptimizedMethod.Default;
		}
		if (affectedElement.UniqueId == list_btn[3].UniqueId) {
			Main.instance.MouseText("Multi-structure saving");
			CurrentUI_State = StructureUI_State.MultiStructureSelection;
			method = SaverOptimizedMethod.MultiStructure;
		}
		if (affectedElement.UniqueId == list_btn[4].UniqueId) {
			Main.instance.MouseText("");
		}
		VisibilitySettingUI(true);
	}

	private void Panel_OnUpdate(UIElement affectedElement) {
		if (panel.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
	}

	private void TextBox_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		IsFocus = true;
	}

	private void Btn_cancel_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		ModContent.GetInstance<UniversalSystem>().DeactivateUI();
		textBox.SetText("");
	}

	private void Btn_confirm_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		GenerationHelper.SaveToFile(new(TopLeft.X, TopLeft.Y, WidthStruct - 1, HeightStruct - 1), textBox.Text);
		textBox.SetText("");
		ModContent.GetInstance<UniversalSystem>().DeactivateUI();
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (Main.keyState.IsKeyDown(Keys.Escape) || Main.mouseLeft && !IsMouseHovering)
			IsFocus = false;
		if (IsFocus) {

		}
	}
}

public class RogueLikeWorldGenSystem : ModSystem {

	public override void Load() {
		On_Main.DrawInterface += On_Main_DrawInterface;
	}
	private void On_Main_DrawInterface(On_Main.orig_DrawInterface orig, Main self, GameTime gameTime) {
		//Code source credit : Structure Helper
		//SpriteBatch spriteBatch = Main.spriteBatch;

		//if (Main.LocalPlayer.HeldItem.ModItem is StructureWand) {
		//	var wand = (Main.LocalPlayer.HeldItem.ModItem as StructureWand);
		//	//spriteBatch.Begin(default, default, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

		//	Texture2D tex = ModContent.Request<Texture2D>("StructureHelper/corner").Value;
		//	Texture2D tex2 = ModContent.Request<Texture2D>("StructureHelper/box").Value;

		//	Point16 topLeft = wand.TopLeft;
		//	Point16 bottomRight = wand.BottomRight;

		//	bool drawPreview = true;

		//	if (wand.secondPoint) {
		//		var point1 = wand.point1;
		//		var point2 = (Main.MouseWorld / 16).ToPoint16();

		//		topLeft = new Point16(point1.X < point2.X ? point1.X : point2.X, point1.Y < point2.Y ? point1.Y : point2.Y);
		//		bottomRight = new Point16(point1.X > point2.X ? point1.X : point2.X, point1.Y > point2.Y ? point1.Y : point2.Y);
		//		int Width = bottomRight.X - topLeft.X - 1;
		//		int Height = bottomRight.Y - topLeft.Y - 1;

		//		var target = new Rectangle((int)(topLeft.X * 16 - Main.screenPosition.X), (int)(topLeft.Y * 16 - Main.screenPosition.Y), Width * 16 + 16, Height * 16 + 16);
		//		BossRushUtils.DrawOutline(spriteBatch, target, Color.Gold);
		//		spriteBatch.Draw(tex2, target, tex2.Frame(), Color.White * 0.15f);

		//		spriteBatch.Draw(tex, wand.point1.ToVector2() * 16 - Main.screenPosition, tex.Frame(), Color.Cyan, 0, tex.Frame().Size() / 2, 1, 0, 0);
		//		//spriteBatch.Draw(tex, point2.ToVector2() * 16 - Main.screenPosition, tex.Frame(), Color.White * 0.5f, 0, tex.Frame().Size() / 2, 1, 0, 0);
		//	}
		//	else if (wand.Ready) {
		//		int Width = bottomRight.X - topLeft.X - 1;
		//		int Height = bottomRight.Y - topLeft.Y - 1;

		//		var target = new Rectangle((int)(topLeft.X * 16 - Main.screenPosition.X), (int)(topLeft.Y * 16 - Main.screenPosition.Y), Width * 16 + 16, Height * 16 + 16);
		//		BossRushUtils.DrawOutline(spriteBatch, target, Color.Lerp(Color.Gold, Color.White, 0.5f + 0.5f * (float)System.Math.Sin(Main.GameUpdateCount * 0.2f)));
		//		spriteBatch.Draw(tex2, target, tex2.Frame(), Color.White * 0.15f);

		//		float scale1 = Vector2.Distance(Main.MouseWorld, wand.point1.ToVector2() * 16) < 32 ? 1.5f : 1f;
		//		spriteBatch.Draw(tex, wand.point1.ToVector2() * 16 - Main.screenPosition, tex.Frame(), Color.Cyan * scale1, 0, tex.Frame().Size() / 2, scale1, 0, 0);

		//		float scale2 = Vector2.Distance(Main.MouseWorld, wand.point2.ToVector2() * 16) < 32 ? 1.5f : 1f;
		//		spriteBatch.Draw(tex, wand.point2.ToVector2() * 16 - Main.screenPosition, tex.Frame(), Color.Red * scale2, 0, tex.Frame().Size() / 2, scale2, 0, 0);

		//		if (scale1 > 1 || scale2 > 1)
		//			drawPreview = false;
		//	}

		//	if (drawPreview) {
		//		var pos = (Main.MouseWorld / 16).ToPoint16();
		//		spriteBatch.Draw(tex, pos.ToVector2() * 16 - Main.screenPosition, tex.Frame(), Color.White * 0.5f, 0, tex.Frame().Size() / 2, 1, 0, 0);
		//	}

		//	spriteBatch.End();
		//}

		orig(self, gameTime);
	}

	public List<GenPassData> list_genPass = new();
	public Dictionary<string, List<GenPassData>> dict_Struture = new();
	public const string FileDestination = "Assets/Structures/";
	public override void PostSetupContent() {
		Stopwatch watch = new();
		try {
			watch.Start();
			string fileName = "";
			List<GenPassData> list_genPass = new();
			foreach (string filenamepath in Mod.GetFileNames()) {
				if (!filenamepath.StartsWith(FileDestination)) {
					continue;
				}
				StringBuilder strbld = new StringBuilder();
				strbld.AppendLine(filenamepath);
				strbld.Remove(0, FileDestination.Length);
				fileName = strbld.ToString();
				strbld.Remove(fileName.Length - 6, 6);
				fileName = strbld.ToString();
				strbld.Clear();
				Stream filepath = Mod.GetFileStream(filenamepath);
				int currentchar = 0;
				ushort amount = 1;
				TileData tile = TileData.Default;
				using StreamReader r = new StreamReader(filepath);
				while (currentchar != -1) {
					currentchar = r.Read();
					char c = (char)currentchar;
					//This mean the upcoming next tile data is definitely gonna be number or another new tile data
					if (c == '}') {
						currentchar = r.Read();
						if (currentchar == -1) {
							break;
						}
						//We are reading new tile data, as such this mean that previous tile data only have 1
						//So we are creating a new genpass with count of amount
						c = (char)currentchar;
						if (c == '{') {
							if (strbld.Length > 0) {
								list_genPass.Add(new(new(strbld.ToString()), amount));
							}
							strbld.Clear();
							amount = 1;
							continue;
						}
						//This mean there are multiple of said tile above
						//Which mean we should just create a new tile datat and then set count to it after we retrieve all the needed amount
						else {
							tile = new(strbld.ToString());
							strbld.Clear();
							strbld.Append(c);
							continue;
						}
					}
					//This mean we are entering a new tile data
					if (c == '{') {
						//Check in case the previous check if tile data is present or not
						if (!tile.Equals(TileData.Default)) {
							amount = ushort.Parse(strbld.ToString());
							list_genPass.Add(new(tile, amount));
							tile = TileData.Default;
						}
						amount = 1;
						strbld.Clear();
						continue;
					}
					if (currentchar != -1)
						strbld.Append(c);
				}
				if (strbld.Length > 0) {
					if (ushort.TryParse(strbld.ToString(), out ushort result)) {
						list_genPass.Add(new(tile, result));
					}
				}
				dict_Struture.Add(fileName, new(list_genPass));
				list_genPass.Clear();
			}
		}
		catch {

		}
		finally {
			watch.Stop();
			string result = $"{Mod.Name} : Time it take to load structure dictionary : {watch.ToString()}";
			Mod.Logger.Info(result);
			Console.WriteLine(result);
		}
	}
}
public class GenPassData {
	public TileData tileData { get; private set; }
	public ushort Count { get; private set; }
	public ushort CountX { get; private set; }
	public ushort CountY { get; private set; }
	public GenPassData() {

	}
	public GenPassData(TileData tileData, ushort count) {
		this.tileData = tileData;
		Count = count;
	}
	public void Default_Set(TileData data, ushort count) {
		this.Count = count;
		this.tileData = data;
	}
	public void Rect_Set(TileData data, ushort countX, ushort countY) {
		this.CountX = countX;
		this.CountY = countY;
		this.tileData = data;
	}
	public void Clear() {
		this.Count = 0;
		this.CountX = 0;
		this.CountY = 0;
		this.tileData = new();
	}
}
