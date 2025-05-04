using Terraria;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using System.Collections.Generic;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using BossRush.Common.Systems.ArtifactSystem;
using Terraria.UI;
using System;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent;
using System.Linq;
using BossRush.Contents.Perks;
using BossRush.Contents.Items.RelicItem;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Consumable.Potion;
using BossRush.Contents.Items.Consumable.SpecialReward;
using BossRush.Contents.Transfixion.WeaponEnchantment;
using BossRush.Contents.Transfixion.Arguments;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Toggle {
	class UserInfoTablet : ModItem {
		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.autoReuse = false;
			Item.noUseGraphic = true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			if (string.IsNullOrEmpty(InfoUI.InfoShowToItem)) {
				return;
			}
			tooltips.ForEach((t) => { if (t.Name != "ItemName") t.Hide(); });
			tooltips.Add(new(Mod, "Stats", InfoUI.InfoShowToItem.Substring(0, InfoUI.InfoShowToItem.Length - 1)));
		}
		public override bool? UseItem(Player player) {
			if (player.ItemAnimationJustStarted) {
				ModContent.GetInstance<UniversalSystem>().ActivateInfoUI();
			}
			return base.UseItem(player);
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
	public class Roguelike_Info {
		public Roguelike_UIImageButton btn;
		public Roguelike_UIText text;
		public string Info = string.Empty;
		public bool StatePressed = false;
		public Action action;
		public int Index = -1;
		public Roguelike_Info(UIElement state) {
			btn = new(ModContent.Request<Texture2D>(BossRushTexture.PinIcon));
			btn.UISetWidthHeight(18, 18);
			btn.OnLeftClick += Btn_OnLeftClick;
			btn.OnUpdate += Btn_OnUpdate;
			state.Append(btn);

			text = new("");
			text.MarginLeft = 24;
			state.Append(text);
		}

		private void Btn_OnUpdate(UIElement affectedElement) {
			if (StatePressed) {
				InfoUI.InfoShowToItem += Info + "\n";
			}
			affectedElement.Disable_MouseItemUsesWhenHoverOverAUI();
		}

		private void Btn_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
			StatePressed = !StatePressed;
			if (StatePressed) {
				btn.SetVisibility(1f, 1f);
				text.TextColor = Color.Yellow;
			}
			else {
				btn.SetVisibility(1f, .4f);
				text.TextColor = Color.White;
			}
		}
		public void Hide(bool Hide) {
			text.Hide = Hide;
			btn.Hide = Hide;
		}
		public void SetAlign(float x, float y) {
			btn.HAlign = x;
			text.HAlign = x;
			btn.VAlign = y;
			text.VAlign = y;
		}
		public void SetInfo(string info) {
			Info = info;
			text.SetText(info);
		}
		public void Visibility(bool hide) {
			text.Hide = hide;
			btn.Hide = hide;
		}
	}
	class InfoUI : UIState {
		public static string InfoShowToItem = string.Empty;
		UIPanel mainPanel;
		UIPanel panel;
		Roguelike_WrapTextUIPanel textpanel;
		Roguelike_UITextPanel generalTextPanel;
		Dictionary<Roguelike_UIText, int> textlist;
		UIImageButton btn_Stats;
		UIImageButton btn_ModStats;
		UIImageButton btn_Perks;
		ExitUI btn_Exit;
		int CurrentState = 0;
		public List<Roguelike_Info> list_info = new();
		public override void OnInitialize() {
			GeneralPage_Initialization();

			float marginTopForButton = 0;

			//Normal stats
			BasicStats_Initialization(ref marginTopForButton);

			//Modded stats
			ModdedStats_Initialization(ref marginTopForButton);

			//Perk
			PerkPage_Initialization(ref marginTopForButton);

			//Artifact
			ArtifactPage_Initialization(ref marginTopForButton);

			btn_Exit = new ExitUI(TextureAssets.InventoryBack);
			btn_Exit.HAlign = .5f;
			btn_Exit.VAlign = 1;
			btn_Exit.UISetWidthHeight(52, 52);
			panel.Append(btn_Exit);
		}
		public void GeneralPage_Initialization() {
			mainPanel = new();
			mainPanel.HAlign = .5f;
			mainPanel.VAlign = .5f;
			mainPanel.UISetWidthHeight(700, 700);
			Append(mainPanel);

			textpanel = new Roguelike_WrapTextUIPanel("");
			textpanel.HAlign = 1;
			textpanel.VAlign = .5f;
			textpanel.Width.Pixels = 550;
			textpanel.Height.Precent = 1;
			mainPanel.Append(textpanel);

			panel = new UIPanel();
			panel.VAlign = .5f;
			panel.Width.Pixels = 100;
			panel.Height.Precent = 1;
			mainPanel.Append(panel);

			generalTextPanel = new Roguelike_UITextPanel("");
			generalTextPanel.UISetWidthHeight(10, 10);
			generalTextPanel.Hide = true;
			Append(generalTextPanel);
		}
		public void BasicStats_Initialization(ref float marginForBtn) {
			btn_Stats = new UIImageButton(TextureAssets.InventoryBack);
			btn_Stats.HAlign = .5f;
			btn_Stats.UISetWidthHeight(52, 52);
			marginForBtn += btn_Stats.Height.Pixels + 10;
			btn_Stats.OnLeftClick += Btn_Stats_OnLeftClick;
			btn_Stats.SetVisibility(1, 1);
			panel.Append(btn_Stats);
		}
		public void ModdedStats_Initialization(ref float marginForBtn) {
			btn_ModStats = new UIImageButton(TextureAssets.InventoryBack);
			btn_ModStats.UISetWidthHeight(52, 52);
			btn_ModStats.HAlign = .5f;
			btn_ModStats.MarginTop = marginForBtn;
			marginForBtn += btn_ModStats.Height.Pixels + 10;
			btn_ModStats.OnLeftClick += Btn_ModStats_OnLeftClick;
			panel.Append(btn_ModStats);
		}
		public void PerkPage_Initialization(ref float marginForBtn) {
			textlist = new Dictionary<Roguelike_UIText, int>();
			btn_Perks = new UIImageButton(TextureAssets.InventoryBack);
			btn_Perks.HAlign = .5f;
			btn_Perks.UISetWidthHeight(52, 52);
			btn_Perks.MarginTop = marginForBtn;
			marginForBtn += btn_Perks.Height.Pixels + 10;
			btn_Perks.OnLeftClick += Btn_Perks_OnLeftClick;
			panel.Append(btn_Perks);
		}
		UIImageButton btn_Artifact;
		Info_ArtifactImage Info_artifact;
		Roguelike_WrapTextUIPanel artifactcustomtextpanel;
		Roguelike_UIPanel artifactHeaderpanel;
		Roguelike_UIText artifact_text;
		Roguelike_UIImageButton artifact_upgradebtn;
		Roguelike_UIPanel artifactUpgradePanel;

		public void ArtifactUpgradePanelAppend(Roguelike_UIPanel panel) {
			artifactUpgradePanel = panel;
			Append(artifactUpgradePanel);
		}
		public void ArtifactPage_Initialization(ref float marginForBtn) {
			btn_Artifact = new UIImageButton(TextureAssets.InventoryBack);
			btn_Artifact.UISetWidthHeight(52, 52);
			btn_Artifact.HAlign = .5f;
			btn_Artifact.MarginTop = marginForBtn;
			marginForBtn += btn_Artifact.Height.Pixels + 10;
			btn_Artifact.OnLeftClick += Btn_Artifact_OnLeftClick;
			panel.Append(btn_Artifact);

			artifactHeaderpanel = new();
			artifactHeaderpanel.Width.Percent = 1f;
			artifactHeaderpanel.Height.Pixels = 80;
			artifactHeaderpanel.Hide = true;
			textpanel.Append(artifactHeaderpanel);

			artifactcustomtextpanel = new Roguelike_WrapTextUIPanel("");
			artifactcustomtextpanel.VAlign = 1;
			artifactcustomtextpanel.Width.Percent = 1;
			artifactcustomtextpanel.Height.Precent = 1;
			artifactcustomtextpanel.Height.Pixels -= artifactHeaderpanel.Height.Pixels + 10;
			artifactcustomtextpanel.Hide = true;
			artifactcustomtextpanel.DrawPanel = false;
			textpanel.Append(artifactcustomtextpanel);

			Info_artifact = new Info_ArtifactImage(TextureAssets.InventoryBack);
			Info_artifact.VAlign = .5f;
			Info_artifact.Hide = true;
			artifactHeaderpanel.Append(Info_artifact);

			artifact_text = new("");
			artifact_text.VAlign = .5f;
			artifact_text.MarginLeft = Info_artifact.Width.Pixels + 10;
			artifact_text.Hide = true;
			artifactHeaderpanel.Append(artifact_text);

			artifact_upgradebtn = new(TextureAssets.InventoryBack);
			artifact_upgradebtn.UISetWidthHeight(52, 52);
			artifact_upgradebtn.HAlign = 1f;
			artifact_upgradebtn.Hide = true;
			artifact_upgradebtn.SetVisibility(.76f, 1f);
			artifact_upgradebtn.OnLeftClick += Artifact_upgradebtn_OnLeftClick;
			artifactHeaderpanel.Append(artifact_upgradebtn);

			artifactUpgradePanel = new();
			artifactUpgradePanel.UISetWidthHeight(600, 600);
			artifactUpgradePanel.HAlign = .5f;
			artifactUpgradePanel.VAlign = .5f;
			artifactUpgradePanel.Hide = true;
			Append(artifactUpgradePanel);
		}

		private void Artifact_upgradebtn_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
			artifactUpgradePanel.Hide = !artifactUpgradePanel.Hide;
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
				generalTextPanel.Left.Set(Main.MouseScreen.X, 0);
				generalTextPanel.Top.Set(Main.MouseScreen.Y, 0);
			}
		}
		/// <summary>
		/// Set true to hide item, related to artifact, otherwise false to show them
		/// </summary>
		/// <param name="sett"></param>
		public void ArtifactPage_VisibilitySetting(bool sett = false) {
			artifactHeaderpanel.Hide = sett;
			Info_artifact.Hide = sett;
			artifactcustomtextpanel.Hide = sett;
			artifact_text.Hide = sett;
			artifact_upgradebtn.Hide = sett;
			if (artifactcustomtextpanel.Text == "") {
				string line = "";
				var artifactplayer = Main.LocalPlayer.GetModPlayer<ArtifactPlayer>();
				line = $"Current active artifact : {Artifact.GetArtifact(artifactplayer.ActiveArtifact).DisplayName}";
				line += $"\n{Artifact.GetArtifact(artifactplayer.ActiveArtifact).Description}";
				artifactcustomtextpanel.SetText(line);
			}
			if (!sett) {
				CurrentState = 2;
				btn_Artifact.SetVisibility(1, 1);
				textpanel.SetText("");
			}
			else {
				btn_Artifact.SetVisibility(.7f, .6f);
			}
		}
		private void Btn_Stats_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
			foreach (var item in textlist.Keys) {
				item.Hide = true;
			}
			ArtifactPage_VisibilitySetting(true);

			btn_Stats.SetVisibility(1, 1);
			btn_ModStats.SetVisibility(.7f, .6f);
			btn_Perks.SetVisibility(.7f, .6f);
			CurrentState = 0;
			generalTextPanel.Hide = true;
		}
		private void Btn_ModStats_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
			foreach (var item in textlist.Keys) {
				item.Hide = true;
			}
			ArtifactPage_VisibilitySetting(true);

			btn_ModStats.SetVisibility(1, 1);
			btn_Stats.SetVisibility(.7f, .6f);
			btn_Perks.SetVisibility(.7f, .6f);
			CurrentState = 1;
			generalTextPanel.Hide = true;
		}
		private void Btn_Artifact_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
			foreach (var item in textlist.Keys) {
				item.Hide = true;
			}
			ArtifactPage_VisibilitySetting(false);

			btn_ModStats.SetVisibility(.7f, .6f);
			btn_Perks.SetVisibility(.7f, .6f);
			btn_Stats.SetVisibility(.7f, .6f);
			generalTextPanel.Hide = true;
		}

		private void Btn_Perks_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) {
			ArtifactPage_VisibilitySetting(true);

			foreach (var item in textlist.Keys) {
				item.Hide = false;
			}
			btn_Perks.SetVisibility(1, 1);
			btn_ModStats.SetVisibility(.7f, .6f);
			btn_Stats.SetVisibility(.7f, .6f);
			CurrentState = 3;
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
		public static string ItemIcon(int ItemID) => "[i:" + ItemID + "]";
		public override void Update(GameTime gameTime) {
			InfoShowToItem = string.Empty;
			base.Update(gameTime);
			if (panel.ContainsPoint(Main.MouseScreen)) {
				Main.LocalPlayer.mouseInterface = true;
			}
			if (textpanel.ContainsPoint(Main.MouseScreen)) {
				Main.LocalPlayer.mouseInterface = true;
			}
			var player = Main.LocalPlayer;
			string line;
			var statshandle = player.GetModPlayer<PlayerStatsHandle>();
			var chestplayer = player.GetModPlayer<ChestLootDropPlayer>();
			var enchantplayer = player.GetModPlayer<EnchantmentModplayer>();
			var augmentation = player.GetModPlayer<AugmentsPlayer>();
			switch (CurrentState) {
				case 0:
					foreach (var item in list_info) {
						item.Hide(false);
					}
					// 0 to 17 is default stats
					if (list_info.Count < 1) {
						list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[0].SetInfo($"{ItemIcon(ItemID.BoneSword)} Melee Damage : {Math.Round(player.GetTotalDamage(DamageClass.Melee).ToFloatValue(100, 1) - 100)}% Base : {player.GetTotalDamage(DamageClass.Melee).Base} Flat : {player.GetTotalDamage(DamageClass.Melee).Flat} Crit chance : {player.GetTotalCritChance(DamageClass.Melee)}%");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[1].SetInfo($"{ItemIcon(ItemID.PlatinumBow)} Range Damage : {Math.Round(player.GetTotalDamage(DamageClass.Ranged).ToFloatValue(100, 1) - 100)}% Base : {player.GetTotalDamage(DamageClass.Ranged).Base} Flat : {player.GetTotalDamage(DamageClass.Ranged).Flat} Crit chance : {player.GetTotalCritChance(DamageClass.Ranged)}%");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[2].SetInfo($"{ItemIcon(ItemID.RubyStaff)} Magic Damage : {Math.Round(player.GetTotalDamage(DamageClass.Magic).ToFloatValue(100, 1) - 100)}% Base : {player.GetTotalDamage(DamageClass.Magic).Base} Flat : {player.GetTotalDamage(DamageClass.Magic).Flat} Crit chance : {player.GetTotalCritChance(DamageClass.Magic)}%");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[3].SetInfo($"{ItemIcon(ItemID.BabyBirdStaff)} Summon Damage : {Math.Round(player.GetTotalDamage(DamageClass.Summon).ToFloatValue(100, 1) - 100)}% Base : {player.GetTotalDamage(DamageClass.Summon).Base} Flat : {player.GetTotalDamage(DamageClass.Summon).Flat} Crit chance : {player.GetTotalCritChance(DamageClass.Summon)}%");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[4].SetInfo($"{ItemIcon(ItemID.AvengerEmblem)} Generic Damage : {Math.Round(player.GetTotalDamage(DamageClass.Generic).ToFloatValue(100, 1) - 100)}% Base : {player.GetTotalDamage(DamageClass.Generic).Base} Flat : {player.GetTotalDamage(DamageClass.Generic).Flat} Crit chance : {player.GetTotalCritChance(DamageClass.Generic)}%");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[5].SetInfo($"{ItemIcon(ItemID.DestroyerEmblem)} Crit damage : {Math.Round((statshandle.UpdateCritDamage.ApplyTo(1) + 1) * 100, 2)}%");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[6].SetInfo($"{ItemIcon(ItemID.BreakerBlade)} First strike damage : {Math.Round((statshandle.UpdateFullHPDamage.ApplyTo(1) - 1) * 100, 2)}%");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[7].SetInfo($"{ItemIcon(ItemID.ShroomiteDiggingClaw)} Attack speed: {RelicTemplateLoader.RelicValueToPercentage(player.GetTotalAttackSpeed(DamageClass.Generic))}");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[8].SetInfo($"{ItemIcon(ItemID.BandofRegeneration)} Health regenaration : {player.lifeRegen}");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[9].SetInfo($"{ItemIcon(ItemID.ManaRegenerationBand)} Mana regenaration : {player.manaRegen}");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[10].SetInfo($"{ItemIcon(ItemID.ManaFlower)} Mana reduction : {player.manaCost}");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[11].SetInfo($"{ItemIcon(ItemID.ShieldStatue)} Defense effectiveness : {player.DefenseEffectiveness.Value}");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[12].SetInfo($"{ItemIcon(ItemID.WormScarf)} Damage reduction: {Math.Round(player.endurance * 100, 2)}%");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[13].SetInfo($"{ItemIcon(ItemID.HermesBoots)} Movement speed : {Math.Round(player.moveSpeed, 2)}");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[14].SetInfo($"{ItemIcon(ItemID.FrogLeg)} Jump boost : {player.jumpSpeedBoost}");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[15].SetInfo($"{ItemIcon(ItemID.BewitchingTable)} Max minion : {player.maxMinions}");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[16].SetInfo($"{ItemIcon(ItemID.WarTable)} Max sentry/turret : {player.maxTurrets}");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[17].SetInfo($"{ItemIcon(ItemID.Turtle)} Thorn : {player.thorns}");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[18].SetInfo($"{ItemIcon(ModContent.ItemType<WoodenLootBox>())} Amount drop : {chestplayer.DropModifier.ApplyTo(1)}");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[19].SetInfo($"{ItemIcon(ModContent.ItemType<DivineHammer>())} Bonus chance getting enchanted : {RelicTemplateLoader.RelicValueToPercentage(1 + statshandle.RandomizeChanceEnchantment)}");
						list_info[list_info.Count - 1].action.Invoke(); list_info.Add(new(textpanel));
						list_info[list_info.Count - 1].action = () => list_info[20].SetInfo($"Bonus chance getting augmentation : {RelicTemplateLoader.RelicValueToPercentage(1 + statshandle.AugmentationChance)}");
						for (int i = 0; i < list_info.Count; i++) {
							float Y = MathHelper.Lerp(0, 1f, i / (list_info.Count - 1f));
							list_info[i].SetAlign(0, Y);
						}
					}
					for (int i = 0; i < list_info.Count; i++) {
						if (list_info[i].action != null) {
							list_info[i].action.Invoke();
						}
					}
					break;
				case 1:
					foreach (var item in list_info) {
						item.Hide(true);
					}
					var drugplayer = player.GetModPlayer<WonderDrugPlayer>();
					var nohitPlayer = player.GetModPlayer<NoHitPlayerHandle>();
					chestplayer.GetAmount();
					line =
						$"Amount drop chest final weapon : {chestplayer.weaponAmount}" +
						$"\nAmount drop chest final potion type : {chestplayer.potionTypeAmount}" +
						$"\nAmount drop chest final potion amount : {chestplayer.potionNumAmount}" +
						$"\nMelee drop chance : {chestplayer.UpdateMeleeChanceMutilplier}" +
						$"\nRange drop chance : {chestplayer.UpdateRangeChanceMutilplier}" +
						$"\nMagic drop chance : {chestplayer.UpdateMagicChanceMutilplier}" +
						$"\nSummon drop chance : {chestplayer.UpdateSummonChanceMutilplier}" +
						$"\nWonder drug consumed rate : {drugplayer.DrugDealer}" +
						$"\nAmount boss no-hit : {nohitPlayer.BossNoHitNumber.Count}" +
						$"\nRun amount : {RoguelikeData.Run_Amount}" +
						$"\nLootbox opened: {RoguelikeData.Lootbox_AmountOpen}" +
						$"\nAmount boss don't-hit : {nohitPlayer.DontHitBossNumber.Count}";
					textpanel.SetText(line);
					break;
				case 2:
					foreach (var item in list_info) {
						item.Hide(true);
					}
					var artifactplayer = Main.LocalPlayer.GetModPlayer<ArtifactPlayer>();
					artifact_text.SetText(Artifact.GetArtifact(artifactplayer.ActiveArtifact).DisplayName);
					line = $"{Artifact.GetArtifact(artifactplayer.ActiveArtifact).Description}";
					artifactcustomtextpanel.SetText(line);
					break;
				case 3:
					foreach (var item in list_info) {
						item.Hide(true);
					}
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
		}
	}
}
