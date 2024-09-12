using BossRush.Contents.Items.Chest;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.Systems.TrialSystem;

public class TestTrial : ModTrial {
	public override Dictionary<int, int> NPCpool() {
		return new Dictionary<int, int>() {
			{ NPCID.Zombie, 3 },
			{ NPCID.Skeleton, 4},
			{NPCID.DemonEye, 5 }
		};
	}
	public override int WaveAmount() {
		return 1;
	}
	public override void TrialReward(IEntitySource source, Player player) {
		player.QuickSpawnItem(source, ModContent.ItemType<WeaponLootBox>());
	}
	public override Rectangle TrialSize(Vector2 TrialStartPosition) {
		int width = 200, height = 100;
		return new Rectangle((int)TrialStartPosition.X - width, (int)TrialStartPosition.Y - height, width * 2, height * 2);
	}
}
