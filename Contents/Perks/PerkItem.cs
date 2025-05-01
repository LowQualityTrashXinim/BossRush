using BossRush.Common.Systems;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Perks;
class WorldEssence : ModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("Perk");
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 23);
		Item.maxStack = 999;
	}
	public override bool? UseItem(Player player) {
		UniversalSystem uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		uiSystemInstance.ActivatePerkUI(PerkUIState.DefaultState);
		return true;
	}
}
class GlitchWorldEssence : ModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("Perk");
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 23);
		Item.maxStack = 999;
	}
	public override bool? UseItem(Player player) {
		UniversalSystem uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		uiSystemInstance.ActivatePerkUI(PerkUIState.DefaultState, "Glitch");
		return true;
	}
}
class CelestialEssence : ModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("Perk");
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 23);
		Item.maxStack = 999;
	}

	public override bool? UseItem(Player player) {
		UniversalSystem uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		uiSystemInstance.ActivatePerkUI(PerkUIState.StarterPerkState);
		return true;
	}
}
class LuckEssence : ModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("Perk");
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 23);
		Item.maxStack = 999;
	}
	public override bool? UseItem(Player player) {
		UniversalSystem uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		if (player.ItemAnimationJustStarted) {
			PerkPlayer modplayer = Main.LocalPlayer.GetModPlayer<PerkPlayer>();
			List<int> listOfPerk = new List<int>();
			for (int i = 0; i < ModPerkLoader.TotalCount; i++) {
				if (modplayer.perks.ContainsKey(i)) {
					if ((!ModPerkLoader.GetPerk(i).CanBeStack && modplayer.perks[i] > 0)
						|| modplayer.perks[i] >= ModPerkLoader.GetPerk(i).StackLimit) {
						continue;
					}
				}
				if (!ModPerkLoader.GetPerk(i).SelectChoosing()) {
					continue;
				}
				if (!ModPerkLoader.GetPerk(i).CanBeChoosen) {
					continue;
				}
				listOfPerk.Add(i);
			}
			int perkType = Main.rand.Next(listOfPerk);
			UniversalSystem.AddPerk(perkType);
			BossRushUtils.CombatTextRevamp(Main.LocalPlayer.Hitbox, Color.AliceBlue, ModPerkLoader.GetPerk(perkType).DisplayName);
		}
		return true;
	}
}
