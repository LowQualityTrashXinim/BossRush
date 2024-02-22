using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Contents.NPCs;

namespace BossRush.Contents.Items.Spawner;
internal class SpawnMerchant : BaseSpawnerItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override int[] NPCtypeToSpawn => new int[] { ModContent.NPCType<M_018T>() };
	public override void SetSpawnerDefault(out int width, out int height) {
		height = 32;
		width = 32;
	}
}
