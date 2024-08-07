﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.BuffAndDebuff;

namespace BossRush.Contents.Items.Accessories.EnragedBossAccessories.EvilEye {
	internal class EvilEye : ModItem {
		public override void SetDefaults() {
			Item.accessory = true;
			Item.height = 40;
			Item.width = 40;
			Item.rare = ItemRarityID.Lime;
			Item.value = 10000000;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			Player player = Main.player[Main.myPlayer];
			if (player.HeldItem.type == ItemID.TheEyeOfCthulhu) {
				tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.TheEyeOfCthulhu}]Damage is scale with progression, for each 4 hit, you spawn out EoC Servant"));
			}
			if (player.GetModPlayer<EvilEyePlayer>().EoCShieldUpgrade) {
				tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.EoCShield}]Whenever you walk, you spawn a EoC servant that home into enemy"));
			}
			if (player.HasItem(ItemID.EyeofCthulhuTrophy)) {
				tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.EyeofCthulhuTrophy}]EoC servant spawn out more often on hit, for each 10 hit you spawn out Phantasmal Eye"));
			}
		}
		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.statDefense += 10;
			if (player.statLife <= player.statLifeMax2 * 0.45f) {
				player.moveSpeed *= 1.25f;
				player.GetDamage(DamageClass.Generic) *= 1.25f;
			}
			else {
				player.GetDamage(DamageClass.Generic) *= 1.05f;
				player.endurance += 0.05f;
			}
			player.GetModPlayer<EvilEyePlayer>().EoCBless = true;
		}
	}
	class EvilEyesPlayer : GlobalItem {
		public override void UpdateAccessory(Item item, Player player, bool hideVisual) {
			if (item.type == ItemID.EoCShield) {
				player.GetModPlayer<EvilEyePlayer>().EoCShieldUpgrade = true;
			}
		}
	}
	internal class EvilEyePlayer : ModPlayer {
		public int EoCCounter = 0;
		public int EoCCounter2 = 0;
		public int EoCCounter3 = 0;
		public bool EyeProtection = true;
		public bool EoCBless;
		public bool EoCShieldUpgrade;
		public override void ResetEffects() {
			EyeProtection = true;
			EoCBless = false;
			EoCShieldUpgrade = false;
		}
		public override void UpdateEquips() {
			if (Player.velocity != Vector2.Zero && EoCBless) {
				EoCCounter++;
			}
			if (EoCShieldUpgrade && EoCCounter >= 30) {
				Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, -Player.velocity, ModContent.ProjectileType<EoCServant>(), (int)(Player.GetWeaponDamage(Player.HeldItem) * 0.5f) + 10, 1f, Player.whoAmI);
				EoCCounter = 0;
			}
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (EoCShieldUpgrade && EoCBless && Player.HeldItem.type == ItemID.TheEyeOfCthulhu && proj.type != ModContent.ProjectileType<EoCServant>() && proj.type != ModContent.ProjectileType<PhantasmalEye>()) {
				EntitySource_ItemUse EoC = new EntitySource_ItemUse(Player, new Item(ModContent.ItemType<EvilEye>()));
				if (EoCCounter2 >= 4) {
					Projectile.NewProjectile(EoC, Player.Center, Main.rand.NextVector2CircularEdge(10, 10), ModContent.ProjectileType<EoCServant>(), (int)(Player.GetWeaponDamage(Player.HeldItem) * 0.5f) + 10, 1f, Player.whoAmI);
					EoCCounter2 = 0;
				}
				if (EoCCounter3 >= 10) {
					Projectile.NewProjectile(EoC, Player.Center, Main.rand.NextVector2CircularEdge(20, 20), ModContent.ProjectileType<PhantasmalEye>(), (int)(Player.GetWeaponDamage(Player.HeldItem) * 2.5f) + 100, 1f, Player.whoAmI);
					EoCCounter3 = 0;
				}
				if (Player.HasItem(ItemID.EyeofCthulhuTrophy)) {
					EoCCounter2++;
					EoCCounter3++;
				}
				EoCCounter2++;
			}
		}

		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (!NPC.downedPlantBoss && !EoCBless && item.type == ItemID.TheEyeOfCthulhu) {
				damage *= 0.05f;
			}
			else if (EoCBless && item.type == ItemID.TheEyeOfCthulhu) {
				float DamageMultiplier = 0.05f;
				if (NPC.downedMoonlord) {
					DamageMultiplier += 1.7f;
				}
				if (NPC.downedAncientCultist) {
					DamageMultiplier += 0.15f;
				}
				if (NPC.downedGolemBoss) {
					DamageMultiplier += 0.15f;
				}
				if (NPC.downedPlantBoss) {
					DamageMultiplier += 0.3f;
				}
				if (NPC.downedMechBossAny) {
					DamageMultiplier += 0.2f;
				}
				if (Main.hardMode) {
					DamageMultiplier += 0.15f;
				}
				if (NPC.downedBoss3) {
					DamageMultiplier += 0.1f;
				}
				if (NPC.downedBoss2) {
					DamageMultiplier += 0.1f;
				}
				if (NPC.downedBoss1) {
					DamageMultiplier += 0.1f;
				}
				damage *= DamageMultiplier;
			}
		}
		public override void ModifyHurt(ref Player.HurtModifiers modifiers) {
			if (Player.statLife <= Player.statLifeMax2 * 0.1f && EyeProtection && EoCBless) {
				Player.AddBuff(ModContent.BuffType<EvilEyeProtection>(), 7200);
				Player.Heal((int)(Player.statLifeMax2 * 0.7f));
			}
		}
	}
}
