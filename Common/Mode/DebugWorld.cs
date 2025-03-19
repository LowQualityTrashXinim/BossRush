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
	public void GenerateHorizonTemplate() {
		for (int i = 0; i < 9; i++) {
			Rectangle re = GenerationHelper.GridPositionInTheWorld24x24(new(1, 1, 64, 32));
			re.X += re.Width * i + 10 * i;
			GenerationHelper.PlaceStructure("Template/WG_Template" + "Horizontal" + i, re);
		}
	}
}
