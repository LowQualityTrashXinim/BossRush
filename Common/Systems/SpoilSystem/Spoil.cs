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
