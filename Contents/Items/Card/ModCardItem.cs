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

namespace BossRush.Contents.Items.Card;
public class Relic : ModItem {
	public override string Texture => BossRushTexture.ACCESSORIESSLOT;
	//List<CardTemplate> templatelist = new List<CardTemplate>();
	//List<StatModifier> value = new List<StatModifier>();
	//List<PlayerStats> stat = new List<PlayerStats>();
	CardTemplate template;
	StatModifier modifier;
	PlayerStats stat;
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.rare = ItemRarityID.Gray;
		Item.value = Item.buyPrice(silver: 50);
	}
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		if (template == null) {
			return;
		}
		TooltipLine line = tooltips.FirstOrDefault(l => l.Name == "Tooltip0");
		if (line != null) {
			line.Text = template.ModifyToolTip(stat, modifier);
		}
	}
	public override void UpdateInventory(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		//I was going to do something that allow mutation but for now, it is not possible
		//if (templatelist.Count < 0) {
		//	templatelist.Add(CardTemplateLoader.GetTemplate(Main.rand.Next(CardTemplateLoader.TotalCount)));
		//}
		//for (int i = 0; i < templatelist.Count; i++) {
		//	templatelist[i].Effect(modplayer, player, value[i], stat[i]);
		//}
		if (template == null) {
			template = CardTemplateLoader.GetTemplate(Main.rand.Next(CardTemplateLoader.TotalCount));
			stat = template.StatCondition(player);
			modifier = template.ValueCondition(player, stat);
		}
		template.Effect(modplayer, player, modifier, stat);
	}
	public override void SaveData(TagCompound tag) {
		if (template == null) {
			return;
		}
		tag.Add("stat", (byte)stat);
		tag.Add("templateType", template.Type);
		tag.Add("modifier", modifier);
	}
	public override void LoadData(TagCompound tag) {
		byte bytestat = tag.Get<byte>("stat");
		if (Enum.IsDefined(typeof(PlayerStats), bytestat)) {
			stat = (PlayerStats)bytestat;
		}
		template = CardTemplateLoader.GetTemplate(tag.Get<int>("templateType"));
		modifier = tag.Get<StatModifier>("modifier");
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
