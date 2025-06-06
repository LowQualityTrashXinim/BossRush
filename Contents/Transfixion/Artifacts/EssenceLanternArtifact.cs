﻿using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems.ArtifactSystem;
using BossRush.Common.Global;

namespace BossRush.Contents.Transfixion.Artifacts;
internal class EssenceLanternArtifact : Artifact {
	public override string TexturePath => BossRushTexture.Get_MissingTexture("Artifact");
	public override Color DisplayNameColor => Color.AntiqueWhite;
}
public class EssenceLanternPlayer : ModPlayer {
	bool EssenceLantern = false;
	int Essence_CoolDown = 600;
	int Essence_CoolDownCounter = 0;
	public bool CurrentlyHaveWeaknessEssence = false;
	public override void ResetEffects() {
		EssenceLantern = Player.HasArtifact<EssenceLanternArtifact>();
		Essence_CoolDownCounter = BossRushUtils.CountDown(Essence_CoolDownCounter);
		CurrentlyHaveWeaknessEssence = false;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (CurrentlyHaveWeaknessEssence) {
			target.AddBuff<NPC_Weakness>(BossRushUtils.ToSecond(1));
		}
		if (EssenceLantern && Essence_CoolDownCounter <= 0) {
			Essence_CoolDownCounter = Essence_CoolDown + BossRushUtils.ToSecond(Main.rand.Next(-2, 3));
			int proj = Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), target.Center + Main.rand.NextVector2CircularEdge(target.width, target.height) + Main.rand.NextVector2CircularEdge(target.width, target.height) * Main.rand.NextFloat(.5f, 1f), Vector2.Zero, ModContent.ProjectileType<EssenceProjectile>(), 1, 0, Player.whoAmI);
			if (Main.projectile[proj].ModProjectile is EssenceProjectile essproj) {
				essproj.EssenceType = Main.rand.Next(11);
			}
		}
	}
}
public abstract class EssenceBuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void Update(Player player, ref int buffIndex) {
		UpdatePlayer(player);
		if (player.buffTime[buffIndex] == 0) {
			OnEnded(player);
		}
	}
	public virtual void UpdatePlayer(Player player) { }
	public virtual void OnEnded(Player player) { }
}
public class EssenceOfWrath : EssenceBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, Additive: 1.12f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, Base: -10);
	}
}
public class EssenceOfRage : EssenceBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritChance, Base: 5);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritDamage, Additive: 1.15f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, Base: -10);
	}
}
public class EssenceOfRejuvenate : EssenceBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.RegenHP, Additive: 1.27f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, Additive: 1 - .12f);
	}
	public override void OnEnded(Player player) {
		player.Heal(100);
	}
}
public class EssenceOfTitan : EssenceBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, Base: 15);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, 1 - .1f);
	}
}
public class EssenceOfSwift : EssenceBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MovementSpeed, Additive: 1.14f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.AttackSpeed, Additive: 1.08f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritChance, Base: -10);
	}
}
public class EssenceOfDrowsy : EssenceBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void UpdatePlayer(Player player) {
		player.endurance += .15f;
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MovementSpeed, Additive: 1 - .14f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.AttackSpeed, Additive: 1 - .06f);
	}
}
public class EssenceOfWeakness : EssenceBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<EssenceLanternPlayer>().CurrentlyHaveWeaknessEssence = true;
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, 1 - .1f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritDamage, 1 - .3f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, Base: -10);
	}
}
public class NPC_Weakness : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().VelocityMultiplier -= .35f;
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().StatDefense.Base -= 5;
	}
}
public class EssenceOfKindness : EssenceBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.HealEffectiveness, Additive: 1.25f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Iframe, Additive: 1.35f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, Additive: 1 - .12f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritChance, Base: -10);
	}
}
public class EssenceOfCalmness : EssenceBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritChance, Base: 25);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritDamage, Additive: 1 - .3f);
	}
}
public class EssenceOfWither : EssenceBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.RegenHP, Additive: 1 - .2f, Base: -18);
	}
}
public class EssenceProjectile : ModProjectile {
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 0;
		ProjectileID.Sets.TrailCacheLength[Type] = 20;
	}
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = BossRushUtils.ToSecond(45);
		Projectile.penetrate = -1;
		Projectile.extraUpdates = 3;
	}
	public override bool? CanDamage() => false;
	public int EssenceType = -1;
	public override void AI() {
		Projectile.alpha = (int)MathHelper.Lerp(255, 0, Projectile.timeLeft / (float)BossRushUtils.ToSecond(45));
		Player player = Main.player[Projectile.owner];
		if (Projectile.Center.IsCloseToPosition(player.Center, 175)) {
			Projectile.velocity += (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * .05f;
			Projectile.velocity = Projectile.velocity.LimitedVelocity(1);
		}
		else {
			Projectile.velocity *= .98f;
		}
		OnContactWithPlayer(player);
	}
	public override Color? GetAlpha(Color lightColor) {
		return EssenceColor();
	}
	public Color EssenceColor() {
		switch (EssenceType) {
			case 0:
				return Color.Red;
			case 1:
				return Color.Orange;
			case 2:
				return Color.Blue;
			case 3:
				return Color.LightGreen;
			case 4:
				return Color.Yellow;
			case 5:
				return new(100, 100, 0);
			case 6:
				return Color.DarkRed;
			case 7:
				return Color.DarkBlue;
			case 8:
				return Color.Gray;
			case 9:
				return Color.DarkOrange;
			default:
				return Color.Black;
		}
	}
	private void OnContactWithPlayer(Player player) {
		if (Projectile.Center.IsCloseToPosition(player.Center, 15)) {
			switch (EssenceType) {
				case 0:
					player.AddBuff(ModContent.BuffType<EssenceOfWrath>(), BossRushUtils.ToSecond(9));
					break;
				case 1:
					player.AddBuff(ModContent.BuffType<EssenceOfRage>(), BossRushUtils.ToSecond(9));
					break;
				case 2:
					player.AddBuff(ModContent.BuffType<EssenceOfTitan>(), BossRushUtils.ToSecond(9));
					break;
				case 3:
					player.AddBuff(ModContent.BuffType<EssenceOfRejuvenate>(), BossRushUtils.ToSecond(9));
					break;
				case 4:
					player.AddBuff(ModContent.BuffType<EssenceOfSwift>(), BossRushUtils.ToSecond(9));
					break;
				case 5:
					player.AddBuff(ModContent.BuffType<EssenceOfDrowsy>(), BossRushUtils.ToSecond(9));
					break;
				case 6:
					player.AddBuff(ModContent.BuffType<EssenceOfKindness>(), BossRushUtils.ToSecond(9));
					break;
				case 7:
					player.AddBuff(ModContent.BuffType<EssenceOfWeakness>(), BossRushUtils.ToSecond(9));
					break;
				case 8:
					player.AddBuff(ModContent.BuffType<EssenceOfWither>(), BossRushUtils.ToSecond(9));
					break;
				case 9:
					player.AddBuff(ModContent.BuffType<EssenceOfCalmness>(), BossRushUtils.ToSecond(9));
					break;
				default:
					player.statLife = Math.Clamp(player.statLife - 25, 0, player.statLifeMax2);
					break;
			}
			Projectile.Kill();
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrailWithoutColorAdjustment(EssenceColor(), .05f);
		return base.PreDraw(ref lightColor);
	}
}
