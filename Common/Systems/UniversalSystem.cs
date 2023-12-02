using Terraria;
using Terraria.UI;
using Terraria.ID;
using Terraria.GameInput;
using Terraria.ModLoader;
using BossRush.Contents.Perks;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.WeaponEnchantment;
using BossRush.Contents.Items.Card;
using BossRush.Contents.Items;

namespace BossRush.Common.Systems;
internal class UniversalSystem : ModSystem {
	public const string SYNERGY_MODE = "SynergyModeEnable";
	public const string CHALLENGE_MODE = "ChallengeModeEnable";
	public const string NIGHTMARE_MODE = "NightmareEnable";
	public const string HARDCORE_MODE = "Hardcore";
	/// <summary>
	/// Use this to lock content behind hardcore
	/// </summary>
	/// <param name="player"></param>
	/// <param name="context">Use <see cref="CHALLENGE_MODE"/> or any kind of mode that seem fit</param>
	/// <returns></returns>
	public static bool CanAccessContent(Player player, string context) {
		BossRushModConfig config = ModContent.GetInstance<BossRushModConfig>();
		if (config.HardEnableFeature || player.IsDebugPlayer())
			return true;
		if (context == HARDCORE_MODE)
			return player.difficulty == PlayerDifficultyID.Hardcore && config.AutoHardCore;
		if (context == NIGHTMARE_MODE)
			return config.Nightmare;
		if (context == CHALLENGE_MODE)
			return player.difficulty == PlayerDifficultyID.Hardcore && config.EnableChallengeMode;
		if (context == SYNERGY_MODE)
			return player.difficulty == PlayerDifficultyID.Hardcore && config.SynergyMode;
		return false;
	}
	internal UserInterface userInterface;
	public EnchantmentUIState Enchant_uiState;
	public PerkUIState perkUIstate;

	public CardUI cardUIstate;
	public DeCardUIState DeCardUIState;
	public static ModKeybind EnchantmentKeyBind { get; private set; }
	public override void Load() {
		EnchantmentKeyBind = KeybindLoader.RegisterKeybind(Mod, "Enchantment UI", "L");

		//UI stuff
		if (!Main.dedServ) {
			//Mod custom UI
			Enchant_uiState = new();
			perkUIstate = new();

			DeCardUIState = new();
			cardUIstate = new();

			userInterface = new();
		}
	}
	public override void Unload() {
		EnchantmentKeyBind = null;
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
}

public class UniversalModPlayer : ModPlayer {
	public override void OnEnterWorld() {
		var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		if (uiSystemInstance.userInterface.CurrentState != null)
			uiSystemInstance.userInterface.SetState(null);
	}
	public override void ProcessTriggers(TriggersSet triggersSet) {
		if (UniversalSystem.EnchantmentKeyBind.JustPressed) {
			var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
			if (uiSystemInstance.userInterface.CurrentState == null) {
				//Debugging purpose
				uiSystemInstance.Enchant_uiState.WhoAmI = Player.whoAmI;
				uiSystemInstance.userInterface.SetState(uiSystemInstance.Enchant_uiState);
			}
			else
				uiSystemInstance.userInterface.SetState(null);
		}
	}
	public override bool CanUseItem(Item item) {
		var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		if (!item.consumable || item.damage > 0 || item.buffType != 0)
			if (uiSystemInstance.userInterface.CurrentState == uiSystemInstance.DeCardUIState)
				return false;
			else if (uiSystemInstance.userInterface.CurrentState != null)
				return false;
		return base.CanUseItem(item);
	}
}
