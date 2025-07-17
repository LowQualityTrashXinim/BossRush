using BossRush.Texture;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ItemOverhaul;
public class Roguelike_TinBow : GlobalItem {
	public override void SetDefaults(Item entity) {
		if (entity.type != ItemID.TinBow) {
			return;
		}
		entity.damage += 5;
		entity.useTime = entity.useAnimation = 22;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (item.type == ItemID.TinBow) {
			BossRushUtils.AddTooltip(ref tooltips, new(Mod, "Roguelike_TinBow", BossRushUtils.LocalizationText("RoguelikeRework", item.Name)));
		}
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (item.type != ItemID.TinBow) {
			return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
		}
		int counter = player.GetModPlayer<Roguelike_TinBow_ModPlayer>().Counter;
		player.GetModPlayer<Roguelike_TinBow_ModPlayer>().Counter = -22;
		if (counter >= 120) {
			int amount = 3;
			bool randomizeYAxis = false;
			if (counter >= 240) {
				amount = 12;
				randomizeYAxis = true;
			}
			for (int i = 0; i < amount; i++) {
				Vector2 pos = position.Add(Main.rand.Next(-300, 300), 1000);
				if (randomizeYAxis) {
					pos.Y -= Main.rand.Next(0, 1000);
				}
				Projectile.NewProjectile(source, pos, (Main.MouseWorld + Main.rand.NextVector2Circular(50, 50) - pos).SafeNormalize(Vector2.Zero) * 2.5f, ModContent.ProjectileType<TinBolt>(), damage * 2, knockback, player.whoAmI);
				if (Main.rand.NextBool(3)) {
					pos = position.Add(Main.rand.Next(-300, 300), 1000);
					pos.Y -= Main.rand.Next(0, 200);
					Projectile.NewProjectile(source, pos, (Main.MouseWorld + Main.rand.NextVector2Circular(50, 50) - pos).SafeNormalize(Vector2.Zero) * 5, ModContent.ProjectileType<TinOreMeteor>(), (int)(damage * 2.5f), knockback, player.whoAmI);
				}
			}
		}
		Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
		Projectile.NewProjectile(source, position, velocity.SafeNormalize(Vector2.Zero).Vector2RotateByRandom(15) * 5, ModContent.ProjectileType<TinOreMeteor>(), damage, knockback, player.whoAmI);
		proj.extraUpdates = 1;
		return false;
	}
}
public class Roguelike_TinBow_ModPlayer : ModPlayer {
	public int Counter = 0;
	public override void ResetEffects() {
		if (++Counter >= 240) {
			Counter = 240;
		}
	}
}
public class TinBolt : ModProjectile {
	public override string Texture => BossRushTexture.WHITEDOT;
	public Vector2 initialMousePos = Vector2.Zero;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 100;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 2;
		Projectile.scale = 1.5f;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 9999;
		Projectile.extraUpdates = 25;
		Projectile.light = 1;
	}
	public override void AI() {
		if (Projectile.timeLeft == 9999) {
			initialMousePos = Main.MouseWorld;
		}
		if (Projectile.Center.Y >= initialMousePos.Y) {
			Projectile.tileCollide = true;
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrail(lightColor, .01f);
		return false;
	}
}
public class TinOreMeteor : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.TinOre);
	public Vector2 initialMousePos = Vector2.Zero;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 16;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 9999;
		Projectile.light = 1;
		Projectile.extraUpdates = 3;
	}
	public override void AI() {
		if (Projectile.timeLeft == 9999) {
			initialMousePos = Main.MouseWorld;
		}
		if (Projectile.Center.Y >= initialMousePos.Y) {
			Projectile.tileCollide = true;
		}
		Projectile.ai[1] = BossRushUtils.CountDown((int)Projectile.ai[1]);
		Projectile.rotation += MathHelper.ToRadians(20) * (Projectile.velocity.X > 0 ? 1 : -1);
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 25; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Tin);
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2Circular(5, 5);
		}
	}
}
