using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

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
	public float ArgumentChance = 1;
	public override void SetDefaults(Item entity) {
		switch (entity.type) {
			case ItemID.CopperBroadsword:
				ArgumentChance = 1;
				break;
		}
	}
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.IsAWeapon();
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		for (int i = 0; i < ArgumentSlots.Length; i++) {
			ModArgument argument = ArgumentLoader.GetArgument(ArgumentSlots[i]);
			if (argument == null) {
				continue;
			}
			tooltips.Add(new TooltipLine(Mod, $"Argument{i + 1}", $"[c/{argument.tooltipColor.Hex3()}:{argument.Name}] : {argument.Description}"));
		}
	}
	public override bool InstancePerEntity => true;
	public int[] ArgumentSlots = new int[5];
	/// <summary>
	/// Can only applied to weapon that is <see cref="BossRushUtils.IsAWeapon(Item)"/>
	/// </summary>
	/// <param name="item"></param>
	public static void AddArgument(Player player, Item item) {
		if (!item.IsAWeapon()) {
			return;
		}
		if (item.TryGetGlobalItem(out ArgumentWeapon weapon)) {
			List<int> ArgumentList = new List<int>();
			for (int i = 1; i <= ArgumentLoader.TotalCount; i++) {
				if (ArgumentLoader.GetArgument(i).ConditionToBeApplied(player, item)) {
					ArgumentList.Add(i);
				}
			}
			int currentEmptySlot = 0;
			bool passException = false;
			for (int i = 0; i < weapon.ArgumentSlots.Length && currentEmptySlot < weapon.ArgumentSlots.Length; i++) {
				if (Main.rand.NextFloat() > weapon.ArgumentChance && !passException) {
					continue;
				}
				if (weapon.ArgumentSlots[currentEmptySlot] == 0) {
					passException = false;
					int type = Main.rand.Next(ArgumentList);
					weapon.ArgumentSlots[currentEmptySlot] = type;
					ArgumentList.Remove(type);
				}
				else {
					currentEmptySlot++;
					passException = true;
					i--;
				}
			}
		}
	}
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
	public override void SaveData(Item item, TagCompound tag) {
		tag.Add("ArgumentSlot", ArgumentSlots);
	}
	public override void LoadData(Item item, TagCompound tag) {
		if (tag.TryGet("ArgumentSlot", out int[] TypeValue))
			ArgumentSlots = TypeValue;
	}
}
public abstract class ModArgument : ModType {
	protected override void Register() {
		ArgumentLoader.Register(this);
		SetStaticDefaults();
	}
	public Color tooltipColor = Color.White;
	public string Description => Language.GetTextValue($"Mods.BossRush.ModArgument.{Name}.Description");
	public virtual void ModifyHitNPC(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) { }
	public virtual void OnHitNPC(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) { }
	public virtual void UpdateHeld(Player player, Item item) { }
	/// <summary>
	/// By default argument will always be applied on weapon
	/// </summary>
	/// <param name="player"></param>
	/// <param name="item"></param>
	/// <returns></returns>
	public virtual bool ConditionToBeApplied(Player player, Item item) => true;
}
public class ArgumentPlayer : ModPlayer {
	ArgumentWeapon weapon = null;
	int ItemTypeCurrent;
	/// <summary>
	/// With this, we can comfortably use <see cref="weapon"/>
	/// </summary>
	/// <param name="item"></param>
	/// <returns></returns>
	public bool IsArgumentable(Item item) => weapon != null && item.IsAWeapon();
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (IsArgumentable(Player.HeldItem)) {
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
		if (IsArgumentable(Player.HeldItem)) {
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
		if(item.TryGetGlobalItem(out ArgumentWeapon argumentWeapon)) {
			weapon = argumentWeapon;
		}
	}
}
