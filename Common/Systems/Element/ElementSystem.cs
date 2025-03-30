using BossRush.Common.RoguelikeChange;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.Systems.Element;
internal class ElementSystem : ModSystem {
	public static List<Element> list_Element { get; private set; } = new();
	public static Dictionary<string, Element> dict_Element { get; private set; } = new();
	public static int TotalCount => list_Element.Count;
	public static ushort Register(Element element) {
		ModTypeLookup<Element>.Register(element);
		list_Element.Add(element);
		dict_Element.Add(element.Name, element);
		return (ushort)list_Element.Count;
	}
	public static Element GetElement(int ElemID) => ElemID <= 0 && ElemID >= TotalCount ? null : list_Element[ElemID - 1];
	public static Element GetElement(string ElementName) => dict_Element.ContainsKey(ElementName) ? dict_Element[ElementName] : null;
	public override void Load() {
		list_Element = new();
		dict_Element = new();
		On_Player.StrikeNPCDirect += On_Player_StrikeNPCDirect;
	}

	private void On_Player_StrikeNPCDirect(On_Player.orig_StrikeNPCDirect orig, Player self, NPC npc, NPC.HitInfo hit) {
		orig(self, npc, hit);
		////This is for the very late reaction
		//if (self.TryGetModPlayer(out Element_ModPlayer modplayer)) {
		//	foreach (var item in modplayer.ForcingElement) {
		//		item.OnHitNPC(npc, hit, modplayer.NPC_Element.Select(i => i.Type).ToHashSet());
		//	}
		//	//Cleaning the hashset
		//	modplayer.NPC_Element.Clear();
		//	modplayer.Item_Element.Clear();
		//}
	}
	//public static Dictionary<int, HashSet<ushort>> dict_NPC_Element = new();
	//public static Dictionary<int, HashSet<ushort>> dict_Item_Element = new();
	//public static Dictionary<int, HashSet<ushort>> dict_Armor_Element = new();
	public override void Unload() {
		list_Element = null;
		dict_Element = null;
		//dict_NPC_Element = null;
		//dict_Item_Element = null;
		//dict_Armor_Element = null;
	}
}
public class Element_Item : GlobalItem {
	public override bool InstancePerEntity => true;
	public Dictionary<ushort, float> dict_Element = new();
	public void SetElementRes(ushort type, float res) {
		dict_Element.Add(type, MathF.Round(res, 2));
	}
	public override void SetDefaults(Item entity) {
		foreach (Element el in ElementSystem.list_Element) {
			el.Item_SetDefault(entity, this);
		}
	}
	public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers) {
		float mulitplier = 1;
		if (dict_Element.Count == 1) {
			mulitplier += dict_Element.Values.First();
		}
		else {
			mulitplier += dict_Element.Values.Sum();
		}
		HashSet<float> NPCelement = new();
		if (target.TryGetGlobalNPC(out Element_NPC globalNPC)) {
			NPCelement = globalNPC.dict_Element.Values.ToHashSet();
		}
		modifiers.SourceDamage *= mulitplier - NPCelement.Sum();
	}
	public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
		HashSet<ushort> NPCelement = new();
		if (target.TryGetGlobalNPC(out Element_NPC globalNPC)) {
			NPCelement = globalNPC.dict_Element.Keys.ToHashSet();
		}
		if (dict_Element.Count == 1) {
			int Element = dict_Element.Keys.First();
			ElementSystem.GetElement(Element).OnHitNPC(player, item, null, target, hit, NPCelement);
		}
		else {
			HashSet<ushort> elementType = dict_Element.Keys.ToHashSet();
			foreach (var e in elementType) {
				ElementSystem.GetElement(e).OnHitNPC(player, item, null, target, hit, NPCelement);
			}
		}
	}
}
public class Element_Projectile : GlobalProjectile {
	public override bool InstancePerEntity => true;
	public HashSet<ushort> ElementContainFromSource = new();
	public override void OnSpawn(Projectile projectile, IEntitySource source) {
		if (source == null) {
			return;
		}
		if (source is EntitySource_ItemUse parentItem) {
			if (parentItem.Item.TryGetGlobalItem(out Element_Item globalitem)) {
				ElementContainFromSource.UnionWith(globalitem.dict_Element.Keys);
			}
			else {
				if (parentItem.Player.TryGetModPlayer(out Element_ModPlayer modplayer)) {
					ElementContainFromSource.UnionWith(modplayer.dict_Element.Keys);
				}
			}
		}
		else if (source is EntitySource_Parent parent) {
			if (parent.Entity is NPC npc) {
				if (npc.TryGetGlobalNPC(out Element_NPC globalNPC)) {
					ElementContainFromSource.UnionWith(globalNPC.dict_Element.Keys);
				}
			}
			else if (parent.Entity is Projectile parentProjectile) {
				if (parentProjectile.TryGetGlobalProjectile(out Element_Projectile globalProjectile)) {
					ElementContainFromSource.UnionWith(globalProjectile.ElementContainFromSource);
				}
			}
			else if (parent.Entity is Player player) {
				if (player.TryGetModPlayer(out Element_ModPlayer modplayer)) {
					ElementContainFromSource.UnionWith(modplayer.dict_Element.Keys);
				}
			}
			else if (parent.Entity is Item itemParent) {
				if (itemParent.TryGetGlobalItem(out Element_Item globalitem)) {
					ElementContainFromSource.UnionWith(globalitem.dict_Element.Keys);
				}
			}
		}
	}
	public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
		HashSet<ushort> NPCelement = new();
		if (target.TryGetGlobalNPC(out Element_NPC globalNPC)) {
			NPCelement = globalNPC.dict_Element.Keys.ToHashSet();
		}
		Player player = Main.player[projectile.owner];
		if (ElementContainFromSource.Count == 1) {
			int Element = ElementContainFromSource.First();
			ElementSystem.GetElement(Element).OnHitNPC(player, null, projectile, target, hit, NPCelement);
		}
		else {
			HashSet<ushort> elementType = ElementContainFromSource;
			foreach (var e in elementType) {
				ElementSystem.GetElement(e).OnHitNPC(player, null, projectile, target, hit, NPCelement);
			}
		}
	}
	public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info) {
		HashSet<ushort> TargetElement = new();
		if (target.TryGetModPlayer(out Element_ModPlayer modplayer)) {
			TargetElement = modplayer.dict_Element.Keys.ToHashSet();
		}
		if (ElementContainFromSource.Count == 1) {
			int Element = ElementContainFromSource.First();
			ElementSystem.GetElement(Element).OnHitPlayer(null, target, info, TargetElement);
		}
		else {
			HashSet<ushort> elementType = ElementContainFromSource;
			foreach (var e in elementType) {
				ElementSystem.GetElement(e).OnHitPlayer(null, target, info, TargetElement);
			}
		}
	}
}
public class Element_NPC : GlobalNPC {
	public override bool InstancePerEntity => true;
	public Dictionary<ushort, float> dict_Element = new();
	public void SetElementRes(ushort type, float res) {
		dict_Element.Add(type, MathF.Round(res, 2));
	}
	public override void SetDefaults(NPC entity) {
		foreach (Element el in ElementSystem.list_Element) {
			el.NPC_SetDefault(entity, this);
		}
	}
	public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo) {
		HashSet<ushort> TargetElement = new();
		if (target.TryGetModPlayer(out Element_ModPlayer modplayer)) {
			TargetElement = modplayer.dict_Element.Keys.ToHashSet();
		}
		if (dict_Element.Count == 1) {
			int Element = dict_Element.Keys.First();
			ElementSystem.GetElement(Element).OnHitPlayer(npc, target, hurtInfo, TargetElement);
		}
		else {
			HashSet<ushort> elementType = dict_Element.Keys.ToHashSet();
			foreach (var e in elementType) {
				ElementSystem.GetElement(e).OnHitPlayer(npc, target, hurtInfo, TargetElement);
			}
		}
	}

}
public class Element_ModPlayer : ModPlayer {
	public Dictionary<ushort, float> dict_Element = new();
	public void AddElementAndRes(ushort elementID, float res) {
		if (dict_Element.ContainsKey(elementID)) {
			dict_Element[elementID] += res;
		}
		else {
			dict_Element.Add(elementID, 1 + res);
		}
	}
	public override void ResetEffects() {
		dict_Element.Clear();
	}
}

