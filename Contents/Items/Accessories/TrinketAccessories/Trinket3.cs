using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Accessories.TrinketAccessories;
internal class Trinket_of_Ample_Perception : BaseTrinket {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void TrinketDefault() {
		Item.Set_InfoItem(true);
	}
	public override void UpdateTrinket(Player player, TrinketPlayer modplayer) {
		Trinket_of_Ample_Perception_ModPlayer trinketplayer = player.GetModPlayer<Trinket_of_Ample_Perception_ModPlayer>();
		trinketplayer.Trinket_of_Ample_Perception = true;
		PlayerStatsHandle statsplayer = modplayer.GetStatsHandle();
		statsplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 12 + 3 * trinketplayer.PointCounter);
		statsplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1 + .06f * trinketplayer.PointCounter);
	}
}
public class Trinket_of_Ample_Perception_ModPlayer : ModPlayer {
	public bool Trinket_of_Ample_Perception = false;
	public int PointCounter = 0;
	public int PointTimeLeft = 0;
	public int CountDown = 0;
	public override void ResetEffects() {
		Trinket_of_Ample_Perception = false;
	}
	public override void PreUpdate() {
		PointTimeLeft = BossRushUtils.CountDown(PointTimeLeft);
		CountDown = BossRushUtils.CountDown(CountDown);
		if (PointTimeLeft <= 0 && PointCounter > 0) {
			PointCounter--;
			PointTimeLeft = BossRushUtils.ToSecond(7);
		}
		for (int i = 0; i < PointCounter; i++) {
			Vector2 pos = Player.Center +
				Vector2.One.Vector2DistributeEvenly(PointCounter, 360, i)
				.RotatedBy(MathHelper.ToRadians(Player.GetModPlayer<BossRushUtilsPlayer>().counterToFullPi)) * 30;
			int dust = Dust.NewDust(pos, 0, 0, DustID.GemAmber);
			Main.dust[dust].velocity = Vector2.Zero;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].fadeIn = 0;
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		Trinket3_OnHitNPCEffect(hit);
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		Trinket3_OnHitNPCEffect(hit);
	}
	private void Trinket3_OnHitNPCEffect(NPC.HitInfo hit) {
		if (!Trinket_of_Ample_Perception)
			return;
		if (!hit.Crit)
			return;
		if (CountDown > 0)
			return;
		PointCounter = Math.Clamp(++PointCounter, 0, 4);
		PointTimeLeft = BossRushUtils.ToSecond(7);
		CountDown = BossRushUtils.ToSecond(2);
	}
}
