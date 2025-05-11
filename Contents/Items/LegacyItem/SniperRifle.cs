using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using BossRush.Common.Global;
using BossRush.Texture;

namespace BossRush.Contents.Items.LegacyItem;

public class SniperRifle : ModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(120, 53, 1200, 10, 45, 45, ItemUseStyleID.Shoot, ProjectileID.Bullet, 20, false, AmmoID.Bullet);
		Item.reuseDelay = 60;
		Item.rare = ItemRarityID.Red;
		Item.UseSound = SoundID.Item38 with { Pitch = -1 };
		Item.scale = .86f;
		Item.crit = 50;
		Item.ArmorPenetration = 100;
		Item.Set_ItemCriticalDamage(2f);
	}
	public override Vector2? HoldoutOffset() {
		return new Vector2(-30, -7);
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (player.GetModPlayer<LegacySniperRiflePlayer>().Counter >= 120 + player.itemAnimationMax) {
			float velocityRotate = velocity.ToRotation();
			float rotation = MathHelper.ToRadians(20);
			Vector2 newPos = position.PositionOFFSET(velocity, 80);
			for (int i = 0; i < 500; i++) {
				Dust dust = Dust.NewDustDirect(newPos, 0, 0, DustID.GemDiamond);
				dust.noGravity = true;
				if (i % 2 == 0) {
					dust.velocity = -velocity.RotatedBy(rotation).SafeNormalize(Vector2.Zero) * 29 + Main.rand.NextVector2Circular(20, 1).RotatedBy(rotation + velocityRotate) * Main.rand.NextFloat(1, 1.3f);
					dust.scale = Main.rand.NextFloat(.9f, 1.35f);
					dust.fadeIn = 1.5f;
				}
				else {
					dust.velocity = -velocity.RotatedBy(-rotation).SafeNormalize(Vector2.Zero) * 29 + Main.rand.NextVector2Circular(20, 1).RotatedBy(-rotation + velocityRotate) * Main.rand.NextFloat(1, 1.3f);
					dust.scale = Main.rand.NextFloat(.9f, 1.35f);
					dust.fadeIn = 1.5f;
				}
			}
			rotation = MathHelper.ToRadians(10);
			for (int i = 0; i < 300; i++) {
				Dust dust = Dust.NewDustDirect(newPos, 0, 0, DustID.GemDiamond);
				dust.noGravity = true;
				if (i % 2 == 0) {
					dust.velocity = -velocity.RotatedBy(rotation).SafeNormalize(Vector2.Zero) * 21 + Main.rand.NextVector2Circular(20, 1).RotatedBy(rotation + velocityRotate) * Main.rand.NextFloat(1, 1.3f);
					dust.scale = Main.rand.NextFloat(.9f, 1.35f);
					dust.fadeIn = 1.5f;
				}
				else {
					dust.velocity = -velocity.RotatedBy(-rotation).SafeNormalize(Vector2.Zero) * 21 + Main.rand.NextVector2Circular(20, 1).RotatedBy(-rotation + velocityRotate) * Main.rand.NextFloat(1, 1.3f);
					dust.scale = Main.rand.NextFloat(.9f, 1.35f);
					dust.fadeIn = 1.5f;
				}
			}
			rotation = MathHelper.ToRadians(30);
			for (int i = 0; i < 150; i++) {
				Dust dust = Dust.NewDustDirect(newPos, 0, 0, DustID.GemDiamond);
				dust.noGravity = true;
				if (i % 2 == 0) {
					dust.velocity = -velocity.RotatedBy(rotation).SafeNormalize(Vector2.Zero) * 15 + Main.rand.NextVector2Circular(14, 1).RotatedBy(rotation + velocityRotate) * Main.rand.NextFloat(1, 1.3f);
					dust.scale = Main.rand.NextFloat(.9f, 1.35f);
					dust.fadeIn = 1.5f;
				}
				else {
					dust.velocity = -velocity.RotatedBy(-rotation).SafeNormalize(Vector2.Zero) * 15 + Main.rand.NextVector2Circular(14, 1).RotatedBy(-rotation + velocityRotate) * Main.rand.NextFloat(1, 1.3f);
					dust.scale = Main.rand.NextFloat(.9f, 1.35f);
					dust.fadeIn = 1.5f;
				}
			}
			for (int i = 0; i < 200; i++) {
				if (i < 60) {
					int dust = Dust.NewDust(position.PositionOFFSET(velocity, 10), 0, 0, DustID.GemDiamond);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].position += Main.rand.NextVector2CircularEdge(5, 3.5f).RotatedBy(velocity.ToRotation() + MathHelper.PiOver2) * 2;
					Main.dust[dust].velocity = velocity * .4f;
					Main.dust[dust].fadeIn = 1.2f;
				}
				if (i < 100) {
					int dust1 = Dust.NewDust(position, 0, 0, DustID.GemDiamond);
					Main.dust[dust1].noGravity = true;
					Main.dust[dust1].position += Main.rand.NextVector2CircularEdge(12.5f, 4.5f).RotatedBy(velocity.ToRotation() + MathHelper.PiOver2) * 2;
					Main.dust[dust1].velocity = velocity * .35f;
					Main.dust[dust1].fadeIn = 1.5f;
					int dust2 = Dust.NewDust(position.PositionOFFSET(velocity, -5), 0, 0, DustID.GemDiamond);
					Main.dust[dust2].noGravity = true;
					Main.dust[dust2].position += Main.rand.NextVector2CircularEdge(20, 5.5f).RotatedBy(velocity.ToRotation() + MathHelper.PiOver2) * 2;
					Main.dust[dust2].velocity = velocity * .2f;
					Main.dust[dust2].fadeIn = 1.5f;
				}
				Vector2 rotate = Main.rand.NextVector2CircularEdge(10, 3.5f).RotatedBy(velocity.ToRotation() + MathHelper.PiOver2);
				int dust3 = Dust.NewDust(position.PositionOFFSET(velocity, -10), 0, 0, DustID.GemDiamond);
				Main.dust[dust3].noGravity = true;
				Main.dust[dust3].velocity = rotate;
				Main.dust[dust3].fadeIn = 1.6f;
			}
		}
		Vector2 offset = HoldoutOffset() ?? new Vector2(-30, -7);
		Projectile proj = Projectile.NewProjectileDirect(source, position + (offset * player.direction).RotatedBy(velocity.ToRotation()), velocity, ProjectileID.BulletHighVelocity, damage, knockback, player.whoAmI);
		proj.extraUpdates = 10;
		proj.penetrate = -1;
		proj.maxPenetrate = -1;
		return false;
	}
}
public class LegacySniperRiflePlayer : ModPlayer {
	public int Counter = 0;
	public bool DoACircleExplosionForFunni = true;
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		if (Player.IsHeldingModItem<SniperRifle>()) {
			if (Player.ItemAnimationActive && Player.ItemAnimationEndingOrEnded) {
				DoACircleExplosionForFunni = true;
				Counter = 0;
			}
			if (++Counter >= 120 + Player.itemAnimationMax) {
				if (DoACircleExplosionForFunni) {
					for (int i = 0; i < 400; i++) {
						int dust = Dust.NewDust(Player.position.Add(0, 60), 0, 0, DustID.GemEmerald, 0, 0, 0, Color.Green, Main.rand.NextFloat(1.5f, 1.72f));
						Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(17, 17);
						Main.dust[dust].noGravity = true;
					}
					for (int i = 0; i < 300; i++) {
						int dust = Dust.NewDust(Player.position.Add(0, 60), 0, 0, DustID.GemEmerald, 0, 0, 0, Color.Green, Main.rand.NextFloat(1.5f, 1.72f));
						Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(15, 15);
						Main.dust[dust].noGravity = true;
					}
					for (int i = 0; i < 200; i++) {
						int dust = Dust.NewDust(Player.position.Add(0, 60), 0, 0, DustID.GemEmerald, 0, 0, 0, Color.Green, Main.rand.NextFloat(1.5f, 1.72f));
						Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(10, 10);
						Main.dust[dust].noGravity = true;
					}
					DoACircleExplosionForFunni = false;
				}
				for (int i = 0; i < 4; i++) {
					Vector2 Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i)) * (3 + Main.rand.NextFloat()) * 5;
					for (int l = 0; l < 8; l++) {
						float multiplier = Main.rand.NextFloat();
						float scale = MathHelper.Lerp(1.1f, .1f, multiplier);
						int dust = Dust.NewDust(Player.Center.Add(0, -60), 0, 0, DustID.GemEmerald, 0, 0, 0, Color.Green, scale);
						Main.dust[dust].velocity = Toward * multiplier;
						Main.dust[dust].noGravity = true;
						Main.dust[dust].Dust_GetDust().FollowEntity = true;
						Main.dust[dust].Dust_BelongTo(Player);
					}
				}
			}
		}
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		if (Counter >= 120 + Player.itemAnimationMax && item.type == ModContent.ItemType<SniperRifle>()) {
			damage *= 2;
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (Counter >= 120 + Player.itemAnimationMax && proj.Check_ItemTypeSource(ModContent.ItemType<SniperRifle>())) {
			if (Main.rand.NextBool()) {
				target.StrikeInstantKill();
			}
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Counter >= 120 + Player.itemAnimationMax && proj.Check_ItemTypeSource(ModContent.ItemType<SniperRifle>())) {
			target.Center.LookForHostileNPC(out List<NPC> npclist, 500);
			if (npclist.Count < 1) {
				return;
			}
			foreach (var npc in npclist) {
				int direction = Player.Center.X - npc.Center.X > 0 ? -1 : 1;
				npc.StrikeNPC(npc.CalculateHitInfo((int)(hit.Damage * .75f), direction, false, 10));
				npc.AddBuff<CompletelyShatter>(BossRushUtils.ToSecond(20));
			}
			for (int i = 0; i < 150; i++) {
				int smokedust = Dust.NewDust(target.Center, 0, 0, DustID.Smoke);
				Main.dust[smokedust].noGravity = true;
				Main.dust[smokedust].velocity = Main.rand.NextVector2Circular(500 / 12f, 500 / 12f);
				Main.dust[smokedust].scale = Main.rand.NextFloat(.75f, 2f);
				int dust = Dust.NewDust(target.Center, 0, 0, DustID.Torch);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(500 / 12f, 500 / 12f);
				Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
			}
		}
	}
}
public class CompletelyShatter : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().StatDefense *= 0;
	}
	public override void Update(Player player, ref int buffIndex) {
		player.ModPlayerStats().UpdateDefenseBase *= 0;
	}
}
