using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using BossRush.Texture;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.Localization;
using BossRush.Common.Graphics.Structs.TrailStructs;

namespace BossRush.Contents.Items.Weapon.SummonerSynergyWeapon.StarWhip;
public class StarWhip : SynergyModItem {
	public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(StarWhipDebuff.TagDamage);
	public override void SetDefaults() {
		Item.DefaultToWhip(ModContent.ProjectileType<StarWhipProj>(), 100, 1, 9f, 30);
		Item.UseSound = SoundID.Item116;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.RainbowWhip)
			.AddIngredient(ItemID.MaceWhip)
			.Register();
	}

	public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
		foreach (NPC npc in Main.ActiveNPCs)
			if (npc.type == ModContent.NPCType<FallenStarPower>())
				return;
		if (player.HasBuff<StarWhipBuff>())
			return;
		Vector2 pos = (player.Center + new Vector2(500, 0)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi), player.Center);
		if (!WorldGen.TileEmpty(pos.ToTileCoordinates().X, pos.ToTileCoordinates().Y))
			return;
		if (Main.myPlayer == player.whoAmI)
			NPC.NewNPC(player.GetSource_FromThis(), (int)pos.X, (int)pos.Y, ModContent.NPCType<FallenStarPower>());
	}

	public override Color? GetAlpha(Color lightColor) {
		return Color.White;
	}
}
public class StarWhipProj : ModProjectile {
	int tracker;
	bool empoweredHit = false;
	bool empowered = false;
	public override void SetDefaults() {
		Projectile.DefaultToWhip();
		Projectile.WhipSettings.Segments = 50;
	}
	public override void OnSpawn(IEntitySource source) {
		empoweredHit = false;
		empowered = false;
		tracker = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<StarWhipTipTracker>(), 0, 0, Projectile.owner);
		if (Main.player[Projectile.owner].HasBuff(ModContent.BuffType<StarWhipBuff>()))
			empowered = true;
	}
	public override void SetStaticDefaults() {
		ProjectileID.Sets.IsAWhip[Type] = true;
	}

	public override void AI() {
		List<Vector2> temp = new List<Vector2>();
		Projectile.FillWhipControlPoints(Projectile, temp);
		Main.projectile[tracker].position = temp.Last();
		Main.projectile[tracker].timeLeft = 2;
		Main.projectile[tracker].rotation = Projectile.rotation;
		base.AI();
	}

	public override bool PreDraw(ref Color lightColor) {
		List<Vector2> list = new List<Vector2>();
		Projectile.FillWhipControlPoints(Projectile, list);

		Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
		Main.DrawWhip_WhipBland(Projectile, list);

		BossRushUtils.DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, list.ElementAt(list.Count - 2) - Main.screenPosition, Color.Yellow, Color.White, 0.1f, 0f, 0.1f, 0f, 0.25f, Projectile.rotation, new Vector2(4f, 4f), new Vector2(3f));
		default(StarTrail).Draw(Main.projectile[tracker].oldPos, Main.projectile[tracker].oldRot, Projectile.Size * 0.5f);

		if (empowered)
			default(StarTrailEmpowered).Draw(Main.projectile[tracker].oldPos, Main.projectile[tracker].oldRot, Projectile.Size * 0.5f);

		return false;
	}

	public override void OnKill(int timeLeft) {
		if (empowered)
			Main.player[Projectile.owner].ClearBuff(ModContent.BuffType<StarWhipBuff>());
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (empowered && !empoweredHit && target.type != ModContent.NPCType<FallenStarPower>()) {
			empoweredHit = true;
			Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_OnHit(target), target.Center - new Vector2(0, 500), Vector2.UnitY * 5, ModContent.ProjectileType<StarWhipStarProj>(), Projectile.damage, 0, Projectile.owner, 4, 0, -1);
			proj.ai[2] = target.whoAmI;
		}
	}

}

// mainly used for smoother trails using the TrailingMode 3 
public class StarWhipTipTracker : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.AbigailMinion);

	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 15;
		ProjectileID.Sets.TrailingMode[Type] = 3;
	}
	public override bool PreDraw(ref Color lightColor) {
		return false;
	}
}


