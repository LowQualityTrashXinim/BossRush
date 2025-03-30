using System;
using Terraria;
using System.IO;
using Terraria.UI;
using Terraria.ID;
using System.Linq;
using ReLogic.Content;
using BossRush.Texture;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using BossRush.Contents.NPCs;
using BossRush.Contents.Perks;
using Microsoft.Xna.Framework;
using BossRush.Contents.Skill;
using BossRush.Common.General;
using System.Collections.Generic;
using BossRush.Common.ChallengeMode;
using Microsoft.Xna.Framework.Input;
using Terraria.GameContent.UI.States;
using BossRush.Common.WorldGenOverhaul;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using BossRush.Contents.Items.aDebugItem;
using BossRush.Common.Systems.Achievement;
using BossRush.Common.Systems.SpoilSystem;
using BossRush.Common.Systems.CursesSystem;
using BossRush.Common.Mode.DreamLikeWorldMode;
using BossRush.Contents.Items.Consumable.Spawner;
using BossRush.Contents.Items.aDebugItem.RelicDebug;
using BossRush.Contents.Items.aDebugItem.SkillDebug;
using BossRush.Contents.Transfixion.WeaponEnchantment;
using BossRush.Contents.Items.Toggle;
using BossRush.Common.Global;
using Terraria.Achievements;

namespace BossRush.Common.Systems;
public static class RoguelikeData {
	public static int Lootbox_AmountOpen = 0;
	public static int Run_Amount = 0;
	public static int EliteKill = 0;
}
/// <summary>
/// This not only include main stuff that make everything work but also contain some fixes to vanilla<br/>
/// Also, very unholy class, do not look into it
/// </summary>
internal class UniversalSystem : ModSystem {
	public static bool DidPlayerBeatTheMod(bool BossRushAllowed = true) => Main.hardMode && (BossRushAllowed && ModContent.GetInstance<RogueLikeConfig>().BossRushMode);
	public const string BOSSRUSH_MODE = "ChallengeModeEnable";
	public const string NIGHTMARE_MODE = "NightmareEnable";
	public const string HELLISH_MODE = "HellishEnable";
	public const string CHAOS_MODE = "ChaosEnable";
	public const string HARDCORE_MODE = "Hardcore";
	public const string SYNERGYFEVER_MODE = "SynergyFeverMode";
	public static bool NotNormalMode() => Main.expertMode || Main.masterMode;
	/// <summary>
	/// Use this to universally lock content behind hardcore, it basically act like a wrapper for <see cref="RogueLikeConfig"/>
	/// </summary>
	/// <param name="player"></param>
	/// <param name="context">Use <see cref="BOSSRUSH_MODE"/> or any kind of mode that seem fit</param>
	/// <returns></returns>
	public static bool CanAccessContent(Player player, string context) {
		RogueLikeConfig config = ModContent.GetInstance<RogueLikeConfig>();
		if (context == SYNERGYFEVER_MODE)
			return config.SynergyFeverMode;
		if (config.HardEnableFeature || player.IsDebugPlayer())
			return true;
		if (context == NIGHTMARE_MODE)
			return config.Nightmare;
		if (context == HELLISH_MODE)
			return config.HellishEndeavour;
		if (context == HARDCORE_MODE)
			return player.difficulty == PlayerDifficultyID.Hardcore || config.AutoHardCore;
		if (player.difficulty != PlayerDifficultyID.Hardcore && !config.AutoHardCore)
			return false;
		if (context == BOSSRUSH_MODE)
			return config.BossRushMode;
		return false;
	}
	/// <summary>
	/// Use this to lock content behind certain config, it basically act like a wrapper for <see cref="RogueLikeConfig"/>
	/// </summary>
	/// <param name="context">Use <see cref="BOSSRUSH_MODE"/> or any kind of mode that seem fit</param>
	/// <returns></returns>
	public static bool CanAccessContent(string context) {
		RogueLikeConfig config = ModContent.GetInstance<RogueLikeConfig>();
		if (context == BOSSRUSH_MODE)
			return config.BossRushMode;
		if (config.HardEnableFeature)
			return true;
		if (context == NIGHTMARE_MODE)
			return config.Nightmare;
		if (context == HELLISH_MODE)
			return config.HellishEndeavour;
		if (context == CHAOS_MODE)
			return config.DreamlikeWorld;
		if (context == HARDCORE_MODE)
			return config.AutoHardCore;
		if (context == SYNERGYFEVER_MODE)
			return config.SynergyFeverMode;
		return false;
	}
	public const string LEGACY_LOOTBOX = "lootbox";
	public const string LEGACY_WORLDGEN = "worldgen";
	/// <summary>
	/// Check legacy option whenever or not if it enable or not
	/// </summary>
	/// <param name="option"></param>
	/// <returns>
	/// return true if it is enable
	/// </returns>
	public static bool CheckLegacy(string option) {
		RogueLikeConfig config = ModContent.GetInstance<RogueLikeConfig>();
		if (option == LEGACY_LOOTBOX)
			return config.LegacyLootBoxDrop;
		if (option == LEGACY_WORLDGEN)
			return config.LegacyBossRushWorldGen;
		return false;
	}
	public static bool Check_RLOH() => ModContent.GetInstance<RogueLikeConfig>().RoguelikeOverhaul;

	public static bool Check_TotalRNG() => ModContent.GetInstance<RogueLikeConfig>().TotalRNG;


	public const string CHECK_LOSTACC = "lostacc";
	public const string CHECK_RARELOOTBOX = "lootboxrare";
	public const string CHECK_RARESPOILS = "rarespoil";
	public const string CHECK_WWEAPONENCHANT = "weaponenchant";
	public const string CHECK_RELICRANDOMVALUE = "relicvalue";
	public const string CHECK_PREFIX = "prefix";
	/// <summary>
	/// Check config setting
	/// </summary>
	/// <param name="option">use <see cref="CHECK_LOSTACC"/> and other related string</param>
	/// <returns>
	///		return true if config is enable<br/>
	///		return false if config is disable
	/// </returns>
	public static bool LuckDepartment(string option) {
		RogueLikeConfig config = ModContent.GetInstance<RogueLikeConfig>();
		if (option == CHECK_LOSTACC)
			return config.LostAccessory;
		if (option == CHECK_RARELOOTBOX)
			return config.RareLootbox;
		if (option == CHECK_RARESPOILS)
			return config.RareSpoils;
		if (option == CHECK_WWEAPONENCHANT)
			return config.WeaponEnchantment;
		if (option == CHECK_PREFIX)
			return config.AccessoryPrefix;
		return false;
	}
	internal UserInterface userInterface;
	internal UserInterface user2ndInterface;

	public EnchantmentUIState Enchant_uiState;
	public PerkUIState perkUIstate;
	public SkillUI skillUIstate;
	public DefaultUI defaultUI;
	public UISystemMenu UIsystemmenu;
	public TransmutationUIState transmutationUI;

	public RelicTransmuteUI relicUI;
	public SkillGetterUI skillUI;
	public SpoilGetterUI spoilUI;

	public SpoilsUIState spoilsState;
	public TeleportUI teleportUI;
	public InfoUI infoUI;
	public AchievementUI achievementUI;
	public StructureUI structUI;

	public static bool EnchantingState = false;
	private static string DirectoryPath => Path.Join(Program.SavePathShared, "RogueLikeData");
	private static string FilePath => Path.Join(DirectoryPath, "Data");
	public static ModKeybind WeaponActionKey { get; private set; }
	public TimeSpan timeBeatenTheGame = TimeSpan.Zero;
	public override void Load() {
		WeaponActionKey = KeybindLoader.RegisterKeybind(Mod, "Weapon action", Keys.X);
		try {
			if (File.Exists(FilePath)) {
				var tag = TagIO.FromFile(FilePath);
				FieldInfo[] fields = typeof(RoguelikeData).GetFields(BindingFlags.Static | BindingFlags.Public);
				foreach (var field in fields) {
					if (!tag.ContainsKey(field.Name)) {
						continue;
					}
					field.SetValue(null, tag[field.Name]);
				}
			}
		}
		catch {

		}


		GivenBossSpawnItem = new();
		//UI stuff
		if (!Main.dedServ) {
			//Mod custom UI
			Enchant_uiState = new();
			perkUIstate = new();
			skillUIstate = new();
			defaultUI = new();
			user2ndInterface = new();
			userInterface = new();
			UIsystemmenu = new();
			transmutationUI = new();
			relicUI = new();
			skillUI = new();
			spoilsState = new();
			teleportUI = new();
			infoUI = new();
			achievementUI = new();
			structUI = new();
			spoilUI = new();
		}
		On_UIElement.OnActivate += On_UIElement_OnActivate;
		On_WorldGen.StartHardmode += On_WorldGen_StartHardmode;
	}

