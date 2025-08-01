using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.Items.aDebugItem.Trial;
internal class TrialWand : ModItem {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.CelestialWand);
	public override void SetDefaults() {
		Item.width = Item.height = 32;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.useAnimation = Item.useTime = 1;
		Item.noUseGraphic = true;
		Item.consumable = false;
		Item.autoReuse = true;
	}
	public override bool? UseItem(Player player) {
		return true;
	}
}

public class TrialWandSystem : ModSystem {
	public static TrialWandSystem instance => ModContent.GetInstance<TrialWandSystem>();
	public bool CheckTrialSizeSet => trialSize.X != 0 && trialSize.Y != 0 && trialSize.Width != 0 && trialSize.Height != 0;
	public Rectangle trialSize = new Rectangle();
	public List<TrialNPCCord> cords = new();
}
public class TrialNPCCord {
	public int NPCType;
	public Point TilePosition;
}
