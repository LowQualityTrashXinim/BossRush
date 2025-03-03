using System;
using Terraria;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using BossRush.Contents.Perks;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader.IO;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.UI;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Contents.Items.Chest;

namespace BossRush.Common.Systems.SpoilSystem;
public class ModSpoilSystem : ModSystem {
	private static Dictionary<string, ModSpoil> _spoils = new();
	public static int TotalCount => _spoils.Count;
	public override void Load() {
		foreach (var type in Mod.Code.GetTypes().Where(type => !type.IsAbstract && type.IsAssignableTo(typeof(ModSpoil)))) {
			var spoil = (ModSpoil)Activator.CreateInstance(type);
			spoil.SetStaticDefault();
			_spoils.Add(spoil.Name, spoil);
		}
	}
	public static List<ModSpoil> GetSpoilsList() => new(_spoils.Values);

	public override void Unload() {
		_spoils = null;
	}
	public static ModSpoil GetSpoils(string name) {
		return _spoils.ContainsKey(name) ? _spoils[name] : null;
	}
}
public static class SpoilDropRarity {
	public readonly static int Common = ItemRarityID.White;
	public readonly static int Uncommon = ItemRarityID.Blue;
	public readonly static int Rare = ItemRarityID.Yellow;
	public readonly static int SuperRare = ItemRarityID.Purple;
	public readonly static int SSR = ItemRarityID.Red;
	public static bool ChanceWrapper(float chance) {
		if (!UniversalSystem.LuckDepartment(UniversalSystem.CHECK_RARESPOILS) || !UniversalSystem.CheckLegacy(UniversalSystem.LEGACY_SPOIL) && !Main.LocalPlayer.IsDebugPlayer()) {
			return false;
		}
		if (Main.LocalPlayer.IsDebugPlayer()) {
			return true;
		}
		if (Main.LocalPlayer.GetModPlayer<PerkPlayer>().HasPerk<BlessingOfPerk>()) {
			chance *= 1.5f;
		}
		return Main.rand.NextFloat() <= chance;
	}
	public static bool UncommonDrop() => ChanceWrapper(.44f);
	public static bool RareDrop() => ChanceWrapper(.10f);
	public static bool SuperRareDrop() => ChanceWrapper(.025f);
	public static bool SSRDrop() => ChanceWrapper(.001f);
}
public abstract class ModSpoil {
	public string Name => GetType().Name;
	public int RareValue = 0;
	public string DisplayName => $"- {Language.GetTextValue($"Mods.BossRush.Spoils.{Name}.DisplayName")} -";
	public string Description => Language.GetTextValue($"Mods.BossRush.Spoils.{Name}.Description");
	public virtual void SetStaticDefault() { }
	public virtual string FinalDisplayName() => DisplayName;
	public virtual string FinalDescription() => Description;
	public virtual bool IsSelectable(Player player, Item itemsource) {
		return true;
	}
	public virtual void OnChoose(Player player, int itemsource) { }
	public sealed override string ToString() {
		return base.ToString();
	}
}
public class SpoilsPlayer : ModPlayer {
	public List<int> LootBoxSpoilThatIsNotOpen = new List<int>();
	public List<string> SpoilsGift = new();
	public override void Initialize() {
		LootBoxSpoilThatIsNotOpen = new();
		SpoilsGift = new();
	}
	public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
		ModPacket packet = Mod.GetPacket();
		packet.Write((byte)BossRush.MessageType.Perk);
		packet.Write((byte)Player.whoAmI);
		packet.Write(LootBoxSpoilThatIsNotOpen.Count);
		foreach (int item in LootBoxSpoilThatIsNotOpen) {
			packet.Write(LootBoxSpoilThatIsNotOpen[item]);
		}
		packet.Send(toWho, fromWho);
	}
	public void ReceivePlayerSync(BinaryReader reader) {
		LootBoxSpoilThatIsNotOpen.Clear();
		int count = reader.ReadInt32();
		for (int i = 0; i < count; i++)
			LootBoxSpoilThatIsNotOpen.Add(reader.ReadInt32());
	}

	public override void CopyClientState(ModPlayer targetCopy) {
		SpoilsPlayer clone = (SpoilsPlayer)targetCopy;
		clone.LootBoxSpoilThatIsNotOpen = LootBoxSpoilThatIsNotOpen;
	}

	public override void SendClientChanges(ModPlayer clientPlayer) {
		SpoilsPlayer clone = (SpoilsPlayer)clientPlayer;
		if (LootBoxSpoilThatIsNotOpen != clone.LootBoxSpoilThatIsNotOpen) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
	}
	public override void SaveData(TagCompound tag) {
		tag["LootBoxSpoilThatIsNotOpen"] = LootBoxSpoilThatIsNotOpen;
	}
	public override void LoadData(TagCompound tag) {
		LootBoxSpoilThatIsNotOpen = tag.Get<List<int>>("LootBoxSpoilThatIsNotOpen");
	}
}
public class SpoilsUIState : UIState {
	public int Limit_Spoils = 5;
	public List<SpoilsUIButton> btn_List;
	public int lootboxItem = -1;
	public UITextPanel<string> panel;
	public override void OnInitialize() {
		panel = new UITextPanel<string>(Language.GetTextValue($"Mods.BossRush.SystemTooltip.Spoil.Header"));
		panel.HAlign = .5f;
		panel.VAlign = .3f;
		panel.UISetWidthHeight(150, 53);
		Append(panel);
		Limit_Spoils = 5;
		btn_List = new List<SpoilsUIButton>();
	}
	public override void OnActivate() {
		btn_List.Clear();
		SpoilsPlayer modplayer = Main.LocalPlayer.GetModPlayer<SpoilsPlayer>();
		lootboxItem = modplayer.LootBoxSpoilThatIsNotOpen.FirstOrDefault();
		if (lootboxItem <= 0) {
			return;
		}
		Player player = Main.LocalPlayer;
		List<ModSpoil> SpoilList = ModSpoilSystem.GetSpoilsList();
		if (modplayer.SpoilsGift.Count > Limit_Spoils - 1 && modplayer.LootBoxSpoilThatIsNotOpen.Count > 0) {
			SpoilList.Clear();
			SpoilList = modplayer.SpoilsGift.Select(ModSpoilSystem.GetSpoils).ToList();
			modplayer.SpoilsGift.Clear();
		}
		else {
			modplayer.SpoilsGift.Clear();
			for (int i = SpoilList.Count - 1; i >= 0; i--) {
				ModSpoil spoil = SpoilList[i];
				if (!spoil.IsSelectable(player, ContentSamples.ItemsByType[lootboxItem])) {
					SpoilList.Remove(spoil);
				}
			}
		}
		if (SpoilList.Count < 1) {
			SpoilList = ModSpoilSystem.GetSpoilsList();
		}
		//prioritize rarer spoil
		int spoilPriortize = 1;
		for (int i = 0; i < Limit_Spoils; i++) {
			ModSpoil spoil = Main.rand.Next(SpoilList);
			if (spoilPriortize > 0) {
				spoilPriortize--;
				foreach (var item in SpoilList) {
					if (item.RareValue > SpoilDropRarity.Rare) {
						spoil = item;
					}
				}
			}
			float Hvalue = MathHelper.Lerp(.3f, .7f, i / (float)(Limit_Spoils - 1));
			SpoilsUIButton btn = new SpoilsUIButton(TextureAssets.InventoryBack, spoil);
			modplayer.SpoilsGift.Add(spoil.Name);
			SpoilList.Remove(spoil);
			btn.HAlign = Hvalue;
			btn.VAlign = .4f;
			btn_List.Add(btn);
			Append(btn);
		}
		//SpoilsUIButton btna = new SpoilsUIButton(TextureAssets.InventoryBack10, null);
		//btna.HAlign = .7f;
		//btna.VAlign = .4f;
		//btn_List.Add(btna);
		//Append(btna);
	}
}
public class SpoilsUIButton : UIImageButton {
	public ModSpoil spoil;
	int LootboxItem = 0;
	public SpoilsUIButton(Asset<Texture2D> texture, ModSpoil Spoil) : base(texture) {
		spoil = Spoil;
		if (Main.LocalPlayer.TryGetModPlayer(out SpoilsPlayer spoilplayer)) {
			if (spoilplayer.LootBoxSpoilThatIsNotOpen.Count > 0)
				LootboxItem = spoilplayer.LootBoxSpoilThatIsNotOpen.First();
		}
	}
	public override void LeftClick(UIMouseEvent evt) {
		Player player = Main.LocalPlayer;
		SpoilsPlayer modplayer = player.GetModPlayer<SpoilsPlayer>();
		if (modplayer.LootBoxSpoilThatIsNotOpen.Count > 0)
			LootboxItem = modplayer.LootBoxSpoilThatIsNotOpen.First();
		if (spoil == null || LootboxItem == 0) {
			List<ModSpoil> SpoilList = ModSpoilSystem.GetSpoilsList();
			for (int i = SpoilList.Count - 1; i >= 0; i--) {
				ModSpoil spoil = SpoilList[i];
				if (!spoil.IsSelectable(player, ContentSamples.ItemsByType[LootboxItem])) {
					SpoilList.Remove(spoil);
				}
			}
			Main.rand.Next(SpoilList).OnChoose(player, LootboxItem);
			modplayer.LootBoxSpoilThatIsNotOpen.RemoveAt(0);
			modplayer.SpoilsGift.Clear();
			ModContent.GetInstance<UniversalSystem>().DeactivateUI();
			return;
		}
		spoil.OnChoose(player, LootboxItem);
		modplayer.LootBoxSpoilThatIsNotOpen.RemoveAt(0);
		modplayer.SpoilsGift.Clear();
		ModContent.GetInstance<UniversalSystem>().DeactivateUI();
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (IsMouseHovering) {
			if (LootboxSystem.GetItemPool(LootboxItem) == null && !Main.LocalPlayer.IsDebugPlayer()) {
				return;
			}
			if (spoil == null) {
				Main.instance.MouseText(Language.GetTextValue($"Mods.BossRush.SystemTooltip.Spoil.Randomize"));
			}
			else {
				Main.instance.MouseText(spoil.FinalDisplayName(), spoil.FinalDescription(), spoil.RareValue);
			}
		}
		else {
			if (!Parent.Children.Where(e => e.IsMouseHovering).Any()) {
				Main.instance.MouseText("");
			}
		}
	}
}
