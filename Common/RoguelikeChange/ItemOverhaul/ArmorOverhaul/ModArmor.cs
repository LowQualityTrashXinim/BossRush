using Terraria;
using System.Linq;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using BossRush.Common.Systems;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul;
public class ArmorLoader : ModSystem {
	private static Dictionary<string, ModArmorSet> _armor = new();
	private static Dictionary<int, ModArmorPiece> _armorpiece = new();
	public static void Register(ModArmorSet armor) {
		ModTypeLookup<ModArmorSet>.Register(armor);
		_armor.Add(armor.Name, armor);
	}
	public static void Register(ModArmorPiece armor) {
		ModTypeLookup<ModArmorPiece>.Register(armor);
		_armorpiece.Add(armor._pieceID, armor);
	}
	public override void Load() {
		_armor = new();
		_armorpiece = new();
	}
	public override void Unload() {
		_armor = null;
		_armorpiece = null;
	}
	/// <summary>
	/// Remember to check null
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public static ModArmorPiece GetArmorPieceInfo(int type) {
		if (_armorpiece.ContainsKey(type)) {
			return _armorpiece[type];
		}
		return null;
	}
	public static string ConvertIntoArmorSetFormat(int headID, int bodyID, int legID) => $"{headID}:{bodyID}:{legID}";
	public static ModArmorSet GetModArmor(int head, int body, int leg) {
		ModArmorSet armor = _armor.Values.ToList().Where(armor => armor.ToString() == ConvertIntoArmorSetFormat(head, body, leg)).FirstOrDefault();
		if (armor == null)
			return _armor.ContainsKey("None") ? _armor["None"] : null;
		return armor;
	}
	public static ModArmorSet GetModArmor(string name) => _armor.ContainsKey(name) ? _armor[name] : _armor.ContainsKey("None") ? _armor["None"] : null;
	public static void SetModPlayer(string name, ModPlayer modplayer) => GetModArmor(name).modplayer = modplayer;
}
public class PlayerArmorHandle : ModPlayer {
	private ModArmorSet ActiveArmor = ArmorLoader.GetModArmor("");
	public ModArmorSet GetActiveArmorSet() => ActiveArmor;
	public override sealed void ResetEffects() {
		ActiveArmor = ArmorLoader.GetModArmor(Player.armor[0].type, Player.armor[1].type, Player.armor[2].type);
		if (ActiveArmor.modplayer != null && ActiveArmor.modplayer is PlayerArmorHandle) {
			Armor_ResetEffects();
		}
	}
	public virtual void Armor_ResetEffects() { }
	public override sealed void UpdateEquips() {
		int headtype = Player.armor[0].type, bodytype = Player.armor[1].type, legstype = Player.armor[2].type;
		bool Head = headtype == ActiveArmor.HeadID;
		bool Body = bodytype == ActiveArmor.BodyID;
		bool Leg = legstype == ActiveArmor.LegID;

		if (ActiveArmor.modplayer != null && ActiveArmor.modplayer is PlayerArmorHandle) {
			if (Head && Body && Leg) {
				Armor_UpdateEquipsSet();
			}
		}
	}
	public virtual void Armor_UpdateEquipsSet() { }

	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (ActiveArmor.modplayer != null && ActiveArmor.modplayer is PlayerArmorHandle) {
			return Armor_Shoot(item, source, position, velocity, type, damage, knockback);
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
	public virtual bool Armor_Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) => true;

	public override sealed void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (ActiveArmor.modplayer != null && ActiveArmor.modplayer is PlayerArmorHandle) {
			Armor_OnHitNPC(target, hit, damageDone);
		}
	}
	public virtual void Armor_OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) { }
	public override sealed void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (ActiveArmor.modplayer != null && ActiveArmor.modplayer is PlayerArmorHandle) {
			Armor_OnHitByProjectile(proj, hurtInfo);
		}
	}
	public virtual void Armor_OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) { }
	public override sealed void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (ActiveArmor.modplayer != null && ActiveArmor.modplayer is PlayerArmorHandle) {
			Armor_OnHitByNPC(npc, hurtInfo);
		}
	}
	public virtual void Armor_OnHitByNPC(NPC target, Player.HurtInfo hurtInfo) { }
	public override sealed void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (ActiveArmor.modplayer != null && ActiveArmor.modplayer is PlayerArmorHandle) {
			Armor_OnHitWithProj(proj, target, hit, damageDone);
		}
	}
	public virtual void Armor_OnHitWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) { }
	public override sealed void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (ActiveArmor.modplayer != null && ActiveArmor.modplayer is PlayerArmorHandle) {
			Armor_OnHitWithItem(item, target, hit, damageDone);
		}
	}
	public virtual void Armor_OnHitWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) { }
}
public abstract class ModArmorPiece : ModType {
	public virtual int _pieceID => ItemID.None;
	public virtual int Add_Defense => 0;
	public virtual bool AddTooltip => false;
	public virtual void UpdateEquip(Player player, Item item) { }
	public string ToolTip => Language.GetTextValue($"Mods.BossRush.Armor.{Name}");
	protected override void Register() {
		ArmorLoader.Register(this);
	}
}
public abstract class ModArmorSet : ModType {
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

public class None : ModArmorSet {
}
