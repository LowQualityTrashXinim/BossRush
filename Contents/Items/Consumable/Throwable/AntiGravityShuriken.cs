using BossRush.Contents.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Consumable.Throwable;
internal class AntiGravityShuriken : ModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(24, 24, 96, 1f, 15, 15, ItemUseStyleID.Swing, ModContent.ProjectileType<AntiGravityShurikenProjectile>(), 10, false);
		Item.consumable = true;
		Item.maxStack = 15;
		Item.noUseGraphic = true;
		Item.UseSound = SoundID.Item1;
	}
}
public class AntiGravityShurikenProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<AntiGravityShuriken>();
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 10;
		ProjectileID.Sets.TrailingMode[Type] = 2;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 24;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 3600;
		Projectile.tileCollide = true;
		Projectile.extraUpdates = 2;
		Projectile.usesIDStaticNPCImmunity = true;
		Projectile.idStaticNPCHitCooldown = 10;
	}
	public override void AI() {
		Projectile.rotation = MathHelper.ToRadians(Projectile.timeLeft * 5 * -Projectile.direction);
		Projectile.spriteDirection = Projectile.direction;
		Projectile.velocity *= .97f;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Projectile.damage >= 50) {
			Projectile.damage = (int)(Projectile.damage * .98f);
		}
		else {
			Projectile.damage = 50;
		}
		Player player = Main.player[Projectile.owner];
		Vector2 spawnPosition = target.Center + Main.rand.NextVector2CircularEdge(target.width, target.height);
		Vector2 velocityToward = (target.Center - spawnPosition).SafeNormalize(Vector2.Zero);
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), spawnPosition, velocityToward, ModContent.ProjectileType<SimplePiercingProjectile>(), (int)(hit.Damage * .85f), 0, player.whoAmI, target.Size.Length() / 8f);
	}
	public override bool PreDraw(ref Color lightColor) {
		if (Projectile.velocity.IsLimitReached(.01f))
			Projectile.DrawTrail(lightColor);
		return base.PreDraw(ref lightColor);
	}
}
