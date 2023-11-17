using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Items.BuilderItem {
	internal class SuperBuilderTool : ModItem {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.ArchitectGizmoPack);
		public override void SetDefaults() {
			Item.width = 30;
			Item.height = 34;
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
			Item.vanity = true;
		}
		public override void UpdateEquip(Player player) {
			player.tileSpeed += 10;
			player.pickSpeed *= .1f;
			player.blockRange += 10;
			player.wallSpeed += 10;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
			Main.instance.LoadItem(Item.type);
			var texture = TextureAssets.Item[Item.type].Value;
			var redAlpha = new Color(0, 255, 255, 30);
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, position + new Vector2(2, 2), null, redAlpha, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-2, 2), null, redAlpha, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(2, -2), null, redAlpha, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-2, -2), null, redAlpha, 0, origin, scale, SpriteEffects.None, 0);
			}
			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
			//if (Item.whoAmI != whoAmI)
			//{
			//    return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
			//}
			Main.instance.LoadItem(Item.type);
			var texture = TextureAssets.Item[Item.type].Value;
			var origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			var drawPos = Item.position - Main.screenPosition + origin;
			var redAlpha = new Color(0, 255, 255, 30);
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, drawPos + new Vector2(2, 2), null, redAlpha, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos + new Vector2(-2, 2), null, redAlpha, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos + new Vector2(2, -2), null, redAlpha, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos + new Vector2(-2, -2), null, redAlpha, 0, origin, scale, SpriteEffects.None, 0);
			}
			return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.BuilderPotion)
				.AddIngredient(ItemID.Toolbox)
				.AddIngredient(ItemID.ArchitectGizmoPack)
				.Register();
		}
	}
}
