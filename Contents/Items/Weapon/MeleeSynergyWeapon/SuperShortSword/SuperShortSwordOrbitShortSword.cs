using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.SuperShortSword {
	internal class SuperShortSwordOrbitShortSword : SynergyModProjectile {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override void SetDefaults() {
			Projectile.height = Projectile.width = 32;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.light = 0.7f;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}
		public static Vector2[] projPos = new Vector2[]{
		new Vector2(10,0),
		new Vector2(9,.15f),
		new Vector2(8,-.25f),
		new Vector2(7,.3f),
		new Vector2(6,-.44f),
		new Vector2(5,.5f),
		new Vector2(4,.64f),
		new Vector2(3,-.69f)
		};
		public float Index { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
		Vector2 RotatePosition = Vector2.Zero;
		Vector2 FixedMousePosition;
		Vector2 FixedProjectilePos;
		Vector2 HitNPCPos = Vector2.Zero;
		int timeLeft = 9999;
		bool IsInAtk2 = false;
		public override void SynergyPreAI(Player player, PlayerSynergyItemHandle modplayer, out bool runAI) {
			if (player.dead || !player.active || !player.HasBuff(ModContent.BuffType<SuperShortSwordPower>()) || player.HeldItem.type != ModContent.ItemType<SuperShortSword>()) {
				Projectile.Kill();
			}
			RotatePosition = getPosToReturn(player, MathHelper.PiOver4 * Index, modplayer.SuperShortSword_Counter);

			if (modplayer.SuperShortSword_AttackType == 1) {
				NormalAttackHandle(player, modplayer);
				runAI = false;
				return;
			}
			if (modplayer.SuperShortSword_AttackType == 2) {
				AltAttackHandle(player, modplayer);
				runAI = false;
				return;
			}
			if (timeLeft <= 0) {
				timeLeft = 9999;
			}
			runAI = true;
		}
		private void AltAttackHandle(Player player, PlayerSynergyItemHandle modplayer) {
			if (modplayer.SuperShortSword_IsHoldingDownRightMouse) {
				Vector2 PositionThatNeedToBe = projPos[(int)Index].RotatedBy((Main.MouseWorld - player.Center).ToRotation()) * 12.5f + player.Center;
				Vector2 ToPos = PositionThatNeedToBe - Projectile.Center;
				Projectile.velocity = ToPos.SafeNormalize(Vector2.Zero) * ToPos.Length() * .25f;
				Projectile.rotation = (Main.MouseWorld - Projectile.Center).ToRotation() + MathHelper.PiOver4;
				IsInAtk2 = true;
			}
			else {
				if (timeLeft > 20) {
					Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 30;
					timeLeft = 20;
				}
				if (timeLeft == 1) {
					Projectile.velocity += Main.rand.NextVector2Circular(20, 20);
				}
				if (timeLeft != 0) {
					timeLeft = BossRushUtils.CountDown(timeLeft);
				}
				else {
					Vector2 dis = RotatePosition - Projectile.Center;
					float length = dis.Length();
					if (length <= 15) {
						Projectile.velocity = Vector2.Zero;
						Projectile.Center = RotatePosition;
						if (IsInAtk2) {
							modplayer.SuperShortSword_ProjectileInReadyPosition++;
							IsInAtk2 = false;
						}
						return;
					}
					if (length > 2) {
						length = 2;
					}
					Projectile.velocity -= Projectile.velocity * .085f;
					Projectile.velocity += dis.SafeNormalize(Vector2.Zero) * length;
					Projectile.velocity = Projectile.velocity.LimitedVelocity(40);
					Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
				}
			}
		}
		bool HasHitNPC = false;
		//Turn out this not all sync so again
		private void NormalAttackHandle(Player player, PlayerSynergyItemHandle modplayer) {
			float duration = player.itemAnimationMax;
			if (timeLeft > duration) {
				timeLeft = (int)duration;
				HasHitNPC = false;
				HitNPCPos = Vector2.Zero;
				FixedMousePosition = Main.MouseWorld;
				FixedProjectilePos = Projectile.Center;
				IsInAtk2 = true;
			}
			Vector2 PositionToMouse = FixedMousePosition - FixedProjectilePos;
			Vector2 ToMouse = PositionToMouse.SafeNormalize(Vector2.UnitX);
			float halfProgress = duration * .5f;
			float progress;
			if (timeLeft < halfProgress) {
				progress = timeLeft / halfProgress;
			}
			else {
				progress = (duration - timeLeft) / halfProgress;
			}
			float length = PositionToMouse.Length();
			if (HasHitNPC) {
				Projectile.Center = Vector2.SmoothStep(RotatePosition, HitNPCPos, progress);
			}
			else {
				Projectile.Center = RotatePosition + Vector2.SmoothStep(ToMouse, ToMouse * length, progress);
				Projectile.rotation = ToMouse.ToRotation() + MathHelper.PiOver4;
			}
			timeLeft = BossRushUtils.CountDown(timeLeft);
			if(timeLeft == 0) {
				if (IsInAtk2) {
					modplayer.SuperShortSword_ProjectileInReadyPosition++;
					IsInAtk2 = false;
				}
			}
		}
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			Projectile.damage = (int)(player.GetWeaponDamage(player.HeldItem) * 0.25f * player.GetTotalDamage(DamageClass.Melee).Additive);
			Projectile.CritChance = (int)(player.GetCritChance(DamageClass.Melee) + player.GetCritChance(DamageClass.Generic));
			Vector2 SafeDegree = Main.MouseWorld - Projectile.Center;
			if (!player.ItemAnimationActive) Projectile.rotation = SafeDegree.ToRotation() + MathHelper.PiOver4;
			Projectile.Center = RotatePosition;
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
			if (!HasHitNPC) {
				HitNPCPos = Projectile.Center;
				HasHitNPC = true;
			}
		}
		public Vector2 getPosToReturn(Player player, float offSet, int Counter, float Distance = 50) => player.Center + Vector2.One.RotatedBy(offSet + Counter * 0.05f) * Distance;
		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = ModContent.Request<Texture2D>(BossRushUtils.GetVanillaTexture<Item>(TerrariaArrayID.AllOreShortSword[(int)Index])).Value;
			Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}
