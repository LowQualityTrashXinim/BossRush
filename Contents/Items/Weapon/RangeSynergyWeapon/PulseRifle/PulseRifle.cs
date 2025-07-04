﻿using BossRush.Common.Global;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.PulseRifle;
internal class PulseRifle : SynergyModItem {
	public override void Synergy_SetStaticDefaults() {
		SynergyBonus_System.Add_SynergyBonus(Type, ItemID.SniperRifle, $"[i:{ItemID.SniperRifle}] 20% critical strike chance, 100% critical strike damage and pulse bolt ignore armor");
		SynergyBonus_System.Add_SynergyBonus(Type, ItemID.MagicMissile, $"[i:{ItemID.MagicMissile}] Have 1 in 10 chance to shoot additional magic missle");
		SynergyBonus_System.Add_SynergyBonus(Type, ItemID.ClockworkAssaultRifle, $"[i:{ItemID.ClockworkAssaultRifle}] Shoot burst arch and missle more common");
	}
	public override void SetDefaults() {
		Item.BossRushDefaultRange(94, 34, 64, 4f, 7, 7, ItemUseStyleID.Shoot, ProjectileID.PulseBolt, 16f, true, AmmoID.Bullet);
		Item.scale = .78f;
		Item.crit = 12;
		Item.UseSound = SoundID.Item75 with { Pitch = 1 };
	}
	public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
		SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.SniperRifle);
		SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.MagicMissile);
		SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.ClockworkAssaultRifle);
	}
	public override Vector2? HoldoutOffset() {
		return new Vector2(-20, 0);
	}
	public int Counter = 0;
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		type = ProjectileID.PulseBolt;
	}
	public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
		if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.SniperRifle)) {
			PlayerStatsHandle statplayer = player.GetModPlayer<PlayerStatsHandle>();
			statplayer.AddStatsToPlayer(PlayerStats.CritDamage, 2);
			statplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 20);
		}
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		Counter++;
		if (Counter >= 30 || SynergyBonus_System.Check_SynergyBonus(Type, ItemID.ClockworkAssaultRifle) && Counter >= 10) {
			for (int i = 0; i < 4; i++) {
				int proj = Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 50), velocity.Vector2DistributeEvenlyPlus(4, 40, i), type, damage, knockback, player.whoAmI);
				Main.projectile[proj].penetrate = 1;
			}
			Counter = 0;
		}
		if (Main.rand.NextBool(5) || SynergyBonus_System.Check_SynergyBonus(Type, ItemID.ClockworkAssaultRifle)) {
			Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 50), velocity.Vector2RotateByRandom(30) * .1f, ModContent.ProjectileType<PulseHomingProjectile>(), (int)(damage * 1.25f), knockback, player.whoAmI);
		}
		if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.MagicMissile) && (Main.rand.NextBool(5) || SynergyBonus_System.Check_SynergyBonus(Type, ItemID.ClockworkAssaultRifle))) {
			int proj = Projectile.NewProjectile(source, position.PositionOFFSET(velocity, 50), velocity.Vector2RotateByRandom(30) * .1f, ProjectileID.MagicMissile, (int)(damage), knockback, player.whoAmI);
			Main.projectile[proj].penetrate = 1;
			Main.projectile[proj].maxPenetrate = 1;
		}
		base.SynergyShoot(player, modplayer, source, position, velocity, type, damage, knockback, out CanShootItem);
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.PulseBow)
			.AddIngredient(ItemID.Megashark)
			.Register();
	}
}
public class PulseRifle_ModPlayer : ModPlayer {
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.type == ProjectileID.PulseBolt && proj.Check_ItemTypeSource(ModContent.ItemType<PulseRifle>()) && SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<PulseRifle>(), ItemID.SniperRifle)) {
			modifiers.ScalingArmorPenetration += 1;
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type == ProjectileID.PulseBolt && proj.Check_ItemTypeSource(ModContent.ItemType<PulseRifle>())) {
			target.AddBuff<PlasmaDefensesMelt>(BossRushUtils.ToSecond(.5f));
		}
	}
}
public class PlasmaDefensesMelt : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().StatDefense -= .5f;
	}
}
public class PulseHomingProjectile : SynergyModProjectile {
	public override string Texture => BossRushTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 0;
		ProjectileID.Sets.TrailCacheLength[Type] = 100;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 1800;
		Projectile.extraUpdates = 20;
		Projectile.penetrate = 1;
	}
	NPC npc = null;
	public override Color? GetAlpha(Color lightColor) {
		return new Color(255, 255, 255, 255);
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		if (npc == null) {
			if (Main.MouseWorld.LookForHostileNPC(out NPC target, 1500f)) {
				npc = target;
			}
		}
		else {
			if (!npc.active || npc.life <= 0) {
				npc = null;
				return;
			}
			Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * (npc.Center - Projectile.Center).Length() / 32f;
			Projectile.velocity = Projectile.velocity.LimitedVelocity(5);
		}
	}
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC npc, NPC.HitInfo hit, int damageDone) {
		for (int i = 0; i < 35; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.GemSapphire);
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2CircularEdge(5, 5) * Main.rand.NextFloat(.9f, 1.2f);
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
		for (int i = 0; i < Projectile.oldPos.Length; i++) {
			Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			Color color = new Color(100, 100, 255, 255) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
			Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, Projectile.scale - i * .01f, SpriteEffects.None, 0);
		}
		for (int i = 0; i < Projectile.oldPos.Length; i++) {
			Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			Color color2 = new Color(255, 255, 255, 255) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
			Main.EntitySpriteDraw(texture, drawPos, null, color2, Projectile.rotation, origin, (Projectile.scale - i * .01f) * .35f, SpriteEffects.None, 0);
		}
		return base.PreDraw(ref lightColor);
	}
}
