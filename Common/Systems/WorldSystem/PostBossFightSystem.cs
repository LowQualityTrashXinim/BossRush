using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.Systems.WorldSystem;
internal class PostBossFightSystem : ModSystem {

}
class PostBossFightGlobalNPC : GlobalNPC {
	public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) {
		return;
		if (NPC.AnyNPCs(NPCID.KingSlime)) {
			//Slime
			pool.Add(NPCID.GreenSlime, 1.75f);
			pool.Add(NPCID.BlueSlime, 1.75f);
			pool.Add(NPCID.PurpleSlime, 1.75f);
			pool.Add(NPCID.RedSlime, 1.75f);
			pool.Add(NPCID.YellowSlime, 1.75f);
			pool.Add(NPCID.BlackSlime, 1.75f);
			pool.Add(NPCID.MotherSlime, 1.75f);
			pool.Add(NPCID.SpikedJungleSlime, 1.55f);
			pool.Add(NPCID.SpikedIceSlime, 1.55f);
			pool.Add(NPCID.UmbrellaSlime, 1.75f);
			pool.Add(NPCID.SlimeSpiked, 1.75f);
			if (Main.getGoodWorld) {
				pool.Add(NPCID.LavaSlime, 0.75f);
			}
		}
		if (NPC.AnyNPCs(NPCID.EyeofCthulhu)) {
			//eye
			pool.Add(NPCID.DemonEye, 0.75f);
			pool.Add(NPCID.DemonEye2, 0.75f);
			pool.Add(NPCID.DemonEyeOwl, 0.75f);
			pool.Add(NPCID.DemonEyeSpaceship, 0.75f);
			pool.Add(NPCID.CataractEye, 0.75f);
			pool.Add(NPCID.CataractEye2, 0.75f);
			pool.Add(NPCID.DialatedEye, 0.75f);
			pool.Add(NPCID.DialatedEye2, 0.75f);
			pool.Add(NPCID.GreenEye, 0.75f);
			pool.Add(NPCID.GreenEye2, 0.75f);
			pool.Add(NPCID.PurpleEye, 0.75f);
			pool.Add(NPCID.PurpleEye2, 0.75f);
			pool.Add(NPCID.WanderingEye, 0.65f);
			pool.Add(NPCID.EyeballFlyingFish, 0.45f);
		}
		if (NPC.AnyNPCs(NPCID.EaterofWorldsBody) && spawnInfo.Player.ZoneOverworldHeight) {
			pool.Add(NPCID.Corruptor, 0.25f);
			pool.Add(NPCID.Slimer, 0.25f);
		}
		if (NPC.AnyNPCs(NPCID.BrainofCthulhu) && spawnInfo.Player.ZoneOverworldHeight) {
			pool.Add(NPCID.CrimsonBunny, 0.25f);
			pool.Add(NPCID.CrimsonGoldfish, 0.25f);
		}
		if (NPC.AnyNPCs(NPCID.QueenBee)) {
			//bee
			pool.Add(NPCID.Bee, 0.8f);
			pool.Add(NPCID.BeeSmall, 0.8f);
			//Hornet
			pool.Add(NPCID.Hornet, 0.7f);
			pool.Add(NPCID.HornetFatty, 0.7f);
			pool.Add(NPCID.HornetHoney, 0.7f);
			pool.Add(NPCID.HornetLeafy, 0.7f);
			pool.Add(NPCID.HornetSpikey, 0.7f);
			pool.Add(NPCID.HornetStingy, 0.7f);
			//MossHornet
			pool.Add(NPCID.MossHornet, 0.5f);
			pool.Add(NPCID.BigMossHornet, 0.5f);
			pool.Add(NPCID.GiantMossHornet, 0.5f);
			pool.Add(NPCID.LittleMossHornet, 0.5f);
			pool.Add(NPCID.TinyMossHornet, 0.5f);
		}
	}

	public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns) {
		return;
		if (NPC.AnyNPCs(NPCID.KingSlime)) {
			spawnRate = 70;
			maxSpawns = 150;
		}
		if (NPC.AnyNPCs(NPCID.EyeofCthulhu)) {
			spawnRate = 80;
			maxSpawns = 175;
		}
		if (NPC.AnyNPCs(NPCID.EaterofWorldsHead)) {
			spawnRate = 80;
			maxSpawns = 250;
		}
		if (NPC.AnyNPCs(NPCID.BrainofCthulhu) && player.ZoneOverworldHeight) {
			spawnRate = 80;
			maxSpawns = 250;
		}
		if (NPC.AnyNPCs(NPCID.QueenBee)) {
			spawnRate = 75;
			maxSpawns = 290;
		}
	}
}
