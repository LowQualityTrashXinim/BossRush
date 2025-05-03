using Terraria;
using Terraria.ID;
using System.Linq;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

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
		_armorpiece.Add(armor.PieceID, armor);
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
	public static ModArmorSet Default => _armor["None"];
	public static ModArmorSet GetModArmor(string name) => _armor.ContainsKey(name) ? _armor[name] : _armor.ContainsKey("None") ? _armor["None"] : null;
	/// <summary>
	/// The <paramref name="name"/> param use the same as name of the <see cref="ModArmorSet"/> class name<br/>
	/// Raw writing it is recommended
	/// </summary>
	/// <param name="name"></param>
	/// <param name="modplayer"></param>
	public static void SetModPlayer(string name, ModPlayer modplayer) => GetModArmor(name).modplayer = modplayer;
}
public abstract class PlayerArmorHandle : ModPlayer {
	private RoguelikeArmorPlayer modplayer => Player.GetModPlayer<RoguelikeArmorPlayer>();
	public override sealed void PreUpdateMovement() {
		if (modplayer.ArmorSetCheck(this)) {
			Armor_PreUpdateMovement();
		}
	}
	public virtual void Armor_PreUpdateMovement() { }
	public override sealed void ResetEffects() {
		if (modplayer.ArmorSetCheck(this)) {
			Armor_ResetEffects();
		}
	}
	public virtual void Armor_ResetEffects() { }
	public override sealed void UpdateEquips() {
		if (modplayer.ArmorSetCheck(this)) {
			Armor_UpdateEquipsSet();
		}
	}
	public virtual void Armor_UpdateEquipsSet() { }
	public override sealed void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (modplayer.ArmorSetCheck(this)) {
			Armor_ModifyShootStats(item, ref position, ref velocity, ref type, ref damage, ref knockback);
		}
	}
	public virtual void Armor_ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) { }
	public override sealed bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (modplayer.ArmorSetCheck(this)) {
			return Armor_Shoot(item, source, position, velocity, type, damage, knockback);
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
	public virtual bool Armor_Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) => true;

	public override sealed void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (modplayer.ArmorSetCheck(this)) {
			Armor_OnHitNPC(target, hit, damageDone);
		}
	}
	public override sealed void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (modplayer.ArmorSetCheck(this)) {
			Armor_ModifyHitByNPC(npc, ref modifiers);
		}
	}
	public virtual void Armor_ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) { }
	public override sealed void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (modplayer.ArmorSetCheck(this)) {
			Armor_ModifyHitByProjectile(proj, ref modifiers);
		}
	}
	public virtual void Armor_ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) { }
	public virtual void Armor_OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) { }
	public override sealed void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (modplayer.ArmorSetCheck(this)) {
			Armor_OnHitByProjectile(proj, hurtInfo);
		}
	}
	public virtual void Armor_OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) { }
	public override sealed void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (modplayer.ArmorSetCheck(this)) {
			Armor_OnHitByNPC(npc, hurtInfo);
		}
	}
	public virtual void Armor_OnHitByNPC(NPC target, Player.HurtInfo hurtInfo) { }
	public override sealed void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (modplayer.ArmorSetCheck(this)) {
			Armor_OnHitWithProj(proj, target, hit, damageDone);
		}
	}
	public virtual void Armor_OnHitWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) { }
	public override sealed void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (modplayer.ArmorSetCheck(this)) {
			Armor_OnHitWithItem(item, target, hit, damageDone);
		}
	}
	public virtual void Armor_OnHitWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) { }
	public override sealed void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (modplayer.ArmorSetCheck(this)) {
			Armor_ModifyHitNPC(target, ref modifiers);
		}
	}
	public virtual void Armor_ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) { }
	public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (modplayer.ArmorSetCheck(this)) {
			Armor_ModifyHitNPCWithItem(item, target, ref modifiers);
		}
	}
	public virtual void Armor_ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) { }
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (modplayer.ArmorSetCheck(this)) {
			Armor_ModifyHitNPCWithProj(proj, target, ref modifiers);
		}
	}
	public virtual void Armor_ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) { }

	public override sealed void NaturalLifeRegen(ref float regen) {
		if (modplayer.ArmorSetCheck(this)) {
			Armor_NaturalLifeRegen(ref regen);
		}
	}
	public virtual void Armor_NaturalLifeRegen(ref float regen) { }
}
public abstract class ModArmorPiece : ModType {
	public const string Type_Head = "Head";
	public const string Type_Body = "Body";
	public const string Type_Leg = "Leg";
	public int PieceID = ItemID.None;
	
	/// <summary>
	/// Uses to add defenses to armor piece, do note that you can also substract the defenese
	/// </summary>
	public int Add_Defense = 0;
	/// <summary>
	/// <b>False</b> to avoid tooltip of this to be added<br/>
	/// <b>True</b> to add localization of this armor piece into armor tooltip
	/// </summary>
	public bool AddTooltip = false;
	/// <summary>
	/// Allow you to completely override original tooltip
	/// </summary>
	public bool OverrideTooltip = false;
	/// <summary>
	/// Make a attempt to delete vanity tooltip if has any
	/// </summary>
	public bool DeleteVanityTooltip = false;
	/// <summary>
	/// Uses this to add always update effect such as damage increases
	/// </summary>
	/// <param name="player">the player</param>
	/// <param name="item">the equipped item</param>
	public virtual void UpdateEquip(Player player, Item item) { }
	public string ToolTip => Language.GetTextValue($"Mods.BossRush.Armor.{ArmorName}.{TypeEquipment}");
	protected override void Register() {
		SetDefault();
		ArmorLoader.Register(this);
	}
	/// <summary>
	/// You will need to set this in <see cref="SetDefault"/> for the localization to work properly <br/>
	/// Use <see cref="Type_Body"/> or any that you see fit
	/// </summary>
	public string TypeEquipment = "";
	/// <summary>
	/// This is require if you want localization to work properly<br/>
	/// What name you set for <see cref="ModArmorSet"/> should be set here<br/>
	/// Set this to the exact same string as <see cref="ArmorLoader.SetModPlayer(string, ModPlayer)"/>
	/// </summary>
	public string ArmorName = "";
	public virtual void SetDefault() { }
	public override sealed void SetStaticDefaults() {
		base.SetStaticDefaults();
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
	protected sealed override void Register() {
		SetDefault();
		ArmorLoader.Register(this);
	}
	public virtual void SetDefault() { }
	public override sealed void SetStaticDefaults() {
	}
	public override sealed bool Equals(object obj) {
		return obj.ToString() == this.ToString();
	}

	public override int GetHashCode() {
		return 0;
	}
}
public class None : ModArmorSet {
	public override void SetDefault() {
		modplayer = null;
	}
}
