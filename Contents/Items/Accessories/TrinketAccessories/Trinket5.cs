using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Accessories.Trinket;
internal class Trinket5 : BaseTrinket {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void UpdateTrinket(Player player, TrinketPlayer modplayer) {
		player.statDefense += 25;
		player.GetModPlayer<Trinket5_ModPlayer>().Trinket5 = true;
	}
}
public class Trinket5_ModPlayer : ModPlayer {
	public bool Trinket5 = false;
	public int HitCount = 0;
	public int CoolDown = 0;
	public int Delay = 0;
	public bool JustEnded = false;
	public override void ResetEffects() {
		Trinket5 = false;
		if (!Player.HasBuff<Trinket5_Buff>()) {
			HitCount = 0;
			CoolDown = BossRushUtils.CountDown(CoolDown);
		}
	}
	public override void PostUpdate() {
		Delay = BossRushUtils.CountDown(Delay);
		if (!Player.HasBuff<Trinket5_Buff>() && JustEnded) {
			if (Player.statLifeMax2 <= Player.statDefense) {
				for (int i = 0; i < 300; i++) {
					int smokedust = Dust.NewDust(Player.Center, 0, 0, DustID.Smoke);
					Main.dust[smokedust].noGravity = true;
					Main.dust[smokedust].velocity = Main.rand.NextVector2Circular(25, 25);
					Main.dust[smokedust].scale = Main.rand.NextFloat(.75f, 2f);
					int dust = Dust.NewDust(Player.Center, 0, 0, DustID.Torch);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = Main.rand.NextVector2Circular(25, 25);
					Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
				}
				int leftover = Player.statDefense - Player.statLifeMax2;
				Player.Center.LookForHostileNPC(out List<NPC> npclist, 500);
				foreach (NPC npc in npclist) {
					Player.StrikeNPCDirect(npc, npc.CalculateHitInfo(leftover * 100, Player.Center.X > npc.Center.X ? -1 : 1, true, 20, damageVariation: true));
				}
			}
			Player.Heal(Player.statDefense);
			JustEnded = false;
		}
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		Check_Buff();
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		Check_Buff();
	}
	private void Check_Buff() {
		if (CoolDown <= 0 && Trinket5) {
			Player.AddBuff(ModContent.BuffType<Trinket5_Buff>(), BossRushUtils.ToSecond(10));
			CoolDown = BossRushUtils.ToSecond(30);
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Player.HasBuff<Trinket5_Buff>() && Delay <= 0 && Trinket5) {
			HitCount++;
			Delay = 6;
		}
	}
}
public class Trinket5_Buff : TrinketBuff {
	public override void TrinketSetStaticDefaults() {
		Main.debuff[Type] = false;
	}
	public override void UpdateTrinketPlayer(Player player, TrinketPlayer modplayer, ref int buffIndex) {
		player.statDefense += player.GetModPlayer<Trinket5_ModPlayer>().HitCount;
		modplayer.DamageStats += player.statDefense * .001f;
	}
	public override void OnEnded(Player player) {
		player.GetModPlayer<Trinket5_ModPlayer>().HitCount = 0;
		player.GetModPlayer<Trinket5_ModPlayer>().JustEnded = true;
	}
}
