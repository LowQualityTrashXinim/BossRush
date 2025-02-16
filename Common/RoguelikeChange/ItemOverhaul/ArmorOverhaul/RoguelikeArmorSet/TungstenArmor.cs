using BossRush.Common.Systems;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class TungstenArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.TungstenHelmet;
		bodyID = ItemID.TungstenChainmail;
		legID = ItemID.TungstenGreaves;
	}
}
public class TungstenHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.TungstenHelmet;
		Add_Defense = 4;
		AddTooltip = true;
		TypeEquipment = Type_Head;
		ArmorName = "TungstenArmor";
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.RangeDMG, 1.06f);
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 3);
	}
}
public class TungstenChainmail : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.TungstenChainmail;
		Add_Defense = 5;
		AddTooltip = true;
		TypeEquipment = Type_Body;
		ArmorName = "TungstenArmor";
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.MeleeDMG, 1.06f);
		modplayer.AddStatsToPlayer(PlayerStats.MagicDMG, 1.06f);
	}
}

public class TungstenGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.TungstenGreaves;
		Add_Defense = 4;
		AddTooltip = true;
		TypeEquipment = Type_Leg;
		ArmorName = "TungstenArmor";
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.SummonDMG, 1.06f);
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1.08f);
	}
}
public class TungstenArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("TungstenArmor", this);
	}
	public override void Armor_UpdateEquipsSet() {
		Player.moveSpeed += .3f;
		if (!Player.HasBuff<TungstenEmpowerment>()) {
			Player.AddBuff<TungstenEmpowerment>(BossRushUtils.ToSecond(10));
		}
	}
	public int Rotate = 0;
}
public class TungstenEmpowerment : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) {
		Player player = Main.LocalPlayer;
		int rotateValue = player.GetModPlayer<TungstenArmorPlayer>().Rotate;
		switch (rotateValue) {
			case 0:
				tip += "20% damage increased";
				break;
			case 1:
				tip += "30% melee damage increased";
				break;
			case 2:
				tip += "30% range damage increased";
				break;
			case 3:
				tip += "30% magic damage increased";
				break;
			case 4:
				tip += "30% summon damage increased";
				break;
			case 5:
				tip += "50% synergy damage increased";
				break;
		}
	}
	public override void Update(Player player, ref int buffIndex) {
		TungstenArmorPlayer modplayer = player.GetModPlayer<TungstenArmorPlayer>();
		if (player.buffTime[buffIndex] <= 0) {
			modplayer.Rotate = BossRushUtils.Safe_SwitchValue(modplayer.Rotate, 5);
		}
		switch (modplayer.Rotate) {
			case 0:
				PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, 1.2f);
				break;
			case 1:
				PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.MeleeDMG, 1.3f);
				break;
			case 2:
				PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RangeDMG, 1.3f);
				break;
			case 3:
				PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.MagicDMG, 1.3f);
				break;
			case 4:
				PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.SummonDMG, 1.3f);
				break;
			case 5:
				PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.SynergyDamage, 1.5f);
				break;
		}
	}
}
