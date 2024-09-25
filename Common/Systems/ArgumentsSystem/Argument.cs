using System;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace BossRush.Common.Systems.ArgumentsSystem;
internal class ArgumentLoader : ModSystem {
	private static readonly List<ModArgument> _arguments = new();
	public static int TotalCount => _arguments.Count;
	public static int Register(ModArgument enchant) {
		ModTypeLookup<ModArgument>.Register(enchant);
		_arguments.Add(enchant);
		return _arguments.Count;
	}
	public static ModArgument GetArgument(int type) {
		return type > 0 && type <= _arguments.Count ? _arguments[type - 1] : null;
	}
}
public class ArgumentWeapon : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.IsAWeapon();
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		for (int i = 0; i < ArgumentSlots.Length; i++) {
			ModArgument argument = ArgumentLoader.GetArgument(ArgumentSlots[i]);
			if (argument == null) {
				continue;
			}
			tooltips.Add(new TooltipLine(Mod, $"Argument{i + 1}", argument.Description) { OverrideColor = argument.tooltipColor });
		}
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
	public override void HoldItem(Item item, Player player) {
		if (ArgumentSlots == null) {
			ArgumentSlots = new int[5];
		}
		for (int i = 0; i < ArgumentSlots.Length; i++) {
			ModArgument argument = ArgumentLoader.GetArgument(ArgumentSlots[i]);
			if (argument == null) {
				continue;
			}
			argument.UpdateHeld(player, item);
		}
	}
}
public abstract class ModArgument : ModType {
	protected override void Register() {
		ArgumentLoader.Register(this);
	}
	public Color tooltipColor = Color.White;
	public string Description => Language.GetTextValue($"Mods.BossRush.ModArgument.{Name}.Description");
	public virtual void ModifyHitNPC(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) { }
	public virtual void OnHitNPC(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) { }
	public virtual void UpdateHeld(Player player, Item item) { }
}
public class ArgumentPlayer : ModPlayer {
	ArgumentWeapon weapon = null;
	int ItemTypeCurrent;
	/// <summary>
	/// With this, we can comfortably use <see cref="weapon"/>
	/// </summary>
	/// <param name="item"></param>
	/// <returns></returns>
	public bool IsArgumentWeapon(Item item) => weapon != null && item.IsAWeapon();
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (IsArgumentWeapon(Player.HeldItem)) {
			for (int i = 0; i < weapon.ArgumentSlots.Length; i++) {
				ModArgument argument = ArgumentLoader.GetArgument(weapon.ArgumentSlots[i]);
				if (argument == null) {
					continue;
				}
				argument.ModifyHitNPC(Player, Player.HeldItem, target, ref modifiers);
			}
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (IsArgumentWeapon(Player.HeldItem)) {
			for (int i = 0; i < weapon.ArgumentSlots.Length; i++) {
				ModArgument argument = ArgumentLoader.GetArgument(weapon.ArgumentSlots[i]);
				if (argument == null) {
					continue;
				}
				argument.OnHitNPC(Player, Player.HeldItem, target, hit);
			}
		}
	}
	public override void PreUpdate() {
		Item item = Player.HeldItem;
		if (!item.IsAWeapon()) {
			weapon = null;
			return;
		}
		if (ItemTypeCurrent != Player.HeldItem.type) {
			weapon = item.GetGlobalItem<ArgumentWeapon>();
			ItemTypeCurrent = Player.HeldItem.type;
		}
	}
}
