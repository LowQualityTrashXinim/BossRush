using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using System.Collections.Generic;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul;
class RoguelikeArmorOverhaul : GlobalItem {
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (!UniversalSystem.Check_RLOH()) {
			return;
		}
		Player player = Main.LocalPlayer;
		ModifyArmorSetToolTip(player, item, tooltips);
		ModifyArmorTooltip(player, item, tooltips);
	}
	private void ModifyArmorSetToolTip(Player player, Item item, List<TooltipLine> tooltips) {
		int index = tooltips.FindIndex(line => line.Name == "SetBonus");
		if (index == -1) {
			return;
		}
		if (player.TryGetModPlayer(out RoguelikeArmorPlayer modplayer)) {
			var armor = modplayer.ActiveArmor;
			if (!armor.ContainAnyOfArmorPiece(item.type)) {
				return;
			}
			if (armor.Name == "None") {
				return;
			}
			string text = armor.SetBonusToolTip;
			if (armor.OverrideOriginalToolTip) {
				tooltips[index].Text = text;
			}
			else {
				tooltips[index].Text += "\n" + text;
			}
		}
	}
	private void ModifyArmorTooltip(Player player, Item item, List<TooltipLine> tooltips) {
		int index = tooltips.FindIndex(line => line.Name == "Defense");
		var info = ArmorLoader.GetArmorPieceInfo(item.type);
		if (info == null) {
			return;
		}

		if (index == -1) {
			tooltips.Insert(2, new(Mod, "Defense", $"{info.Add_Defense} Defense"));
			return;
		}
		string text = tooltips[index].Text;
		string defenseStringSimulation = "";
		int indexWhereNumEnd = 0;
		for (int i = 0; i < text.Length; i++) {
			if (char.IsNumber(text[i])) {
				defenseStringSimulation += text[i];
			}
			else {
				indexWhereNumEnd = i;
				break;
			}
		}
		int defense = int.Parse(defenseStringSimulation);
		text = text.Substring(indexWhereNumEnd);
		tooltips[index].Text = (defense + info.Add_Defense) + text;

		index = tooltips.FindIndex(line => line.Name == "Tooltip0");
		var armorinfo = ArmorLoader.GetArmorPieceInfo(item.type);
		if (index == -1) {
			if (armorinfo == null) {
				return;
			}
			if (armorinfo.AddTooltip) {
				tooltips.Insert(3, new(Mod, $"{Mod.Name}_Tooltip0", armorinfo.ToolTip));
			}
		}
		else {
			if (armorinfo.AddTooltip) {
				if (armorinfo.OverrideTooltip) {
					tooltips[index].Text = armorinfo.ToolTip;
				}
				else {
					tooltips[index].Text += "\n" + armorinfo.ToolTip;
				}
			}
		}
	}
	private string GetToolTip(int type) {
		//if (type == ItemID.PlatinumHelmet || type == ItemID.PlatinumChainmail || type == ItemID.PlatinumGreaves) {
		//	return Language.GetTextValue($"Mods.BossRush.ArmorSet.PlatinumArmor");
		//}
		return "";
	}
	public override void UpdateArmorSet(Player player, string set) {
		RoguelikeArmorPlayer modplayer = player.GetModPlayer<RoguelikeArmorPlayer>();

		if (OreTypeArmor(player, modplayer, set)) { return; }
	}
	private bool OreTypeArmor(Player player, RoguelikeArmorPlayer modplayer, string set) {
		if (set == ArmorSet.ConvertIntoArmorSetFormat(ItemID.PlatinumHelmet, ItemID.PlatinumChainmail, ItemID.PlatinumGreaves)) {
			modplayer.PlatinumArmor = true;
			return true;
		}
		return false;
	}
	public override void UpdateEquip(Item item, Player player) {
		int type = item.type;

		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		ModArmorPiece def = ArmorLoader.GetArmorPieceInfo(type);
		if (def != null) {
			def.UpdateEquip(player, item);
			if (def.Add_Defense > 0)
				modplayer.AddStatsToPlayer(PlayerStats.Defense, Base: def.Add_Defense);
		}
		if (item.type == ItemID.NightVisionHelmet) {
			player.GetModPlayer<RangerOverhaulPlayer>().SpreadModify -= .25f;
		}
		if (item.type == ItemID.VikingHelmet) {
			player.GetModPlayer<GlobalItemPlayer>().RoguelikeOverhaul_VikingHelmet = true;
		}
		if (item.type == ItemID.ObsidianRose || item.type == ItemID.ObsidianSkullRose) {
			player.buffImmune[BuffID.OnFire] = true;
		}
	}
}
class RoguelikeArmorPlayer : ModPlayer {
	public bool PlatinumArmor = false;
	int PlatinumArmorCountEffect = 0;
	public ModArmorSet ActiveArmor = ArmorLoader.Default;
	public override void ResetEffects() {
		ActiveArmor = ArmorLoader.GetModArmor(Player.armor[0].type, Player.armor[1].type, Player.armor[2].type);
		PlatinumArmor = false;
	}
	public override void UpdateDead() {
		PlatinumArmor = false;
	}
	public override void PreUpdate() {
		if (PlatinumArmor) {
			if (Player.ItemAnimationActive) {
				PlatinumArmorCountEffect = Math.Clamp(PlatinumArmorCountEffect + 1, 0, 1200);
			}
			else {
				PlatinumArmorCountEffect = BossRushUtils.CountDown(PlatinumArmorCountEffect);
			}
		}
	}
	public override void PostUpdate() {
		if (PlatinumArmorCountEffect >= 600) {
			Player.AddBuff(BuffID.OnFire, 300);
			Dust.NewDust(Player.Center, 0, 0, DustID.Torch, 0, 0, 0, default, Main.rand.NextFloat(1, 1.5f));
		}
	}
	public override float UseSpeedMultiplier(Item item) {
		if (PlatinumArmor) {
			return 1.35f;
		}
		return base.UseSpeedMultiplier(item);
	}
}
public class ArmorSet {
	public int headID, bodyID, legID;
	protected string ArmorSetBonusToolTip = "";
	public ArmorSet(int headID, int bodyID, int legID) {
		this.headID = headID;
		this.bodyID = bodyID;
		this.legID = legID;
	}
	public static string ConvertIntoArmorSetFormat(int headID, int bodyID, int legID) => $"{headID}:{bodyID}:{legID}";
	/// <summary>
	/// Expect there is only 3 item in a array
	/// </summary>
	/// <param name="armor"></param>
	/// <returns></returns>
	public static string ConvertIntoArmorSetFormat(int[] armor) => $"{armor[0]}:{armor[1]}:{armor[2]}";
	public override string ToString() => $"{headID}:{bodyID}:{legID}";

	public bool ContainAnyOfArmorPiece(int type) => type == headID || type == bodyID || type == legID;
}