	public override void Unload() {
		WeaponActionKey = null;
		if (!File.Exists(FilePath)) {
			if (!Directory.Exists(DirectoryPath)) {
				Directory.CreateDirectory(DirectoryPath);
			}

			File.Create(FilePath);
		}
		try {
			TagCompound tag = new();
			FieldInfo[] fields = typeof(RoguelikeData).GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (var field in fields) {
				tag.Set(field.Name, field.GetValue(null));
			}
			TagIO.ToFile(tag, FilePath);
		}
		catch {

		}
		InfoUI.InfoShowToItem = null;
		GivenBossSpawnItem = null;

		Enchant_uiState = null;
		perkUIstate = null;

		skillUIstate = null;
		defaultUI = null;

		userInterface = null;
		user2ndInterface = null;
		UIsystemmenu = null;
		transmutationUI = null;
		relicUI = null;
		skillUI = null;
		spoilsState = null;
		teleportUI = null;
		infoUI = null;
		achievementUI = null;
		structUI = null;
		spoilUI = null;
	}
	private void On_WorldGen_StartHardmode(On_WorldGen.orig_StartHardmode orig) {
		if (CanAccessContent(BOSSRUSH_MODE) && CheckLegacy(LEGACY_WORLDGEN) || !CanAccessContent(BOSSRUSH_MODE)) {
			orig();
		}
		Main.hardMode = true;
	}
	private void On_UIElement_OnActivate(On_UIElement.orig_OnActivate orig, UIElement self) {
		try {
			if (ModContent.GetInstance<RogueLikeConfig>().AutoRandomizeCharacter) {
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
	public override void UpdateUI(GameTime gameTime) {
		userInterface?.Update(gameTime);
		user2ndInterface?.Update(gameTime);
		if (infoUI != null) {
			InfoUI.InfoShowToItem = string.Empty;
			foreach (var item in infoUI.list_info) {
				if (item.StatePressed) {
					if (item.action != null) {
						item.action.Invoke();
						InfoUI.InfoShowToItem += item.text.Text + "\n";
					}
				}
			}
		}
	}
	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
		int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
		if (InventoryIndex != -1)
			layers.Insert(InventoryIndex, new LegacyGameInterfaceLayer(
				"BossRush: UI",
				delegate {
					GameTime gametime = new GameTime();
					userInterface.Draw(Main.spriteBatch, gametime);
					user2ndInterface.Draw(Main.spriteBatch, gametime);
					return true;
				},
				InterfaceScaleType.UI)
			);
	}
	public void ActivatePerkUI(short state, string extra = "") {
		perkUIstate.Info = extra;
		if (!Check_TotalRNG()) {
			DeactivateUI();
			perkUIstate.StateofState = state;
			user2ndInterface.SetState(perkUIstate);
			return;
		}
		if (state == PerkUIState.DefaultState) {
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
			AddPerk(perkType);
			BossRushUtils.CombatTextRevamp(Main.LocalPlayer.Hitbox, Color.AliceBlue, ModPerkLoader.GetPerk(perkType).DisplayName);
		}
		else if (state == PerkUIState.StarterPerkState) {
			int perkType = Main.rand.Next(PerkModSystem.StarterPerkType);
			AddPerk(perkType);
			BossRushUtils.CombatTextRevamp(Main.LocalPlayer.Hitbox, Color.AliceBlue, ModPerkLoader.GetPerk(perkType).DisplayName);
		}
	}
	public static bool CanEnchantmentBeAccess() => LuckDepartment(CHECK_WWEAPONENCHANT) && !Check_TotalRNG();
	public void ActivateStructureSaverUI() {
		DeactivateUI();
		user2ndInterface.SetState(structUI);
	}
	public void ActivateInfoUI() {
		DeactivateUI();
		user2ndInterface.SetState(infoUI);
	}
	public void ActivateSkillUI() {
		DeactivateUI();
		user2ndInterface.SetState(skillUIstate);
	}
	public void ActivateEnchantmentUI() {
		DeactivateUI();
		user2ndInterface.SetState(Enchant_uiState);
	}
	public void ActivateTransmutationUI() {
		DeactivateUI();
		user2ndInterface.SetState(transmutationUI);
	}
	public void ActivateDebugUI(string context = "relic") {
		DeactivateUI();
		if (context.Trim() == "relic") {
			user2ndInterface.SetState(relicUI);
		}
		if (context.Trim() == "skill") {
			user2ndInterface.SetState(skillUI);
		}
		if (context.Trim() == "spoil") {
			user2ndInterface.SetState(spoilUI);
		}
	}
	public void ActivateAchievementUI() {
		DeactivateUI();
		user2ndInterface.SetState(achievementUI);
	}
	/// <summary>
	/// Activate spoils ui state, it is required that lootboxtype come from lootbox item ID
	/// </summary>
	/// <param name="lootboxType">the lootbox item ID</param>
	/// <param name="IsReopening">set true to disable dupilicate lootbox</param>
	public void ActivateSpoilsUI(int lootboxType, bool IsReopening = false) {
		DeactivateUI();
		if (ChaosModeSystem.Chaos()) {
			if (Main.rand.NextFloat() <= Main.rand.NextFloat(.1f, .9f)) {
				return;
			}
		}
		if (Check_TotalRNG()) {
			List<ModSpoil> SpoilList = ModSpoilSystem.GetSpoilsList();
			for (int i = SpoilList.Count - 1; i >= 0; i--) {
				ModSpoil spoil = SpoilList[i];
				if (!spoil.IsSelectable(Main.LocalPlayer, ContentSamples.ItemsByType[lootboxType])) {
					SpoilList.Remove(spoil);
				}
			}
			Main.rand.Next(SpoilList).OnChoose(Main.LocalPlayer, lootboxType);
			return;
		}
		if (!IsReopening) {
			Main.LocalPlayer.GetModPlayer<SpoilsPlayer>().LootBoxSpoilThatIsNotOpen.Add(lootboxType);
		}

		user2ndInterface.SetState(spoilsState);
	}
	public void ActivateTeleportUI() {
		DeactivateUI();
		user2ndInterface.SetState(teleportUI);
	}
	public void DeactivateUI() {
		user2ndInterface.SetState(null);
	}
	public List<int> GivenBossSpawnItem = new List<int>();
	public List<int> ListOfBossKilled = new List<int>();
	public List<int> LootBoxOpen = new();
	public override void ClearWorld() {
		GivenBossSpawnItem = new List<int>();
		ListOfBossKilled = new();
		LootBoxOpen = new();
	}
	public override void SaveWorldData(TagCompound tag) {
		tag["GivenBossSpawnItem"] = GivenBossSpawnItem;
		tag["ListOfBossKilled"] = ListOfBossKilled;
		tag["LootBoxOpen"] = LootBoxOpen;
		if (timeBeatenTheGame != TimeSpan.Zero) {
			tag["TimeBeaten"] = timeBeatenTheGame;
		}
	}
	public override void LoadWorldData(TagCompound tag) {
		GivenBossSpawnItem = tag.Get<List<int>>("GivenBossSpawnItem");
		ListOfBossKilled = tag.Get<List<int>>("ListOfBossKilled");
		LootBoxOpen = tag.Get<List<int>>("LootBoxOpen");
		if (tag.TryGet("TimeBeaten", out TimeSpan time)) {
			timeBeatenTheGame = time;
		}
	}
	public static void AddPerk(int perkType) {
		UniversalSystem uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		PerkPlayer perkplayer = Main.LocalPlayer.GetModPlayer<PerkPlayer>();
		if (ModPerkLoader.GetPerk(perkType) != null) {
			if (ModPerkLoader.GetPerk(perkType).StackLimit == -1 && ModPerkLoader.GetPerk(perkType).CanBeStack) {
				ModPerkLoader.GetPerk(perkType).OnChoose(perkplayer.Player);
				uiSystemInstance.DeactivateUI();
				return;
			}
		}
		if (perkplayer.perks.Count < 0 || !perkplayer.perks.ContainsKey(perkType))
			perkplayer.perks.Add(perkType, 1);
		else
			if (perkplayer.perks.ContainsKey(perkType) && ModPerkLoader.GetPerk(perkType).CanBeStack)
			perkplayer.perks[perkType]++;
		ModPerkLoader.GetPerk(perkType).OnChoose(perkplayer.Player);
		uiSystemInstance.DeactivateUI();
	}
	private static string cachedstringeffect = string.Empty;
	public static string GetRandomGlitchyNameEffect(int delay) {
		if ((int)Main.time % delay == 0) {
			int length = Main.rand.Next(20, 25);
			cachedstringeffect = string.Empty;
			for (int i = 0; i < length; i++) {
				byte b = (byte)(Main.rand.Next() % 256);
				cachedstringeffect += (char)b;
			}
		}
		return cachedstringeffect;
	}
	public bool IsAttemptingToBringItemToNewPlayer = false;
	public string WorldState = "";
	public override void OnWorldUnload() {
		WorldState = "Exited";
		var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		uiSystemInstance.DeactivateUI();
		timeBeatenTheGame = TimeSpan.Zero;
	}
}
public class TimeSerializer : TagSerializer<TimeSpan, TagCompound> {
	public override TagCompound Serialize(TimeSpan value) => new TagCompound {
		["Days"] = value.Days,
		["Hours"] = value.Hours,
		["Minutes"] = value.Minutes,
		["Seconds"] = value.Seconds,
		["MiliSeconds"] = value.Milliseconds,
	};

	public override TimeSpan Deserialize(TagCompound tag) {
		return new TimeSpan(tag.Get<int>("Days"), tag.Get<int>("Hours"), tag.Get<int>("Minutes"), tag.Get<int>("Seconds"), tag.Get<int>("MiliSeconds"));
	}
}
public class UniversalGlobalItem : GlobalItem {
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (!UniversalSystem.EnchantingState)
			return;
		for (int i = 0; i < tooltips.Count; i++) {
			if (tooltips[i].Name == "ItemName") {
				string tooltipText = Language.GetTextValue($"Mods.BossRush.SystemTooltip.WeaponEnchantment.None");
				if (EnchantmentLoader.GetEnchantmentItemID(item.type) != null) {
					tooltipText = EnchantmentLoader.GetEnchantmentItemID(item.type).Description;
				}
				tooltips[i].Text = tooltipText;
				continue;
			}
			tooltips[i].Hide();
		}
	}
}
public class UniversalModPlayer : ModPlayer {
	public override void OnEnterWorld() {
		var uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
		if (uiSystemInstance.IsAttemptingToBringItemToNewPlayer) {
			BossRushUtils.CombatTextRevamp(Player.Hitbox, Color.Yellow, "Trying to cheat huh ? that is not very nice");
			Vector2 randomSpamLocation = Main.rand.NextVector2CircularEdge(1500, 1500) + Player.Center;
			NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (int)randomSpamLocation.X, (int)randomSpamLocation.Y, ModContent.NPCType<ElderGuardian>());
		}
		uiSystemInstance.WorldState = "Entered";
		uiSystemInstance.DeactivateUI();
		uiSystemInstance.userInterface.SetState(uiSystemInstance.defaultUI);
		if (!UniversalSystem.CanAccessContent(Player, UniversalSystem.HARDCORE_MODE) && WarnAlready == 0) {
			WarnAlready = 1;
		}
	}
	int WarnAlready = 0;
	public override void SaveData(TagCompound tag) {
		tag.Add("WarnAlready", WarnAlready);
	}
	public override void LoadData(TagCompound tag) {
		WarnAlready = (int)tag["WarnAlready"];
	}
}
class DefaultUI : UIState {
	Roguelike_ProgressUIBar energyBar;
	Roguelike_ProgressUIBar energyCoolDownBar;
	Roguelike_ProgressUIBar energyCostBar;

