using BossRush.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ItemOverhaul;
public class Roguelike_Starfury : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return UniversalSystem.Check_RLOH();
	}
	public override void SetDefaults(Item entity) {
		if (entity.type == ItemID.Starfury) {
			entity.damage += 5;
		}
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (item.type == ItemID.Starfury) {
			BossRushUtils.AddTooltip(ref tooltips, new(Mod, "Roguelike_TinBow", BossRushUtils.LocalizationText("RoguelikeRework", item.Name)));
		}
	}
	public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (item.type != ItemID.Starfury) {
			return;
		}
		int counter = player.GetModPlayer<Roguelike_Starfury_ModPlayer>().Starfury_Counter;
		if(player.GetModPlayer<Roguelike_Starfury_ModPlayer>().PerfectStrike) {
			counter = 300;
		}
		if (counter >= 150) {
			type = ModContent.ProjectileType<Roguelike_Starfury_Projectile>();
			damage += counter / 5;
			damage *= 5;
		}
		player.GetModPlayer<Roguelike_Starfury_ModPlayer>().Starfury_Counter = -player.itemAnimationMax;
	}
}
public class Roguelike_Starfury_ModPlayer : ModPlayer {
	public int Starfury_Counter = 0;
	public bool PerfectStrike = false;
	public override void ResetEffects() {
		if (!UniversalSystem.Check_RLOH()) {
			return;
		}
		if (++Starfury_Counter >= 300) {
			Starfury_Counter = 300;
		}
		if (Player.HeldItem.type != ItemID.Starfury) {
			return;
		}
		PerfectStrike = Starfury_Counter >= 150 && Starfury_Counter <= 165;
		if (PerfectStrike && Starfury_Counter == 150) {
			SpawnStarEffect();
		}
	}
	private void SpawnStarEffect() {
		//Taken from vanilla code :>>
		Vector2 center3 = Player.Center - Vector2.UnitY * 150;
		Color celeb2Color3 = new Color(255, 255, 0, 0);
		float num9 = .1f;
		float num10 = .1f;

		float num11 = Main.rand.NextFloatDirection();
		for (float num12 = 0f; num12 < 5f; num12 += 1f) {
			Vector2 spinningpoint = new Vector2(0f, -100f);
			Vector2 vector4 = center3 + spinningpoint.RotatedBy(num11 + num12 * ((float)Math.PI * 2f / 5f));
			Vector2 vector5 = center3 + spinningpoint.RotatedBy(num11 + (num12 + 1f) * ((float)Math.PI * 2f / 5f));
			Vector2 vector6 = center3 + spinningpoint.RotatedBy(num11 + (num12 + 0.5f) * ((float)Math.PI * 2f / 5f)) * 0.4f;
			for (int num13 = 0; num13 < 2; num13++) {
				Vector2 value = vector4;
				Vector2 value2 = vector6;
				if (num13 == 1) {
					value = vector6;
					value2 = vector5;
				}

				for (float num14 = 0f; num14 < 1f; num14 += num10) {
					Vector2 vector7 = Vector2.Lerp(value, value2, num14);
					Vector2 vector8 = Vector2.Lerp(vector7, center3, 0.9f);
					Vector2 vector9 = (vector7 - vector8).SafeNormalize(Vector2.Zero);
					Dust dust5 = Dust.NewDustPerfect(vector8, 267, Vector2.Zero, 0, celeb2Color3, 0.5f);
					dust5.fadeIn = 1.2f;
					dust5.noGravity = true;
					dust5.velocity = vector9 * Vector2.Distance(vector7, vector8) * num9;
				}
			}
		}
	}
}
public class Roguelike_Starfury_Projectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.FallenStar);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.friendly = true;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 9999;
		Projectile.tileCollide = false;
		Projectile.extraUpdates = 10;
	}
	Vector2 toMouse = Vector2.Zero;
	public override void AI() {
		if (Projectile.timeLeft == 9999) {
			toMouse = Main.MouseWorld;
		}
		if (Main.rand.NextBool(5)) {
			Dust dust = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(22, 22), 0, 0, DustID.Enchanted_Pink);
			dust.noGravity = true;
			dust.scale = Main.rand.NextFloat(.8f, 1.2f);
			dust.velocity = Vector2.Zero;
		}
		if (Main.rand.NextBool(15)) {
			Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromAI(), Projectile.Center + Main.rand.NextVector2Circular(22, 22), Vector2.Zero, 16 + Main.rand.NextBool().ToInt());
			gore.rotation += MathHelper.ToRadians(Main.rand.Next(1, 45));
			gore.scale += Main.rand.NextFloat() * .5f;
		}
		Projectile.rotation += MathHelper.ToRadians(5) * -Projectile.direction;
		Projectile.velocity = (toMouse - Projectile.Center).SafeNormalize(Vector2.Zero) * 2f;
		if (++Projectile.frameCounter >= 8) {
			Projectile.frameCounter = 0;
			Projectile.frame = BossRushUtils.Safe_SwitchValue(Projectile.frame, 7);
		}
		if (Projectile.Center.IsCloseToPosition(toMouse, 20)) {
			Projectile.Kill();
		}
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 18; i++) {
			Projectile projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One.Vector2DistributeEvenlyPlus(18, 360, i) * 15, ProjectileID.Starfury, Math.Max(Projectile.damage / 4, 1), 3f, Projectile.owner);
			projectile.timeLeft = 300;
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
		origin = new Vector2(11, 11);

		Texture2D value28 = TextureAssets.Extra[91].Value;
		Rectangle value29 = value28.Frame();
		Vector2 vector43 = Projectile.Center + Projectile.velocity;
		Vector2 spinningpoint2 = new Vector2(0f, -15f);
		float num195 = Projectile.timeLeft;
		float num196 = 1.5f;
		float num197 = 1.1f;
		float num198 = 1.3f;
		Color color = Color.Purple;
		color.A = 0;
		Main.EntitySpriteDraw(value28, vector43 - Main.screenPosition + spinningpoint2.RotatedBy(MathHelper.ToRadians(num195)), value29, color, Projectile.velocity.ToRotation() + (float)Math.PI / 2f, origin, num196, SpriteEffects.None);
		Main.EntitySpriteDraw(value28, vector43 - Main.screenPosition + spinningpoint2.RotatedBy(MathHelper.ToRadians(num195 + 90)), value29, color, Projectile.velocity.ToRotation() + (float)Math.PI / 2f, origin, num197, SpriteEffects.None);

		Vector2 drawpos = Projectile.position - Main.screenPosition + origin;
		Main.EntitySpriteDraw(texture, drawpos, texture.Frame(1, 8, frameY: Projectile.frame), lightColor, Projectile.rotation, origin, 2f, SpriteEffects.None);

		Main.EntitySpriteDraw(value28, vector43 - Main.screenPosition + spinningpoint2.RotatedBy(MathHelper.ToRadians(num195 + 180)), value29, color, Projectile.velocity.ToRotation() + (float)Math.PI / 2f, origin, num198, SpriteEffects.None);
		return false;
	}
}
