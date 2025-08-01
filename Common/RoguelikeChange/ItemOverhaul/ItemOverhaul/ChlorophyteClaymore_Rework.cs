using BossRush.Common.Systems;
using BossRush.Contents.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ItemOverhaul;

public class Roguelike_ChlorophyteClaymore : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return UniversalSystem.Check_RLOH();
	}
	public override void SetDefaults(Item entity) {
		if (entity.type == ItemID.ChlorophyteClaymore) {
			entity.damage += 20;
			entity.knockBack += 5;
			entity.useTime = entity.useAnimation = 35;
			entity.shootsEveryUse = true;
		}
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (item.type == ItemID.ChlorophyteClaymore) {
			BossRushUtils.AddTooltip(ref tooltips, new(Mod, "Roguelike_ChlorophyteClaymore", BossRushUtils.LocalizationText("RoguelikeRework", item.Name)));
		}
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (item.type != ItemID.ChlorophyteClaymore) {
			return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
		}
		int counter = player.GetModPlayer<Roguelike_ChlorophyteClaymore_ModPlayer>().ChlorophyteClaymore_Counter - 150;
		player.GetModPlayer<Roguelike_ChlorophyteClaymore_ModPlayer>().ChlorophyteClaymore_Counter = -player.itemAnimationMax;
		if (player.GetModPlayer<Roguelike_ChlorophyteClaymore_ModPlayer>().PerfectStrike) {
			damage *= 2;
			counter = 100;
		}
		if (counter >= 100) {
			int amount = counter / 10;
			for (int i = 0; i < amount; i++) {
				Projectile.NewProjectileDirect(source, position, velocity.SafeNormalize(Vector2.Zero).Vector2RotateByRandom(45), ModContent.ProjectileType<ChlorophyteOrb_SimplePiercingTrailProjectile>(), damage, knockback, player.whoAmI, Main.rand.NextFloat(3, 5), 180);
			}
		}
		if (counter >= 0) {
			Projectile proj = Projectile.NewProjectileDirect(source, position, velocity.SafeNormalize(Vector2.Zero) * 15, ModContent.ProjectileType<FlyingSlashProjectile>(), damage * 3, knockback, player.whoAmI);
			if (proj.ModProjectile is FlyingSlashProjectile slash) {
				slash.projectileColor = new(90, 255, 90, 0);
			}
			proj.scale += .5f;
			proj.Resize(120, 120);
			return false;
		}
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
}
public class Roguelike_ChlorophyteClaymore_ModPlayer : ModPlayer {
	public int ChlorophyteClaymore_Counter = 0;
	public bool PerfectStrike = false;
	public override void ResetEffects() {
		if (!UniversalSystem.Check_RLOH()) {
			return;
		}
		if (++ChlorophyteClaymore_Counter > 250) {
			ChlorophyteClaymore_Counter = 250;
		}
		if (Player.HeldItem.type != ItemID.ChlorophyteClaymore) {
			return;
		}
		PerfectStrike = ChlorophyteClaymore_Counter >= 150 && ChlorophyteClaymore_Counter <= 170;
		if (PerfectStrike && ChlorophyteClaymore_Counter == 150) {
			SpawnSpecialDustEffect();
		}
	}
	public void SpawnSpecialDustEffect() {
		SoundEngine.PlaySound(SoundID.Item71 with { Pitch = .5f }, Player.Center);
		for (int o = 0; o < 10; o++) {
			for (int i = 0; i < 4; i++) {
				var Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i)) * (3 + Main.rand.NextFloat()) * 5;
				for (int l = 0; l < 8; l++) {
					float multiplier = Main.rand.NextFloat();
					float scale = MathHelper.Lerp(1.1f, .1f, multiplier);
					int dust = Dust.NewDust(Player.Center.Add(0, -60), 0, 0, DustID.GemDiamond, 0, 0, 0, Color.Green, scale);
					Main.dust[dust].velocity = Toward * multiplier;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].Dust_GetDust().FollowEntity = true;
					Main.dust[dust].Dust_BelongTo(Player);
				}
			}
		}
	}
}
public class Roguelike_ChlorophyteClaymore_GlobalProjectile : GlobalProjectile {
	public override void SetDefaults(Projectile entity) {
		if (entity.type == ProjectileID.ChlorophyteOrb) {
			entity.tileCollide = true;
			entity.timeLeft = 600;
		}
	}
	public override void PostAI(Projectile projectile) {
		if (projectile.type == ProjectileID.ChlorophyteOrb) {
			Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.ChlorophyteWeapon);
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
		}
	}
	public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity) {
		if (projectile.type == ProjectileID.ChlorophyteOrb) {
			if (projectile.velocity.Y != oldVelocity.Y) {
				projectile.velocity.Y = -oldVelocity.Y;
			}
			if (projectile.velocity.X != oldVelocity.X) {
				projectile.velocity.X = -oldVelocity.X;
			}
			Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, projectile.velocity, ModContent.ProjectileType<ChlorophyteOrb_SimplePiercingTrailProjectile>(), projectile.damage / 2, projectile.knockBack, projectile.owner, 5, 180);
			return false;
		}
		return base.OnTileCollide(projectile, oldVelocity);
	}
}
/// <summary>
/// Ai0 : shoot velocity<br/>
/// Ai1 : time left of a AI, recommend setting it above 0<br/>
/// Ai2 : Do not touch ai2
/// </summary>
public class ChlorophyteOrb_SimplePiercingTrailProjectile : ModProjectile {
	public Color ProjectileColor = Color.Green;
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.PiercingStarlight);
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 20;
		ProjectileID.Sets.TrailingMode[Type] = 2;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 36;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 60;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.extraUpdates = 2;
	}
	public override void OnSpawn(IEntitySource source) {
		if (Projectile.ai[0] <= 0) {
			Projectile.ai[0] = 1;
		}
		Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * Projectile.ai[0];
		Projectile.rotation = Projectile.velocity.ToRotation();
		if (Projectile.ai[1] <= 0) {
			Projectile.ai[1] = 15;
		}
		Projectile.timeLeft = (int)Projectile.ai[1];
	}
	public override Color? GetAlpha(Color lightColor) {
		ProjectileColor.A = 0;
		return ProjectileColor * Projectile.ai[2];
	}
	public override void AI() {
		ProjectileColor.A = 0;
		Projectile.ai[2] = Projectile.timeLeft / Projectile.ai[1];
		if (Projectile.timeLeft > Projectile.ai[1] * .8f) {
			return;
		}
		if (Projectile.Center.LookForHostileNPC(out NPC npc, 700)) {
			float length = (npc.Center - Projectile.Center).Length() / 32f;
			if (length < 3) {
				length = 3;
			}
			Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * length;
			Projectile.velocity = Projectile.velocity.LimitedVelocity(5);
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.timeLeft = 90;
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(ProjectileID.PiercingStarlight);
		Texture2D texture = TextureAssets.Projectile[ProjectileID.PiercingStarlight].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawPos = Projectile.position - Main.screenPosition + origin * .5f + new Vector2(0f, Projectile.gfxOffY);
		Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(ProjectileColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		DrawTrail2(texture, lightColor, origin);
		return false;
	}
	public void DrawTrail2(Texture2D texture, Color color, Vector2 origin) {
		for (int k = 0; k < Projectile.oldPos.Length; k++) {
			Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin * .5f + new Vector2(0f, Projectile.gfxOffY);
			color = color * .45f * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
			Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(ProjectileColor), Projectile.oldRot[k], origin, (Projectile.scale - k * .05f) * .5f, SpriteEffects.None, 0);
		}
	}
}
