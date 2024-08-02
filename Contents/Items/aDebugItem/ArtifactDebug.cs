using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using BossRush.Common.Systems.ArtifactSystem;

namespace BossRush.Contents.Items.aDebugItem;
internal class ArtifactDebug : ModItem {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.WorldGlobe);
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.useTime = Item.useAnimation = 15;
		Item.useStyle = ItemUseStyleID.HoldUp;
	}
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		base.ModifyTooltips(tooltips);
		if (Main.netMode == NetmodeID.MultiplayerClient) {
			tooltips.Add(new TooltipLine(Mod, "netcodeshit", "This do not work in multiplayer"));
			return;
		}
		ArtifactPlayer artifactplayer = Main.LocalPlayer.GetModPlayer<ArtifactPlayer>();
		tooltips.Add(new TooltipLine(Mod, "CurrentArtifact", $"Current select artifact to imprint : {Artifact.GetArtifact(artifactplayer.ActiveArtifact).DisplayName}"));
	}
	public override bool? UseItem(Player player) {
		if (Main.netMode == NetmodeID.SinglePlayer) {
			ArtifactPlayer modplayer = player.GetModPlayer<ArtifactPlayer>();
			if (modplayer.ActiveArtifact != Artifact.ArtifactCount - 1) {
				modplayer.ActiveArtifact = Math.Clamp(modplayer.ActiveArtifact + 1, 0, Artifact.ArtifactCount - 1);
			}
			else {
				modplayer.ActiveArtifact = 0;
			}
			return true;
		}
		return true;
	}
}
