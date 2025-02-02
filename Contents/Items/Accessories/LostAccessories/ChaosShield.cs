using Terraria;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using Microsoft.Xna.Framework;
using BossRush.Common.Utils;
using System;
using BossRush.Texture;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class ChaosShield : ModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("LostAcc");
	public override void SetDefaults() {
		Item.Set_LostAccessory(32, 32);
		Item.Set_ShieldStats(500, 3);
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.Defense, Base: 5);
		player.GetModPlayer<ChaosShieldPlayer>().ChaosShield = true;
	}
}
public class ChaosShieldPlayer : ModPlayer {
	public bool ChaosShield = false;
	public override void ResetEffects() {
		ChaosShield = false;
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (ChaosShield) {
			modifiers.SourceDamage -= Main.rand.NextFloat(.3f, .5f);
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (ChaosShield) {
			modifiers.SourceDamage -= Main.rand.NextFloat(.3f, .5f);
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (ChaosShield) {
			if (Main.rand.NextBool(10)) {
				Player.Heal(hurtInfo.Damage * 2);
			}
			SpawnProjectile(hurtInfo.Damage);
		}
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (ChaosShield && Main.rand.NextBool(10)) {
			if (Main.rand.NextBool(10)) {
				Player.Heal(hurtInfo.Damage * 2);
			}
			SpawnProjectile(hurtInfo.Damage);
		}
	}
	private void SpawnProjectile(int Damage) {
		if (Main.rand.NextBool(4)) {
			Vector2 pos = Player.Center + Main.rand.NextVector2Circular(400, 400);
			Vector2 vel = Main.rand.NextVector2CircularEdge(10, 10);
			int min = Math.Min(10, Damage);
			int max = Math.Max(10, Damage);
			int damageraw = Main.rand.Next(min -1, max);
			if(damageraw <= 0) {
				damageraw = 1;
			}
			Projectile.NewProjectile(Player.GetSource_FromThis(), pos, vel, Main.rand.Next(TerrariaArrayID.UltimateProjPack), damageraw, Main.rand.NextFloat(2, 5), Player.whoAmI);
		}
	}
}
