using Terraria;
using Terraria.UI;
using Terraria.ID;
using System.Reflection;
using Terraria.ModLoader;
using BossRush.Contents.Perks;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items;
using BossRush.Contents.Skill;
using System.Collections.Generic;
using BossRush.Contents.Items.Card;
using Terraria.GameContent.UI.States;
using BossRush.Contents.WeaponEnchantment;

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
			return player.difficulty == PlayerDifficultyID.Hardcore || config.AutoHardCore;
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
		if (context == BOSSRUSH_MODE)
			return config.BossRushMode;
		if (config.HardEnableFeature)
			return true;
		if (context == NIGHTMARE_MODE)
			return config.Nightmare;
		if (context == HARDCORE_MODE)
			return config.AutoHardCore;
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
	public SkillBarUI defaultUI;

	public TransmutationUIState transmutation_uiState;
	public override void Load() {

		//UI stuff
		if (!Main.dedServ) {
			//Mod custom UI
			Enchant_uiState = new();
			perkUIstate = new();

			transmutation_uiState = new();
			skillUIstate = new();
			defaultUI = new();

			userInterface = new();
		}
		On_UIElement.OnActivate += On_UIElement_OnActivate;
	}
	private void On_UIElement_OnActivate(On_UIElement.orig_OnActivate orig, UIElement self) {
		try {
			if (ModContent.GetInstance<BossRushModConfig>().AutoRandomizeCharacter) {
				if (self is UICharacterCreation el && Main.MenuUI.CurrentState is UICharacterCreation) {
					MethodInfo method = typeof(UICharacterCreation).GetMethod("Click_RandomizePlayer", BindingFlags.NonPublic | BindingFlags.Instance);
					method.Invoke(el, new object[] { null, null });
				}
			}
		}
		finally {
			orig(self);
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
	public void SetState(UIState state) {
		if (userInterface.CurrentState == null || userInterface.CurrentState == defaultUI) {
			userInterface.SetState(state);
		}
	}
	public void DeactivateState() {
		if (userInterface.CurrentState != null) {
			userInterface.SetState(defaultUI);
		}
	}
	//public override void SetStaticDefaults() {
	//	//I am unsure why this is set to true
	//	Main.debuff[BuffID.Campfire] = false;
	//}
}

public class UniversalModPlayer : ModPlayer {
	public override void OnEnterWorld() {
		var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		uiSystemInstance.SetState(uiSystemInstance.defaultUI);
	}
	public override bool CanUseItem(Item item) {
		var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		if (uiSystemInstance.userInterface.CurrentState != null && uiSystemInstance.userInterface.CurrentState != uiSystemInstance.defaultUI) {
			return false;
		}
		return base.CanUseItem(item);
	}
}
