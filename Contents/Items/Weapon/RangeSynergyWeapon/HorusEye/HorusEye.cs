using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.HorusEye;
/// <summary>
///  Uwaaa blue archive weapon but not really
/// </summary>
internal class HorusEye : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(45, 120, 23, 7f, 12, 12, ItemUseStyleID.Shoot, ProjectileID.Bullet, 6f, false, AmmoID.Bullet);
		Item.crit = 12;
		Item.reuseDelay = 8;
		Item.rare = ItemRarityID.Orange;
		Item.value = Item.buyPrice(gold: 50);
		Item.scale = 0.7f;
		Item.UseSound = SoundID.Item38 with {
			Pitch = -.7f,
			PitchVariance = .2f
		};
		Item.scale = .7f;
	}
	public override Vector2? HoldoutOffset() {
		return new Vector2(-33, 5f);
	}
	public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
		if (modplayer.HorusEye_ResonanceScepter) {
			tooltips.Add(new(Mod, "HorusEye_ResonanceScepter", $"[i:{ItemID.PrincessWeapon}] Shoot out a powerful bolt that will knock you back slightly"));
		}
	}
	public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
		if(player.HasItem(ItemID.PrincessWeapon)) {
			modplayer.HorusEye_ResonanceScepter = true;
			modplayer.SynergyBonus++;
		}
	}
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		position = position.PositionOFFSET(velocity, 70);
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		player.GetModPlayer<HorusEyePlayer>().ShieldCharge = Math.Clamp(player.GetModPlayer<HorusEyePlayer>().ShieldCharge + 1, 0, 10);
		for (int i = 0; i < 30; i++) {
			int dust = Dust.NewDust(position, 0, 0, DustID.Firework_Pink);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(1.5f, 10f).RotatedBy(velocity.ToRotation());
			Main.dust[dust].scale = Main.rand.NextFloat(.9f, 1.5f);
			int dust1 = Dust.NewDust(position, 0, 0, DustID.Firework_Pink);
			Main.dust[dust1].noGravity = true;
			Main.dust[dust1].velocity = Main.rand.NextVector2Unit(-MathHelper.PiOver4 * .5f, MathHelper.PiOver4).RotatedBy(velocity.ToRotation()) * Main.rand.NextFloat(7f, 19f);
			Main.dust[dust1].scale = Main.rand.NextFloat(.9f, 1.5f);
		}
		CanShootItem = false;
		for (int i = 0; i < 10; i++) {
			Projectile.NewProjectile(source, position, Main.rand.NextVector2Unit(-MathHelper.PiOver4 * .5f, MathHelper.PiOver4).RotatedBy(velocity.ToRotation()) * Main.rand.NextFloat(7f, 21f), type, damage, knockback, player.whoAmI);
		}
		if (modplayer.HorusEye_ResonanceScepter) {
			player.velocity += -velocity * .35f;
			Projectile.NewProjectile(source, position, velocity * .8f, ModContent.ProjectileType<HorusEye_Projectile>(), damage, knockback, player.whoAmI);
		}
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Shotgun)
			.AddIngredient(ItemID.BouncingShield)
			.Register();
	}
}
public class HorusEyePlayer : ModPlayer {
	public int ShieldCharge = 0;
	public override void ResetEffects() {
		if(Player.HeldItem.type != ModContent.ItemType<HorusEye>()) {
			ShieldCharge = 0;
		}
	}
	public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo) {
		if (ShieldCharge >= 10) {
			BossRushUtils.BresenhamCircle(Player.Center, 30, Color.LightPink);
			for (int i = 0; i < 2; i++) {
				int dust = Dust.NewDust(Player.Center + Main.rand.NextVector2CircularEdge(30, 30) + -Vector2.One * 3, 0, 0, DustID.Firework_Pink);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Vector2.Zero;
				Main.dust[dust].scale = .5f;
			}
		}
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (ShieldCharge >= 10) {
			modifiers.SetMaxDamage(1);
			modifiers.Knockback *= 0;
			ShieldCharge = 0;
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (ShieldCharge >= 10) {
			modifiers.SetMaxDamage(1);
			modifiers.Knockback *= 0;
			ShieldCharge = 0;
		}
	}
}
class HorusEye_Projectile : SynergyModProjectile {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 20;
		Projectile.penetrate = 1;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.hide = true;
		Projectile.extraUpdates = 10;
		Projectile.timeLeft = BossRushUtils.ToSecond(10);
	}
	public override void SynergyAI(Player player, PlayerSynergyItemHandle modplayer) {
		base.SynergyAI(player, modplayer);
		for (int i = 0; i < 3; i++) {
			int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.GemDiamond);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Vector2.Zero;
			Main.dust[dust].color = new Color(255, 0, 100);
		}
	}
	public override void SynergyKill(Player player, PlayerSynergyItemHandle modplayer, int timeLeft) {
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ProjectileID.PrincessWeapon, Projectile.damage, Projectile.knockBack, Projectile.owner);
	}
}
