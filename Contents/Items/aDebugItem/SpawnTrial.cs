using BossRush.Common.Systems.ObjectSystem;
using BossRush.Common.Systems.TrialSystem;
using BossRush.Texture;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.aDebugItem;
internal class SpawnTrial : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32);
		Item.Set_DebugItem(true);
	}
	public override bool? UseItem(Player player) {
		if (player.itemAnimation == player.itemAnimationMax) {
			TrialModSystem.SetTrial(0, player.Center);
		}
		return base.UseItem(player);
	}
}
class TrialProtal : ModObject {
	public override void SetDefaults() {
		timeLeft = 9999;
	}
	public override void AI() {
		base.AI();
	}
	public override void Draw(SpriteBatch spritebatch) {
		base.Draw(spritebatch);
	}
}
