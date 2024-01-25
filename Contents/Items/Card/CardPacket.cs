using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using System.Collections.Generic;
using BossRush.Contents.Artifacts;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Items.Card {
	abstract class CardPacketBase : ModItem {
		private int countX = 0;
		private float positionRotateX = 0;
		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			if (BossRushUtils.IsAnyVanillaBossAlive()) {
				tooltips.Add(new TooltipLine(Mod, "BossPreventYou", "Boss lock the card packet to be open, kill the boss to open the packet"));
			}
		}
		public override bool CanRightClick() {
			return !BossRushUtils.IsAnyVanillaBossAlive();
		}
		private void PositionHandle() {
			if (positionRotateX < 3 && countX == 1)
				positionRotateX += .3f;
			else
				countX = -1;
			if (positionRotateX > 0 && countX == -1)
				positionRotateX -= .3f;
			else
				countX = 1;
		}
		public virtual int PacketType => 0;
		public virtual int CardAmount => 1;
		public override void RightClick(Player player) {
			base.RightClick(player);
			var entitySource = player.GetSource_OpenItem(Type);
			int amount = CardAmount;
			PlayerStatsHandle cardplayer = player.GetModPlayer<PlayerStatsHandle>();
			if (player.HasArtifact<TokenOfGreedArtifact>() || player.HasArtifact<EternalWealthArtifact>())
				amount += 2;
			if (player.HasArtifact<MagicalCardDeckArtifact>())
				amount += Main.rand.Next(4);
			if (PacketType == 4)
				amount = 1;
			for (int i = 0; i < amount; i++) {
				player.QuickSpawnItem(entitySource, ModContent.ItemType<CopperCard>());
			}
		}
		Color auraColor;
		private void ColorHandle() {
			switch (PacketType) {
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
			Main.instance.LoadItem(Item.type);
			Texture2D texture = TextureAssets.Item[Item.type].Value;
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, position + new Vector2(positionRotateX, positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(positionRotateX, -positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-positionRotateX, positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-positionRotateX, -positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
			}
			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
			//if (Item.whoAmI != whoAmI)
			//{
			//    return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
			//}
			Main.instance.LoadItem(Item.type);
			Texture2D texture = TextureAssets.Item[Item.type].Value;
			Vector2 origin = new Vector2(texture.Width * 0.5f, Item.width * 0.5f);
			Vector2 drawPos = Item.position - Main.screenPosition + origin;
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, drawPos + new Vector2(positionRotateX, positionRotateX), null, auraColor, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos + new Vector2(positionRotateX, -positionRotateX), null, auraColor, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos + new Vector2(-positionRotateX, positionRotateX), null, auraColor, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos + new Vector2(-positionRotateX, -positionRotateX), null, auraColor, rotation, origin, scale, SpriteEffects.None, 0);
			}
			return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
		}
	}

	internal class CardPacket : CardPacketBase {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Chest);
		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 28;
			Item.maxStack = 30;
		}
		public override int PacketType => 1;
	}
	internal class BigCardPacket : CardPacketBase {
		public override int PacketType => 2;
		public override int CardAmount => 3;
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.GoldChest);
		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 28;
			Item.maxStack = 30;
		}
	}
	internal class BoxOfCard : CardPacketBase {
		public override int PacketType => 3;
		public override int CardAmount => 5;
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.ShadowChest);
		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 28;
			Item.maxStack = 30;
		}
	}
	internal class PremiumCardPacket : CardPacketBase {
		public override int PacketType => 4;
		public override int CardAmount => 1;
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.DesertChest);
		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 28;
			Item.maxStack = 30;
		}
	}
}