public class FallenStarPower : ModNPC {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.StarCannonStar);
	public override void SetDefaults() {
		NPC.friendly = false;
		NPC.lifeMax = 1;
		NPC.damage = 0;
		NPC.noGravity = true;
		NPC.width = NPC.height = 25;
		NPC.dontTakeDamageFromHostiles = true;
		NPC.DeathSound = SoundID.Item4;
		NPC.noTileCollide = true;
	}
	public override void OnSpawn(IEntitySource source) {
		for (int i = 0; i < 35; i++) {
			var dust = Dust.NewDustPerfect(NPC.position, DustID.YellowStarDust, Main.rand.NextVector2CircularEdge(15, 15));
			dust.noGravity = true;
		}
	}
	public override bool? CanBeHitByItem(Player player, Item item) => false;
	public override bool? CanBeHitByProjectile(Projectile projectile) => projectile.type == ModContent.ProjectileType<StarWhipProj>() ? true : false;
	public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers) {
		modifiers.HideCombatText();
		Main.player[projectile.owner].AddBuff(ModContent.BuffType<StarWhipBuff>(), BossRushUtils.ToSecond(5));

	}
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
		Asset<Texture2D> texture = TextureAssets.Projectile[ProjectileID.StarCannonStar];
		Main.EntitySpriteDraw(texture.Value, NPC.Center - screenPos + new Vector2(MathF.Cos(NPC.ai[0]) * 10, MathF.Cos(NPC.ai[0]) * 10), null, Color.White * 0.5f, 0, texture.Size() / 2f, 1f, SpriteEffects.None);
		Main.EntitySpriteDraw(texture.Value, NPC.Center - screenPos + new Vector2(MathF.Sin(NPC.ai[0]), 0), null, Color.White, 0, texture.Size() / 2f, 1f, SpriteEffects.None);
		Main.EntitySpriteDraw(texture.Value, NPC.Center - screenPos + new Vector2(MathF.Cos(-NPC.ai[0]) * 10, MathF.Sin(-NPC.ai[0]) * 10), null, Color.White * 0.5f, 0, texture.Size() / 2f, 1f, SpriteEffects.None);
		Main.EntitySpriteDraw(texture.Value, NPC.Center - screenPos + new Vector2(MathF.Sin(NPC.ai[0]), 0), null, Color.White, 0, texture.Size() / 2f, 1f, SpriteEffects.None);
		Main.EntitySpriteDraw(texture.Value, NPC.Center - screenPos, null, new Color(255, 255, 255, 0), 0, texture.Size() / 2f, 0.8f, SpriteEffects.None);
		BossRushUtils.DrawPrettyStarSparkle(NPC.Opacity, SpriteEffects.None, NPC.Center - Main.screenPosition, Color.Yellow, Color.White, 0.25f, 0f, 0.5f, 0f, 1f, NPC.rotation, new Vector2(1f, 1f), new Vector2(15f));
		return false;
	}

	public override void AI() {
		NPC.ai[0] += 0.2f;
		NPC.ai[1]++;

		if (NPC.ai[1] >= BossRushUtils.ToSecond(10))
			NPC.EncourageDespawn(2);
		NPC.TargetClosest();
		Player player = Main.player[NPC.target];
		NPC.NPCMoveToPosition(player.Center, 4f, 300);
	}
}

public class StarWhipBuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
}

public class StarWhipGlobalNPC : GlobalNPC {
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		if (projectile.npcProj || projectile.trap || !projectile.IsMinionOrSentryRelated)
			return;
		var damageMultiplier = ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type];
		if (npc.HasBuff<StarWhipDebuff>()) {
			modifiers.FlatBonusDamage += StarWhipDebuff.TagDamage * damageMultiplier;
		}
	}
}
public class StarWhipDebuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public static readonly int TagDamage = 30;
	public override void SetStaticDefaults() {
		BuffID.Sets.IsATagBuff[Type] = true;
	}
}
public class StarWhipStarProj : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Projectile>(ProjectileID.StarCannonStar);
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 3;
		ProjectileID.Sets.TrailCacheLength[Type] = 35;
	}
	Vector2 spawnPos = Vector2.Zero;
	public override void SetDefaults() {
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.width = Projectile.height = 30;
		Projectile.aiStyle = -1;
		Projectile.tileCollide = false;
		Projectile.extraUpdates = 5;
	}
	public override void OnSpawn(IEntitySource source) {
		spawnPos = Projectile.Center;
		SoundEngine.PlaySound(SoundID.Item125, Projectile.Center);
		for (int i = 0; i < 35; i++) {

			var dust = Dust.NewDustPerfect(Projectile.position, DustID.YellowStarDust, Main.rand.NextVector2CircularEdge(30, 30));
			dust.noGravity = true;

		}
	}
	public override Color? GetAlpha(Color lightColor) {
		return Color.White;
	}
	public override bool PreDraw(ref Color lightColor) {
		Asset<Texture2D> StarTexture = TextureAssets.Projectile[Type];
		Main.instance.LoadProjectile(Type);
		Main.EntitySpriteDraw(StarTexture.Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.velocity.ToRotation() + MathHelper.PiOver2, StarTexture.Size() / 2f, 1f, SpriteEffects.None);
		default(StarTrailEmpowered).Draw(Projectile.oldPos, Projectile.oldRot, Projectile.Size * 0.5f);
		return false;
	}

	public override void AI() {
		Projectile.ai[1]++;
		Projectile.rotation = Projectile.velocity.ToRotation();
		Vector2 targetPos = Vector2.Zero;
		if (Projectile.ai[2] < 0 || Projectile.ai[2] >= 255) {
			targetPos = Projectile.Center.LookForHostileNPCPositionClosest(1255);
		}
		else {
			NPC npc = Main.npc[(int)Projectile.ai[2]];
			if (npc.active) {
				targetPos = npc.Center;
			}
		}
		if (targetPos == Vector2.Zero)
			targetPos = Projectile.Center + Vector2.UnitY;
		float dirToTargetPos = Projectile.AngleTo(targetPos);

		Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(dirToTargetPos, MathHelper.ToRadians(3)).ToRotationVector2() * Projectile.velocity.Length();
		if (Projectile.ai[0] > 0 && Projectile.ai[1] == 30) {

			Projectile.ai[0]--;
			Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), spawnPos, (Vector2.UnitY * 5).RotatedByRandom(MathHelper.ToRadians(15)), ModContent.ProjectileType<StarWhipStarProj>(), Projectile.damage, 0, Projectile.owner, Projectile.ai[0], 0, Projectile.ai[2]);
		}
	}
}