	private UITextPanel<string> EndOfDemoPanel;
	private UITextPanel<string> EndOfDemoPanelClose;

	private UITextPanel<string> popUpWarning;
	private UITextPanel<string> popUpWarningClose;

	private UIImageButton staticticUI;

	private UITextBox timer;

	private UIImage itemUseTexture;
	private UITextBox totalDMG;
	private UITextBox totalHitTaken;
	private UITextBox dmgTaken;
	public void TurnOnEndOfDemoMessage() {
		Player player = Main.LocalPlayer;
		EndOfDemoPanel = new UITextPanel<string>(Language.GetTextValue($"Mods.BossRush.SystemTooltip.DemoEnding.Tooltip"));
		EndOfDemoPanel.Height.Set(500, 0);
		EndOfDemoPanel.HAlign = .5f;
		EndOfDemoPanel.VAlign = .5f;
		Append(EndOfDemoPanel);
		if (player.HeldItem.type != 0) {
			itemUseTexture = new(TextureAssets.Item[player.HeldItem.type]);
			itemUseTexture.Width.Set(64, 0);
			itemUseTexture.Height.Set(64, 0);
			itemUseTexture.HAlign = 0;
			itemUseTexture.VAlign = .2f;
			EndOfDemoPanel.Append(itemUseTexture);
		}
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		ulong dps = modplayer.DPStracker;
		totalDMG = new("Total damage dealt : 0");
		string damage = dps.ToString();
		totalDMG.UISetWidthHeight(0, 20);
		totalDMG.HAlign = 0f;
		totalDMG.VAlign = .3f;
		totalDMG.ShowInputTicker = false;
		totalDMG.SetTextMaxLength(999);
		totalDMG.SetText($"Total damage dealt : {damage}");
		EndOfDemoPanel.Append(totalDMG);

		totalHitTaken = new("Total hit taken : 0");
		string taken = modplayer.HitTakenCounter.ToString();
		totalHitTaken.UISetWidthHeight(0, 20);
		totalHitTaken.HAlign = 0f;
		totalHitTaken.VAlign = .4f;
		totalHitTaken.ShowInputTicker = false;
		totalHitTaken.SetTextMaxLength(999);
		totalHitTaken.SetText($"Total hit taken : {taken}");
		EndOfDemoPanel.Append(totalHitTaken);

		dmgTaken = new("Damage taken : 0");
		string dmgT = modplayer.DmgTaken.ToString();
		dmgTaken.UISetWidthHeight(0, 20);
		dmgTaken.HAlign = 0f;
		dmgTaken.VAlign = .5f;
		dmgTaken.ShowInputTicker = false;
		dmgTaken.SetTextMaxLength(999);
		dmgTaken.SetText($"Damage taken : {dmgT}");
		EndOfDemoPanel.Append(dmgTaken);

		EndOfDemoPanelClose = new UITextPanel<string>(Language.GetTextValue($"Mods.BossRush.SystemTooltip.DemoEnding.Close"));
		EndOfDemoPanelClose.HAlign = 1f;
		EndOfDemoPanelClose.VAlign = 1f;
		EndOfDemoPanelClose.OnLeftClick += EndOfDemoPanelClose_OnLeftClick;
		EndOfDemoPanel.Append(EndOfDemoPanelClose);
	}
	private void EndOfDemoPanelClose_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		EndOfDemoPanel.Remove();
		EndOfDemoPanelClose.Remove();
	}

	private ColorInfo colorChanging1;
	private ColorInfo colorChanging2;
	private ColorInfo colorchanging3;
	public override void OnInitialize() {
		CreateEnergyBar();
		CreateCoolDownBar();
		staticticUI = new UIImageButton(ModContent.Request<Texture2D>(BossRushTexture.MENU));
		staticticUI.UISetWidthHeight(32, 32);
		staticticUI.HAlign = .3f;
		staticticUI.VAlign = .02f;
		staticticUI.OnLeftClick += StaticticUI_OnLeftClick;
		staticticUI.SetVisibility(.7f, 1f);
		Append(staticticUI);
		colorChanging1 = new(new() { Color.DarkBlue, Color.LightCyan });
		colorChanging2 = new(new() { Color.LightCyan, Color.DarkBlue }, .5f);
		colorchanging3 = new(new() { Color.DarkRed, Color.Red });

		timer = new("00:00:00:00");
		timer.UISetWidthHeight(150, 20);
		timer.HAlign = .5f;
		timer.VAlign = .02f;
		timer.ShowInputTicker = false;
		Append(timer);
	}
	private void StaticticUI_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		UniversalSystem system = ModContent.GetInstance<UniversalSystem>();
		if (system.user2ndInterface.CurrentState != null) {
			return;
		}
		if (Main.LocalPlayer.GetModPlayer<SpoilsPlayer>().LootBoxSpoilThatIsNotOpen.Count > 0) {
			system.ActivateSpoilsUI(Main.LocalPlayer.GetModPlayer<SpoilsPlayer>().LootBoxSpoilThatIsNotOpen.First(), true);
			return;
		}
		if (system.user2ndInterface.CurrentState == null) {
			system.DeactivateUI();
			system.user2ndInterface.SetState(system.UIsystemmenu);
		}
		else {
			system.user2ndInterface.SetState(null);
		}
	}
	public override void OnActivate() {
		if (!UniversalSystem.CanAccessContent(Main.LocalPlayer, UniversalSystem.HARDCORE_MODE)) {
			string text = Language.GetTextValue($"Mods.BossRush.SystemTooltip.PopUpWarning.Tooltip");
			popUpWarning = new UITextPanel<string>(text);
			popUpWarning.Height.Set(66, 0);
			popUpWarning.HAlign = .5f;
			popUpWarning.VAlign = .5f;
			Append(popUpWarning);
			popUpWarningClose = new UITextPanel<string>(Language.GetTextValue($"Mods.BossRush.SystemTooltip.PopUpWarning.ClosingText"));
			popUpWarningClose.HAlign = .5f;
			popUpWarningClose.VAlign = .6f;
			popUpWarningClose.OnLeftClick += PopUpWarning_OnLeftClick;
			Append(popUpWarningClose);
		}
	}
	private void PopUpWarning_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		Elements.Remove(popUpWarning);
		Elements.Remove(popUpWarningClose);
	}

	private void CreateEnergyBar() {
		energyCostBar = new Roguelike_ProgressUIBar(null, Color.DarkRed, Color.DarkRed, "", .8f);
		energyCostBar.SetPosition(new Rectangle(-22, 0, 138, 34), new Rectangle(0, 40, 138, 34));
		energyCostBar.VAlign = .02f;
		energyCostBar.HAlign = .37f;
		energyCostBar.Width.Set(182, 0);
		energyCostBar.Height.Set(60, 0);
		energyCostBar.HideBar = true;
		energyCostBar.HideText = true;
		energyCostBar.OnUpdate += EnergyCostBar_OnUpdate;
		Append(energyCostBar);
		energyBar = new Roguelike_ProgressUIBar(null, Color.DarkBlue, Color.LightCyan, "0/0", .8f);
		energyBar.SetPosition(new Rectangle(-22, 0, 138, 34), new Rectangle(0, 40, 138, 34));
		energyBar.VAlign = .02f;
		energyBar.HAlign = .37f;
		energyBar.Width.Set(182, 0);
		energyBar.Height.Set(60, 0);
		energyBar.OnUpdate += EnergyBar_OnUpdate;
		Append(energyBar);
	}

	private void EnergyCostBar_OnUpdate(UIElement affectedElement) {
		var modPlayer = Main.LocalPlayer.GetModPlayer<SkillHandlePlayer>();
		energyCostBar.BarProgress = modPlayer.SimulateSkillCost() / (float)modPlayer.EnergyCap;
		energyCostBar.SetColorA(colorchanging3.MultiColor(5));
	}

	private void EnergyBar_OnUpdate(UIElement affectedElement) {
		var modPlayer = Main.LocalPlayer.GetModPlayer<SkillHandlePlayer>();
		energyBar.text.SetText($"Energy : {modPlayer.Energy}/{modPlayer.EnergyCap}");
		energyBar.BarProgress = modPlayer.Energy / (float)modPlayer.EnergyCap;
		energyBar.SetColorA(colorChanging1.MultiColor(5));
		energyBar.SetColorB(colorChanging2.MultiColor(5));
	}

	private void CreateCoolDownBar() {
		energyCoolDownBar = new Roguelike_ProgressUIBar(null, Color.Red, Color.Yellow, "0/0", .8f);
		energyCoolDownBar.VAlign = .02f;
		energyCoolDownBar.HAlign = .47f;
		energyCoolDownBar.SetPosition(new Rectangle(-22, 0, 138, 34), new Rectangle(0, 40, 138, 34));
		energyCoolDownBar.Width.Set(182, 0);
		energyCoolDownBar.Height.Set(60, 0);
		energyCoolDownBar.OnUpdate += EnergyCoolDownBar_OnUpdate;
		energyCoolDownBar.Hide = true;
		Append(energyCoolDownBar);
	}

	private void EnergyCoolDownBar_OnUpdate(UIElement affectedElement) {
		var modPlayer = Main.LocalPlayer.GetModPlayer<SkillHandlePlayer>();
		// Setting the text per tick to update and show our resource values.
		if (modPlayer.CoolDown > 0) {
			energyCoolDownBar.Hide = false;
			energyCoolDownBar.text.SetText($"CoolDown : {MathF.Round(modPlayer.CoolDown / 60f, 2)}");
		}
		else {
			energyCoolDownBar.DelayHide(120);
			energyCoolDownBar.text.SetText("");
		}
		energyCoolDownBar.BarProgress = modPlayer.CoolDown / (float)modPlayer.MaximumCoolDown;
	}
	public override void Update(GameTime gameTime) {
		TimeSpan time = Main.ActivePlayerFileData.GetPlayTime();
		UniversalSystem system = ModContent.GetInstance<UniversalSystem>();
		if (system.timeBeatenTheGame == TimeSpan.Zero) {
			if (UniversalSystem.DidPlayerBeatTheMod()) {
				ModContent.GetInstance<UniversalSystem>().timeBeatenTheGame = time;
			}
			string ToTimer =
				$"{time.Hours}" +
				$":{(time.Minutes >= 10 ? time.Minutes : "0" + time.Minutes)}" +
				$":{(time.Seconds >= 10 ? time.Seconds : "0" + time.Seconds)}" +
				$":{(time.Milliseconds >= 100 ? (time.Milliseconds >= 10 ? "0" + time.Milliseconds : time.Milliseconds) : "0" + time.Milliseconds)}";
			timer.SetText(ToTimer);
		}
		else {
			string ToTimer =
			$"{system.timeBeatenTheGame.Hours}" +
			$":{(system.timeBeatenTheGame.Minutes >= 10 ? system.timeBeatenTheGame.Minutes : "0" + system.timeBeatenTheGame.Minutes)}" +
			$":{(system.timeBeatenTheGame.Seconds >= 10 ? system.timeBeatenTheGame.Seconds : "0" + time.Seconds)}" +
			$":{(system.timeBeatenTheGame.Milliseconds >= 100 ? (system.timeBeatenTheGame.Milliseconds >= 10 ? system.timeBeatenTheGame.Milliseconds : system.timeBeatenTheGame.Milliseconds) : "0" + system.timeBeatenTheGame.Milliseconds)}";
			timer.SetText(ToTimer);
		}
		if (staticticUI.ContainsPoint(Main.MouseScreen)) {
			Player player = Main.LocalPlayer;
			if (player.GetModPlayer<SpoilsPlayer>().LootBoxSpoilThatIsNotOpen.Count > 0) {
				SpoilsPlayer spoilplayer = player.GetModPlayer<SpoilsPlayer>();
				string text = string.Format(Language.GetTextValue($"Mods.BossRush.SystemTooltip.Spoil.Tooltip"), spoilplayer.LootBoxSpoilThatIsNotOpen.Count);
				Main.instance.MouseText(text);
			}
			else {
				Main.instance.MouseText("Roguelike Menu");
			}
			Main.LocalPlayer.mouseInterface = true;
		}
		base.Update(gameTime);
	}
}
class UISystemMenu : UIState {
	UIPanel panel;
	UITextPanel<string> uitextpanel;
	UIImageButton open_skill_UI;
	UIImageButton open_Enchantment_UI;
	UIImageButton open_Transmutation_UI;
	UIImageButton open_AchievmentUI;
	bool EnchantmentHover = false;
	bool SkillHover = false;
	bool Transmutation = false;
	bool Achievement = false;
	private Asset<Texture2D> lockIcon;
	private float SetHAlign(float value) => MathHelper.Lerp(.43f, .57f, value / 3f);
	public override void OnInitialize() {
		panel = new UIPanel();
		panel.HAlign = .5f;
		panel.VAlign = .4f;
		panel.UISetWidthHeight(360, 80);
		Append(panel);

		uitextpanel = new UITextPanel<string>(" ");
		uitextpanel.HAlign = .5f;
		uitextpanel.VAlign = .7f;
		uitextpanel.UISetWidthHeight(450, 350);
		Append(uitextpanel);

		open_skill_UI = new UIImageButton(TextureAssets.InventoryBack);
		open_skill_UI.UISetWidthHeight(52, 52);
		open_skill_UI.VAlign = .4f;
		open_skill_UI.HAlign = SetHAlign(0);
		open_skill_UI.SetVisibility(1f, 67f);
		open_skill_UI.OnLeftClick += Open_skill_UI_OnLeftClick;
		open_skill_UI.OnUpdate += Open_skill_UI_OnUpdate;
		Append(open_skill_UI);


		open_Transmutation_UI = new UIImageButton(TextureAssets.InventoryBack);
		open_Transmutation_UI.UISetWidthHeight(52, 52);
		open_Transmutation_UI.VAlign = .4f;
		open_Transmutation_UI.HAlign = SetHAlign(1);
		open_Transmutation_UI.SetVisibility(1f, 67f);
		open_Transmutation_UI.OnLeftClick += Open_Transmutation_UI_OnLeftClick;
		open_Transmutation_UI.OnUpdate += Open_Transmutation_UI_OnUpdate;
		Append(open_Transmutation_UI);

		open_AchievmentUI = new(TextureAssets.InventoryBack);
		open_AchievmentUI.UISetWidthHeight(52, 52);
		open_AchievmentUI.VAlign = .4f;
		open_AchievmentUI.HAlign = SetHAlign(2);
		open_AchievmentUI.SetVisibility(1f, 67f);
		open_AchievmentUI.OnLeftClick += Open_AchievmentUI_OnLeftClick;
		open_AchievmentUI.OnUpdate += Open_AchievmentUI_OnUpdate;
		Append(open_AchievmentUI);

		lockIcon = ModContent.Request<Texture2D>("BossRush/Texture/UI/lock");
	}

