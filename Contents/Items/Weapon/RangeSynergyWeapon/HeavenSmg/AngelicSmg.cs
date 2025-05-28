using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using BossRush.Texture;
using Terraria.Graphics;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.HeavenSmg {
	public class AngelicSmg : SynergyModItem {
		public override void SetDefaults() {
			BossRushUtils.BossRushDefaultRange(Item, 32, 32, 24, 1, 42, 42, ItemUseStyleID.Shoot, ProjectileID.Bullet, 25, true, AmmoID.Bullet);
			Item.reuseDelay = 30;
			Item.noUseGraphic = true;
		}
		public override bool AltFunctionUse(Player player) => true;
		public override float UseSpeedMultiplier(Player player) {
			if (player.altFunctionUse == 2) {
				return base.UseSpeedMultiplier(player) * 10;
			}
			return base.UseSpeedMultiplier(player);
		}
		public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (player.altFunctionUse == 2) {
				type = ModContent.ProjectileType<AngelicSmgThrow>();
				damage *= 3;
			}
			else {
				SoundEngine.PlaySound(SoundID.Item36 with { Pitch = 1.5f }, player.Center);
			}
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			if (player.altFunctionUse != 2) {
				int useAnimation = (int)(Item.useAnimation * player.GetWeaponAttackSpeed(Item));
				Projectile projectile = Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, ModContent.ProjectileType<AngelicSmgHeld>(), damage, knockback, player.whoAmI, useAnimation / 14f, useAnimation);
				AngelicSmgHeld modprojectile = (AngelicSmgHeld)projectile.ModProjectile;
				modprojectile.shootVelocity = velocity;
				modprojectile.type = type;
				modprojectile.ammoItemID = source.AmmoItemIdUsed;
			}
			CanShootItem = true;
		}
		public override bool CanUseItem(Player player) {
			return player.ownedProjectileCounts[ModContent.ProjectileType<AngelicSmgThrow>()] < 1;
		}
		public override Vector2? HoldoutOffset() {
			return new(-5, 0);
		}
	}
	public struct HeavenTrail {
		private static VertexStrip _vertexStrip = new VertexStrip();
		public void Draw(Projectile proj) {
			MiscShaderData miscShaderData = GameShaders.Misc["MagicMissile"];
			miscShaderData.UseSaturation(-2f);
			miscShaderData.UseOpacity(2f);
			miscShaderData.Apply();
			_vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, StripColors, StripWidth, -Main.screenPosition + proj.Size * .5f);
			_vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}
		private Color StripColors(float progressOnStrip) {
			Color result = new Color(255, 255, 255, 0);
			//result.A /= 2;
			return result;
		}
		private float StripWidth(float progressOnStrip) => MathHelper.Lerp(5f, 32f, Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true)) * Utils.GetLerpValue(0f, 0.07f, progressOnStrip, clamped: true);
	}
	public class AngelicSmgHeld : ModProjectile {
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 5;
		}
		public override void SetDefaults() {
			Projectile.width = 56;
			Projectile.height = 56;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
		}
		public override bool? CanDamage() {
			return false;
		}
		public Vector2 shootVelocity = Vector2.Zero;
		public Vector2 positiionOffSet = Vector2.Zero;
		public int ammoItemID = ItemID.MusketBall;
		public int type = ProjectileID.Bullet;
		public int useTime { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
		public int useAnimation { get => (int)Projectile.ai[1]; set => Projectile.ai[1] = value; }
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if (Projectile.timeLeft > player.itemAnimationMax + 30) {
				Projectile.ai[1] = shootVelocity.Length();
				Projectile.timeLeft = player.itemAnimationMax + 30;
			}
			player.heldProj = Projectile.whoAmI;
			Vector2 velocity = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * Projectile.ai[1];
			Vector2 OFFSET = velocity.SafeNormalize(Vector2.Zero);
			Projectile.Center = player.Center + OFFSET * 10 + new Vector2(0, -5f);
			if (++Projectile.ai[2] >= useTime && Projectile.timeLeft >= 30) {
				int damage = Projectile.damage;
				if (player.HasBuff<AngelicSmgBuff>()) {
					type = ModContent.ProjectileType<AngelicBolt>();
					damage = (int)(damage * 1.5f);
				}
				SoundEngine.PlaySound(SoundID.Item36 with { Pitch = 1.5f }, player.Center);
				Vector2 vel = velocity.Vector2RotateByRandom(10);
				Projectile.NewProjectile(
					player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, ammoItemID),
					Projectile.Center + OFFSET * 40,
					vel,
					type,
					damage,
					Projectile.knockBack,
					Projectile.owner
					);
				Projectile.rotation = vel.ToRotation() + (player.direction == 1 ? 0 : MathHelper.Pi);
				Projectile.ai[2] = 0;
			}
			player.direction = velocity.X > 0 ? 1 : -1;
			Projectile.spriteDirection = player.direction;
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2 * player.direction);
			if (++Projectile.frameCounter >= 3) {
				Projectile.frameCounter = 0;
				Projectile.frame += 1;
				if (Projectile.frame >= Main.projFrames[Type]) {
					Projectile.frame = 0;
				}
			}
		}
	}
	public class AngelicSmgThrow : SynergyModProjectile {
		static bool returningToOwner = false;
		static bool targetHit = false;
		int oldposFrameAmount = 15;
		public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<AngelicSmg>();
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = oldposFrameAmount;
		}
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.timeLeft = 30;
			Projectile.aiStyle = -1;
			Projectile.alpha = 0;
			Projectile.penetrate = -1;
			Projectile.idStaticNPCHitCooldown = 30;
			returningToOwner = false;
			Projectile.usesIDStaticNPCImmunity = true;
			currentSpeed = 0;
			targetHit = false;
		}
		public override void OnSpawn(IEntitySource source) {
			SoundEngine.PlaySound(SoundID.Item71 with { Pitch = 2f }, Projectile.Center);
			SoundEngine.PlaySound(SoundID.Item71 with { Pitch = 0.25f }, Projectile.Center);
		}
		private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawpos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness) {
			Texture2D sparkleTexture = TextureAssets.Extra[98].Value;
			Color bigColor = shineColor * opacity * 0.5f;
			bigColor.A = 0;
			Vector2 origin = sparkleTexture.Size() * .5f;
			Color smallColor = drawColor * 0.5f;
			float lerpValue = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
			Vector2 scaleLeftRight = new Vector2(fatness.X * 0.5f, scale.X) * lerpValue;
			Vector2 scaleUpDown = new Vector2(fatness.Y * 0.5f, scale.Y) * lerpValue;
			bigColor *= lerpValue;
			smallColor *= lerpValue;
			// Bright, large part
			Main.EntitySpriteDraw(sparkleTexture, drawpos, null, bigColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight, dir);
			Main.EntitySpriteDraw(sparkleTexture, drawpos, null, bigColor, 0f + rotation, origin, scaleUpDown, dir);
			// Dim, small part
			Main.EntitySpriteDraw(sparkleTexture, drawpos, null, smallColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight * 0.6f, dir);
			Main.EntitySpriteDraw(sparkleTexture, drawpos, null, smallColor, 0f + rotation, origin, scaleUpDown * 0.6f, dir);
		}
		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2f, 1f, SpriteEffects.None);
			if (targetHit) {
				DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, Projectile.Center - Main.screenPosition, new Color(255, 255, 255, 0), new Color(200, 255, 200, 0), currentSpeed / maxThrowSpeed, 0f, 0.1f, 0f, 1f, 0f, texture.Size() / 2f, Vector2.One);
				DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, Projectile.Center - Main.screenPosition, new Color(255, 255, 255, 125), new Color(200, 255, 200, 125), currentSpeed / maxThrowSpeed, 0f, 0.25f, 0f, 0.75f, MathHelper.PiOver4, texture.Size() / (maxThrowSpeed / currentSpeed), Vector2.One);
				if (Projectile.timeLeft < timeleftReset - oldposFrameAmount / 3)
					default(HeavenTrail).Draw(Projectile);
			}
			return false;
		}
		public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
			if (!returningToOwner) {
				targetHit = true;
				player.AddBuff(ModContent.BuffType<AngelicSmgBuff>(), 300);
				player.GetModPlayer<HeavenSmgPlayer>().IncreaseStack();
			}
			returnToPlayer();
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			resetStacks();
			returnToPlayer();
			return false;
		}
		private void resetStacks() {
			Player player = Main.player[Projectile.owner];
			player.GetModPlayer<HeavenSmgPlayer>().ModPlayer_resetStacks();
		}
		private void returnToPlayer() {
			if (!returningToOwner) {
				Projectile.velocity.Y -= 15f;
				returningToOwner = true;
				Projectile.timeLeft = timeleftReset;
			}
		}
		float currentSpeed = 0;
		float maxThrowSpeed = 50f;
		float accel = 0.8f;
		public int timeleftReset = 120;
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			if (!returningToOwner && Projectile.timeLeft <= 2) {
				resetStacks();
				returnToPlayer();
			}
			Projectile.rotation += Projectile.velocity.ToRotation();
			if (returningToOwner) {
				Vector2 vel = player.Center - Projectile.Center;
				vel.Normalize();
				currentSpeed += accel;
				if (currentSpeed > maxThrowSpeed) {
					currentSpeed = maxThrowSpeed;
				}
				vel *= currentSpeed;
				vel.Y -= 1.25f * (maxThrowSpeed / currentSpeed);
				Projectile.velocity = vel;
				if (Projectile.Center.Distance(player.Center) <= 35)
					Projectile.Kill();
			}
		}
		public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
			if (targetHit)
				for (int i = 1; i < oldposFrameAmount; i++) {
					Vector2 oldVel = Projectile.oldPos[i].DirectionTo(Projectile.oldPos[i - 1]);
					oldVel *= 50;
					for (int j = 0; j < 10; j++) {
						int dustyDust = Dust.NewDust(Projectile.oldPos[i], (int)Projectile.Size.X / 2, (int)Projectile.Size.Y / 2, DustID.WhiteTorch, oldVel.X, oldVel.Y, (i * 20), default, 1);
						Main.dust[dustyDust].noGravity = true;
					}
				}
		}
	}
	internal class AngelicSmgBuff : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void Update(Player player, ref int buffIndex) {
			player.slowFall = true;
			player.jumpSpeedBoost = 5;
			if (player.buffTime[buffIndex] <= 0) {
				player.GetModPlayer<HeavenSmgPlayer>().IncreaseStack();
			}
		}
	}
	internal class AngelicBolt : SynergyModProjectile {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		static int oldposFrameAmount = 25;
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = oldposFrameAmount;
		}
		bool isMiniProjectile = false;
		int miniProjectileAmount = 3;
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.extraUpdates = 1;
			Projectile.timeLeft = 570;
			Projectile.aiStyle = -1;
			Projectile.alpha = 0;
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			isMiniProjectile = false;
			projSpeed = 0f;
		}
		public override void OnSpawn(IEntitySource source) {
			if (Projectile.ai[0] == 1)
				isMiniProjectile = true;
		}
		float maxProjSpeed = 15f;
		float projSpeed = 0f;
		public override bool? CanHitNPC(NPC target) => true;
		public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			float maxDetectRadius = 2000f;
			Vector2 vel = Projectile.velocity;
			float accel = 2f;
			Projectile.Center.LookForHostileNPC(out NPC closestNPC, maxDetectRadius);
			if (closestNPC == null || (Projectile.timeLeft > 570 && Projectile.ai[0] == 1)) {
				accel = 1f;
			}
			else {
				vel = closestNPC.Center - Projectile.Center;
			}
			vel.Normalize();
			projSpeed += accel;
			if (projSpeed > maxProjSpeed) {
				projSpeed = maxProjSpeed;
			}
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Projectile.Center + vel) * projSpeed, 0.025f);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D MainTextureForProj = TextureAssets.Extra[98].Value;
			Color MainColor = new Color(200 + (MathF.Sin(Projectile.timeLeft) * 55), 255, 225, 0);
			Vector2 projectilePos = Projectile.Center - Main.screenPosition;
			Vector2 projectileOrigin = MainTextureForProj.Size() / 2f;
			Main.EntitySpriteDraw(MainTextureForProj, projectilePos, null, MainColor, Projectile.rotation, projectileOrigin, isMiniProjectile == true ? 1f : 1.5f, SpriteEffects.None);
			if (Projectile.ai[0] == 0)
				DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, projectilePos, MainColor, MainColor, 0.5f, 0f, 0.5f, 0.5f, 1f, 0f, Vector2.One * 2, Vector2.One);
			default(HeavenTrail).Draw(Projectile);
			return false;
		}
		private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawpos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness) {
			Texture2D sparkleTexture = TextureAssets.Extra[98].Value;
			Color bigColor = shineColor * opacity * 0.5f;
			bigColor.A = 0;
			Vector2 origin = sparkleTexture.Size() / 2f;
			Color smallColor = drawColor * 0.5f;
			float lerpValue = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
			Vector2 scaleLeftRight = new Vector2(fatness.X * 0.5f, scale.X) * lerpValue;
			Vector2 scaleUpDown = new Vector2(fatness.Y * 0.5f, scale.Y) * lerpValue;
			bigColor *= lerpValue;
			smallColor *= lerpValue;
			// Bright, large part
			Main.EntitySpriteDraw(sparkleTexture, drawpos, null, bigColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight, dir);
			Main.EntitySpriteDraw(sparkleTexture, drawpos, null, bigColor, 0f + rotation, origin, scaleUpDown, dir);
			// Dim, small part
			Main.EntitySpriteDraw(sparkleTexture, drawpos, null, smallColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight * 0.6f, dir);
			Main.EntitySpriteDraw(sparkleTexture, drawpos, null, smallColor, 0f + rotation, origin, scaleUpDown * 0.6f, dir);
		}
		public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
			SoundEngine.PlaySound(SoundID.Item125 with { Pitch = 2f }, Projectile.Center);
			for (int i = 0; i < 35; i++) {
				var dust = Dust.NewDustPerfect(Projectile.Center, DustID.WhiteTorch, Main.rand.NextVector2CircularEdge(1f, 1f) * (isMiniProjectile == true ? 5f : 15f), default, new Color(255, 255, 255, 0), 2f);
				dust.noGravity = true;
			}
		}
	}
	public class HeavenSmgPlayer : ModPlayer {
		public int HeavenSmg_Stacks = 0;
		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
			if (Player.HeldItem.type == ModContent.ItemType<AngelicSmg>()) {
				ModPlayer_resetStacks();
			}
		}
		public void IncreaseStack() {
			if (!Player.HasBuff<AngelicSmgBuff>()) {
				for (int i = 0; i < 5 + HeavenSmg_Stacks; i++) {
					Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.One.Vector2DistributeEvenly(5f + HeavenSmg_Stacks, 360, i), ModContent.ProjectileType<AngelicBolt>(), 30, 0, Player.whoAmI, 1);
				}
			}
			else {
				if (HeavenSmg_Stacks >= 40) {
					SoundEngine.PlaySound(SoundID.Item9 with { Pitch = -2f }, Player.Center);
					return;
				}
				HeavenSmg_Stacks++;
				SoundEngine.PlaySound(SoundID.NPCHit5 with { Pitch = HeavenSmg_Stacks * 0.075f }, Player.Center);
			}
		}
		public void ModPlayer_resetStacks() {
			if (Player.HeldItem.type == ModContent.ItemType<AngelicSmg>()) {
				SoundEngine.PlaySound(SoundID.NPCDeath7, Player.Center);
			}
			HeavenSmg_Stacks = 0;
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (item.type == ModContent.ItemType<AngelicSmg>()) {
				damage += HeavenSmg_Stacks * 0.05f;
			}
		}
		public override float UseSpeedMultiplier(Item item) {
			float multiplier = base.UseAnimationMultiplier(item);
			if (item.type == ModContent.ItemType<AngelicSmg>()) {
				return multiplier + HeavenSmg_Stacks * 0.01f;
			}
			return base.UseSpeedMultiplier(item);
		}
	}
}
