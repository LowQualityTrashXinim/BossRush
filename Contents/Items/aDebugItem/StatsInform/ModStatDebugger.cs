using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Contents.Items.Consumable.Potion;
using BossRush.Contents.Items.Consumable.SpecialReward;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.aDebugItem.StatsInform {
	internal class ModStatsDebugger : ModItem {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Item.width = Item.height = 10;
			Item.Set_DebugItem(true);
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			base.ModifyTooltips(tooltips);
			var chestplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
			var drugplayer = Main.LocalPlayer.GetModPlayer<WonderDrugPlayer>();
			var nohitPlayer = Main.LocalPlayer.GetModPlayer<NoHitPlayerHandle>();
			var artifactplayer = Main.LocalPlayer.GetModPlayer<ArtifactPlayer>();
			chestplayer.GetAmount();
			var line = new TooltipLine(Mod, "StatsShowcase",
				$"Amount drop : {chestplayer.DropModifier.ApplyTo(1)}" +
				$"\nAmount drop chest final weapon : {chestplayer.weaponAmount}" +
				$"\nAmount drop chest final potion type : {chestplayer.potionTypeAmount}" +
				$"\nAmount drop chest final potion amount : {chestplayer.potionNumAmount}" +
				$"\nMelee drop chance : {chestplayer.UpdateMeleeChanceMutilplier}" +
				$"\nRange drop chance : {chestplayer.UpdateRangeChanceMutilplier}" +
				$"\nMagic drop chance : {chestplayer.UpdateMagicChanceMutilplier}" +
				$"\nSummon drop chance : {chestplayer.UpdateSummonChanceMutilplier}" +
				$"\nWonder drug consumed rate : {drugplayer.DrugDealer}" +
				$"\nAmount boss no-hit : {nohitPlayer.BossNoHitNumber.Count}" +
				$"\nAmount boss don't-hit : {nohitPlayer.DontHitBossNumber.Count}" +
				$"\nCurrent active artifact : {Artifact.GetArtifact(artifactplayer.ActiveArtifact).DisplayName}"
				);
			tooltips.Add(line);
		}
	}
}
