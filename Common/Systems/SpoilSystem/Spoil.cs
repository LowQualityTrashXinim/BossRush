using System;
using System.Linq;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Terraria;

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
