using System;
using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.Trinket;
internal class Trinket_of_Perpetuation : BaseTrinket {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void UpdateTrinket(Player player, TrinketPlayer modplayer) {
		player.GetModPlayer<Trinket_of_Perpetuation_ModPlayer>().Trinket_of_Perpetuation = true;
		player.GetAttackSpeed(DamageClass.Generic) += .15f;
	}
}
public class Samsara_of_Retribution : TrinketBuff {
	public override bool ReApply(NPC npc, int time, int buffIndex) {
		npc.GetGlobalNPC<Trinket_GlobalNPC>().Trinket_of_Perpetuation_PointStack = Math.Clamp(++npc.GetGlobalNPC<Trinket_GlobalNPC>().Trinket_of_Perpetuation_PointStack, 0, 10);
		return base.ReApply(npc, time, buffIndex);
	}
	public override void UpdateTrinketNPC(NPC npc) {
		npc.lifeRegen -= 10 + npc.GetGlobalNPC<Trinket_GlobalNPC>().Trinket_of_Perpetuation_PointStack * 10;
	}
	public override void OnEnded(NPC npc) {
		npc.GetGlobalNPC<Trinket_GlobalNPC>().Trinket_of_Perpetuation_PointStack = 0;
	}
}
public class Trinket_of_Perpetuation_ModPlayer : ModPlayer {
	public bool Trinket_of_Perpetuation = false;
	public override void ResetEffects() {
		Trinket_of_Perpetuation = false;
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		Trinket_of_Perpetuation_OnHitNPCEffect(target, hit);
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		Trinket_of_Perpetuation_OnHitNPCEffect(target, hit);
	}
	private void Trinket_of_Perpetuation_OnHitNPCEffect(NPC target, NPC.HitInfo hit) {
		if (!Trinket_of_Perpetuation)
			return;
		target.AddBuff(ModContent.BuffType<Samsara_of_Retribution>(), BossRushUtils.ToSecond(1));
		if (hit.Crit) {
			NPC.HitInfo hitExtra = hit;
			hitExtra.Crit = false;
			hitExtra.Damage += (int)(hitExtra.Damage * target.GetGlobalNPC<Trinket_GlobalNPC>().Trinket_of_Perpetuation_PointStack * .1f);
			Player.StrikeNPCDirect(target, hitExtra);
		}
	}
}
