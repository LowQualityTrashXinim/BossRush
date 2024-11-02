using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class SilverArmor : ModArmorSet{
	public override void SetDefault() {
		headID = ItemID.SilverHelmet;
		bodyID = ItemID.SilverChainmail;
		legID = ItemID.SilverGreaves;
	}
}
public class SilverHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.SilverHelmet;
		Add_Defense = 2;
	}
}
public class SilverChainmail : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.SilverChainmail;
		Add_Defense = 3;
	}
}

public class SilverGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.SilverGreaves;
		Add_Defense = 2;
	}
}
public class SilverArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("SilverArmor", this);
	}
	public override void Armor_UpdateEquipsSet() {
		bool IsAbover = Player.statLife < Player.statLifeMax2 * .75f;
		if (Main.IsItDay()) {
			Player.statDefense += IsAbover ? 10 : 20;
		}
		else {
			Player.GetDamage(DamageClass.Generic) += IsAbover ? .1f : .2f;
		}
	}
}
