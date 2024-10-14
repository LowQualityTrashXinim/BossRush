using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Items.RelicItem;
using BossRush.Texture;
public class AltarItem : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.width = Item.height = 30;
		Item.useTime = Item.useAnimation = 30;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.createTile = ModContent.TileType<Altar>();
	}
}
public abstract class Altar : ModTile {
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
		WorldGen.KillTile(i, j, noItem: true);
		On_RightClick(player, i, j);
		return base.RightClick(i, j);
	}
	public virtual void On_RightClick(Player player, int i, int j) { }
}
public class RelicAltar : Altar {
	public override void On_RightClick(Player player, int i, int j) {
		player.QuickSpawnItem(player.GetSource_TileInteraction(i, j), ModContent.ItemType<Relic>());
		for (int a = 0; a < 30; a++) {
			int dust = Dust.NewDust(new Vector2(i, j).ToWorldCoordinates(), 0, 0, DustID.Cloud, Scale: Main.rand.NextFloat(2, 3));
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(4, 4);
		}
	}
}
