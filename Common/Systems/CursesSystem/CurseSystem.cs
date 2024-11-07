using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using System.Linq;

namespace BossRush.Common.Systems.CursesSystem;
internal class CursesLoader : ModSystem {
	private static Dictionary<string, ModCurse> _curses = new();
	private static Dictionary<string, ModCurse> _TabooCurse = new();
	private static Dictionary<string, ModCurse> _BlessingCurse = new();
	public static int Register(ModCurse curse) {
		ModTypeLookup<ModCurse>.Register(curse);
		_curses.Add(curse.Name, curse);
		if (curse.catagory.Contains(CursesCatagory.Taboo)) {
			_TabooCurse.Add(curse.Name, curse);
		}
		if (curse.catagory.Contains(CursesCatagory.Blessing)) {
			_BlessingCurse.Add(curse.Name, curse);
		}
		return _curses.Count - 1;
	}
	public static int CurseTabooCount => _TabooCurse.Keys.Count;
	public static int CurseBlessingCount => _BlessingCurse.Keys.Count;
	public override void Load() {
		_curses = new();
		_TabooCurse = new();
		_BlessingCurse = new();
	}
	public override void Unload() {
		_curses = null;
		_TabooCurse = null;
		_BlessingCurse = null;
	}
	public static ModCurse GetCurses(string name, CursesCatagory type = CursesCatagory.None) {
		if (type == CursesCatagory.Taboo) {
			return _TabooCurse.ContainsKey(name) ? _TabooCurse[name] : null;
		}
		if (type == CursesCatagory.Blessing) {
			return _BlessingCurse.ContainsKey(name) ? _BlessingCurse[name] : null;
		}
		return _curses.ContainsKey(name) ? _curses[name] : null;
	}
	public static int GetCurseValue(Player player, string name) => player.GetModPlayer<PlayerCursesHandle>().curses[GetCurses(name)];
	public static void AddCurses(Player player, string name, CursesCatagory catagory) {
		if (player.TryGetModPlayer(out PlayerCursesHandle curseplayer)) {
			ModCurse curse = GetCurses(name, catagory);
			if (!curseplayer.curses.ContainsKey(curse)) {
				curseplayer.curses.Add(curse, 1);
			}
			else {
				curseplayer.curses[curse]++;
			}
		}
	}
}
public enum CursesCatagory {
	None,
	Blessing,
	Taboo
}
public abstract class ModCurse : ModType {
	public List<CursesCatagory> catagory = new();
	public int Type = 0;
	public int Value = 0;
	public string DisplayName => $"- {Language.GetTextValue($"Mods.BossRush.Curse.{Name}.DisplayName")} -";
	public string Description => Language.GetTextValue($"Mods.BossRush.Curse.{Name}.Description");
	protected override void Register() {
		SetDefault();
		Type = CursesLoader.Register(this);
	}
	protected void AddCatagory(CursesCatagory cata) {
		if (!catagory.Contains(cata)) {
			return;
		}
		catagory.Add(cata);
	}
	protected int GetValue(Player player) => CursesLoader.GetCurseValue(player, Name);
	public virtual void SetDefault() { }
	public virtual string FinalDisplayName() => DisplayName;
	public virtual string FinalDescription() => Description;
	public virtual void Update(Player player) { }
	public virtual void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) { }
	public virtual void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) { }
	public virtual void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) { }
	public virtual void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) { }
	public override sealed string ToString() {
		return Name;
	}
	public override sealed bool Equals(object obj) {
		return this.Name == obj.ToString();
	}
}
public class PlayerCursesHandle : ModPlayer {
	public Dictionary<ModCurse, int> curses = new();
	public int TotalCursesValue => curses.Select(c => c.Key.Value * c.Value).Sum();
	public override void Initialize() {
		curses = new();
	}
	public override void UpdateEquips() {
		curses.Keys.ToList().ForEach(c => c.Update(Player));
	}
	public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (curses.Keys.Count < 1) {
			return;
		}
		foreach (ModCurse curse in curses.Keys) {
			curse.ModifyHitNPCWithItem(Player, item, target, ref modifiers);
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (curses.Keys.Count < 1) {
			return;
		}
		foreach (ModCurse curse in curses.Keys) {
			curse.ModifyHitNPCWithProj(Player, proj, target, ref modifiers);
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		curses.Keys.ToList().ForEach(c => c.OnHitByNPC(Player, npc, hurtInfo));
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		curses.Keys.ToList().ForEach(c => c.OnHitByProjectile(Player, proj, hurtInfo));
	}
	public override void SaveData(TagCompound tag) {
		tag["PlayerCurses"] = curses.Keys.Select(c => c.Name).ToList();
		tag["PlayerCursesValue"] = curses.Values.ToList();
	}
	public override void LoadData(TagCompound tag) {
		var cursesNameList = tag.Get<List<string>>("PlayerCurses");
		var cursesValueList = tag.Get<List<int>>("PlayerCursesValue");

		curses = cursesNameList.Zip(cursesValueList, (k, v) => new { Key = CursesLoader.GetCurses(k), Value = v }).ToDictionary(x => x.Key, x => x.Value);
	}
}
