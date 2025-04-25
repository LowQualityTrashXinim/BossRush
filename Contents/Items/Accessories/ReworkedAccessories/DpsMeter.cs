using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.Accessories.ReworkedAccessories;
public class DpsMeter : ItemReworker {


	public override int VanillaItemType => ItemID.DPSMeter;

	public override void UpdateEquip(Player player) {

		player.GetDamage(Terraria.ModLoader.DamageClass.Generic).Flat += 5;
		player.GetAttackSpeed(Terraria.ModLoader.DamageClass.Generic) += 0.10f;

	}

}
