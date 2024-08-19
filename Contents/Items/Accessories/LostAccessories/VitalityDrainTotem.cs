﻿using BossRush.Common.Systems;
using BossRush.Contents.Items.Weapon;
using BossRush.Texture;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class VitalityDrainTotem : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
		Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<VitalityDrainTotemPlayer>().VitalityDrainTotem = true;
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.LifeStealEffectiveness, Additive: 1.25f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.RegenHP, Base: 3);
	}
}
class VitalityDrainTotemPlayer : ModPlayer {
	public bool VitalityDrainTotem = false;
	int cooldown = 0;
	public override void ResetEffects() {
		VitalityDrainTotem = false;
	}
	public override void PostUpdate() {
		if (!Player.immune || !VitalityDrainTotem) {
			return;
		}
		if (++cooldown <= 12) {
			return;
		}
		Player.Center.LookForHostileNPC(out List<NPC> npclist, 225f);
		int amount = 0;
		foreach (NPC npc in npclist) {
			Player.StrikeNPCDirect(npc, npc.CalculateHitInfo(Math.Clamp((int)(npc.lifeMax * .005f), 1, 1000), 0));
			amount++;
		}
		if (amount > 0) {
			Player.Heal(amount);
		}
		cooldown = 0;
	}
}
