using BossRush.Common.Global;
using BossRush.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class NinjaArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.NinjaHood;
		bodyID = ItemID.NinjaShirt;
		legID = ItemID.NinjaPants;
	}
}
public class NinjaHood : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.NinjaHood;
		Add_Defense = 2;
		TypeEquipment = Type_Head;
		ArmorName = "NinjaArmor";
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetModPlayer<PlayerStatsHandle>().UpdateCritDamage += .15f;
		player.GetModPlayer<PlayerStatsHandle>().DodgeChance += .03f;
	}
}
public class NinjaShirt : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.NinjaShirt;
		Add_Defense = 2;
		TypeEquipment = Type_Body;
		ArmorName = "NinjaArmor";
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetModPlayer<PlayerStatsHandle>().AttackSpeed += .05f;
		player.GetModPlayer<PlayerStatsHandle>().DodgeChance += .05f;
	}
}
public class NinjaPants : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.NinjaPants;
		Add_Defense = 2;
		TypeEquipment = Type_Leg;
		ArmorName = "NinjaArmor";
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetDamage(DamageClass.Generic) += .05f;
		player.GetModPlayer<PlayerStatsHandle>().DodgeChance += .02f;
	}
}
public class NinjaArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("NinjaArmor", this);
	}
	public override void Armor_UpdateEquipsSet() {
		if (Player.immune) {
			Player.GetCritChance<GenericDamageClass>() += 50;
			Player.ModPlayerStats().UpdateCritDamage += 1.5f;
		}
	}
	public override bool Armor_FreeDodge(Player.HurtInfo info) {
		if (Player.HasBuff<DodgeJutsu>()) {
			Player.DelBuff(Player.FindBuffIndex(ModContent.BuffType<DodgeJutsu>()));
			Player.AddBuff<DodgeJutsuCoolDown>(BossRushUtils.ToSecond(20));
			return true;
		}
		return false;
	}

}
public class DodgeJutsu : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
	}
}
public class DodgeJutsuCoolDown : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
	}
}
