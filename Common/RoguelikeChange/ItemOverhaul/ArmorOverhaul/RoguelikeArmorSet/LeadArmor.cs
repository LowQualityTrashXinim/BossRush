using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.BuffAndDebuff;
using BossRush.Common.Systems;

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
		ArmorName = "LeadArmor";
		TypeEquipment = Type_Head;
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.DebuffDurationInflict, 1 + .1f);
	}
}
public class LeadChainmail : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.LeadChainmail;
		Add_Defense = 3;
		ArmorName = "LeadArmor";
		TypeEquipment = Type_Body;
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.DebuffDurationInflict, 1 + .1f);
	}
}

public class LeadGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.LeadGreaves;
		Add_Defense = 2; 
		ArmorName = "LeadArmor";
		TypeEquipment = Type_Leg;
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.DebuffDurationInflict, 1 + .1f);
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
