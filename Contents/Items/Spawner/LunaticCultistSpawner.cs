using BossRush.Texture;
using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.Spawner {
	internal class LunaticCultistSpawner : BaseSpawnerItem {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override int[] NPCtypeToSpawn => new int[] { NPCID.CultistBoss };
		public override bool UseSpecialSpawningMethod => true;
		public override void SpecialSpawningLogic(Player player) {
			int spawnY = 250;
			NPC.SpawnBoss((int)(player.Center.X), (int)(player.Center.Y - spawnY), NPCtypeToSpawn[0], player.whoAmI);
		}
	}
}