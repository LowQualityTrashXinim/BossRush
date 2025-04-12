using BossRush.Common;
using BossRush.Common.Graphics;
using BossRush.Common.Graphics.Primitives;
using BossRush.Common.Graphics.RenderTargets;
using BossRush.Contents.Items.Weapon.SummonerSynergyWeapon.StarWhip;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Common.Graphics.AnimationSystems;
using BossRush.Common.Graphics.Structs.TrailStructs;
using BossRush.Common.Graphics.Structs.QuadStructs;
using Terraria.Audio;

namespace BossRush.Contents.Items.Weapon.ArcaneRange.LaserSniper;
internal class LaserSniper : SynergyModItem {
	public override void Synergy_SetStaticDefaults() {
		SynergyBonus_System.Add_SynergyBonus(Type, ItemID.LaserRifle);
	}
	ItemProjectile itemProj;
	public override void SetDefaults() {
		Item.BossRushDefaultRange(86, 26, 250, 20, 65, 65, ItemUseStyleID.Shoot, ModContent.ProjectileType<LaserSniperProjectile>(), 1, false, AmmoID.Bullet);
		Item.crit = 20;
		Item.UseSound = SoundID.Item91 with {
			Pitch = .9f
		};
		Item.rare = ItemRarityID.Orange;
		Item.value = Item.buyPrice(gold: 50);
		Item.scale = 0.9f;
		Item.mana = 20;
		Item.DamageType = ModContent.GetInstance<RangeMageHybridDamageClass>();
		Item.noUseGraphic = true;

	}

	#region Animation

	TweenHandler<float> recoilHandler = new TweenHandler<float>();
	Tween<Vector2> recoilScale = new Tween<Vector2>(Vector2.Lerp);
	Tween<Vector2> recoilPos = new Tween<Vector2>(Vector2.Lerp);
	public override bool? UseItem(Player player) {

		if (Main.myPlayer == player.whoAmI && player.altFunctionUse != 2) {

			var dir = Main.MouseWorld.X > player.Center.X ? 1 : -1;


			if (itemProj != null) {
				if (itemProj.Type == ModContent.ProjectileType<ItemProjectile>()) {
					itemProj.Projectile.Kill();
				}
			}

			var rot = player.Center.DirectionTo(Main.MouseWorld);

			itemProj = ItemProjectile.SpawnItemProjectile(player, player.Center + player.Center.DirectionTo(Main.MouseWorld) * 15, rot.ToRotation(), Item.useAnimation, TextureAssets.Item[Type].Value, Rectangle.Empty, TextureAssets.Item[Type].Value.Size() / 2f, new Vector2(1f, 1f), dir == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, BoltActionAnimation);

			recoilScale.SetProperties(new Vector2(0.8f, 2f), new Vector2(1f, 1f), TweenEaseType.OutExpo, Item.useAnimation / 3);
			recoilPos.SetProperties(new Vector2(0, 0), new Vector2(0, 0).PositionOFFSET(rot, 15), TweenEaseType.OutExpo, player.itemAnimationMax / 3);
			recoilPos.Start();
			recoilScale.Start();

			recoilHandler.tweens.Clear();
			recoilHandler.tweens.Add(new Tween<float>(MathHelper.Lerp).SetProperties(rot.ToRotation() - MathHelper.PiOver2 / 2 * dir, rot.ToRotation(), TweenEaseType.None, player.itemAnimationMax / 2));
			recoilHandler.PlayTweens();
		}

		return true;
	}
	public bool BoltActionAnimation(Player player, Vector2 pos, Vector2 offset) {

		recoilScale.Update();
		recoilPos.Update();
		recoilHandler.Update();


		if (player.itemAnimation == player.itemAnimationMax)
			itemProj.PositionOffset = player.Center.DirectionTo(Main.MouseWorld) * 15;

		itemProj.spriteScale = recoilScale.currentProgress;
		itemProj.Projectile.rotation = recoilHandler.currentTween.currentProgress;
		player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Quarter, player.direction == 1 ? itemProj.Projectile.rotation : itemProj.Projectile.rotation - MathHelper.Pi);
		player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, player.direction == 1 ? itemProj.Projectile.rotation - MathHelper.PiOver2 : itemProj.Projectile.rotation - MathHelper.PiOver2);

		return true;
	}

	#endregion

	public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
		if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.LaserRifle)) {
			tooltips.Add(new(Mod, "LaserSniper_LaserRifle", $"[i:{ItemID.LaserRifle}] : Activate rifle mode"));
		}
	}
	public override bool? CanAutoReuseItem(Player player) {
		bool result = SynergyBonus_System.Check_SynergyBonus(Type, ItemID.LaserRifle);
		if (result) {
			return true;
		}
		return base.CanAutoReuseItem(player);
	}
	public override float UseSpeedMultiplier(Player player) {
		float speed = base.UseSpeedMultiplier(player);
		if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.LaserRifle)) {
			speed += 2.5f;
		}
		return speed;
	}
	public override void OnMissingMana(Player player, int neededMana) {
		player.GetModPlayer<LaserSniperPlayer>().ManaMissing = true;
		player.statMana += neededMana;
	}
	public override bool AltFunctionUse(Player player) {
		return true;
	}
	public override Vector2? HoldoutOffset() {
		return new Vector2(-20, 0);
	}
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (player.GetModPlayer<LaserSniperPlayer>().ManaMissing) {
			damage = (int)(damage * .8f);
		}
		else {
			damage *= 2;
		}
		type = ModContent.ProjectileType<LaserSniperProjectile>();
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		if (player.altFunctionUse == 2) {
			Projectile.NewProjectileDirect(source, position, velocity * 2, ModContent.ProjectileType<PlasmaGrenade>(), damage, knockback, player.whoAmI);
		}
		else {
			Projectile proj = Projectile.NewProjectileDirect(source, position.PositionOFFSET(velocity, 90), velocity, type, damage, knockback, player.whoAmI);
			if (!player.GetModPlayer<LaserSniperPlayer>().ManaMissing) {
				proj.penetrate = 3;
				proj.maxPenetrate = 3;
			}
		}
		CanShootItem = false;
	}
}
public class LaserSniperPlayer : ModPlayer {
	public bool ManaMissing = false;
	public override void ResetEffects() {
		ManaMissing = false;
	}
}

