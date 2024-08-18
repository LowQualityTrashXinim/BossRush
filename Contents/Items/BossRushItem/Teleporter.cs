using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Systems;

namespace BossRush.Contents.Items.BossRushItem;
class ExoticTeleporter : ModItem {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.CellPhone);
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32);
	}
	public override bool? UseItem(Player player) {
		if (player.itemAnimation == player.itemAnimationMax && !BossRushUtils.IsAnyVanillaBossAlive()) {
			ModContent.GetInstance<UniversalSystem>().ActivateTeleportUI();
		}
		return base.UseItem(player);
	}
}
