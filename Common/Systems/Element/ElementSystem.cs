using BossRush.Common.General;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
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
		//This is for the very late reaction
		if (self.TryGetModPlayer(out Element_ModPlayer modplayer)) {
			foreach (var item in modplayer.ForcingElement) {
				item.OnHitNPC(npc, hit, modplayer.NPC_Element.Select(i => i.Type).ToHashSet());
			}
			//Cleaning the hashset
			modplayer.NPC_Element.Clear();
			modplayer.Item_Element.Clear();
		}
	}

	public override void Unload() {
		list_Element = null;
		dict_Element = null;
	}
	/// <summary>
	/// Use this to assigned basic element stats
	/// </summary>
	/// <param name="npcID">The ID of NPC, use <see cref="NPCID"/> for this</param>
	/// <param name="ElemID">The ElemID, use <see cref="Element.Type"/></param>
	/// <param name="res">The multiplicative value that will be applied</param>
	/// <param name="NPCHasElem">Determined whenever or not the NPC in <paramref name="npcID"/> has this Element or not</param>
	public static void AssignedNPC(int npcID, int ElemID, float res, bool NPCHasElem = true) {
		Element element = GetElement(ElemID);
		if (element == null) {
			BossRush.Instance.Logger.Info($"Unable to find element correct to ElemID, did you correctly assigned id ? | Element ID : {ElemID}");
			return;
		}
		else if (npcID < 0 || npcID >= NPCID.Count) {
			BossRush.Instance.Logger.Info($"There are no NPC with this ID, did you correctly write the ID ? | NPC ID : {npcID}");
			return;
		}
		element.NPCElemResValue[npcID] = res;
		element.AppliedToNPC[npcID] = NPCHasElem;
	}
	/// <summary>
	/// Use this to assigned Element to Item
	/// </summary>
	///  <param name="ItemID">The ID of Item, use <see cref="ItemID"/> for this</param>
	/// <param name="ItemHasElem">Determined whenever or not the Item in <paramref name="ItemID"/> in has this Element or not</param>
	public static void AssignedItem(int ItemID, int ElemID, bool ItemHasElem = true) {
		Element element = GetElement(ElemID);
		if (element == null) {
			BossRush.Instance.Logger.Info($"Unable to find element correct to ElemID, did you correctly assigned id ? | Element ID : {ElemID}");
			return;
		}
		element.AppliedToItem[ItemID] = ItemHasElem;
	}
}
public class Element_ModPlayer : ModPlayer {
	public HashSet<Element> NPC_Element = new HashSet<Element>();
	public HashSet<ushort> Item_Element = new HashSet<ushort>();
	public float[] ElementRes = [];
	public HashSet<Element> ForcingElement = new();
	public override void ResetEffects() {
		if (ElementRes.Length != ElementSystem.TotalCount) {
			Array.Resize(ref ElementRes, ElementSystem.TotalCount);
		}
		else {
			Array.Fill(ElementRes, 1f);
		}
		ForcingElement.Clear();
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		foreach (Element el in ElementSystem.list_Element) {
			//Checking if this npc have element
			if (!el.AppliedToNPC[npc.type]) {
				//continue cause they don't
				continue;
			}
			//Get player elemental resistance
			float res = ElementRes[el.Type];
			if (res >= 0) {
				//Multiply elemental resistance
				modifiers.SourceDamage *= res;
			}
		}
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		foreach (Element el in ElementSystem.list_Element) {
			//Checking whenever or not this NPC has this Element
			if (el.AppliedToNPC[target.type]) {
				NPC_Element.Add(el);
			}
			//Checking whenever or not the item player is holding have the element or not, if not skip
			if (!el.AppliedToItem[Player.HeldItem.type]) {
				continue;
			}
			//Get elemental resistance value from NPC
			float res = el.NPCElemResValue[target.type];
			if (res >= 0) {
				modifiers.SourceDamage *= res;
			}
			//Add this element type into the hashset
			Item_Element.Add(el.Type);
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.TryGetGlobalProjectile(out RoguelikeGlobalProjectile global)) {
			if (global.Source_ItemType == 0) {
				return;
			}
			foreach (Element el in ElementSystem.list_Element) {
				//Checking whenever or not this NPC has this Element
				if (el.AppliedToNPC[target.type]) {
					NPC_Element.Add(el);
				}
				//Checking whenever or not the item player is holding have the element or not, if not skip
				if (!el.AppliedToItem[global.Source_ItemType]) {
					continue;
				}
				//Get elemental resistance value from NPC
				float res = el.NPCElemResValue[target.type];
				if (res >= 0) {
					modifiers.SourceDamage *= res;
				}
				//Add this element type into the hashset
				Item_Element.Add(el.Type);
			}
		}
	}
	public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
		foreach (Element el in ElementSystem.list_Element) {
			//Checking whenever or not this NPC has this Element
			if (el.AppliedToNPC[target.type]) {
				NPC_Element.Add(el);
			}
			//Checking whenever or not the item player is holding have the element or not, if not skip
			if (!el.AppliedToItem[item.type]) {
				continue;
			}
			//Get elemental resistance value from NPC
			float res = el.NPCElemResValue[target.type];
			if (res >= 0) {
				modifiers.SourceDamage *= res;
			}
			//Add this element type into the hashset
			Item_Element.Add(el.Type);
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		foreach (Element el in NPC_Element) {
			//Do elemental custom reaction here
			el.OnHitNPC(target, hit, Item_Element);
		}
	}
}

public abstract class Element : ModType {
	public ushort Type { get; private set; } = 0;
	/// <summary>
	/// Use this to assgined how resistance the NPC are against this element, however it is recommend to use <see cref="ElementSystem.AssignedNPC(int, int, float)"/> or <see cref="ElementSystem.AssignedNPC(int, string, float)"/>
	/// </summary>
	public float[] NPCElemResValue = NPCID.Sets.Factory.CreateFloatSet(1f);
	/// <summary>
	/// Use this to assigned if wherever or not this NPC type can have this element
	/// </summary>
	public bool[] AppliedToNPC = NPCID.Sets.Factory.CreateBoolSet();
	/// <summary>
	/// Use this to assigned if wherever or not this Item type can have this element
	/// </summary>
	public bool[] AppliedToItem = ItemID.Sets.Factory.CreateBoolSet();
	public static ushort GetElementType<T>() where T : Element {
		return ModContent.GetInstance<T>().Type;
	}
	protected sealed override void Register() {
		Type = ElementSystem.Register(this);
		SetDefault();
	}
	/// <summary>
	/// This is where you set all of your value and basic stats 
	/// </summary>
	public virtual void SetDefault() {
		ElementSystem.AssignedNPC(NPCID.Demon, Type, .5f, true);
		ElementSystem.AssignedItem(ItemID.FieryGreatsword, Type, true);
	}
	/// <summary>
	/// </summary>
	/// <param name="target"></param>
	/// <param name="info"></param>
	/// <param name="hash_elementType">List of element from item</param>
	public virtual void OnHitNPC(NPC target, NPC.HitInfo info, HashSet<ushort> hash_elementType) {

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
