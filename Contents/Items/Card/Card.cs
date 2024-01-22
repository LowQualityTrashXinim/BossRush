using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using System.Collections.Generic;
using BossRush.Contents.Artifacts;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Items.Card {
	public abstract class CardItem : ModItem {
		public const int PlatinumCardDropChance = 40;
		public const int GoldCardDropChance = 20;
		public const int SilverCardDropChance = 10;
		public float Multiplier = 1f;
		public override void SetDefaults() {
			Item.BossRushDefaultToConsume(30, 24);
			Item.UseSound = SoundID.Item35;
			PostCardSetDefault();
		}
		public virtual void PostCardSetDefault() { }
		public virtual void ModifyCardToolTip(ref List<TooltipLine> tooltips, PlayerStatsHandle modplayer) { }
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			PlayerStatsHandle modplayer = Main.LocalPlayer.GetModPlayer<PlayerStatsHandle>();
			ModifyCardToolTip(ref tooltips, modplayer);
			if (Tier > 0) {
				tooltips.Add(new TooltipLine(Mod, "HelpfulText", "Use the card to get choose from 1 of 3 stats bonus" +
					"\nThe more cards you uses, the higher the chance of getting bad stats will be"));
			}
		}
		/// <summary>
		/// Use this if <see cref="Tier"/> value set within the card item have value larger than 0
		/// </summary>
		public virtual void OnTierItemSpawn() { }

		/// <summary>
		/// 1 = Copper<br/>
		/// 2 = Silver<br/>
		/// 3 = Gold<br/>
		/// 4 = Platinum<br/>
		/// </summary>
		public virtual int Tier => 0;
		public virtual int PostTierModify => Main.LocalPlayer.HasArtifact<MagicalCardDeckArtifact>() ? Tier + 1 : Tier;
		public override bool CanUseItem(Player player) {
			return !BossRushUtils.IsAnyVanillaBossAlive();
		}
		public virtual void OnUseItem(Player player, PlayerStatsHandle modplayer) { }
		public override bool? UseItem(Player player) {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			OnUseItem(player, modplayer);
			if (Tier <= 0)
				return true;
			UniversalSystem uiSystemInstance = ModContent.GetInstance<UniversalSystem>();
			uiSystemInstance.cardUIstate.Tier = Tier;
			uiSystemInstance.SetState(uiSystemInstance.cardUIstate);
			return true;
		}
		private int countX = 0;
		private float positionRotateX = 0;
		private void PositionHandle() {
			if (positionRotateX < 3.5f && countX == 1) {
				positionRotateX += .2f;
			}
			else {
				countX = -1;
			}
			if (positionRotateX > 0 && countX == -1) {
				positionRotateX -= .2f;
			}
			else {
				countX = 1;
			}
		}
		Color auraColor;
		private void ColorHandle() {
			switch (Tier) {
				case 1:
					auraColor = new Color(255, 100, 0, 30);
					break;
				case 2:
					auraColor = new Color(200, 200, 200, 30);
					break;
				case 3:
					auraColor = new Color(255, 255, 0, 30);
					break;
				default:
					auraColor = new Color(255, 255, 255, 30);
					break;
			}
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
			PositionHandle();
			ColorHandle();
			Item.DrawAuraEffect(spriteBatch, position, positionRotateX, positionRotateX, auraColor, 0, scale);
			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
			Item.DrawAuraEffect(spriteBatch, positionRotateX, positionRotateX, auraColor, rotation, scale);
			return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
		}
	}
}
