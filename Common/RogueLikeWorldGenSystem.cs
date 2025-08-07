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
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria;
using ReLogic.Content;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Input;
using Microsoft.Build.Tasks;

namespace BossRush.Common;
public enum StructureUI_State : byte {
	Default,
	Selecting,
	Saving,
}
public class StructureUI : UIState {
	public Roguelike_UIPanel panel;
	public Roguelike_UITextPanel listtext_panel;
	public Roguelike_UIImageButton btn_confirm;
	public Roguelike_UIImageButton btn_cancel;
	public Roguelike_UITextPanel textPanel;
	public Roguelike_TextBox txt_FileName;

	public List<Roguelike_UIImageButton> list_btn = new();
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
			btn.VAlign = .5f;
			btn.HAlign = MathHelper.Lerp(0f, 1, i / 4f);
			btn.OnUpdate += List_Btn_OnUpdate;
			btn.OnLeftClick += List_Btn_OnLeftClick;
			panel.Append(btn);
			list_btn.Add(btn);
		}

		textPanel = new("Save this structure ? Please name the file");
		textPanel.UISetWidthHeight(400, 40);
		textPanel.HAlign = .5f;
		textPanel.VAlign = .1f;
		textPanel.Hide = true;
		panel.Append(textPanel);

		txt_FileName = new("");
		txt_FileName.HAlign = .5f;
		txt_FileName.VAlign = .45f;
		txt_FileName.Hide = true;
		panel.Append(txt_FileName);

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
		if (listeningElement.UniqueId != list_btn[0].UniqueId) {
			if (listeningElement.UniqueId == list_btn[1].UniqueId) {
				CurrentUI_State = StructureUI_State.Selecting;
				method = SaverOptimizedMethod.Default;
			}
			else if (listeningElement.UniqueId == list_btn[2].UniqueId) {
				CurrentUI_State = StructureUI_State.Selecting;
				method = SaverOptimizedMethod.Template;
			}
			else if (listeningElement.UniqueId == list_btn[3].UniqueId) {
				CurrentUI_State = StructureUI_State.Selecting;
				method = SaverOptimizedMethod.MultiStructure;
			}
			VisibilitySettingUI(true);
			VisibilityUI(true);
			panel.Hide = true;
			Main.NewText("Press key P to confirm selection");
		}
		else {
			ModContent.GetInstance<UniversalSystem>().DeactivateUI();
			CurrentUI_State = StructureUI_State.Default;
			method = SaverOptimizedMethod.Default;
		}
	}
	private void VisibilityUI(bool hide) {
		textPanel.Hide = hide;
		txt_FileName.Hide = hide;
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
		}
		else if (affectedElement.UniqueId == list_btn[1].UniqueId) {
			Main.instance.MouseText("Default saving ( uses this )");
		}
		else if (affectedElement.UniqueId == list_btn[2].UniqueId) {
			Main.instance.MouseText("Template saving ( recommended for basic template )");
		}
		else if (affectedElement.UniqueId == list_btn[3].UniqueId) {
			Main.instance.MouseText("Detailed saving");
		}
		else if (affectedElement.UniqueId == list_btn[4].UniqueId) {
			Main.instance.MouseText("(Not Supported");
		}
	}

	private void Panel_OnUpdate(UIElement affectedElement) {
		if (panel.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
	}

	private void Btn_cancel_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		ModContent.GetInstance<UniversalSystem>().DeactivateUI();
		txt_FileName.SetText("");
		VisibilityUI(true);
		VisibilitySettingUI(false);
	}

	private void Btn_confirm_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (method == SaverOptimizedMethod.Default) {
			GenerationHelper.SaveToFile(new Rectangle(TopLeft.X, TopLeft.Y, thisWidth - 1, thisHeight - 1), txt_FileName.Text);
			txt_FileName.SetText("");
			ModContent.GetInstance<UniversalSystem>().DeactivateUI();
			Main.NewText("Successfully save structure");
		}
		else if (method == SaverOptimizedMethod.Template) {
			GenerationHelper.SaveToFile(new Rectangle(TopLeft.X, TopLeft.Y, thisWidth - 1, thisHeight - 1), txt_FileName.Text, SaverOptimizedMethod.Template);
			txt_FileName.SetText("");
			ModContent.GetInstance<UniversalSystem>().DeactivateUI();
			Main.NewText("Successfully save structure");
		}
		else if (method == SaverOptimizedMethod.MultiStructure) {
			GenerationHelper.SaveToFile(new Rectangle(TopLeft.X, TopLeft.Y, thisWidth - 1, thisHeight - 1), txt_FileName.Text, SaverOptimizedMethod.MultiStructure);
			txt_FileName.SetText("");
			ModContent.GetInstance<UniversalSystem>().DeactivateUI();
			Main.NewText("Successfully save structure");
		}
		VisibilityUI(true);
		VisibilitySettingUI(false);
	}
	public bool secondPoint;

	public Point16 point1;
	public Point16 point2;

	public bool movePoint1;
	public bool movePoint2;
	public bool Ready => !secondPoint && point1 != default;
	public Point16 TopLeft => new Point16(point1.X < point2.X ? point1.X : point2.X, point1.Y < point2.Y ? point1.Y : point2.Y);
	public Point16 BottomRight => new Point16(point1.X > point2.X ? point1.X : point2.X, point1.Y > point2.Y ? point1.Y : point2.Y);
	public int thisWidth => BottomRight.X - TopLeft.X;
	public int thisHeight => BottomRight.Y - TopLeft.Y;
	int delay = 0;
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (CurrentUI_State == StructureUI_State.Saving) {
			if (btn_cancel.IsMouseHovering) {
				Main.instance.MouseText("Cancel ?");
			}
			if (btn_confirm.IsMouseHovering) {
				Main.instance.MouseText("Confirm");
			}
		}
		if (CurrentUI_State != StructureUI_State.Selecting) {
			return;
		}
		delay = BossRushUtils.CountDown(delay);
		if (Ready && Main.mouseRight && delay <= 0) {
			CurrentUI_State = StructureUI_State.Saving;
			VisibilityUI(false);
			panel.Hide = false;
			return;
		}
		if (PlayerInput.GetPressedKeys().Contains(Keys.P) && delay <= 0) {
			delay = 60;
			if (Ready) {
				if (Vector2.Distance(Main.MouseWorld, point1.ToVector2() * 16) <= 32) {
					movePoint1 = true;
				}

				if (Vector2.Distance(Main.MouseWorld, point2.ToVector2() * 16) <= 32) {
					movePoint2 = true;
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
		}
		if (movePoint1)
			point1 = (Main.MouseWorld / 16).ToPoint16();

		if (movePoint2)
			point2 = (Main.MouseWorld / 16).ToPoint16();

		if (!Main.mouseLeft) {
			movePoint1 = false;
			movePoint2 = false;
		}
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		if (CurrentUI_State == StructureUI_State.Selecting) {
			Texture2D tex = ModContent.Request<Texture2D>("BossRush/Texture/StructureHelper_corner").Value;
			Texture2D tex2 = ModContent.Request<Texture2D>("BossRush/Texture/StructureHelper_Box").Value;
			Point16 topLeft = TopLeft;
			Point16 bottomRight = BottomRight;

			bool drawPreview = true;

			if (secondPoint) {
				var point1 = this.point1;
				var point2 = (Main.MouseWorld / 16).ToPoint16();

				topLeft = new Point16(point1.X < point2.X ? point1.X : point2.X, point1.Y < point2.Y ? point1.Y : point2.Y);
				bottomRight = new Point16(point1.X > point2.X ? point1.X : point2.X, point1.Y > point2.Y ? point1.Y : point2.Y);
				int Width = bottomRight.X - topLeft.X - 1;
				int Height = bottomRight.Y - topLeft.Y - 1;

				var target = new Rectangle((int)(topLeft.X * 16 - Main.screenPosition.X), (int)(topLeft.Y * 16 - Main.screenPosition.Y), Width * 16 + 16, Height * 16 + 16);
				BossRushUtils.DrawOutline(spriteBatch, target, Color.Gold);
				spriteBatch.Draw(tex2, target, tex2.Frame(), Color.White * 0.15f);

				spriteBatch.Draw(tex, this.point1.ToVector2() * 16 - Main.screenPosition, tex.Frame(), Color.Cyan, 0, tex.Frame().Size() / 2, 1, 0, 0);
				//spriteBatch.Draw(tex, point2.ToVector2() * 16 - Main.screenPosition, tex.Frame(), Color.White * 0.5f, 0, tex.Frame().Size() / 2, 1, 0, 0);
			}
			else if (Ready) {
				int Width = bottomRight.X - topLeft.X - 1;
				int Height = bottomRight.Y - topLeft.Y - 1;

				var target = new Rectangle((int)(topLeft.X * 16 - Main.screenPosition.X), (int)(topLeft.Y * 16 - Main.screenPosition.Y), Width * 16 + 16, Height * 16 + 16);
				BossRushUtils.DrawOutline(spriteBatch, target, Color.Lerp(Color.Gold, Color.White, 0.5f + 0.5f * (float)System.Math.Sin(Main.GameUpdateCount * 0.2f)));
				spriteBatch.Draw(tex2, target, tex2.Frame(), Color.White * 0.15f);

				float scale1 = Vector2.Distance(Main.MouseWorld, this.point1.ToVector2() * 16) < 32 ? 1.5f : 1f;
				spriteBatch.Draw(tex, this.point1.ToVector2() * 16 - Main.screenPosition, tex.Frame(), Color.Cyan * scale1, 0, tex.Frame().Size() / 2, scale1, 0, 0);

				float scale2 = Vector2.Distance(Main.MouseWorld, this.point2.ToVector2() * 16) < 32 ? 1.5f : 1f;
				spriteBatch.Draw(tex, this.point2.ToVector2() * 16 - Main.screenPosition, tex.Frame(), Color.Red * scale2, 0, tex.Frame().Size() / 2, scale2, 0, 0);

				if (scale1 > 1 || scale2 > 1)
					drawPreview = false;
			}

			if (drawPreview) {
				var pos = (Main.MouseWorld / 16).ToPoint16();
				spriteBatch.Draw(tex, pos.ToVector2() * 16 - Main.screenPosition, tex.Frame(), Color.White * 0.5f, 0, tex.Frame().Size() / 2, 1, 0, 0);
			}
		}
	}
}

