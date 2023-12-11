using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using System.Collections.Generic;

namespace BossRush.Common.WorldGenOverhaul.XinimVer;
public partial class RogueLikeWorldGen : ModSystem{
	public static int GridPart_X = Main.maxTilesX / 24;
	public static int GridPart_Y = Main.maxTilesY / 24;
	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
		tasks.Clear();
		tasks.AddRange(((ITaskCollection)this).Tasks);
	}
}
