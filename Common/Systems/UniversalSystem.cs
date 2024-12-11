using System;
using Terraria;
using System.IO;
using Terraria.UI;
using Terraria.ID;
using System.Linq;
using Terraria.Audio;
using ReLogic.Content;
using BossRush.Texture;
using System.Reflection;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using BossRush.Common.Utils;
using BossRush.Contents.NPCs;
using Terraria.DataStructures;
using BossRush.Contents.Perks;
using Microsoft.Xna.Framework;
using BossRush.Contents.Skill;
using BossRush.Common.General;
using BossRush.Contents.Items;
using System.Collections.Generic;
using BossRush.Common.ChallengeMode;
using BossRush.Contents.Items.Chest;
using Microsoft.Xna.Framework.Input;
using Terraria.GameContent.UI.States;
using BossRush.Common.WorldGenOverhaul;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using BossRush.Contents.Items.RelicItem;
using BossRush.Common.Systems.Achievement;
using BossRush.Contents.WeaponEnchantment;
using BossRush.Common.Systems.SpoilSystem;
using BossRush.Common.Systems.CursesSystem;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Common.Mode.DreamLikeWorldMode;
using BossRush.Contents.Items.Consumable.Potion;
using BossRush.Contents.Items.Consumable.Spawner;
using BossRush.Contents.Items.aDebugItem.RelicDebug;
using BossRush.Contents.Items.aDebugItem.SkillDebug;
using BossRush.Contents.Items.Consumable.SpecialReward;

