﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.Swotaff {
	internal class RubySwotaff : SwotaffGemItem {
		public override void PreSetDefaults(out int damage, out int ProjectileType, out int ShootType) {
			damage = 17;
			ProjectileType = ModContent.ProjectileType<RubySwotaffProjectile>();
			ShootType = ProjectileID.RubyBolt;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.RubyStaff)
				.AddIngredient(ItemID.GoldBroadsword)
				.Register();
		}
	}
	public class RubySwotaffPlayer : ModPlayer {
		public override void UpdateEquips() {
			if (Player.IsHeldingModItem<RubySwotaff>()) {
				Player.ModPlayerStats().AddStatsToPlayer(PlayerStats.MagicDMG, Base: .5f * Player.ownedProjectileCounts[ModContent.ProjectileType<RubyGemProjectileSwotaff>()]);
			}
		}
	}
	class RubySwotaffProjectile : SwotaffProjectile {
		public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<RubySwotaff>();
		public override void SwotaffCustomSetDefault(out float AltAttackAmountProjectile, out int AltAttackProjectileType, out int NormalBoltProjectile, out int DustType, out int ManaCost) {
			AltAttackAmountProjectile = 6;
			AltAttackProjectileType = ModContent.ProjectileType<RubySwotaffGemProjectile>();
			NormalBoltProjectile = ProjectileID.RubyBolt;
			DustType = DustID.GemRuby;
			ManaCost = 100;
		}
	}
	class RubySwotaffGemProjectile : SynergyModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Ruby);
		public override void SetDefaults() {
			Projectile.width = 14;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 200;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Magic;
		}
		Vector2 firstframePos = Vector2.Zero;
		float offset = 1;
		public override void SynergyPreAI(Player player, PlayerSynergyItemHandle modplayer, out bool runAI) {
			if (Projectile.timeLeft == 200) {
				firstframePos = player.GetModPlayer<BossRushUtilsPlayer>().MouseLastPositionBeforeAnimation - player.Center;
			}
			Vector2 positionToGo = player.Center + firstframePos.SafeNormalize(Vector2.One).RotatedBy(MathHelper.ToRadians(60 * Projectile.ai[2] + 360 * offset)) * Projectile.timeLeft;
			Projectile.velocity = (positionToGo - Projectile.Center).SafeNormalize(Vector2.Zero) * (positionToGo - Projectile.Center).Length() * .25f;
			base.SynergyPreAI(player, modplayer, out runAI);
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			int reversecountdown = 200 - Projectile.timeLeft;
			offset = MathHelper.Lerp(1, -10, BossRushUtils.InExpo(reversecountdown / 200f));
			if (Main.rand.NextBool(5)) {
				int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemRuby);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = Main.rand.NextFloat(1f);
			}
			if (!player.active || player.dead) {
				Projectile.Kill();
				return;
			}
		}
		public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
			if (timeLeft <= 0) {
				for (int i = 0; i < 6; i++) {
					Vector2 vec = Vector2.One.Vector2DistributeEvenly(6, 360, i).RotatedBy(MathHelper.ToRadians(Projectile.ai[2] * 10)) * 3f;
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vec, ModContent.ProjectileType<RubyGemProjectileSwotaff>(), Projectile.damage, 0, Projectile.owner);
				}
			}
			for (int i = 0; i < 10; i++) {
				int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemRuby);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = Main.rand.NextFloat(1.25f, 1.75f);
			}
		}
	}
	class RubyGemProjectileSwotaff : SynergyModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Ruby);
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 14;
			Projectile.hide = true;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 900;
			Projectile.penetrate = -1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 6;
			Projectile.DamageType = DamageClass.Magic;
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			for (int i = 0; i < 3; i++) {
				int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemRuby);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(3f, 3f);
				Main.dust[dust].scale = Main.rand.NextFloat(1.25f, 1.75f);
				Main.dust[dust].Dust_GetDust().FollowEntity = true;
				Main.dust[dust].Dust_BelongTo(Projectile);
			}
			Projectile.Center += player.velocity;
			if (Projectile.timeLeft > 150) {
				Projectile.velocity -= Projectile.velocity * .01f;
			}
		}
	}
}
