using System;
using Terraria;
using System.Text;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.Graphics;
using System.Collections.Generic;
using BossRush.Contents.Projectiles;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ItemOverhaul;

public class Roguelike_CobaltSword : GlobalItem {
	public override void SetDefaults(Item entity) {
		if (entity.type == ItemID.CobaltSword) {
			entity.shoot = ModContent.ProjectileType<SimplePiercingProjectile2>();
			entity.shootSpeed = 1;
			entity.damage += 20;
		}
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (item.type == ItemID.CobaltSword) {
			tooltips.Add(new(Mod, $"RoguelikeOverhaul_{item.Name}", BossRushUtils.LocalizationText("RoguelikeRework", item.Name)));
		}
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (item.type != ItemID.CobaltSword) {
			return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
		}
		int counter = player.GetModPlayer<Roguelike_CobaltSword_ModPlayer>().CobaltSword_Counter;
		if (counter >= 150) {
			for (int i = 0; i < 16; i++) {
				Vector2 velocityToward = velocity.RotatedBy(MathHelper.PiOver2).Vector2RotateByRandom(55) * Main.rand.NextBool().ToDirectionInt();
				Projectile Swordprojectile = Projectile.NewProjectileDirect(source, position + velocity * item.Size.Length() * (i * .25f), velocityToward, ModContent.ProjectileType<SimplePiercingProjectile2>(), (int)(damage * .85f + counter - 150), 2f, player.whoAmI, 2f + Main.rand.NextFloat(2), 5 + i, 3 + i * .5f);
				if (Swordprojectile.ModProjectile is SimplePiercingProjectile2 modproj) {
					modproj.ProjectileColor = SwordSlashTrail.averageColorByID[ItemID.CobaltSword] * 2;
					Swordprojectile.scale += .2f;
				}
			}
			return false;
		}
		for (int i = 0; i < 2; i++) {
			Vector2 velocityToward = velocity.RotatedBy(MathHelper.PiOver2 * Main.rand.NextBool().ToDirectionInt()).Vector2RotateByRandom(55);
			Projectile Swordprojectile = Projectile.NewProjectileDirect(source, position + velocity * item.Size.Length() * Main.rand.NextFloat(.4f, 1.2f), velocityToward, ModContent.ProjectileType<SimplePiercingProjectile2>(), (int)(damage * .85f), 2f, player.whoAmI, 2f + Main.rand.NextFloat(2));
			if (Swordprojectile.ModProjectile is SimplePiercingProjectile2 modproj) {
				modproj.ProjectileColor = SwordSlashTrail.averageColorByID[ItemID.CobaltSword] * 2;
			}
		}
		return false;
	}
}
public class Roguelike_CobaltSword_ModPlayer : ModPlayer {
	public int CobaltSword_Counter = 0;
	public bool CobaltSword_CounterSurpass = false;
	public override void ResetEffects() {
		Item item = Player.HeldItem;
		CobaltSword_Counter++;
		if(CobaltSword_Counter > 300) {
			CobaltSword_Counter = 300;
		}
		if (item.type != ItemID.CobaltSword) {
			return;
		}
		if (!Player.ItemAnimationActive) {
			if (CobaltSword_Counter >= 150 && !CobaltSword_CounterSurpass) {
				CobaltSword_CounterSurpass = true;
				SpawnSpecialCobaltDustEffect();
			}
		}
		else {
			CobaltSword_Counter = 0;
			CobaltSword_CounterSurpass = false;
		}
	}
	public void SpawnSpecialCobaltDustEffect() {
		for (int o = 0; o < 10; o++) {
			for (int i = 0; i < 4; i++) {
				var Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i)) * (3 + Main.rand.NextFloat()) * 5;
				for (int l = 0; l < 8; l++) {
					float multiplier = Main.rand.NextFloat();
					float scale = MathHelper.Lerp(1.1f, .1f, multiplier);
					int dust = Dust.NewDust(Player.Center.Add(0, -60), 0, 0, DustID.GemDiamond, 0, 0, 0, Color.Blue, scale);
					Main.dust[dust].velocity = Toward * multiplier;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].Dust_GetDust().FollowEntity = true;
					Main.dust[dust].Dust_BelongTo(Player);
				}
			}
		}
	}
}
