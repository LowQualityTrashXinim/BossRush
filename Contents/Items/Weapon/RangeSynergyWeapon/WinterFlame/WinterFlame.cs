using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using static tModPorter.ProgressUpdate;
using Humanizer;
using BossRush.Common.Graphics.Structs.TrailStructs;
using Terraria.Audio;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.WinterFlame;
public class WinterFlame : SynergyModItem {

	public override void SetDefaults() {

		Item.CloneDefaults(ItemID.Flamethrower);
		Item.useAnimation = 60;
		Item.useTime = 10;
		Item.width = 120;
		Item.height = 30;
		Item.shoot = ModContent.ProjectileType<WinterFlameProjectile>();
		Item.shootSpeed = 15;
		Item.damage = 55;
		Item.ArmorPenetration = 10;

	}

	public override bool AltFunctionUse(Player player) => true;

	public override Vector2? HoldoutOffset() {
		return new Vector2(-5, 0);
	}
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		SoundEngine.PlaySound(Item.UseSound, position);
		if (player.altFunctionUse == 2) {
			type = ModContent.ProjectileType<WinterFlamesProjV2>();
			velocity *= 1.1f;
			position += velocity.SafeNormalize(Vector2.UnitY) * 10;

			return;

		}
		position += velocity.SafeNormalize(Vector2.UnitY) * 40;

		Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(Item, Item.ammo), position, velocity, type, damage, knockback, player.whoAmI, 1);

	}

	public override void AddRecipes() {
		CreateRecipe()
		.AddIngredient(ItemID.ElfMelter)
		.AddIngredient(ItemID.Flamethrower)
		.Register();
	}

}

public class WinterFlameProjectile : SynergyModProjectile {

	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.Flames);
	bool isFrost;
	bool flipped;
	Vector2 startingVelocity;
	private readonly static float maxCounter = 35f;
	public int coutner {
		get => (int)Projectile.ai[1];
		set => Projectile.ai[1] = value;
	}
	Point flameTexutreSize = new Point(98, 686);



	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 30;
		ProjectileID.Sets.TrailingMode[Type] = 3;
	}
	public override void SetDefaults() {
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 300;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 120;
		Projectile.penetrate = 3;
		Projectile.localNPCHitCooldown = 15;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void OnSpawn(IEntitySource source) {
		Projectile.FillProjectileOldPosAndRot();
		startingVelocity = Projectile.velocity;
		flipped = Main.player[Projectile.owner].direction == 1 ? true : false;
		if (Projectile.ai[0] == 0)
			isFrost = false;
		else
			isFrost = true;
	}


	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {

		if (isFrost)
			npc.AddBuff(BuffID.OnFire3, 60);
		else
			npc.AddBuff(BuffID.Frostburn2, 60);

	}



	public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {

		Dust dust;

		for (int i = 0; i < Projectile.oldPos.Length; i++)
			for (int j = 0; j < 5; j++)
				if (isFrost) {
					dust = Dust.NewDustDirect(Projectile.oldPos[i], 16, 16, DustID.InfernoFork, startingVelocity.X, startingVelocity.Y);
					dust.noGravity = true;

				}
				else {

					dust = Dust.NewDustDirect(Projectile.oldPos[i], 16, 16, DustID.FrostHydra, startingVelocity.X, startingVelocity.Y);
					dust.noGravity = true;

				}

	}

	public override bool PreDraw(ref Color lightColor) {

		//Asset<Texture2D> flameTexture = TextureAssets.Projectile[Type];

		//Main.instance.LoadProjectile(Projectile.type);

		//float scale = MathHelper.Clamp(MathHelper.Lerp(0f, 1f, coutner / maxCounter), 0f, 1f);
		//Color flameColor = Color.Lerp(isFrost ? Color.OrangeRed : Color.DeepSkyBlue, Color.Lerp(isFrost ? Color.Orange : Color.CornflowerBlue, Color.Gray, (coutner - 60f) / maxCounter), coutner / maxCounter);


		//Main.EntitySpriteDraw(flameTexture.Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 1 * 98, 98, 98), flameColor, Projectile.rotation, new Vector2(98) / 2f, scale, SpriteEffects.None);



		if (!isFrost)
			default(FlameThrowerFrost).Draw(Projectile.oldPos, Projectile.oldRot, Projectile.Size * 0.5f, coutner);
		else
			default(FlameThrowerFire).Draw(Projectile.oldPos, Projectile.oldRot, Projectile.Size * 0.5f, coutner);

		return false;
	}

	public override bool OnTileCollide(Vector2 oldVelocity) {
		startingVelocity = Vector2.Zero;
		Projectile.velocity = Vector2.Zero;
		return base.OnTileCollide(oldVelocity);
	}

	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {

		coutner++;
		float sinOffset = -2f;
		Dust dust;

		for (int j = 0; j < 2; j++)
			if (isFrost) {
				dust = Dust.NewDustDirect(Projectile.Center, 16, 16, DustID.InfernoFork, -Projectile.velocity.X, -Projectile.velocity.Y);
				dust.noGravity = true;

			}
			else {

				dust = Dust.NewDustDirect(Projectile.Center, 16, 16, DustID.FrostHydra, -Projectile.velocity.X, -Projectile.velocity.Y);
				dust.noGravity = true;

			}

		if (flipped)
			Projectile.velocity = startingVelocity.RotatedBy(isFrost ? MathF.Sin((coutner + sinOffset) * 0.25f) / 2 * 1f : -MathF.Sin((coutner + sinOffset) * .25f) / 2 * 1f);
		else
			Projectile.velocity = startingVelocity.RotatedBy(isFrost ? -MathF.Sin((coutner + sinOffset) * 0.25f) / 2 * 1f : MathF.Sin((coutner + sinOffset) * .25f) / 2 * 1f);

		Projectile.rotation = Projectile.velocity.ToRotation();

	}

}