	private void Open_AchievmentUI_OnUpdate(UIElement affectedElement) {
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (affectedElement.IsMouseHovering) {
			Achievement = true;
		}
		else {
			Achievement = false;
		}
	}
	private void Open_AchievmentUI_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		ModContent.GetInstance<UniversalSystem>().ActivateAchievementUI();
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (SkillHover) {
			Main.instance.MouseText("Skill inventory");
			uitextpanel.SetText(Language.GetTextValue($"Mods.BossRush.SystemTooltip.Skill.Tooltip"));
		}
		else if (Transmutation) {
			Main.instance.MouseText("Transmutation menu");
			uitextpanel.SetText(Language.GetTextValue($"Mods.BossRush.SystemTooltip.Transmutation.Tooltip"));
		}
		else if (Achievement) {
			Main.instance.MouseText("Achievement menu");
			uitextpanel.SetText(Language.GetTextValue($"Mods.BossRush.SystemTooltip.Achievement.Tooltip"));
		}
		else {
			uitextpanel.SetText("");
		}
	}


	private void Open_Transmutation_UI_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		ModContent.GetInstance<UniversalSystem>().ActivateTransmutationUI();
	}
	private void Open_Transmutation_UI_OnUpdate(UIElement affectedElement) {
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (affectedElement.IsMouseHovering) {
			Transmutation = true;
		}
		else {
			Transmutation = false;
		}
	}

	private void Open_skill_UI_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		ModContent.GetInstance<UniversalSystem>().ActivateSkillUI();
	}
	private void Open_skill_UI_OnUpdate(UIElement affectedElement) {
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (affectedElement.IsMouseHovering) {
			SkillHover = true;
		}
		else {
			SkillHover = false;
		}
	}
}
public class TeleportUI : UIState {
	public List<btn_Teleport> btn_List;
	public UITextPanel<string> panel;
	public override void OnInitialize() {
		panel = new UITextPanel<string>("Select place to teleport below");
		panel.HAlign = .5f;
		panel.VAlign = .3f;
		panel.UISetWidthHeight(150, 53);
		Append(panel);

		btn_List = new List<btn_Teleport>();
		Dictionary<int, short> stuffPreHM = new Dictionary<int, short> {
			{ ItemID.SlimeCrown, BiomeAreaID.Slime },
			{ ItemID.SuspiciousLookingEye, BiomeAreaID.FleshRealm },
			{ ItemID.WormFood, BiomeAreaID.Corruption },
			{ ItemID.BloodySpine, BiomeAreaID.Crimson },
			{ ModContent.ItemType<CursedDoll>(), BiomeAreaID.Dungeon },
			{ ItemID.Abeemination, BiomeAreaID.BeeNest },
			{ ItemID.DeerThing, BiomeAreaID.Tundra },
			{ ModContent.ItemType<WallOfFleshSpawner>(), BiomeAreaID.Underground }
		};

		for (int i = 0; i < stuffPreHM.Count; i++) {
			float Hvalue = MathHelper.Lerp(.3f, .7f, i / (float)(8 - 1));
			int keyvalue = stuffPreHM.Keys.ElementAt(i);
			btn_Teleport btn = new btn_Teleport(TextureAssets.InventoryBack, keyvalue, stuffPreHM[keyvalue]);
			btn.VAlign = .4f;
			btn.HAlign = Hvalue;
			btn_List.Add(btn);
			Append(btn);
		}
		if (Main.hardMode) {
			Dictionary<int, short> stuffHM = new Dictionary<int, short> {
				{ ItemID.QueenSlimeCrystal, BiomeAreaID.Hallow },
				{ ItemID.MechanicalSkull, BiomeAreaID.Hallow },
				{ ItemID.MechanicalWorm, BiomeAreaID.Hallow },
				{ ItemID.MechanicalEye, BiomeAreaID.Hallow },
				{ ModContent.ItemType<PlanteraSpawn>(), BiomeAreaID.Jungle },
				{ ItemID.LihzahrdPowerCell, BiomeAreaID.Jungle },
				{ ModContent.ItemType<LunaticTablet>(), BiomeAreaID.Dungeon },
				{ ItemID.EmpressButterfly, BiomeAreaID.Hallow },
				{ ItemID.TruffleWorm, BiomeAreaID.Ocean }
			};
			for (int i = 8; i < stuffHM.Count; i++) {
				float Hvalue = MathHelper.Lerp(.3f, .7f, i / (float)(8 - 1));
				int keyvalue = stuffHM.Keys.ElementAt(i);
				btn_Teleport btn = new btn_Teleport(TextureAssets.InventoryBack, keyvalue, stuffHM[keyvalue]);
				btn.VAlign = .6f;
				btn.HAlign = Hvalue;
				btn_List.Add(btn);
				Append(btn);
			}
		}
	}
	public override void OnActivate() {
		foreach (var item in btn_List) {
			item.Remove();
		}
		btn_List.Clear();
		Dictionary<int, short> stuffPreHM = new Dictionary<int, short> {
			{ ItemID.SlimeCrown, BiomeAreaID.Slime },
			{ ItemID.SuspiciousLookingEye, BiomeAreaID.FleshRealm },
			{ ItemID.WormFood, BiomeAreaID.Corruption },
			{ ItemID.BloodySpine, BiomeAreaID.Crimson },
			{ ModContent.ItemType<CursedDoll>(), BiomeAreaID.Dungeon },
			{ ItemID.Abeemination, BiomeAreaID.BeeNest },
			{ ItemID.DeerThing, BiomeAreaID.Tundra },
			{ ModContent.ItemType<WallOfFleshSpawner>(), BiomeAreaID.Underground }
		};
		for (int i = 0; i < stuffPreHM.Count; i++) {
			float Hvalue = MathHelper.Lerp(.3f, .7f, i / (float)(8 - 1));
			int keyvalue = stuffPreHM.Keys.ElementAt(i);
			btn_Teleport btn = new btn_Teleport(TextureAssets.InventoryBack, keyvalue, stuffPreHM[keyvalue]);
			btn.VAlign = .4f;
			btn.HAlign = Hvalue;
			btn_List.Add(btn);
			Append(btn);
		}
		if (Main.hardMode) {
			Dictionary<int, short> stuffHM = new Dictionary<int, short> {
				{ ItemID.QueenSlimeCrystal, BiomeAreaID.Hallow },
				{ ItemID.MechanicalSkull, BiomeAreaID.Hallow },
				{ ItemID.MechanicalWorm, BiomeAreaID.Hallow },
				{ ItemID.MechanicalEye, BiomeAreaID.Hallow },
				{ ModContent.ItemType<PlanteraSpawn>(), BiomeAreaID.Jungle },
				{ ItemID.LihzahrdPowerCell, BiomeAreaID.Jungle },
				{ ModContent.ItemType<LunaticTablet>(), BiomeAreaID.Dungeon },
				{ ItemID.EmpressButterfly, BiomeAreaID.Hallow },
				{ ItemID.TruffleWorm, BiomeAreaID.Ocean }
			};
			for (int i = 0; i < stuffHM.Count; i++) {
				float Hvalue = MathHelper.Lerp(.3f, .7f, i / (float)(stuffHM.Count - 1));
				int keyvalue = stuffHM.Keys.ElementAt(i);
				btn_Teleport btn = new btn_Teleport(TextureAssets.InventoryBack, keyvalue, stuffHM[keyvalue]);
				btn.VAlign = .6f;
				btn.HAlign = Hvalue;
				btn_List.Add(btn);
				Append(btn);
			}
		}
	}
}
public class btn_Teleport : UIImageButton {
	Texture2D tex;
	int bossitemid;
	short biomeid;
	public string ZoneText = "";
	public btn_Teleport(Asset<Texture2D> texture, int BossItemID, short biomeID) : base(texture) {
		bossitemid = BossItemID;
		biomeid = biomeID;
		tex = texture.Value;
	}

