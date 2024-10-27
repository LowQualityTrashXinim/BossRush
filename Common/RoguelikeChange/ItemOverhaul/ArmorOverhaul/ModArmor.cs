using Terraria;
using System.Linq;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using BossRush.Common.Systems;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul;
public class ArmorLoader : ModSystem {
	private static Dictionary<string, ModArmor> _armor = new();
	public static Dictionary<int, int> HeadDef = new();
	public static Dictionary<int, int> BodyDef = new();
	public static Dictionary<int, int> LegDef = new();
	public static void Register(ModArmor armor) {
		ModTypeLookup<ModArmor>.Register(armor);
		_armor.Add(armor.Name, armor);
	}
	public override void Load() {
		_armor = new();
		HeadDef = new();
		BodyDef = new();
		LegDef = new();
	}
	public override void Unload() {
		_armor = null;
		HeadDef = null;
		BodyDef = null;
		LegDef = null;
	}

	public static int GetHeadDef(int head) {
		if (HeadDef.ContainsKey(head)) {
			return HeadDef[head];
		}
		return 0;
	}
	public static int GetBodyDef(int body) {
		if (BodyDef.ContainsKey(body)) {
			return BodyDef[body];
		}
		return 0;
	}
	public static int GetLegDef(int leg) {
		if (LegDef.ContainsKey(leg)) {
			return LegDef[leg];
		}
		return 0;
	}
	public static string ConvertIntoArmorSetFormat(int headID, int bodyID, int legID) => $"{headID}:{bodyID}:{legID}";
	public static ModArmor GetModArmor(int head, int body, int leg) {
		ModArmor armor = _armor.Values.ToList().Where(armor => armor.ToString() == ConvertIntoArmorSetFormat(head, body, leg)).FirstOrDefault();
		if (armor == null)
			return _armor.ContainsKey("None") ? _armor["None"] : null;
		return armor;
	}
	public static ModArmor GetModArmor(string name) => _armor.ContainsKey(name) ? _armor[name] : _armor.ContainsKey("None") ? _armor["None"] : null;
}
public class PlayerArmorHandle : ModPlayer {
	private ModArmor ActiveArmor = ArmorLoader.GetModArmor("");
	public ModArmor GetActiveArmorSet() => ActiveArmor;
	public override sealed void ResetEffects() {
		ActiveArmor = ArmorLoader.GetModArmor(Player.armor[0].type, Player.armor[1].type, Player.armor[2].type);
		if (ActiveArmor.modplayer == null)
			return;
		if (ActiveArmor.modplayer != null && ActiveArmor.modplayer is PlayerArmorHandle) {
			Armor_ResetEffects();
		}
	}
	public virtual void Armor_ResetEffects() { }
	public override sealed void UpdateEquips() {
		bool Head = Player.armor[0].type == ActiveArmor.HeadID;
		bool Body = Player.armor[1].type == ActiveArmor.BodyID;
		bool Leg = Player.armor[2].type == ActiveArmor.LegID;
		PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.Defense, Base: ArmorLoader.GetHeadDef(Player.armor[0].type));
		modplayer.AddStatsToPlayer(PlayerStats.Defense, Base: ArmorLoader.GetBodyDef(Player.armor[1].type));
		modplayer.AddStatsToPlayer(PlayerStats.Defense, Base: ArmorLoader.GetLegDef(Player.armor[2].type));
		if (ActiveArmor.modplayer != null && ActiveArmor.modplayer is PlayerArmorHandle) {
			if (Head && Body && Leg) {
				Armor_UpdateEquipsSet();
			}
		}
	}
	public virtual void Armor_UpdateEquipsSet() { }
	public override sealed void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (ActiveArmor.modplayer == null)
			return;
		if (ActiveArmor.modplayer is PlayerArmorHandle) {
			Armor_OnHitNPC(target, hit, damageDone);
		}
	}
	public virtual void Armor_OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) { }
	public override sealed void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (ActiveArmor.modplayer == null)
			return;
		if (ActiveArmor.modplayer is PlayerArmorHandle) {
			Armor_OnHitByProjectile(proj, hurtInfo);
		}
	}
	public virtual void Armor_OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) { }
	public override sealed void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (ActiveArmor.modplayer == null)
			return;
		if (ActiveArmor.modplayer is PlayerArmorHandle) {
			Armor_OnHitByNPC(npc, hurtInfo);
		}
	}
	public virtual void Armor_OnHitByNPC(NPC target, Player.HurtInfo hurtInfo) { }
	public override sealed void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (ActiveArmor.modplayer == null)
			return;
		if (ActiveArmor.modplayer is PlayerArmorHandle) {
			Armor_OnHitWithProj(proj, target, hit, damageDone);
		}
	}
	public virtual void Armor_OnHitWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) { }
	public override sealed void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (ActiveArmor.modplayer == null)
			return;
		if (ActiveArmor.modplayer is PlayerArmorHandle) {
			Armor_OnHitWithItem(item, target, hit, damageDone);
		}
	}
	public virtual void Armor_OnHitWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) { }
}
public abstract class ModArmor : ModType {
	protected int headID;
	protected int legID;
	protected int bodyID;

	public ModPlayer modplayer = null;
	public string SetBonusToolTip => Language.GetTextValue($"Mods.BossRush.Armor.{Name}.SetBonus");
	public string HeadToolTip => Language.GetTextValue($"Mods.BossRush.Armor.{Name}.Head");
	public string BodyToolTip => Language.GetTextValue($"Mods.BossRush.Armor.{Name}.Body");
	public string LegToolTip => Language.GetTextValue($"Mods.BossRush.Armor.{Name}.Leg");
	public bool OverrideOriginalToolTip = false;
	public int HeadID { get => headID; }
	public int BodyID { get => bodyID; }
	public int LegID { get => legID; }
	public override string ToString() => $"{headID}:{bodyID}:{legID}";
	public bool ContainAnyOfArmorPiece(int type) => type == headID || type == bodyID || type == legID;
	protected override void Register() {
		ArmorLoader.Register(this);
		SetDefault();
	}
	public virtual void SetDefault() { }
}

public class None : ModArmor {
}
