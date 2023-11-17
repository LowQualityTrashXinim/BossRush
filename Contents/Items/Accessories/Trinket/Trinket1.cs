using Terraria;
using BossRush.Texture;

namespace BossRush.Contents.Items.Accessories.Trinket;
public class Trinket1 : BaseTrinket {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.DefaultToAccessory();
	}
	public override void UpdateTrinket(Player player, Trinketplayer modplayer) {
		modplayer.Trinket1 = true;
		modplayer.HPstats += .2f;
		player.statDefense += modplayer.Trinket1_Point * 4;
	}
}
public class Trinket1_Buff : TrinketBuff {
	public override void UpdateTrinketPlayer(Player player, Trinketplayer modplayer, ref int buffIndex) {
		player.accRunSpeed += .25f;
	}
}
