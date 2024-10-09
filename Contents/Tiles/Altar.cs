using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.RelicItem;
using BossRush.Texture;
public class AlterItem : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.width = Item.height = 30;
		Item.useTime = Item.useAnimation = 30;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.createTile = ModContent.TileType<Altar>();
	}
}
public class Altar : ModTile {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override string HighlightTexture => BossRushTexture.MissingTexture_Default;
	public bool Activated = false;
	public override void SetStaticDefaults() {
		Main.tileSolid[Type] = false;
		Main.tileMergeDirt[Type] = true;
		Main.tileBlockLight[Type] = true;
		Main.tileLighted[Type] = true;
		DustType = DustID.Stone;
		AddMapEntry(new Color(200, 200, 200));
	}
	public override bool RightClick(int i, int j) {
		Player player = Main.LocalPlayer;
		player.QuickSpawnItem(player.GetSource_TileInteraction(i, j), ModContent.ItemType<Relic>());
		WorldGen.KillTile(i, j, noItem: true);
		for (int a = 0; a < 30; a++) {
			int dust = Dust.NewDust(new Vector2(i, j).ToWorldCoordinates(), 0, 0, DustID.Cloud, Scale: Main.rand.NextFloat(2, 3));
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(4, 4);
		}
		return base.RightClick(i, j);
	}
}
