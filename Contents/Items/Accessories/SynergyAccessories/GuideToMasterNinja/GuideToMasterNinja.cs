using BossRush.Common.Systems;
using BossRush.Contents.Items.Weapon;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
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
				tooltips.Add(new TooltipLine(Mod, "", $"[i:{ItemID.NinjaHood}][i:{ItemID.NinjaShirt}][i:{ItemID.NinjaPants}]Increase melee attack is faster by 15% and increase melee damage by 25%"));
			}
			string ThrowingKnife = $"[i:{ItemID.ThrowingKnife}]";
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
				tooltips.Add(new TooltipLine(Mod, "", ThrowingKnife + PoisonedKnife + FrostDaggerfish + BoneDagger + "You can throw even faster"));
			}
			if (HasThrowingKnife && HasPoisonedKnife) {
				tooltips.Add(new TooltipLine(Mod, "", ThrowingKnife + PoisonedKnife + " You will sometime throw 1 of 2 knife and have a chance to spawn itself ontop of the enemy, Increases damage by 10"));
			}
			else {
				if (HasThrowingKnife) {
					tooltips.Add(new TooltipLine(Mod, "", ThrowingKnife + "You will sometime throw throwing knife, damage +5"));
				}
				else if (HasPoisonedKnife) {
					tooltips.Add(new TooltipLine(Mod, "", PoisonedKnife + "You will sometime throw poisoned throwing knife, damage +5"));
				}
			}
			if (HasFrostDaggerFish) {
				tooltips.Add(new TooltipLine(Mod, "", FrostDaggerfish + "Attack now inflict FrostBurn and you sometime throw FrostDaggerFish, damage +5"));
			}
			if (HasBoneDagger) {
				tooltips.Add(new TooltipLine(Mod, "", BoneDagger + "Attack now inflict OnFire! and you sometime throw BoneDagger, damage +5"));
			}
			if (player.HasItem(ItemID.Katana)) {
				tooltips.Add(new TooltipLine(Mod, "KatanaPower", $"[i:{ItemID.Katana}] Increase sword size and damage by 50% and melee speed by 35%"));
			}
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<PlayerNinjaBook>().GuidetoMasterNinja = true;
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1.15f);
			modplayer.AddStatsToPlayer(PlayerStats.JumpBoost, 1.15f);
			modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 5);

			player.GetAttackSpeed(DamageClass.Melee) += .1f;
			if (player.GetModPlayer<PlayerNinjaBook>().NinjaWeeb) {
				player.GetAttackSpeed(DamageClass.Melee) += .15f;
				modplayer.AddStatsToPlayer(PlayerStats.MeleeDMG, 1.25f);
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
	internal class PlayerNinjaBook : ModPlayer {
		public bool GuidetoMasterNinja = false;
		public bool NinjaWeeb = false;
		//counter for accessory
		//GuidetoMasterNinja
		int GTMNcount = 0;
		int GTMNlimitCount = 15;
		int TimerForUltimate = 0;
		public override void ResetEffects() {
			GuidetoMasterNinja = false;
			if (Player.head == ArmorIDs.Head.NinjaHood && Player.body == ArmorIDs.Body.NinjaShirt && Player.legs == ArmorIDs.Legs.NinjaPants) {
				NinjaWeeb = true;
			}
			else {
				NinjaWeeb = false;
			}
		}
		public override void UpdateEquips() {
			if (!GuidetoMasterNinja) {
				return;
			}
			if (Player.ownedProjectileCounts[ModContent.ProjectileType<ThrowingKnifeCustom>()] > 0
				|| Player.ownedProjectileCounts[ModContent.ProjectileType<ThrowingKnifeCustom>()] > 0) {
				TimerForUltimate = 0;
				return;
			}
			if (++TimerForUltimate >= 60) {
				int damage = Player.GetWeaponDamage(Player.HeldItem);
				if (Player.ownedProjectileCounts[ModContent.ProjectileType<ThrowingKnifeCustom>()] < 1) {
					for (int i = 0; i < 8; i++)
						Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(1 + i, 1 + i), ModContent.ProjectileType<ThrowingKnifeCustom>(), damage + 10, 0, Player.whoAmI);
				}
				if (Player.ownedProjectileCounts[ModContent.ProjectileType<ShurikenCustom>()] < 1) {
					for (int i = 0; i < 5; i++)
						Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(1 + i, 1 + i), ModContent.ProjectileType<ShurikenCustom>(), damage + 10, 0, Player.whoAmI);
				}
			}
		}
		public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (GuidetoMasterNinja) {
				if (++GTMNcount >= GTMNlimitCount) {
					var GTMNcontain = new List<int>();
					int StaticDamage = 10;
					if (NinjaWeeb) {
						StaticDamage = (int)(StaticDamage * 1.2f);
					}
					if (Player.HasItem(ItemID.ThrowingKnife)
						&& Player.HasItem(ItemID.PoisonedKnife)
						&& Player.HasItem(ItemID.FrostDaggerfish)
						&& Player.HasItem(ItemID.BoneDagger)) {
						GTMNcount++;
					}
					GTMNcontain.Add(ProjectileID.Shuriken);
					if (Player.HasItem(ItemID.ThrowingKnife)) {
						StaticDamage += 5;
						GTMNcontain.Add(ProjectileID.ThrowingKnife);
					}
					if (Player.HasItem(ItemID.PoisonedKnife)) {
						GTMNcontain.Add(ProjectileID.PoisonedKnife);
						StaticDamage += 5;
					}
					if (Player.HasItem(ItemID.FrostDaggerfish)) {
						GTMNcontain.Add(ProjectileID.FrostDaggerfish);
						StaticDamage += 5;
					}
					if (Player.HasItem(ItemID.BoneDagger)) {
						GTMNcontain.Add(ProjectileID.BoneDagger);
						StaticDamage += 5;
					}
					var Aimto = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.UnitX) * 20;

					StaticDamage = (int)Player.GetTotalDamage(DamageClass.Generic).ApplyTo(StaticDamage);
					int proj1 = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Aimto, Main.rand.NextFromCollection(GTMNcontain), StaticDamage, 1f, Player.whoAmI);
					Main.projectile[proj1].penetrate = 1;
					GTMNcount = 0;
				}
			}
			return base.Shoot(item, source, position, velocity, type, damage, knockback);
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (!GuidetoMasterNinja) {
				return;
			}
			if (Player.HasItem(ItemID.FrostDaggerfish)) {
				target.AddBuff(BuffID.Frostburn, 150);
			}
			if (Player.HasItem(ItemID.BoneDagger)) {
				target.AddBuff(BuffID.OnFire, 150);
			}
			if ((Player.HasItem(ItemID.Shuriken) || Player.HasItem(ItemID.ThrowingKnife) || Player.HasItem(ItemID.PoisonedKnife)) && Main.rand.NextBool(10)) {
				var NinjaBag = new List<int>() { ProjectileID.Shuriken, ProjectileID.ThrowingKnife, ProjectileID.PoisonedKnife };
				if (Player.HasItem(ItemID.BoneDagger)) {
					NinjaBag.Add(ProjectileID.BoneDagger);
				}
				if (Player.HasItem(ItemID.FrostDaggerfish)) {
					NinjaBag.Add(ProjectileID.FrostDaggerfish);
				}
				Vector2 SpawnProjPos = target.Center.Add(0, 200);
				for (int i = 0; i < 12; i++) {
					var randomSpeed = Main.rand.NextVector2Circular(1, 1);
					Dust.NewDust(SpawnProjPos, 0, 0, DustID.Smoke, randomSpeed.X, randomSpeed.Y, 0, default, Main.rand.NextFloat(2f, 3.5f));
				}
				int proj1 = Projectile.NewProjectile(Player.GetSource_FromThis(), SpawnProjPos, Vector2.Zero, Main.rand.NextFromCollection(NinjaBag), proj.damage, proj.knockBack, Player.whoAmI);
				Main.projectile[proj1].penetrate = 1;
			}
		}
		public override void ModifyItemScale(Item item, ref float scale) {
			if (GuidetoMasterNinja && item.type == ItemID.Katana) {
				scale += .5f;
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (!GuidetoMasterNinja) {
				return;
			}
			if (item.type == ItemID.Shuriken
				|| item.type == ItemID.ThrowingKnife
				|| item.type == ItemID.PoisonedKnife
				|| item.type == ItemID.BoneDagger
				|| item.type == ItemID.FrostDaggerfish) {
				damage.Flat += 5f;
				if (Player.HasItem(ItemID.BoneDagger)) {
					damage.Flat += 5f;
				}
				if (Player.HasItem(ItemID.FrostDaggerfish)) {
					damage.Flat += 5f;
				}
			}
			if (item.type == ItemID.Katana) {
				damage += .5f;
			}
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
		public void Behavior(Player player, float offSet, float Counter, float Distance = 125) {
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
		public float Counter { get => Projectile.ai[2]; set => Projectile.ai[2] = value; }
		public float Multiplier { get => Projectile.ai[1]; set => Projectile.ai[1] = value; }
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
		public void Behavior(Player player, float offSet, float Counter, float Distance = 100) {
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
		public float Counter { get => Projectile.ai[2]; set => Projectile.ai[2] = value; }
		public float Multiplier { get => Projectile.ai[1]; set => Projectile.ai[1] = value; }
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
