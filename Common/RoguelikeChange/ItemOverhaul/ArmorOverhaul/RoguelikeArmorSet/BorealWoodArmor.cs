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
		RoguelikeArmorPlayer roguelike = player.GetModPlayer<RoguelikeArmorPlayer>();
		roguelike.FrostBurnChance += .02f;
		roguelike.SnowBallDamage = true;
		roguelike.SnowSpawnChance += .03f;
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
		RoguelikeArmorPlayer roguelike = player.GetModPlayer<RoguelikeArmorPlayer>();
		roguelike.FrostBurnChance += .03f;
		roguelike.ReplaceSnowBallWithSnow = true;
		roguelike.SnowSpawnChance += .04f;
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
		RoguelikeArmorPlayer roguelike = player.GetModPlayer<RoguelikeArmorPlayer>();
		roguelike.FrostBurnChance += .01f;
		roguelike.RunningCauseSnowToShoot = true;
		roguelike.SnowSpawnChance += .02f;
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
		RoguelikeArmorPlayer rougelike = Player.GetModPlayer<RoguelikeArmorPlayer>();
		rougelike.FrostBurnChance += .07f;
		rougelike.SnowSpawnChance += .1f;

	}
	public override void Armor_OnHitByNPC(NPC target, Player.HurtInfo hurtInfo) {
		if (target.HasBuff(BuffID.Frostburn) || target.HasBuff(BuffID.Frostburn2)) {
			Player.AddBuff(BuffID.Frostburn, BossRushUtils.ToSecond(10));
		}
	}
	public override void Armor_ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
	}
}
