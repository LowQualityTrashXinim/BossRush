using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.ParadoxPistol {
	public class UltimatePistolMinion : ModProjectile {
		public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<UltimatePistol>();
		public override void SetDefaults() {
			Projectile.width = 39;
			Projectile.height = 21;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Generic;
			Projectile.penetrate = -1;
			Projectile.minionSlots = 1f;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 1800;
		}
		bool ISFACINGRIGHT = false;
		public override bool PreAI() {
			Player player = Main.player[Projectile.owner];

			//Idle Position and rotation
			Projectile.velocity = player.velocity;


			Vector2 DegreeToAim = Main.MouseWorld - Projectile.position;
			Vector2 SafeDegree = DegreeToAim.SafeNormalize(Vector2.UnitX);
			float SaferRotate = SafeDegree.ToRotation();

			Projectile.rotation = SaferRotate;
			//Actual AI here

			float distanceFromTarget = 1000f;
			Vector2 targetCenter = Projectile.position;
			bool foundTarget = false;

			ISFACINGRIGHT = IsfacingRight(DegreeToAim, Vector2.Zero, false);

			if (!foundTarget) {
				for (int i = 0; i < Main.maxNPCs; i++) {
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy()) {
						float between = Vector2.Distance(npc.Center, Projectile.Center);
						bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
						if ((closest && inRange || !foundTarget) && lineOfSight) {
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
						}
					}
				}
			}
			if (foundTarget) {
				Vector2 DistanceFromProjToAim = targetCenter - Projectile.position;
				Vector2 DirectionFromProjToAim = DistanceFromProjToAim.SafeNormalize(Vector2.UnitX) * 15f; ;
				float aimtoTarget = DirectionFromProjToAim.ToRotation();
				Projectile.rotation = aimtoTarget;
				ISFACINGRIGHT = IsfacingRight(Vector2.Zero, DistanceFromProjToAim, foundTarget);
				Projectile.ai[0] += 1f;
				if (Projectile.ai[0] >= 15f) {
					Projectile.ai[0] = 0;
					Projectile.netUpdate = true;

					int type = Main.rand.Next(new int[] { ProjectileID.IceSickle, ProjectileID.DeathSickle, ProjectileID.DemonScythe, ProjectileID.UnholyTridentFriendly, ProjectileID.MoonlordArrow, ProjectileID.ShadowFlameArrow, ProjectileID.BeeArrow, ProjectileID.ChlorophyteArrow, ProjectileID.Hellwing, ProjectileID.VenomArrow, ProjectileID.IchorArrow, ProjectileID.FrostburnArrow, ProjectileID.FrostArrow, ProjectileID.BoneArrow, ProjectileID.CursedArrow, ProjectileID.HolyArrow, ProjectileID.HellfireArrow, ProjectileID.JestersArrow, ProjectileID.UnholyArrow, ProjectileID.FireArrow, ProjectileID.WoodenArrowFriendly, ProjectileID.MoonlordBullet, ProjectileID.BulletHighVelocity, ProjectileID.IchorBullet, ProjectileID.PartyBullet, ProjectileID.VenomBullet, ProjectileID.ExplosiveBullet, ProjectileID.NanoBullet, ProjectileID.ChlorophyteBullet, ProjectileID.CursedBullet, ProjectileID.GoldenBullet, ProjectileID.MeteorShot, ProjectileID.CrystalBullet, ProjectileID.FruitcakeChakram, ProjectileID.BloodyMachete, ProjectileID.Bananarang, ProjectileID.PaladinsHammerFriendly, ProjectileID.PossessedHatchet, ProjectileID.LightDisc, ProjectileID.Flamarang, ProjectileID.ThornChakram, ProjectileID.IceBoomerang, ProjectileID.WoodenBoomerang, ProjectileID.EnchantedBoomerang, ProjectileID.Starfury, ProjectileID.HallowStar, ProjectileID.StarWrath, ProjectileID.FallingStar, ProjectileID.BallofFire, ProjectileID.CursedFlameFriendly, ProjectileID.BallofFrost, ProjectileID.BouncyGrenade, ProjectileID.Grenade, ProjectileID.GrenadeI, ProjectileID.Beenade, ProjectileID.StickyGrenade, ProjectileID.MolotovCocktail, ProjectileID.PartyGirlGrenade, ProjectileID.ThrowingKnife, ProjectileID.PoisonedKnife, ProjectileID.MagicDagger, ProjectileID.VampireKnife, ProjectileID.ShadowFlameKnife, ProjectileID.QuarterNote, ProjectileID.EighthNote, ProjectileID.TiedEighthNote, ProjectileID.AmethystBolt, ProjectileID.TopazBolt, ProjectileID.SapphireBolt, ProjectileID.EmeraldBolt, ProjectileID.RubyBolt, ProjectileID.DiamondBolt, ProjectileID.IceBolt, ProjectileID.AmberBolt, ProjectileID.InfernoFriendlyBolt, ProjectileID.PulseBolt, ProjectileID.BlackBolt, ProjectileID.SwordBeam, ProjectileID.FrostBoltSword, ProjectileID.TerraBeam, ProjectileID.LightBeam, ProjectileID.NightBeam, ProjectileID.EnchantedBeam, ProjectileID.InfluxWaver, ProjectileID.CrystalDart, ProjectileID.CursedDart, ProjectileID.IchorDart, ProjectileID.GiantBee, ProjectileID.Wasp, ProjectileID.Bee, ProjectileID.CopperCoin, ProjectileID.SilverCoin, ProjectileID.GoldCoin, ProjectileID.PlatinumCoin, ProjectileID.JackOLantern, ProjectileID.CandyCorn, ProjectileID.Bat, ProjectileID.RottenEgg, ProjectileID.Stake });
					Projectile.NewProjectile(null, Projectile.Center, DirectionFromProjToAim, type, 400, 1f, player.whoAmI);
				}
			}
			return true;
		}
		public bool IsfacingRight(Vector2 mouseFace, Vector2 ToTarget, bool targetFound = false) {
			if (targetFound) {
				if (ToTarget.X > 0) {
					return true;
				}
				else {
					return false;
				}
			}
			else {
				if (mouseFace.X > 0) {
					return true;
				}
				else {
					return false;
				}
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
			SpriteEffects sprite = !ISFACINGRIGHT ? SpriteEffects.FlipVertically : SpriteEffects.None;
			Main.EntitySpriteDraw(texture, drawPos, null, Main.DiscoColor, Projectile.rotation, origin, Projectile.scale, sprite, 0);
			return false;
		}
	}
}