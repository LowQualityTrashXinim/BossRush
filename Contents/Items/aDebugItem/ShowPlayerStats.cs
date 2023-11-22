using BossRush.Contents.Items.Card;
using BossRush.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.aDebugItem {
	internal class ShowPlayerStats : ModItem {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override void SetDefaults() {
			Item.width = Item.height = 10;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			base.ModifyTooltips(tooltips);
			PlayerCardHandle cardplayer = Main.LocalPlayer.GetModPlayer<PlayerCardHandle>();
			string Curses = "";
			foreach (var curses in cardplayer.listCursesID) {
				Curses += $"{{{cardplayer.CursedStringStats(curses)}}}\n";
			}
			TooltipLine line = new TooltipLine(Mod, "StatsShowcase",
				"The below are card's bonus stats" +
				$"\nMelee Damage : {Main.LocalPlayer.GetTotalDamage(DamageClass.Melee).ToFloatValue(100,1)}% Critical strike chance : {Main.LocalPlayer.GetTotalCritChance(DamageClass.Melee)}%" +
				$"\nRange Damage : {Main.LocalPlayer.GetTotalDamage(DamageClass.Ranged).ToFloatValue(100,1)}% Critical strike chance : {Main.LocalPlayer.GetTotalCritChance(DamageClass.Ranged)}%" +
				$"\nMagic Damage : {Main.LocalPlayer.GetTotalDamage(DamageClass.Magic).ToFloatValue(100,1)}% Critical strike chance : {Main.LocalPlayer.GetTotalCritChance(DamageClass.Magic)}%" +
				$"\nSummon Damage : {Main.LocalPlayer.GetTotalDamage(DamageClass.Summon).ToFloatValue(100,1)}% Critical strike chance : {Main.LocalPlayer.GetTotalCritChance(DamageClass.Summon)}%" +
				$"\nGeneric Damage : {Main.LocalPlayer.GetTotalDamage(DamageClass.Generic).ToFloatValue(100,1)}% Critical strike chance : {Main.LocalPlayer.GetTotalCritChance(DamageClass.Generic)}%" +
				$"\nCrit damage : {cardplayer.CritDamage}" +
				$"\nMax HP : {Main.LocalPlayer.statLifeMax} + ({Main.LocalPlayer.statLifeMax2 - Main.LocalPlayer.statLifeMax})" +
				$"\nMax Mana : {cardplayer.ManaMax}" +
				$"\nHP regen : {cardplayer.HPRegen}" +
				$"\nMana regen : {cardplayer.ManaRegen}" +
				$"\nDefense : {cardplayer.DefenseBase}" +
				$"\nDefense effectiveness : {cardplayer.DefenseEffectiveness}" +
				$"\nMovement speed : {cardplayer.Movement}" +
				$"\nJump speed : {cardplayer.JumpBoost}" +
				$"\nMax minion : {cardplayer.MinionSlot}" +
				$"\nMax sentry/turret : {cardplayer.SentrySlot}" +
				$"\nThorn : {cardplayer.Thorn}" +
				$"\nCard luck ( max value 200 ) : {cardplayer.CardLuck}" +
				"\n-Below are curses that you have-\n" +
				Curses
				);
			tooltips.Add(line);
		}
	}
}