public class RogueLikeWorldGenSystem : ModSystem {

	public List<GenPassData> list_genPass = new();
	public List<Structure_XinimVer> list_Structure = new();
	/// <summary>
	/// This dictionary only load file in folder "Assets/Structures/..." so make sure you got the naming right<br/>
	/// You can ignore inputing "Assets/Structures/..." 
	/// </summary>
	public Dictionary<string, List<GenPassData>> dict_Struture = new();
	public const string FileDestination = "Assets/Structures/";
	public override void Unload() {
		list_genPass = null;
		dict_Struture = null;
		list_Structure = null;
	}
	public override void PostSetupContent() {
		Stopwatch watch = new();
		try {
			watch.Start();
			string fileName = "";
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
				StreamReader r = new StreamReader(filepath);
				currentchar = r.Peek();
				if (currentchar == '1') {
					r.Read();
					FileFormatVer1(ref currentchar, ref r, ref strbld, ref amount, ref tile);
					if (strbld.Length > 0) {
						if (ushort.TryParse(strbld.ToString(), out ushort result)) {
							list_genPass.Add(new(tile, result));
						}
					}
					list_Structure.Add(new(fileName, new(list_genPass)));
					dict_Struture.Add(fileName, new(list_genPass));
				}
				else if (currentchar == '2') {
					r.Read();
					FileFormatVer2(ref currentchar, ref r, ref strbld, ref tile);
					list_Structure.Add(new(fileName, new(list_genPass)));
					dict_Struture.Add(fileName, new(list_genPass));
				}
				else {
					FileFormatVer1(ref currentchar, ref r, ref strbld, ref amount, ref tile);
					if (strbld.Length > 0) {
						if (ushort.TryParse(strbld.ToString(), out ushort result)) {
							list_genPass.Add(new(tile, result));
						}
					}
					list_Structure.Add(new(fileName, new(list_genPass)));
					dict_Struture.Add(fileName, new(list_genPass));
				}
				list_genPass.Clear();
				r.Close();
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
	Dictionary<char, TileData> data = new();
	public void FileFormatVer2(ref int currentchar, ref StreamReader r, ref StringBuilder strbld, ref TileData tile) {
		bool SwitchToTileMap = false;
		bool DataTileSaving = true;
		char cached = ' ';
		//Since the format is like a{}b{}c{}d{}
		//We can safely assumed there are no missing symbol
		while (currentchar != -1) {
			currentchar = r.Read();
			char c = (char)currentchar;
			//This mean the upcoming next tile data is definitely gonna be number or another new tile data
			if (!SwitchToTileMap) {
				//Checking whenever or not we are reaching the new tile data
				if (c == '}') {
					//Skip over the current value because it is not needed
					currentchar = r.Read();
					//if skipping over somehow ended up in -1, break the operation, since this is a indicator that the reader has reach the end of the file
					if (currentchar == -1) {
						break;
					}
					//Updating char reader
					c = (char)currentchar;
					//Creating tiledata
					tile = new(strbld.ToString());
					//Resetting

					if (!data.ContainsKey(cached)) {
						//Add data
						data.Add(cached, tile);
					}
					//Resetting
					tile = TileData.Default;

					strbld.Clear();
					DataTileSaving = true;
				}
				//This mean we are entering a new tile data
				else if (c == '{') {
					//Check in case the previous check if tile data is present or not
					strbld.Clear();
					continue;
				}
				if (c == '>') {
					SwitchToTileMap = true;
					DataTileSaving = true;
					strbld.Clear();
					continue;
				}
			}
			else {
				//Check current iteration is a character or not
				//If true then check the previous iteration contain number data or not
				if (!char.IsNumber(c)) {
					if (ushort.TryParse(strbld.ToString(), out ushort result)) {
						list_genPass.Add(new(data[cached], result));
						DataTileSaving = true;
						strbld.Clear();
					}
				}
			}
			//Checking if the currentchar value is not equal to -1, -1 dictate that the reader has reach the end of the file
			//This also act as our actual reader, while currentchar is more like iterator
			if (currentchar != -1) {
				//Is in file saving part
				//This should be handle delicately
				//To ensure correct data enter
				if (DataTileSaving) {
					cached = c;
					DataTileSaving = false;
				}
				else {
					strbld.Append(c);
				}
			}
		}
		if (strbld.Length > 0) {
			if (ushort.TryParse(strbld.ToString(), out ushort result)) {
				list_genPass.Add(new(data[cached], result));
			}
		}
		data.Clear();
	}
	public void FileFormatVer1(ref int currentchar, ref StreamReader r, ref StringBuilder strbld, ref ushort amount, ref TileData tile) {
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
			}
			if (currentchar != -1)
				strbld.Append(c);
		}
	}
}
public class Structure_XinimVer {
	private string filePath = "";
	private GenPassData[] data = null;
	int cachedIndexOld = -1, cachedIndex = -1, cachedcounter = -1;
	public string Get_FilePath => filePath;
	public Structure_XinimVer(string path, List<GenPassData> gendata) {
		filePath = path;
		data = gendata.ToArray();
	}
	/// <summary>
	/// It is highly recommended to cached this result
	/// </summary>
	/// <returns></returns>
	public int Get_TotalLength() {
		int currentIndex = 0;
		for (int i = 0; i < data.Length; i++) {
			currentIndex += data[i].Count;
		}
		return currentIndex;
	}
	public GenPassData[] Get_Data => (GenPassData[])data.Clone();

