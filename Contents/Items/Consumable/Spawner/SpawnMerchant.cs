using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Contents.NPCs;
using Terraria;

namespace BossRush.Contents.Items.Consumable.Spawner;
internal class SpawnMerchant : BaseSpawnerItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override int[] NPCtypeToSpawn => new int[] { ModContent.NPCType<M_018T>() };
	public override bool UseSpecialSpawningMethod => true;
	public override void SpecialSpawningLogic(Player player) {
		NPC.NewNPC(player.GetSource_ItemUse(Item), (int)player.position.X, (int)player.position.Y, ModContent.NPCType<M_018T>());
	}
	public override void SetSpawnerDefault(out int width, out int height) {
		height = 32;
		width = 32;
	}
}
