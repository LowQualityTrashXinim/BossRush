using System;
using Terraria;
using Terraria.ID;
using System.Linq;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using BossRush.Common.Systems;
using System.Collections.Generic;
using static BossRush.Contents.Items.Weapon.SummonerSynergyWeapon.MothWeapon.MothProj;

namespace BossRush.Contents.Items.RelicItem;
public class Relic : ModItem {
	public override string Texture => BossRushTexture.ACCESSORIESSLOT;
	List<int> templatelist = new List<int>();
	List<PlayerStats> statlist = new List<PlayerStats>();
	List<StatModifier> valuelist = new List<StatModifier>();
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.rare = ItemRarityID.Gray;
		Item.value = Item.buyPrice(silver: 50);
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
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		var line = tooltips.FirstOrDefault(l => l.Name == "Tooltip0");
		if (templatelist == null || line == null) {
			tooltips.Add(new TooltipLine(Mod, "", "Something gone wrong"));
			return;
		}
		line.Text = "";
		for (int i = 0; i < templatelist.Count; i++) {
			line.Text += CardTemplateLoader.GetTemplate(templatelist[i]).ModifyToolTip(statlist[i], valuelist[i]);
			//if (Main.LocalPlayer.IsDebugPlayer()) {
			//	line.Text +=
			//		$"\nTemplate Name : {CardTemplateLoader.GetTemplate(templatelist[i]).FullName}" +
			//		$"\nTemplate Desc : {CardTemplateLoader.GetTemplate(templatelist[i]).Description}" +
			//		$"\nTemplate ID : {templatelist[i]}" +
			//		$"\nStat to be increased : {Enum.GetName(typeof(PlayerStats), statlist[i])}" +
			//		$"\nIncreases value : Additive[{valuelist[i].Additive}] Multiplicative[{valuelist[i].Multiplicative}] Base[{valuelist[i].Base}] Flat[{valuelist[i].Flat}]";
			//}
			if (i + 1 != templatelist.Count) {
				line.Text += "\n";
			}
		}
	}
	public override void UpdateInventory(Player player) {
		var modplayer = player.GetModPlayer<PlayerStatsHandle>();
		if (templatelist.Count <= 0) {
			templatelist.Add(Main.rand.Next(CardTemplateLoader.TotalCount));
			statlist.Add(CardTemplateLoader.GetTemplate(templatelist[0]).StatCondition(player));
			valuelist.Add(CardTemplateLoader.GetTemplate(templatelist[0]).ValueCondition(player, statlist[0]));
		}
		for (int i = 0; i < templatelist.Count; i++) {
			CardTemplateLoader.GetTemplate(templatelist[i]).Effect(modplayer, player, valuelist[i], statlist[i]);
		}
	}
	public void SetRelicData(List<int> type, List<PlayerStats> stat, List<StatModifier> value) {
		templatelist = type;
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
			stat = statlist;
			value = valuelist;
		}
	}
	public int TemplateCount => templatelist.Count;
	public void MergeRelicData(int type, PlayerStats stat, StatModifier value) {
		templatelist.Add(type);
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
public abstract class CardTemplate : ModType {
	public string Description => Language.GetTextValue($"Mods.BossRush.CardTemplate.{Name}.Description");
	public int Type { get; private set; }
	protected sealed override void Register() {
		Type = CardTemplateLoader.Register(this);
	}
	public virtual string ModifyToolTip(PlayerStats stat, StatModifier value) => "";
	public virtual StatModifier ValueCondition(Player player, PlayerStats stat) => new StatModifier();
	public virtual PlayerStats StatCondition(Player player) => PlayerStats.None;
	public virtual void Effect(PlayerStatsHandle modplayer, Player player, StatModifier value, PlayerStats stat) {

	}
}
public static class CardTemplateLoader {
	private static readonly List<CardTemplate> _template = new();
	public static int TotalCount => _template.Count;
	public static int Register(CardTemplate template) {
		ModTypeLookup<CardTemplate>.Register(template);
		_template.Add(template);
		return _template.Count - 1;
	}
	public static CardTemplate GetTemplate(int type) {
		return type >= 0 && type < _template.Count ? _template[type] : null;
	}
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
}
public class StatModifierSerializer : TagSerializer<StatModifier, TagCompound> {
	public override TagCompound Serialize(StatModifier value) => new TagCompound {
		["Base"] = value.Base,
		["Flat"] = value.Flat,
		["Additive"] = value.Additive,
		["Multiplicative"] = value.Multiplicative
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
