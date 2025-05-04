using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.ReworkedAccessories;
public class SharkToothNecklace : ItemReworker {

	public override int VanillaItemType => ItemID.SharkToothNecklace;

	public override void UpdateEquip(Player player) {
		player.GetArmorPenetration<GenericDamageClass>() += 5;
		player.GetDamage<GenericDamageClass>() += player.GetArmorPenetration<GenericDamageClass>() * 0.015f;
	}

}
