using System;
using Terraria;
using Terraria.ID;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Common.Systems.ArgumentsSystem;
internal class AugmentsLoader : ModSystem {
	private static readonly List<ModAugments> _Augmentss = new();
	public static int TotalCount => _Augmentss.Count;
	public static int Register(ModAugments enchant) {
		ModTypeLookup<ModAugments>.Register(enchant);
		_Augmentss.Add(enchant);
		return _Augmentss.Count;
	}
	public static ModAugments GetAugments(int type) {
		return type > 0 && type <= _Augmentss.Count ? _Augmentss[type - 1] : null;
	}
}
public class AugmentsWeapon : GlobalItem {
	public float AugmentsChance = 0;
	public override void SetDefaults(Item entity) {
		switch (entity.type) {
			default:
				AugmentsChance = 0.01f;
				break;
		}
	}
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.accessory;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		for (int i = 0; i < AugmentsSlots.Length; i++) {
			ModAugments Augments = AugmentsLoader.GetAugments(AugmentsSlots[i]);
			if (Augments == null) {
				continue;
			}
			tooltips.Add(new TooltipLine(Mod, $"Augments{i + 1}", $"[c/{Augments.tooltipColor.Hex3()}:{Augments.DisplayName}] : {Augments.Description}"));
		}
	}
	public float ItemConditionalChance(Item item, Player player) {
		float Chance = 0;
		if (item.prefix == PrefixID.Broken || item.prefix == PrefixID.Annoying) {
			Chance += 1;
		}
		return Chance;
	}
	public override bool InstancePerEntity => true;
	public int[] AugmentsSlots = new int[5];
	/// <summary>
	/// Can only applied to weapon that is <see cref="BossRushUtils.IsAWeapon(Item)"/><br/>
	/// Augments won't always be added, instead it work base on weapon's chance stat<br/>
	/// Use <paramref name="chance"/> to increases the chance directly, be aware it will decay overtime<br/>
	/// Weapon's chance stat and <see cref="ModAugments"/> chance have fixed chance, meaning it won't be decay
	/// Set <paramref name="decayable"/> to disable decay
	/// </summary>
	/// <param name="player">The player</param>
	/// <param name="item">The item</param>
	/// <param name="limit">The limit amount of Augments can have on weapon</param>
	/// <param name="chance">the chance to add Augments</param>
	/// <param name="decayable">disable the decay of custom chance</param>
	public static void AddAugments(Player player, ref Item item, int limit = -1, float chance = 0, bool decayable = true) {
		if (!item.accessory) {
			return;
		}
		if (item.TryGetGlobalItem(out AugmentsWeapon weapon)) {
			Dictionary<int, float> AugmentsList = new();
			for (int i = 1; i <= AugmentsLoader.TotalCount; i++) {
				ModAugments Augments = AugmentsLoader.GetAugments(i);
				if (Augments.ConditionToBeApplied(player, item, out float Chance)) {
					AugmentsList.Add(i, Augments.Chance + Chance);
				}
			}
			AugmentsPlayer modplayer = player.GetModPlayer<AugmentsPlayer>();
			chance += modplayer.Request_ChanceAugments;
			limit += modplayer.Request_LimitAugments;
			if (modplayer.Request_Decayable != null)
				decayable = (bool)modplayer.Request_Decayable;

			int currentEmptySlot = 0;
			bool passException = false;

			if (player.name.Trim().ToLower() == "hero") {
				chance += .1f;
			}
			if (player.IsDebugPlayer()) {
				chance += 10;
			}
			float chanceDecay = modplayer.IncreasesChance + chance + weapon.ItemConditionalChance(item, player);
			ModAugments modAugments = null;
			float augmentChance = 0;
			for (int i = 0; i < weapon.AugmentsSlots.Length && currentEmptySlot < weapon.AugmentsSlots.Length; i++) {
				if (modAugments == null) {
					modAugments = AugmentsLoader.GetAugments(Main.rand.Next(AugmentsList.Keys.ToArray()));
					augmentChance = AugmentsList[modAugments.Type];
					AugmentsList.Remove(modAugments.Type);
				}
				if (limit <= -1) {
					if (currentEmptySlot <= limit) {
						break;
					}
				}
				if (Main.rand.NextFloat() > weapon.AugmentsChance + chanceDecay + augmentChance && !passException) {
					continue;
				}
				if (weapon.AugmentsSlots[currentEmptySlot] == 0) {
					if (decayable) {
						chanceDecay *= .5f;
					}
					passException = false;
					weapon.AugmentsSlots[currentEmptySlot] = modAugments.Type;
					modAugments = null;
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
		AugmentsSlots = new int[5];
		return base.NewInstance(target);
	}
	public override GlobalItem Clone(Item from, Item to) {
		AugmentsWeapon clone = (AugmentsWeapon)base.Clone(from, to);
		Array.Copy((int[])AugmentsSlots?.Clone(), clone.AugmentsSlots, 5);
		return base.Clone(from, to);
	}
	public override void UpdateAccessory(Item item, Player player, bool hideVisual) {
		if (AugmentsSlots == null) {
			AugmentsSlots = new int[5];
		}
		bool added = false;
		AugmentsPlayer augmentationplayer = player.GetModPlayer<AugmentsPlayer>();
		for (int i = 0; i < AugmentsSlots.Length; i++) {
			ModAugments Augments = AugmentsLoader.GetAugments(AugmentsSlots[i]);
			if (Augments == null) {
				continue;
			}
			if (!added) {
				augmentationplayer.accItemUpdate.Add(item);
				added = true;
			}
			augmentationplayer.valid++;
			Augments.UpdateAccessory(player, item);
		}
	}
	public override void SaveData(Item item, TagCompound tag) {
		tag.Add("AugmentsSlot", AugmentsSlots);
	}
	public override void LoadData(Item item, TagCompound tag) {
		if (tag.TryGet("AugmentsSlot", out int[] TypeValue))
			AugmentsSlots = TypeValue;
	}
}
public abstract class ModAugments : ModType {
	public int Type { get; internal set; }
	protected override void Register() {
		Type = AugmentsLoader.Register(this);
		SetStaticDefaults();
	}
	/// <summary>
	/// Be aware it's chance is not decayable and is fixed
	/// </summary>
	public float Chance = 0;
	public Color tooltipColor = Color.White;
	public string DisplayName => Language.GetTextValue($"Mods.BossRush.ModAugments.{Name}.DisplayName");
	public string Description => Language.GetTextValue($"Mods.BossRush.ModAugments.{Name}.Description");
	public virtual void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) { }
	public virtual void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) { }
	public virtual void OnHitNPCWithItem(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) { }
	public virtual void OnHitNPCWithProj(Player player, Projectile proj, NPC npc, NPC.HitInfo hitInfo) { }
	public virtual void OnHitNPC(Player player, Item item, NPC npc, NPC.HitInfo hitInfo) { }
	public virtual void UpdateAccessory(Player player, Item item) { }
	/// <summary>
	/// By default Augments will always be applied on weapon
	/// </summary>
	/// <param name="player"></param>
	/// <param name="item"></param>
	/// <param name="Chance">This is not decayable, so be aware of setting</param>
	/// <returns></returns>
	public virtual bool ConditionToBeApplied(Player player, Item item, out float Chance) {
		Chance = 0f;
		return true;
	}
}
public class AugmentsPlayer : ModPlayer {
	/// <summary>
	/// This chance will decay for each success roll
	/// </summary>
	public float IncreasesChance = 0;
	public List<Item> accItemUpdate = new();
	public void SafeRequest_AddAugments(float chance, int limit, bool decayable) {
		Request_ChanceAugments = chance;
		Request_LimitAugments = limit;
		Request_Decayable = decayable;
	}
	public float Request_ChanceAugments = 0;
	public int Request_LimitAugments = 1;
	public bool? Request_Decayable = false;
	/// <summary>
	/// The amount of augmentation currently equipped
	/// </summary>
	public int valid = 0;
	public override void ResetEffects() {
		IncreasesChance = 0;
		valid = 0;
		accItemUpdate.Clear();
	}
	private bool IsAugmentsable(Item item) => item.accessory;
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		foreach (var item in accItemUpdate) {
			if (IsAugmentsable(item)) {
				AugmentsWeapon moditem = item.GetGlobalItem<AugmentsWeapon>();
				for (int i = 0; i < moditem.AugmentsSlots.Length; i++) {
					ModAugments Augments = AugmentsLoader.GetAugments(moditem.AugmentsSlots[i]);
					if (Augments == null) {
						continue;
					}
					Augments.OnHitNPCWithItem(Player, Player.HeldItem, target, hit);
				}
			}
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		foreach (var item in accItemUpdate) {
			if (IsAugmentsable(item)) {
				AugmentsWeapon moditem = item.GetGlobalItem<AugmentsWeapon>();
				for (int i = 0; i < moditem.AugmentsSlots.Length; i++) {
					ModAugments Augments = AugmentsLoader.GetAugments(moditem.AugmentsSlots[i]);
					if (Augments == null) {
						continue;
					}
					Augments.OnHitNPCWithProj(Player, proj, target, hit);
				}
			}
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		foreach (var item in accItemUpdate) {
			if (IsAugmentsable(item)) {
				AugmentsWeapon moditem = item.GetGlobalItem<AugmentsWeapon>();
				for (int i = 0; i < moditem.AugmentsSlots.Length; i++) {
					ModAugments Augments = AugmentsLoader.GetAugments(moditem.AugmentsSlots[i]);
					if (Augments == null) {
						continue;
					}
					Augments.ModifyHitNPCWithProj(Player, proj, target, ref modifiers);
				}
			}
		}
	}
	public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
		foreach (var acc in accItemUpdate) {
			if (IsAugmentsable(acc)) {
				AugmentsWeapon moditem = acc.GetGlobalItem<AugmentsWeapon>();
				for (int i = 0; i < moditem.AugmentsSlots.Length; i++) {
					ModAugments Augments = AugmentsLoader.GetAugments(moditem.AugmentsSlots[i]);
					if (Augments == null) {
						continue;
					}
					Augments.ModifyHitNPCWithItem(Player, item, target, ref modifiers);
				}
			}
		}
	}
	public override void PreUpdate() {
	}
}
