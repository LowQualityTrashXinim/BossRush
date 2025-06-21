using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Contents.Perks;
using System;
using Humanizer;
using BossRush.Common.Systems.Achievement;
using BossRush.Common.Global;

namespace BossRush.Contents.Transfixion.Artifacts;
internal class TokenOfWrathArtifact : Artifact {
	public override string TexturePath => BossRushTexture.Get_MissingTexture("Artifact");
	public override Color DisplayNameColor => Color.LimeGreen;
}
public class TokenOfWrathPlayer : ModPlayer {
	bool TokenOfWrath = false;
	public override void ResetEffects() {
		TokenOfWrath = Player.HasArtifact<TokenOfWrathArtifact>();
	}
	public override void UpdateEquips() {
		if (!TokenOfWrath) {
			return;
		}
		PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.1f);
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, .25f);
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 50);
		modplayer.NonCriticalDamage += .5f;
	}
}
public class StrikeOfFury : Perk {
	public override bool SelectChoosing() {
		return Artifact.PlayerCurrentArtifact<TokenOfWrathArtifact>() || AchievementSystem.IsAchieved("TokenOfWrath");
	}
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 3;
		list_category.Add(PerkCategory.ArtifactExclusive);
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.NonCriticalDamage += .25f * StackAmount(player);
		modplayer.UpdateCritDamage -= .1f;
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: -20);
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (player.HasBuff<FuryStrike>()
			&& player.GetModPlayer<PlayerStatsHandle>().ModifyHit_Before_Crit) {
			modifiers.ArmorPenetration += 10;
		}
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		if (player.HasBuff<FuryStrike>()
			&& player.GetModPlayer<PlayerStatsHandle>().ModifyHit_Before_Crit) {
			modifiers.ArmorPenetration += 10;
		}
	}
	public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		CriticalEffect(player, target, hit);
	}
	public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		CriticalEffect(player, target, hit);
	}
	private void CriticalEffect(Player player, NPC target, NPC.HitInfo hit) {
		if (!player.HasBuff<FuryStrike>()) {
			if (Main.rand.NextFloat() <= .1f + .1f * StackAmount(player)) {
				player.AddBuff(ModContent.BuffType<FuryStrike>(), BossRushUtils.ToSecond(Main.rand.Next(3, 8)));
			}
		}
		else {
			if (Main.rand.NextFloat() <= Math.Clamp(.01f * StackAmount(player), 0, .25f)) {
				player.StrikeNPCDirect(target, hit);
			}
		}
		if (Main.rand.NextFloat() <= .05f * StackAmount(player)) {
			NPC.HitInfo newinfo = new NPC.HitInfo();
			newinfo.Damage = 45;
			newinfo.Crit = true;
			newinfo.DamageType = DamageClass.Default;
			player.StrikeNPCDirect(target, newinfo);
		}
	}
}
public class FuryStrike : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<PlayerStatsHandle>().NonCriticalDamage += .2f;
		player.GetModPlayer<PlayerStatsHandle>().UpdateCritDamage += .5f;
	}
	public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) {
		tip = tip.FormatWith(1);
		if (Main.LocalPlayer.TryGetModPlayer(out PerkPlayer perkplayer)) {
			if (perkplayer.perks.ContainsKey(Perk.GetPerkType<StrikeOfFury>()))
				tip = tip.FormatWith(perkplayer.perks[Perk.GetPerkType<StrikeOfFury>()]);
		}
	}
}

public class RuthlessRage : Perk {
	public override bool SelectChoosing() {
		return Artifact.PlayerCurrentArtifact<TokenOfWrathArtifact>() || AchievementSystem.IsAchieved("TokenOfWrath");
	}
	public override void SetDefaults() {
		CanBeStack = true;
		StackLimit = 3;
		list_category.Add(PerkCategory.ArtifactExclusive);
	}
	public override void UpdateEquip(Player player) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: -20 * StackAmount(player));
		modplayer.NonCriticalDamage += .35f * StackAmount(player);
		modplayer.UpdateCritDamage += .55f * StackAmount(player);
	}
	public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		if (Main.rand.NextFloat() <= .05f * StackAmount(player)) {
			modifiers.FinalDamage *= 2;
		}
		if (Main.rand.NextFloat() <= .01f || modplayer.ModifyHit_Before_Crit && Main.rand.NextFloat() <= .05f) {
			modifiers.FinalDamage *= 4;
		}
	}
	public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		if (Main.rand.NextFloat() <= .05f * StackAmount(player)) {
			modifiers.FinalDamage *= 2;
		}
		if (Main.rand.NextFloat() <= .01f || modplayer.ModifyHit_Before_Crit && Main.rand.NextFloat() <= .05f) {
			modifiers.FinalDamage *= 4;
		}
	}
	public override void OnHitByAnything(Player player) {
		if (Main.rand.NextFloat() <= .2f * StackAmount(player) && !player.HasBuff<RageEmpowerment>()) {
			player.AddBuff(ModContent.BuffType<RageEmpowerment>(), BossRushUtils.ToSecond(Main.rand.Next(1, 10)));
		}
	}
}
public class RageEmpowerment : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AlwaysCritValue++;
	}
}