namespace BossRush.Common.Systems;
public static class RoguelikeData {
	public static int Lootbox_AmountOpen = 0;
	public static int Run_Amount = 0;
}
/// <summary>
/// This not only include main stuff that make everything work but also contain some fixes to vanilla<br/>
/// Also, very unholy class, do not look into it
/// </summary>
internal class UniversalSystem : ModSystem {
	public static bool DidPlayerBeatTheMod() => Main.hardMode;
	public const string SYNERGY_MODE = "SynergyModeEnable";
	public const string BOSSRUSH_MODE = "ChallengeModeEnable";
	public const string NIGHTMARE_MODE = "NightmareEnable";
	public const string HELLISH_MODE = "HellishEnable";
	public const string CHAOS_MODE = "ChaosEnable";
	public const string HARDCORE_MODE = "Hardcore";
	public const string TRUE_MODE = "TrueMode";
	public const string SYNERGYFEVER_MODE = "SynergyFeverMode";
	/// <summary>
	/// Use this to universally lock content behind hardcore, it basically act like a wrapper for <see cref="RogueLikeConfig"/>
	/// </summary>
	/// <param name="player"></param>
	/// <param name="context">Use <see cref="BOSSRUSH_MODE"/> or any kind of mode that seem fit</param>
	/// <returns></returns>
	public static bool CanAccessContent(Player player, string context) {
		RogueLikeConfig config = ModContent.GetInstance<RogueLikeConfig>();
		if (context == SYNERGYFEVER_MODE)
			return config.SynergyMode && config.SynergyFeverMode;
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
		if (context == SYNERGY_MODE)
			return config.SynergyMode;
		if (context == TRUE_MODE)
			return config.SynergyMode && config.BossRushMode;
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
		if (context == SYNERGY_MODE)
			return config.SynergyMode;
		if (context == SYNERGYFEVER_MODE)
			return config.SynergyMode && config.SynergyFeverMode;
		if (context == TRUE_MODE)
			return config.SynergyMode && config.BossRushMode;
		return false;
	}
	public const string LEGACY_LOOTBOX = "lootbox";
	public const string LEGACY_WORLDGEN = "worldgen";
	public const string LEGACY_SPOIL = "spoil";
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
		if (option == LEGACY_SPOIL)
			return config.LegacySpoils;
		return false;
	}
	public static bool Check_RLOH() => ModContent.GetInstance<RogueLikeConfig>().RoguelikeOverhaul;

	public static bool Check_TotalRNG() => ModContent.GetInstance<RogueLikeConfig>().TotalRNG;


	public const string CHECK_LOSTACC = "lostacc";
	public const string CHECK_RARELOOTBOX = "lootboxrare";
	public const string CHECK_RARESPOILS = "rarespoil";
	public const string CHECK_WWEAPONENCHANT = "weaponenchant";
	public const string CHECK_RELICRANDOMVALUE = "relicvalue";

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

	public SpoilsUIState spoilsState;
	public TeleportUI teleportUI;
	public InfoUI infoUI;
	public AchievementUI achievementUI;
	public StructureUI structUI;

	public static bool EnchantingState = false;
	private static string DirectoryPath => Path.Join(Program.SavePathShared, "RogueLikeData");
	private static string FilePath => Path.Join(DirectoryPath, "Data");
	public static ModKeybind WeaponActionKey { get; private set; }
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
		}
		On_UIElement.OnActivate += On_UIElement_OnActivate;
		On_WorldGen.StartHardmode += On_WorldGen_StartHardmode;
		On_Main.DrawInterface += On_Main_DrawInterface;
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
	}
	private void On_Main_DrawInterface(On_Main.orig_DrawInterface orig, Main self, GameTime gameTime) {
		//Code source credit : Structure Helper
		SpriteBatch spriteBatch = Main.spriteBatch;

		if (Main.LocalPlayer.HeldItem.ModItem is StructureWand) {
			var wand = (Main.LocalPlayer.HeldItem.ModItem as StructureWand);

			spriteBatch.Begin(default, default, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

			Texture2D tex = ModContent.Request<Texture2D>("StructureHelper/corner").Value;
			Texture2D tex2 = ModContent.Request<Texture2D>("StructureHelper/box").Value;

			Point16 topLeft = wand.TopLeft;
			Point16 bottomRight = wand.BottomRight;

			bool drawPreview = true;

			if (wand.secondPoint) {
				var point1 = wand.point1;
				var point2 = (Main.MouseWorld / 16).ToPoint16();

				topLeft = new Point16(point1.X < point2.X ? point1.X : point2.X, point1.Y < point2.Y ? point1.Y : point2.Y);
				bottomRight = new Point16(point1.X > point2.X ? point1.X : point2.X, point1.Y > point2.Y ? point1.Y : point2.Y);
				int Width = bottomRight.X - topLeft.X - 1;
				int Height = bottomRight.Y - topLeft.Y - 1;

				var target = new Rectangle((int)(topLeft.X * 16 - Main.screenPosition.X), (int)(topLeft.Y * 16 - Main.screenPosition.Y), Width * 16 + 16, Height * 16 + 16);
				BossRushUtils.DrawOutline(spriteBatch, target, Color.Gold);
				spriteBatch.Draw(tex2, target, tex2.Frame(), Color.White * 0.15f);

				spriteBatch.Draw(tex, wand.point1.ToVector2() * 16 - Main.screenPosition, tex.Frame(), Color.Cyan, 0, tex.Frame().Size() / 2, 1, 0, 0);
				//spriteBatch.Draw(tex, point2.ToVector2() * 16 - Main.screenPosition, tex.Frame(), Color.White * 0.5f, 0, tex.Frame().Size() / 2, 1, 0, 0);
			}
			else if (wand.Ready) {
				int Width = bottomRight.X - topLeft.X - 1;
				int Height = bottomRight.Y - topLeft.Y - 1;

				var target = new Rectangle((int)(topLeft.X * 16 - Main.screenPosition.X), (int)(topLeft.Y * 16 - Main.screenPosition.Y), Width * 16 + 16, Height * 16 + 16);
				BossRushUtils.DrawOutline(spriteBatch, target, Color.Lerp(Color.Gold, Color.White, 0.5f + 0.5f * (float)System.Math.Sin(Main.GameUpdateCount * 0.2f)));
				spriteBatch.Draw(tex2, target, tex2.Frame(), Color.White * 0.15f);

				float scale1 = Vector2.Distance(Main.MouseWorld, wand.point1.ToVector2() * 16) < 32 ? 1.5f : 1f;
				spriteBatch.Draw(tex, wand.point1.ToVector2() * 16 - Main.screenPosition, tex.Frame(), Color.Cyan * scale1, 0, tex.Frame().Size() / 2, scale1, 0, 0);

				float scale2 = Vector2.Distance(Main.MouseWorld, wand.point2.ToVector2() * 16) < 32 ? 1.5f : 1f;
				spriteBatch.Draw(tex, wand.point2.ToVector2() * 16 - Main.screenPosition, tex.Frame(), Color.Red * scale2, 0, tex.Frame().Size() / 2, scale2, 0, 0);

				if (scale1 > 1 || scale2 > 1)
					drawPreview = false;
			}

			if (drawPreview) {
				var pos = (Main.MouseWorld / 16).ToPoint16();
				spriteBatch.Draw(tex, pos.ToVector2() * 16 - Main.screenPosition, tex.Frame(), Color.White * 0.5f, 0, tex.Frame().Size() / 2, 1, 0, 0);
			}

			spriteBatch.End();
		}

		orig(self, gameTime);
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
			int perkType = Main.rand.Next(TerrariaArrayID.StarterPerk);
			AddPerk(perkType);
			BossRushUtils.CombatTextRevamp(Main.LocalPlayer.Hitbox, Color.AliceBlue, ModPerkLoader.GetPerk(perkType).DisplayName);
		}
	}
	public void ActivateStructureSaverUI(Point16 TopLeft, Point16 BottomRight) {
		DeactivateUI();
		structUI.TopLeft = TopLeft;
		structUI.BottomRight = BottomRight;
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
	}
	public override void LoadWorldData(TagCompound tag) {
		GivenBossSpawnItem = tag.Get<List<int>>("GivenBossSpawnItem");
		ListOfBossKilled = tag.Get<List<int>>("ListOfBossKilled");
		LootBoxOpen = tag.Get<List<int>>("LootBoxOpen");
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
	}
}
public class UniversalGlobalBuff : GlobalBuff {
	public override void SetStaticDefaults() {
		//I am unsure why this is set to true
		Main.debuff[BuffID.Campfire] = false;
		Main.debuff[BuffID.Honey] = false;
		Main.debuff[BuffID.StarInBottle] = false;
		Main.debuff[BuffID.HeartLamp] = false;
		Main.debuff[BuffID.CatBast] = false;
		Main.debuff[BuffID.Sunflower] = false;
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

	public void TurnOnEndOfDemoMessage() {
		EndOfDemoPanel = new UITextPanel<string>(Language.GetTextValue($"Mods.BossRush.SystemTooltip.DemoEnding.Tooltip"));
		EndOfDemoPanel.Height.Set(66, 0);
		EndOfDemoPanel.HAlign = .5f;
		EndOfDemoPanel.VAlign = .5f;
		Append(EndOfDemoPanel);
		EndOfDemoPanelClose = new UITextPanel<string>(Language.GetTextValue($"Mods.BossRush.SystemTooltip.DemoEnding.Close"));
		EndOfDemoPanelClose.HAlign = .5f;
		EndOfDemoPanelClose.VAlign = .6f;
		EndOfDemoPanelClose.OnLeftClick += EndOfDemoPanelClose_OnLeftClick;
		Append(EndOfDemoPanelClose);
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
		Append(staticticUI);
		colorChanging1 = new(new() { Color.DarkBlue, Color.LightCyan });
		colorChanging2 = new(new() { Color.LightCyan, Color.DarkBlue }, .5f);
		colorchanging3 = new(new() { Color.DarkRed, Color.Red });
	}
	private void StaticticUI_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		UniversalSystem system = ModContent.GetInstance<UniversalSystem>();
		if (system.user2ndInterface.CurrentState != null) {
			return;
		}
		if (Main.LocalPlayer.GetModPlayer<SpoilsPlayer>().LootBoxSpoilThatIsNotOpen.Count > 0) {
			system.ActivateSpoilsUI(Main.LocalPlayer.GetModPlayer<SpoilsPlayer>().LootBoxSpoilThatIsNotOpen.First(), true);
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
	UIImageButton show_playerMod_Info;
	UIImageButton open_skill_UI;
	UIImageButton open_Enchantment_UI;
	UIImageButton open_Transmutation_UI;
	UIImageButton open_AchievmentUI;
	bool EnchantmentHover = false;
	bool SkillHover = false;
	bool InfoHover = false;
	bool Transmutation = false;
	bool Achievement = false;
	private Asset<Texture2D> lockIcon;
	private float SetHAlign(float value) => MathHelper.Lerp(.43f, .57f, value / 4f);
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

		show_playerMod_Info = new UIImageButton(TextureAssets.InventoryBack);
		show_playerMod_Info.UISetWidthHeight(52, 52);
		show_playerMod_Info.VAlign = .4f;
		show_playerMod_Info.HAlign = SetHAlign(0);
		show_playerMod_Info.SetVisibility(1f, 67f);
		show_playerMod_Info.OnLeftClick += Show_playerMod_Info_OnLeftClick;
		show_playerMod_Info.OnUpdate += Show_playerMod_Info_OnUpdate;
		Append(show_playerMod_Info);

		open_skill_UI = new UIImageButton(TextureAssets.InventoryBack);
		open_skill_UI.UISetWidthHeight(52, 52);
		open_skill_UI.VAlign = .4f;
		open_skill_UI.HAlign = SetHAlign(1);
		open_skill_UI.SetVisibility(1f, 67f);
		open_skill_UI.OnLeftClick += Open_skill_UI_OnLeftClick;
		open_skill_UI.OnUpdate += Open_skill_UI_OnUpdate;
		Append(open_skill_UI);

		open_Enchantment_UI = new UIImageButton(TextureAssets.InventoryBack);
		open_Enchantment_UI.UISetWidthHeight(52, 52);
		open_Enchantment_UI.VAlign = .4f;
		open_Enchantment_UI.HAlign = SetHAlign(2);
		open_Enchantment_UI.SetVisibility(1f, 67f);
		open_Enchantment_UI.OnLeftClick += Open_Enchantment_UI_OnLeftClick;
		open_Enchantment_UI.OnUpdate += Open_Enchantment_UI_OnUpdate;
		Append(open_Enchantment_UI);

		open_Transmutation_UI = new UIImageButton(TextureAssets.InventoryBack);
		open_Transmutation_UI.UISetWidthHeight(52, 52);
		open_Transmutation_UI.VAlign = .4f;
		open_Transmutation_UI.HAlign = SetHAlign(3);
		open_Transmutation_UI.SetVisibility(1f, 67f);
		open_Transmutation_UI.OnLeftClick += Open_Transmutation_UI_OnLeftClick;
		open_Transmutation_UI.OnUpdate += Open_Transmutation_UI_OnUpdate;
		Append(open_Transmutation_UI);

		open_AchievmentUI = new(TextureAssets.InventoryBack);
		open_AchievmentUI.UISetWidthHeight(52, 52);
		open_AchievmentUI.VAlign = .4f;
		open_AchievmentUI.HAlign = SetHAlign(4);
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

	private bool CanEnchantmentBeAccess() =>
		UniversalSystem.LuckDepartment(UniversalSystem.CHECK_WWEAPONENCHANT)
		&& !UniversalSystem.Check_TotalRNG();
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		if (!CanEnchantmentBeAccess()) {
			Vector2 pos = open_Enchantment_UI.GetDimensions().Position();
			Vector2 origin = -lockIcon.Size() * .1f;
			Main.EntitySpriteDraw(lockIcon.Value, pos, null, Color.White, 0, origin, 1, SpriteEffects.None);
		}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (EnchantmentHover) {
			Main.instance.MouseText("Enchantment Menu");
			if (CanEnchantmentBeAccess()) {
				uitextpanel.SetText(Language.GetTextValue($"Mods.BossRush.SystemTooltip.WeaponEnchantment.Tooltip"));
			}
			else {
				uitextpanel.SetText(Language.GetTextValue($"Mods.BossRush.SystemTooltip.BlockWeaponEnchantment.Tooltip"));
			}
		}
		else if (SkillHover) {
			Main.instance.MouseText("Skill inventory");
			uitextpanel.SetText(Language.GetTextValue($"Mods.BossRush.SystemTooltip.Skill.Tooltip"));
		}
		else if (InfoHover) {
			Main.instance.MouseText("Show player's info");
			uitextpanel.SetText(Language.GetTextValue($"Mods.BossRush.SystemTooltip.ShowPlayerInfo.Tooltip"));
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

	private void Open_Enchantment_UI_OnUpdate(UIElement affectedElement) {
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (affectedElement.IsMouseHovering) {
			EnchantmentHover = true;
		}
		else {
			EnchantmentHover = false;
		}
	}
	private void Open_Enchantment_UI_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		if (CanEnchantmentBeAccess()) {
			ModContent.GetInstance<UniversalSystem>().ActivateEnchantmentUI();
		}
	}

	private void Show_playerMod_Info_OnUpdate(UIElement affectedElement) {
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (affectedElement.IsMouseHovering) {
			InfoHover = true;
		}
		else {
			InfoHover = false;
		}
	}
	private void Show_playerMod_Info_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		ModContent.GetInstance<UniversalSystem>().ActivateInfoUI();
	}
}
class Info_ArtifactImage : Roguelike_UIImage {
	public Info_ArtifactImage(Asset<Texture2D> texture) : base(texture) {
	}
	public override void DrawImage(SpriteBatch spriteBatch) {
		Artifact artifact = Artifact.GetArtifact(Main.LocalPlayer.GetModPlayer<ArtifactPlayer>().ActiveArtifact);
		CalculatedStyle style = GetInnerDimensions();
		artifact.DrawInUI(spriteBatch, style);
	}
}
class InfoUI : UIState {
	UIPanel panel;
	Roguelike_WrapTextUIPanel textpanel;
	Roguelike_UITextPanel generalTextPanel;
	Dictionary<Roguelike_UIText, int> textlist;
	UIImageButton btn_Stats;
	UIImageButton btn_ModStats;
	UIImageButton btn_Artifact;
	Info_ArtifactImage Info_artifact;
	UIImageButton btn_Perks;
	ExitUI btn_Exit;
	int CurrentState = 0;
	public override void OnInitialize() {
		textlist = new Dictionary<Roguelike_UIText, int>();

		panel = new UIPanel();
		panel.HAlign = .35f;
		panel.VAlign = .5f;
		panel.UISetWidthHeight(100, 450);
		Append(panel);

		textpanel = new Roguelike_WrapTextUIPanel("");
		textpanel.HAlign = .53f;
		textpanel.VAlign = .5f;
		textpanel.UISetWidthHeight(450, 600);
		Append(textpanel);

		generalTextPanel = new Roguelike_UITextPanel("");
		generalTextPanel.UISetWidthHeight(10, 10);
		generalTextPanel.Hide = true;
		Append(generalTextPanel);

		btn_Stats = new UIImageButton(TextureAssets.InventoryBack);
		btn_Stats.HAlign = .5f;
		btn_Stats.VAlign = .1f;
		btn_Stats.UISetWidthHeight(52, 52);
		btn_Stats.OnLeftClick += Btn_Stats_OnLeftClick;
		btn_Stats.SetVisibility(1, 1);
		panel.Append(btn_Stats);

		btn_ModStats = new UIImageButton(TextureAssets.InventoryBack);
		btn_ModStats.HAlign = .5f;
		btn_ModStats.VAlign = MathHelper.Lerp(.1f, .9f, 1 / 4f);
		btn_ModStats.OnLeftClick += Btn_ModStats_OnLeftClick;
		btn_ModStats.UISetWidthHeight(52, 52);
		panel.Append(btn_ModStats);

		btn_Artifact = new UIImageButton(TextureAssets.InventoryBack);
		btn_Artifact.HAlign = .5f;
		btn_Artifact.VAlign = MathHelper.Lerp(.1f, .9f, 2 / 4f);
		btn_Artifact.UISetWidthHeight(52, 52);
		btn_Artifact.OnLeftClick += Btn_Artifact_OnLeftClick;
		panel.Append(btn_Artifact);

		Info_artifact = new Info_ArtifactImage(TextureAssets.InventoryBack);
		Info_artifact.HAlign = .38f;
		Info_artifact.VAlign = .22f;
		Info_artifact.Hide = true;
		Append(Info_artifact);

		btn_Perks = new UIImageButton(TextureAssets.InventoryBack);
		btn_Perks.HAlign = .5f;
		btn_Perks.VAlign = MathHelper.Lerp(.1f, .9f, 3 / 4f);
		btn_Perks.UISetWidthHeight(52, 52);
		btn_Perks.OnLeftClick += Btn_Perks_OnLeftClick;
		panel.Append(btn_Perks);

		btn_Exit = new ExitUI(TextureAssets.InventoryBack);
		btn_Exit.HAlign = .5f;
		btn_Exit.VAlign = .9f;
		btn_Exit.UISetWidthHeight(52, 52);
		panel.Append(btn_Exit);
	}

