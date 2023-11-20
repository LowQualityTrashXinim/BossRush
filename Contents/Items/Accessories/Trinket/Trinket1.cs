using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using System;

namespace BossRush.Contents.Items.Accessories.Trinket;
public class Trinket_of_Swift_Health : BaseTrinket {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.DefaultToAccessory();
	}
	public override void UpdateTrinket(Player player, TrinketPlayer modplayer) {
		modplayer.Trinket_of_Swift_Health = true;
		modplayer.HPstats += .2f;
	}
}
public class SwiftSteal_Buff : TrinketBuff {
	public override void UpdateTrinketPlayer(Player player, TrinketPlayer modplayer, ref int buffIndex) {
		modplayer.HPstats += .05f * modplayer.Trinket_of_Swift_Health_PointCounter;
		player.GetCritChance(DamageClass.Generic) += 3 * modplayer.Trinket_of_Swift_Health_PointCounter;
		player.GetAttackSpeed(DamageClass.Generic) += .1f;
		player.moveSpeed += .25f;
		modplayer.DamageStats.Base += player.statLife * .05f;
	}
	public override void OnEnded(Player player, TrinketPlayer modplayer) {
		modplayer.Trinket_of_Swift_Health_CoolDown = BossRushUtils.ToSecond(60);
	}
}
