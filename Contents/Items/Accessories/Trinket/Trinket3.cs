using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.Trinket;
internal class Trinket3 : BaseTrinket {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.DefaultToAccessory();
	}
	public override void UpdateTrinket(Player player, TrinketPlayer modplayer) {
		modplayer.Trinket3 = true;
		player.GetCritChance(DamageClass.Generic) += 12 + 3 * modplayer.Trinket3_PointCounter;
		modplayer.DamageStats += .06f * modplayer.Trinket3_PointCounter;
	}
}
