using BossRush.Texture;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using BossRush.Common.RoguelikeChange.ItemOverhaul;
using BossRush.Contents.Projectiles;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class LightSpeedRound : ModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("LostAcc");
	public override void SetDefaults() {
		Item.Set_LostAccessory(32, 32);
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<LightSpeedRound_Player>().LightSpeedRound = true;
		player.GetModPlayer<RangerOverhaulPlayer>().SpreadModify *= 0;
	}
}
public class LightSpeedRound_Player : ModPlayer {
	public bool LightSpeedRound = false;
	public override void ResetEffects() {
		LightSpeedRound = false;
	}
	public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (LightSpeedRound) {
			if (type == ProjectileID.Bullet) {
				type = ModContent.ProjectileType<HitScanBullet>();
			}
		}
	}
}