	private void Text_OnUpdate(UIElement affectedElement) {
		if (affectedElement.IsMouseHovering) {
			Roguelike_UIText text = textlist.Keys.Where(e => e.UniqueId == affectedElement.UniqueId).FirstOrDefault();
			if (text == null || text.Hide) {
				return;
			}
			generalTextPanel.Hide = false;
			int perkType = textlist[text];
			generalTextPanel.SetText(ModPerkLoader.GetPerk(perkType).Description);
			generalTextPanel.UISetPosition(Main.MouseScreen);
		}
	}

	private void Btn_Stats_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		foreach (var item in textlist.Keys) {
			item.Hide = true;
		}
		btn_Stats.SetVisibility(1, 1);
		btn_ModStats.SetVisibility(.7f, .6f);
		btn_Perks.SetVisibility(.7f, .6f);
		btn_Artifact.SetVisibility(.7f, .6f);
		CurrentState = 0;
		Info_artifact.Hide = true;
		generalTextPanel.Hide = true;
	}
	private void Btn_ModStats_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		foreach (var item in textlist.Keys) {
			item.Hide = true;
		}
		btn_ModStats.SetVisibility(1, 1);
		btn_Stats.SetVisibility(.7f, .6f);
		btn_Perks.SetVisibility(.7f, .6f);
		btn_Artifact.SetVisibility(.7f, .6f);
		CurrentState = 1;
		Info_artifact.Hide = true;
		generalTextPanel.Hide = true;
	}
	private void Btn_Artifact_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		foreach (var item in textlist.Keys) {
			item.Hide = true;
		}
		btn_Artifact.SetVisibility(1, 1);
		btn_ModStats.SetVisibility(.7f, .6f);
		btn_Perks.SetVisibility(.7f, .6f);
		btn_Stats.SetVisibility(.7f, .6f);
		CurrentState = 2;
		Info_artifact.Hide = false;
		generalTextPanel.Hide = true;
	}

	private void Btn_Perks_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		foreach (var item in textlist.Keys) {
			item.Hide = false;
		}
		btn_Perks.SetVisibility(1, 1);
		btn_ModStats.SetVisibility(.7f, .6f);
		btn_Stats.SetVisibility(.7f, .6f);
		btn_Artifact.SetVisibility(.7f, .6f);
		CurrentState = 3;
		Info_artifact.Hide = true;
	}
	public override void OnActivate() {
		foreach (var item in textlist.Keys) {
			textpanel.RemoveChild(item);
		}
		textlist.Clear();
		Player player = Main.LocalPlayer;
		var perkplayer = player.GetModPlayer<PerkPlayer>();
		int counter = 0;
		foreach (var perkType in perkplayer.perks.Keys) {
			if (ModPerkLoader.GetPerk(perkType) != null) {
				Roguelike_UIText text = new Roguelike_UIText(ModPerkLoader.GetPerk(perkType).DisplayName + $" | current stack : [{perkplayer.perks[perkType]}]");
				text.OnUpdate += Text_OnUpdate;
				text.Top.Pixels += 25 * counter;
				text.Hide = true;
				textpanel.Append(text);
				textlist.Add(text, perkType);
				counter++;
			}
		}
	}
	public override void Update(GameTime gameTime) {
		if (panel.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		var player = Main.LocalPlayer;
		string line;
		switch (CurrentState) {
			case 0:
				var statshandle = player.GetModPlayer<PlayerStatsHandle>();
				line =
					$"Melee Damage : {player.GetTotalDamage(DamageClass.Melee).ToFloatValue(100, 1) - 100}% Crit chance : {player.GetTotalCritChance(DamageClass.Melee)}%" +
					$"\nRange Damage : {player.GetTotalDamage(DamageClass.Ranged).ToFloatValue(100, 1) - 100}% Crit chance : {player.GetTotalCritChance(DamageClass.Ranged)}%" +
					$"\nMagic Damage : {player.GetTotalDamage(DamageClass.Magic).ToFloatValue(100, 1) - 100}% Crit chance : {player.GetTotalCritChance(DamageClass.Magic)}%" +
					$"\nSummon Damage : {player.GetTotalDamage(DamageClass.Summon).ToFloatValue(100, 1) - 100}% Crit chance : {player.GetTotalCritChance(DamageClass.Summon)}%" +
					$"\nGeneric Damage : {player.GetTotalDamage(DamageClass.Generic).ToFloatValue(100, 1) - 100}% Crit chance : {player.GetTotalCritChance(DamageClass.Generic)}%" +
					$"\nCrit damage : {Math.Round((statshandle.UpdateCritDamage.ApplyTo(1) + 1) * 100, 2)}%" +
					$"\nDamage bonus to undamaged NPC : {Math.Round((statshandle.UpdateFullHPDamage.ApplyTo(1) - 1) * 100, 2)}%" +
					$"\nAttack speed: {RelicTemplateLoader.RelicValueToPercentage(player.GetTotalAttackSpeed(DamageClass.Generic))}" +
					$"\nHealth regenaration : {player.lifeRegen}" +
					$"\nMana regenaration : {player.manaRegen}" +
					$"\nMana reduction : {player.manaCost}" +
					$"\nDefense effectiveness : {player.DefenseEffectiveness.Value}" +
					$"\nDamage reduction: {Math.Round(player.endurance * 100, 2)}%" +
					$"\nMovement speed : {Math.Round(player.moveSpeed, 2)}" +
					$"\nJump speed : {player.jumpSpeedBoost}" +
					$"\nMax minion : {player.maxMinions}" +
					$"\nMax sentry/turret : {player.maxTurrets}" +
					$"\nThorn : {player.thorns}";
				textpanel.SetText(line);
				break;
			case 1:
				var chestplayer = player.GetModPlayer<ChestLootDropPlayer>();
				var drugplayer = player.GetModPlayer<WonderDrugPlayer>();
				var nohitPlayer = player.GetModPlayer<NoHitPlayerHandle>();
				chestplayer.GetAmount();
				line =
					$"Amount drop : {chestplayer.DropModifier.ApplyTo(1)}" +
					$"\nAmount drop chest final weapon : {chestplayer.weaponAmount}" +
					$"\nAmount drop chest final potion type : {chestplayer.potionTypeAmount}" +
					$"\nAmount drop chest final potion amount : {chestplayer.potionNumAmount}" +
					$"\nMelee drop chance : {chestplayer.UpdateMeleeChanceMutilplier}" +
					$"\nRange drop chance : {chestplayer.UpdateRangeChanceMutilplier}" +
					$"\nMagic drop chance : {chestplayer.UpdateMagicChanceMutilplier}" +
					$"\nSummon drop chance : {chestplayer.UpdateSummonChanceMutilplier}" +
					$"\nWonder drug consumed rate : {drugplayer.DrugDealer}" +
					$"\nAmount boss no-hit : {nohitPlayer.BossNoHitNumber.Count}" +
					$"\nAmount boss don't-hit : {nohitPlayer.DontHitBossNumber.Count}";
				textpanel.SetText(line);
				break;
			case 2:
				var artifactplayer = player.GetModPlayer<ArtifactPlayer>();
				line = $"Current active artifact : {Artifact.GetArtifact(artifactplayer.ActiveArtifact).DisplayName}";
				line += $"\n{Artifact.GetArtifact(artifactplayer.ActiveArtifact).Description}";
				textpanel.SetText(line);
				break;
			case 3:
				foreach (var item in textlist.Keys) {
					item.Hide = false;
				}
				if (textlist.Keys.Where(e => !e.IsMouseHovering).Count() == textlist.Keys.Count) {
					generalTextPanel.Hide = true;
				}
				break;
			default:
				line = "";
				textpanel.SetText(line);
				break;
		}
		base.Update(gameTime);
	}
}

