using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Common.Systems.ArgumentsSystem;
internal class ArgumentLoader : ModSystem {
	private static readonly List<ModArgument> _arguments = new();
	public static int TotalCount => _arguments.Count;
	public static int Register(ModArgument enchant) {
		ModTypeLookup<ModArgument>.Register(enchant);
		_arguments.Add(enchant);
		return _arguments.Count - 1;
	}
	public static ModArgument GetArgument(int type) {
		return type >= 0 && type < _arguments.Count ? _arguments[type] : null;
	}
}
public class ArgumentWeapon : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.IsAWeapon();
	}
	public override bool InstancePerEntity => true;
	public int[] ArgumentSlots = new int[5];
	public override GlobalItem NewInstance(Item target) {
		ArgumentSlots = new int[5];
		return base.NewInstance(target);
	}
	public override GlobalItem Clone(Item from, Item to) {
		ArgumentWeapon clone = (ArgumentWeapon)base.Clone(from, to);
		Array.Copy((int[])ArgumentSlots?.Clone(), clone.ArgumentSlots, 5);
		return base.Clone(from, to);
	}
}
public abstract class ModArgument : ModType {
	protected override void Register() {
		ArgumentLoader.Register(this);
	}
	public virtual void OnHitNPC(Player player, NPC npc, NPC.HitInfo hitInfo) { }
	public virtual void UpdateHeld(Player player, Item item) { }
}
public class ArgumentPlayer : ModPlayer {
	ArgumentWeapon weapon = null;
	int ItemTypeCurrent;
	int ItemTypeOld;
	/// <summary>
	/// With this, we can comfortably use <see cref="weapon"/>
	/// </summary>
	/// <param name="item"></param>
	/// <returns></returns>
	public bool IsArgumentWeapon(Item item) => weapon != null && item.IsAWeapon();
	
	public override void PreUpdate() {
		Item item = Player.HeldItem;
		if(!item.IsAWeapon()) {
			return;
		}
		if (ItemTypeCurrent != Player.HeldItem.type) {
			ItemTypeCurrent = Player.HeldItem.type;
		}
		if (Player.itemAnimation == 1) {
			ItemTypeOld = ItemTypeCurrent;
		}
	}
}
