using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Annihiliation;
internal class Annihiliation : SynergyModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("Synergy");
	public override void SetDefaults() {
		Item.BossRushDefaultRange(32, 32, 33, 3f, 2, 6, ItemUseStyleID.Shoot, ModContent.ProjectileType<AnnihiliationBullet>(), 20f, true, AmmoID.Bullet);

	}
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		velocity = velocity.Vector2RotateByRandom(6) * .1f;
		type = Item.shoot;
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		base.SynergyShoot(player, modplayer, source, position, velocity, type, damage, knockback, out CanShootItem);
	}
}
public class AnnihiliationBullet : SynergyModProjectile {
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 100;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.timeLeft = BossRushUtils.ToSecond(30);
		Projectile.extraUpdates = 10;
		Projectile.penetrate = 12;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 20;
	}
	public override bool? CanDamage() {
		return Projectile.penetrate != 1;
	}
	public override Color? GetAlpha(Color lightColor) {
		Color color = Color.White;
		color.A = 0;
		//money symbol
		return color * Projectile.Opacity;
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		if (Projectile.penetrate <= 10) {
			int timeleft = 300;
			if (Projectile.timeLeft > timeleft) {
				Projectile.timeLeft = timeleft;
			}
			float progress = Projectile.timeLeft / (float)timeleft;
			//Projectile.alpha = (int)(255 * progress);
			Projectile.Opacity = BossRushUtils.InOutExpo(progress, 15);
			if (Projectile.ai[0] < 0) {
				return;
			}
			Projectile.velocity *= .995f;
		}
	}
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
		float randomrotation = Main.rand.NextFloat(90);
		Vector2 randomPosOffset = Main.rand.NextVector2Circular(20f, 20f);
		for (int i = 0; i < 4; i++) {
			Vector2 Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i + randomrotation)) * Main.rand.NextFloat(2, 3);
			for (int l = 0; l < 4; l++) {
				float multiplier = Main.rand.NextFloat();
				float scale = MathHelper.Lerp(1.1f, .1f, multiplier);
				int dust = Dust.NewDust(Projectile.Center + randomPosOffset, 0, 0, DustID.GemAmethyst, 0, 0, 0, Main.rand.Next(new Color[] { Color.White, Color.Purple }), scale);
				Main.dust[dust].velocity = Toward * multiplier;
				Main.dust[dust].noGravity = true;
			}
		}
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.velocity = Vector2.Zero;
		Projectile.ai[0] = -1;
		return false;
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
		for (int k = 0; k < Projectile.oldPos.Length; k++) {
			Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			Color color = Color.Magenta * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
			color.A = 0;
			float scaling = Math.Clamp(k * .01f, 0, 10f);
			Main.EntitySpriteDraw(texture, drawPos, null, color * Projectile.Opacity, Projectile.oldRot[k], origin, Projectile.scale - scaling, SpriteEffects.None, 0);

			Color color2 = Color.White * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
			color2.A = 0;
			Main.EntitySpriteDraw(texture, drawPos, null, color2 * Projectile.Opacity, Projectile.oldRot[k], origin, (Projectile.scale - scaling) * .5f, SpriteEffects.None, 0);
		}
		return true;
	}
}
