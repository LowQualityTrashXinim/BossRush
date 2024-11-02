using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.BuffAndDebuff;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class LeadArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.LeadHelmet;
		bodyID = ItemID.LeadChainmail;
		legID = ItemID.LeadGreaves;
	}
}
public class LeadHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.LeadHelmet;
		Add_Defense = 2;
	}
}
public class LeadChainmail : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.LeadChainmail;
		Add_Defense = 3;
	}
}

public class LeadGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.LeadGreaves;
		Add_Defense = 2;
	}
}
public class LeadArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("LeadArmor", this);
	}
	public override void Armor_UpdateEquipsSet() {
		Player.statDefense += 7;
	}
	public override void Armor_OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(ModContent.BuffType<LeadIrradiation>(), 600);
	}
}
