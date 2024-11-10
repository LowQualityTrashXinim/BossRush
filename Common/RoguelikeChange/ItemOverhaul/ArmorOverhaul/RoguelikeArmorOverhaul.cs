using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using System.Collections.Generic;
using System.Linq;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul;
class RoguelikeArmorOverhaul : GlobalItem {
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (!UniversalSystem.Check_RLOH()) {
			return;
		}
		Player player = Main.LocalPlayer;
		ModifyArmorSetToolTip(player, item, tooltips);
		ModifyArmorTooltip(item, tooltips);
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
	private void ModifyArmorTooltip(Item item, List<TooltipLine> tooltips) {
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
	public ModArmorSet ActiveArmor = ArmorLoader.Default;
	public List<ModArmorSet> ForceActive = new();
	public bool ArmorSetCheck(ModPlayer modplayer = null) {
		if (ActiveArmor.Equals(ArmorLoader.Default) && ForceActive == null && ForceActive.Count < 1) {
			return false;
		}
		if (ActiveArmor.modplayer != null && ActiveArmor.modplayer.Name == modplayer.Name) {
			return true;
		}
		if (ForceActive != null && ForceActive.Where(ar => !ar.Equals(ArmorLoader.Default) && ar.modplayer.Name == modplayer.Name).Any()) {
			return true;
		}
		return false;
	}
	public void SafeAddArmorSet(string armorSetName) {
		ModArmorSet set = ArmorLoader.GetModArmor(armorSetName);
		if (set.Equals(ArmorLoader.Default)) {
			return;
		}
		ForceActive.Add(set);
	}
	public override void ResetEffects() {
		ForceActive.Clear();
		ActiveArmor = ArmorLoader.GetModArmor(Player.armor[0].type, Player.armor[1].type, Player.armor[2].type);
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
