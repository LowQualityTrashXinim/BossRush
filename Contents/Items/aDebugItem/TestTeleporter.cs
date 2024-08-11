using BossRush.Texture;
using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using BossRush.Common.ChallengeMode;
using BossRush.Common.WorldGenOverhaul;

namespace BossRush.Contents.Items.aDebugItem;
internal class TestTeleporter : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.width = 32;
		Item.height = 32;
		Item.useTime = 15;
		Item.useAnimation = 15;
		Item.useStyle = ItemUseStyleID.HoldUp;
	}
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		base.ModifyTooltips(tooltips);
		if (Main.netMode == NetmodeID.MultiplayerClient) {
			tooltips.Add(new TooltipLine(Mod, "netcodeshit", "This do not work in multiplayer"));
			return;
		}
		tooltips.Add(new TooltipLine(Mod, "currentZone", $"Teleport to {Main.LocalPlayer.GetModPlayer<TeleportTestPlayer>().BiomeText()}"));
	}
	public override bool AltFunctionUse(Player player) {
		return true;
	}
	public override bool? UseItem(Player player) {
		if (Main.netMode == NetmodeID.SinglePlayer) {
			TeleportTestPlayer modplayer = player.GetModPlayer<TeleportTestPlayer>();
			if (player.altFunctionUse == 2) {
				if (modplayer.Zoneindex >= 15) {
					modplayer.Zoneindex = 0;
				}
				modplayer.Zoneindex = (short)Math.Clamp(modplayer.Zoneindex + 1, 0, 15);
			}
			else {
				if (BossRushWorldGen.Room.ContainsKey(modplayer.Zoneindex)) {
					BossRushWorldGen.FindSuitablePlaceToTeleport(Mod, player, modplayer.Zoneindex);
				}
			}
			return true;
		}
		return true;
	}
}
public class TeleportTestPlayer : ModPlayer {
	public short Zoneindex = 0;
	public string BiomeText() {
		if (RogueLikeWorldGen.BiomeID.ContainsKey(Zoneindex)) {
			string text = RogueLikeWorldGen.BiomeID[Zoneindex];
			return text;
		}
		else {
			return $"Room not found ! current index {Zoneindex}";
		}
	}
	private void SetBiome() {
		if (!Player.HasItem(ModContent.ItemType<TestTeleporter>())) {
			return;
		}
		if (Main.netMode != NetmodeID.SinglePlayer) {
			return;
		}

	}
}
