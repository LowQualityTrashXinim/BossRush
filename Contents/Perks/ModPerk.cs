using System;
using Terraria;
using Terraria.ID;
using System.Linq;
using Terraria.Audio;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using BossRush.Contents.Items;
using BossRush.Common.Systems;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Projectiles;
using BossRush.Contents.Items.Potion;
using BossRush.Contents.Items.Toggle;
using BossRush.Contents.Items.Weapon;
using BossRush.Contents.Items.BuilderItem;

namespace BossRush.Contents.Perks {
	public class PowerUp : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<PowerUp>();
			CanBeStack = false;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .25f;
		}
		public override void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) {
			crit += 10;
		}
		public override void ModifyUseSpeed(Player player, Item item, ref float useSpeed) {
			useSpeed -= .1f;
		}
	}
	public class LifeForceOrb : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<LifeForceOrb>();
			CanBeStack = false;
		}
		public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			LifeForceSpawn(player, target);
		}
		public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			LifeForceSpawn(player, target);
		}
		private void LifeForceSpawn(Player player, NPC target) {
			if (Main.rand.NextBool(20))
				Projectile.NewProjectile(player.GetSource_FromThis(), target.Center + Main.rand.NextVector2Circular(100, 100), Vector2.Zero, ModContent.ProjectileType<LifeOrb>(), 0, 0, player.whoAmI);
		}
	}
	public class PoisonAura : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
		}
		public override void OnChoose(Player player) {
			player.QuickSpawnItem(player.GetSource_Loot(), ItemID.Bezoar);
		}
		public override void Update(Player player) {
			if (player.buffImmune[BuffID.Poisoned]) {
				float radius = player.GetModPlayer<PlayerStatsHandle>().GetAuraRadius();
				BossRushUtils.LookForHostileNPC(player.Center, out List<NPC> npclist, radius);
				for (int i = 0; i < 6; i++) {
					int dustRing = Dust.NewDust(player.Center + Main.rand.NextVector2CircularEdge(radius, radius), 0, 0, DustID.Poisoned);
					Main.dust[dustRing].noGravity = true;
					Main.dust[dustRing].velocity = Vector2.Zero;
					Main.dust[dustRing].scale = Main.rand.NextFloat(.75f, 1.5f);
					int dust = Dust.NewDust(player.Center + Main.rand.NextVector2Circular(radius, radius), 0, 0, DustID.Poisoned);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = Vector2.Zero;
					Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
				}
				if (npclist.Count > 0) {
					foreach (NPC npc in npclist) {
						npc.AddBuff(BuffID.Poisoned, 180);
					}
				}
			}
		}
	}
	public class OnFireAura : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<OnFireAura>();
			CanBeStack = false;
		}
		public override void OnChoose(Player player) {
			player.QuickSpawnItem(player.GetSource_Loot(), ItemID.ObsidianRose);
		}
		public override void Update(Player player) {
			if (player.buffImmune[BuffID.OnFire]) {
				float radius = player.GetModPlayer<PlayerStatsHandle>().GetAuraRadius();
				BossRushUtils.LookForHostileNPC(player.Center, out List<NPC> npclist, radius);
				for (int i = 0; i < 4; i++) {
					int dustRing = Dust.NewDust(player.Center + Main.rand.NextVector2CircularEdge(radius, radius), 0, 0, DustID.Torch);
					Main.dust[dustRing].noGravity = true;
					Main.dust[dustRing].velocity = Vector2.Zero;
					Main.dust[dustRing].scale = Main.rand.NextFloat(.75f, 1.5f);
					int dust = Dust.NewDust(player.Center + Main.rand.NextVector2Circular(radius, radius), 0, 0, DustID.Torch);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = -Vector2.UnitY * 4f;
					Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
				}
				if (npclist.Count > 0) {
					foreach (NPC npc in npclist) {
						npc.AddBuff(BuffID.OnFire, 180);
					}
				}
			}
		}
	}
	public class IllegalTrading : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 5;
		}
		public override void ResetEffect(Player player) {
			player.GetModPlayer<ChestLootDropPlayer>().WeaponAmountAddition += 1 * StackAmount;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage -= .15f * StackAmount;
		}
	}
	public class BackUpMana : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<BackUpMana>();
			CanBeStack = false;
		}
		public override void OnMissingMana(Player player, Item item, int neededMana) {
			player.statMana += neededMana;
			player.statLife = Math.Clamp(player.statLife - (int)(neededMana * .5f), 0, player.statLifeMax2);
		}
	}
	public class PeaceWithGod : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<PeaceWithGod>();
			CanBeStack = false;
		}
		public override void ResetEffect(Player player) {
			player.GetModPlayer<PlayerSynergyItemHandle>().SynergyBonusBlock = true;
			player.GetModPlayer<ChestLootDropPlayer>().CanDropSynergyEnergy = true;
		}
	}
	public class ChaoticImbue : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
		}
		public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			bool Opportunity = Main.rand.NextBool(10);
			int[] debuffArray = new int[] { BuffID.OnFire, BuffID.OnFire3, BuffID.Bleeding, BuffID.Frostburn, BuffID.Frostburn2, BuffID.ShadowFlame, BuffID.CursedInferno, BuffID.Ichor, BuffID.Venom, BuffID.Poisoned, BuffID.Confused, BuffID.Midas };
			if (!debuffArray.Where(d => !target.HasBuff(d)).Any())
				return;
			for (int i = 0; i < debuffArray.Length; i++) {
				if (Opportunity && !target.HasBuff(debuffArray[i])) {
					target.AddBuff(debuffArray[i], 1800);
					break;
				}
				else {
					if (!Opportunity)
						Opportunity = Main.rand.NextBool(10);
				}
				if (i == debuffArray.Length - 1 && Opportunity)
					i = 0;
			}
		}
	}
	public class AlchemistKnowledge : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void ResetEffect(Player player) {
			player.GetModPlayer<MysteriousPotionPlayer>().PotionPointAddition += StackAmount;
			player.GetModPlayer<ChestLootDropPlayer>().PotionTypeAmountAddition += StackAmount;
		}
	}
	public class Dirt : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
		}
		public override void ResetEffect(Player player) {
			base.ResetEffect(player);
			if (player.HasItem(ItemID.DirtBlock)) {
				player.statDefense += 15;
				player.AddBuff(BuffID.WellFed3, 60);
			}
		}
	}
	public class PotionExpert : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<PotionExpert>();
			CanBeStack = false;
		}
		public override void ResetEffect(Player player) {
			player.GetModPlayer<PerkPlayer>().perk_PotionExpert = true;
			player.GetModPlayer<PlayerStatsHandle>().BuffTime += .25f;
		}
	}
	public class SniperCharge : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
		}
		int RandomCountDown = 0;
		int OpportunityWindow = 0;
		public override void Update(Player player) {
			if (!player.ItemAnimationActive)
				RandomCountDown = BossRushUtils.CountDown(RandomCountDown);
			if (RandomCountDown <= 0) {
				if (OpportunityWindow == 0) {
					BossRushUtils.CombatTextRevamp(player.Hitbox, Color.ForestGreen, "!");
					SoundEngine.PlaySound(SoundID.MaxMana);
				}
				OpportunityWindow++;
				if (OpportunityWindow >= 600 || player.ItemAnimationActive) {
					OpportunityWindow = 0;
					RandomCountDown = Main.rand.Next(150, 210);
				}
			}
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			if (item.DamageType == DamageClass.Ranged && RandomCountDown <= 0 && OpportunityWindow < 600) {
				damage *= 2;
			}
		}
	}
	public class SelfExplosion : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 5;
		}
		public override void OnHitByAnything(Player player) {
			float radius = player.GetModPlayer<PlayerStatsHandle>().GetAuraRadius() * 2;
			player.Center.LookForHostileNPC(out List<NPC> npclist, radius);
			foreach (NPC npc in npclist) {
				int direction = player.Center.X - npc.Center.X > 0 ? -1 : 1;
				npc.StrikeNPC(npc.CalculateHitInfo(75 * StackAmount, direction, false, 10));
			}
			for (int i = 0; i < 150; i++) {
				int smokedust = Dust.NewDust(player.Center, 0, 0, DustID.Smoke);
				Main.dust[smokedust].noGravity = true;
				Main.dust[smokedust].velocity = Main.rand.NextVector2Circular(radius / 12f, radius / 12f);
				Main.dust[smokedust].scale = Main.rand.NextFloat(.75f, 2f);
				int dust = Dust.NewDust(player.Center, 0, 0, DustID.Torch);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(radius / 12f, radius / 12f);
				Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
			}
		}
	}
	public class SpecialPotion : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
		}
		public override void OnChoose(Player player) {
			int type = Main.rand.Next(new int[] { ModContent.ItemType<TitanElixir>(), ModContent.ItemType<BerserkerElixir>(), ModContent.ItemType<GunslingerElixir>(), ModContent.ItemType<CommanderElixir>(), ModContent.ItemType<SageElixir>(), });
			player.QuickSpawnItem(player.GetSource_FromThis(), type);
		}
	}
	public class ProjectileProtection : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 5;
		}
		public override void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) {
			hurtInfo.SourceDamage = (int)(hurtInfo.SourceDamage * (1 - .15f * StackAmount));
		}
	}
	public class ProjectileDuplication : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 5;
		}
		public override void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (Main.rand.NextFloat() <= .1f * StackAmount && type != ModContent.ProjectileType<ArenaMakerProj>())
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(10), type, damage, knockback, player.whoAmI);
		}
	}
	public class SpeedArmor : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 2;
		}
		public override void ResetEffect(Player player) {
			player.statDefense += (int)Math.Round(player.velocity.Length()) * StackAmount;
		}
	}
	public class CelestialRage : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
		}
		public override void OnChoose(Player player) {
			player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<CelestialWrath>());
		}
	}
	public class BlessingOfSolar : Perk {
		public override void SetDefaults() {
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = false;
			CanBeChoosen = false;
		}
		public override void Update(Player player) {
			player.GetModPlayer<ChestLootDropPlayer>().UpdateMeleeChanceMutilplier += 1.92f;
		}
		public override void ModifyItemScale(Player player, Item item, ref float scale) {
			if (item.DamageType == DamageClass.Melee)
				scale += .08f;
		}
		public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			if (Main.rand.NextFloat() <= .05f && (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed)) {
				Item.NewItem(item.GetSource_FromThis(), target.Hitbox, new Item(ItemID.Heart));
			}
		}
	}
	public class BlessingOfVortex : Perk {
		public override void SetDefaults() {
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = false;
			CanBeChoosen = false;
		}
		public override void Update(Player player) {
			player.GetModPlayer<ChestLootDropPlayer>().UpdateRangeChanceMutilplier += 1.92f;
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			if (Main.rand.NextFloat() <= .01f && proj.DamageType == DamageClass.Ranged)
				modifiers.SourceDamage *= 4;
		}
		public override void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) {
			if (item.DamageType == DamageClass.Ranged)
				crit += 5;
		}
	}
	public class BlessingOfNebula : Perk {
		public override void SetDefaults() {
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = false;
			CanBeChoosen = false;
		}
		public override void Update(Player player) {
			player.GetModPlayer<ChestLootDropPlayer>().UpdateMagicChanceMutilplier += 1.92f;
		}
		public override void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi) {
			multi -= .06f;
		}
		public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (Main.rand.NextFloat() <= .05f && proj.DamageType == DamageClass.Magic) {
				Item.NewItem(proj.GetSource_FromThis(), target.Hitbox, new Item(ItemID.Star));
			}
		}
	}
	public class BlessingOfStarDust : Perk {
		public override void SetDefaults() {
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = false;
			CanBeChoosen = false;
		}
		public override void ResetEffect(Player player) {
			player.maxMinions += 1;
			player.maxTurrets += 1;
		}
		public override void Update(Player player) {
			player.GetModPlayer<ChestLootDropPlayer>().UpdateSummonChanceMutilplier += 1.92f;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage.Base += (player.maxMinions + player.maxTurrets) / 2;
		}
	}
	public class BlessingOfPerk : Perk {
		public override void SetDefaults() {
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = true;
			CanBeChoosen = false;
			Tooltip =
				"+ Increases perk range amount by 1";
			StackLimit = 999;
		}
	}
	public class ArenaBlessing : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 4;
		}
		public override string ModifyToolTip() {
			switch (StackAmount) {
				case 1:
					return Language.GetTextValue($"Mods.BossRush.ModPerk.{Name}1.Description");
				case 2:
					return Language.GetTextValue($"Mods.BossRush.ModPerk.{Name}2.Description");
				case 3:
					return Language.GetTextValue($"Mods.BossRush.ModPerk.{Name}3.Description");
			}
			return Language.GetTextValue($"Mods.BossRush.ModPerk.{Name}.Description");
		}
		public override void Update(Player player) {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<AdventureSpirit>()] < 1) {
				Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<AdventureSpirit>(), 0, 0, player.whoAmI);
			}
		}
	}
	public class StellarRetirement : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
			textureString = BossRushTexture.ACCESSORIESSLOT;
		}
		public override void Update(Player player) {
			if (Main.rand.NextBool(100) && !Main.dayTime) {
				Projectile.NewProjectile(Entity.GetSource_NaturalSpawn(), player.Center + new Vector2(Main.rand.NextFloat(-1000, 1000), -1500), (Vector2.UnitY * 15).Vector2RotateByRandom(25), ProjectileID.FallingStar, 1000, 5);
			}
		}
	}
	public class GodGiveDice : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
			Tooltip =
				"+ God give you a dice";
			CanBeChoosen = false;
		}
		public override void OnChoose(Player player) {
			player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<GodDice>());
		}
	}
	public class PotionCleanse : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
			textureString = BossRushTexture.ACCESSORIESSLOT;
		}
		public override void Update(Player player) {
			player.GetModPlayer<PerkPlayer>().perk_PotionCleanse = true;
		}
	}
	public class OverchargedMana : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
			textureString = BossRushTexture.ACCESSORIESSLOT;
		}
		public override void ModifyMaxStats(Player player, ref StatModifier health, ref StatModifier mana) {
			mana /= 2;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			if (player.statMana == player.statLifeMax2 && item.DamageType == DamageClass.Magic) {
				damage += 0.33f;
			}
		}
		public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (hit.DamageType == DamageClass.Magic) {
				target.Center.LookForHostileNPC(out List<NPC> npclist, 64);
				for (int i = 0; i < 120; i++) {
					var d = Dust.NewDust(target.Center + Main.rand.NextVector2CircularEdge(64, 64), 0, 0, DustID.BlueTorch);
					Main.dust[d].noGravity = true;
				}
				foreach (var i in npclist) {
					player.StrikeNPCDirect(target, i.CalculateHitInfo(5 + (int)(damageDone * 0.1f), 1, Main.rand.NextBool(10)));
				}
			}
		}
	}
	public class ShopPerk : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 2;
		}
		public override void Update(Player player) {
			player.coinLuck *= StackAmount;
		}
	}
	public class CritPerk : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void Update(Player player) {
			player.GetCritChance(DamageClass.Generic) += 5 * StackAmount;
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			if (Main.rand.NextFloat(1, 101) <= proj.CritChance) {
				modifiers.ScalingArmorPenetration += 0.9f;
			}
		}
	}
	public class SummonBuffPerk : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 4;
		}
		public override void Update(Player player) {
			player.maxMinions += StackAmount * 2;
		}
		public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (hit.DamageType != DamageClass.Summon) {
				hit.SourceDamage -= (int)(hit.SourceDamage * 0.05f) * StackAmount;
			}
		}
	}
	public class TrueMeleeBuffPerk : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 4;
		}
		public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
			if (modifiers.DamageType == DamageClass.Melee && item.shoot == ProjectileID.None) {
				modifiers.DamageVariationScale *= 0f;
				if (!player.immune) {
					modifiers.ScalingBonusDamage += .25f * StackAmount;
				}
			}
		}
	}
	public class KnockpackPerk : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
		}
		public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
			modifiers.HitDirectionOverride = 0;
			modifiers.Knockback *= 2;
			if (target.knockBackResist >= 1f) {
				modifiers.Defense.Base -= player.GetWeaponKnockback(item);
			}
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			modifiers.HitDirectionOverride = 0;
			modifiers.Knockback *= 2;
			if (target.knockBackResist >= 1f) {
				modifiers.Defense.Base -= proj.knockBack;
			}
		}
	}
	public class ImprovedManaPotion : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
		}
	}
}