public abstract class Element : ModType {
	public ushort Type { get; private set; } = 0;
	public static ushort GetElementType<T>() where T : Element {
		return ModContent.GetInstance<T>().Type;
	}
	protected sealed override void Register() {
		Type = ElementSystem.Register(this);
	}
	/// <summary>
	/// Use this to set default element value to specific npc
	/// </summary>
	/// <param name="npc"></param>
	/// <param name="globalNPC"></param>
	public virtual void NPC_SetDefault(NPC npc, Element_NPC globalNPC) {

	}
	/// <summary>
	/// Use this to set default element value to specific item
	/// </summary>
	/// <param name="item"></param>
	/// <param name="globalItem"></param>
	public virtual void Item_SetDefault(Item item, Element_Item globalItem) {

	}
	/// <summary>
	/// </summary>
	/// <param name="player">The Player</param>
	/// <param name="item">the item maybe null, so used with caution, do a null check before doing anything with item</param>
	/// <param name="proj">the projectile maybe null, so used with caution, do a null check before doing anything with projectile</param>
	/// <param name="target"></param>
	/// <param name="info"></param>
	/// <param name="hash_elementType">List of element from item</param>
	public virtual void OnHitNPC(Player player, Item item, Projectile proj, NPC target, NPC.HitInfo info, HashSet<ushort> hash_elementType) {

	}
	/// <summary>
	/// </summary>
	/// <param name="npc">The NPC value maybe null so do a null check before doing anything related to the NPC</param>
	/// <param name="target"></param>
	/// <param name="info"></param>
	/// <param name="hash_elementType">List of element from item</param>
	public virtual void OnHitPlayer(NPC npc, Player target, Player.HurtInfo info, HashSet<ushort> hash_elementType) {

	}
	public override sealed void SetStaticDefaults() {
		base.SetStaticDefaults();
	}
	public override sealed void SetupContent() {
		base.SetupContent();
	}
	public override sealed void Load() {
		base.Load();
	}
	public override sealed void Unload() {
		base.Unload();
	}
}