	/// <summary>
	/// The current index of 1d array, the function will simulated a tile map
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	public TileData Get_CurrentTileData(int index) {
		if (data == null) {
			Console.WriteLine("Tile map haven't been initialized, returning new tiledata");
			return new();
		}
		if (index < 0) {
			Console.WriteLine("Out of bound indexes");
			return new();
		}
		if (index == cachedIndex) {
			return data[cachedcounter].tileData;
		}
		if (cachedIndexOld != -1) {
			if (index >= cachedIndexOld && index <= cachedIndexOld + data[cachedcounter].Count) {
				return data[cachedcounter].tileData;
			}
			int AttemptToJump = cachedcounter + 1;
			if (AttemptToJump < data.Length) {
				int JumpCounter = cachedIndexOld + data[cachedcounter].Count;
				if (index >= JumpCounter && index <= JumpCounter + data[AttemptToJump].Count) {
					return data[AttemptToJump].tileData;
				}
			}
			int AttemptToFall = cachedcounter - 1;
			if (AttemptToFall > -1) {
				int JumpCounter = cachedIndexOld - data[AttemptToFall].Count;
				if (index >= JumpCounter && index <= cachedIndexOld) {
					return data[AttemptToFall].tileData;
				}
			}
		}
		int currentIndex = 0;
		for (int i = 0; i < data.Length; i++) {
			currentIndex += data[i].Count;
			if (currentIndex >= index) {
				cachedIndex = index;
				cachedcounter = i;
				return data[i].tileData;
			}
			cachedIndexOld = currentIndex;
		}
		cachedIndex = cachedcounter = cachedIndexOld = -1;
		Console.WriteLine("Out of bound indexes");
		return new();
	}
}
/// <summary>
/// A light weight container for structure
/// </summary>
public struct GenPassData {
	public TileData tileData { get; private set; }
	public ushort Count { get; private set; }
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
	public void Clear() {
		this.Count = 0;
		this.tileData = new();
	}
}
