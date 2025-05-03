using Terraria;
using Terraria.ID;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Common.Graphics.TrailStructs;
using BossRush.Common.RoguelikeChange.ItemOverhaul;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.MythrilBeamSword;
public class MythrilBeamSword : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultMeleeShootCustomProjectile(72, 72, 88, 6f, 28, 28, ItemUseStyleID.Swing, ModContent.ProjectileType<MythrilBeam>(), 15, true);
		Item.GetGlobalItem<MeleeWeaponOverhaul>().SwingType = BossRushUseStyle.Swipe;
		Item.UseSound = SoundID.Item1;
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		MeleeOverhaulPlayer meleeplayer = player.GetModPlayer<MeleeOverhaulPlayer>();
		for (int i = 0; i < 6; i++) {
			Vector2 rotate = velocity.Vector2DistributeEvenly(5, 120 * player.direction, meleeplayer.ComboNumber % 2 == 0 ? i : 5 - i) * .5f;
			Projectile.NewProjectile(source, position.PositionOFFSET(rotate, 62), rotate, type, damage, knockback, player.whoAmI, ai1: i);
		}
		Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 62), velocity, type, damage, knockback, player.whoAmI);
		CanShootItem = false;
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.MythrilSword)
			.AddIngredient(ItemID.BeamSword)
			.Register();
	}

}
public class MythrilBeam : SynergyModProjectile {
	bool retargeting = false;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 3;
		ProjectileID.Sets.TrailCacheLength[Type] = 35;
	}
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.SwordBeam);
	public override void SetDefaults() {
		Projectile.CloneDefaults(ProjectileID.SwordBeam);
		Projectile.aiStyle = -1;

	}
	public override void OnSpawn(IEntitySource source) {
		retargeting = false;
		Projectile.FillProjectileOldPosAndRot();
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(ProjectileID.SwordBeam);
		Asset<Texture2D> texture = TextureAssets.Projectile[ProjectileID.SwordBeam];
		Vector2 origin = texture.Size() * .5f;
		if (!retargeting) {
			Main.EntitySpriteDraw(texture.Value, Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * 10f - Main.screenPosition + origin * .5f, null, new(255, 255, 0, 50), Projectile.rotation + MathHelper.PiOver4, origin, 1f, SpriteEffects.None);
			default(BeamTrail).Draw(Projectile, new(255, 255, 0, 50), texture.Size() / 2f - Main.screenPosition);
		}
		else {
			//R:199,G:21,B:133
			Main.EntitySpriteDraw(texture.Value, Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * 10f - Main.screenPosition + origin * .5f, null, new(199, 21, 133, 50), Projectile.rotation + MathHelper.PiOver4, origin, 1f, SpriteEffects.None);
			default(BeamTrail).Draw(Projectile, new(199, 21, 133, 50), -Main.screenPosition + texture.Size() / 2f);
		}

		return false;
	}
	int timer = 0;
	Vector2 localOriginalvelocity;
	public override void SynergyPreAI(Player player, PlayerSynergyItemHandle modplayer, out bool runAI) {
		runAI = false;
		Projectile.rotation = Projectile.velocity.ToRotation();
		if (timer == 0) {
			localOriginalvelocity = Projectile.velocity.SafeNormalize(Vector2.UnitX);
		}
		if (timer <= 20 + Projectile.ai[1] * 2) {
			Projectile.timeLeft = 200;
			Projectile.velocity -= Projectile.velocity * .1f;
			timer++;
		}
		else {
			runAI = true;
			if (!Projectile.velocity.IsLimitReached(20) && Projectile.ai[0] < 25) Projectile.velocity += localOriginalvelocity;
		}
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		Projectile.ai[0]++;
		Projectile.rotation = Projectile.velocity.ToRotation();
		int range = 1200;
		Vector2 targetPos = Projectile.Center.LookForHostileNPCPositionClosest(range);

		if (Projectile.ai[0] == 25) {
			retargeting = true;
			Projectile.damage = (int)(Projectile.damage * 1.25f);
			for (int i = 0; i < 35; i++) {
				var dust = Dust.NewDustPerfect(Projectile.position, DustID.OrangeTorch, Main.rand.NextVector2CircularEdge(10, 10));
				dust.noGravity = true;

			}
			Projectile.velocity =
			targetPos != Vector2.Zero
			? Projectile.Center.DirectionTo(targetPos) * Projectile.velocity.Length()
			: Projectile.velocity;
		}
	}
}
