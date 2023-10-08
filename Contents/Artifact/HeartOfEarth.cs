using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Artifact {
	internal class HeartOfEarth : ArtifactModItem {
		public override void ArtifactSetDefault() {
			width = 34; height = 48;
			Item.rare = ItemRarityID.Cyan;
		}
		private int countX = 0;
		private float positionRotateX = 0;
		private void PositionHandle() {
			if (positionRotateX < 2.5f && countX == 1) {
				positionRotateX += .1f;
			}
			else {
				countX = -1;
			}
			if (positionRotateX > 0 && countX == -1) {
				positionRotateX -= .1f;
			}
			else {
				countX = 1;
			}
		}
		Color auraColor = Color.White;
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
			PositionHandle();
			auraColor = new Color(0, 255, 0, 25);
			Main.instance.LoadItem(Item.type);
			Texture2D texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw(texture, position + new Vector2(positionRotateX, positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, position + new Vector2(positionRotateX, -positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, position + new Vector2(-positionRotateX, positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, position + new Vector2(-positionRotateX, -positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, position + new Vector2(0, -positionRotateX * 1.5f), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
			spriteBatch.Draw(texture, position + new Vector2(0, positionRotateX * 1.5f), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
	}
	class HeartOfEarthPlayer : ModPlayer {
		bool Earth = false;
		public override void ResetEffects() {
			Earth = Player.GetModPlayer<ArtifactPlayerHandleLogic>().ArtifactDefinedID == ModContent.ItemType<HeartOfEarth>();
		}
		int EarthCD = 0;
		int ShortStanding = 0;
		public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
			base.ModifyMaxStats(out health, out mana);
			if (Earth) {
				health.Base = 100 + Player.statLifeMax * 1.5f;
			}
		}
		public override void PostUpdate() {
			if (Player.velocity == Vector2.Zero) {
				ShortStanding++;
				if (ShortStanding > 120) {
					if (ShortStanding % Math.Clamp((10 - ShortStanding / 100), 1, 10) == 0) {
						Player.statLife = Math.Clamp(Player.statLife + 1, 0, Player.statLifeMax2);
					}
				}
			}
			else {
				ShortStanding = 0;
			}
			if (Earth) {
				EarthCD = BossRushUtils.CoolDown(EarthCD);
				if (EarthCD > 0) {
					for (int i = 0; i < 5; i++) {
						int dust = Dust.NewDust(Player.Center + Main.rand.NextVector2Circular(10, 30), 0, 0, DustID.Blood);
						Main.dust[dust].velocity = -Vector2.UnitX * Player.direction * 2f;
					}
				}
			}
		}
		public override bool CanUseItem(Item item) {
			if (Earth) {
				return EarthCD <= 0;
			}
			return base.CanUseItem(item);
		}
		public override void OnHurt(Player.HurtInfo info) {
			if (Earth) {
				EarthCD = 300;
			}
		}
	}
}