public class LaserSniperProjectile : ModProjectile {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 3;
		ProjectileID.Sets.TrailCacheLength[Type] = 60;
	}

	public override void SetDefaults() {
		Projectile.width = Projectile.height = 16;
		Projectile.hostile = false;
		Projectile.friendly = true;
		Projectile.aiStyle = -1;
		Projectile.penetrate = 1;
		Projectile.DamageType = ModContent.GetInstance<RangeMageHybridDamageClass>();
		Projectile.extraUpdates = 15;
		Projectile.timeLeft = 1200;
	}

	public override void OnSpawn(IEntitySource source) {
		Projectile.FillProjectileOldPosAndRot();
	}
	public override bool PreDraw(ref Color lightColor) {
		var s = new TrailShaderSettings();
		s.offset = Projectile.Size * .5f;
		s.image1 = TextureAssets.Extra[193];
		s.oldPos = Projectile.oldPos;
		s.oldRot = Projectile.oldRot;
		s.Color = Color.Aqua;
		s.shaderType = "FlameEffect";
		s.image2 = null;
		s.image3 = null;
		default(GenericTrail).Draw(s, (a) => MathHelper.Lerp(2f, 6f, Utils.GetLerpValue(0f, 0.2f, a, clamped: true)) * Utils.GetLerpValue(0f, 0.07f, a, clamped: true), (a) => Color.Aqua);
		return false;
	}
}

public class PlasmaGrenade : SynergyModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(657);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.friendly = true;
		Projectile.timeLeft = 300;
		Projectile.penetrate = 1;
		Projectile.tileCollide = false;
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		Projectile.velocity *= .98f;
		Projectile.rotation += Projectile.velocity.ToRotation() * .1f;

		foreach (Projectile proj in Main.ActiveProjectiles) {
			if (proj.type == ModContent.ProjectileType<LaserSniperProjectile>()
				&& Projectile.Center.IsCloseToPosition(proj.Center, 32)) {
				Projectile.damage *= 3;
				Projectile.Kill();
			}
		}

	}
	public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PlasmaExplosion>(), Projectile.damage / 3, 0, Projectile.owner);
		SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
	}

	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadItem(ItemID.Grenade);
		Main.EntitySpriteDraw(TextureAssets.Item[ItemID.Grenade].Value, Projectile.Center - Main.screenPosition, null, new Color(255, 255, 255, 0), Projectile.rotation, TextureAssets.Item[ItemID.Grenade].Size() * .5f, 1.25f, SpriteEffects.None);
		Main.EntitySpriteDraw(TextureAssets.Item[ItemID.Grenade].Value, Projectile.Center - Main.screenPosition, null, Color.Aqua, Projectile.rotation, TextureAssets.Item[ItemID.Grenade].Size() * .5f, 1f, SpriteEffects.None);
		return false;
	}
}

public class PlasmaExplosion : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.Flames);
	public override void SetStaticDefaults() {
		Main.projFrames[Type] = 6;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 98;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.DamageType = ModContent.GetInstance<RangeMageHybridDamageClass>();
		Projectile.penetrate = -1;
		Projectile.frame = 0;
		Projectile.timeLeft = 15;
		Projectile.scale = 4;
		Projectile.tileCollide = false;
	}

	public override void AI() {
		Projectile.ai[0] += 0.1f;
	}
	public override bool PreDraw(ref Color lightColor) {
		ShaderSettings shaderSettings = new ShaderSettings();
		shaderSettings.image1 = TextureAssets.Extra[193];
		shaderSettings.Color = Color.Turquoise;
		shaderSettings.shaderData = new Vector4(Projectile.ai[0]);
		default(ExplosionQuad).Draw(Projectile.Center, 0, Vector2.One * 512, shaderSettings);
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		return false;
	}
}
