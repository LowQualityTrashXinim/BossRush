using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.Trinket;
public class Trinket_of_Swift_Health : BaseTrinket {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.DefaultToAccessory();
	}
	public override void UpdateTrinket(Player player, Trinketplayer modplayer) {
		modplayer.Trinket1 = true;
		modplayer.HPstats += .2f;
	}
}
public class SwiftSteal_Buff : TrinketBuff {
	public override void UpdateTrinketPlayer(Player player, Trinketplayer modplayer, ref int buffIndex) {
		player.statDefense += modplayer.Trinket1_Point * 4;
		player.GetCritChance(DamageClass.Generic) += 3 * modplayer.Trinket1_Point;
		player.GetAttackSpeed(DamageClass.Generic) += .1f;
		player.moveSpeed += .25f;
		modplayer.DamageStats.Base += player.statLife * .05f;
	}
	public override void OnEnded(Player player, Trinketplayer modplayer) {
		modplayer.Trinket1_Delay = 1500;
	}
}
