using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Localization;

namespace BossRush.Contents.Items {
	internal class CelestialWrath : ModItem {
		public override void SetStaticDefaults() {
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 14));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}
		public override void SetDefaults() {
			Item.BossRushDefaultToConsume(42, 42);
			Item.shoot = ModContent.ProjectileType<CelestialWrathProjectile>();
			Item.shootSpeed = 10;
			Item.noUseGraphic = true;
			Item.maxStack = 999;
		}
		public override bool? UseItem(Player player) {
			if (player.ItemAnimationJustStarted)
				BossRushUtils.CombatTextRevamp(player.Hitbox, Color.Red, Language.GetTextValue("Mods.BossRush.Items.CelestialWrath.WarningMessage"));
			return true;
		}
	}
	class CelestialWrathProjectile : ModProjectile {
		public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<CelestialWrath>();
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 14;
		}
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 42;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 1200;
			Projectile.friendly = true;
			Projectile.hostile = true;
			Projectile.damage = 0;
			Projectile.penetrate = -1;
		}
		public override void AI() {
			SelectFrame();
			if (++Projectile.ai[0] >= 20) {
				Projectile.velocity.X *= .96f;
				Projectile.velocity.Y += .25f;
			}
			int dust1 = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(20, 20), 0, 0, DustID.GemEmerald);
			Main.dust[dust1].noGravity = true;
			Main.dust[dust1].velocity = -Vector2.UnitY * Main.rand.NextFloat(1.5f);
			float timer = MathHelper.Lerp(1, 120, BossRushUtils.InExpo((1200 - Projectile.timeLeft) / 1200f));
			float countdown = 480 - timer * 4;
			for (int i = 0; i < timer; i++) {
				Vector2 vec = Main.rand.NextVector2CircularEdge(countdown, countdown);
				int dust = Dust.NewDust(Projectile.Center + vec, 0, 0, DustID.GemEmerald);
				Main.dust[dust].noGravity = true;
			}
		}
		public void SelectFrame() {
			if (++Projectile.frameCounter >= 6) {
				Projectile.frameCounter = 0;
				Projectile.frame += 1;
				if (Projectile.frame >= Main.projFrames[Projectile.type]) {
					Projectile.frame = 0;
				}
			}
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
			fallThrough = false;
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) => false;
		public override bool? CanDamage() => false;
		public override bool? CanHitNPC(NPC target) => false;
		public override bool CanHitPlayer(Player target) => false;
		public override void OnKill(int timeLeft) {
			for (int i = 0; i < 300; i++) {
				int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemEmerald);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(50, 50);
				Main.dust[dust].scale = Main.rand.NextFloat(3, 3.5f);
				Main.dust[dust].fadeIn = 4f;
			}
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 500), Vector2.Zero, ModContent.ProjectileType<CelestialWrathBeamProjectile>(), 1, 0);
		}
	}
	class CelestialWrathBeamProjectile : ModProjectile {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Projectile.width = 400;
			Projectile.height = 1000;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 1;
			Projectile.friendly = true;
			Projectile.hostile = true;
			Projectile.hide = true;
			Projectile.penetrate = -1;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.StrikeInstantKill();
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info) {
			PlayerDeathReason reason = new PlayerDeathReason();
			reason.SourceCustomReason = $"Celestial wrath has rain down on {target.name}";
			target.KillMe(reason, 999999999, 1);
		}
		public override bool? CanDamage() => true;
		public override bool? CanHitNPC(NPC target) => true;
		public override bool CanHitPlayer(Player target) => true;
		public override void OnKill(int timeLeft) {
			for (int i = 0; i < 2000; i++) {
				int dust = Dust.NewDust(Projectile.Center + new Vector2(Main.rand.NextFloat(-200, 200), Main.rand.NextFloat(-500, 500)), 0, 0, DustID.GemEmerald);
				Main.dust[dust].scale = Main.rand.NextFloat(2, 3);
				Main.dust[dust].fadeIn = 2f;
			}
		}
	}
}