internal class SkillUI : UIState {
	public List<btn_SkillSlotHolder> skill = new List<btn_SkillSlotHolder>();
	public List<btn_SkillSlotHolder> inventory = new List<btn_SkillSlotHolder>();
	public ExitUI exitUI;
	public btn_SkillDeletion btn_delete;
	public const string UItype_SKILL = "skill";
	public const string UIType_INVENTORY = "inventory";
	public UIPanel panel;
	public UIText energyCostText;
	public UIText durationText;
	public UIText cooldownText;
	public override void OnInitialize() {
		panel = new UIPanel();
		Append(panel);
		energyCostText = new UIText("");
		Append(energyCostText);
		durationText = new UIText("");
		Append(durationText);
		cooldownText = new UIText("");
		Append(cooldownText);
	}

	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		SkillHandlePlayer modplayer = Main.LocalPlayer.GetModPlayer<SkillHandlePlayer>();
		modplayer.SkillStatTotal(out int energy, out int duration, out int cooldown);
		Color color = energy <= modplayer.EnergyCap ? Color.Green : Color.Red;
		energyCostText.SetText($"[c/{color.Hex3()}:Energy cost = {energy}]");
		durationText.SetText($"Duration = {MathF.Round(duration / 60f, 2)}s");
		cooldownText.SetText($"Cool down = {MathF.Round(cooldown / 60f, 2)}s");
	}

	private void ActivateSkillUI(Player player) {
		panel.UISetWidthHeight(200, 90);
		panel.Left.Pixels = 860;
		panel.Top.Pixels = 330;
		energyCostText.Top.Pixels = 349;
		energyCostText.Left.Pixels = 880;
		durationText.Top.Pixels = 370;
		durationText.Left.Pixels = 880;
		cooldownText.Top.Pixels = 390;
		cooldownText.Left.Pixels = 880;
		if (player.TryGetModPlayer(out SkillHandlePlayer modplayer)) {
			//Explain : since most likely in the future we aren't gonna expand the skill slot, we just hard set it to 10
			//We are also pre render these UI first
			int[] SkillHolder = modplayer.GetCurrentActiveSkillHolder();
			Vector2 textureSize = new Vector2(52, 52);
			Vector2 OffSetPosition_Skill = player.Center;
			OffSetPosition_Skill.X -= textureSize.X * 5;
			if (skill.Count < 1) {
				Vector2 customOffSet = OffSetPosition_Skill;
				customOffSet.Y -= 60;
				for (int i = 0; i < 3; i++) {
					btn_SkillSlotSelection btn_Selection = new btn_SkillSlotSelection(TextureAssets.InventoryBack7, i + 1);
					btn_Selection.UISetPosition(customOffSet + new Vector2(52, 0) * i, textureSize);
					Append(btn_Selection);
				}
				for (int i = 0; i < 10; i++) {
					btn_SkillSlotHolder skillslot = new btn_SkillSlotHolder(TextureAssets.InventoryBack17, i, SkillHolder[i], UItype_SKILL);
					skillslot.UISetPosition(OffSetPosition_Skill + new Vector2(52, 0) * i, textureSize);
					skill.Add(skillslot);
					Append(skill[i]);
				}
			}
			if (inventory.Count < 1) {
				Vector2 InvOffSet = new Vector2(520, -55);
				for (int i = 0; i < 30; i++) {
					btn_SkillSlotHolder skillslot = new btn_SkillSlotHolder(TextureAssets.InventoryBack, i, modplayer.SkillInventory[i], UIType_INVENTORY);
					Vector2 InvPos = OffSetPosition_Skill + new Vector2(0, 72);
					if (i >= 10) {
						InvPos -= InvOffSet;
					}
					if (i >= 20) {
						InvPos -= InvOffSet;
					}
					skillslot.UISetPosition(InvPos + new Vector2(52, 0) * i, textureSize);
					inventory.Add(skillslot);
					Append(inventory[i]);
				}
			}
			if (exitUI == null) {
				exitUI = new ExitUI(TextureAssets.InventoryBack10);
				exitUI.UISetPosition(player.Center + new Vector2(275, 0), textureSize);
				Append(exitUI);
			}
			if (btn_delete == null) {
				btn_delete = new btn_SkillDeletion(TextureAssets.InventoryBack, modplayer);
				btn_delete.UISetPosition(player.Center - new Vector2(330, 0), textureSize);
				Append(btn_delete);
			}
		}
	}
	public override void OnActivate() {
		Player player = Main.LocalPlayer;
		ActivateSkillUI(player);
	}
	public override void OnDeactivate() {
		SkillModSystem.SelectInventoryIndex = -1;
		SkillModSystem.SelectSkillIndex = -1;
	}
}
class btn_SkillSlotSelection : UIImage {
	int SelectionIndex = 0;
	public btn_SkillSlotSelection(Asset<Texture2D> texture, int selection) : base(texture) {
		SelectionIndex = selection;
	}
	public override void LeftClick(UIMouseEvent evt) {
		base.LeftClick(evt);
		if (SelectionIndex == 0) {
			return;
		}
		Main.LocalPlayer.GetModPlayer<SkillHandlePlayer>().ChangeHolder(SelectionIndex);
	}
	public override void Draw(SpriteBatch spriteBatch) {
		if (SelectionIndex != Main.LocalPlayer.GetModPlayer<SkillHandlePlayer>().CurrentActiveIndex) {
			Color = new Color(255, 255, 255, 100);
		}
		else {
			Color = Color.White;
		}
		base.Draw(spriteBatch);
	}
}
class btn_SkillDeletion : UIImage {
	SkillHandlePlayer modplayer;
	Vector2 size;
	public btn_SkillDeletion(Asset<Texture2D> texture, SkillHandlePlayer modplayer) : base(texture) {
		this.modplayer = modplayer;
		size = texture.Size();
	}
	public override void LeftClick(UIMouseEvent evt) {
		if (SkillModSystem.SelectInventoryIndex != -1) {
			modplayer.RequestSkillRemoval_SkillInventory(SkillModSystem.SelectInventoryIndex);
			SkillModSystem.SelectInventoryIndex = -1;
		}
		if (SkillModSystem.SelectSkillIndex != -1) {
			modplayer.RequestSkillRemoval_SkillHolder(SkillModSystem.SelectSkillIndex);
			SkillModSystem.SelectSkillIndex = -1;
		}
	}
	public override void Update(GameTime gameTime) {
		if (IsMouseHovering) {
			Main.instance.MouseText(Language.GetTextValue($"Mods.BossRush.SystemTooltip.Skill.Delete"));
		}
		base.Update(gameTime);
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		Vector2 drawpos = new Vector2(Left.Pixels, Top.Pixels) + size * .5f;
		Texture2D trashbin = TextureAssets.Trash.Value;
		float scaling = ScaleCalculation(size, trashbin.Size());
		Vector2 origin = trashbin.Size() * .5f;
		spriteBatch.Draw(trashbin, drawpos, null, new Color(0, 0, 0, 150), 0, origin, scaling, SpriteEffects.None, 0);
	}
	private float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / (textureSize.Length() * 1.5f);
}
class btn_SkillSlotHolder : UIImageButton {
	public int whoAmI = -1;
	public int sKillID = -1;
	public string uitype = "";
	Texture2D Texture;
	public btn_SkillSlotHolder(Asset<Texture2D> texture, int WhoAmI, int SkillID, string UItype) : base(texture) {
		//player = Tplayer;
		whoAmI = WhoAmI;
		sKillID = SkillID;
		Texture = texture.Value;
		uitype = UItype;
		SetVisibility(1, .67f);
	}
	public override void LeftClick(UIMouseEvent evt) {
		Player player = Main.LocalPlayer;
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		//Moving skill around in inventory
		if (uitype == SkillUI.UIType_INVENTORY) {
			if (SkillModSystem.SelectInventoryIndex == -1) {
				if (SkillModSystem.SelectSkillIndex == -1) {
					//This mean the player haven't select anything
					SkillModSystem.SelectInventoryIndex = whoAmI;
				}
				else {
					//Player are Attempting to move a skill from their skill slot back to inventory
					modplayer.ReplaceSkillFromSkillHolderToInv(SkillModSystem.SelectSkillIndex, whoAmI);
					SkillModSystem.SelectSkillIndex = -1;
				}
			}
			else {
				//Player are moving skill around their inventory
				int cache = modplayer.SkillInventory[whoAmI];
				modplayer.SkillInventory[whoAmI] = modplayer.SkillInventory[SkillModSystem.SelectInventoryIndex];
				modplayer.SkillInventory[SkillModSystem.SelectInventoryIndex] = cache;
				SkillModSystem.SelectInventoryIndex = -1;
				//It is impossible where SelectSkillIndex can't be equal to -1
			}
		}
		else if (uitype == SkillUI.UItype_SKILL) {
			if (SkillModSystem.SelectSkillIndex == -1) {
				if (SkillModSystem.SelectInventoryIndex == -1) {
					//This mean the player haven't select anything
					SkillModSystem.SelectSkillIndex = whoAmI;
				}
				else {
					//Player are Attempting to move a skill from their inventory into a skill holder
					modplayer.ReplaceSkillFromInvToSkillHolder(whoAmI, SkillModSystem.SelectInventoryIndex);
					SkillModSystem.SelectInventoryIndex = -1;
				}
			}
			else {
				//Player are moving skill around their skill holder
				modplayer.SwitchSkill(whoAmI, SkillModSystem.SelectSkillIndex);
				SkillModSystem.SelectSkillIndex = -1;
			}
		}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		Player player = Main.LocalPlayer;
		SkillHandlePlayer modplayer = player.GetModPlayer<SkillHandlePlayer>();
		if (uitype == SkillUI.UIType_INVENTORY) {
			if (modplayer.SkillInventory[whoAmI] != sKillID) {
				sKillID = modplayer.SkillInventory[whoAmI];
			}
		}
		else if (uitype == SkillUI.UItype_SKILL) {
			int[] skillholder = modplayer.GetCurrentActiveSkillHolder();
			if (skillholder[whoAmI] != sKillID) {
				sKillID = skillholder[whoAmI];
			}
		}
		if (IsMouseHovering) {
			string tooltipText = "";
			string Name = "";
			if (SkillModSystem.GetSkill(sKillID) != null) {
				Name = SkillModSystem.GetSkill(sKillID).DisplayName;
				tooltipText = SkillModSystem.GetSkill(sKillID).Description;
				tooltipText +=
					$"\n[c/{Color.Yellow.Hex3()}:Skill duration] : {Math.Round(SkillModSystem.GetSkill(sKillID).Duration / 60f, 2)}s" +
					$"\n[c/{Color.DodgerBlue.Hex3()}:Energy require] : {SkillModSystem.GetSkill(sKillID).EnergyRequire}" +
					$"\n[c/{Color.OrangeRed.Hex3()}:Skill cooldown] : {Math.Round(SkillModSystem.GetSkill(sKillID).CoolDown / 60f, 2)}s";
			}
			Main.instance.MouseText(Name + "\n" + tooltipText);
		}
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		Vector2 drawpos = new Vector2(Left.Pixels, Top.Pixels) + Texture.Size() * .5f;
		if ((SkillModSystem.SelectInventoryIndex == whoAmI && uitype == SkillUI.UIType_INVENTORY)
			|| (SkillModSystem.SelectSkillIndex == whoAmI && uitype == SkillUI.UItype_SKILL)) {
			BossRushUtils.DrawAuraEffect(spriteBatch, Texture, drawpos, 2, 2, new Color(255, 255, 255, 100), 0, 1f);
		}
		if (sKillID < 0 || sKillID >= SkillModSystem.TotalCount) {
			return;
		}
		Texture2D skilltexture = ModContent.Request<Texture2D>(SkillModSystem.GetSkill(sKillID).Texture).Value;
		Vector2 origin = skilltexture.Size() * .5f;
		float scaling = ScaleCalculation(Texture.Size(), skilltexture.Size());
		spriteBatch.Draw(skilltexture, drawpos, null, new Color(255, 255, 255), 0, origin, scaling, SpriteEffects.None, 0);
	}
	private float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / (textureSize.Length() * 1.5f);
}
internal class PerkUIState : UIState {
	public string Info = "";
	public const short DefaultState = 0;
	public const short StarterPerkState = 1;
	public const short DebugState = 2;
	public const short GamblerState = 3;
	public short StateofState = 0;
	public UIText toolTip;
	public Roguelike_UIImageButton reroll = null;
	List<PerkUIImageButton> list_perkbtn = new();
	public override void OnInitialize() {
		reroll = new(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT));
		reroll.OnLeftClick += Reroll_OnLeftClick;
		reroll.OnUpdate += Reroll_OnUpdate;
		reroll.UISetWidthHeight(52, 52);
		reroll.HAlign = .5f;
		reroll.VAlign = .5f;
		Append(reroll);

