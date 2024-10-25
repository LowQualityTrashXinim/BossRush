using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using System.Linq;

namespace BossRush.Common.Systems.CursesSystem;
internal class CursesLoader : ModSystem {
	private static Dictionary<string, ModCurse> _curses = new();
	public static int Register(ModCurse enchant) {
		ModTypeLookup<ModCurse>.Register(enchant);
		_curses.Add(enchant.Name, enchant);
		return _curses.Count - 1;
	}
	public override void Load() {
		_curses = new();
	}
	public override void Unload() {
		_curses = null;
	}
	public static ModCurse GetCurses(string name) => _curses.ContainsKey(name) ? _curses[name] : null;
}
public enum CursesCatagory {
	None,
	Blessing,
	Curse
}
public abstract class ModCurse : ModType {
	public List<CursesCatagory> catagory = new();
	public int Type = 0;
	public string DisplayName => $"- {Language.GetTextValue($"Mods.BossRush.Curse.{Name}.DisplayName")} -";
	public string Description => Language.GetTextValue($"Mods.BossRush.Curse.{Name}.Description");
	public virtual void SetStaticDefault() { }
	public virtual string FinalDisplayName() => DisplayName;
	public virtual string FinalDescription() => Description;
	protected override void Register() {
		Type = CursesLoader.Register(this);
		SetStaticDefaults();
	}
	public virtual void Update(Player player) { }
	public virtual void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) { }
	public virtual void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) { }
}
public class PlayerCursesHandle : ModPlayer {
	public List<ModCurse> curses = new();
	public override void Initialize() {
		curses = new();
	}
	public override void UpdateEquips() {
		curses.ForEach(c => c.Update(Player));
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		curses.ForEach(c => c.OnHitByNPC(Player, npc, hurtInfo));
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		curses.ForEach(c => c.OnHitByProjectile(Player, proj, hurtInfo));
	}
	public override void SaveData(TagCompound tag) {
		tag["PlayerCurses"] = curses.Select(c => c.Name);
	}
	public override void LoadData(TagCompound tag) {
		if (tag.TryGet("PlayerCurses", out List<string> cursesNameList)) {
			curses = cursesNameList.Select(CursesLoader.GetCurses).ToList();
		}
	}
}
