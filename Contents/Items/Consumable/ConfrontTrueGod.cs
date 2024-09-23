using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Items.Consumable;
internal class ConfrontTrueGod : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32);
	}
	int delayBetweenSwitch = 0;
	int CurrentItemTexture = 0;
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		string text = "";
		if (NPC.downedMoonlord) {
			text = "There are more to be discover...";
		}
		else if (Main.hardMode) {
			text = "Now is not the time...";
		}
		if (text == "") {
			return;
		}
		foreach (TooltipLine line in tooltips) {
			if (line.Name == "Tooltip0") {
				line.Text = text;
			}
		}
	}
	public override bool? UseItem(Player player) {
		if (!player.dead) {
			string whoAmI = "False God";
			if(!NPC.downedMoonlord) {
				whoAmI = "\"True\" God";
			}
			PlayerDeathReason reason = new PlayerDeathReason();
			reason.SourceCustomReason = $"{player.name} has confront {whoAmI}";
			player.KillMe(reason, 1, 1);
		}
		return true;
	}
	public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
		int length = TextureAssets.Item.Length;
		if (length > 0) {
			int itemToLoad;
			if (delayBetweenSwitch <= 0) {
				itemToLoad = Main.rand.Next(length);
				CurrentItemTexture = itemToLoad;
				delayBetweenSwitch = 5;
			}
			else {
				itemToLoad = CurrentItemTexture;
				delayBetweenSwitch--;
			}
			Main.instance.LoadItem(itemToLoad);
			Texture2D texture = TextureAssets.Item[itemToLoad].Value;
			spriteBatch.Draw(texture, position, null, Color.White, 0, texture.Size() * .5f, scale, SpriteEffects.None, 0);
			return false;
		}
		return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
	}
}
