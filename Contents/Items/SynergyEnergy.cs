﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using BossRush.Contents.Items.Weapon;
using BossRush.Common.Global;

namespace BossRush.Contents.Items {
	internal class SynergyEnergy : ModItem {
		public override void SetDefaults() {
			Item.rare = ItemRarityID.Red;
			Item.width = 54;
			Item.height = 20;
			Item.material = true;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<SynergyModPlayer>().acc_SynergyEnergy = true;
			PlayerStatsHandle handle = player.GetModPlayer<PlayerStatsHandle>();
			handle.AddStatsToPlayer(PlayerStats.PureDamage, Multiplicative: 1.01f);
			handle.AddStatsToPlayer(PlayerStats.Defense, Multiplicative: 1.01f);
			handle.AddStatsToPlayer(PlayerStats.MovementSpeed, Multiplicative: 1.01f);
			handle.AddStatsToPlayer(PlayerStats.JumpBoost, Multiplicative: 1.01f);
			handle.Iframe += 1.1f;
			handle.RandomizeChanceEnchantment += .01f;
			handle.BuffTime *= 1.01f;
			handle.DebuffTime *= 1.01f;
			handle.DebuffBuffTime *= .99f;
		}
	}
	public class SynergyModPlayer : ModPlayer {
		public int ItemTypeCurrent = 0;
		public int ItemTypeOld = 0;
		public bool acc_SynergyEnergy = false;
		public bool IsTheItemInQuestionASynergyItem = false;
		public override void ResetEffects() {
			acc_SynergyEnergy = false;
			Item item = Player.HeldItem;
			if (ItemTypeCurrent != item.type) {
				ItemTypeCurrent = item.type;
			}
			if (Player.itemAnimation == 1) {
				ItemTypeOld = ItemTypeCurrent;
			}
			if (item.ModItem is SynergyModItem) {
				IsTheItemInQuestionASynergyItem = true;
			}
			else {
				IsTheItemInQuestionASynergyItem = false;
			}
		}
		public bool CompareOldvsNewItemType => ItemTypeCurrent != ItemTypeOld || IsTheItemInQuestionASynergyItem;
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (UniversalSystem.CanAccessContent(Player, UniversalSystem.SYNERGYFEVER_MODE) && !Player.IsDebugPlayer()) {
			}
			if (!CompareOldvsNewItemType) {
				if (item.ModItem is SynergyModItem) {
					damage = damage.CombineWith(Player.GetModPlayer<PlayerStatsHandle>().SynergyDamage);
				}
				return;
			}
			if (acc_SynergyEnergy) {
				damage.Base += 5;
			}
			damage = damage.CombineWith(Player.GetModPlayer<PlayerStatsHandle>().SynergyDamage);
		}
	}
}
