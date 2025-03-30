using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.BuffAndDebuff;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class CopperArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.CopperHelmet;
		bodyID = ItemID.CopperChainmail;
		legID = ItemID.CopperGreaves;
	}
}
public class CopperHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.CopperHelmet;
		Add_Defense = 2; 
		TypeEquipment = Type_Head;
		ArmorName = "CopperArmor";
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetModPlayer<RoguelikeArmorPlayer>().ElectricityChance += .02f;
	}
}
public class CopperChainmail : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.CopperChainmail;
		Add_Defense = 3;
		TypeEquipment = Type_Body;
		ArmorName = "CopperArmor";
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetModPlayer<RoguelikeArmorPlayer>().ElectricityChance += .03f;
	}
}
public class CopperGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.CopperGreaves;
		Add_Defense = 2;
		TypeEquipment = Type_Leg;
		ArmorName = "CopperArmor";
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetModPlayer<RoguelikeArmorPlayer>().ElectricityChance += .01f;
	}
}
public class CopperArmorPlayer : PlayerArmorHandle {
	int CopperArmorChargeCounter = 0;
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("CopperArmor", this);
	}
	public override void Armor_UpdateEquipsSet() {
		Player.moveSpeed += 0.25f;
	}
	public override void Armor_OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		OnHitNPC_CopperArmor();
	}
	private void OnHitNPC_CopperArmor() {
		if (Player.ZoneRain)
			CopperArmorChargeCounter++;
		if (++CopperArmorChargeCounter >= 50) {
			Player.AddBuff(ModContent.BuffType<OverCharged>(), 300);
			CopperArmorChargeCounter = 0;
		}
	}
}
