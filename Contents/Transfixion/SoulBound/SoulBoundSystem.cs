using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace BossRush.Contents.Transfixion.SoulBound;
internal class SoulBoundLoader : ModSystem {
	private static readonly List<ModSoulBound> _SoulBounds = new();
	public static int TotalCount => _SoulBounds.Count;
	public static short Register(ModSoulBound enchant) {
		ModTypeLookup<ModSoulBound>.Register(enchant);
		_SoulBounds.Add(enchant);
		return (short)_SoulBounds.Count;
	}
	public static ModSoulBound GetSoulBound(short type) {
		return type > 0 && type <= _SoulBounds.Count ? _SoulBounds[type - 1] : null;
	}
	public const byte Spirit = 0;
	public const byte Ancestral = 1;
	public const byte Elemental = 2;
	public const byte Primordial = 3;
	public static Color RarityColor(byte rarity) {
		switch (rarity) {
			case 0: return Color.White;
			case 1: return Color.Green;
			case 2: return Color.Cyan;
			case 3: return Color.Magenta;
			default: return Color.White;
		}
	}
}
public class SoulBoundGlobalItem : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.headSlot > 0 || entity.legSlot > 0 || entity.bodySlot > 0;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModSoulBound SoulBound = SoulBoundLoader.GetSoulBound(SoulBoundSlots.assignedType);
		if (SoulBound == null) {
			return;
		}
		tooltips.Add(new TooltipLine(Mod, $"SoulBound",
			$"~ [c/{SoulBoundLoader.RarityColor(SoulBound.Rarity).Hex3()}:{SoulBound.DisplayName}] ~\n" +
			$"{SoulBound.ModifiedToolTip(item)}\n" +
			$"Level : {SoulBoundSlots.Level}\n" +
			$"Experience : {SoulBoundSlots.Exp}/{SoulBoundSlots.ExperienceRequired}"));

	}
	public override bool InstancePerEntity => true;

	public LevelingValue SoulBoundSlots = LevelingValue.Default;
	public static void AddSoulBound(ref Item item, short SoulBoundType) {
		if (item.headSlot <= 0 && item.legSlot <= 0 && item.bodySlot <= 0) {
			return;
		}
		if (item.TryGetGlobalItem(out SoulBoundGlobalItem armorItem)) {
			ModSoulBound modSoulBound = SoulBoundLoader.GetSoulBound(SoulBoundType);
			if (modSoulBound == null) {
				return;
			}
			armorItem.SoulBoundSlots = new(modSoulBound.Type, 0);
		}
	}
	public override GlobalItem NewInstance(Item target) {
		SoulBoundSlots = new();
		return base.NewInstance(target);
	}
	public override GlobalItem Clone(Item from, Item to) {
		SoulBoundGlobalItem clone = (SoulBoundGlobalItem)base.Clone(from, to);
		clone.SoulBoundSlots = new();
		clone.SoulBoundSlots = SoulBoundSlots;
		return clone;
	}
	public override void UpdateEquip(Item item, Player player) {
		if (SoulBoundSlots.assignedType == -1) {
			return;
		}
		bool added = false;
		SoulBoundPlayer augmentationplayer = player.GetModPlayer<SoulBoundPlayer>();
		ModSoulBound SoulBound = SoulBoundLoader.GetSoulBound(SoulBoundSlots.assignedType);
		if (SoulBound == null) {
			return;
		}
		if (!added) {
			augmentationplayer.armorItemUpdate.Add(item);
		}
		SoulBound.UpdateEquip(player, item);
	}

	public override void SaveData(Item item, TagCompound tag) {
		tag.Add("SoulBoundSlot", SoulBoundSlots);
	}
	public override void LoadData(Item item, TagCompound tag) {
		if (tag.TryGet("SoulBoundSlot", out LevelingValue TypeValue))
			SoulBoundSlots = TypeValue;
	}
}
public abstract class ModSoulBound : ModType {
	public short Type { get; internal set; }
	public byte Rarity = 0;
	protected override void Register() {
		SetStaticDefaults();
		Type = SoulBoundLoader.Register(this);
	}
	public virtual string ModifiedToolTip(Item item) => Description;
	public string DisplayName => Language.GetTextValue($"Mods.BossRush.SoulBound.{Name}.DisplayName");
	public string Description => Language.GetTextValue($"Mods.BossRush.SoulBound.{Name}.Description");
	public int GetLevel(Item item) {
		if (item.TryGetGlobalItem(out SoulBoundGlobalItem globalitem)) {
			return globalitem.SoulBoundSlots.Level;
		}
		return 0;
	}
	public static short GetSoulBoundType<T>() where T : ModSoulBound {
		return ModContent.GetInstance<T>().Type;
	}
	public virtual void MeleeEffect(Player player, Item acc, Item item, Rectangle hitbox) { }
	public virtual void Shoot(Player player, Item acc, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) { }
	public virtual void ModifyHitNPCWithItem(Player player, Item acc, Item item, NPC target, ref NPC.HitModifiers modifiers) { }
	public virtual void ModifyHitNPCWithProj(Player player, Item acc, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) { }
	public virtual void OnHitNPCWithItem(Player player, Item acc, Item item, NPC npc, NPC.HitInfo hitInfo) { }
	public virtual void OnHitNPCWithProj(Player player, Item acc, Projectile proj, NPC npc, NPC.HitInfo hitInfo) { }
	public virtual void OnHitNPC(Player player, Item acc, NPC npc, NPC.HitInfo hitInfo) { }
	public virtual void OnHitByNPC(Player player, Item acc, NPC npc, Player.HurtInfo info) { }
	public virtual void OnHitByProj(Player player, Item acc, Projectile projectile, Player.HurtInfo info) { }
	public virtual void UpdateEquip(Player player, Item item) { }
	public virtual bool FreeDodge(Player player, Player.HurtInfo info, Item acc) { return false; }
	/// <summary>
	/// Please uses <code>PlayerStatsHandle.SetSecondLifeCondition</code> in <see cref="UpdateEquip(Player, Item)"/><br/>
	/// And then check condition using <code>PlayerStatsHandle.GetSecondLife</code>
	/// </summary>
	/// <param name="player"></param>
	/// <param name="damage"></param>
	/// <param name="hitDirection"></param>
	/// <param name="pvp"></param>
	/// <param name="playSound"></param>
	/// <param name="genDust"></param>
	/// <param name="damageSource"></param>
	/// <returns></returns>
	public virtual bool PreKill(Player player, Item acc, double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource) { return true; }
}
public class SoulBoundPlayer : ModPlayer {
	public List<Item> armorItemUpdate = new();
	public int IndexSoulBoundItem = -1;
	public override void ResetEffects() {
		armorItemUpdate.Clear();
	}
	public static bool IsSoulBoundable(Item item) => item.headSlot > 0 || item.legSlot > 0 || item.bodySlot > 0;
	public override void MeleeEffects(Item item, Rectangle hitbox) {
		foreach (var acc in armorItemUpdate) {
			if (IsSoulBoundable(acc)) {
				SoulBoundGlobalItem moditem = acc.GetGlobalItem<SoulBoundGlobalItem>();

				ModSoulBound SoulBound = SoulBoundLoader.GetSoulBound(moditem.SoulBoundSlots.assignedType);
				if (SoulBound == null) {
					continue;
				}
				SoulBound.MeleeEffect(Player, acc, item, hitbox);
			}
		}
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		foreach (var acc in armorItemUpdate) {
			if (IsSoulBoundable(acc)) {
				SoulBoundGlobalItem moditem = acc.GetGlobalItem<SoulBoundGlobalItem>();

				ModSoulBound SoulBound = SoulBoundLoader.GetSoulBound(moditem.SoulBoundSlots.assignedType);
				if (SoulBound == null) {
					continue;
				}
				SoulBound.Shoot(Player, acc, item, source, position, velocity, type, damage, knockback);
			}
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		foreach (var item in armorItemUpdate) {
			if (IsSoulBoundable(item)) {
				SoulBoundGlobalItem moditem = item.GetGlobalItem<SoulBoundGlobalItem>();

				ModSoulBound SoulBound = SoulBoundLoader.GetSoulBound(moditem.SoulBoundSlots.assignedType);
				if (SoulBound == null) {
					continue;
				}
				SoulBound.OnHitNPC(Player, item, target, hit);

				moditem.SoulBoundSlots.Modify_Exp(hit.Damage);
				if (moditem.SoulBoundSlots.ReachLevelCondition(10)) {
					BossRushUtils.CombatTextRevamp(Player.Hitbox, SoulBoundLoader.RarityColor(SoulBound.Rarity), $"{SoulBound.DisplayName} level up !");
				}
			}
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		foreach (var itemAcc in armorItemUpdate) {
			if (IsSoulBoundable(itemAcc)) {
				SoulBoundGlobalItem moditem = itemAcc.GetGlobalItem<SoulBoundGlobalItem>();

				ModSoulBound SoulBound = SoulBoundLoader.GetSoulBound(moditem.SoulBoundSlots.assignedType);
				if (SoulBound == null) {
					continue;
				}
				SoulBound.OnHitNPCWithItem(Player, itemAcc, item, target, hit);
			}

		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		foreach (var item in armorItemUpdate) {
			if (IsSoulBoundable(item)) {
				SoulBoundGlobalItem moditem = item.GetGlobalItem<SoulBoundGlobalItem>();

				ModSoulBound SoulBound = SoulBoundLoader.GetSoulBound(moditem.SoulBoundSlots.assignedType);
				if (SoulBound == null) {
					continue;
				}
				SoulBound.OnHitNPCWithProj(Player, item, proj, target, hit);
			}
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		foreach (var item in armorItemUpdate) {
			if (IsSoulBoundable(item)) {
				SoulBoundGlobalItem moditem = item.GetGlobalItem<SoulBoundGlobalItem>();

				ModSoulBound SoulBound = SoulBoundLoader.GetSoulBound(moditem.SoulBoundSlots.assignedType);
				if (SoulBound == null) {
					continue;
				}
				SoulBound.ModifyHitNPCWithProj(Player, item, proj, target, ref modifiers);
			}

		}
	}
	public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
		foreach (var acc in armorItemUpdate) {
			if (IsSoulBoundable(acc)) {
				SoulBoundGlobalItem moditem = acc.GetGlobalItem<SoulBoundGlobalItem>();

				ModSoulBound SoulBound = SoulBoundLoader.GetSoulBound(moditem.SoulBoundSlots.assignedType);
				if (SoulBound == null) {
					continue;
				}
				SoulBound.ModifyHitNPCWithItem(Player, acc, item, target, ref modifiers);
			}

		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		foreach (var acc in armorItemUpdate) {
			if (IsSoulBoundable(acc)) {
				SoulBoundGlobalItem moditem = acc.GetGlobalItem<SoulBoundGlobalItem>();

				ModSoulBound SoulBound = SoulBoundLoader.GetSoulBound(moditem.SoulBoundSlots.assignedType);
				if (SoulBound == null) {
					continue;
				}
				SoulBound.OnHitByNPC(Player, acc, npc, hurtInfo);

			}
		}
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		foreach (var acc in armorItemUpdate) {
			if (IsSoulBoundable(acc)) {
				SoulBoundGlobalItem moditem = acc.GetGlobalItem<SoulBoundGlobalItem>();
				ModSoulBound SoulBound = SoulBoundLoader.GetSoulBound(moditem.SoulBoundSlots.assignedType);
				if (SoulBound == null) {
					continue;
				}
				SoulBound.OnHitByProj(Player, acc, proj, hurtInfo);
			}
		}
	}
	public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource) {
		foreach (var acc in armorItemUpdate) {
			if (IsSoulBoundable(acc)) {
				SoulBoundGlobalItem moditem = acc.GetGlobalItem<SoulBoundGlobalItem>();
				ModSoulBound SoulBound = SoulBoundLoader.GetSoulBound(moditem.SoulBoundSlots.assignedType);
				if (SoulBound == null) {
					continue;
				}
				if (!SoulBound.PreKill(Player, acc, damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource)) {
					return false;
				}
			}
		}
		return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
	}
	public override bool FreeDodge(Player.HurtInfo info) {
		foreach (var acc in armorItemUpdate) {
			if (IsSoulBoundable(acc)) {
				SoulBoundGlobalItem moditem = acc.GetGlobalItem<SoulBoundGlobalItem>();
				ModSoulBound SoulBound = SoulBoundLoader.GetSoulBound(moditem.SoulBoundSlots.assignedType);
				if (SoulBound == null) {
					continue;
				}
				if (SoulBound.FreeDodge(Player, info, acc)) {
					return true;
				}
			}
		}
		return base.FreeDodge(info);
	}
}
public class LevelingSerializer : TagSerializer<LevelingValue, TagCompound> {
	public override TagCompound Serialize(LevelingValue value) => new TagCompound {
		["ID"] = value.assignedType,
		["Level"] = value.Level,
		["Exp"] = value.Exp,
	};

	public override LevelingValue Deserialize(TagCompound tag) => new(tag.Get<short>("ID"), tag.Get<byte>("Level"), tag.Get<ulong>("Exp"));
}
public class LevelingValue {
	public static readonly LevelingValue Default = new();
	public short assignedType { get; private set; }
	public byte Level { get; private set; }
	public ulong Exp { get; private set; }
	public LevelingValue() {
		assignedType = -1;
		Level = 0;
		Exp = 0;
	}
	public LevelingValue(short Type, byte level) {
		assignedType = Type;
		Level = level;
	}
	public LevelingValue(short Type, byte level, ulong exp) {
		assignedType = Type;
		Level = level;
		Exp = exp;
	}
	public void Modify_Exp(int exp) {
		if (exp <= 0) {
			return;
		}
		if ((ulong)exp < ExperienceRequired) {
			Exp = Math.Clamp(Exp + (ulong)exp, 0, int.MaxValue);
		}
	}
	public bool ReachLevelCondition(byte levelCap) {
		if (Exp < ExperienceRequired || Level >= levelCap) {
			return false;
		}
		if (Exp >= ExperienceRequired) {
			Level = (byte)Math.Clamp(Level + 1, byte.MinValue, levelCap);
			Exp = 0;
			return true;
		}
		return false;
	}
	public ulong ExperienceRequired => 20000 * (ulong)Math.Pow(Level, (Level - 1) / 2);
}
