﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BossRush.Contents.Items.Accessories.SynergyAccessories.GuideToMasterNinja {
	internal class PlayerNinjaBook : ModPlayer {
		public bool GuidetoMasterNinja = false;
		public bool GuidetoMasterNinja2 = false;
		public bool NinjaWeeb = false;
		//counter for accessory
		//GuidetoMasterNinja
		int GTMNcount = 0;
		int GTMNlimitCount = 15;
		int TimerForUltimate = 0;
		public override void ResetEffects() {
			GuidetoMasterNinja = false;
			GuidetoMasterNinja2 = false;
			NinjaWeeb = false;
		}
		public override void UpdateEquips() {
			if (Player.head == ArmorIDs.Head.NinjaHood && Player.body == ArmorIDs.Body.NinjaShirt && Player.legs == ArmorIDs.Legs.NinjaPants) {
				NinjaWeeb = true;
			}
		}
		public override void PostUpdate() {
			if (!GuidetoMasterNinja || !GuidetoMasterNinja2) {
				return;
			}
			if (Player.ownedProjectileCounts[ModContent.ProjectileType<ThrowingKnifeCustom>()] > 0
				|| Player.ownedProjectileCounts[ModContent.ProjectileType<ThrowingKnifeCustom>()] > 0) {
				TimerForUltimate = 0;
				return;
			}
			if (++TimerForUltimate >= 60) {
				if (Player.ownedProjectileCounts[ModContent.ProjectileType<ThrowingKnifeCustom>()] < 1) {
					for (int i = 0; i < 8; i++)
						Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(1 + i, 1 + i), ModContent.ProjectileType<ThrowingKnifeCustom>(), Player.HeldItem.damage + 10, 0, Player.whoAmI);
				}
				if (Player.ownedProjectileCounts[ModContent.ProjectileType<ShurikenCustom>()] < 1) {
					for (int i = 0; i < 5; i++)
						Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(1 + i, 1 + i), ModContent.ProjectileType<ShurikenCustom>(), Player.HeldItem.damage + 10, 0, Player.whoAmI);
				}
			}
		}

		public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (GuidetoMasterNinja) {
				var GTMNcontain = new List<int>();
				//Independant damage
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
				if (Player.HasItem(ItemID.ThrowingKnife)) {
					StaticDamage += 5;
				}
				if (Player.HasItem(ItemID.PoisonedKnife)) {
					StaticDamage += 5;
				}
				if (Player.HasItem(ItemID.FrostDaggerfish)) {
					StaticDamage += 5;
				}
				if (Player.HasItem(ItemID.BoneDagger)) {
					StaticDamage += 5;
				}
				GTMNcount++;
				if (GTMNcount >= GTMNlimitCount) {
					var Aimto = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.UnitX) * 20;

					GTMNcontain.Clear();
					GTMNcontain.Add(ProjectileID.Shuriken);
					if (Player.HasItem(ItemID.PoisonedKnife)) {
						GTMNcontain.Add(ProjectileID.PoisonedKnife);
					}
					if (Player.HasItem(ItemID.ThrowingKnife)) {
						GTMNcontain.Add(ProjectileID.ThrowingKnife);
					}
					if (Player.HasItem(ItemID.FrostDaggerfish)) {
						GTMNcontain.Add(ProjectileID.FrostDaggerfish);
					}
					if (Player.HasItem(ItemID.BoneDagger)) {
						GTMNcontain.Add(ProjectileID.BoneDagger);
					}
					int proj1 = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Aimto, Main.rand.NextFromCollection(GTMNcontain), StaticDamage, 1f, Player.whoAmI);
					Main.projectile[proj1].penetrate = 1;
					GTMNcount = 0;
				}
			}
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			OnHitNPCWithProj_GuidetoMasterNinja1(target);
			OnHitNPCWithProj_GuidetoMasterNinja2(target.Center, hit.Damage, hit.Knockback);
		}
		private void OnHitNPCWithProj_GuidetoMasterNinja1(NPC target) {
			if (!GuidetoMasterNinja) {
				return;
			}
			if (Player.HasItem(ItemID.FrostDaggerfish)) {
				target.AddBuff(BuffID.Frostburn, 150);
			}
			if (Player.HasItem(ItemID.BoneDagger)) {
				target.AddBuff(BuffID.OnFire, 150);
			}
		}
		private void OnHitNPCWithProj_GuidetoMasterNinja2(Vector2 target, int damage, float knockback) {
			if (!GuidetoMasterNinja2) {
				return;
			}
			var NinjaBag = new List<int>();
			int[] RandomThrow = new int[] { ProjectileID.Shuriken, ProjectileID.ThrowingKnife, ProjectileID.PoisonedKnife };
			if ((Player.HasItem(ItemID.Shuriken) || Player.HasItem(ItemID.ThrowingKnife) || Player.HasItem(ItemID.PoisonedKnife)) && Main.rand.NextBool(10)) {
				NinjaBag.Clear();
				NinjaBag.AddRange(RandomThrow);
				if (Player.HasItem(ItemID.BoneDagger)) {
					NinjaBag.Add(ProjectileID.BoneDagger);
				}
				if (Player.HasItem(ItemID.FrostDaggerfish)) {
					NinjaBag.Add(ProjectileID.FrostDaggerfish);
				}
				Vector2 SpawnProjPos = target + new Vector2(0, -200);
				for (int i = 0; i < 12; i++) {
					var randomSpeed = Main.rand.NextVector2Circular(1, 1);
					Dust.NewDust(SpawnProjPos, 0, 0, DustID.Smoke, randomSpeed.X, randomSpeed.Y, 0, default, Main.rand.NextFloat(2f, 3.5f));
				}
				int proj1 = Projectile.NewProjectile(Player.GetSource_FromThis(), SpawnProjPos, Vector2.Zero, Main.rand.NextFromCollection(NinjaBag), damage, knockback, Player.whoAmI);
				Main.projectile[proj1].penetrate = 1;
			}
		}
		public override void ModifyItemScale(Item item, ref float scale) {
			if (GuidetoMasterNinja2 && item.type == ItemID.Katana) {
				scale += .5f;
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (!GuidetoMasterNinja2) {
				return;
			}
			if (item.type == ItemID.Shuriken || item.type == ItemID.ThrowingKnife || item.type == ItemID.PoisonedKnife) {
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
}
