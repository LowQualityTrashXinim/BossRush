using BossRush.Contents.Items.Consumable.SpecialReward;
using BossRush.Contents.NPCs;
using BossRush.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Consumable.Spawner {
	internal class LootboxLordSummon : BaseSpawnerItem {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override int[] NPCtypeToSpawn => new int[] { ModContent.NPCType<LootBoxLord>() };
		public override void PostSetStaticDefaults() {
			NPCID.Sets.MPAllowedEnemies[ModContent.NPCType<LootBoxLord>()] = true;
		}
		public override void SetSpawnerDefault(out int width, out int height) {
			height = 32;
			width = 32;
		}
		public override bool UseSpecialSpawningMethod => true;
		public override void SpecialSpawningLogic(Player player) {
			int spawnY = 250;
			NPC.SpawnBoss((int)player.Center.X, (int)(player.Center.Y - spawnY), NPCtypeToSpawn[0], player.whoAmI);
		}
	}
}