	public override void LeftClick(UIMouseEvent evt) {
		Player player = Main.LocalPlayer;
		BossRushWorldGen.FindSuitablePlaceToTeleport(player, biomeid, ModContent.GetInstance<BossRushWorldGen>().Room);
		if (!ModContent.GetInstance<UniversalSystem>().GivenBossSpawnItem.Contains(bossitemid)) {
			ModContent.GetInstance<UniversalSystem>().GivenBossSpawnItem.Add(bossitemid);
			player.QuickSpawnItem(player.GetSource_FromThis(), bossitemid);
		}
		ModContent.GetInstance<UniversalSystem>().DeactivateUI();
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		Main.instance.LoadItem(bossitemid);
		Texture2D texture1 = TextureAssets.Item[bossitemid].Value;
		Vector2 origin = texture1.Size() * .5f;
		Vector2 drawpos = GetInnerDimensions().Position() + tex.Size() * .5f;
		spriteBatch.Draw(texture1, drawpos, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (IsMouseHovering) {
			Main.instance.MouseText(RogueLikeWorldGen.BiomeID[biomeid]);
		}
		else {
			if (!Parent.Children.Where(e => e.IsMouseHovering).Any()) {
				Main.instance.MouseText("");
			}
		}
	}
}

public class AchievementUI : UIState {
	private const int Row = 10;
	UIPanel achievementSelectingPanel, headerPanel;
	Roguelike_WrapTextUIPanel textpanel_main;
	Roguelike_WrapTextUIPanel textpanel_bottom;
	public List<AchievementButton> btn_Achievement;
	public List<UITextPanel<string>> txt_Achievement;
	ExitUI exitbtn;
	public static string ActiveAchievement = "";
	public int State = 0;
	public int CurrentSelectedIndex = -1;
	UIPanel main;
	Roguelike_UIImageButton buttonLeft;
	Roguelike_UIImageButton buttonRight;
	UIPanel footerPanel;
	List<PageImage> pagnitation = new();
	Roguelike_UIImageButton tagTutorial;
	Roguelike_UIImageButton tagEasy;
	Roguelike_UIImageButton tagHard;
	Roguelike_UIImageButton tagExpert;
	Roguelike_UIImageButton tagMastery;
	Roguelike_UIImageButton tagChallenge;
	Roguelike_UIImageButton tagMisc;
	HashSet<AchievementTag> hash_tag = new();
	Asset<Texture2D> asset = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
	List<ModAchievement> lib_achievement = new();
	UIPanel currentSelectAchievement;
	AchievementHeaderPreview achievementheader;
	UITextPanel<string> txtachievementheader;
	public override void OnInitialize() {
		asset = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
		hash_tag = new();
		lib_achievement = new();

		main = new();
		main.HAlign = .5f;
		main.VAlign = .5f;
		main.UISetWidthHeight(700, 700);
		Append(main);

		headerPanel = new UIPanel();
		headerPanel.UISetWidthHeight(700, 72);
		headerPanel.HAlign = .5f;
		headerPanel.VAlign = .5f;
		headerPanel.MarginBottom = main.Height.Pixels + 80;
		Append(headerPanel);

		footerPanel = new();
		footerPanel.VAlign = 1;
		footerPanel.Width.Percent = 1;
		footerPanel.Height.Pixels = 72f;
		main.Append(footerPanel);

		textpanel_main = new Roguelike_WrapTextUIPanel("");
		textpanel_main.HAlign = 1;
		textpanel_main.VAlign = 1f;
		textpanel_main.MarginBottom = footerPanel.Height.Pixels + 5;
		textpanel_main.UISetWidthHeight(325, 510);
		main.Append(textpanel_main);

		currentSelectAchievement = new();
		currentSelectAchievement.HAlign = 1;
		currentSelectAchievement.MarginBottom = textpanel_main.Height.Pixels + 10;
		currentSelectAchievement.UISetWidthHeight(325, 80);
		main.Append(currentSelectAchievement);

		achievementheader = new(asset, "");
		achievementheader.UISetWidthHeight(52, 52);
		achievementheader.VAlign = .5f;
		currentSelectAchievement.Append(achievementheader);

		txtachievementheader = new("");
		txtachievementheader.VAlign = .5f;
		txtachievementheader.TextHAlign = .5f;
		txtachievementheader.MarginLeft = 60;
		txtachievementheader.Width.Precent = .8f;
		txtachievementheader.TextHAlign = .5f;
		txtachievementheader.TextScale = .8f;
		currentSelectAchievement.Append(txtachievementheader);

		achievementSelectingPanel = new UIPanel();
		achievementSelectingPanel.HAlign = 0;
		achievementSelectingPanel.VAlign = 0;
		achievementSelectingPanel.Width.Set(325, 0);
		achievementSelectingPanel.Height.Set(600, 0);
		achievementSelectingPanel.MarginRight = 100;
		main.Append(achievementSelectingPanel);

		exitbtn = new(asset);
		exitbtn.UISetWidthHeight(52, 52);
		exitbtn.HAlign = 1f;
		exitbtn.VAlign = .5f;
		headerPanel.Append(exitbtn);

		tagTutorial = new(asset);
		tagTutorial.HAlign = 0;
		tagTutorial.VAlign = .5f;
		tagTutorial.OnLeftClick += TagUniversal_OnLeftClick;
		headerPanel.Append(tagTutorial);

		tagEasy = new(asset);
		tagEasy.HAlign = .1f;
		tagEasy.VAlign = .5f;
		tagEasy.OnLeftClick += TagUniversal_OnLeftClick;
		headerPanel.Append(tagEasy);

		tagHard = new(asset);
		tagHard.HAlign = .2f;
		tagHard.VAlign = .5f;
		tagHard.OnLeftClick += TagUniversal_OnLeftClick;
		headerPanel.Append(tagHard);

		tagExpert = new(asset);
		tagExpert.HAlign = .3f;
		tagExpert.VAlign = .5f;
		tagExpert.OnLeftClick += TagUniversal_OnLeftClick;
		headerPanel.Append(tagExpert);

		tagMastery = new(asset);
		tagMastery.HAlign = .4f;
		tagMastery.VAlign = .5f;
		tagMastery.OnLeftClick += TagUniversal_OnLeftClick;
		headerPanel.Append(tagMastery);

		tagChallenge = new(asset);
		tagChallenge.HAlign = .5f;
		tagChallenge.VAlign = .5f;
		tagChallenge.OnLeftClick += TagUniversal_OnLeftClick;
		headerPanel.Append(tagChallenge);

		tagMisc = new(asset);
		tagMisc.HAlign = .6f;
		tagMisc.VAlign = .5f;
		tagMisc.OnLeftClick += TagUniversal_OnLeftClick;
		headerPanel.Append(tagMisc);

		btn_Achievement = new();
		txt_Achievement = new();
		for (int i = 0; i < Row; i++) {
			AchievementButton btn = new(asset, "");
			btn.VAlign = MathHelper.Lerp(0f, 1f, i / (Row - 1f));
			btn.UISetWidthHeight(52, 52);
			btn_Achievement.Add(btn);
			achievementSelectingPanel.Append(btn);

			UITextPanel<string> txt_panel = new("");
			txt_panel.MarginLeft = 60;
			txt_panel.Width.Precent = .8f;
			txt_panel.TextHAlign = .5f;
			txt_panel.TextScale = .8f;
			txt_panel.VAlign = btn.VAlign;
			txt_Achievement.Add(txt_panel);
			achievementSelectingPanel.Append(txt_panel);
		}

		textpanel_bottom = new Roguelike_WrapTextUIPanel("", .77f);
		textpanel_bottom.HAlign = 1f;
		textpanel_bottom.UISetWidthHeight(325, 135);
		textpanel_bottom.VAlign = 1f;
		textpanel_main.Append(textpanel_bottom);

		buttonLeft = new(asset);
		buttonLeft.HAlign = 0;
		buttonLeft.VAlign = 1f;
		buttonLeft.postTex = ModContent.Request<Texture2D>(BossRushTexture.Arrow_Left);
		buttonLeft.OnLeftClick += ButtonLeft_OnLeftClick;
		footerPanel.Append(buttonLeft);

		buttonRight = new(asset);
		buttonRight.HAlign = 1f;
		buttonRight.VAlign = 1f;
		buttonRight.postTex = ModContent.Request<Texture2D>(BossRushTexture.Arrow_Right);
		buttonRight.OnLeftClick += ButtonRight_OnLeftClick;
		footerPanel.Append(buttonRight);

		pagnitation = new();

	}
	int pageIndex = 0;
	int maxPage = 1;
	public void SetPageIndex(int index) {
		pageIndex = Math.Clamp(index, 0, maxPage);
	}
	private void TagUniversal_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (tagTutorial.UniqueId == listeningElement.UniqueId) {
			tagTutorial.SetVisibility(1, 1);
			if (!hash_tag.Add(AchievementTag.Tutorial)) {
				hash_tag.Remove(AchievementTag.Tutorial);
				tagTutorial.SetVisibility(1, .5f);
			}
		}
		else if (tagEasy.UniqueId == listeningElement.UniqueId) {
			tagEasy.SetVisibility(1, 1);
			if (!hash_tag.Add(AchievementTag.Easy)) {
				hash_tag.Remove(AchievementTag.Easy);
				tagEasy.SetVisibility(1, .5f);
			}
		}
		else if (tagHard.UniqueId == listeningElement.UniqueId) {
			tagHard.SetVisibility(1, 1);
			if (!hash_tag.Add(AchievementTag.Hard)) {
				hash_tag.Remove(AchievementTag.Hard);
				tagHard.SetVisibility(1, .5f);
			}
		}
		else if (tagExpert.UniqueId == listeningElement.UniqueId) {
			tagExpert.SetVisibility(1, 1);
			if (!hash_tag.Add(AchievementTag.Expert)) {
				hash_tag.Remove(AchievementTag.Expert);
				tagExpert.SetVisibility(1, .5f);
			}
		}
		else if (tagMastery.UniqueId == listeningElement.UniqueId) {
			tagMastery.SetVisibility(1, 1);
			if (!hash_tag.Add(AchievementTag.Mastery)) {
				hash_tag.Remove(AchievementTag.Mastery);
				tagMastery.SetVisibility(1, .5f);
			}
		}
		else if (tagChallenge.UniqueId == listeningElement.UniqueId) {
			tagChallenge.SetVisibility(1, 1);
			if (!hash_tag.Add(AchievementTag.Challenge)) {
				hash_tag.Remove(AchievementTag.Challenge);
				tagChallenge.SetVisibility(1, .5f);
			}
		}
		else if (tagMisc.UniqueId == listeningElement.UniqueId) {
			tagMisc.SetVisibility(1, 1);
			if (!hash_tag.Add(AchievementTag.Misc)) {
				hash_tag.Remove(AchievementTag.Misc);
				tagMisc.SetVisibility(1, .5f);
			}
		}
		lib_achievement.Clear();
		foreach (var item in AchievementSystem.Achievements) {
			if (hash_tag.Contains(item.DifficultyTag) || hash_tag.Contains(item.CategoryTag)) {
				lib_achievement.Add(item);
			}
		}