		list_perkbtn = new();
		toolTip = new UIText("");
		Info = "";
		Append(toolTip);
	}

	private void Reroll_OnUpdate(UIElement affectedElement) {
		if (Main.LocalPlayer.GetModPlayer<PerkPlayer>().Reroll == 0) {
			reroll.Hide = true;
		}
		if (affectedElement.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (affectedElement.IsMouseHovering) {
			Main.instance.MouseText("Reroll Perk !");
		}
	}

	private void Reroll_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		SoundEngine.PlaySound(SoundID.Item35 with { Pitch = 1 });
		List<int> listOfPerk = new List<int>();
		Player player = Main.LocalPlayer;
		player.TryGetModPlayer(out PerkPlayer modplayer);
		if (StateofState == DefaultState) {
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
		}
		if (StateofState == StarterPerkState) {
			listOfPerk = [.. TerrariaArrayID.StarterPerk];
		}
		foreach (var item in list_perkbtn) {
			if (listOfPerk.Count < 1) {
				item.ChangePerkType(Main.rand.Next(new int[] { Perk.GetPerkType<SuppliesDrop>(), Perk.GetPerkType<GiftOfRelic>() }));
			}
			else {
				item.ChangePerkType(Main.rand.Next(listOfPerk));
			}
		}
		modplayer.Modify_RerollCount(1, true);
	}

	public override void OnActivate() {
		list_perkbtn.Clear();
		for (int i = Elements.Count - 1; i >= 0; i--) {
			if (Elements[i].UniqueId != reroll.UniqueId) {
				Elements[i].Remove();
			}
		}
		Player player = Main.LocalPlayer;
		if (player.TryGetModPlayer(out PerkPlayer modplayer)) {
			if (StateofState == DefaultState) {
				ActivateNormalPerkUI(modplayer, player);
			}
			if (StateofState == StarterPerkState) {
				ActivateStarterPerkUI(modplayer, player);
			}
			if (StateofState == DebugState) {
				ActivateDebugPerkUI(player);
			}
			if (StateofState == GamblerState) {
				ActivateGamblerUI(modplayer, player);
			}
		}
	}
	private void ActivateDebugPerkUI(Player player) {
		int amount = ModPerkLoader.TotalCount;
		Vector2 originDefault = new Vector2(26, 26);
		for (int i = 0; i < amount; i++) {
			Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(amount + 1, 360, i) * Math.Clamp(amount * 20, 0, 260);
			Asset<Texture2D> texture;
			if (ModPerkLoader.GetPerk(i).textureString is not null)
				texture = ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(i).textureString);
			else
				texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
			//After that we assign perk
			PerkUIImageButton btn = new PerkUIImageButton(texture);
			btn.UISetWidthHeight(52, 52);
			btn.UISetPosition(player.Center + offsetPos, originDefault);
			btn.perkType = i;
			Append(btn);
			ModPerkLoader.GetPerk(i);
		}
		reroll.Hide = true;
	}
	private void ActivateNormalPerkUI(PerkPlayer modplayer, Player player) {
		reroll.Hide = false;
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
		Vector2 originDefault = new Vector2(26, 26);
		int amount = listOfPerk.Count;
		int perkamount = modplayer.PerkAmountModified();
		for (int i = 0; i < perkamount; i++) {
			Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(perkamount, 360, i) * Math.Clamp(perkamount * 20, 0, 200);
			int newperk = Main.rand.Next(listOfPerk);
			Asset<Texture2D> texture;
			if (ModPerkLoader.GetPerk(newperk).textureString is not null)
				texture = ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(newperk).textureString);
			else
				texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
			if (i >= amount) {
				newperk = Main.rand.Next(new int[] { Perk.GetPerkType<SuppliesDrop>(), Perk.GetPerkType<GiftOfRelic>() });
				if (ModPerkLoader.GetPerk(newperk).textureString is not null)
					texture = ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(newperk).textureString);
				else
					texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
				PerkUIImageButton buttonWeapon = new PerkUIImageButton(texture);
				buttonWeapon.perkType = newperk;
				buttonWeapon.UISetWidthHeight(52, 52);
				buttonWeapon.UISetPosition(player.Center + offsetPos, originDefault);
				buttonWeapon.Info = Info;
				list_perkbtn.Add(buttonWeapon);
				Append(buttonWeapon);
				continue;
			}
			listOfPerk.Remove(newperk);
			//After that we assign perk
			PerkUIImageButton btn = new PerkUIImageButton(texture);
			btn.UISetWidthHeight(52, 52);
			btn.UISetPosition(player.Center + offsetPos, originDefault);
			btn.perkType = newperk;
			btn.Info = Info;
			list_perkbtn.Add(btn);
			Append(btn);
		}
	}
	private void ActivateStarterPerkUI(PerkPlayer modplayer, Player player) {
		reroll.Hide = false;
		Vector2 originDefault = new Vector2(26, 26);
		List<int> starterPerk = [.. TerrariaArrayID.StarterPerk];
		int limit = 3;
		for (int i = 0; i < limit; i++) {
			Perk choosenperk = ModPerkLoader.GetPerk(Main.rand.Next(starterPerk));
			starterPerk.Remove(choosenperk.Type);
			Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(limit, 360, i) * 120;
			//After that we assign perk
			if (modplayer.perks.ContainsKey(choosenperk.Type)) {
				if (modplayer.perks[choosenperk.Type] >= choosenperk.StackLimit) {
					continue;
				}
			}
			PerkUIImageButton btn = new PerkUIImageButton(ModContent.Request<Texture2D>(choosenperk.textureString));
			btn.UISetWidthHeight(52, 52);
			btn.UISetPosition(player.Center + offsetPos, originDefault);
			btn.perkType = choosenperk.Type;
			list_perkbtn.Add(btn);
			Append(btn);
		}
	}
	private void ActivateGamblerUI(PerkPlayer modplayer, Player player) {
		Vector2 originDefault = new Vector2(26, 26);
		int[] starterPerk = new int[]
		{ Perk.GetPerkType<UncertainStrike>(),
			Perk.GetPerkType<StrokeOfLuck>(),
			Perk.GetPerkType<BlessingOfPerk>()
		};
		for (int i = 0; i < starterPerk.Length; i++) {
			Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(starterPerk.Length, 360, i) * starterPerk.Length * 20;
			//After that we assign perk
			if (modplayer.perks.ContainsKey(starterPerk[i])) {
				if (modplayer.perks[starterPerk[i]] >= ModPerkLoader.GetPerk(starterPerk[i]).StackLimit) {
					continue;
				}
			}
			PerkUIImageButton btn = new PerkUIImageButton(ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(starterPerk[i]).textureString));
			btn.UISetWidthHeight(52, 52);
			btn.UISetPosition(player.Center + offsetPos, originDefault);
			btn.perkType = starterPerk[i];
			Append(btn);
		}
		reroll.Hide = true;
	}
}
//Do all the check in UI state since that is where the perk actually get create and choose
class PerkUIImageButton : UIImageButton {
	public int perkType;
	public string Info = "";
	private Asset<Texture2D> texture;
	public PerkUIImageButton(Asset<Texture2D> texture) : base(texture) {
		this.texture = texture;
	}
	public void ChangePerkType(int type) {
		perkType = type;
		Perk perk = ModPerkLoader.GetPerk(perkType);
		if (perk != null && perk.textureString != null) {
			texture = ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(perkType).textureString);
		}
		else {
			texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
		}
		SetImage(texture);
		this.UISetWidthHeight(52, 52);
	}
	public override void LeftClick(UIMouseEvent evt) {
		SoundEngine.PlaySound(SoundID.Item35 with { Pitch = -1 });
		UniversalSystem.AddPerk(perkType);
		if (Info == "Glitch") {
			Perk perk = ModPerkLoader.GetPerk(perkType);
			int stack = 0;
			if (perk.CanBeStack) {
				stack = Main.LocalPlayer.GetModPlayer<PerkPlayer>().perks[perkType];
			}
			int length = Math.Clamp(perk.StackLimit - stack, 0, 999999);
			for (int i = 0; i < length; i++) {
				UniversalSystem.AddPerk(perkType);
			}
		}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if(ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (IsMouseHovering && ModPerkLoader.GetPerk(perkType) != null) {
			Main.instance.MouseText(ModPerkLoader.GetPerk(perkType).DisplayName + "\n" + ModPerkLoader.GetPerk(perkType).ModifyToolTip());
		}
		else {
			if (!Parent.Children.Where(e => e.IsMouseHovering).Any()) {
				Main.instance.MouseText("");
			}
		}
	}
	public override void Draw(SpriteBatch spriteBatch) {
		if (Info == "Glitch") {
			spriteBatch.Draw(texture.Value, this.GetInnerDimensions().Position() + new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4)), null, Color.Red * .5f);
			spriteBatch.Draw(texture.Value, this.GetInnerDimensions().Position() + new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4)), null, Color.Blue * .5f);
		}
		base.Draw(spriteBatch);
	}
}
internal class EnchantmentUIState : UIState {
	UIPanel panel;
	WeaponEnchantmentUIslot weaponEnchantmentUIslot;
	ExitUI weaponEnchantmentUIExit;
	bool isMousePressed = false;
	Vector2 position = Main.ScreenSize.ToVector2() / 2f;
	Vector2 panelSize = new Vector2(70 * 3 - 8, 62 * 2 + 8);
	Vector2 UIclampOffset = new Vector2(60, 60);
	public override void OnInitialize() {
		panel = new UIPanel();
		panel.UISetPosition(Main.LocalPlayer.Center, panelSize / 2f);
		panel.OnLeftMouseDown += mousePressed;
		panel.OnLeftMouseUp += mouseUp;
		panel.UISetWidthHeight(panelSize.X, panelSize.Y);

		Append(panel);

		weaponEnchantmentUIslot = new WeaponEnchantmentUIslot(TextureAssets.InventoryBack);
		weaponEnchantmentUIslot.UISetWidthHeight(52, 52);
		weaponEnchantmentUIslot.UISetPosition(position + Vector2.UnitX * 120, new Vector2(26, 26));
		Append(weaponEnchantmentUIslot);
		weaponEnchantmentUIExit = new ExitUI(TextureAssets.InventoryBack13);
		weaponEnchantmentUIExit.UISetWidthHeight(52, 52);
		weaponEnchantmentUIExit.UISetPosition(position + Vector2.UnitX * 178, new Vector2(26, 26));
		Append(weaponEnchantmentUIExit);
	}

