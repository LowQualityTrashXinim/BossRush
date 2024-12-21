using BossRush.Contents.Items.Weapon.SummonerSynergyWeapon.StarWhip;
using BossRush.Texture;
using BossRush.TrailStructs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace BossRush.Contents.Items.Weapon.MagicSynergyWeapon.WyvernWrath;
public class WyvernWrath : SynergyModItem {


	public override void Synergy_SetStaticDefaults() {
		Item.staff[Type] = true;
	}
	public override void SetDefaults() {
		Item.damage = 32;
		Item.ArmorPenetration = 10;
		Item.DamageType = DamageClass.Magic;
		Item.knockBack = 15;
		Item.crit = 30;
		Item.useAnimation = 9;
		Item.useTime = 3;
		Item.reuseDelay = 25;
		Item.shoot = ModContent.ProjectileType<WyvernWrathMainProjectile>();
		Item.width = Item.height = 37;
		Item.mana = 24;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.shootSpeed = 20;
		Item.noMelee = true;
		Item.UseSound = SoundID.Item125;
		Item.scale = .66f;
	}


	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {

		int playerDir = player.direction;
		position = player.Center.PositionOFFSET(velocity, -Main.rand.NextFloat(70, 90)) + Main.rand.NextVector2Circular(25, 60).RotatedBy(velocity.ToRotation());
		velocity = position.DirectionTo(Main.MouseWorld).SafeNormalize(Vector2.UnitY) * velocity.Length();


		for (int i = 0; i < 25; i++) {
			var dust = Dust.NewDustPerfect(position, DustID.WhiteTorch, Main.rand.NextVector2CircularEdge(5, 5), 0, Color.Turquoise);
			dust.noGravity = true;
		}
	}

	public override void AddRecipes() {
		CreateRecipe().AddIngredient(ItemID.SkyFracture).AddIngredient(ItemID.CrystalSerpent).Register();
	}

}
public class WyvernWrathMainProjectile : SynergyModProjectile {
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 3;
		ProjectileID.Sets.TrailCacheLength[Type] = 35;
	}

	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.SkyFracture);

	public override void SetDefaults() {

		Projectile.width = Projectile.height = 16;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.light = 0.8f;
		Projectile.tileCollide = false;
		Projectile.frame = Main.rand.Next(14);
		Projectile.extraUpdates = 1;


	}
	public override void OnSpawn(IEntitySource source) {
		Projectile.FillProjectileOldPosAndRot();
		Projectile.frame = Main.rand.Next(14);
		float randomrotation = Main.rand.NextFloat(90);
		Vector2 randomPosOffset = Main.rand.NextVector2Circular(20f, 20f);
		for (int i = 0; i < 4; i++) {
			Vector2 Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i + randomrotation)) * (3 + Main.rand.NextFloat());
			for (int l = 0; l < 8; l++) {
				float multiplier = Main.rand.NextFloat();
				float scale = MathHelper.Lerp(1.1f, .1f, multiplier);
				int dust = Dust.NewDust(Projectile.Center + randomPosOffset, 0, 0, DustID.GemEmerald, 0, 0, 0, Main.rand.Next(new Color[] { Color.White, Color.Green }), scale);
				Main.dust[dust].velocity = Toward * multiplier;
				Main.dust[dust].noGravity = true;
			}
		}
	}

	public override bool PreDraw(ref Color lightColor) {
		Vector2 randomPosOffset = Main.rand.NextVector2Circular(20f, 20f);
		int dust = Dust.NewDust(Projectile.Center + randomPosOffset, 0, 0, Main.rand.NextBool() ? DustID.GemEmerald : DustID.GemDiamond, 0, 0, 0, Color.White, Scale: Main.rand.NextFloat(.7f, 1.1f));
		Main.dust[dust].noGravity = true;
		Asset<Texture2D> texture = TextureAssets.Projectile[Type];
		Main.instance.LoadProjectile(ProjectileID.SkyFracture);

		Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, new Rectangle(Projectile.frame * 38, 0, 38, 38), Color.Turquoise, Projectile.velocity.ToRotation() + MathHelper.PiOver4, new Vector2(38) / 2f, 1f, SpriteEffects.None);
		default(WyvernTrailMain).Draw(Projectile.oldPos, Projectile.oldRot, Projectile.Size * 0.5f);

		return false;
	}
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
		Vector2 randomPosOffset = Main.rand.NextVector2Circular(20f, 20f);

		for (int l = 0; l < 20; l++) {
			float multiplier = Main.rand.NextFloat();
			float scale = MathHelper.Lerp(1.1f, .1f, multiplier) + 1f;
			float randomrotate = MathHelper.Lerp(50f, 1f, BossRushUtils.InOutSine(multiplier));
			int dust = Dust.NewDust(Projectile.Center + randomPosOffset, 0, 0, DustID.GemEmerald, 0, 0, 0, Color.White, scale);
			Main.dust[dust].velocity = -Projectile.velocity.RotatedBy(MathHelper.ToRadians(randomrotate)) * multiplier * .5f;
			Main.dust[dust].noGravity = true;
			int dust2 = Dust.NewDust(Projectile.Center + randomPosOffset, 0, 0, DustID.GemEmerald, 0, 0, 0, Color.White, scale);
			Main.dust[dust2].velocity = -Projectile.velocity.RotatedBy(MathHelper.ToRadians(-randomrotate)) * multiplier * .5f;
			Main.dust[dust2].noGravity = true;
		}

		Vector2 pos = (Projectile.Center + new Vector2(750, 0)).RotatedBy(Main.rand.NextFloat(MathHelper.ToRadians(360)), Projectile.Center);
		Projectile.NewProjectile(Projectile.GetSource_OnHit(npc), pos, pos.DirectionTo(Projectile.Center).SafeNormalize(Vector2.UnitY) * 25, ModContent.ProjectileType<WyvernWrathMiniProjectile>(), (int)(Projectile.damage * 0.8f), 0f, Projectile.owner);
	}
}

public class WyvernWrathMiniProjectile : SynergyModProjectile {
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 3;
		ProjectileID.Sets.TrailCacheLength[Type] = 35;
	}

	public override void SetDefaults() {


		Projectile.width = Projectile.height = 8;
		Projectile.friendly = true;
		Projectile.penetrate = 1;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.tileCollide = false;
		Projectile.extraUpdates = 3;
		Projectile.penetrate = -1;
		Projectile.localNPCHitCooldown = 15;
		Projectile.usesLocalNPCImmunity = true;


	}

	public override void OnSpawn(IEntitySource source) {
		Projectile.FillProjectileOldPosAndRot();

		for (int i = 0; i < 25; i++) {
			var dust = Dust.NewDustPerfect(Projectile.Center, DustID.WhiteTorch, Main.rand.NextVector2CircularEdge(5, 5), 0, Color.Turquoise);
			dust.noGravity = true;
		}
	}

	public override string Texture => BossRushTexture.MissingTexture_Default;


	public override bool PreDraw(ref Color lightColor) {

		default(WyvernTrailMini).Draw(Projectile.oldPos, Projectile.oldRot, Projectile.Size * 0.5f);

		return false;
	}

}
