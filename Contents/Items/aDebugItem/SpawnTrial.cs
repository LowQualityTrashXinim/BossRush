using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.aDebugItem;
internal class SpawnTrial : ModItem{
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32);
		Item.Set_DebugItem(true);
	}
	public override bool? UseItem(Player player) {
		return base.UseItem(player);
	}
}