		for (int i = 0; i < btn_Achievement.Count; i++) {
			AchievementButton btn = btn_Achievement[i];
			if (lib_achievement.Count - 1 < i) {
				btn.achievementname = string.Empty;
				txt_Achievement[i].SetText("");
				continue;
			}
			ModAchievement achievement = lib_achievement[i];
			btn.SetAchievement(achievement.Name);
			txt_Achievement[i].SetText(achievement.DisplayName);
		}

		pageIndex = 0;
		maxPage = lib_achievement.Count / 10 + 1;
		foreach (var item in pagnitation) {
			footerPanel.RemoveChild(item);
		}
		pagnitation.Clear();
		float realpageamount = MathF.Ceiling(lib_achievement.Count / 10f);
		if (realpageamount <= 1) {
			return;
		}
		for (int i = 0; i < realpageamount; i++) {
			PageImage img = new(asset);
			if (maxPage == 1) {
				img.HAlign = .5f;
			}
			else {
				img.HAlign = MathHelper.Lerp(.1f, .9f, i / (realpageamount - 1f));
			}
			img.VAlign = .5f;
			img.OnLeftClick += Img_OnLeftClick;
			pagnitation.Add(img);
			footerPanel.Append(img);
		}
	}
	private void Img_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		SetPageIndex(pagnitation.Select(el => el.UniqueId).ToList().IndexOf(listeningElement.UniqueId));
		RefleshAchievementSelectionUIBaseOnPageIndex();
	}

	private void ButtonLeft_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (pageIndex > 0) {
			pageIndex--;
		}
		RefleshAchievementSelectionUIBaseOnPageIndex();
	}

	private void ButtonRight_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (pageIndex < maxPage) {
			pageIndex++;
		}
		RefleshAchievementSelectionUIBaseOnPageIndex();
	}
	public void RefleshAchievementSelectionUIBaseOnPageIndex() {
		if (pageIndex > maxPage || pageIndex < 0 || maxPage <= 1) {
			return;
		}
		int startingPoint = Row * pageIndex;
		for (int i = 0; i < btn_Achievement.Count; i++) {
			AchievementButton btn = btn_Achievement[i];
			int indexChecker = startingPoint + i;
			if (lib_achievement.Count - 1 < i || indexChecker >= lib_achievement.Count) {
				btn.achievementname = string.Empty;
				txt_Achievement[i].SetText("");
				continue;
			}
			ModAchievement achievement = lib_achievement[indexChecker];
			btn.SetAchievement(achievement.Name);
			txt_Achievement[i].SetText(achievement.DisplayName);
		}
	}

	public override void ScrollWheel(UIScrollWheelEvent evt) {
		//RowOffSet -= MathF.Sign(evt.ScrollWheelValue);
		//RowOffSet = Math.Clamp(RowOffSet, 0, Math.Max(AchievementSystem.Achievements.Count, Row));

		//for (int i = 0; i < AchievementSystem.Achievements.Count; i++) {
		//	if (i >= btn_Achievement.Count) {
		//		break;
		//	}
		//	btn_Achievement[i].SetAchievement("");
		//	if (i + RowOffSet >= AchievementSystem.Achievements.Count) {
		//		continue;
		//	}
		//	btn_Achievement[i].SetAchievement(AchievementSystem.Achievements[i + RowOffSet].Name);
		//}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		for (int i = 0; i < pagnitation.Count; i++) {
			var item = pagnitation[i];
			if (i == pageIndex) {
				item.toggled = true;
			}
			else {
				item.toggled = false;
			}
			if (item.IsMouseHovering) {
				Main.instance.MouseText("page " + (i + 1).ToString());
			}
		}
		if (tagTutorial.IsMouseHovering) {
			Main.instance.MouseText("Tutorial");
		}
		else if (tagEasy.IsMouseHovering) {
			Main.instance.MouseText("Easy");
		}
		else if (tagHard.IsMouseHovering) {
			Main.instance.MouseText("Hard");
		}
		else if (tagExpert.IsMouseHovering) {
			Main.instance.MouseText("Expert");
		}
		else if (tagMastery.IsMouseHovering) {
			Main.instance.MouseText("Mastery");
		}
		else if (tagChallenge.IsMouseHovering) {
			Main.instance.MouseText("Challenge");
		}
		else if (tagMisc.IsMouseHovering) {
			Main.instance.MouseText("Misc");
		}
		else if (buttonLeft.IsMouseHovering) {
			Main.instance.MouseText("Previous page");
		}
		else if (buttonRight.IsMouseHovering) {
			Main.instance.MouseText("Next page");
		}

		if (ActiveAchievement == "") {
			return;
		}
		//Main.NewText(CurrentSelectedIndex);
		ModAchievement achievement = AchievementSystem.GetAchievement(ActiveAchievement);
		if (achievement == null) {
			return;
		}
		//achievementName.SetText(achievement.DisplayName);
		string text = $"Description : {achievement.Description}";
		if (achievement.AdditionalConditionTipAfterAchieve && achievement.Achieved) {
			textpanel_bottom.SetText("Condition: " + achievement.ConditionTipAfterAchieve);
		}
		else {
			textpanel_bottom.SetText("Condition: " + achievement.ConditionTip);
		}
		text += "\nStatus : ";
		if (achievement.Achieved) {
			text += "Completed";
		}
		else {
			text += "Unfinished";
		}
		textpanel_main.SetText(text);
		achievementheader.SetAchievement(achievement.Name);
		txtachievementheader.SetText(achievement.DisplayName);
	}
}
public class PageImage : UIImage {
	public Asset<Texture2D> unselected;
	public Asset<Texture2D> selected;
	public bool CustomDraw = true;
	public bool toggled = false;
	public PageImage(Asset<Texture2D> texture) : base(texture) {
		unselected = ModContent.Request<Texture2D>(BossRushTexture.Page_StateUnselected);
		selected = ModContent.Request<Texture2D>(BossRushTexture.Page_StateSelected);
	}
	public override void Draw(SpriteBatch spriteBatch) {
		if (!CustomDraw) {
			base.Draw(spriteBatch);
		}
		else {
			Vector2 originalorigin = new Vector2(26, 26);
			Vector2 drawpos = this.GetInnerDimensions().Position() + originalorigin * .5f + originalorigin * .25f;
			Vector2 origin;
			if (!toggled) {
				origin = unselected.Value.Size() * .5f;
				drawpos += origin * .5f;
				spriteBatch.Draw(unselected.Value, drawpos, null, Color.White, 0, origin, 1, SpriteEffects.None, 1);
			}
			else {
				origin = selected.Value.Size() * .5f;
				drawpos += origin * .5f;
				spriteBatch.Draw(selected.Value, drawpos, null, Color.White, 0, origin, 1, SpriteEffects.None, 1);
			}
		}
	}
}
public class AchievementHeaderPreview : UIImageButton {
	public string achievementname;
	private ModAchievement achievement;
	Texture2D texture;
	Asset<Texture2D> Lock;
	Asset<Texture2D> achieved;
	public void SetAchievement(string name) {
		achievementname = name;
		achievement = AchievementSystem.GetAchievement(achievementname);
	}
	public AchievementHeaderPreview(Asset<Texture2D> texture, string achievementName) : base(texture) {
		this.texture = texture.Value;
		SetAchievement(achievementName);
		achieved = ModContent.Request<Texture2D>(BossRushTexture.CommonTextureStringPattern + "UI/complete");
		Lock = ModContent.Request<Texture2D>(BossRushTexture.Lock);
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (achievement != null) {
			this.SetVisibility(1f, 1f);
		}
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		string texturestring;
		bool checkAchievement = achievement != null;
		if (achievementname == string.Empty) {
			return;
		}
		if (checkAchievement) {
			texturestring = achievement.Texture;
			if (achievement.SpecialDraw) {
				achievement.Draw(this, spriteBatch);
				if (!achievement.Achieved) {
					Texture2D locktex = Lock.Value;
					Vector2 origin2 = locktex.Size() * .5f;
					Vector2 drawpos2 = this.GetDimensions().Position() + texture.Size() * .5f;
					spriteBatch.Draw(locktex, drawpos2, null, new Color(255, 255, 255) * .45f, 0, origin2, .9f, SpriteEffects.None, 0);
				}
				else {
					Texture2D achievetex = achieved.Value;
					Vector2 origin2 = achievetex.Size() * .5f;
					Vector2 drawpos2 = this.GetDimensions().Position() + texture.Size() * .5f;
					spriteBatch.Draw(achievetex, drawpos2.Add(0, 3.5f), null, Color.White, 0, origin2, .9f, SpriteEffects.None, 0);
				}
				return;
			}
		}
		else {
			texturestring = BossRushTexture.MissingTexture_Default;
		}
		Texture2D skilltexture = ModContent.Request<Texture2D>(texturestring).Value;
		Vector2 origin = skilltexture.Size() * .5f;
		float scaling = ScaleCalculation(texture.Size(), skilltexture.Size());
		Vector2 drawpos = this.GetDimensions().Position() + texture.Size() * .5f;
		spriteBatch.Draw(skilltexture, drawpos, null, new Color(255, 255, 255), 0, origin, scaling, SpriteEffects.None, 0);
		if (checkAchievement) {
			if (!achievement.Achieved) {
				Texture2D locktex = Lock.Value;
				origin = locktex.Size() * .5f;
				spriteBatch.Draw(locktex, drawpos, null, new Color(255, 255, 255) * .45f, 0, origin, .9f, SpriteEffects.None, 0);
			}
			else {
				Texture2D achievetex = achieved.Value;
				origin = achievetex.Size() * .5f;
				spriteBatch.Draw(achievetex, drawpos.Add(0, 3.5f), null, Color.White, 0, origin, .9f, SpriteEffects.None, 0);
			}
		}
	}
	private float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / (textureSize.Length() * 1.5f);
}
public class AchievementButton : UIImageButton {
	public string achievementname;
	private ModAchievement achievement;
	Texture2D texture;
	Asset<Texture2D> Lock;
	Asset<Texture2D> achieved;
	public void SetAchievement(string name) {
		achievementname = name;
		achievement = AchievementSystem.GetAchievement(achievementname);
	}
	public AchievementButton(Asset<Texture2D> texture, string achievementName) : base(texture) {
		this.texture = texture.Value;
		SetAchievement(achievementName);
		achieved = ModContent.Request<Texture2D>(BossRushTexture.CommonTextureStringPattern + "UI/complete");
		Lock = ModContent.Request<Texture2D>(BossRushTexture.Lock);
	}
	public override void LeftClick(UIMouseEvent evt) {
		AchievementUI.ActiveAchievement = achievementname;
		UniversalSystem uni = ModContent.GetInstance<UniversalSystem>();
		uni.achievementUI.State = 1;
		uni.achievementUI.CurrentSelectedIndex = uni.achievementUI.btn_Achievement.Select(el => el.UniqueId).ToList().IndexOf(UniqueId);
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (achievement != null) {
			this.SetVisibility(1f, 1f);
		}
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (IsMouseHovering) {
			Main.instance.MouseText(achievementname);
		}
		else {
			if (!Parent.Children.Where(e => e.IsMouseHovering).Any()) {
				Main.instance.MouseText("");
			}
		}
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		string texturestring;
		bool checkAchievement = achievement != null;
		if (achievementname == string.Empty) {
			return;
		}
		if (checkAchievement) {
			texturestring = achievement.Texture;
			if (achievement.SpecialDraw) {
				achievement.Draw(this, spriteBatch);
				if (!achievement.Achieved) {
					Texture2D locktex = Lock.Value;
					Vector2 origin2 = locktex.Size() * .5f;
					Vector2 drawpos2 = this.GetDimensions().Position() + texture.Size() * .5f;
					spriteBatch.Draw(locktex, drawpos2, null, new Color(255, 255, 255) * .45f, 0, origin2, .9f, SpriteEffects.None, 0);
				}
				else {
					Texture2D achievetex = achieved.Value;
					Vector2 origin2 = achievetex.Size() * .5f;
					Vector2 drawpos2 = this.GetDimensions().Position() + texture.Size() * .5f;
					spriteBatch.Draw(achievetex, drawpos2.Add(0, 3.5f), null, Color.White, 0, origin2, .9f, SpriteEffects.None, 0);
				}
				return;
			}
		}
		else {
			texturestring = BossRushTexture.MissingTexture_Default;
		}
		Texture2D skilltexture = ModContent.Request<Texture2D>(texturestring).Value;
		Vector2 origin = skilltexture.Size() * .5f;
		float scaling = ScaleCalculation(texture.Size(), skilltexture.Size());
		Vector2 drawpos = this.GetDimensions().Position() + texture.Size() * .5f;
		spriteBatch.Draw(skilltexture, drawpos, null, new Color(255, 255, 255), 0, origin, scaling, SpriteEffects.None, 0);
		if (checkAchievement) {
			if (!achievement.Achieved) {
				Texture2D locktex = Lock.Value;
				origin = locktex.Size() * .5f;
				spriteBatch.Draw(locktex, drawpos, null, new Color(255, 255, 255) * .45f, 0, origin, .9f, SpriteEffects.None, 0);
			}
			else {
				Texture2D achievetex = achieved.Value;
				origin = achievetex.Size() * .5f;
				spriteBatch.Draw(achievetex, drawpos.Add(0, 3.5f), null, Color.White, 0, origin, .9f, SpriteEffects.None, 0);
			}
		}
	}
	private float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / (textureSize.Length() * 1.5f);
}
public enum InputType {
	text,
	integer,
	number
}
public class CursesButtonMenu : UIImageButton {
	public CursesButtonMenu(Asset<Texture2D> texture) : base(texture) {
	}
}
public class CurseState : UIState {
	UIPanel panel;
	List<Roguelike_UIText> textlist;
	List<ModCurse> cursesLib;
	public override void OnInitialize() {
		panel = new();
		textlist = new();
		cursesLib = new();
	}
}
