using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using System;
using BossRush.Common.Systems;

namespace BossRush.Contents.Items.Accessories.TrinketAccessories;
public class Trinket_of_Swift_Health : BaseTrinket {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void TrinketDefault() {
		Item.Set_InfoItem(true);
	}
	public override void UpdateTrinket(Player player, TrinketPlayer modplayer) {
		player.GetModPlayer<Trinket_of_Swift_Health_ModPlayer>().Trinket_of_Swift_Health = true;
		modplayer.GetStatsHandle().AddStatsToPlayer(PlayerStats.MaxHP, 1.25f);
	}
}
public class SwiftSteal_Buff : TrinketBuff {
	public override void UpdateTrinketPlayer(Player player, TrinketPlayer modplayer, ref int buffIndex) {
		int Point = player.GetModPlayer<Trinket_of_Swift_Health_ModPlayer>().Trinket_of_Swift_Health_PointCounter;
		PlayerStatsHandle statsplayer = modplayer.GetStatsHandle();
		statsplayer.AddStatsToPlayer(PlayerStats.MaxHP, 1 + .05f * Point);
		statsplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1 + .1f * Point);
		statsplayer.AddStatsToPlayer(PlayerStats.PureDamage, Base: player.statLife * .05f);
	}
	public override void OnEnded(Player player) {
		player.GetModPlayer<Trinket_of_Swift_Health_ModPlayer>().Trinket_of_Swift_Health_CoolDown = BossRushUtils.ToSecond(60);
	}
}
public class Trinket_of_Swift_Health_ModPlayer : ModPlayer {
	public bool Trinket_of_Swift_Health = false;
	public int Trinket_of_Swift_Health_PointCounter = 0;
	public int Trinket_of_Swift_Health_CoolDown = 0;
	public int Trinket_of_Swift_Health_DelayBetweenEachHit = 0;
	public override void ResetEffects() {
		Trinket_of_Swift_Health = false;
	}
	public override void PreUpdate() {
		Trinket_of_Swift_Health_DelayBetweenEachHit = BossRushUtils.CountDown(Trinket_of_Swift_Health_DelayBetweenEachHit);
		if (!Player.HasBuff(ModContent.BuffType<SwiftSteal_Buff>())) {
			Trinket_of_Swift_Health_PointCounter = 0;
			Trinket_of_Swift_Health_CoolDown = BossRushUtils.CountDown(Trinket_of_Swift_Health_CoolDown);
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		Trinket_of_Swift_Health_OnHitEffect();
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		Trinket_of_Swift_Health_OnHitEffect();
	}
	private void Trinket_of_Swift_Health_OnHitEffect() {
		if (Trinket_of_Swift_Health_DelayBetweenEachHit > 0)
			return;
		if (Trinket_of_Swift_Health_CoolDown > 0) {
			return;
		}
		if (!Trinket_of_Swift_Health)
			return;
		if (Player.HasBuff(ModContent.BuffType<SwiftSteal_Buff>())) {
			Trinket_of_Swift_Health_DelayBetweenEachHit = BossRushUtils.ToSecond(2);
			Trinket_of_Swift_Health_PointCounter = Math.Clamp(++Trinket_of_Swift_Health_PointCounter, 0, 6);
		}
		else {
			Player.AddBuff(ModContent.BuffType<SwiftSteal_Buff>(), BossRushUtils.ToSecond(30));
		}
	}
}
