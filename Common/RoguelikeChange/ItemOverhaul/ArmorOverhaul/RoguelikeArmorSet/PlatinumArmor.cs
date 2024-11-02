using Terraria.ID;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class PlatinumArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.PlatinumHelmet;
		bodyID = ItemID.PlatinumChainmail;
		legID = ItemID.PlatinumGreaves;
	}
}
public class PlatinumHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.PlatinumHelmet;
		Add_Defense = 2;
	}
}
public class PlatinumChainmail : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.PlatinumChainmail;
		Add_Defense = 2;
	}
}
public class PlatinumGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.PlatinumGreaves;
		Add_Defense = 2;
	}
}
