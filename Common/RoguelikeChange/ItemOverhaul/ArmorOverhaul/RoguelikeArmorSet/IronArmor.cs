using Terraria;
using Terraria.ID;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class IronArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.IronHelmet;
		bodyID = ItemID.IronChainmail;
		legID = ItemID.IronGreaves;
	}
}
public class IronHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.IronHelmet;
		Add_Defense = 3;
	}
}
public class IronChainmail : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.IronChainmail;
		Add_Defense = 4;
	}
}

public class IronGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.IronGreaves;
		Add_Defense = 3;
	}
}
public class IronArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("IronArmor", this);
	}
	public override void Armor_UpdateEquipsSet() {
		Player.endurance += 0.1f;
		Player.DefenseEffectiveness *= 1.25f;
		if (!Player.ComparePlayerHealthInPercentage(.5f)) {
			Player.statDefense += 25;
		}
	}
	public override void Armor_ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if(Main.rand.NextBool(5)) {
			modifiers.FinalDamage -= .5f;
		}
	}
	public override void Armor_ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (Main.rand.NextBool(5)) {
			modifiers.FinalDamage -= .5f;
		}
	}
}
