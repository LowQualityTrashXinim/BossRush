using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace BossRush.Contents.Items.Accessories.Trinket;
internal class Trinket_of_Ample_Perception : BaseTrinket {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.DefaultToAccessory();
	}
	public override void UpdateTrinket(Player player, TrinketPlayer modplayer) {
		for (int i = 0; i < modplayer.Trinket_of_Ample_Perception_PointCounter; i++) {
			Vector2 pos = player.Center + 
				Vector2.One.Vector2DistributeEvenly(modplayer.Trinket_of_Ample_Perception_PointCounter, 360, i)
				.RotatedBy(MathHelper.ToRadians(modplayer.counterToFullPi)) * 30;
			int dust = Dust.NewDust(pos, 0, 0, DustID.GemAmber);
			Main.dust[dust].velocity = Vector2.Zero;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].fadeIn = 0;
		}
		modplayer.Trinket_of_Ample_Perception = true;
		player.GetCritChance(DamageClass.Generic) += 12 + 3 * modplayer.Trinket_of_Ample_Perception_PointCounter;
		modplayer.DamageStats += .06f * modplayer.Trinket_of_Ample_Perception_PointCounter;
	}
}
