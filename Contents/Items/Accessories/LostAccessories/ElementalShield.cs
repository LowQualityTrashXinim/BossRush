using Terraria;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using Terraria.ID;
using System.Collections.Generic;
using System;
using BossRush.Texture;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class ElementalShield : ModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("LostAcc");
	public override void SetDefaults() {
		Item.Set_LostAccessory(32, 32);
		Item.Set_ShieldStats(500, 3);
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<ElementalShieldPlayer>().ElementalShield = true;
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.Defense, Additive: 1.1f, Flat: 12);
	}
}
public class ElementalShieldPlayer : ModPlayer {
	public bool ElementalShield = false;
	public override void ResetEffects() {
		ElementalShield = false;
	}
	public override void UpdateEquips() {
		if (!ElementalShield) {
			return;
		}
		PlayerStatsHandle statplayer = Player.GetModPlayer<PlayerStatsHandle>();
		if (Player.HasBuff(BuffID.OnFire) || Player.HasBuff(BuffID.OnFire3) || Player.HasBuff(BuffID.Burning)) {
			statplayer.AddStatsToPlayer(PlayerStats.RegenHP, Base: 10);
		}
		if (Player.HasBuff(BuffID.Frostburn) || Player.HasBuff(BuffID.Frostburn2) || Player.HasBuff(BuffID.Chilled) || Player.HasBuff(BuffID.Frozen)) {
			statplayer.AddStatsToPlayer(PlayerStats.Defense, Additive: 1.2f);
		}
		if (Player.HasBuff(BuffID.Poisoned) || Player.HasBuff(BuffID.Venom)) {
			statplayer.AddStatsToPlayer(PlayerStats.PureDamage, Additive: 1.15f);
		}
		if (Player.HasBuff(BuffID.CursedInferno)) {
			statplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, Additive: 1.2f);
		}
		if (Player.HasBuff(BuffID.Ichor)) {
			Player.endurance += .1f;
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (!ElementalShield) {
			return;
		}
		int min = Math.Min(40, hurtInfo.Damage);
		int max = Math.Max(40, hurtInfo.Damage);
		ElementalExplosion(Main.rand.Next(min - 1, max));
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (!ElementalShield) {
			return;
		}
		int min = Math.Min(40, hurtInfo.Damage);
		int max = Math.Max(40, hurtInfo.Damage);
		ElementalExplosion(Main.rand.Next(min - 1, max));
	}
	private void ElementalExplosion(int damage) {
		if(damage <= 0) {
			damage = 1;
		}
		Player.Center.LookForHostileNPC(out List<NPC> npclist, 150f);
		foreach (NPC npc in npclist) {
			npc.AddBuff(BuffID.OnFire3, BossRushUtils.ToSecond(Main.rand.Next(1, 6)));
			npc.AddBuff(BuffID.Frostburn2, BossRushUtils.ToSecond(Main.rand.Next(1, 6)));
			npc.AddBuff(BuffID.Venom, BossRushUtils.ToSecond(Main.rand.Next(1, 6)));
			npc.AddBuff(BuffID.CursedInferno, BossRushUtils.ToSecond(Main.rand.Next(1, 6)));
			npc.AddBuff(BuffID.Ichor, BossRushUtils.ToSecond(Main.rand.Next(1, 6)));
			npc.AddBuff(BuffID.Poisoned, BossRushUtils.ToSecond(Main.rand.Next(1, 6)));
			Player.StrikeNPCDirect(npc, npc.CalculateHitInfo(damage, BossRushUtils.DirectionFromPlayerToNPC(Player.Center.X, npc.Center.X), false, 10f));
		}
	}
}
