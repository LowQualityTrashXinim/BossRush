using BossRush.Common.Systems;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria;
using System.Collections.Generic;

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
		uiSystemInstance.ActivatePerkUI(PerkUIState.GamblerState);
		return true;
	}
}
class WindSlashPerk : ModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("Perk");
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		tooltips.Add(new(Mod, "Perk_WindSlash", ModPerkLoader.GetPerk(Perk.GetPerkType<WindSlash>()).Description));
	}
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 23);
		Item.maxStack = 999;
	}
	public override bool? UseItem(Player player) {
		if (player.itemAnimation == player.itemAnimationMax) {
			PerkPlayer modplayer = player.GetModPlayer<PerkPlayer>();
			if (!modplayer.perks.ContainsKey(Perk.GetPerkType<WindSlash>())) {
				modplayer.perks.Add(Perk.GetPerkType<WindSlash>(), 1);
			}
			else {
				modplayer.perks[Perk.GetPerkType<WindSlash>()]++;
			}
		}
		return true;
	}
}
