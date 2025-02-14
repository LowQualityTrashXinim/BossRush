using BossRush.Contents.Items.Chest;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BossRush.Contents.Items.RelicItem;
internal class WeaponTicket : ModItem {
	public override string Texture => BossRushTexture.EMPTYCARD;
	public override void Load() {
		info = new();
	}
	public override void Unload() {
		info = null;
	}
	public override void SetDefaults() {
		Item.width = Item.height = 32;
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
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		if (RequestItem.Count > 0) {
			int index = tooltips.FindIndex(i => i.Name == "JourneyResearch");
			int counter = 0;
			if (index >= 0) {
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
				tooltips.Insert(index, new(Mod, "ticket", textlength));
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
	public override void UpdateInventory(Player player) {
		if (RequestItem.Count < 1) {
			int randomAmount = Main.rand.Next(10, 30);
			for (int i = 0; i < randomAmount; i++) {
				if (!RequestItem.Add(Main.rand.NextFromHashSet(BossRushModSystem.List_Weapon).type)) {
					i--;
				}
			}
			return;
		}
		ChestLootDropPlayer chestplayer = player.GetModPlayer<ChestLootDropPlayer>();
		for (int i = 0; i < RequestItem.Count; i++) {
			Item item = ContentSamples.ItemsByType[RequestItem.ElementAt(i)];
			if (item.DamageType == DamageClass.Melee) {
				chestplayer.Request_AddMelee.Add(item.type);
			}
			else if (item.DamageType == DamageClass.Ranged) {
				chestplayer.Request_AddRange.Add(item.type);
			}
			else if (item.DamageType == DamageClass.Magic) {
				chestplayer.Request_AddMagic.Add(item.type);
			}
			else if (item.DamageType == DamageClass.Summon) {
				chestplayer.Request_AddSummon.Add(item.type);
			}
			else {
				chestplayer.Request_AddMisc.Add(item.type);
			}
		}
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