	public override void Update(GameTime gameTime) {
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		position = Vector2.Clamp(position, Vector2.Zero + UIclampOffset * Main.UIScale, Main.ScreenSize.ToVector2() - UIclampOffset * Main.UIScale);
		if (isMousePressed)
			this.position = Vector2.Clamp(Main.MouseScreen, Vector2.Zero + UIclampOffset * Main.UIScale, Main.ScreenSize.ToVector2() - UIclampOffset * Main.UIScale);
		for (int i = 0; i < Children.Count(); i++) {
			var children = Children.ElementAt(i);
			if (children is MoveableUIImage) {
				var child = children as MoveableUIImage;
				child.UISetPosition(position + child.positionOffset);
				child.position = position;
			}
		}
		panel.UISetPosition(position);
		weaponEnchantmentUIExit.UISetPosition(position + new Vector2(60, 0));
	}

	private void mousePressed(UIMouseEvent evt, UIElement listeningElement) {
		isMousePressed = true;
	}


	private void mouseUp(UIMouseEvent evt, UIElement listeningElement) {
		isMousePressed = false;
	}

	public override void OnDeactivate() {
		int count = Children.Count();
		for (int i = count - 1; i >= 0; i--) {
			UIElement child = Children.ElementAt(i);
			if (child is EnchantmentUIslot wmslot) {
				if (wmslot.itemOwner == null) {
					continue;
				}
				else {
					child.Deactivate();
					child.Remove();
				}
			}
			if (child is UIText) {
				child.Deactivate();
				child.Remove();
			}
		}
	}
}
public class MoveableUIImage : UIImage {
	public MoveableUIImage(Asset<Texture2D> texture) : base(texture) {
	}

	public Vector2 positionOffset = Vector2.Zero;
	public Vector2 position = Vector2.Zero;

}

public class WeaponEnchantmentUIslot : MoveableUIImage {
	public int WhoAmI = -1;
	public Texture2D textureDraw;
	public Item item;



