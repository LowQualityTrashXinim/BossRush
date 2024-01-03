using System;
using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using System.Collections.Generic;

namespace BossRush.Contents.Items.aDebugItem {
	internal class ShowPlayerStats : ModItem {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override void SetDefaults() {
			Item.width = Item.height = 10;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			base.ModifyTooltips(tooltips);
			Player player = Main.LocalPlayer;
			PlayerStatsHandle cardplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
			TooltipLine line = new TooltipLine(Mod, "StatsShowcase",
				$"Melee Damage : {player.GetTotalDamage(DamageClass.Melee).ToFloatValue(100, 1)}% Critical strike chance : {player.GetTotalCritChance(DamageClass.Melee)}%" +
				$"\nRange Damage : {player.GetTotalDamage(DamageClass.Ranged).ToFloatValue(100, 1)}% Critical strike chance : {player.GetTotalCritChance(DamageClass.Ranged)}%" +
				$"\nMagic Damage : {player.GetTotalDamage(DamageClass.Magic).ToFloatValue(100, 1)}% Critical strike chance : {player.GetTotalCritChance(DamageClass.Magic)}%" +
				$"\nSummon Damage : {player.GetTotalDamage(DamageClass.Summon).ToFloatValue(100, 1)}% Critical strike chance : {player.GetTotalCritChance(DamageClass.Summon)}%" +
				$"\nGeneric Damage : {player.GetTotalDamage(DamageClass.Generic).ToFloatValue(100, 1)}% Critical strike chance : {player.GetTotalCritChance(DamageClass.Generic)}%" +
				$"\nCrit damage : {Math.Round((cardplayer.CritDamage + cardplayer.UpdateCritDamage) * 100)}%" +
				$"\nHP regen : {player.lifeRegen}" +
				$"\nMana regen : {player.manaRegen}" +
				$"\nMana reduction : {player.manaCost}" +
				$"\nDefense effectiveness : {player.DefenseEffectiveness.Value}" +
				$"\nDamage reduction: {Math.Round(player.endurance, 2)}" +
				$"\nMovement speed : {Math.Round(player.moveSpeed, 2)}" +
				$"\nJump speed : {player.jumpSpeedBoost}" +
				$"\nMax minion : {player.maxMinions}" +
				$"\nMax sentry/turret : {player.maxTurrets}" +
				$"\nThorn : {player.thorns}" +
				$"\nCard luck ( max value 200 ) : {cardplayer.CardLuck}");
			tooltips.Add(line);
		}
	}
}
