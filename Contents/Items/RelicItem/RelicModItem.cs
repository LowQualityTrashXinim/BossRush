﻿using System;
using Terraria;
using Terraria.ID;
using System.Linq;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.RelicItem;
public class Relic : ModItem {
	public const float chanceTier1 = .5f;
	public const float chanceTier2 = .6f;
	public const float chanceTier3 = .45f;
	public const float chanceTier4 = .6f;
	public static float GetTierChance(int tier) {
		if (tier <= 0) {
			return 0;
		}
		switch (tier) {
			case 1:
				return chanceTier1;
			case 2:
				return chanceTier2;
			case 3:
				return chanceTier3;
			case 4:
				return chanceTier4;
			default:
				return chanceTier4;
		}
	}
	public override string Texture => BossRushTexture.ACCESSORIESSLOT;
	List<int> templatelist = new List<int>();
	List<PlayerStats> statlist = new List<PlayerStats>();
	List<StatModifier> valuelist = new List<StatModifier>();
	public ColorInfo relicColor = new ColorInfo(new List<Color> { Color.Red, Color.Purple, Color.AliceBlue });
	public override void SetStaticDefaults() {
		relicColor = new ColorInfo(new List<Color> { Color.Red, Color.Purple, Color.AliceBlue });
	}
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.rare = ItemRarityID.Gray;
		Item.value = Item.buyPrice(silver: 50);
		Item.Set_InfoItem(true);
	}
	/// <summary>
	/// Use this to add stats before the item automatic add stats
	/// </summary>
	/// <param name="templateid"></param>
	/// <param name="value"></param>
	/// <param name="stats"></param>
	public void AddRelicTemplate(int templateid, StatModifier value, PlayerStats stats = PlayerStats.None, float valueMulti = 1) {
		if (templatelist == null) {
			templatelist = new List<int>();
			statlist = new List<PlayerStats>();
			valuelist = new List<StatModifier>();
		}
		templatelist.Add(templateid);
		RelicTemplateLoader.GetTemplate(templatelist.Count - 1).OnSettingTemplate();
		statlist.Add(stats);
		value = value.Scale(valueMulti);
		valuelist.Add(value);
	}
	/// <summary>
	/// Use this to add stats before the item automatic add stats
	/// </summary>
	/// <param name="templateid"></param>
	/// <param name="value"></param>
	/// <param name="stats"></param>
	public void AddRelicTemplate(Player player, int templateid, float valueMulti = 1) {
		if (templatelist == null) {
			templatelist = new List<int>();
			statlist = new List<PlayerStats>();
			valuelist = new List<StatModifier>();
		}
		templatelist.Add(templateid);
		RelicTemplateLoader.GetTemplate(templatelist.Count - 1).OnSettingTemplate();
		PlayerStats innerStats = RelicTemplateLoader.GetTemplate(templateid).StatCondition(this, player);
		statlist.Add(innerStats);
		StatModifier value = RelicTemplateLoader.GetTemplate(templateid).ValueCondition(this, player, innerStats);
		value = value.Scale(valueMulti);
		valuelist.Add(value);
	}
	/// <summary>
	/// Use this to add stats before the item automatic add stats
	/// </summary>
	/// <param name="templateid"></param>
	/// <param name="value"></param>
	/// <param name="stats"></param>
	public void AddRelicTemplate(Player player, int templateid, PlayerStats stats, float valueMulti = 1) {
		if (templatelist == null) {
			templatelist = new List<int>();
			statlist = new List<PlayerStats>();
			valuelist = new List<StatModifier>();
		}
		templatelist.Add(templateid);
		RelicTemplateLoader.GetTemplate(templatelist.Count - 1).OnSettingTemplate();
		statlist.Add(stats);
		StatModifier value = RelicTemplateLoader.GetTemplate(templateid).ValueCondition(this, player, stats);
		value = value.Scale(valueMulti);
		valuelist.Add(value);
	}
	public override ModItem Clone(Item newEntity) {
		Relic clone = (Relic)base.Clone(newEntity);
		if (templatelist == null) {
			clone.SetRelicData(null, null, null);
			return clone;
		}
		clone.SetRelicData(templatelist, statlist, valuelist);
		return clone;
	}
	public int RelicTier => templatelist != null ? templatelist.Count : 0;
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		TooltipLine NameLine = tooltips.Where(t => t.Name == "ItemName").FirstOrDefault();
		NameLine.Text = $"[Tier {TemplateCount}] {this.DisplayName}";
		NameLine.OverrideColor = relicColor.MultiColor(5);
		var index = tooltips.FindIndex(l => l.Name == "Tooltip0");
		if (templatelist == null || index == -1) {
			tooltips.Add(new TooltipLine(Mod, "", "Something gone wrong"));
			return;
		}
		string line = "";
		for (int i = 0; i < templatelist.Count; i++) {
			if (RelicTemplateLoader.GetTemplate(templatelist[i]) == null) {
				continue;
			}
			line += RelicTemplateLoader.GetTemplate(templatelist[i]).ModifyToolTip(this, statlist[i], valuelist[i]);
			//if (Main.LocalPlayer.IsDebugPlayer()) {
			//	line.Text +=
			//		$"\nTemplate Name : {RelicTemplateLoader.GetTemplate(templatelist[i]).FullName}" +
			//		$"\nTemplate Desc : {RelicTemplateLoader.GetTemplate(templatelist[i]).Description}" +
			//		$"\nTemplate ID : {templatelist[i]}" +
			//		$"\nStat to be increased : {Enum.GetName(typeof(PlayerStats), statlist[i])}" +
			//		$"\nIncreases value : Additive[{valuelist[i].Additive}] Multiplicative[{valuelist[i].Multiplicative}] Base[{valuelist[i].Base}] Flat[{valuelist[i].Flat}]";
			//}
			if (i + 1 != templatelist.Count) {
				line += "\n";
			}
		}
		tooltips.Insert(index, new(Mod, "Relic_Tooltip", line));
		tooltips.Add(new(Mod, "RelicItem", $"[Passive active item]") { OverrideColor = Main.DiscoColor });
	}
	/// <summary>
	/// This is shorthand for <see cref="AddRelicTemplate"/> where templateid is set random in a for loop
	/// </summary>
	/// <param name="amount"></param>
	public void AutoAddRelicTemplate(Player player, int amount) {
		for (int i = 0; i < amount; i++) {
			templatelist.Add(Main.rand.Next(RelicTemplateLoader.TotalCount));
			RelicTemplateLoader.GetTemplate(templatelist[i]).OnSettingTemplate();
			statlist.Add(RelicTemplateLoader.GetTemplate(templatelist[i]).StatCondition(this, player));
			valuelist.Add(RelicTemplateLoader.GetTemplate(templatelist[i]).ValueCondition(this, player, statlist[i]));
		}
	}
	public override void UpdateInventory(Player player) {
		var modplayer = player.GetModPlayer<PlayerStatsHandle>();
		if (templatelist == null) {
			templatelist = new List<int>();
			statlist = new List<PlayerStats>();
			valuelist = new List<StatModifier>();
		}
		if (templatelist.Count <= 0) {
			templatelist.Add(Main.rand.Next(RelicTemplateLoader.TotalCount));
			RelicTemplateLoader.GetTemplate(templatelist[0]).OnSettingTemplate();
			statlist.Add(RelicTemplateLoader.GetTemplate(templatelist[0]).StatCondition(this, player));
			valuelist.Add(RelicTemplateLoader.GetTemplate(templatelist[0]).ValueCondition(this, player, statlist[0]));
		}
		for (int i = 0; i < templatelist.Count; i++) {
			if (RelicTemplateLoader.GetTemplate(templatelist[i]) != null) {
				RelicTemplateLoader.GetTemplate(templatelist[i]).Effect(this, modplayer, player, valuelist[i], statlist[i]);
			}
			else {
				templatelist[i] = Main.rand.Next(RelicTemplateLoader.TotalCount);
				RelicTemplateLoader.GetTemplate(templatelist[i]).OnSettingTemplate();
				statlist[i] = RelicTemplateLoader.GetTemplate(templatelist[i]).StatCondition(this, player);
				valuelist[i] = RelicTemplateLoader.GetTemplate(templatelist[i]).ValueCondition(this, player, statlist[i]);
			}
		}
	}
	public void SetRelicData(List<int> type, List<PlayerStats> stat, List<StatModifier> value) {
		templatelist = type;
		for (int i = 0; i < templatelist.Count; i++) {
			RelicTemplateLoader.GetTemplate(templatelist[i]).OnSettingTemplate();
		}
		statlist = stat;
		valuelist = value;
	}
	public void GetRelicData(out List<int> templateType, out List<PlayerStats> stat, out List<StatModifier> value) {
		if (templatelist == null) {
			templateType = null;
			stat = null;
			value = null;
		}
		else {
			templateType = templatelist;
			for (int i = 0; i < templatelist.Count; i++) {
				RelicTemplateLoader.GetTemplate(templatelist[i]).OnSettingTemplate();
			}
			stat = statlist;
			value = valuelist;
		}
	}
	public int TemplateCount => templatelist.Count;
	public void MergeRelicData(int type, PlayerStats stat, StatModifier value) {
		templatelist.Add(type);
		RelicTemplateLoader.GetTemplate(type).OnSettingTemplate();
		statlist.Add(stat);
		valuelist.Add(value);
	}
	public override void SaveData(TagCompound tag) {
		if (templatelist == null) {
			return;
		}
		tag.Add("templatetypeList", templatelist);
		tag.Add("statList", statlist);
		tag.Add("modifierList", valuelist);
	}
	public override void LoadData(TagCompound tag) {
		statlist = tag.Get<List<PlayerStats>>("statList");
		templatelist = tag.Get<List<int>>("templatetypeList");
		valuelist = tag.Get<List<StatModifier>>("modifierList");
	}
}
public abstract class RelicTemplate : ModType {
	public static int GetRelicType<T>() where T : RelicTemplate {
		return ModContent.GetInstance<T>().Type;
	}
	public string Description => DisplayName + "\n - " + Language.GetTextValue($"Mods.BossRush.RelicTemplate.{Name}.Description");
	public string DisplayName => Language.GetTextValue($"Mods.BossRush.RelicTemplate.{Name}.DisplayName");
	public int Type { get; private set; }
	protected sealed override void Register() {
		SetStaticDefaults();
		Type = RelicTemplateLoader.Register(this);
	}
	public virtual void OnSettingTemplate() { }
	public virtual string ModifyToolTip(Relic relic, PlayerStats stat, StatModifier value) => "";
	public virtual StatModifier ValueCondition(Relic relic, Player player, PlayerStats stat) => new StatModifier();
	public virtual PlayerStats StatCondition(Relic relic, Player player) => PlayerStats.None;
	public virtual void Effect(Relic relic, PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {

	}
}
public static class RelicTemplateLoader {
	private static readonly List<RelicTemplate> _template = new();
	public static int TotalCount => _template.Count;
	public static int Register(RelicTemplate template) {
		ModTypeLookup<RelicTemplate>.Register(template);
		_template.Add(template);
		return _template.Count - 1;
	}
	public static RelicTemplate GetTemplate(int type) {
		return type >= 0 && type < _template.Count ? _template[type] : null;
	}
	/// <summary>
	/// This will merge stats of relicItem 1 and relicItem 2 together<br/>
	/// relicItem1 will remain while the relicItem2 will be turn to air
	/// </summary>
	/// <param name="relicItem1"></param>
	/// <param name="relicItem2"></param>
	/// <returns></returns>
	public static bool MergeStat(Relic relicItem1, Relic relicItem2) {
		relicItem1.GetRelicData(out List<int> type1, out List<PlayerStats> stat1, out List<StatModifier> value1);
		relicItem2.GetRelicData(out List<int> type2, out List<PlayerStats> stat2, out List<StatModifier> value2);
		if (type1 == null || type2 == null) {
			return false;
		}
		if (stat1 == null || stat2 == null) {
			return false;
		}
		if (value1 == null || value2 == null) {
			return false;
		}
		for (int i = 0; i < type2.Count; i++) {
			relicItem1.MergeRelicData(type2[i], stat2[i], value2[i]);
		}
		relicItem2.Item.TurnToAir();
		return true;
	}
	public static string RelicValueToPercentage(StatModifier value) => Math.Round((value.ApplyTo(1) - 1) * 100, 2).ToString() + "%";
	public static string RelicValueToNumber(StatModifier value) => Math.Round(value.ApplyTo(1) - 1, 2).ToString();
	public static string RelicValueToPercentage(float value) => Math.Round((value - 1) * 100).ToString() + "%";
	public static string RelicValueToNumber(float value) => Math.Round(value).ToString();

}
public class StatModifierSerializer : TagSerializer<StatModifier, TagCompound> {
	public override TagCompound Serialize(StatModifier value) => new TagCompound {
		["Base"] = MathF.Round(value.Base, 2),
		["Flat"] = MathF.Round(value.Flat, 2),
		["Additive"] = MathF.Round(value.Additive, 2),
		["Multiplicative"] = MathF.Round(value.Multiplicative, 2)
	};

	public override StatModifier Deserialize(TagCompound tag) =>
		new StatModifier(tag.Get<float>("Additive"), tag.Get<float>("Multiplicative"), tag.Get<float>("Flat"), tag.Get<float>("Base"));
}

public class PlayerStatsSerializer : TagSerializer<PlayerStats, TagCompound> {
	public override TagCompound Serialize(PlayerStats value) => new TagCompound {
		["PlayerStat"] = (byte)value
	};

	public override PlayerStats Deserialize(TagCompound tag) => (PlayerStats)tag.Get<byte>("PlayerStat");
}
