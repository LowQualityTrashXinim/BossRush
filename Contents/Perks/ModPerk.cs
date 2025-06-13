using System;
using Terraria;
using Terraria.ID;
using System.Linq;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.Localization;
using BossRush.Common.Utils;
using Terraria.DataStructures;
using BossRush.Contents.Items;
using Microsoft.Xna.Framework;
using BossRush.Contents.Skill;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Projectiles;
using BossRush.Contents.Items.Weapon;
using BossRush.Contents.BuffAndDebuff;
using BossRush.Common.Systems.Mutation;
using BossRush.Contents.Items.RelicItem;
using BossRush.Contents.Items.BuilderItem;
using BossRush.Contents.Items.Accessories.LostAccessories;
using BossRush.Common.Global;
using BossRush.Contents.Perks.BlessingPerk;
using BossRush.Common.ChallengeMode;

namespace BossRush.Contents.Perks {
	public class MarkOfSpectre : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<MarkOfSpectre>();
			CanBeStack = false;
		}
		public override void UpdateEquip(Player player) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MovementSpeed, 1.35f);
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.JumpBoost, 1.65f);
		}
		public override void ModifyHitByNPC(Player player, NPC npc, ref Player.HurtModifiers modifiers) {
			ModifyHit(ref modifiers);
		}
		public override void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers) {
			ModifyHit(ref modifiers);
		}
		private static void ModifyHit(ref Player.HurtModifiers modifiers) {
			modifiers.FinalDamage += .25f;
			modifiers.Knockback *= .35f;
		}
		public override bool FreeDodge(Player player, Player.HurtInfo hurtInfo) {
			if (!player.immune && Main.rand.NextFloat() <= .6f) {
				player.AddImmuneTime(hurtInfo.CooldownCounter, 60);
				player.immune = true;
				return true;
			}
			return base.FreeDodge(player, hurtInfo);
		}
	}
	public class LethalKnockBack : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<LethalKnockBack>();
			list_category.Add(PerkCategory.WeaponUpgrade);
			CanBeStack = false;
		}
		public override void ModifyKnockBack(Player player, Item item, ref StatModifier knockback) {
			if (item.DamageType == DamageClass.Melee) {
				knockback += .15f;
			}
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage -= .11f;
		}
		public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
			modifiers.SourceDamage += item.knockBack * .1f * Math.Clamp(Math.Abs(target.knockBackResist - 1), 0, 3f);
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			modifiers.SourceDamage += proj.knockBack * .1f * Math.Clamp(Math.Abs(target.knockBackResist - 1), 0, 3f);
		}
	}
	public class PowerUp : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<PowerUp>();
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .25f * StackAmount(player);
		}
		public override void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) {
			crit += 10 * StackAmount(player);
		}
		public override void ModifyUseSpeed(Player player, Item item, ref float useSpeed) {
			useSpeed -= .25f + .1f * StackAmount(player);
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
		private static void LifeForceSpawn(Player player, NPC target) {
			if (Main.rand.NextBool(10))
				Projectile.NewProjectile(player.GetSource_FromThis(), target.Center + Main.rand.NextVector2Circular(target.width + 100, target.height + 100), Vector2.Zero, ModContent.ProjectileType<LifeOrb>(), 0, 0, player.whoAmI);
		}
	}
	public class BackUpMana : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<BackUpMana>();
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void ModifyMaxStats(Player player, ref StatModifier health, ref StatModifier mana) {
			mana.Base += 67 * StackAmount(player);
		}
		public override void OnMissingMana(Player player, Item item, int neededMana) {
			//player.statMana += neededMana;
			//player.statLife = Math.Clamp(player.statLife - (int)(neededMana * .5f), 1, player.statLifeMax2);
			if (!player.HasBuff<BackUpMana_CoolDown>()) {
				player.statMana = player.statManaMax2;
				player.AddBuff(ModContent.BuffType<BackUpMana_CoolDown>(), BossRushUtils.ToSecond(Math.Clamp(37 - 7 * StackAmount(player), 1, 9999)));
			}
		}
	}
	public class BackUpMana_CoolDown : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff(true);
		}
		public override void Update(Player player, ref int buffIndex) {
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RegenMana, -.5f);
		}
	}
	public class Dirt : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
			textureString = BossRushUtils.GetTheSameTextureAsEntity<Dirt>();
		}
		public override void UpdateEquip(Player player) {
			PlayerStatsHandle handle = player.GetModPlayer<PlayerStatsHandle>();
			handle.AugmentationChance += .1f * StackAmount(player);
			handle.RandomizeChanceEnchantment += .1f * StackAmount(player);
		}
	}
	public class AlchemistEmpowerment : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAs<AlchemistEmpowerment>("PotionExpert");
			CanBeStack = false;
		}
		public override void ResetEffect(Player player) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MysteriousPotionEffectiveness, Base: 3);
			player.GetModPlayer<PerkPlayer>().perk_AlchemistPotion = true;
			player.GetModPlayer<PerkPlayer>().perk_PotionCleanse = true;
			player.GetModPlayer<PerkPlayer>().perk_PotionExpert = true;
			player.GetModPlayer<PlayerStatsHandle>().LootboxCanDropSpecialPotion = true;
		}
	}
	public class SelfExplosion : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<SelfExplosion>();
			CanBeStack = true;
			StackLimit = 2;
		}
		public override void OnHitByAnything(Player player) {
			player.Center.LookForHostileNPC(out List<NPC> npclist, 500);
			foreach (NPC npc in npclist) {
				int direction = player.Center.X - npc.Center.X > 0 ? -1 : 1;
				npc.StrikeNPC(npc.CalculateHitInfo((120 + player.statLife) * StackAmount(player), direction, false, 10));
			}
			for (int i = 0; i < 150; i++) {
				int smokedust = Dust.NewDust(player.Center, 0, 0, DustID.Smoke);
				Main.dust[smokedust].noGravity = true;
				Main.dust[smokedust].velocity = Main.rand.NextVector2Circular(500 / 12f, 500 / 12f);
				Main.dust[smokedust].scale = Main.rand.NextFloat(.75f, 2f);
				int dust = Dust.NewDust(player.Center, 0, 0, DustID.Torch);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(500 / 12f, 500 / 12f);
				Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
			}
			player.AddBuff(ModContent.BuffType<ExplosionHealing>(), BossRushUtils.ToSecond(5 + StackAmount(player)));
		}
	}
	public class ExplosionHealing : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			player.lifeRegen += 22;
		}
	}
	public class ProjectileProtection : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<ProjectileProtection>();
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void UpdateEquip(Player player) {
			player.endurance += .05f * StackAmount(player);
		}
		public override void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers) {
			modifiers.SourceDamage += -.3f * StackAmount(player);
		}
	}
	public class ProjectileDuplication : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			modifiers.SourceDamage -= (StackLimit - StackAmount(player) + 1) * .15f;
		}
		public override void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (type != ModContent.ProjectileType<ArenaMakerProj>()
				|| type == ModContent.ProjectileType<NeoDynamiteExplosion>()
				|| type == ModContent.ProjectileType<TowerDestructionProjectile>()
				|| !ContentSamples.ProjectilesByType[type].minion) {
				player.GetModPlayer<PlayerStatsHandle>().Request_ShootExtra(StackAmount(player), 5 + 5 * StackAmount(player));
			}
		}
	}
	public class ScatterShot : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<ScatterShot>();
			CanBeStack = false;
		}
		public override void ResetEffect(Player player) {
			player.GetModPlayer<PerkPlayer>().perk_ScatterShot = true;
		}
	}
	public class BloodStrike : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<BloodStrike>();
			CanBeStack = false;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage *= 1.36f;
			damage.Flat += 7;
		}
		public override void OnUseItem(Player player, Item item) {
			if (item.IsAWeapon() && player.itemAnimation == player.itemAnimationMax && player.ItemAnimationActive) {
				int damage = (int)Math.Round(player.GetWeaponDamage(player.HeldItem) * .05f);
				player.statLife = Math.Clamp(player.statLife - damage, 0, player.statLifeMax2);
				BossRushUtils.CombatTextRevamp(player.Hitbox, Color.Red, "-" + damage, Main.rand.Next(-10, 40));
			}
		}
	}
	public class SpeedArmor : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<SpeedArmor>();
			CanBeStack = true;
			StackLimit = 2;
		}
		public override void UpdateEquip(Player player) {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, Additive: 1 + .45f * StackAmount(player));
			modplayer.AddStatsToPlayer(PlayerStats.JumpBoost, Additive: 1 + .2f * StackAmount(player));
			modplayer.AddStatsToPlayer(PlayerStats.Defense, Base: (int)Math.Round(player.velocity.Length()) * StackAmount(player));
		}
		public override void PostUpdateRun(Player player) {
			player.runAcceleration += .5f;
			player.runSlowdown += .25f;
		}
	}
	public class CelestialRage : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
		}
		public override void OnChoose(Player player) {
			player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<CelestialWrath>());
		}
		public override void UpdateEquip(Player player) {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 2f);
			modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 5);
			modplayer.AddStatsToPlayer(PlayerStats.PureDamage, Multiplicative: 1.1f);
		}
	}
	public class ArenaBlessing : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 4;
		}
		public override string ModifyToolTip() {
			int stack = StackAmount(Main.LocalPlayer);
			if (stack > 0) {
				return Language.GetTextValue($"Mods.BossRush.ModPerk.{Name}.Description{stack}");
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
			if (Main.rand.NextBool(200)) {
				int damage = (int)player.GetDamage(DamageClass.Generic).ApplyTo(1000);
				int proj = Projectile.NewProjectile(Entity.GetSource_NaturalSpawn(), player.Center + new Vector2(Main.rand.NextFloat(-1000, 1000), -1500), (Vector2.UnitY * 15).Vector2RotateByRandom(25), ProjectileID.SuperStar, damage, 5);
				Main.projectile[proj].tileCollide = false;
			}
		}
	}
	public class OverchargedMana : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
			textureString = BossRushTexture.ACCESSORIESSLOT;
		}
		public override void UpdateEquip(Player player) {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.RegenMana, Base: 20);
			if (player.statMana <= player.statManaMax2 / 2) {
				modplayer.AddStatsToPlayer(PlayerStats.RegenMana, Base: 40);
			}
			if (player.statMana > player.statLife) {
				modplayer.AddStatsToPlayer(PlayerStats.MagicDMG, 1.25f);
			}
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			if (player.statMana == player.statManaMax2 && item.DamageType == DamageClass.Magic) {
				damage += 0.77f;
			}
		}
		public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (hit.DamageType == DamageClass.Magic && player.statMana == player.statLifeMax2) {
				target.Center.LookForHostileNPC(out List<NPC> npclist, 200);
				for (int i = 0; i < 65; i++) {
					var d = Dust.NewDust(target.Center + Main.rand.NextVector2CircularEdge(64, 64), 0, 0, DustID.BlueTorch);
					Main.dust[d].noGravity = true;
				}
				float damage = .15f;
				if (player.statMana > player.statLife) {
					damage += .3f;
				}
				foreach (var i in npclist) {
					player.StrikeNPCDirect(target, i.CalculateHitInfo(5 + (int)(proj.damage * damage), 1, Main.rand.NextBool(10)));
				}
			}
		}
	}
	public class BeyondCritcal : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void Update(Player player) {
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.CritChance, Base: 5 * StackAmount(player));
		}
		public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
			if (Main.rand.NextFloat() <= .04f * StackAmount(player)) {
				modifiers.FinalDamage *= 2.5f;
				modifiers.ScalingArmorPenetration += 0.9f;
			}
			else if (player.GetModPlayer<PlayerStatsHandle>().ModifyHit_Before_Crit && Main.rand.NextFloat() <= .15f * StackAmount(player)) {
				modifiers.FinalDamage *= 2.5f;
				modifiers.ScalingArmorPenetration += 0.9f;
			}
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			if (Main.rand.NextFloat() <= .04f * StackAmount(player)) {
				modifiers.FinalDamage *= 2.5f;
				modifiers.ScalingArmorPenetration += 0.9f;
			}
			else if (player.GetModPlayer<PlayerStatsHandle>().ModifyHit_Before_Crit && Main.rand.NextFloat() <= .15f * StackAmount(player)) {
				modifiers.FinalDamage *= 2.5f;
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
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.MaxMinion, Base: 2 * StackAmount(player));
		}
		public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (hit.DamageType != DamageClass.Summon) {
				hit.SourceDamage -= (int)(hit.SourceDamage * 0.05f) * StackAmount(player);
			}
		}
	}
	public class TrueMeleeBuffPerk : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 2;
			list_category.Add(PerkCategory.WeaponUpgrade);
		}
		public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
			if (modifiers.DamageType == DamageClass.Melee && item.shoot == ProjectileID.None) {
				modifiers.DamageVariationScale *= 0f;
				modifiers.SourceDamage += .25f * StackAmount(player);
				if (!player.immune) {
					modifiers.SourceDamage += .44f;
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
	public class ImprovedPotion : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
		}
		public override void UpdateEquip(Player player) {
			player.GetModPlayer<PlayerStatsHandle>().BuffTime += .45f;
			player.GetModPlayer<PerkPlayer>().perk_ImprovedPotion = true;
		}
	}
	public class WeaponExpert : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
			textureString = BossRushTexture.ACCESSORIESSLOT;
			list_category.Add(PerkCategory.WeaponUpgrade);
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			if (item.damage <= 20 || (Main.hardMode && item.damage <= 42)) {
				damage += 1;
			}
		}
	}
	public class AspectOfTheUnderworld : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
			textureString = BossRushTexture.ACCESSORIESSLOT;
			DataStorer.AddContext("Perk_RingOfFire", new(
				300,
				Vector2.Zero,
				false,
				Color.DarkRed
				));
		}
		public override void UpdateEquip(Player player) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, 1.15f, 1, 10);
			if (player.IsHealthAbovePercentage(.66f)) {
				player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.RegenHP, Flat: -32);
			}
			player.buffImmune[BuffID.OnFire] = true;
		}
		public override void Update(Player player) {
			DataStorer.ActivateContext(player, "Perk_RingOfFire");
			BossRushUtils.LookForHostileNPC(player.Center, out List<NPC> npclist, 300);
			for (int i = 0; i < 4; i++) {
				int dust = Dust.NewDust(player.Center + Main.rand.NextVector2Circular(300, 300), 0, 0, DustID.Torch);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = -Vector2.UnitY * 4f;
				Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
			}
			if (npclist.Count > 0) {
				foreach (NPC npc in npclist) {
					npc.AddBuff(BuffID.OnFire, 180);
					if (player.IsHealthAbovePercentage(.67f)) {
						npc.AddBuff(BuffID.OnFire3, 180);
						npc.AddBuff(ModContent.BuffType<TheUnderworldWrath>(), 180);
					}
				}
			}
		}
		public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
			if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
				modifiers.CritDamage *= 1.15f;
			}
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
				modifiers.CritDamage *= 1.15f;
			}
		}
	}
	public class AspectOfFirstChaos : Perk {
		public override bool SelectChoosing() {
			return Main.LocalPlayer.IsEquipAcc(ModContent.ItemType<ChaosTablet>());
		}
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 2;
			textureString = BossRushTexture.ACCESSORIESSLOT;
		}
		public override string ModifyToolTip() {
			if (StackAmount(Main.LocalPlayer) >= 1) {
				return Language.GetTextValue($"Mods.BossRush.ModPerk.{Name}.Description1");
			}
			return base.ModifyToolTip();
		}
		public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (StackAmount(player) >= 1) {
				var globalproj = proj.GetGlobalProjectile<RoguelikeGlobalProjectile>();
				if (globalproj.Source_CustomContextInfo == "AspectOfFirstChaos") {
					if (!Main.rand.NextBool(4)) {
						Vector2 newPos = BossRushUtils.SpawnRanPositionThatIsNotIntoTile(target.Center, 300, 300);
						Projectile.NewProjectile(proj.GetSource_FromAI(), newPos, (target.Center - newPos).SafeNormalize(Vector2.Zero) * Main.rand.Next(10, 15), Main.rand.Next(TerrariaArrayID.UltimateProjPack), proj.damage, proj.knockBack, player.whoAmI);
					}
				}
			}
			int randcount = 1 + StackAmount(player);
			for (int i = 0; i < randcount; i++) {
				if (Main.rand.NextBool(5)) {
					randcount++;
					player.StrikeNPCDirect(target, target.CalculateHitInfo(proj.damage, 0));
				}
			}
			bool Opportunity = Main.rand.NextBool(10);
			int[] debuffArray =
				{ BuffID.OnFire, BuffID.OnFire3, BuffID.Bleeding, BuffID.Frostburn, BuffID.Frostburn2, BuffID.ShadowFlame,
				BuffID.CursedInferno, BuffID.Ichor, BuffID.Venom, BuffID.Poisoned, BuffID.Confused, BuffID.Midas };
			if (debuffArray.Where(d => !target.HasBuff(d)).Count() >= debuffArray.Length)
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
		public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			int randcount = 1 + StackAmount(player);
			for (int i = 0; i < randcount; i++) {
				if (Main.rand.NextBool(5)) {
					randcount++;
					player.StrikeNPCDirect(target, target.CalculateHitInfo(item.damage, 0));
				}
			}
			if (!Main.rand.NextBool(4)) {
				Projectile.NewProjectile(player.GetSource_ItemUse(item, "AspectOfFirstChaos"), player.Center, (target.Center - player.Center).SafeNormalize(Vector2.Zero) * 4, Main.rand.Next(TerrariaArrayID.UltimateProjPack), item.damage, item.knockBack, player.whoAmI);
			}
			bool Opportunity = Main.rand.NextBool(10);
			int[] debuffArray =
				{ BuffID.OnFire, BuffID.OnFire3, BuffID.Bleeding, BuffID.Frostburn, BuffID.Frostburn2, BuffID.ShadowFlame,
				BuffID.CursedInferno, BuffID.Ichor, BuffID.Venom, BuffID.Poisoned, BuffID.Confused, BuffID.Midas };
			if (debuffArray.Where(d => !target.HasBuff(d)).Count() >= debuffArray.Length)
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
		public override void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (!Main.rand.NextBool(Math.Clamp(4 - StackAmount(player), 1, 4))) {
				return;
			}
			EntitySource_ItemUse_WithAmmo newsource = new EntitySource_ItemUse_WithAmmo(source.Player, source.Item, source.AmmoItemIdUsed, "AspectOfFirstChaos");
			Projectile.NewProjectile(newsource, position, velocity, Main.rand.Next(TerrariaArrayID.UltimateProjPack), damage, knockback, player.whoAmI);
		}
	}
	public class EnergyAbsorption : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
		}
		public override void UpdateEquip(Player player) {
			player.endurance += .1f;
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.EnergyCap, 1.2f);
		}
		public override void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) {
			player.GetModPlayer<SkillHandlePlayer>().Modify_EnergyAmount((int)(hurtInfo.Damage * .25f));
		}
	}
	public class HybridRanger : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
		}
		public override void UpdateEquip(Player player) {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.RangeDMG, Additive: 1.1f);
			modplayer.AddStatsToPlayer(PlayerStats.MaxMinion, Base: 1);
			modplayer.AddStatsToPlayer(PlayerStats.MaxSentry, Base: 1);
		}
		public override void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (item.DamageType != DamageClass.Ranged) {
				return;
			}
			int amount = (player.maxMinions + player.maxTurrets) / 2;
			for (int i = 0; i < amount; i++) {
				Vector2 pos = position + Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(300, 400), Main.rand.NextFloat(300, 400));
				for (int l = 0; l < 20; l++) {
					Dust dust = Dust.NewDustDirect(pos, 0, 0, DustID.SpectreStaff);
					dust.noGravity = true;
					dust.velocity = Main.rand.NextVector2Circular(5, 5);
					dust.scale = Main.rand.NextFloat(.9f, 2.25f);
				}
				Projectile proj = Projectile.NewProjectileDirect(source, pos,
					Vector2.One.Vector2RotateByRandom(180), ProjectileID.SpectreWrath, damage, knockback, player.whoAmI);
				proj.timeLeft = 1800;
				proj.extraUpdates = 6;
			}
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			if (proj.minion || proj.DamageType == DamageClass.Summon) {
				if (Main.rand.Next(1, 101) <= player.GetTotalCritChance<RangedDamageClass>()) {
					modifiers.SetCrit();
				}
			}
		}
	}
	public class OathOfSword : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
		}
		public override void UpdateEquip(Player player) {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			if (BossRushUtils.IsAVanillaSword(player.HeldItem.type)
				|| player.HeldItem.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckVanillaSwingWithModded)
				&& player.HeldItem.DamageType == DamageClass.Melee) {
				modplayer.AddStatsToPlayer(PlayerStats.MeleeDMG, 2f, 1.11f);
			}
			else {
				modplayer.AddStatsToPlayer(PlayerStats.MagicDMG, .45f);
				modplayer.AddStatsToPlayer(PlayerStats.RangeDMG, .45f);
				modplayer.AddStatsToPlayer(PlayerStats.SummonDMG, .45f);
			}
		}
	}
	public class BlessingOfMoon : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
		}
		public override bool SelectChoosing() {
			Player player = Main.LocalPlayer;
			PerkPlayer perkplayer = player.GetModPlayer<PerkPlayer>();
			if (perkplayer.perks.ContainsKey(GetPerkType<BlessingOfNebula>())
				&& perkplayer.perks.ContainsKey(GetPerkType<BlessingOfSolar>())
				&& perkplayer.perks.ContainsKey(GetPerkType<BlessingOfVortex>())
				&& perkplayer.perks.ContainsKey(GetPerkType<BlessingOfStardust>())) {
				return true;
			}
			return false;
		}
		public override bool FreeDodge(Player player, Player.HurtInfo hurtInfo) {
			if (!player.immune && Main.rand.NextFloat() <= .75f && !Main.dayTime) {
				player.AddImmuneTime(hurtInfo.CooldownCounter, 60);
				player.immune = true;
				return true;
			}
			return base.FreeDodge(player, hurtInfo);
		}
		public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			int damage = player.GetWeaponDamage(item);
			float knockback = player.GetWeaponKnockback(item);
			player.StrikeNPCDirect(target, target.CalculateHitInfo((int)(damage * 1.25f) + target.defense / 2, hit.HitDirection, hit.Crit, knockback));

			IEntitySource source = player.GetSource_OnHit(target);
			Vector2 pos = target.Center.Add(Main.rand.Next(-100, 100), Main.rand.Next(300, 350));
			Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * 8;
			Projectile.NewProjectile(source, pos, vel, ProjectileID.LunarFlare, (int)(damage * .77f), knockback, player.whoAmI);


			target.AddBuff(ModContent.BuffType<MoonLightDebuff>(), BossRushUtils.ToSecond(Main.rand.Next(4, 8)));
		}
		public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			int damage = proj.damage;
			float knockback = proj.knockBack;
			player.StrikeNPCDirect(target, target.CalculateHitInfo((int)(damage * 1.25f) + target.defense / 2, hit.HitDirection, hit.Crit, knockback));

			if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == player.HeldItem.type) {
				IEntitySource source = player.GetSource_OnHit(target);
				Vector2 pos = target.Center.Add(Main.rand.Next(-100, 100), Main.rand.Next(300, 350));
				Vector2 vel = (target.Center - pos).SafeNormalize(Vector2.Zero) * 8;
				Projectile.NewProjectile(source, pos, vel, ProjectileID.LunarFlare, (int)(damage * .77f), knockback, player.whoAmI);

			}
			target.AddBuff(ModContent.BuffType<MoonLightDebuff>(), BossRushUtils.ToSecond(Main.rand.Next(4, 8)));
		}
	}
	public class MoonLightDebuff : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff();
		}
		public override void Update(NPC npc, ref int buffIndex) {
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().StatDefense *= 0;
		}
	}
	public class TitanPower : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
		}
		public override bool SelectChoosing() {
			Player player = Main.LocalPlayer;
			PerkPlayer perkplayer = player.GetModPlayer<PerkPlayer>();
			if (perkplayer.perks.ContainsKey(GetPerkType<BlessingOfTitan>())
				&& perkplayer.perks.ContainsKey(GetPerkType<ProjectileProtection>())
				&& player.IsEquipAcc(ModContent.ItemType<TitanBlood>())) {
				return true;
			}
			return false;
		}
		public override void ResetEffect(Player player) {
			PlayerStatsHandle.SetSecondLifeCondition(player, "P_TP", !player.HasBuff(ModContent.BuffType<TitanPowerBuff>()));
		}
		public override void UpdateEquip(Player player) {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.MaxHP, Base: 200);
			modplayer.AddStatsToPlayer(PlayerStats.Thorn, Base: 1.5f);
			modplayer.AddStatsToPlayer(PlayerStats.Defense, Additive: 1.25f, Flat: 15);
			player.endurance += .4f;
		}
		public override bool PreKill(Player player) {
			if (PlayerStatsHandle.GetSecondLife(player, "P_TP")) {
				player.AddBuff(ModContent.BuffType<TitanPowerBuff>(), BossRushUtils.ToMinute(4));
				player.Heal(player.statLifeMax2);
				return true;
			}
			return false;
		}
	}
	public class TitanPowerBuff : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff(true);
		}
	}
	public class DemolitionistGunner : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 3;
		}
		public override string ModifyToolTip() {
			if (StackAmount(Main.LocalPlayer) >= 2) {
				return DescriptionIndex(1);
			}
			return Description;
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			float chance = 0.25f * StackAmount(player);
			if (item.useAmmo == AmmoID.Bullet && type == ProjectileID.Bullet && Main.rand.NextFloat() <= chance) {
				type = ProjectileID.ExplosiveBullet;
			}
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			float chance = 0.1f * StackAmount(player);
			if (proj.type == ProjectileID.ExplosiveBullet && Main.rand.NextFloat() <= chance) {
				modifiers.SourceDamage += .55f;
			}
		}
		public override void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (StackAmount(player) >= 2) {
				if (Main.rand.NextFloat() <= .05f) {
					Vector2 vel = -Vector2.UnitY.RotatedBy(MathHelper.ToRadians(30 * player.direction)) * 15;
					Projectile.NewProjectile(source, position, vel, ModContent.ProjectileType<FriendlyGrenadeProjectile>(), damage * 3, knockback, player.whoAmI);
				}
			}
		}
		public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			float chance = 0.05f * StackAmount(player);
			if (proj.type != ProjectileID.ExplosiveBullet || Main.rand.NextFloat() > chance) {
				return;
			}
			for (int i = 0; i < 25; i++) {
				int smokedust = Dust.NewDust(target.Center, 0, 0, DustID.Smoke);
				Main.dust[smokedust].noGravity = true;
				Main.dust[smokedust].velocity = Main.rand.NextVector2Circular(15f, 15f);
				Main.dust[smokedust].scale = Main.rand.NextFloat(.75f, 2f);
				int dust = Dust.NewDust(target.Center, 0, 0, DustID.Torch);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(15f, 15f);
				Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
			}
			target.Center.LookForHostileNPC(out List<NPC> npclist, 150f);
			foreach (NPC npc in npclist) {
				player.StrikeNPCDirect(npc, hit);
			}
		}
	}

	public class MindOfBattlefield : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void UpdateEquip(Player player) {
			PlayerStatsHandle statplayer = player.GetModPlayer<PlayerStatsHandle>();
			int stack = StackAmount(player);
			if (player.HeldItem.useAmmo == AmmoID.Bullet) {
				statplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, 1 + .14f * stack);
			}
			else if (player.HeldItem.useAmmo == AmmoID.Arrow) {
				statplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 7 * stack);
			}
			else if (player.HeldItem.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckVanillaSwingWithModded)) {
				statplayer.AddStatsToPlayer(PlayerStats.Iframe, 1 + .11f * stack);
			}
			statplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1 + .2f * stack);
			statplayer.AddStatsToPlayer(PlayerStats.JumpBoost, 1 + .15f * stack);
			statplayer.AddStatsToPlayer(PlayerStats.StaticDefense, Base: 10 * stack);
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			//Hope this work exactly what I expect it to do
			Projectile sample = ContentSamples.ProjectilesByType[proj.type];
			Rectangle defaultProjSize = new((int)proj.position.X, (int)proj.position.Y, sample.width, sample.height);
			if (ProjectileID.Sets.Explosive[proj.type] && defaultProjSize.Intersects(target.Hitbox)) {
				modifiers.SourceDamage += .5f * StackAmount(player);
			}
		}
	}

	public class SoulShatter : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void UpdateEquip(Player player) {
			PlayerStatsHandle handle = player.GetModPlayer<PlayerStatsHandle>();
			handle.AddStatsToPlayer(PlayerStats.FullHPDamage, 6f);
			handle.AddStatsToPlayer(PlayerStats.CritDamage, 1.4f);
			SoulShatter_ModPlayer charged = player.GetModPlayer<SoulShatter_ModPlayer>();
			charged.Perk = true;
			if (charged.Charged) {
				handle.AddStatsToPlayer(PlayerStats.PureDamage, 1.4f);
				handle.AddStatsToPlayer(PlayerStats.CritDamage, 2);
			}
		}
		public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			Chance_InstantKill(player, target, hit);
		}
		public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			Chance_InstantKill(player, target, hit);
		}
		private static void Chance_InstantKill(Player player, NPC target, NPC.HitInfo info) {
			if (player.GetModPlayer<PlayerStatsHandle>().NPC_HitCount == 1) {
				target.Center.LookForHostileNPC(out List<NPC> npclist, 400);
				foreach (NPC npc in npclist) {
					npc.AddBuff(ModContent.BuffType<Shatter>(), BossRushUtils.ToSecond(Main.rand.Next(10, 17)));
				}
			}
			if (Main.rand.NextFloat() <= 0.0001f) {
				info.InstantKill = true;
				player.StrikeNPCDirect(target, info);
			}
		}
	}
	public class SoulShatter_ModPlayer : ModPlayer {
		public bool Charged = false;
		public int counter = 0;
		public const int ChargeTime = 150;
		public bool Perk = false;
		public override void ResetEffects() {
			Perk = false;
			Charged = false;
		}
		public override void UpdateEquips() {
			if (!Perk) {
				return;
			}
			if (++counter > ChargeTime) {
				Charged = true;
			}
		}
		public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Charged = false;
			counter = 0;
			return base.Shoot(item, source, position, velocity, type, damage, knockback);
		}
	}
	public class UntappedPotential : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
		}
		public override bool SelectChoosing() {
			return Main.LocalPlayer.inventory.Where(i => i.ModItem != null && i.ModItem is SynergyModItem).Any();
		}
	}
	public class GlassCannon : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 999;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage *= 1 + StackAmount(player) * .25f;
		}
		public override void UpdateEquip(Player player) {
			player.ModPlayerStats().CappedHealthAmount = 50;
		}
	}
}
