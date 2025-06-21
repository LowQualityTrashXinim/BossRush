using System;
using Terraria;
using System.Linq;
using System.Text;
using Terraria.ID;
using BossRush.Common.Global;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class GladiatorArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.GladiatorHelmet;
		bodyID = ItemID.GladiatorBreastplate;
		legID = ItemID.GladiatorLeggings;
	}
}
public class GladiatorHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.GladiatorHelmet;
		Add_Defense = 6;
		TypeEquipment = Type_Head;
		ArmorName = "GladiatorArmor";
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.ModPlayerStats().UpdateCritDamage += .15f;
	}
}
public class GladiatorBreastplate : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.GladiatorBreastplate;
		Add_Defense = 6;
		TypeEquipment = Type_Body;
		ArmorName = "GladiatorArmor";
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.endurance += .05f;
	}
}
public class GladiatorLeggings : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.GladiatorLeggings;
		Add_Defense = 5;
		TypeEquipment = Type_Leg;
		ArmorName = "GladiatorArmor";
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.ModPlayerStats().UpdateMovement += .1f;
		player.ModPlayerStats().UpdateJumpBoost += .1f;
	}
}
public class GladiatorArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("GladiatorArmor", this);
	}
	public override void Armor_UpdateEquipsSet() {
		PlayerStatsHandle modplayer = Player.ModPlayerStats();
		modplayer.UpdateDefenseBase.Base += 10;
		int percentage = (int)((1 - Player.statLife / (float)Player.statLifeMax2) * 10);
		float increases = MathF.Round(percentage / 4f * .1f, 2);
		modplayer.UpdateMovement += increases;
		modplayer.UpdateJumpBoost += increases;
		modplayer.UpdateCritDamage += increases;
		Player.endurance += increases;
	}
}
