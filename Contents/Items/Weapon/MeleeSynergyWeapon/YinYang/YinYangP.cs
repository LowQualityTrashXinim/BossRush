using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.YinYang {
	public class YinYangP : ModProjectile {
		public override void SetStaticDefaults() {
			ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 25f;
		}
		public override void SetDefaults() {
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 400f;
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 15;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.aiStyle = 99;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.width = 24;
			Projectile.height = 24;
		}
		int charge = 0;
		int spawnCounter = 5;
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			charge++;
			Player player = Main.player[Projectile.owner];
			if (charge == 17 && player.yoyoGlove == false) {
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<YinYangShockWave>(), 0, 0, Projectile.owner);
				Projectile.damage = (int)(Projectile.damage * 1.75f);
				ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 24;
				ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 600f;
			}
			if (charge >= 17 || player.yoyoGlove == true) {
				//Vector2 RotatePos = Main.rand.NextVector2CircularEdge(75f, 75f) * 5 + Projectile.position;
				//Vector2 Aimto = (target.Center - RotatePos).SafeNormalize(Vector2.UnitX) * 3;
				//Projectile.NewProjectile(Projectile.GetSource_FromAI(), RotatePos, Aimto, ModContent.ProjectileType<YinLight>(), (int)(Projectile.damage * 0.75f), 2f, Projectile.owner, 0);
				//RotatePos = Main.rand.NextVector2CircularEdge(75f, 75f) * 5 + Projectile.position;
				//Aimto = (target.Center - RotatePos).SafeNormalize(Vector2.UnitX) * 3;
				//Projectile.NewProjectile(Projectile.GetSource_FromAI(), RotatePos, Aimto, ModContent.ProjectileType<YangDark>(), (int)(Projectile.damage * 0.75f), 2f, Projectile.owner, 0);
				if (spawnCounter >= 5) {
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<YinLight>(), Projectile.damage, 0, Projectile.owner, 1);
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<YangDark>(), Projectile.damage, 0, Projectile.owner, 1);
					spawnCounter = 0;
				}
				spawnCounter++;
			}
		}
	}

	public abstract class BaseBoltProjectile : ModProjectile {
		public override void SetDefaults() {
			Projectile.DamageType = DamageClass.Melee;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.light = 1f;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 999;
			Projectile.friendly = true;
			Projectile.extraUpdates = 6;
			Projectile.tileCollide = false;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}
		public virtual int Offset() => 0;
		int Counter = 0;
		int frame = 0;
		int distance = 1;
		Vector2 projCenter;
		public override void AI() {
			if (Projectile.ai[0] == 0) {
				Projectile.penetrate = 1;
				return;
			}
			if (frame == 0) {
				projCenter = Projectile.Center;
			}
			distance += frame % 6 == 0 ? 1 : 0;
			frame++;
			Counter++;
			Projectile.alpha = (int)MathHelper.Lerp(0, 255, (999 - Projectile.timeLeft) / 999f);
			Projectile.Center = getPosToReturn(Offset(), Counter, projCenter, distance);
		}
		private static Vector2 getPosToReturn(float offSet, int Counter, Vector2 pos, float Distance = 50) {
			Vector2 Rotate = new Vector2(1, 1).RotatedBy(MathHelper.ToRadians(offSet));
			return pos + Rotate.RotatedBy(MathHelper.ToRadians(Counter)) * Distance;
		}
		public override bool PreDraw(ref Color lightColor) {
			Projectile.DrawTrail(lightColor, .02f);
			return true;
		}
	}
	public class YinLight : BaseBoltProjectile {
		public override int Offset() => 0;
	}

	public class YangDark : BaseBoltProjectile {
		public override int Offset() => 180;
	}
}
