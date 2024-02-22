using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.HorusEye;
/// <summary>
///  Uwaaa blue archive weapon but not really
/// </summary>
internal class HorusEye : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(45, 120, 60, 7f, 28, 28, ItemUseStyleID.Shoot, ProjectileID.Bullet, 6f, false, AmmoID.Bullet);
		Item.crit = 12;
		Item.rare = ItemRarityID.Orange;
		Item.value = Item.buyPrice(gold: 50);
		Item.scale = 0.7f;
		Item.UseSound = SoundID.Item38 with {
			Pitch = -.7f,
			PitchVariance = .2f
		};
		Item.scale = .7f;
	}
	public override Vector2? HoldoutOffset() {
		return new Vector2(-33, 5f);
	}
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		position = position.PositionOFFSET(velocity, 70);
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		for (int i = 0; i < 30; i++) {
			int dust = Dust.NewDust(position, 0, 0, DustID.Firework_Pink);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(1.5f, 10f).RotatedBy(velocity.ToRotation());
			Main.dust[dust].scale = Main.rand.NextFloat(.9f, 1.5f);
		}
		for (int i = 0; i < 30; i++) {
			int dust = Dust.NewDust(position, 0, 0, DustID.Firework_Pink);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Unit(-MathHelper.PiOver4 * .5f, MathHelper.PiOver4).RotatedBy(velocity.ToRotation()) * Main.rand.NextFloat(7f, 19f);
			Main.dust[dust].scale = Main.rand.NextFloat(.9f, 1.5f);
		}
		player.velocity += -velocity * .35f;
		CanShootItem = false;
		for (int i = 0; i < 10; i++) {
			Projectile.NewProjectile(source, position, Main.rand.NextVector2Unit(-MathHelper.PiOver4 * .5f, MathHelper.PiOver4).RotatedBy(velocity.ToRotation()) * Main.rand.NextFloat(7f, 21f), type, damage, knockback, player.whoAmI);
		}
		Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<HorusEye_Projectile>(), (int)(damage * 1.5f), knockback, player.whoAmI);
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Shotgun)
			.AddIngredient(ItemID.BouncingShield)
			.AddIngredient(ItemID.PrincessWeapon)
			.Register();
	}
}
class HorusEye_Projectile : SynergyModProjectile {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 20;
		Projectile.penetrate = 1;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.hide = true;
		Projectile.extraUpdates = 3;
		Projectile.timeLeft = BossRushUtils.ToSecond(10);
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		base.SynergyAI(player, modplayer);
		for (int i = 0; i < 3; i++) {
			int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.GemDiamond);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Vector2.Zero;
			Main.dust[dust].color = new Color(255, 0, 100);
		}
	}
	public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ProjectileID.PrincessWeapon, Projectile.damage, Projectile.knockBack, Projectile.owner);
	}
}
