using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;
using System.Collections.Generic;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class SpiritEmblem : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
		Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.MaxMinion, Base: 1);
		modplayer.AddStatsToPlayer(PlayerStats.MaxSentry, Base: 1);
		player.GetModPlayer<SpiritEmblemPlayer>().SpiritEmblem = true;
	}
}
public class SpiritEmblemPlayer : ModPlayer {
	public bool SpiritEmblem = false;
	int counter = 280;
	public override void ResetEffects() {
		SpiritEmblem = false;
	}
	public override void UpdateEquips() {
		if (!SpiritEmblem) {
			return;
		}
		for (int i = 0; i < Main.maxProjectiles; i++) {
			Projectile projectile = Main.projectile[i];
			if (!projectile.active) {
				continue;
			}
			if (!projectile.minion || Player.whoAmI != projectile.owner) {
				continue;
			}
			float PlayerDisToMinion = Vector2.DistanceSquared(Player.Center, projectile.Center);
			if (PlayerDisToMinion < 275 * 275) {
				Player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, Base: 2);
			}
		}
	}
	public override void PostUpdate() {
		if (!SpiritEmblem) {
			return;
		}
		Player.Center.LookForHostileNPC(out List<NPC> npclist, 150f);
		if (npclist.Count < 1) {
			return;
		}
		if (++counter >= 120) {
			counter = 0;
			foreach (NPC npc in npclist) {
				npc.AddBuff(BuffID.Confused, 150);
			}
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!SpiritEmblem) {
			return;
		}
		if (proj.DamageType == DamageClass.SummonMeleeSpeed) {
			target.AddBuff(ModContent.BuffType<Crystalized>(), BossRushUtils.ToSecond(5));
		}
	}
}
public class Crystalized : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = true;

	}
	public override void Update(NPC npc, ref int buffIndex) {
	}
}
