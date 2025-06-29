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
	public override void SetDefaults(Item entity) {
		if (entity.type == ItemID.Starfury) {
			entity.damage += 5;
		}
	}
	public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (item.type != ItemID.Starfury) {
			return;
		}
		int counter = player.GetModPlayer<Roguelike_Starfury_ModPlayer>().Starfury_Counter;
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
	public override void ResetEffects() {
		if (++Starfury_Counter >= 300) {
			Starfury_Counter = 300;
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
