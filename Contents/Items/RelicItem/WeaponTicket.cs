using BossRush.Texture;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Contents.Items.RelicItem;
internal class WeaponTicket : ModItem {
	public override string Texture => BossRushTexture.EMPTYCARD;
	public override void Load() {
		if (info == null) {
			info = new();
		}
	}
	public override void Unload() {
		info = null;
	}
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32);
	}
	HashSet<int> RequestItem = new HashSet<int>();
	List<ColorInfo> info = new List<ColorInfo>();
	public string ItemIcon(int type) => $"[i:{type}]";
	public string PutIntoColorOffSetStuff(char c, int index) {
		if (index + 1 > info.Count) {
			info.Add(new(new() { Color.Pink, Color.Aqua, Color.Orange, Color.Lime }, 0));
			info[index].OffSet(index * 10);
		}
		return $"[c/{info[index].MultiColor(5).Hex3()}:{c}]";
	}
	public bool Add_Item(int itemID) {
		if (RequestItem.Contains(itemID)) {
			return false;
		}
		return RequestItem.Add(itemID);
	}
	public void Add_HashSet(HashSet<int> pool) {
		RequestItem.UnionWith(pool);
	}
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		if (RequestItem.Count > 0) {
			int index = tooltips.FindIndex(i => i.Name == "JourneyResearch");
			int counter = 0;
			TooltipLine line;
			string textlength = string.Empty;
			foreach (var item in RequestItem) {
				if (++counter >= 5) {
					textlength += "+ [" + ItemIcon(item) + "]\n";
					counter = 0;
				}
				else {
					textlength += "+ [" + ItemIcon(item) + "] ";
				}
			}
			if (RequestItem.Count % 2 == 1) {
				textlength = textlength.Substring(0, textlength.Length - 1);
			}
			line = new(Mod, "ticket", textlength);
			if (index >= 0) {
				tooltips.Insert(index, line);
			}
			else {
				tooltips.Add(line);
			}
		}
		int indexName = tooltips.FindIndex(i => i.Name == "ItemName");
		if (indexName != -1) {
			string itemName = tooltips[indexName].Text;
			string newitemNameEffect = string.Empty;
			int offset = 0;
			for (int i = 0; i < itemName.Length; i++) {
				if (itemName[i] == ' ') {
					offset--;
					newitemNameEffect += ' ';
					continue;
				}
				newitemNameEffect += PutIntoColorOffSetStuff(itemName[i], i + offset);
			}
			tooltips[indexName].Text = newitemNameEffect;
		}
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			if (RequestItem != null && RequestItem.Count > 0) {
				player.QuickSpawnItem(player.GetSource_OpenItem(Type), Main.rand.NextFromHashSet(RequestItem));
			}
		}
		return true;
	}
	public override void SaveData(TagCompound tag) {
		if (RequestItem.Count > 0) {
			tag.Add("Request_ItemAdd", RequestItem.ToArray());
		}
	}
	public override void LoadData(TagCompound tag) {
		if (tag.TryGet("Request_ItemAdd", out int[] arr)) {
			RequestItem = arr.ToHashSet();
		}
	}
}
