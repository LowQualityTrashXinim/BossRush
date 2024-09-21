using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Utils;
using System.Diagnostics;
using Terraria.ID;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class ChaosTablet : ModItem {
	public override void SetDefaults() {
		Item.Set_LostAccessory(32, 32);
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<ChaosTabletPlayer>().ChaosTablet = true;
	}
}
class ChaosTabletPlayer : ModPlayer {
	public bool ChaosTablet = false;
	public override void ResetEffects() {
		ChaosTablet = false;
	}
	public override void UpdateEquips() {
		if (!ChaosTablet) {
			return;
		}
		if (Main.rand.NextBool(100)) {
			int weapondmg = Player.GetWeaponDamage(Player.HeldItem);
			int dmg = Main.rand.Next(40, 90) + weapondmg;
			int buffid = Main.rand.Next(TerrariaArrayID.Debuff);
			Projectile.NewProjectile(Player.GetSource_Misc("ChaosTablet"), Player.Center + Main.rand.NextVector2Circular(600, 600), Vector2.Zero, ModContent.ProjectileType<ChaosExplosion>(), dmg, Main.rand.NextFloat(3, 9), buffid);
		}
	}
}
class ChaosExplosion : ModProjectile {
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 1;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 10;
	}
	public int BuffID { get => (int)Projectile.ai[0]; }
	public override bool? CanDamage() {
		return false;
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 100; i++) {
			int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.FireworksRGB);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(20, 20);
		}
		for (int i = 0; i < 50; i++) {
			int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.FireworksRGB);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(20, 20);
		}
		Projectile.Center.LookForHostileNPC(out List<NPC> npclist, 200);
		Player player = Main.player[Projectile.owner];
		NPC.HitInfo info = new();
		info.Damage = Projectile.damage;
		info.Knockback = Projectile.knockBack;
		foreach (NPC npc in npclist) {
			info.HitDirection = BossRushUtils.DirectionFromPlayerToNPC(Projectile.Center.X, npc.Center.X);
			info.Crit = Main.rand.NextBool(7);
			player.StrikeNPCDirect(npc, info);
			npc.AddBuff(BuffID, BossRushUtils.ToSecond(Main.rand.Next(5, 16)));
		}
	}
}
