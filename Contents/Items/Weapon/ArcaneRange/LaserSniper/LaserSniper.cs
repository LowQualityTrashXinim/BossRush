using BossRush.Common;
using BossRush.Common.Graphics;
using BossRush.Common.Graphics.Primitives;
using BossRush.Common.Graphics.RenderTargets;
using BossRush.Common.Graphics.TrailStructs;
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

		if (Main.myPlayer == player.whoAmI) {

			var dir = Main.MouseWorld.X > player.Center.X ? 1 : -1;


			if (itemProj != null)
				itemProj.Projectile.Kill();

			var rot = player.Center.DirectionTo(Main.MouseWorld);

			itemProj = ItemProjectile.SpawnItemProjectile(player, player.Center + player.Center.DirectionTo(Main.MouseWorld) * 15, rot.ToRotation(), Item.useAnimation, TextureAssets.Item[Type].Value, Rectangle.Empty, TextureAssets.Item[Type].Value.Size() / 2f, new Vector2(1f, 1f), dir == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, BoltActionAnimation);
	
			recoilScale.SetProperties(new Vector2(0.8f, 2f), new Vector2(1f, 1f), TweenEaseType.OutExpo, Item.useAnimation / 3);
			recoilPos.SetProperties(new Vector2(0,0),new Vector2(0,0).PositionOFFSET(rot,15), TweenEaseType.OutExpo, player.itemAnimationMax / 3);
			recoilPos.Start();
			recoilScale.Start();

			recoilHandler.tweens.Clear();
			recoilHandler.tweens.Add(new Tween<float>(MathHelper.Lerp).SetProperties(rot.ToRotation() - MathHelper.PiOver2 / 2 * dir, rot.ToRotation(), TweenEaseType.None, player.itemAnimationMax / 2));
			recoilHandler.PlayTweens();
		}

		return true;
	}
	public bool BoltActionAnimation(Player player, Vector2 pos, Vector2 offset) 
	{

		recoilScale.Update();
		recoilPos.Update();
		recoilHandler.Update();


		if(player.itemAnimation == player.itemAnimationMax)
			itemProj.PositionOffset = player.Center.DirectionTo(Main.MouseWorld) * 15;

		itemProj.spriteScale = recoilScale.currentProgress;
		itemProj.Projectile.rotation = recoilHandler.currentTween.currentProgress;
		player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Quarter, player.direction == 1 ? itemProj.Projectile.rotation : itemProj.Projectile.rotation - MathHelper.Pi );
		player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, player.direction == 1 ? itemProj.Projectile.rotation - MathHelper.PiOver2: itemProj.Projectile.rotation - MathHelper.PiOver2);

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
	public override Vector2? HoldoutOffset() {
		return new Vector2(-20, 0);
	}
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (player.GetModPlayer<LaserSniperPlayer>().ManaMissing) {
			damage = (int)(damage * .8f);
		}
		type = ModContent.ProjectileType<LaserSniperProjectile>();
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {

		Projectile projectile = Projectile.NewProjectileDirect(source, position.PositionOFFSET(velocity, 90), velocity, type, damage, knockback, player.whoAmI);

		if (!player.GetModPlayer<LaserSniperPlayer>().ManaMissing) {
			projectile.damage *= 2;
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

public class LaserSniperProjectile : ModProjectile 
{
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
		s.offset = Projectile.Size / 2f;
		s.image1 = TextureAssets.Extra[193];
		s.oldPos = Projectile.oldPos;
		s.oldRot = Projectile.oldRot;
		s.Color = Color.Aqua;
		s.shaderType = "FlameEffect";
		default(GenericTrail).Draw(s,(a) => MathHelper.Lerp(2f, 6f, Utils.GetLerpValue(0f, 0.2f, a, clamped: true)) * Utils.GetLerpValue(0f, 0.07f, a, clamped: true), (a) => Color.Aqua);

		return false;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(3) || target.type == ModContent.NPCType<PlasmaGrenade>()) 
		{

			var grenade = NPC.NewNPCDirect(Projectile.GetSource_OnHit(target),(int)Projectile.Center.X,(int)Projectile.Center.Y,ModContent.NPCType<PlasmaGrenade>());
			grenade.velocity.Y = -10;

		}
	}
}

public class PlasmaGrenade : ModNPC 
{
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(657);
	public override void SetDefaults() {
		NPC.CloneDefaults(ModContent.NPCType<FallenStarPower>());
		NPC.HitSound = SoundID.DD2_ExplosiveTrapExplode;
		NPC.noTileCollide = true;
	}

	public override bool? CanBeHitByProjectile(Projectile projectile) {
		return projectile.type == ModContent.ProjectileType<LaserSniperProjectile>();
	}

	public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone) {
		Projectile.NewProjectile(NPC.GetSource_OnHurt(Main.player[projectile.owner]),projectile.Center,Vector2.Zero,ModContent.ProjectileType<PlasmaExplosion>(),projectile.damage / 3, 0, projectile.owner);
	}

	
	public override void AI() {
		NPC.velocity.Y += 0.2f;
		if (NPC.collideY || NPC.collideX)
			NPC.StrikeInstantKill();

		NPC.rotation += 0.1f;
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {

		Main.instance.LoadItem(ItemID.Grenade);
		Main.EntitySpriteDraw(TextureAssets.Item[ItemID.Grenade].Value, NPC.Center - Main.screenPosition, null, Color.Aqua, NPC.rotation, TextureAssets.Item[ItemID.Grenade].Size() / 2f, 1.1f, SpriteEffects.None);
		Main.EntitySpriteDraw(TextureAssets.Item[ItemID.Grenade].Value, NPC.Center - Main.screenPosition, null, Color.Aqua, NPC.rotation, TextureAssets.Item[ItemID.Grenade].Size() / 2f, 1f, SpriteEffects.None);

		return false;
	}
}

public class PlasmaExplosion : ModProjectile 
{

	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.Flames);

	public override void SetStaticDefaults() {
		Main.projFrames[Type] = 6;
	}
	private PrimitiveDrawer primitiveDrawer;

	public override void SetDefaults() {
		Projectile.width = Projectile.height = 98;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.DamageType = ModContent.GetInstance<RangeMageHybridDamageClass>();
		Projectile.penetrate = -1;
		Projectile.frame = 0;
		Projectile.timeLeft = 35;
		Projectile.scale = 4;
	}

	public override void OnSpawn(IEntitySource source) {
		primitiveDrawer = new PrimitiveDrawer(PrimitiveShape.Quad);
	}

	public override void AI() {


		Projectile.ai[0] += 0.1f;
	}

	public override bool PreDraw(ref Color lightColor) {

		ModdedShaderHandler shader = EffectsLoader.shaderHandlers["FlameEffect"];
		shader.setProperties(Color.Aqua, TextureAssets.Extra[193].Value,shaderData: new Vector4(Projectile.ai[0], Projectile.ai[0], Projectile.ai[0],0));
		shader.apply();

		primitiveDrawer.Draw([Projectile.Center], [Color.White], [new Vector2(512)]);

		Main.pixelShader.CurrentTechnique.Passes[0].Apply();

		return false;
	}
}
