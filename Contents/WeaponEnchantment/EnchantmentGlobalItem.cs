using Terraria;
using BossRush.Common;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria.ID;

namespace BossRush.Contents.WeaponEnchantment;
internal class EnchantmentGlobalItem : GlobalItem {
	public override bool InstancePerEntity => true;
	public int[] EnchantmenStlot = new int[3];
	public override void OnCreated(Item item, ItemCreationContext context) {
	}
	public string GetWeaponModificationStats() =>
		$"Item's enchantment slot : {EnchantmenStlot.Length}";
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (!UniversalSystem.CanAccessContent(Main.LocalPlayer, UniversalSystem.SYNERGY_MODE))
			return;
		if (item.damage > 0 && EnchantmenStlot != null) {
			tooltips.Add(new TooltipLine(Mod, "EnchantmentSlot", $"Item's enchantment slot : {EnchantmenStlot.Length}"));
			string text = "";
			for (int i = 0; i < EnchantmenStlot.Length; i++) {
				if (EnchantmenStlot[i] == ItemID.None) {
					text += $"[{i + 1}]";
					continue;
				}
				text += $"[[i:{EnchantmenStlot[i]}]]";
			}
			tooltips.Add(new TooltipLine(Mod, "", $"{text}"));
		}
	}
	public override void SaveData(Item item, TagCompound tag) {
		if (UniversalSystem.CanAccessContent(Main.LocalPlayer, UniversalSystem.SYNERGY_MODE)) {
			tag.Add("EnchantmentSlot", EnchantmenStlot);
		}
	}
	public override void LoadData(Item item, TagCompound tag) {
		if (UniversalSystem.CanAccessContent(Main.LocalPlayer, UniversalSystem.SYNERGY_MODE)) {
			if (tag.TryGet("EnchantmentSlot", out int[] TypeValue))
				EnchantmenStlot = TypeValue;
		}
	}
}
public class EnchantmentModplayer : ModPlayer {
	Item item;
	EnchantmentGlobalItem globalItem;
	public override void PreUpdate() {
		if (Player.HeldItem.type == ItemID.None)
			return;
		if (item != Player.HeldItem) {
			item = Player.HeldItem;
			globalItem = item.GetGlobalItem<EnchantmentGlobalItem>();
		}
	}
	public override void PostUpdate() {
		if (globalItem == null)
			return;
		if (globalItem.EnchantmenStlot == null)
			return;
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).Update(Player);
		}
	}
	public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (globalItem == null)
			return;
		if (globalItem.EnchantmenStlot == null)
			return;
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyShootStat(Player, item, ref position, ref velocity, ref type, ref damage, ref knockback);
		}
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (globalItem == null)
			return base.Shoot(item, source, position, velocity, type, damage, knockback);
		if (globalItem.EnchantmenStlot != null) {
			for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
				if (globalItem.EnchantmenStlot[i] == 0)
					continue;
				EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).Shoot(Player, item, source, position, velocity, type, damage, knockback);
			}
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
	public override void OnMissingMana(Item item, int neededMana) {
		if (globalItem == null)
			return;
		if (globalItem.EnchantmenStlot == null)
			return;
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnMissingMana(Player, item, neededMana);
		}
	}
	public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
		base.ModifyMaxStats(out health, out mana);
		if (globalItem == null)
			return;
		if (globalItem.EnchantmenStlot == null)
			return;
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyMaxStats(Player, ref health, ref mana);
		}
	}
	public override void ModifyWeaponCrit(Item item, ref float crit) {
		if (globalItem == null)
			return;
		if (globalItem.EnchantmenStlot == null)
			return;
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyCriticalStrikeChance(Player, item, ref crit);
		}
	}
	public override void ModifyItemScale(Item item, ref float scale) {
		if (globalItem == null)
			return;
		if (globalItem.EnchantmenStlot == null)
			return;
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyItemScale(Player, item, ref scale);
		}
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		if (globalItem == null)
			return;
		if (globalItem.EnchantmenStlot == null)
			return;
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyDamage(Player, item, ref damage);
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem == null)
			return;
		if (globalItem.EnchantmenStlot == null)
			return;
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnHitNPCWithItem(Player, item, target, hit, damageDone);
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (globalItem == null)
			return;
		if (globalItem.EnchantmenStlot == null)
			return;
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnHitNPCWithProj(Player, proj, target, hit, damageDone);
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (globalItem == null)
			return;
		if (globalItem.EnchantmenStlot == null)
			return;
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnHitByAnything(Player);
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnHitByNPC(Player, npc, hurtInfo);
		}
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (globalItem == null)
			return;
		if (globalItem.EnchantmenStlot == null)
			return;
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnHitByAnything(Player);
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnHitByProjectile(Player, proj, hurtInfo);
		}
	}
	public override void ModifyManaCost(Item item, ref float reduce, ref float mult) {
		if (globalItem == null)
			return;
		if (globalItem.EnchantmenStlot == null)
			return;
		for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
			if (globalItem.EnchantmenStlot[i] == 0)
				continue;
			EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyManaCost(Player, item, ref reduce, ref mult);
		}
	}
	public override float UseSpeedMultiplier(Item item) {
		float useSpeed = base.UseSpeedMultiplier(item);
		if (globalItem == null)
			return useSpeed;
		if (globalItem.EnchantmenStlot != null) {
			for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
				if (globalItem.EnchantmenStlot[i] == 0)
					continue;
				EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).ModifyUseSpeed(Player, item, ref useSpeed);
			}
		}
		return useSpeed;
	}
	public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) {
		if (globalItem == null)
			return;
		if (globalItem.EnchantmenStlot != null) {
			for (int i = 0; i < globalItem.EnchantmenStlot.Length; i++) {
				if (globalItem.EnchantmenStlot[i] == 0)
					continue;
				EnchantmentLoader.GetEnchantmentItemID(globalItem.EnchantmenStlot[i]).OnKill(Player);
			}
		}
	}
}
