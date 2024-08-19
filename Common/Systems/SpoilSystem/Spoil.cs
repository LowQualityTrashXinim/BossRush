using System;
using System.Linq;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using BossRush.Contents.Perks;

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
		if (Main.LocalPlayer.GetModPlayer<PerkPlayer>().HasPerk<BlessingOfPerk>()) {
			chance *= 1.5f;
		}
		return Main.rand.NextFloat() <= chance;
	}
	public static bool UncommonDrop() => ChanceWrapper(.44f);
	public static bool RareDrop() => ChanceWrapper(.15f);
	public static bool SuperRareDrop() => ChanceWrapper(.05f);
	public static bool SSRDrop() => ChanceWrapper(.01f);
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
