﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Contents.Items.Accessories.SynergyAccessories.GuideToMasterNinja {
	internal class GuideToMasterNinja2 : SynergyModItem {
		public override void SetDefaults() {
			Item.accessory = true;
			Item.height = 24;
			Item.width = 32;
			Item.rare = ItemRarityID.Orange;
			Item.value = 10000000;
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			var player = Main.LocalPlayer;
			if (player.GetModPlayer<PlayerNinjaBook>().GuidetoMasterNinja2
				&& (player.HasItem(ItemID.Shuriken) || player.HasItem(ItemID.ThrowingKnife) || player.HasItem(ItemID.PoisonedKnife))) {
				tooltips.Add(new TooltipLine(Mod, "Weeb2", $"[i:{ItemID.Shuriken}][i:{ItemID.ThrowingKnife}][i:{ItemID.PoisonedKnife}] Increase shuriken damage flat by 5 and have a chance to spawn itself ontop of the enemy"));
				if (player.HasItem(ItemID.BoneDagger)) {
					tooltips.Add(new TooltipLine(Mod, "BoneDaggerGTMN2", $"[i:{ItemID.BoneDagger}] Shuriken gain 5 flat damage increase"));
				}
				if (player.HasItem(ItemID.FrostDaggerfish)) {
					tooltips.Add(new TooltipLine(Mod, "BoneDaggerGTMN2", $"[i:{ItemID.FrostDaggerfish}] Shuriken gain 5 flat damage increase"));
				}
			}
			if (player.HasItem(ItemID.Katana)) {
				tooltips.Add(new TooltipLine(Mod, "KatanaPower", $"[i:{ItemID.Katana}] Increase sword size and damage by 50% and melee speed by 35%"));
			}
			if (player.GetModPlayer<PlayerNinjaBook>().GuidetoMasterNinja) {
				tooltips.Add(new TooltipLine(Mod, "FinalMaster", $"[i:{ModContent.ItemType<GuideToMasterNinja>()}] You summon a ring of shuriken and knife"));
			}
		}
		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<PlayerNinjaBook>().GuidetoMasterNinja2 = true;
			player.jumpSpeedBoost += .15f;
			player.GetAttackSpeed(DamageClass.Melee) += .1f;
			player.GetCritChance(DamageClass.Generic) += .05f;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.ThrowingKnife)
				.AddIngredient(ItemID.Gi)
				.Register();
		}
	}

	public class ShurikenCustom : ModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Shuriken);
		public override void SetDefaults() {
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
		}
		public void Behavior(Player player, float offSet, int Counter, float Distance = 100) {
			var Rotate = new Vector2(1, 1).RotatedBy(MathHelper.ToRadians(offSet));
			var NewCenter = player.Center + Rotate.RotatedBy(Counter * 0.1f) * Distance;
			Projectile.Center = NewCenter;
			if (Counter == 0) {
				for (int i = 0; i < 12; i++) {
					var randomSpeed = Main.rand.NextVector2Circular(1, 1);
					Dust.NewDust(NewCenter, 0, 0, DustID.Smoke, randomSpeed.X, randomSpeed.Y, 0, default, Main.rand.NextFloat(2f, 2.5f));
				}
			}
		}
		int Counter = 0;
		int Multiplier = 1;
		//What the fuck is this disgusting AI that I wrote back in the past
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			Projectile.timeLeft = 2;
			if (player.dead || !player.active) Projectile.Kill();
			if (Projectile.ai[0] == 0) {
				switch (Projectile.velocity.X) {
					case 1:
						Multiplier = 1;
						break;
					case 2:
						Multiplier = 2;
						break;
					case 3:
						Multiplier = 3;
						break;
					case 4:
						Multiplier = 4;
						break;
					case 5:
						Multiplier = 5;
						break;
				}
				Projectile.velocity = Vector2.Zero;
			}
			if (Projectile.ai[0] >= 60) Projectile.penetrate = 1;
			Projectile.ai[0]++;
			Behavior(player, 72 * Multiplier, Counter);
			if (Counter == MathHelper.TwoPi * 100 + 1) {
				Counter = 1;
			}
			Counter++;
		}
	}
}
