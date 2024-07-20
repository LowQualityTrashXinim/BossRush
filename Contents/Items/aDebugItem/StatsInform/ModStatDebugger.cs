using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Potion;
using BossRush.Contents.Items.SpecialReward;
using BossRush.Common.Systems.ArtifactSystem;

namespace BossRush.Contents.Items.aDebugItem.StatsInform {
	internal class ModStatsDebugger : ModItem {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override void SetDefaults() {
			Item.width = Item.height = 10;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			base.ModifyTooltips(tooltips);
			var chestplayer = Main.LocalPlayer.GetModPlayer<ChestLootDropPlayer>();
			var drugplayer = Main.LocalPlayer.GetModPlayer<WonderDrugPlayer>();
			var nohitPlayer = Main.LocalPlayer.GetModPlayer<NoHitPlayerHandle>();
			var artifactplayer = Main.LocalPlayer.GetModPlayer<ArtifactPlayer>();
			chestplayer.GetAmount();
			var line = new TooltipLine(Mod, "StatsShowcase",
				$"Amount drop chest addition : {chestplayer.amountModifier}" +
				$"\nAmount drop chest multiplication : {chestplayer.finalMultiplier}" +
				$"\nAmount drop chest final weapon : {chestplayer.weaponAmount}" +
				$"\nAmount drop chest final potion type : {chestplayer.potionTypeAmount}" +
				$"\nAmount drop chest final potion amount : {chestplayer.potionNumAmount}" +
				$"\nMelee drop chance : {chestplayer.MeleeChanceMutilplier + chestplayer.UpdateMeleeChanceMutilplier}" +
				$"\nRange drop chance : {chestplayer.RangeChanceMutilplier + chestplayer.UpdateRangeChanceMutilplier}" +
				$"\nMagic drop chance : {chestplayer.MagicChanceMutilplier + chestplayer.UpdateMagicChanceMutilplier}" +
				$"\nSummon drop chance : {chestplayer.SummonChanceMutilplier + chestplayer.UpdateSummonChanceMutilplier}" +
				$"\nWonder drug consumed rate : {drugplayer.DrugDealer}" +
				$"\nAmount boss no-hit : {nohitPlayer.BossNoHitNumber.Count}" +
				$"\nAmount boss don't-hit : {nohitPlayer.DontHitBossNumber.Count}" +
				$"\nCurrent active artifact : {Artifact.GetArtifact(artifactplayer.ActiveArtifact).DisplayName}"
				);
			tooltips.Add(line);
		}
	}
}
