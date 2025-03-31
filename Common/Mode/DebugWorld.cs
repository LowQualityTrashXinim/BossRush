using BossRush.Common.General;
using BossRush.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework;
using System.Diagnostics.Metrics;
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
		for (int i = 0; i < 9; i++) {
			X = re.Width * i + 10 * i;
			GenerationHelper.PlaceStructure("Template/WG_Template" + "Horizontal" + i, new(re.X + X, re.Y, re.Width, re.Height));
		}
	}

	[Task]
	public void GenerateVerticalTemplate() {
		Rectangle re = GenerationHelper.GridPositionInTheWorld24x24(new(1, 2, 32, 64));
		int X = 0;
		for (int i = 0; i < 9; i++) {
			X = re.Width * i + 10 * i;
			GenerationHelper.PlaceStructure("Template/WG_Template" + "Vertical" + i, new(re.X + X, re.Y, re.Width, re.Height));
		}
	}

	[Task]
	public void GenerateDungeonTemplate_Horizontal() {
		ImageData template;
		Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(new(1, 3, 64, 32));
		int X = 0;
		Point counter = Point.Zero;
		for (int i = 0; i < 9; i++) {
			X = rect.Width * i + 10 * i;
			template = ImageStructureLoader.Get_Tempate("WG_Dungeon_TemplateHorizontal" + (i + 1));
			template.EnumeratePixels((a, b, color) => {
				a += rect.X + counter.X + X;
				b += rect.Y + counter.Y;
				GenerationHelper.FastRemoveTile(a, b);
				if (color.R == 255 && color.B == 0 && color.G == 0) {
					GenerationHelper.FastPlaceTile(a, b, TileID.BlueDungeonBrick);
				}
				GenerationHelper.FastPlaceWall(a, b, WallID.BlueDungeon);
			});
		}
	}
	[Task]
	public void GenerateDungeonTemplate_Vertical() {
		ImageData template;
		Rectangle rect = GenerationHelper.GridPositionInTheWorld24x24(new(1, 4, 64, 32));
		int X = 0;
		Point counter = Point.Zero;
		for (int i = 0; i < 9; i++) {
			X = rect.Width * i + 10 * i;
			template = ImageStructureLoader.Get_Tempate("WG_Dungeon_TemplateVertical" + (i + 1));
			template.EnumeratePixels((a, b, color) => {
				a += rect.X + counter.X + X;
				b += rect.Y + counter.Y;
				GenerationHelper.FastRemoveTile(a, b);
				if (color.R == 255 && color.B == 0 && color.G == 0) {
					GenerationHelper.FastPlaceTile(a, b, TileID.BlueDungeonBrick);
				}
				GenerationHelper.FastPlaceWall(a, b, WallID.BlueDungeon);
			});
		}
	}

}
