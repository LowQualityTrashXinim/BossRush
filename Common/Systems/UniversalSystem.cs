using Terraria;
using Terraria.UI;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Perks;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items;
using System.Collections.Generic;
using BossRush.Contents.Items.Card;
using BossRush.Contents.WeaponEnchantment;
using BossRush.Contents.Skill;

namespace BossRush.Common.Systems;
internal class UniversalSystem : ModSystem {
	public const string SYNERGY_MODE = "SynergyModeEnable";
	public const string BOSSRUSH_MODE = "ChallengeModeEnable";
	public const string NIGHTMARE_MODE = "NightmareEnable";
	public const string HARDCORE_MODE = "Hardcore";
	public const string TRUE_MODE = "TrueMode";
	/// <summary>
	/// Use this to lock content behind hardcore
	/// </summary>
	/// <param name="player"></param>
	/// <param name="context">Use <see cref="BOSSRUSH_MODE"/> or any kind of mode that seem fit</param>
	/// <returns></returns>
	public static bool CanAccessContent(Player player, string context) {
		BossRushModConfig config = ModContent.GetInstance<BossRushModConfig>();
		if (config.HardEnableFeature || player.IsDebugPlayer())
			return true;
		if (context == NIGHTMARE_MODE)
			return config.Nightmare;
		if (context == HARDCORE_MODE)
			return player.difficulty == PlayerDifficultyID.Hardcore && config.AutoHardCore;
		if (context == BOSSRUSH_MODE)
			return player.difficulty == PlayerDifficultyID.Hardcore && config.BossRushMode;
		if (context == SYNERGY_MODE)
			return player.difficulty == PlayerDifficultyID.Hardcore && config.SynergyMode;
		if (context == TRUE_MODE)
			return player.difficulty == PlayerDifficultyID.Hardcore && config.SynergyMode && config.BossRushMode;
		return false;
	}
	public static bool CanAccessContent(string context) {
		BossRushModConfig config = ModContent.GetInstance<BossRushModConfig>();
		if (config.HardEnableFeature)
			return true;
		if (context == NIGHTMARE_MODE)
			return config.Nightmare;
		if (context == HARDCORE_MODE)
			return config.AutoHardCore;
		if (context == BOSSRUSH_MODE)
			return config.BossRushMode;
		if (context == SYNERGY_MODE)
			return config.SynergyMode;
		if (context == TRUE_MODE)
			return config.SynergyMode && config.BossRushMode;
		return false;
	}
	internal UserInterface userInterface;
	public EnchantmentUIState Enchant_uiState;
	public PerkUIState perkUIstate;
	public SkillUI skillUIstate;

	public CardUI cardUIstate;
	public DeCardUIState DeCardUIState;
	public override void Load() {

		//UI stuff
		if (!Main.dedServ) {
			//Mod custom UI
			Enchant_uiState = new();
			perkUIstate = new();

			DeCardUIState = new();
			cardUIstate = new();
			skillUIstate = new();

			userInterface = new();
		}
	}
	public override void Unload() {
	}
	public override void UpdateUI(GameTime gameTime) {
		userInterface?.Update(gameTime);
	}
	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
		int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
		if (InventoryIndex != -1)
			layers.Insert(InventoryIndex, new LegacyGameInterfaceLayer(
				"BossRush: UI",
				delegate {
					userInterface.Draw(Main.spriteBatch, new GameTime());
					return true;
				},
				InterfaceScaleType.UI)
			);
	}
	//public override void SetStaticDefaults() {
	//	//I am unsure why this is set to true
	//	Main.debuff[BuffID.Campfire] = false;
	//}
}

public class UniversalModPlayer : ModPlayer {
	public override void OnEnterWorld() {
		var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		if (uiSystemInstance.userInterface.CurrentState != null) {
			uiSystemInstance.userInterface.SetState(null);
		}
	}
	public override bool CanUseItem(Item item) {
		var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		if (uiSystemInstance.userInterface.CurrentState != null) {
			return false;
		}
		return base.CanUseItem(item);
	}
}
