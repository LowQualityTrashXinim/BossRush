using Terraria.ID;
using BossRush.Texture;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.SnowballCannonMK2;
//SnowballCannonMK2
//Uses snowball as ammo, shoot out snowball faster
//After every 10th shot, shoot out a giant snowball that deal 300% of weapon damage
//Ice bolt have 10% to be accompany with every shot, Ice bolt deal 125% weapon damage
internal class SnowballCannonMK2 : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(72, 26, 15, 3f, 14, 14, ItemUseStyleID.Shoot, ProjectileID.SnowBallFriendly, 12, true, AmmoID.Snowball);
		Item.UseSound = SoundID.Item11;
	}
	int counter = 0;
	public override Vector2? HoldoutOffset() {
		return new Vector2(-5, 0);
	}
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		position = position.PositionOFFSET(velocity, 70);
		if (++counter >= 10) {
			type = ModContent.ProjectileType<GiantSnowBall>();
			damage *= 3;
			counter = 0;
		}
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		CanShootItem = true;
		if (!Main.rand.NextBool(10)) {
			return;
		}
		Projectile.NewProjectile(source, position, velocity, ProjectileID.IceBolt, (int)(damage * 1.25f), knockback, player.whoAmI);
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.SnowballCannon)
			.AddIngredient(ItemID.FlareGun)
			.AddIngredient(ItemID.IceBlade)
			.Register();
	}
}
public class GiantSnowBall : SynergyModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.SnowBallFriendly);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 14;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 999;
		Projectile.penetrate = 1;
		Projectile.scale = 3;
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Snow);
		dust.noGravity = true;
		dust.position += Main.rand.NextVector2Circular(32, 32);
		if (++Projectile.ai[0] >= 25) {
			Projectile.velocity.Y += .5f;
		}
		Projectile.velocity.X *= .99f;
		Projectile.rotation += MathHelper.ToRadians(Projectile.ai[0]);
	}
	public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
		SoundEngine.PlaySound(SoundID.Item51 with { Pitch = -.25f });
		for (int i = 0; i < 35; i++) {
			var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Snow);
			dust.noGravity = true;
			dust.position += Main.rand.NextVector2Circular(32, 32);
			dust.velocity = Projectile.velocity.Vector2RotateByRandom(15).Vector2RandomSpread(2, Main.rand.NextFloat(1, 1.44f)) * .55f;
			dust.scale = Main.rand.NextFloat(.85f, 1.34f);
		}
		for (int i = 0; i < 15; i++) {
			var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Snow);
			dust.noGravity = true;
			dust.velocity = -Projectile.velocity.Vector2RotateByRandom(15).SafeNormalize(Vector2.Zero) * 5 * Main.rand.NextFloat();
			dust.scale = Main.rand.NextFloat(.85f, 1.34f);
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.ProjectileDefaultDrawInfo(out var tex, out var origin);
		var drawpos = Projectile.position - Main.screenPosition + origin * Projectile.scale;
		Main.EntitySpriteDraw(tex, drawpos, null, Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
		return false;
	}
}
