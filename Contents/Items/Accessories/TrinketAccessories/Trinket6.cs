using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace BossRush.Contents.Items.Accessories.Trinket;
internal class Trinket6 : BaseTrinket {
	public override void UpdateTrinket(Player player, TrinketPlayer modplayer) {
		player.DefenseEffectiveness *= 1.5f;
	}
}
