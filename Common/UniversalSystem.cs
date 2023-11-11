using Terraria;
using Terraria.UI;
using Terraria.ID;
using Terraria.GameInput;
using Terraria.ModLoader;
using BossRush.Contents.Perks;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.WeaponEnchantment;

namespace BossRush.Common;
internal class UniversalSystem : ModSystem {
	public const string SYNERGY_MODE = "SynergyModeEnable";
	public const string CHALLENGE_MODE = "ChallengeModeEnable";
	public static bool CanAccessContent(Player player, string context) {
		if (ModContent.GetInstance<BossRushModConfig>().HardEnableFeature)
			return true;
		if (player.IsDebugPlayer())
			return true;
		if (context == CHALLENGE_MODE)
			return player.difficulty == PlayerDifficultyID.Hardcore && ModContent.GetInstance<BossRushModConfig>().EnableChallengeMode;
		if (context == SYNERGY_MODE)
			return player.difficulty == PlayerDifficultyID.Hardcore && ModContent.GetInstance<BossRushModConfig>().SynergyMode;
		return false;
	}
	internal UserInterface userInterface;
	public EnchantmentUIState Enchant_uiState;
	public PerkUIState perkUIstate;
	public static ModKeybind EnchantmentKeyBind { get; private set; }
	public override void Load() {
		EnchantmentKeyBind = KeybindLoader.RegisterKeybind(Mod, "Enchantment UI", "L");

		//UI stuff
		if (!Main.dedServ) {
			//Mod custom UI
			Enchant_uiState = new();
			perkUIstate = new();

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
		if (InventoryIndex != -1) {
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
}

public class UniversalModPlayer : ModPlayer {
	public override void OnEnterWorld() {
		UniversalSystem uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		if (uiSystemInstance.userInterface.CurrentState != null) {
			uiSystemInstance.userInterface.SetState(null);
		}
	}
	public override void ProcessTriggers(TriggersSet triggersSet) {
		if (UniversalSystem.EnchantmentKeyBind.JustPressed) {
			UniversalSystem uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
			if (uiSystemInstance.userInterface.CurrentState == null) {
				//Debugging purpose
				uiSystemInstance.Enchant_uiState.WhoAmI = Player.whoAmI;
				uiSystemInstance.userInterface.SetState(uiSystemInstance.Enchant_uiState);
			}
			else {
				uiSystemInstance.userInterface.SetState(null);
			}
		}
	}
	public override bool CanUseItem(Item item) {
		UniversalSystem uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		if (uiSystemInstance.userInterface.CurrentState != null) {
			return false;
		}
		return base.CanUseItem(item);
	}
}
