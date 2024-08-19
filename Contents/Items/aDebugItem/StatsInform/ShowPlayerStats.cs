using System;
using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using System.Collections.Generic;
using BossRush.Contents.Items.RelicItem;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Contents.Items.aDebugItem.StatsInform {
	internal class ShowPlayerStats : ModItem {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Item.width = Item.height = 10;
			Item.GetGlobalItem<GlobalItemHandle>().DebugItem = true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			base.ModifyTooltips(tooltips);
			var player = Main.LocalPlayer;
			var statshandle = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
			var line = new TooltipLine(Mod, "StatsShowcase",
				$"Melee Damage : {player.GetTotalDamage(DamageClass.Melee).ToFloatValue(100, 1)}% Crit chance : {player.GetTotalCritChance(DamageClass.Melee)}%" +
				$"\nRange Damage : {player.GetTotalDamage(DamageClass.Ranged).ToFloatValue(100, 1)}% Crit chance : {player.GetTotalCritChance(DamageClass.Ranged)}%" +
				$"\nMagic Damage : {player.GetTotalDamage(DamageClass.Magic).ToFloatValue(100, 1)}% Crit chance : {player.GetTotalCritChance(DamageClass.Magic)}%" +
				$"\nSummon Damage : {player.GetTotalDamage(DamageClass.Summon).ToFloatValue(100, 1)}% Crit chance : {player.GetTotalCritChance(DamageClass.Summon)}%" +
				$"\nGeneric Damage : {player.GetTotalDamage(DamageClass.Generic).ToFloatValue(100, 1)}% Crit chance : {player.GetTotalCritChance(DamageClass.Generic)}%" +
				$"\nCrit damage : {Math.Round((statshandle.UpdateCritDamage.ApplyTo(1) + 1) * 100, 2)}%" +
				$"\nDamage against undamaged NPC : {Math.Round((statshandle.UpdateFullHPDamage.ApplyTo(1) - 1) * 100, 2)}%" +
				$"\nAttack speed: {RelicTemplateLoader.RelicValueToPercentage(player.GetTotalAttackSpeed(DamageClass.Generic))}" +
				$"\nHealth regenaration : {player.lifeRegen}" +
				$"\nMana regenaration : {player.manaRegen}" +
				$"\nMana reduction : {player.manaCost}" +
				$"\nDefense effectiveness : {player.DefenseEffectiveness.Value}" +
				$"\nDamage reduction: {Math.Round(player.endurance * 100, 2)}%" +
				$"\nMovement speed : {Math.Round(player.moveSpeed, 2)}" +
				$"\nJump speed : {player.jumpSpeedBoost}" +
				$"\nMax minion : {player.maxMinions}" +
				$"\nMax sentry/turret : {player.maxTurrets}" +
				$"\nThorn : {player.thorns}");
			tooltips.Add(line);
		}
	}
}
