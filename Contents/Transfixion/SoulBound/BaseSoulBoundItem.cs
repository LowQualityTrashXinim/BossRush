using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Contents.Transfixion.Arguments;

namespace BossRush.Contents.Transfixion.SoulBound.SoulBoundMaterial;
public abstract class BaseSoulBoundItem : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public virtual short SoulBoundType => -1;
	public override void UpdateInventory(Player player) {
		if (Main.mouseRight) {
			for (int i = 0; i < player.inventory.Length; i++) {
				if (player.inventory[i] == Item) {
					player.GetModPlayer<SoulBoundPlayer>().IndexSoulBoundItem = i;
					break;
				}
			}
		}
	}
	ColorInfo info = new(new List<Color>() { Color.AliceBlue, Color.IndianRed, Color.MediumPurple, Color.DarkGreen, Color.LightGoldenrodYellow });
	public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
		int index = Main.LocalPlayer.GetModPlayer<SoulBoundPlayer>().IndexSoulBoundItem;
		if (index < Main.LocalPlayer.inventory.Length && index >= 0) {
			BossRushUtils.ItemDefaultDrawInfo(Item, out Texture2D texture, out Vector2 _);
			BossRushUtils.DrawAuraEffect(spriteBatch, texture, position, 2, 2, info.MultiColor(5), 0, scale);
		}
		return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
	}
}
