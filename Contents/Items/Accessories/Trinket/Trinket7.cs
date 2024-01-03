using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.Trinket;
internal class Trinket7 : BaseTrinket{
	public override void UpdateTrinket(Player player, TrinketPlayer modplayer) {
		player.GetDamage(DamageClass.Melee) += .35f;
	}
}
