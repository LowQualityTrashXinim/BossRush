using BossRush.Contents.Items.Card;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Potion;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace BossRush.Contents.Perks {
	internal class PerkUIState : UIState {
		public const short DefaultState = 0;
		public const short StarterPerkState = 1;
		public int whoAmI = -1;
		public short StateofState = 0;
		public override void OnActivate() {
			base.OnActivate();
			Elements.Clear();
			if (whoAmI == -1)
				return;
			Player player = Main.player[whoAmI];
			if (player.TryGetModPlayer(out PerkPlayer modplayer)) {
				if (StateofState == DefaultState) {
					ActivateNormalPerkUI(modplayer, player);
				}
				if (StateofState == StarterPerkState) {
					ActivateStarterPerkUI(modplayer, player);
				}
			}
		}
		private void ActivateNormalPerkUI(PerkPlayer modplayer, Player player) {
			List<int> listOfPerk = new List<int>();
			for (int i = 0; i < ModPerkLoader.TotalCount; i++) {
				if (modplayer.perks.ContainsKey(i)) {
					if ((!ModPerkLoader.GetPerk(i).CanBeStack && modplayer.perks[i] > 0)
						|| modplayer.perks[i] >= ModPerkLoader.GetPerk(i).StackLimit) {
						continue;
					}
				}
				if (!ModPerkLoader.GetPerk(i).CanBeChoosen) {
					continue;
				}
				listOfPerk.Add(i);
			}
			int amount = listOfPerk.Count;
			Vector2 originDefault = new Vector2(26, 26);
			for (int i = 0; i < modplayer.PerkAmountModified(); i++) {
				Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(modplayer.PerkAmountModified(), 360, i) * Math.Clamp(modplayer.PerkAmountModified() * 20, 0, 200);
				if (i >= amount || i >= modplayer.PerkAmountModified() - 1) {
					UIImageButton buttonWeapon = Main.rand.Next(new UIImageButton[]
					{ new MaterialPotionUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT)),
						 new MaterialCardUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT)),
						 new MaterialWeaponUIImageButton(ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT))
					});
					buttonWeapon.UISetWidthHeight(52, 52);
					buttonWeapon.UISetPosition(player.Center + offsetPos, originDefault);
					Append(buttonWeapon);
					continue;
				}
				int newperk = Main.rand.Next(listOfPerk);
				Asset<Texture2D> texture;
				if (ModPerkLoader.GetPerk(newperk).textureString is not null)
					texture = ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(newperk).textureString);
				else
					texture = ModContent.Request<Texture2D>(BossRushTexture.ACCESSORIESSLOT);
				listOfPerk.Remove(newperk);
				//After that we assign perk
				PerkUIImageButton btn = new PerkUIImageButton(texture, modplayer);
				btn.UISetWidthHeight(52, 52);
				btn.UISetPosition(player.Center + offsetPos, originDefault);
				btn.perkType = newperk;
				Append(btn);
			}

		}
		private void ActivateStarterPerkUI(PerkPlayer modplayer, Player player) {
			Vector2 originDefault = new Vector2(26, 26);
			int[] starterPerk = new int[]
			{ Perk.GetPerkType<BlessingOfSolar>(),
			Perk.GetPerkType<BlessingOfVortex>(),
			Perk.GetPerkType<BlessingOfNebula>(),
			Perk.GetPerkType<BlessingOfStarDust>(),
			Perk.GetPerkType<BlessingOfPerk>()
			};
			for (int i = 0; i < starterPerk.Length; i++) {
				Vector2 offsetPos = Vector2.UnitY.Vector2DistributeEvenly(starterPerk.Length, 360, i) * starterPerk.Length * 20;
				//After that we assign perk
				if (modplayer.perks.ContainsKey(starterPerk[i])) {
					if (modplayer.perks[starterPerk[i]] >= ModPerkLoader.GetPerk(i).StackLimit) {
						continue;
					}
				}
				PerkUIImageButton btn = new PerkUIImageButton(ModContent.Request<Texture2D>(ModPerkLoader.GetPerk(starterPerk[i]).textureString), modplayer);
				btn.UISetWidthHeight(52, 52);
				btn.UISetPosition(player.Center + offsetPos, originDefault);
				btn.perkType = starterPerk[i];
				Append(btn);
			}
		}
	}
	//Do all the check in UI state since that is where the perk actually get create and choose
	class PerkUIImageButton : UIImageButton {
		PerkPlayer perkplayer;
		public int perkType;
		private UIText toolTip;
		public PerkUIImageButton(Asset<Texture2D> texture, PerkPlayer perkPlayer) : base(texture) {
			perkplayer = perkPlayer;
		}
		public override void OnActivate() {
			base.OnActivate();
			toolTip = new UIText("");
			toolTip.HAlign = .5f;
			Append(toolTip);
		}
		public override void LeftClick(UIMouseEvent evt) {
			if (perkplayer.perks.Count < 0 || !perkplayer.perks.ContainsKey(perkType))
				perkplayer.perks.Add(perkType, 1);
			else
				if (perkplayer.perks.ContainsKey(perkType) && ModPerkLoader.GetPerk(perkType).CanBeStack)
				perkplayer.perks[perkType] = perkplayer.perks[perkType] + 1;
			ModPerkLoader.GetPerk(perkType).OnChoose(perkplayer.Player);
			PerkUISystem uiSystemInstance = ModContent.GetInstance<PerkUISystem>();
			uiSystemInstance.userInterface.SetState(null);
		}
		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			if (toolTip is null) {
				return;
			}
			if (IsMouseHovering) {
				toolTip.Left.Pixels = Main.MouseScreen.X - Left.Pixels;
				toolTip.Top.Pixels = Main.MouseScreen.Y - Top.Pixels - 20;
				toolTip.SetText(ModPerkLoader.GetPerk(perkType).PerkNameToolTip);
			}
			else {
				toolTip.SetText("");
			}
		}
		public override void Draw(SpriteBatch spriteBatch) {
			base.Draw(spriteBatch);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			base.DrawSelf(spriteBatch);
		}
	}
	abstract class SpecialPerkUIImageButton : UIImageButton {
		protected UIText toolTip;
		protected SpecialPerkUIImageButton(Asset<Texture2D> texture) : base(texture) {
		}
		public override void OnActivate() {
			base.OnActivate();
			toolTip = new UIText("");
			toolTip.HAlign = .5f;
			Append(toolTip);
		}
		public new virtual void OnLeftClick(Player player) {

		}
		public override void LeftClick(UIMouseEvent evt) {
			base.LeftClick(evt);
			OnLeftClick(Main.LocalPlayer);
			PerkUISystem uiSystemInstance = ModContent.GetInstance<PerkUISystem>();
			uiSystemInstance.userInterface.SetState(null);
		}
		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			if (IsMouseHovering) {
				toolTip.Left.Pixels = Main.MouseScreen.X - Left.Pixels;
				toolTip.Top.Pixels = Main.MouseScreen.Y - Top.Pixels - 20;
				toolTip.SetText(TooltipText());
			}
			else {
				toolTip.SetText("");
			}
		}
		public virtual string TooltipText() => "";
	}
	class MaterialWeaponUIImageButton : SpecialPerkUIImageButton {
		public MaterialWeaponUIImageButton(Asset<Texture2D> texture) : base(texture) {
		}
		public override void OnLeftClick(Player player) {
			LootBoxBase.GetWeapon(out int weapon, out int amount);
			player.QuickSpawnItem(player.GetSource_FromThis(), weapon, amount);
		}
		public override string TooltipText() => "Give you 1 randomize weapon based on progression";
	}
	class MaterialCardUIImageButton : SpecialPerkUIImageButton {
		public MaterialCardUIImageButton(Asset<Texture2D> texture) : base(texture) {
		}
		public override void OnLeftClick(Player player) {
			player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<BoxOfCard>(), 3);
		}
		public override string TooltipText() => "Give you 3 box of card";
	}
	class MaterialPotionUIImageButton : SpecialPerkUIImageButton {
		public MaterialPotionUIImageButton(Asset<Texture2D> texture) : base(texture) {
		}

		public override void OnLeftClick(Player player) {
			player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<MysteriousPotion>(), 5);
		}
		public override string TooltipText() => "Give you 5 mysterious potions";
	}
}