public class WinterFlamesProjV2 : SynergyModProjectile {

	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.Flames);

	private readonly static int maxCounter = 35;
	public int coutner {
		get => (int)Projectile.ai[1];
		set => Projectile.ai[1] = value;
	}


	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 30;
		ProjectileID.Sets.TrailingMode[Type] = 3;
	}
	public override void SetDefaults() {
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.tileCollide = true;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 35;
		Projectile.penetrate = 3;
		Projectile.localNPCHitCooldown = 15;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.extraUpdates = 1;
		Projectile.DamageType = DamageClass.Ranged;

	}

	public override void OnSpawn(IEntitySource source) {
		Projectile.FillProjectileOldPosAndRot();
	}

	public override bool PreDraw(ref Color lightColor) {

		default(FlameThrowerFire).Draw(Projectile.oldPos, Projectile.oldRot, Projectile.Size / 2f, coutner, 120);
		default(FlameThrowerFrost).Draw(Projectile.oldPos, Projectile.oldRot, Projectile.Size / 2f, coutner, 120);

		return false;
	}

	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		coutner++;
	}

	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
		Projectile.NewProjectile(Projectile.GetSource_OnHit(npc), npc.Center, Vector2.Zero, ProjectileID.Flames, Projectile.damage / 3, 0f, Projectile.owner);
		Projectile.NewProjectile(Projectile.GetSource_OnHit(npc), npc.Center, Vector2.Zero, ProjectileID.Flames, Projectile.damage / 3, 0f, Projectile.owner, 1);

	}

	public override bool OnTileCollide(Vector2 oldVelocity) {
		return true;
	}

	public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
		for (int i = 0; i < Projectile.oldPos.Length; i++)
			for (int j = 0; j < 5; j++) {
				var dust = Dust.NewDustDirect(Projectile.oldPos[i], 64 * j / 5, 64 * j / 5, DustID.WhiteTorch, Projectile.velocity.X, Projectile.velocity.Y, 0, Color.Pink);
				dust.noGravity = true;

			}


	}
}

