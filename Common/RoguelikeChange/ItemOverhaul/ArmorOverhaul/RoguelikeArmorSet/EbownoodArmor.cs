using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class EbownoodArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.EbonwoodHelmet;
		bodyID = ItemID.EbonwoodBreastplate;
		legID = ItemID.EbonwoodGreaves;
	}
}
