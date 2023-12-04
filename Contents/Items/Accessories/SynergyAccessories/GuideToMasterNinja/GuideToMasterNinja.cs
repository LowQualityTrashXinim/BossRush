using BossRush.Contents.Items.Weapon;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.SynergyAccessories.GuideToMasterNinja {
	internal class GuideToMasterNinja : SynergyModItem {
		public override void SetDefaults() {
			Item.accessory = true;
			Item.height = 24;
			Item.width = 32;
			Item.rare = ItemRarityID.Orange;
			Item.value = 10000000;
		}
		//actual wtf happen here, I don't recall doing this
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			var player = Main.LocalPlayer;
			if (player.GetModPlayer<PlayerNinjaBook>().NinjaWeeb) {
				tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.NinjaHood}][i:{ItemID.NinjaShirt}][i:{ItemID.NinjaPants}]Increase thrown damage by 20%, Melee attack is faster by 15% and increase melee damage by 25%"));
			}
			string ThrowingKnife = $"[i:{ ItemID.ThrowingKnife}]";
			string PoisonedKnife = $"[i:{ItemID.PoisonedKnife}]";
			string FrostDaggerfish = $"[i:{ItemID.FrostDaggerfish}]";
			string BoneDagger = $"[i:{ItemID.BoneDagger}]";
			bool HasThrowingKnife = player.HasItem(ItemID.ThrowingKnife);
			bool HasPoisonedKnife = player.HasItem(ItemID.PoisonedKnife);
			bool HasFrostDaggerFish = player.HasItem(ItemID.FrostDaggerfish);
			bool HasBoneDagger = player.HasItem(ItemID.BoneDagger);
			if (HasThrowingKnife
				&& HasPoisonedKnife
				&& HasFrostDaggerFish
				&& HasBoneDagger) {
				tooltips.Add(new TooltipLine(Mod, "",ThrowingKnife +  PoisonedKnife + FrostDaggerfish + BoneDagger + "You can throw even faster"));
			}
			if (HasThrowingKnife && HasPoisonedKnife) {
				tooltips.Add(new TooltipLine(Mod, "", ThrowingKnife + PoisonedKnife + " You will sometime throw 1 of 2 knife, Increases damage by 10"));
			}
			else {
				if (HasThrowingKnife) {
					tooltips.Add(new TooltipLine(Mod, "", ThrowingKnife + "You will sometime throw throwing knife, fixed damage +5"));
				}
				else if (HasPoisonedKnife) {
					tooltips.Add(new TooltipLine(Mod, "", PoisonedKnife + "You will sometime throw poisoned throwing knife, fixed damage +5"));
				}
			}
			if (HasFrostDaggerFish) {
				tooltips.Add(new TooltipLine(Mod, "",FrostDaggerfish + "Attack now inflict FrostBurn and you sometime throw FrostDaggerFish, fixed damage +5"));
			}
			if (HasBoneDagger) {
				tooltips.Add(new TooltipLine(Mod, "", BoneDagger + "Attack now inflict OnFire! and you sometime throw BoneDagger , fixed damage +5"));
			}
			if (player.GetModPlayer<PlayerNinjaBook>().GuidetoMasterNinja) {
				tooltips.Add(new TooltipLine(Mod, "FinalMaster", $"[i:{ModContent.ItemType<GuideToMasterNinja2>()}] You summon a ring of shuriken and knife"));
			}
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<PlayerNinjaBook>().GuidetoMasterNinja = true;
			player.moveSpeed += .15f;
			player.GetCritChance(DamageClass.Generic) += 5;
			if (player.head == 22 && player.body == 14 && player.legs == 14) {
				player.GetAttackSpeed(DamageClass.Melee) += .15f;
				player.GetDamage(DamageClass.Melee) += .25f;
			}
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Shuriken)
				.AddIngredient(ItemID.NinjaHood)
				.AddIngredient(ItemID.NinjaShirt)
				.AddIngredient(ItemID.NinjaPants)
				.Register();
		}
	}
	public class ThrowingKnifeCustom : ModProjectile {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.ThrowingKnife);
		public override void SetDefaults() {
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
		}
		public void Behavior(Player player, float offSet, int Counter, float Distance = 125) {
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
					case 6:
						Multiplier = 6;
						break;
					case 7:
						Multiplier = 7;
						break;
					case 8:
						Multiplier = 8;
						break;
				}
				Projectile.velocity = Vector2.Zero;
			}
			if (Projectile.ai[0] >= 60) Projectile.penetrate = 1;
			Projectile.ai[0]++;
			Behavior(player, 45 * Multiplier, Counter);
			if (Counter == -MathHelper.TwoPi * 100 - 1) {
				Counter = -1;
			}
			Counter--;
		}
	}
}
