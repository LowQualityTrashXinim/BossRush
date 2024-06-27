using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Systems;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems.ArtifactSystem;

namespace BossRush.Contents.Artifacts;
internal class EssenceLanternArtifact : Artifact {
	public override string TexturePath => BossRushTexture.MISSINGTEXTURE;
	public override Color DisplayNameColor => Color.AntiqueWhite;
}
public class EssenceLanternPlayer : ModPlayer {
	bool EssenceLantern = false;
	public override void ResetEffects() {
		EssenceLantern = Player.HasArtifact<EssenceLanternArtifact>();
	}
	public override void PostItemCheck() {
		//if (EssenceLantern)
		//	Player.HeldItem.DamageType = DamageClass.Generic;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(7) && EssenceLantern) {
			int proj = Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), target.Center + Main.rand.NextVector2Circular(target.width, target.height), Vector2.Zero, ModContent.ProjectileType<EssenceProjectile>(), 1, 0, Player.whoAmI);
			if (Main.projectile[proj].ModProjectile is EssenceProjectile essproj) {
				essproj.EssenceType = Main.rand.Next(11);
			}
		}
	}
}
public abstract class EssenceBuff : ModBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
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
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, Multiplicative: 1.2f);
	}
}
public class EssenceOfRage : EssenceBuff {
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritChance, Additive: .1f);
	}
}
public class EssenceOfRejuvenate : EssenceBuff {
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.RegenHP, Additive: .67f);
	}
	public override void OnEnded(Player player) {
		player.Heal(100);
	}
}
public class EssenceOfTitan : EssenceBuff {
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, Additive: .2f);
	}
}
public class EssenceOfSwift : EssenceBuff {
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MovementSpeed, Additive: .34f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.AttackSpeed, Additive: .14f);
	}
}
public class EssenceOfDrowsy : EssenceBuff {
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MovementSpeed, Additive: -.44f);
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.AttackSpeed, Additive: -.24f);
	}
}
public class EssenceOfWeakness : EssenceBuff {
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, Base: -20);
	}
}
public class EssenceOfKindness : EssenceBuff {
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, Additive: -.5f);
	}
}
public class EssenceOfCalmness : EssenceBuff {
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritDamage, Additive: -.5f);
	}
}
public class EssenceOfWither : EssenceBuff {
	public override void UpdatePlayer(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.RegenHP, Additive: -1, Base: -18);
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
		Projectile.timeLeft = BossRushUtils.ToSecond(15);
		Projectile.penetrate = -1;
		Projectile.extraUpdates = 3;
	}
	public override bool? CanDamage() => false;
	public int EssenceType = -1;
	public override void AI() {
		Projectile.alpha = (int)MathHelper.Lerp(255, 0, Projectile.timeLeft / (float)BossRushUtils.ToSecond(15));
		Player player = Main.player[Projectile.owner];
		if (Projectile.Center.IsCloseToPosition(player.Center, 225)) {
			Projectile.velocity += (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * .05f;
			Projectile.velocity = Projectile.velocity.LimitedVelocity(5);
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
				return Color.Green;
			case 5:
				return Color.MediumPurple;
			case 6:
				return Color.DarkSeaGreen;
			case 7:
				return Color.DeepPink;
			case 8:
				return Color.Gray;
			case 9:
				return Color.DarkBlue;
			default:
				return Color.Black;
		}
	}
	private void OnContactWithPlayer(Player player) {
		if (Projectile.Center.IsCloseToPosition(player.Center, 15)) {
			switch (EssenceType) {
				case 0:
					player.AddBuff(ModContent.BuffType<EssenceOfWrath>(), BossRushUtils.ToSecond(6));
					break;
				case 1:
					player.AddBuff(ModContent.BuffType<EssenceOfRage>(), BossRushUtils.ToSecond(6));
					break;
				case 2:
					player.AddBuff(ModContent.BuffType<EssenceOfTitan>(), BossRushUtils.ToSecond(6));
					break;
				case 3:
					player.AddBuff(ModContent.BuffType<EssenceOfRejuvenate>(), BossRushUtils.ToSecond(6));
					break;
				case 4:
					player.AddBuff(ModContent.BuffType<EssenceOfSwift>(), BossRushUtils.ToSecond(6));
					break;
				case 5:
					player.AddBuff(ModContent.BuffType<EssenceOfDrowsy>(), BossRushUtils.ToSecond(6));
					break;
				case 6:
					player.AddBuff(ModContent.BuffType<EssenceOfKindness>(), BossRushUtils.ToSecond(6));
					break;
				case 7:
					player.AddBuff(ModContent.BuffType<EssenceOfWeakness>(), BossRushUtils.ToSecond(6));
					break;
				case 8:
					player.AddBuff(ModContent.BuffType<EssenceOfWither>(), BossRushUtils.ToSecond(6));
					break;
				case 9:
					player.AddBuff(ModContent.BuffType<EssenceOfCalmness>(), BossRushUtils.ToSecond(6));
					break;
				default:
					Math.Clamp(player.statLife - 25, 0, player.statLifeMax2);
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
