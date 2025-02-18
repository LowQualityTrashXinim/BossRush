using Terraria;
using Terraria.ID;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class BorealwoodArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.BorealWoodHelmet;
		bodyID = ItemID.BorealWoodBreastplate;
		legID = ItemID.BorealWoodGreaves;
	}
}
class BorealWoodHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.BorealWoodHelmet;
		Add_Defense = 3;
		ArmorName = "BorealwoodArmor";
		AddTooltip = true;
		TypeEquipment = Type_Head;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetModPlayer<RoguelikeArmorPlayer>().FrostBurnChance += .01f;
	}
}
class BorealWoodBreastplate : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.BorealWoodBreastplate;
		Add_Defense = 4;
		ArmorName = "BorealwoodArmor";
		AddTooltip = true;
		TypeEquipment = Type_Body;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetModPlayer<RoguelikeArmorPlayer>().FrostBurnChance += .02f;
	}
}
class BorealWoodGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.BorealWoodGreaves;
		Add_Defense = 3;
		ArmorName = "BorealwoodArmor";
		AddTooltip = true;
		TypeEquipment = Type_Leg;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetModPlayer<RoguelikeArmorPlayer>().FrostBurnChance += .01f;
	}
}
class BorealWoodArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("BorealwoodArmor", this);
	}
	public override void Armor_UpdateEquipsSet() {
		Player.statDefense += 5;
		Player.moveSpeed += .20f;
		Player.buffImmune[BuffID.Chilled] = true;
		Player.buffImmune[BuffID.Slow] = true;
		Player.GetModPlayer<RoguelikeArmorPlayer>().FrostBurnChance += .07f;
	}
	public override void Armor_OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (target.HasBuff(BuffID.Frostburn) || target.HasBuff(BuffID.Frostburn2)) {
			Player.AddBuff(BuffID.Frostburn, BossRushUtils.ToSecond(10));
		}
	}
	public override void Armor_ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
	}
}
