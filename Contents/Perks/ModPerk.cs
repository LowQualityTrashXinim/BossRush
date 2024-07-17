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
using BossRush.Common.Systems;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Projectiles;
using BossRush.Contents.Items.Toggle;
using BossRush.Contents.Items.Weapon;
using BossRush.Contents.Items.BuilderItem;

namespace BossRush.Contents.Perks {
	public class StrokeOfLuck : Perk {
		public override void SetDefaults() {
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeChoosen = false;
			CanBeStack = false;
		}
		public override void OnHitByNPC(Player player, NPC npc, Player.HurtInfo hurtInfo) {
			if (Main.rand.NextBool(25)) {
				hurtInfo.Damage = Main.rand.Next(1, (int)(hurtInfo.Damage * .85f));
			}
		}
		public override void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) {
			if (Main.rand.NextBool(25)) {
				hurtInfo.Damage = Main.rand.Next(1, (int)(hurtInfo.Damage * .85f));
			}
		}
		public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
			if (Main.rand.NextBool(25)) {
				modifiers.SourceDamage += Main.rand.NextFloat(.15f, 1f);
			}
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			if (Main.rand.NextBool(25)) {
				modifiers.SourceDamage += Main.rand.NextFloat(.15f, 1f);
			}
		}
	}
	public class UncertainStrike : Perk {
		public override void SetDefaults() {
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeChoosen = false;
			CanBeStack = false;
		}
		public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
			if (Main.rand.NextBool(3))
				modifiers.SourceDamage += Main.rand.NextFloat(-.15f, .45f);
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			if (Main.rand.NextBool(3))
				modifiers.SourceDamage += Main.rand.NextFloat(-.15f, .45f);
		}
	}
	public class LethalKnockBack : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
		}
		public override void ModifyKnockBack(Player player, Item item, ref StatModifier knockback) {
			if (item.DamageType == DamageClass.Melee) {
				knockback += .15f;
			}
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage -= .17f;
		}
		public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
			modifiers.SourceDamage += item.knockBack * .1f * Math.Clamp(Math.Abs(target.knockBackResist - 1), 0, 3f);
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			modifiers.SourceDamage += proj.knockBack * .1f * Math.Clamp(Math.Abs(target.knockBackResist - 1), 0, 3f);
		}
	}
	public class WindSlash : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
		}
		public override void Update(Player player) {
			if (player.HeldItem.DamageType == DamageClass.Melee
				&& player.HeldItem.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckVanillaSwingWithModded)
				&& Main.mouseLeft
				&& player.itemAnimation == player.itemAnimationMax) {
				Vector2 speed = Vector2.UnitX * player.direction;
				if (player.HeldItem.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModded)) {
					speed = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
				}
				int damage = (int)(player.HeldItem.damage * .75f);
				float length = player.HeldItem.Size.Length() * player.GetAdjustedItemScale(player.HeldItem);
				if (player.GetModPlayer<WindSlashPerkPlayer>().StrikeOpportunity) {
					speed *= 1.5f;
					damage *= 3;
				}
				Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center.PositionOFFSET(speed, length + 17), speed * 5, ModContent.ProjectileType<WindSlashProjectile>(), damage, 2f, player.whoAmI);
			}
		}
	}
	public class WindSlashPerkPlayer : ModPlayer {
		public int OpportunityWindow = 0;
		public bool StrikeOpportunity = false;
		public override void PostUpdate() {
			if (Player.ItemAnimationActive) {
				OpportunityWindow = 0;
				StrikeOpportunity = false;
			}
			if (OpportunityWindow >= BossRushUtils.ToSecond(3)) {
				StrikeOpportunity = true;
				return;
			}
			OpportunityWindow++;
		}
	}
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
			useSpeed -= .35f;
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
			if (Main.rand.NextBool(10))
				Projectile.NewProjectile(player.GetSource_FromThis(), target.Center + Main.rand.NextVector2Circular(100, 100), Vector2.Zero, ModContent.ProjectileType<LifeOrb>(), 0, 0, player.whoAmI);
		}
	}
	//public class PoisonAura : Perk {
	//	public override void SetDefaults() {
	//		CanBeStack = false;
	//	}
	//	public override void OnChoose(Player player) {
	//		player.QuickSpawnItem(player.GetSource_Loot(), ItemID.Bezoar);
	//	}
	//	public override void Update(Player player) {
	//		if (player.buffImmune[BuffID.Poisoned]) {
	//			float radius = player.GetModPlayer<PlayerStatsHandle>().GetAuraRadius(300);
	//			BossRushUtils.LookForHostileNPC(player.Center, out List<NPC> npclist, radius);
	//			for (int i = 0; i < 6; i++) {
	//				int dustRing = Dust.NewDust(player.Center + Main.rand.NextVector2CircularEdge(radius, radius), 0, 0, DustID.Poisoned);
	//				Main.dust[dustRing].noGravity = true;
	//				Main.dust[dustRing].velocity = Vector2.Zero;
	//				Main.dust[dustRing].scale = Main.rand.NextFloat(.75f, 1.5f);
	//				int dust = Dust.NewDust(player.Center + Main.rand.NextVector2Circular(radius, radius), 0, 0, DustID.Poisoned);
	//				Main.dust[dust].noGravity = true;
	//				Main.dust[dust].velocity = Vector2.Zero;
	//				Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
	//			}
	//			if (npclist.Count > 0) {
	//				foreach (NPC npc in npclist) {
	//					npc.AddBuff(BuffID.Poisoned, 180);
	//				}
	//			}
	//		}
	//	}
	//}
	//public class OnFireAura : Perk {
	//	public override void SetDefaults() {
	//		textureString = BossRushUtils.GetTheSameTextureAsEntity<OnFireAura>();
	//		CanBeStack = false;
	//	}
	//	public override void OnChoose(Player player) {
	//		player.QuickSpawnItem(player.GetSource_Loot(), ItemID.ObsidianRose);
	//	}
	//	public override void Update(Player player) {
	//		if (player.buffImmune[BuffID.OnFire]) {
	//			float radius = player.GetModPlayer<PlayerStatsHandle>().GetAuraRadius(300);
	//			BossRushUtils.LookForHostileNPC(player.Center, out List<NPC> npclist, radius);
	//			for (int i = 0; i < 4; i++) {
	//				int dustRing = Dust.NewDust(player.Center + Main.rand.NextVector2CircularEdge(radius, radius), 0, 0, DustID.Torch);
	//				Main.dust[dustRing].noGravity = true;
	//				Main.dust[dustRing].velocity = Vector2.Zero;
	//				Main.dust[dustRing].scale = Main.rand.NextFloat(.75f, 1.5f);
	//				int dust = Dust.NewDust(player.Center + Main.rand.NextVector2Circular(radius, radius), 0, 0, DustID.Torch);
	//				Main.dust[dust].noGravity = true;
	//				Main.dust[dust].velocity = -Vector2.UnitY * 4f;
	//				Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
	//			}
	//			if (npclist.Count > 0) {
	//				foreach (NPC npc in npclist) {
	//					npc.AddBuff(BuffID.OnFire, 180);
	//				}
	//			}
	//		}
	//	}
	//}
	public class IllegalTrading : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 5;
			CanBeChoosen = false;
		}
		public override void ResetEffect(Player player) {
			player.GetModPlayer<ChestLootDropPlayer>().WeaponAmountAddition += 3 + StackAmount;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage -= .07f * StackAmount;
		}
	}
	public class BackUpMana : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<BackUpMana>();
			CanBeStack = false;
		}
		public override void ModifyMaxStats(Player player, ref StatModifier health, ref StatModifier mana) {
			mana.Base += 67;
		}
		public override void OnMissingMana(Player player, Item item, int neededMana) {
			player.statMana += neededMana;
			player.statLife = Math.Clamp(player.statLife - (int)(neededMana * .5f), 1, player.statLifeMax2);
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
			int[] debuffArray =
				{ BuffID.OnFire, BuffID.OnFire3, BuffID.Bleeding, BuffID.Frostburn, BuffID.Frostburn2, BuffID.ShadowFlame,
				BuffID.CursedInferno, BuffID.Ichor, BuffID.Venom, BuffID.Poisoned, BuffID.Confused, BuffID.Midas };
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
		public override void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (!Main.rand.NextBool(4)) {
				return;
			}
			Projectile.NewProjectile(source, position, velocity, Main.rand.Next(TerrariaArrayID.UltimateProjPack), damage, knockback, player.whoAmI);
		}
	}
	public class Dirt : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
			CanBeChoosen = false;
		}
		public override void ResetEffect(Player player) {
			if (player.HasItem(ItemID.DirtBlock)) {
				player.statDefense += 15;
				player.AddBuff(BuffID.WellFed3, 60);
			}
		}
	}
	public class AlchemistEmpowerment : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAs<PeaceWithGod>("PotionExpert");
			CanBeStack = false;
		}
		public override void ResetEffect(Player player) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MysteriousPotionEffectiveness, Base: 3);
			player.GetModPlayer<PerkPlayer>().perk_AlchemistPotion = true;
			player.GetModPlayer<PlayerStatsHandle>().BuffTime -= .35f;
			player.GetModPlayer<PerkPlayer>().perk_PotionCleanse = true;
			player.GetModPlayer<PerkPlayer>().perk_PotionExpert = true;
			player.GetModPlayer<PlayerStatsHandle>().BuffTime += .35f;
			player.GetModPlayer<ChestLootDropPlayer>().LootboxCanDropSpecialPotion = true;
		}
	}
	public class SelfExplosion : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 2;
		}
		public override void OnHitByAnything(Player player) {
			float radius = player.GetModPlayer<PlayerStatsHandle>().GetAuraRadius(250) * 2;
			player.Center.LookForHostileNPC(out List<NPC> npclist, radius);
			foreach (NPC npc in npclist) {
				int direction = player.Center.X - npc.Center.X > 0 ? -1 : 1;
				npc.StrikeNPC(npc.CalculateHitInfo(120 * StackAmount, direction, false, 10));
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
			player.AddBuff(ModContent.BuffType<ExplosionHealing>(), BossRushUtils.ToSecond(5));
		}
	}
	public class ExplosionHealing : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			player.lifeRegen += 47;
		}
	}
	public class ProjectileProtection : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) {
			hurtInfo.SourceDamage = (int)(hurtInfo.SourceDamage * (1 - .3f * StackAmount));
		}
	}
	public class ProjectileDuplication : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			modifiers.SourceDamage -= (StackLimit - StackAmount + 1) * .15f;
		}
		public override void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (type != ModContent.ProjectileType<ArenaMakerProj>()) {
				player.GetModPlayer<PlayerStatsHandle>().requestShootExtra = StackAmount;
				player.GetModPlayer<PlayerStatsHandle>().requestVelocityChange = 10;
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
			CanBeStack = false;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage *= 1.36f;
			damage.Flat += 7;
		}
		public override void OnUseItem(Player player, Item item) {
			if (player.itemAnimation == player.itemAnimationMax) {
				int damage = (int)Math.Round(player.GetWeaponDamage(player.HeldItem) * .05f);
				player.statLife = Math.Clamp(player.statLife - damage, 0, player.statLifeMax2);
				BossRushUtils.CombatTextRevamp(player.Hitbox, Color.Red, "-" + damage, Main.rand.Next(-10, 40));
			}
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
		public override void Update(Player player) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MovementSpeed, Additive: .15f);
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
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void ModifyItemScale(Player player, Item item, ref float scale) {
			if (item.DamageType == DamageClass.Melee)
				scale += .12f * StackAmount;
		}
		public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			if (item.DamageType != DamageClass.Melee && item.DamageType != DamageClass.MeleeNoSpeed) {
				return;
			}
			if (Main.rand.NextFloat() <= .07f * StackAmount) {
				Item.NewItem(item.GetSource_FromThis(), target.Hitbox, new Item(ItemID.Heart));
			}
			if (Main.rand.NextBool(10)) {
				target.AddBuff(BuffID.Daybreak, BossRushUtils.ToSecond(3.5f));
			}
		}
		public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (proj.DamageType == DamageClass.Melee && Main.rand.NextBool(10)) {
				target.AddBuff(BuffID.Daybreak, BossRushUtils.ToSecond(3.5f));
			}
		}
	}
	public class BlessingOfVortex : Perk {
		public override void SetDefaults() {
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void UpdateEquip(Player player) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritDamage, Additive: 1.5f);
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			if (Main.rand.NextFloat() <= .01f * StackAmount && proj.DamageType == DamageClass.Ranged)
				modifiers.SourceDamage *= 4;
		}
		public override void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) {
			if (item.DamageType == DamageClass.Ranged)
				crit += 7 * StackAmount;
		}
	}
	public class BlessingOfNebula : Perk {
		public override void SetDefaults() {
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi) {
			multi -= .11f * StackAmount;
		}
		public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (Main.rand.NextFloat() <= .06f * StackAmount && proj.DamageType == DamageClass.Magic) {
				Item.NewItem(proj.GetSource_FromThis(), target.Hitbox, new Item(ItemID.Star));
			}
		}
		public override void ModifyMaxStats(Player player, ref StatModifier health, ref StatModifier mana) {
			mana.Base += 78;
		}
	}
	public class BlessingOfStarDust : Perk {
		public override void SetDefaults() {
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void ResetEffect(Player player) {
			player.maxMinions += 1;
			player.maxTurrets += 1;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage.Base += (player.maxMinions + player.maxTurrets) / 2 * StackAmount;
		}
		public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (ProjectileID.Sets.IsAWhip[proj.type]) {
				target.AddBuff(ModContent.BuffType<StarGaze>(),BossRushUtils.ToSecond(Main.rand.Next(1,4)));
			}
		}
	}
	public class StarGaze : ModBuff {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
		}
		public override bool ReApply(NPC npc, int time, int buffIndex) {
			return true;
		}
		public override void Update(NPC npc, ref int buffIndex) {
			npc.lifeRegen -= 15;
			if(Main.hardMode) {
				npc.lifeRegen -= 40;
			}
			if (npc.buffTime[Type] == 0) {
				npc.StrikeNPC(npc.CalculateHitInfo((int)(npc.lifeMax * .05f), 1));
			}
		}
	}
	public class BlessingOfSynergy : Perk {
		public override void SetDefaults() {
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			if (player.GetModPlayer<SynergyModPlayer>().CompareOldvsNewItemType) {
				damage.Flat += 10 * StackAmount;
			}
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
		public override string ModifyToolTip() {
			if (StackAmount == 10) {
				return "don't you think it is enough now ?";
			}
			return base.ModifyToolTip();
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
			CanBeChoosen = false;
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
	public class ExtraCritical : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 5;
		}
		public override string ModifyToolTip() {
			if (StackAmount > 1) {
				return Language.GetTextValue($"Mods.BossRush.ModPerk.{Name}1.Description");
			}
			return Language.GetTextValue($"Mods.BossRush.ModPerk.{Name}.Description");
		}
		public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
			if (Main.rand.NextFloat() < .01f * StackLimit) {
				modifiers.FinalDamage *= 4;
			}
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			if (Main.rand.NextFloat() < .01f * StackLimit) {
				modifiers.FinalDamage *= 4;
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
		public override void ResetEffect(Player player) {
			player.GetModPlayer<PerkPlayer>().perk_ImprovedManaPotion = true;
		}
	}
}
