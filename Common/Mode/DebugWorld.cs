using BossRush.Common.General;
using BossRush.Common.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace BossRush.Common.Mode;
public partial class DebugWorld : ModSystem {
	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
		if (ModContent.GetInstance<RogueLikeConfig>().TemplateTest) {
			tasks.ForEach(g => g.Disable());
			tasks.AddRange(((ITaskCollection)this).Tasks);
		}
	}
}
public partial class DebugWorld : ITaskCollection {
	[Task]
	public void SettingUpPlayerSpawn() {
		Main.spawnTileX = 100;
		Main.spawnTileY = 100;
	}
	[Task]
	public void GenerateHorizonTemplate() {
		Rectangle re = GenerationHelper.GridPositionInTheWorld24x24(new(1, 1, 64, 32));
		int X = 0;
		for (int i = 1; i < 10; i++) {
			X = re.Width * i + 10 * i;
			GenerationHelper.PlaceStructure("Template/WG_Template" + "Horizontal" + i, new(re.X + X, re.Y, re.Width, re.Height));
		}
	}

	[Task]
	public void GenerateVerticalTemplate() {
		Rectangle re = GenerationHelper.GridPositionInTheWorld24x24(new(1, 2, 32, 64));
		int X = 0;
		for (int i = 1; i < 10; i++) {
			X = re.Width * i + 10 * i;
			GenerationHelper.PlaceStructure("Template/WG_Template" + "Vertical" + i, new(re.X + X, re.Y, re.Width, re.Height));
		}
	}

	[Task]
	public void GenerateDungeonTemplate_Horizontal() {
		Rectangle re = GenerationHelper.GridPositionInTheWorld24x24(new(1, 3, 64, 32));
		int X = 0;
		for (int i = 1; i < 10; i++) {
			X = re.Width * i + 10 * i;
			GenerationHelper.PlaceStructure("Template/WG_Dungeon_Template" + "Horizontal" + i, new(re.X + X, re.Y, re.Width, re.Height));
		}
	}
	[Task]
	public void GenerateDungeonTemplate_Vertical() {
		Rectangle re = GenerationHelper.GridPositionInTheWorld24x24(new(1, 4, 32, 64));
		int X = 0;
		for (int i = 1; i < 10; i++) {
			X = re.Width * i + 10 * i;
			GenerationHelper.PlaceStructure("Template/WG_Dungeon_Template" + "Vertical" + i, new(re.X + X, re.Y, re.Width, re.Height));
		}
	}
	[Task]
	public void GenerateEmptyTemplate() {
		Rectangle re = GenerationHelper.GridPositionInTheWorld24x24(new(1, 5, 64, 64));
		int X = re.Width + 10;
		ImageData arena = ImageStructureLoader.Get_Tempate("WG_TemplateHorizontal1");
		arena.EnumeratePixels((a, b, color) => {
			a += re.X;
			b += re.Y;
			GenerationHelper.FastRemoveTile(a, b);
			if (color.R == 255 && color != Color.White) {
				GenerationHelper.FastPlaceTile(a, b, TileID.Dirt);
			}
			GenerationHelper.FastPlaceWall(a, b, WallID.Stone);
		});
		ImageData arena2 = ImageStructureLoader.Get_Tempate("WG_TemplateVertical1");
		arena2.EnumeratePixels((a, b, color) => {
			a += re.X + X;
			b += re.Y;
			GenerationHelper.FastRemoveTile(a, b);
			if (color.R == 255 && color != Color.White) {
				GenerationHelper.FastPlaceTile(a, b, TileID.Dirt);
			}
			GenerationHelper.FastPlaceWall(a, b, WallID.Stone);
		});
	}
	[Task]
	public void GenerateSpaceTemplate_Horizontal() {
		Rectangle re = GenerationHelper.GridPositionInTheWorld24x24(new(1, 6, 64, 32));
		int X = 0;
		for (int i = 1; i < 10; i++) {
			X = re.Width * i + 10 * i;
			GenerationHelper.PlaceStructure("Template/WG_Space_Template" + "Horizontal" + i, new(re.X + X, re.Y, re.Width, re.Height));
		}
	}
	[Task]
	public void GenerateSpaceTemplate_Vertical() {
		Rectangle re = GenerationHelper.GridPositionInTheWorld24x24(new(1, 7, 32, 64));
		int X = 0;
		for (int i = 1; i < 10; i++) {
			X = re.Width * i + 10 * i;
			GenerationHelper.PlaceStructure("Template/WG_Space_Template" + "Vertical" + i, new(re.X + X, re.Y, re.Width, re.Height));
		}
	}
	[Task]
	public void GenerateTestStructure() {
		Rectangle re = GenerationHelper.GridPositionInTheWorld24x24(new(1, 8, 18, 8));
		int X = 0;
		for (int i = 1; i < 10; i++) {
			X = re.Width * i + 10 * i;
			GenerationHelper.PlaceStructure("Detailed_TestSave", new(re.X + X, re.Y, re.Width, re.Height));
		}
	}
}
