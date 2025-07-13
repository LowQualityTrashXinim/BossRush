using System;
using Terraria;
using Terraria.ID;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Common.Global;

namespace BossRush.Contents.Transfixion.Arguments;
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
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.accessory;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		for (int i = 0; i < AugmentsSlots.Length; i++) {
			ModAugments Augments = AugmentsLoader.GetAugments(AugmentsSlots[i]);
			if (Augments == null) {
				continue;
			}
			TooltipLine line = Augments.ModifyDescription(Main.LocalPlayer, this, i, item, Check_ChargeConvertToStackAmount(i));
			line.Text = $"{Augments.ModifyName(Main.LocalPlayer, this, i, item, Check_ChargeConvertToStackAmount(i))} : {line.Text}";
			BossRushUtils.AddTooltip(ref tooltips, line);
		}
	}
	public override bool InstancePerEntity => true;
	public int[] AugmentsSlots = new int[5];
	public int[] AugmentsSlotsCharge = new int[5];
	/// <summary>
	/// Can only applied to accessory<br/>
	/// Augments won't always be added, instead it work base on chance stat<br/>
	/// Use <paramref name="chance"/> to increases the chance directly, be aware it will decay overtime<br/>
	/// Set <paramref name="decayable"/> to disable decay
	/// </summary>
	/// <param name="player">The player</param>
	/// <param name="item">The item</param>
	/// <param name="limit">The limit amount of Augments can have on weapon by pure chance</param>
	/// <param name="chance">the chance to add Augments</param>
	/// <param name="decayable">disable the decay of custom chance</param>
	public static void Chance_AddAugments(Player player, ref Item item, int limit = -1, float chance = 0, bool decayable = true) {
		if (!item.accessory) {
			return;
		}
		if (item.TryGetGlobalItem(out AugmentsWeapon weapon)) {
			Dictionary<int, float> AugmentsList = new();
			for (int i = 1; i <= AugmentsLoader.TotalCount; i++) {
				ModAugments Augments = AugmentsLoader.GetAugments(i);
				if (Augments.ConditionToBeApplied(player, item, out float Chance)) {
					AugmentsList.Add(i, Chance);
				}
			}
			AugmentsPlayer modplayer = player.GetModPlayer<AugmentsPlayer>();
			PlayerStatsHandle handle = player.GetModPlayer<PlayerStatsHandle>();
			chance += modplayer.Request_ChanceAugments;
			limit += modplayer.Request_LimitAugments;
			if (modplayer.Request_Decayable != null)
				decayable = (bool)modplayer.Request_Decayable;

			int currentEmptySlot = 0;
			bool passException = false;

			float chanceDecay = handle.AugmentationChance + chance;
			ModAugments modAugments = null;
			float augmentChance = 0;
			for (int i = 0; i < weapon.AugmentsSlots.Length && currentEmptySlot < weapon.AugmentsSlots.Length; i++) {
				if (modAugments == null) {
					modAugments = AugmentsLoader.GetAugments(Main.rand.Next(AugmentsList.Keys.ToArray()));
					augmentChance = AugmentsList[modAugments.Type];
					AugmentsList.Remove(modAugments.Type);
				}
				if (Main.rand.NextFloat() > chanceDecay + augmentChance && !passException || limit == 0) {
					break;
				}
				if (weapon.AugmentsSlots[currentEmptySlot] == 0) {
					if (decayable) {
						chanceDecay *= .5f;
					}
					passException = false;
					weapon.AugmentsSlots[currentEmptySlot] = modAugments.Type;
					modAugments = null;
					limit--;
				}
				else {
					currentEmptySlot++;
					passException = true;
					i--;
				}
			}
		}
	}
	public static void AddAugments<T>(Player player, ref Item item) where T : ModAugments {
		if (!item.accessory) {
			return;
		}
		float chance = player.ModPlayerStats().AugmentationChance;
		int type = ModAugments.GetAugmentType<T>();
		ModAugments aug = AugmentsLoader.GetAugments(type);
		if (aug == null) {
			Main.NewText($"Augmentation not found ! Look up type: {type}");
			return;
		}
		if (item.TryGetGlobalItem(out AugmentsWeapon acc)) {
			for (int i = 0; i < acc.AugmentsSlots.Length; i++) {
				if (acc.AugmentsSlots[i] == 0) {
					acc.AugmentsSlots[i] = type;
				}
			}
		}
	}
	public static void AddAugments(Player player, ref Item item, int type) {
		if (!item.accessory) {
			return;
		}
		float chance = player.ModPlayerStats().AugmentationChance;
		ModAugments aug = AugmentsLoader.GetAugments(type);
		if (aug == null) {
			Main.NewText($"Augmentation not found ! Look up type: {type}");
			return;
		}
		if (item.TryGetGlobalItem(out AugmentsWeapon acc)) {
			for (int i = 0; i < acc.AugmentsSlots.Length; i++) {
				if (acc.AugmentsSlots[i] == 0) {
					acc.AugmentsSlots[i] = type;
				}
			}
		}
	}
	public void Modify_Charge(Player player, int index, int amount) {
		AugmentsSlotsCharge[index] += amount;
	}
	public int Check_ChargeConvertToStackAmount(int index) {
		int Amount = AugmentsSlotsCharge[index] % 50;
		if (AugmentsSlotsCharge[index] >= 250) {
			return Amount;
		}
		else if (AugmentsSlotsCharge[index] == 0) {
			return 0;
		}
		return Amount + 1;
	}
	public override GlobalItem NewInstance(Item target) {
		AugmentsSlots = new int[5];
		AugmentsSlotsCharge = new int[5];
		return base.NewInstance(target);
	}
	public override GlobalItem Clone(Item from, Item to) {
		AugmentsWeapon clone = (AugmentsWeapon)base.Clone(from, to);
		clone.AugmentsSlots = new int[5];
		clone.AugmentsSlotsCharge = new int[5];
		Array.Copy((int[])AugmentsSlots?.Clone(), clone.AugmentsSlots, 5);
		Array.Copy((int[])AugmentsSlotsCharge?.Clone(), clone.AugmentsSlotsCharge, 5);
		return clone;
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
			Augments.UpdateAccessory(player, this, i, item);
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
	public static int GetAugmentType<T>() where T : ModAugments => ModContent.GetInstance<T>().Type;
	public Color tooltipColor = Color.White;
	public string DisplayName => Language.GetTextValue($"Mods.BossRush.ModAugments.{Name}.DisplayName");
	public string Description => Language.GetTextValue($"Mods.BossRush.ModAugments.{Name}.Description");
	protected string DisplayName2(string Extra) => Language.GetTextValue($"Mods.BossRush.ModAugments.{Name}.DisplayName{Extra}");
	protected string Description2(string Extra) => Language.GetTextValue($"Mods.BossRush.ModAugments.{Name}.Description{Extra}");
	public virtual TooltipLine ModifyDescription(Player player, AugmentsWeapon acc, int index, Item item, int stack) => new(Mod, "", Description);
	public string ColorWrapper(string Name) => $"[c/{tooltipColor.Hex3()}:{Name}]";
	public virtual void OnAdded(Player player, Item itme, AugmentsWeapon acc, int index) { }
	public virtual string ModifyName(Player player, AugmentsWeapon acc, int index, Item item, int stack) => ColorWrapper(DisplayName);
	public virtual void ModifyHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC target, ref NPC.HitModifiers modifiers) { }
	public virtual void ModifyHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) { }
	public virtual void OnHitNPCWithItem(Player player, AugmentsWeapon acc, int index, Item item, NPC npc, NPC.HitInfo hitInfo) { }
	public virtual void OnHitNPCWithProj(Player player, AugmentsWeapon acc, int index, Projectile proj, NPC npc, NPC.HitInfo hitInfo) { }
	public virtual void OnHitByNPC(Player player, AugmentsWeapon acc, int index, NPC npc, Player.HurtInfo info) { }
	public virtual void OnHitByProj(Player player, AugmentsWeapon acc, int index, Projectile projectile, Player.HurtInfo info) { }
	public virtual void UpdateAccessory(Player player, AugmentsWeapon acc, int index, Item item) { }
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
	public List<Item> accItemUpdate = new();
	public void SafeRequest_AddAugments(float chance, int limit, bool? decayable) {
		Request_ChanceAugments = chance;
		Request_LimitAugments = limit;
		Request_Decayable = decayable;
	}
	public float Request_ChanceAugments = 0;
	public int Request_LimitAugments = 0;
	public bool? Request_Decayable = null;
	/// <summary>
	/// The amount of augmentation currently equipped
	/// </summary>
	public int valid = 0;
	public override void ResetEffects() {
		valid = 0;
		accItemUpdate.Clear();
	}
	private static bool IsAugmentsable(Item item) => item.accessory;
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		foreach (var itemAcc in accItemUpdate) {
			if (IsAugmentsable(itemAcc)) {
				AugmentsWeapon moditem = itemAcc.GetGlobalItem<AugmentsWeapon>();
				for (int i = 0; i < moditem.AugmentsSlots.Length; i++) {
					ModAugments Augments = AugmentsLoader.GetAugments(moditem.AugmentsSlots[i]);
					if (Augments == null) {
						continue;
					}
					Augments.OnHitNPCWithItem(Player, moditem, i, item, target, hit);
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
					Augments.OnHitNPCWithProj(Player, moditem, i, proj, target, hit);
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
					Augments.ModifyHitNPCWithProj(Player, moditem, i, proj, target, ref modifiers);
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
					Augments.ModifyHitNPCWithItem(Player, moditem, i, item, target, ref modifiers);
				}
			}
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		foreach (var acc in accItemUpdate) {
			if (IsAugmentsable(acc)) {
				AugmentsWeapon moditem = acc.GetGlobalItem<AugmentsWeapon>();
				for (int i = 0; i < moditem.AugmentsSlots.Length; i++) {
					ModAugments Augments = AugmentsLoader.GetAugments(moditem.AugmentsSlots[i]);
					if (Augments == null) {
						continue;
					}
					Augments.OnHitByNPC(Player, moditem, i, npc, hurtInfo);
				}
			}
		}
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		foreach (var acc in accItemUpdate) {
			if (IsAugmentsable(acc)) {
				AugmentsWeapon moditem = acc.GetGlobalItem<AugmentsWeapon>();
				for (int i = 0; i < moditem.AugmentsSlots.Length; i++) {
					ModAugments Augments = AugmentsLoader.GetAugments(moditem.AugmentsSlots[i]);
					if (Augments == null) {
						continue;
					}
					Augments.OnHitByProj(Player, moditem, i, proj, hurtInfo);
				}
			}
		}
	}
}