	private Texture2D texture;
	public WeaponEnchantmentUIslot(Asset<Texture2D> texture) : base(texture) {
		this.texture = texture.Value;
	}
	List<int> textUqID = new List<int>();
	public override void LeftClick(UIMouseEvent evt) {
		Player player = Main.LocalPlayer;
		if (Main.mouseItem.type != ItemID.None) {
			if (Main.mouseItem.consumable)
				return;
			Item itemcached;
			if (item != null && item.type != ItemID.None) {
				itemcached = item.Clone();
				item = Main.mouseItem.Clone();
				Main.mouseItem = itemcached.Clone();
				player.inventory[58] = itemcached.Clone();
			}
			else {
				item = Main.mouseItem.Clone();
				Main.mouseItem.TurnToAir();
				player.inventory[58].TurnToAir();
				UniversalSystem.EnchantingState = true;
			}
			if (item.TryGetGlobalItem(out EnchantmentGlobalItem globalItem)) {
				int length = globalItem.EnchantmenStlot.Length - 1;
				for (int i = 0; i < length; i++) {
					EnchantmentUIslot slot = new EnchantmentUIslot(TextureAssets.InventoryBack);
					slot.positionOffset = Vector2.UnitY * 60 + Vector2.UnitX * 60 * i;
					slot.UISetWidthHeight(52, 52);
					slot.WhoAmI = i;
					slot.itemOwner = item;
					slot.itemType = globalItem.EnchantmenStlot[i];
					Parent.Append(slot);
					UIText text = new UIText($"{i + 1}");
					text.UISetPosition(positionOffset + Vector2.UnitY * 56, new Vector2(26, 26));
					textUqID.Add(text.UniqueId);
					Parent.Append(text);
				}
			}
		}
		else {
			if (item == null)
				return;
			UniversalSystem.EnchantingState = false;
			Main.mouseItem = item;
			item = null;
			int count = Parent.Children.Count();
			for (int i = count - 1; i >= 0; i--) {
				UIElement child = Parent.Children.ElementAt(i);
				if (child is EnchantmentUIslot wmslot) {
					if (wmslot.itemOwner == null)
						continue;
				}
				if (child is EnchantmentUIslot { itemOwner: not null }) {
					child.Deactivate();
					child.Remove();
				}
				if (child is UIText text && textUqID.Contains(text.UniqueId)) {
					textUqID.Remove(text.UniqueId);
					child.Deactivate();
					child.Remove();
				}
			}
		}
	}
	public override void OnDeactivate() {
		textUqID.Clear();
		Player player = Main.LocalPlayer;
		UniversalSystem.EnchantingState = false;
		if (item == null)
			return;
		for (int i = 0; i < 50; i++) {
			if (player.CanItemSlotAccept(player.inventory[i], item)) {
				if (ModContent.GetInstance<UniversalSystem>().WorldState == "Exited") {
					ModContent.GetInstance<UniversalSystem>().IsAttemptingToBringItemToNewPlayer = true;
					return;
				}
				player.inventory[i] = item.Clone();
				item = null;
				return;
			}
		}
		player.DropItem(player.GetSource_DropAsItem(), player.Center, ref item);
		item = null;
	}
	public override void Draw(SpriteBatch spriteBatch) {
		Vector2 drawpos = position + positionOffset + texture.Size() * .5f;
		base.Draw(spriteBatch);
		if (item != null) {
			Main.instance.LoadItem(item.type);
			Texture2D texture = TextureAssets.Item[item.type].Value;
			Vector2 origin = texture.Size() * .5f;
			float scaling = ScaleCalculation(texture.Size()) * .78f;
			spriteBatch.Draw(texture, drawpos, null, Color.White, 0, origin, scaling, SpriteEffects.None, 0);
		}
		else {

			Texture2D backgroundtexture = TextureAssets.Item[ItemID.SilverBroadsword].Value;
			spriteBatch.Draw(backgroundtexture, drawpos, null, new Color(0, 0, 0, 80), 0, texture.Size() * .35f, ScaleCalculation(backgroundtexture.Size()), SpriteEffects.None, 0);
		}
	}
	private float ScaleCalculation(Vector2 textureSize) => texture.Size().Length() / (textureSize.Length() * 1.25f);
}
public class EnchantmentUIslot : MoveableUIImage {
	public int itemType = 0;
	public int WhoAmI = -1;

