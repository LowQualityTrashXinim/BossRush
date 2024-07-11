using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using BossRush.Contents.Artifacts;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Items.RelicItem {
	public class RelicContainer : ModItem {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Chest);
		private int countX = 0;
		private float positionRotateX = 0;
		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 28;
			Item.maxStack = 30;
			Item.value = Item.buyPrice(gold: 10);
		}
		public override bool CanRightClick() => true;
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
		public override void RightClick(Player player) {
			base.RightClick(player);
			var entitySource = player.GetSource_OpenItem(Type);
			int amount = 1;
			if (player.HasArtifact<TokenOfGreedArtifact>() || player.HasArtifact<EternalWealthArtifact>())
				amount++;
			for (int i = 0; i < amount; i++) player.QuickSpawnItem(entitySource, ModContent.ItemType<Relic>());
		}
		Color auraColor = new Color(255, 100, 0, 30);
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
			PositionHandle();
			Main.instance.LoadItem(Type);
			var texture = TextureAssets.Item[Type].Value;
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, position + new Vector2(positionRotateX, positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(positionRotateX, -positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-positionRotateX, positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-positionRotateX, -positionRotateX), null, auraColor, 0, origin, scale, SpriteEffects.None, 0);
			}
			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
	}
}
