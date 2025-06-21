using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.ForceOfEarth {
	abstract class BaseFOE : SynergyModProjectile {
		public override void SetDefaults() {
			Projectile.tileCollide = false;
			Projectile.width = 16;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.light = 0.5f;
			Projectile.DamageType = DamageClass.Ranged;
		}
		int TwoPiCounter = 0;
		bool SwitchedToRapidFireMode = false;
		public bool CanShootBecauseOfAmmo = true;
		public float speed = 15;
		public virtual float OffsetBehavior => 0;
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			if (++TwoPiCounter > 360) {
				TwoPiCounter -= 360;
			}
			if (player.dead || !player.active || !player.HasBuff(ModContent.BuffType<EarthPower>())) {
				Projectile.Kill();
				return;
			}
			else {
				Projectile.timeLeft = 2;
			}
			if (++Projectile.ai[0] >= 40 && !SwitchedToRapidFireMode) {
				if (player.ItemAnimationActive && player.ItemAnimationJustStarted) {
					Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * speed;
					FireProjectile(player);
				}
			}
			else if (Projectile.ai[0] >= 20 && SwitchedToRapidFireMode && Main.mouseRight) {
				Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * speed;
				FireProjectile(player, Main.rand.Next(-10, 11));
				Projectile.velocity = Vector2.Zero;
			}
			if (Main.mouseRight && !player.mouseInterface && !player.GetModPlayer<BossRushUtilsPlayer>().CurrentHoveringOverChest) {
				Vector2 positionNeedToBeAt = Main.MouseWorld;
				if (positionNeedToBeAt.DistanceSQ(player.Center) >= 125 * 125) {
					positionNeedToBeAt = player.Center + (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 125 + player.velocity;
				}
				Vector2 distance = positionNeedToBeAt + Vector2.One.RotatedBy(MathHelper.ToRadians(TwoPiCounter * 5 + 360 / 8f * Projectile.ai[2])) * 20 - Projectile.Center;
				float length = distance.Length();
				Projectile.velocity = distance.SafeNormalize(Vector2.Zero) * length / 8f + player.velocity;
				Projectile.rotation = (Main.MouseWorld - Projectile.Center).ToRotation();
				SwitchedToRapidFireMode = true;
			}
			else {
				Behavior(player, OffsetBehavior);
			}
		}
		public void FireProjectile(Player player, float slightoffset = 0) {
			if (!CanShootBecauseOfAmmo) {
				return;
			}
			SoundEngine.PlaySound(player.HeldItem.UseSound, Projectile.Center);
			Projectile.ai[0] = 0 + slightoffset;
			int damage = Projectile.damage;
			if (Main.mouseRight) {
				damage = (int)Math.Ceiling(damage * .76f);
			}
			if (Projectile.ai[1] == ProjectileID.None) {
				Projectile.ai[1] = ProjectileID.WoodenArrowFriendly;
			}
			Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), Projectile.Center, Projectile.velocity, (int)Projectile.ai[1], damage, Projectile.knockBack, player.whoAmI);
			if (SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<ForceOfEarth>(), ItemID.IceBlade) && Main.rand.NextBool(3)) {
				Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(player.HeldItem), Projectile.Center, Projectile.velocity, ProjectileID.IceBolt, (int)(Projectile.damage * 1.25f), Projectile.knockBack, player.whoAmI);
				proj.extraUpdates = 4;
				proj.timeLeft = 210;
			}
			CanShootBecauseOfAmmo = false;
		}
		public void Behavior(Player player, float OFFSet, float position = 40) {

			Vector2 Rotate = new Vector2(1, 1).RotatedBy(MathHelper.ToRadians(OFFSet));
			Vector2 positionNeedToBe = player.Center + Rotate * position;
			if (Projectile.Center.IsCloseToPosition(positionNeedToBe, 2) && SwitchedToRapidFireMode || !SwitchedToRapidFireMode) {
				SwitchedToRapidFireMode = false;
			}
			else {
				Vector2 distance = positionNeedToBe - Projectile.Center;
				Projectile.velocity = distance.SafeNormalize(Vector2.Zero) * distance.Length() / 8f + player.velocity;
				return;
			}

			Projectile.Center = player.Center + Rotate * position;
			Vector2 aimto = Main.MouseWorld - Projectile.position;
			Vector2 safeAimto = aimto.SafeNormalize(Vector2.UnitX);
			Projectile.rotation = safeAimto.ToRotation();
			Projectile.velocity = Vector2.Zero;
		}
		public override bool? CanDamage() {
			return false;
		}
	}
}
