using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.Accessories.ReworkedAccessories;
public class LuckyCoin : ItemReworker {

	public override int VanillaItemType => ItemID.LuckyCoin;

	public override void UpdateEquip(Player player) {
		player.luck += 0.15f;
	}

}