	public Item itemOwner = null;
	private Texture2D texture;
	public EnchantmentUIslot(Asset<Texture2D> texture) : base(texture) {
		this.texture = texture.Value;
	}
	public override void LeftClick(UIMouseEvent evt) {
		if (itemOwner == null)
			return;
		if (Main.mouseItem.type != ItemID.None) {
			if (Main.mouseItem.consumable)
				return;
			if (itemType != 0)
				return;
			if (EnchantmentLoader.GetEnchantmentItemID(Main.mouseItem.type) == null)
				return;
			itemType = Main.mouseItem.type;
			Main.mouseItem.TurnToAir();
			Main.LocalPlayer.inventory[58].TurnToAir();
			EnchantmentSystem.EnchantItem(ref itemOwner, WhoAmI, itemType);
		}
	}
	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		try {
			if (itemOwner == null)
				return;
			if (itemType != 0) {
				Vector2 drawpos = new Vector2(Left.Pixels, Top.Pixels) + texture.Size() * .5f;
				Main.instance.LoadItem(itemType);
				Texture2D texture1 = TextureAssets.Item[itemType].Value;
				Vector2 origin = texture1.Size() * .5f;
				spriteBatch.Draw(texture1, drawpos, null, Color.White, 0, origin, .87f, SpriteEffects.None, 0);
			}
		}
		catch (Exception ex) {
			Main.NewText(ex.Message);
		}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if(ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (itemType == ItemID.None)
			return;
		if (IsMouseHovering) {
			string tooltipText = "No enchantment can be found";
			if (EnchantmentLoader.GetEnchantmentItemID(itemType) != null) {
				tooltipText = EnchantmentLoader.GetEnchantmentItemID(itemType).Description;
			}
			Main.instance.MouseText(tooltipText);
		}
		else {
			if (!Parent.Children.Where(e => e.IsMouseHovering).Any()) {
				Main.instance.MouseText("");
			}
		}
	}
}
public class SpoilsUIState : UIState {
	public int Limit_Spoils = 5;
	public List<SpoilsUIButton> btn_List;
	public int lootboxItem = -1;
	public UITextPanel<string> panel;
	public override void OnInitialize() {
		panel = new UITextPanel<string>(Language.GetTextValue($"Mods.BossRush.SystemTooltip.Spoil.Header"));
		panel.HAlign = .5f;
		panel.VAlign = .3f;
		panel.UISetWidthHeight(150, 53);
		Append(panel);
		Limit_Spoils = 5;
		btn_List = new List<SpoilsUIButton>();
	}
	public override void OnActivate() {
		btn_List.Clear();
		SpoilsPlayer modplayer = Main.LocalPlayer.GetModPlayer<SpoilsPlayer>();
		lootboxItem = modplayer.LootBoxSpoilThatIsNotOpen.FirstOrDefault();
		if (lootboxItem <= 0) {
			return;
		}
		Player player = Main.LocalPlayer;
		List<ModSpoil> SpoilList = ModSpoilSystem.GetSpoilsList();
		if (modplayer.SpoilsGift.Count > Limit_Spoils - 1 && modplayer.LootBoxSpoilThatIsNotOpen.Count > 0) {
			SpoilList.Clear();
			SpoilList = modplayer.SpoilsGift.Select(ModSpoilSystem.GetSpoils).ToList();
			modplayer.SpoilsGift.Clear();
		}
		else {
			modplayer.SpoilsGift.Clear();
			for (int i = SpoilList.Count - 1; i >= 0; i--) {
				ModSpoil spoil = SpoilList[i];
				if (!spoil.IsSelectable(player, ContentSamples.ItemsByType[lootboxItem])) {
					SpoilList.Remove(spoil);
				}
			}
		}
		if (SpoilList.Count < 1) {
			SpoilList = ModSpoilSystem.GetSpoilsList();
		}
		for (int i = 0; i < Limit_Spoils; i++) {
			ModSpoil spoil = Main.rand.Next(SpoilList);
			float Hvalue = MathHelper.Lerp(.3f, .7f, i / (float)(Limit_Spoils - 1));
			SpoilsUIButton btn = new SpoilsUIButton(TextureAssets.InventoryBack, spoil);
			modplayer.SpoilsGift.Add(spoil.Name);
			SpoilList.Remove(spoil);
			btn.HAlign = Hvalue;
			btn.VAlign = .4f;
			btn_List.Add(btn);
			Append(btn);
		}
		//SpoilsUIButton btna = new SpoilsUIButton(TextureAssets.InventoryBack10, null);
		//btna.HAlign = .7f;
		//btna.VAlign = .4f;
		//btn_List.Add(btna);
		//Append(btna);
	}
}
public class SpoilsUIButton : UIImageButton {
	public ModSpoil spoil;
	int LootboxItem = 0;
	public SpoilsUIButton(Asset<Texture2D> texture, ModSpoil Spoil) : base(texture) {
		spoil = Spoil;
		LootboxItem = Main.LocalPlayer.GetModPlayer<SpoilsPlayer>().LootBoxSpoilThatIsNotOpen.First();
	}
	public override void LeftClick(UIMouseEvent evt) {
		Player player = Main.LocalPlayer;
		SpoilsPlayer modplayer = player.GetModPlayer<SpoilsPlayer>();
		LootboxItem = modplayer.LootBoxSpoilThatIsNotOpen.First();
		if (spoil == null) {
			List<ModSpoil> SpoilList = ModSpoilSystem.GetSpoilsList();
			for (int i = SpoilList.Count - 1; i >= 0; i--) {
				ModSpoil spoil = SpoilList[i];
				if (!spoil.IsSelectable(player, ContentSamples.ItemsByType[LootboxItem])) {
					SpoilList.Remove(spoil);
				}
			}
			Main.rand.Next(SpoilList).OnChoose(player, LootboxItem);
			modplayer.LootBoxSpoilThatIsNotOpen.RemoveAt(0);
			modplayer.SpoilsGift.Clear();
			ModContent.GetInstance<UniversalSystem>().DeactivateUI();
			return;
		}
		spoil.OnChoose(player, LootboxItem);
		modplayer.LootBoxSpoilThatIsNotOpen.RemoveAt(0);
		modplayer.SpoilsGift.Clear();
		ModContent.GetInstance<UniversalSystem>().DeactivateUI();
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
		if (IsMouseHovering) {
			if (LootboxSystem.GetItemPool(LootboxItem) == null) {
				return;
			}
			if (spoil == null) {
				Main.instance.MouseText(Language.GetTextValue($"Mods.BossRush.SystemTooltip.Spoil.Randomize"));
			}
			else {
				Main.instance.MouseText(spoil.FinalDisplayName(), spoil.FinalDescription(), spoil.RareValue);
			}
		}
		else {
			if (!Parent.Children.Where(e => e.IsMouseHovering).Any()) {
				Main.instance.MouseText("");
			}
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
	UIPanel mainPanel, headerPanel;
	Roguelike_WrapTextUIPanel textpanel;
	Roguelike_WrapTextUIPanel conditiontextpanel;
	List<AchievementButton> btn_Achievement;
	ExitUI exitbtn;
	private int RowOffSet = 0;
	public static string ActiveAchievement = "";
	public override void OnInitialize() {
		mainPanel = new UIPanel();
		mainPanel.HAlign = .35f;
		mainPanel.VAlign = .5f;
		mainPanel.UISetWidthHeight(100, 600);
		Append(mainPanel);

		textpanel = new Roguelike_WrapTextUIPanel("");
		textpanel.HAlign = .53f;
		textpanel.VAlign = .5f;
		textpanel.UISetWidthHeight(450, 600);
		textpanel.offSetDraw.Y += 75;
		Append(textpanel);

		headerPanel = new UIPanel();
		headerPanel.UISetWidthHeight(450, 72);
		textpanel.Append(headerPanel);

		exitbtn = new(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT));
		exitbtn.UISetWidthHeight(52, 52);
		exitbtn.HAlign = 1f;
		exitbtn.VAlign = .5f;
		headerPanel.Append(exitbtn);

		btn_Achievement = new();
		for (int i = 0; i < Row; i++) {
			ModAchievement achievement = AchievementSystem.SafeGetAchievement(i);
			string text = "";
			if (achievement != null) {
				text = achievement.Name;
			}
			AchievementButton btn = new(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT), text);
			btn.HAlign = .5f;
			btn.VAlign = MathHelper.Lerp(0f, 1f, i / (Row - 1f));
			btn.UISetWidthHeight(52, 52);
			btn_Achievement.Add(btn);
			mainPanel.Append(btn);
		}

		conditiontextpanel = new Roguelike_WrapTextUIPanel("", .77f);
		conditiontextpanel.HAlign = .1f;
		conditiontextpanel.VAlign = 1f;
		conditiontextpanel.UISetWidthHeight(450, 100);
		textpanel.Append(conditiontextpanel);
	}
	public override void ScrollWheel(UIScrollWheelEvent evt) {
		RowOffSet -= MathF.Sign(evt.ScrollWheelValue);
		RowOffSet = Math.Clamp(RowOffSet, 0, Math.Max(AchievementSystem.Achievements.Count, Row));

		for (int i = 0; i < AchievementSystem.Achievements.Count; i++) {
			if (i >= btn_Achievement.Count) {
				break;
			}
			btn_Achievement[i].SetAchievement("");
			if (i + RowOffSet >= AchievementSystem.Achievements.Count) {
				continue;
			}
			btn_Achievement[i].SetAchievement(AchievementSystem.Achievements[i + RowOffSet].Name);
		}
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (ActiveAchievement == "") {
			return;
		}
		ModAchievement achievement = AchievementSystem.GetAchievement(ActiveAchievement);
		if (achievement == null) {
			return;
		}
		string text = $"Description : {achievement.Description}";
		if (achievement.AdditionalConditionTipAfterAchieve && achievement.Achieved) {
			conditiontextpanel.SetText("Condition: " + achievement.ConditionTipAfterAchieve);
		}
		else {
			conditiontextpanel.SetText("Condition: " + achievement.ConditionTip);
		}
		text += "\nStatus : ";
		if (achievement.Achieved) {
			text += "Completed";
		}
		else {
			text += "Unfinished";
		}
		textpanel.SetText(text);
	}
}
public class AchievementButton : UIImageButton {
	public string achievementname;
	private ModAchievement achievement;
	Texture2D texture;
	Asset<Texture2D> Lock;
	public void SetAchievement(string name) {
		achievementname = name;
		achievement = AchievementSystem.GetAchievement(achievementname);
		this.SetVisibility(.5f, .5f);
	}
	public AchievementButton(Asset<Texture2D> texture, string achievementName) : base(texture) {
		this.texture = texture.Value;
		SetAchievement(achievementName);
		Lock = ModContent.Request<Texture2D>(BossRushTexture.Lock);
	}
	public override void LeftClick(UIMouseEvent evt) {
		AchievementUI.ActiveAchievement = achievementname;
	}
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (achievement != null) {
			if (achievement.Achieved) {
				this.SetVisibility(1f, 1f);
			}
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
				return;
			}
		}
		else {
			texturestring = BossRushTexture.ACCESSORIESSLOT;
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
		}
	}
	private float ScaleCalculation(Vector2 originalTexture, Vector2 textureSize) => originalTexture.Length() / (textureSize.Length() * 1.5f);
}
public class StructureUI : UIState {
	public UIPanel panel;
	public UITextBox textBox;
	public UIImageButton btn_confirm;
	public UIImageButton btn_cancel;
	public UITextPanel<string> textPanel;
	public Point16 TopLeft = new Point16();
	public Point16 BottomRight = new Point16();
	public bool IsFocus = false;
	public int WidthStruct => BottomRight.X - TopLeft.X;
	public int HeightStruct => BottomRight.Y - TopLeft.Y;
	public override void OnInitialize() {
		panel = new();
		panel.HAlign = .5f;
		panel.VAlign = .5f;
		panel.UISetWidthHeight(450, 200);
		panel.OnUpdate += Panel_OnUpdate;
		Append(panel);

		textPanel = new("Save this structure ? Please name the file");
		textPanel.UISetWidthHeight(400, 40);
		textPanel.HAlign = .5f;
		textPanel.VAlign = .1f;
		panel.Append(textPanel);

		textBox = new("");
		textBox.HAlign = .5f;
		textBox.VAlign = .45f;
		textBox.UISetWidthHeight(400, 40);
		textBox.ShowInputTicker = true;
		textBox.TextHAlign = 0f;
		textBox.OnLeftClick += TextBox_OnLeftClick;
		panel.Append(textBox);

		btn_cancel = new(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT));
		btn_cancel.HAlign = 0f;
		btn_cancel.VAlign = 1f;
		btn_cancel.OnLeftClick += Btn_cancel_OnLeftClick;
		btn_cancel.UISetWidthHeight(52, 52);
		panel.Append(btn_cancel);

		btn_confirm = new(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT));
		btn_confirm.HAlign = 1f;
		btn_confirm.VAlign = 1f;
		btn_confirm.UISetWidthHeight(52, 52);
		btn_confirm.OnLeftClick += Btn_confirm_OnLeftClick;
		panel.Append(btn_confirm);
	}

	private void Panel_OnUpdate(UIElement affectedElement) {
		if (panel.ContainsPoint(Main.MouseScreen)) {
			Main.LocalPlayer.mouseInterface = true;
		}
	}

	private void TextBox_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		IsFocus = true;
	}

	private void Btn_cancel_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		ModContent.GetInstance<UniversalSystem>().DeactivateUI();
		Main.blockInput = false;
		PlayerInput.WritingText = false;
		textBox.SetText("");
	}

	private void Btn_confirm_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
		GenerationHelper.SaveToFile(new(TopLeft.X, TopLeft.Y, WidthStruct - 1, HeightStruct - 1), textBox.Text);
		textBox.SetText("");
		ModContent.GetInstance<UniversalSystem>().DeactivateUI();
	}
	int Delay = 0;
	bool shift = false;
	public override void Update(GameTime gameTime) {
		base.Update(gameTime);
		if (Main.keyState.IsKeyDown(Keys.Escape) || Main.mouseLeft && !IsMouseHovering)
			IsFocus = false;
		if (IsFocus) {
			Main.blockInput = true;
			textBox.ShowInputTicker = true;
			if (--Delay > 0) {
				return;
			}
			var list = PlayerInput.GetPressedKeys();
			if (list.Count > 0) {
				shift = false;
				Delay = BossRushUtils.ToSecond(.1f);
				Keys outKey = Keys.None;
				for (int i = 0; i < list.Count; i++) {
					Keys key = list[0];
					if (key == Keys.Back) {
						textBox.Backspace();
					}
					else if (key == Keys.Space) {
						textBox.Write(" ");
					}
					else {
						if (key == Keys.LeftShift) {
							shift = true;
							continue;
						}
						if (outKey != key) {
							outKey = key;
							continue;
						}
					}
				}
				if (outKey != Keys.None) {
					string c = outKey.ToString();
					if (shift) {
						textBox.Write(c);
					}
					else {
						textBox.Write(c.ToLower());
					}
				}
			}
		}
		else {
			Main.blockInput = false;
			textBox.ShowInputTicker = false;
		}

	}
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
