using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.Systems;
using System.Collections.Generic;

namespace BossRush.Contents.WeaponEnchantment;
public class EnchantmentGlobalItem : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		if (entity.damage > 0 && !entity.accessory && !entity.consumable) {
			return true;
		}
		return false;
	}
	public override bool InstancePerEntity => true;
	public int[] EnchantmenStlot = new int[3];
	public int[] Item_Counter1 = new int[3];
	public int[] Item_Counter2 = new int[3];
	public int[] Item_Counter3 = new int[3];
	public override GlobalItem Clone(Item from, Item to) {
		EnchantmentGlobalItem clone = (EnchantmentGlobalItem)base.Clone(from, to);
		Array.Copy((int[])EnchantmenStlot?.Clone(), clone.EnchantmenStlot, 3);
		return clone;
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
	private bool CommonEnchantmentCheck() => Player.HeldItem.damage <= 0 || globalItem == null || globalItem.EnchantmenStlot == null || !UniversalSystem.CanAccessContent(Player, UniversalSystem.SYNERGY_MODE);
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
						globalItem.Item_Counter1 = new int[3];
						globalItem.Item_Counter2 = new int[3];
						globalItem.Item_Counter3 = new int[3];
					}
				}
			}
			item = Player.HeldItem;
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem localglobal)) {
				globalItem = localglobal;
			}
		}
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
