using System;
using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Accessories.TrinketAccessories;
internal class Trinket6 : BaseTrinket {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void UpdateTrinket(Player player, TrinketPlayer modplayer) {
		player.DefenseEffectiveness *= 1.5f;
		player.GetModPlayer<Trinket6_ModPlayer>().Trinket6 = true;
	}
}
public class Trinket6_ModPlayer : ModPlayer {
	public bool Trinket6 = false;
	public int Trinket6_Stack = 0;
	public int Trinket6_StackDecay = 0;
	public int Trinket6_StackLossses = 0;
	public override void ResetEffects() {
		Trinket6 = false;
		if (Trinket6_Stack > 0) {
			if (--Trinket6_StackDecay <= 0) {
				Trinket6_Stack--;
				Trinket6_StackLossses++;
				Trinket6_StackDecay = BossRushUtils.ToSecond(5);
				Player.AddBuff(ModContent.BuffType<Trinket6_DefensesBonus_Buff>(), BossRushUtils.ToMinute(1));
			}
		}
	}
	public override void UpdateEquips() {
		if (Trinket6) {
			if (Trinket6_Stack > 0) {
				Player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, -.1f * Trinket6_Stack);
			}
			Player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, Multiplicative: Player.DefenseEffectiveness.Value);
		}
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (Trinket6) {
			Trinket6_Stack = Math.Clamp(Trinket6_Stack + 1, 0, 5);
			Trinket6_StackDecay = BossRushUtils.ToSecond(5);
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (Trinket6) {
			Trinket6_Stack = Math.Clamp(Trinket6_Stack + 1, 0, 5);
			Trinket6_StackDecay = BossRushUtils.ToSecond(5);
		}
	}

}
public class Trinket6_DefensesBonus_Buff : TrinketBuff {
	public override void TrinketSetStaticDefaults() {
		Main.debuff[Type] = false;
	}
	public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) {
		Trinket6_ModPlayer trinketplayer = Main.LocalPlayer.GetModPlayer<Trinket6_ModPlayer>();
		tip = $"Compenstate value :{trinketplayer.Trinket6_StackLossses}\nCurrent Stack{trinketplayer.Trinket6_Stack}";
	}
	public override bool ReApply(Player player, int time, int buffIndex) {
		player.buffTime[buffIndex] = time;
		return true;
	}
	public override void UpdateTrinketPlayer(Player player, TrinketPlayer modplayer, ref int buffIndex) {
		Trinket6_ModPlayer trinketplayer = player.GetModPlayer<Trinket6_ModPlayer>();
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, Base: 5 * trinketplayer.Trinket6_StackLossses);
	}
	public override void OnEnded(Player player) {
		Trinket6_ModPlayer trinketplayer = player.GetModPlayer<Trinket6_ModPlayer>();
		trinketplayer.Trinket6_StackLossses = 0;
	}
}
