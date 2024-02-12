using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Card;
public abstract class ModCardItem : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public float StatsValue = 1f;
	public PlayerStats stat = PlayerStats.None;
	public override void SetDefaults() {
		Item.width = Item.height = 32;
	}
}
