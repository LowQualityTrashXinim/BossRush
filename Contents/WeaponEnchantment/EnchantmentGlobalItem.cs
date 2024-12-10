using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.Systems;
using System.Collections.Generic;
using BossRush.Common.Mode.DreamLikeWorldMode;

namespace BossRush.Contents.WeaponEnchantment;
public class EnchantmentSystem : ModSystem {
	public static void EnchantmentRNG(Player self, Item item) {
		if (item == null || !EnchantmentGlobalItem.CanBeEnchanted(item)) {
			return;
		}
		EnchantmentModplayer modplayer = self.GetModPlayer<EnchantmentModplayer>();
		if (modplayer.Request_EnchantedItem > 0) {
			int length = modplayer.Request_EnchantedAmount;
			for (int i = 0; i < length; i++) {
				EnchantItem(ref item, i);
			}
			modplayer.Request_EnchantedItem--;
		}
		if (!ChaosModeSystem.Chaos()) {
			if (!UniversalSystem.Check_TotalRNG()) {
				return;
			}
			if (UniversalSystem.LuckDepartment(UniversalSystem.CHECK_WWEAPONENCHANT)) {
				return;
			}
		}
		for (int i = 0; i < 3; i++) {
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalitem)) {
				if (globalitem.EnchantmenStlot[i] != 0) {
					continue;
				}
			}
			if (Main.rand.NextFloat() <= modplayer.RandomizeChanceEnchantment) {
				EnchantItem(ref item, i);
				continue;
			}
			break;
		}
	}
	public static void EnchantItem(ref Item item, int slot = -1, int enchantmentType = -1) {
		if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalitem)) {
			if (slot == -1) {
				for (int i = 0; i < globalitem.EnchantmenStlot.Length - 1; i++) {
					if (globalitem.EnchantmenStlot[i] != 0) {
						continue;
					}
					slot = i;
				}
			}
			slot = Math.Clamp(slot, 0, globalitem.EnchantmenStlot.Length - 1);
			if (enchantmentType == -1) {
				globalitem.EnchantmenStlot[slot] = Main.rand.Next(EnchantmentLoader.EnchantmentcacheID);
			}
			else {
				ModEnchantment enchant = EnchantmentLoader.GetEnchantmentItemID(enchantmentType);
				if (enchant == null) {
					return;
				}
				enchant.OnAddEnchantment(item, globalitem, enchantmentType, slot);
				globalitem.EnchantmenStlot[slot] = enchantmentType;
			}
		}
	}
}
public class EnchantmentGlobalItem : GlobalItem {
	public static bool CanBeEnchanted(Item entity) => entity.IsAWeapon() && !entity.consumable;
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return CanBeEnchanted(entity);
	}
	public override bool InstancePerEntity => true;
	public int[] EnchantmenStlot = new int[4];
	public int[] Item_Counter1 = new int[4];
	public int[] Item_Counter2 = new int[4];
	public int[] Item_Counter3 = new int[4];
	public override GlobalItem Clone(Item from, Item to) {
		EnchantmentGlobalItem clone = (EnchantmentGlobalItem)base.Clone(from, to);
		if (EnchantmenStlot.Length < 4 || clone.EnchantmenStlot.Length < 4) {
			Array.Resize(ref EnchantmenStlot, 4);
			Array.Resize(ref clone.EnchantmenStlot, 4);
		}
		Array.Copy((int[])EnchantmenStlot?.Clone(), clone.EnchantmenStlot, 4);
		return clone;
	}
	public override GlobalItem NewInstance(Item target) {
		EnchantmenStlot = new int[4];
		Item_Counter1 = new int[4];
		Item_Counter2 = new int[4];
		Item_Counter3 = new int[4];
		return base.NewInstance(target);
	}
	public override void HoldItem(Item item, Player player) {
		if (EnchantmenStlot == null) {
			return;
		}
		//This is here to be consistent
		if (player.HeldItem.type == ItemID.None || item.type == ItemID.None) {
			return;
		}
		for (int i = 0; i < EnchantmenStlot.Length; i++) {
			if (EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(EnchantmenStlot[i]).UpdateHeldItem(i, item, this, player);
		}
	}
	public string GetWeaponModificationStats() => $"Item's enchantment slot : {EnchantmenStlot.Length}";
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (!UniversalSystem.CanAccessContent(Main.LocalPlayer, UniversalSystem.SYNERGY_MODE))
			return;
		if (UniversalSystem.EnchantingState)
			return;
		if (item.damage > 0 && EnchantmenStlot != null) {
			string text = "";
			for (int i = 0; i < EnchantmenStlot.Length; i++) {
				if (EnchantmenStlot[i] == ItemID.None) {
					continue;
				}
				text += $"[[i:{EnchantmenStlot[i]}]]\n{EnchantmentLoader.GetEnchantmentItemID(EnchantmenStlot[i]).Description}\n";
			}
			tooltips.Add(new TooltipLine(Mod, "", $"{text}"));
		}
	}
	public override void SaveData(Item item, TagCompound tag) {
		tag.Add("EnchantmentSlot", EnchantmenStlot);
	}
	public override void LoadData(Item item, TagCompound tag) {
		if (tag.TryGet("EnchantmentSlot", out int[] TypeValue))
			EnchantmenStlot = TypeValue;
	}
}
public class EnchantmentModplayer : ModPlayer {
	Item item;
	EnchantmentGlobalItem globalItem;
	public float RandomizeChanceEnchantment = .2f;
	public void SafeRequest_EnchantItem(int requestAmount, int amountEnchant) {
		Request_EnchantedItem = requestAmount;
		Request_EnchantedAmount = amountEnchant;
	}
	public int Request_EnchantedItem = 0;
	public int Request_EnchantedAmount = 1;
	public override void ResetEffects() {
		RandomizeChanceEnchantment = .2f;
	}
	private bool CommonEnchantmentCheck() => !Player.HeldItem.IsAWeapon() || globalItem == null || globalItem.EnchantmenStlot == null || !UniversalSystem.CanAccessContent(Player, UniversalSystem.SYNERGY_MODE);
	public override void PostUpdate() {
		if (Player.HeldItem.type == ItemID.None)
			return;
		if (item != Player.HeldItem) {
			if (item != null && !CommonEnchantmentCheck()) {
				for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
					if (globalItem.EnchantmenStlot[i] == 0)
						continue;
					if (EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ForcedCleanCounter) {
						EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).PreCleanCounter(i, Player, globalItem, item);
						Array.Fill(globalItem.Item_Counter1, 0);
						Array.Fill(globalItem.Item_Counter2, 0);
						Array.Fill(globalItem.Item_Counter3, 0);
					}
				}
			}
			item = Player.HeldItem;
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem localglobal)) {
				globalItem = localglobal;
			}
		}
	}
	public override void UpdateEquips() {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).Update(Player);
		}
	}
	public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyShootStat(i, Player, globalItem, item, ref position, ref velocity, ref type, ref damage, ref knockback);
		}
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (CommonEnchantmentCheck()) {
			return base.Shoot(item, source, position, velocity, type, damage, knockback);
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).Shoot(i, Player, globalItem, item, source, position, velocity, type, damage, knockback);
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
	public override void OnMissingMana(Item item, int neededMana) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnMissingMana(i, Player, globalItem, item, neededMana);
		}
	}
	public override void ModifyWeaponCrit(Item item, ref float crit) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyCriticalStrikeChance(i, Player, globalItem, item, ref crit);
		}
	}
	public override void ModifyItemScale(Item item, ref float scale) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyItemScale(i, Player, globalItem, item, ref scale);
		}
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyDamage(i, Player, globalItem, item, ref damage);
		}
	}
	public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyHitNPCWithItem(i, Player, globalItem, item, target, ref modifiers);
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnHitNPCWithItem(i, Player, globalItem, item, target, hit, damageDone);
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyHitNPCWithProj(i, Player, globalItem, proj, target, ref modifiers);
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnHitNPCWithProj(i, Player, globalItem, proj, target, hit, damageDone);
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnHitByAnything(Player);
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnHitByNPC(i, globalItem, Player, npc, hurtInfo);
		}
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnHitByAnything(Player);
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnHitByProjectile(i, globalItem, Player, proj, hurtInfo);
		}
	}
	public override void OnConsumeMana(Item item, int manaConsumed) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnConsumeMana(i, Player, globalItem, item, manaConsumed);
		}
	}
	public override void ModifyManaCost(Item item, ref float reduce, ref float mult) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyManaCost(i, Player, globalItem, item, ref reduce, ref mult);
		}
	}
	public override float UseSpeedMultiplier(Item item) {
		float useSpeed = base.UseSpeedMultiplier(item);
		if (CommonEnchantmentCheck()) {
			return useSpeed;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyUseSpeed(i, Player, globalItem, item, ref useSpeed);
		}
		return useSpeed;
	}
	public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) {
		if (CommonEnchantmentCheck()) {
			return;
		}
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;

			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnKill(Player);
		}
	}
}
