﻿using System;
using Terraria;
using Terraria.UI;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Artifacts;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using BossRush.Common;

namespace BossRush.Contents.Items.Card {
	internal class CardUI : UIState {
		public List<PlayerStats> CardStats = new List<PlayerStats>();
		public List<float> CardStatsNumber = new List<float>();
		public float Multiplier = 1f;
		public UIText toolTip;
		/// <summary>
		/// 1 = Copper<br/>
		/// 2 = Silver<br/>
		/// 3 = Gold<br/>
		/// 4 = Platinum<br/>
		/// </summary>
		public int Tier = 0;
		public int PostTierModify = 0;
		public override void OnActivate() {
			Elements.Clear();
			Player player = Main.LocalPlayer;
			if (player.TryGetModPlayer(out PlayerCardHandle modplayer)) {
				if (Tier <= 0)
					return;
				bool hasMagicDeck = player.HasArtifact<MagicalCardDeckArtifact>();
				PostTierModify = hasMagicDeck ? Tier + 1 : Tier;
				for (int l = 0; l < 3; l++) {
					SetBadStatsBaseOnTier(modplayer, hasMagicDeck);
					int offset = 0;
					if (CardStats.Count > 0)
						offset++;
					int cardlength = Math.Clamp(PostTierModify + offset, 0, 19);
					AddCardStatsAndValue(offset, cardlength);
					//CursedHandle(modplayer);
					CardStatsIncreasesSelection cardUI = new CardStatsIncreasesSelection(TextureAssets.InventoryBack10, modplayer);
					cardUI.CardStats.AddRange(CardStats);
					cardUI.CardStatsNumber.AddRange(CardStatsNumber);
					cardUI.PostTierModify = PostTierModify;
					cardUI.UISetPosition(player.Center - new Vector2(-100 + 100 * l, 40), new Vector2(26, 26));
					cardUI.UISetWidthHeight(52, 52);
					Append(cardUI);
					CardStats.Clear();
					CardStatsNumber.Clear();
				}
			}
			toolTip = new UIText("");
			Append(toolTip);
		}
		private void AddCardStatsAndValue(int offset, int length) {
			for (int i = offset; i < length; i++) {
				CardStats.Add(SetStatsToAddBaseOnTier(CardStats, PostTierModify));
				CardStatsNumber.Add(statsCalculator(CardStats[i], Multiplier));
			}
		}
		private void SetBadStatsBaseOnTier(PlayerCardHandle modplayer, bool hasMagicDeck) {
			if (Tier <= 0) {
				return;
			}
			int RandomNumberGen = Main.rand.Next(101 + modplayer.CardLuck / Tier);
			if (RandomNumberGen < modplayer.CardLuck || hasMagicDeck && RandomNumberGen < modplayer.CardLuck * (2 + Tier)) {
				//RandomNumberGen = Main.rand.Next(101);
				//if (RandomNumberGen < modplayer.CardLuck - 100) {
				//	CursedID = 0;
				//}
				PlayerStats badstat = SetStatsToAddBaseOnTier(CardStats, PostTierModify);
				CardStats.Add(badstat);
				CardStatsNumber.Add(statsCalculator(badstat, -Tier));
			}
		}
		protected float statsCalculator(PlayerStats stats, float multi) {
			float statsNum = Main.rand.Next(1, 4);
			if (BossRushUtils.DoesStatsRequiredWholeNumber(stats)) {
				if (stats is PlayerStats.ChestLootDropIncrease
			|| stats is PlayerStats.MaxMinion
			|| stats is PlayerStats.MaxSentry) {
					statsNum = Main.rand.Next((int)(PostTierModify * .5f)) + 1;
				}
				else {
					statsNum = (Main.rand.Next(PostTierModify) + 1) * PostTierModify;
				}
				return (int)(statsNum * multi);
			}
			switch (PostTierModify) {
				case 1:
					statsNum = statsNum * multi * .01f;
					break;
				case 2:
					statsNum = (statsNum + Main.rand.Next(1, 3) + 1) * PostTierModify * multi * .01f;
					break;
				case 3:
					statsNum = (statsNum + Main.rand.Next(2, 5) + 1) * PostTierModify * multi * .01f;
					break;
				case 4:
					statsNum = (statsNum + Main.rand.Next(4, 7) + 1) * PostTierModify * multi * .01f;
					break;
				default:
					statsNum = (statsNum + Main.rand.Next(7) + Main.rand.Next(1, 11)) * PostTierModify * multi * .01f;
					break;
			}
			return statsNum;
		}
		public static PlayerStats SetStatsToAddBaseOnTier(List<PlayerStats> CardStats, int Tier) {
			List<PlayerStats> stats = new List<PlayerStats>();
			if (Tier >= 4) {
				//list.Add(PlayerStats.Luck);
			}
			if (Tier >= 3) {
				stats.Add(PlayerStats.MaxSentry);
				stats.Add(PlayerStats.MaxMinion);
				stats.Add(PlayerStats.ChestLootDropIncrease);
			}
			if (Tier >= 2) {
				stats.Add(PlayerStats.Thorn);
				stats.Add(PlayerStats.DefenseEffectiveness);
				stats.Add(PlayerStats.CritDamage);
				stats.Add(PlayerStats.CritChance);
				stats.Add(PlayerStats.DamageUniverse);
			}
			if (Tier >= 1) {
				stats.Add(PlayerStats.Defense);
				stats.Add(PlayerStats.RegenMana);
				stats.Add(PlayerStats.MaxMana);
				stats.Add(PlayerStats.RegenHP);
				stats.Add(PlayerStats.MaxHP);
				stats.Add(PlayerStats.MovementSpeed);
				stats.Add(PlayerStats.JumpBoost);
				stats.Add(PlayerStats.SummonDMG);
				stats.Add(PlayerStats.MagicDMG);
				stats.Add(PlayerStats.RangeDMG);
				stats.Add(PlayerStats.MeleeDMG);
			}
			if (CardStats.Count > 0 && CardStats.Count != stats.Count) {
				foreach (var item in CardStats) {
					if (stats.Contains(item)) {
						stats.Remove(item);
					}
				}
			}
			return Main.rand.Next(stats);
		}
		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			if (IsMouseHovering) {
			}
			foreach (var element in Elements) {
				if (element.IsMouseHovering && element is CardStatsIncreasesSelection cardUI) {
					toolTip.Left = element.Left;
					toolTip.Top.Pixels = element.Top.Pixels + 70;
					toolTip.SetText(cardUI.CardStatsText());
					return;
				}
			}
		}
	}
	class CardStatsIncreasesSelection : UIImageButton {
		private string StatNumberAsText(PlayerStats stat, float number) {
			string value = "";
			if (number > 0) {
				value = "+";
			}
			if (BossRushUtils.DoesStatsRequiredWholeNumber(stat)) {
				return value + $"{number} {stat}";
			}
			return value + $"{(int)(number * 100)}% {stat}";
		}
		public string CardStatsText() {
			string text = "";
			for (int i = 0; i < CardStats.Count; i++) {
				text += StatNumberAsText(CardStats[i], CardStatsNumber[i]);
				//if (CurseID >= 1 && CurseID <= 12)
				//	text += modplayer.CursedStringStats(CurseID);
				if (i != CardStats.Count - 1)
					text += "\n";
			}
			return text;
		}
		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
		}
		/// <summary>
		/// We assume the length is always = to <see cref="CardStatsNumber"/>
		/// </summary>
		public List<PlayerStats> CardStats = new List<PlayerStats>();
		/// <summary>
		/// We assume the length is always = to <see cref="CardStats"/>
		/// </summary>
		public List<float> CardStatsNumber = new List<float>();
		public int PostTierModify = 0;
		PlayerCardHandle modplayer;
		public CardStatsIncreasesSelection(Asset<Texture2D> texture, PlayerCardHandle Modplayer) : base(texture) {
			modplayer = Modplayer;
		}
		public override void LeftClick(UIMouseEvent evt) {
			modplayer.CardTracker++;
			modplayer.CardLuck = Math.Clamp(modplayer.CardLuck + Main.rand.Next(PostTierModify + 1), 0, 200);
			for (int i = 0; i < CardStats.Count; i++) {
				AddStatsToPlayer(modplayer, CardStats[i], CardStatsNumber[i]);
			}
			UniversalSystem uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
			uiSystemInstance.userInterface.SetState(null);
		}
		private void AddStatsToPlayer(PlayerCardHandle modplayer, PlayerStats stats, float amount) {
			switch (stats) {
				case PlayerStats.MeleeDMG:
					modplayer.MeleeDMG += amount;
					break;
				case PlayerStats.RangeDMG:
					modplayer.RangeDMG += amount;
					break;
				case PlayerStats.MagicDMG:
					modplayer.MagicDMG += amount;
					break;
				case PlayerStats.SummonDMG:
					modplayer.SummonDMG += amount;
					break;
				case PlayerStats.MovementSpeed:
					modplayer.Movement += amount;
					break;
				case PlayerStats.JumpBoost:
					modplayer.JumpBoost += amount;
					break;
				case PlayerStats.MaxHP:
					modplayer.HPMax += (int)amount;
					break;
				case PlayerStats.RegenHP:
					modplayer.HPRegen += amount;
					break;
				case PlayerStats.MaxMana:
					modplayer.ManaMax += (int)amount;
					break;
				case PlayerStats.RegenMana:
					modplayer.ManaRegen += amount;
					break;
				case PlayerStats.Defense:
					modplayer.DefenseBase += (int)amount;
					break;
				case PlayerStats.DamageUniverse:
					modplayer.DamagePure += amount;
					break;
				case PlayerStats.CritChance:
					modplayer.CritStrikeChance += (int)amount;
					break;
				case PlayerStats.CritDamage:
					modplayer.CritDamage += amount;
					break;
				case PlayerStats.DefenseEffectiveness:
					modplayer.DefenseEffectiveness += amount;
					break;
				case PlayerStats.Thorn:
					modplayer.Thorn += amount;
					break;
				case PlayerStats.ChestLootDropIncrease:
					modplayer.DropAmountIncrease += (int)amount;
					break;
				case PlayerStats.MaxMinion:
					modplayer.MinionSlot += (int)amount;
					break;
				case PlayerStats.MaxSentry:
					modplayer.SentrySlot += (int)amount;
					break;
				default:
					break;
			}
		}
	}
}

