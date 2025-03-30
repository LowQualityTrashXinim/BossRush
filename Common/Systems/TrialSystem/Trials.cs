using BossRush.Contents.Items.Chest;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.Systems.TrialSystem;

public class TestTrial : ModTrial {
	public override List<int> NPCpool(int currentWave) {
		return new() {
			NPCID.Zombie,
			NPCID.Skeleton,
			NPCID.DemonEye,
			NPCID.Skeleton,
		};
	}
	//public override Dictionary<int, int> NPCpool(int currentWave) {
	//	return new Dictionary<int, int>() {
	//		{ NPCID.Zombie, 3 },
	//		{ NPCID.Skeleton, 4},
	//		{NPCID.DemonEye, 5 }
	//	};
	//}
	public override int WaveAmount() {
		return 1;
	}
	public override void TrialReward(IEntitySource source, Player player) {
		player.QuickSpawnItem(source, ModContent.ItemType<WeaponLootBox>());
	}
	public override Rectangle TrialSize(Vector2 TrialStartPosition) {
		int width = 25, height = 25;
		Point toTileCoord = new Vector2(TrialStartPosition.X, TrialStartPosition.Y).ToTileCoordinates();
		return new Rectangle(toTileCoord.X - width, toTileCoord.Y - height, width * 2, height * 2);
	}
}
