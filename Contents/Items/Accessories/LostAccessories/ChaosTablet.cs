using Terraria;
using Terraria.ID;
using System.Linq;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Utils;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class ChaosTablet : ModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("LostAcc");
	public override void SetDefaults() {
		Item.Set_LostAccessory(32, 32);
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<ChaosTabletPlayer>().ChaosTablet = true;
	}
}
class ChaosBuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		ChaosTabletPlayer chaosplayer = player.GetModPlayer<ChaosTabletPlayer>();
		PlayerStatsHandle statsplayer = player.GetModPlayer<PlayerStatsHandle>();
		var modplayer = player.GetModPlayer<MysteriousPotionPlayer>();
		switch (chaosplayer.chaosstat) {
			case PlayerStats.MaxHP:
			case PlayerStats.RegenHP:
			case PlayerStats.MaxMana:
			case PlayerStats.RegenMana:
			case PlayerStats.CritChance:
			case PlayerStats.MaxMinion:
			case PlayerStats.Defense:
			case PlayerStats.MaxSentry:
				statsplayer.AddStatsToPlayer(chaosplayer.chaosstat,
					Base: modplayer.ToStatsNumInt(chaosplayer.chaosstat, 2));
				break;
			default:
				statsplayer.AddStatsToPlayer(chaosplayer.chaosstat,
					Additive: 1 + modplayer.ToStatsNumFloat(chaosplayer.chaosstat, 2));
				break;
		}
		if (player.buffTime[buffIndex] <= 0) {
			player.GetModPlayer<ChaosTabletPlayer>().chaosstat = PlayerStats.None;
		}
	}
	public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) {
		ChaosTabletPlayer chaosplayer = Main.LocalPlayer.GetModPlayer<ChaosTabletPlayer>();
		MysteriousPotionPlayer modplayer = Main.LocalPlayer.GetModPlayer<MysteriousPotionPlayer>();
		if (BossRushUtils.DoesStatsRequiredWholeNumber(chaosplayer.chaosstat)) {
			tip = $"+ {modplayer.ToStatsNumInt(chaosplayer.chaosstat,1)} {chaosplayer.chaosstat}";
		}
		else {
			tip = $"+ {modplayer.ToStatsNumInt(chaosplayer.chaosstat,1)}% {chaosplayer.chaosstat}";
		}
	}
	public override bool RightClick(int buffIndex) {
		return false;
	}
}
class ChaosTabletPlayer : ModPlayer {
	public bool ChaosTablet = false;
	public PlayerStats chaosstat = PlayerStats.None;
	public override void ResetEffects() {
		ChaosTablet = false;
	}
	public override void UpdateEquips() {
		if (!ChaosTablet) {
			return;
		}
		if (chaosstat == PlayerStats.None) {
			chaosstat = Main.rand.Next(MysteriousPotionBuff.lookupDictionary.Keys.ToList());
			Player.AddBuff(ModContent.BuffType<ChaosBuff>(), BossRushUtils.ToSecond(15));
		}
		if (!Player.Center.LookForAnyHostileNPC(600)) {
			return;
		}
		if (Main.rand.NextBool(100)) {
			int weapondmg = Player.GetWeaponDamage(Player.HeldItem);
			int dmg = Main.rand.Next(40, 180) + weapondmg;
			int buffid = Main.rand.Next(TerrariaArrayID.Debuff);
			Projectile.NewProjectile(Player.GetSource_Misc("ChaosTablet"), Player.Center + Main.rand.NextVector2Circular(600, 600), Vector2.Zero, ModContent.ProjectileType<ChaosExplosion>(), dmg, Main.rand.NextFloat(3, 9), Player.whoAmI, buffid);
		}
	}
}
class ChaosExplosion : ModProjectile {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 1;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 10;
		Projectile.hide = true;
	}
	public int BuffID { get => (int)Projectile.ai[0]; }
	public override bool? CanDamage() {
		return false;
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 100; i++) {
			int firework = Main.rand.Next(new int[] { DustID.Firework_Blue, DustID.Firework_Green, DustID.Firework_Pink, DustID.Firework_Red, DustID.Firework_Yellow });
			int dust = Dust.NewDust(Projectile.Center, 0, 0, firework);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(20, 20);
		}
		for (int i = 0; i < 50; i++) {
			int firework = Main.rand.Next(new int[] { DustID.Firework_Blue, DustID.Firework_Green, DustID.Firework_Pink, DustID.Firework_Red, DustID.Firework_Yellow });
			int dust = Dust.NewDust(Projectile.Center, 0, 0, firework);
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
