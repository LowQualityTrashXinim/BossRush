using BossRush.Common.Systems;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria;

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
