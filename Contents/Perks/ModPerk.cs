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
using BossRush.Contents.Skill;
using BossRush.Common.General;
using System.Collections.Generic;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Projectiles;
using BossRush.Contents.Items.Weapon;
using BossRush.Contents.BuffAndDebuff;
using BossRush.Common.RoguelikeChange;
using BossRush.Contents.Items.RelicItem;
using BossRush.Contents.Items.BuilderItem;
using BossRush.Contents.Items.Accessories.LostAccessories;
using BossRush.Contents.WeaponEnchantment;
using BossRush.Common.Systems.ArgumentsSystem;
using BossRush.Common.Systems.Mutation;

namespace BossRush.Contents.Perks {
	public class SuppliesDrop : Perk {
		public override void SetDefaults() {
			textureString = BossRushTexture.SUPPILESDROP;
			CanBeStack = true;
			StackLimit = -1;
			CanBeChoosen = false;
		}
		public override void OnChoose(Player player) {
			LootBoxBase.GetWeapon(out int weapon, out int amount);
			player.QuickSpawnItem(player.GetSource_FromThis(), weapon, amount);
		}
	}
	public class GiftOfRelic : Perk {
		public override void SetDefaults() {
			textureString = BossRushTexture.Get_MissingTexture("Perk");
			CanBeStack = true;
			StackLimit = -1;
			CanBeChoosen = false;
		}
		public override void OnChoose(Player player) {
			player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<RelicContainer>());
		}
	}
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
		private void ModifyHit(ref Player.HurtModifiers modifiers) {
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
			textureString = BossRushUtils.GetTheSameTextureAsEntity<WindSlash>();
			list_category.Add(PerkCategory.WeaponUpgrade);
			CanBeStack = false;
		}
		public override bool SelectChoosing() {
			return false;
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
			if (!Player.GetModPlayer<PerkPlayer>().perks.ContainsKey(Perk.GetPerkType<WindSlash>())) {
				return;
			}
			if (Player.ItemAnimationActive) {
				OpportunityWindow = 0;
				StrikeOpportunity = false;
			}
			if (OpportunityWindow >= BossRushUtils.ToSecond(1.5f)) {
				StrikeOpportunity = true;
				Dust.NewDust(Player.Center, 0, 0, DustID.SolarFlare);
				return;
			}
			OpportunityWindow++;
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
		private void LifeForceSpawn(Player player, NPC target) {
			if (Main.rand.NextBool(10))
				Projectile.NewProjectile(player.GetSource_FromThis(), target.Center + Main.rand.NextVector2Circular(target.width + 100, target.height + 100), Vector2.Zero, ModContent.ProjectileType<LifeOrb>(), 0, 0, player.whoAmI);
		}
	}
	//public class IllegalTrading : Perk {
	//	public override void SetDefaults() {
	//		CanBeStack = true;
	//		StackLimit = 5;
	//		CanBeChoosen = false;
	//	}
	//	public override void ResetEffect(Player player) {
	//		player.GetModPlayer<ChestLootDropPlayer>().WeaponAmountAddition += 3 + StackAmount(player);
	//	}
	//	public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
	//		damage -= .07f * StackAmount(player);
	//	}
	//}
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
			player.GetModPlayer<PerkPlayer>().perk_PotionCleanse = true;
			player.GetModPlayer<PerkPlayer>().perk_PotionExpert = true;
			player.GetModPlayer<ChestLootDropPlayer>().LootboxCanDropSpecialPotion = true;
		}
	}
	public class SelfExplosion : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<SelfExplosion>();
			CanBeStack = true;
			StackLimit = 2;
		}
		public override void OnHitByAnything(Player player) {
			float radius = player.GetModPlayer<PlayerStatsHandle>().GetAuraRadius(250) * 2;
			player.Center.LookForHostileNPC(out List<NPC> npclist, radius);
			foreach (NPC npc in npclist) {
				int direction = player.Center.X - npc.Center.X > 0 ? -1 : 1;
				npc.StrikeNPC(npc.CalculateHitInfo((120 + player.statLife) * StackAmount(player), direction, false, 10));
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
		public override void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers) {
			modifiers.SourceDamage -= -.3f * StackAmount(player);
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
				player.GetModPlayer<PlayerStatsHandle>().requestShootExtra = StackAmount(player);
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
		public override void ResetEffect(Player player) {
			player.statDefense += (int)Math.Round(player.velocity.Length()) * StackAmount(player);
		}
		public override void Update(Player player) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MovementSpeed, Additive: 1.15f);
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
			textureString = BossRushUtils.GetTheSameTextureAsEntity<BlessingOfSolar>();
			list_category.Add(PerkCategory.Starter);
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void UpdateEquip(Player player) {
			player.GetModPlayer<ChestLootDropPlayer>().UpdateRangeChanceMutilplier += 1f;
		}
		public override void ModifyItemScale(Player player, Item item, ref float scale) {
			if (item.DamageType == DamageClass.Melee)
				scale += .12f * StackAmount(player);
		}
		public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			if (item.DamageType != DamageClass.Melee && item.DamageType != DamageClass.MeleeNoSpeed) {
				return;
			}
			if (Main.rand.NextFloat() <= .07f * StackAmount(player)) {
				Item.NewItem(item.GetSource_FromThis(), target.Hitbox, new Item(ItemID.Heart));
			}
			if (Main.rand.NextBool(10)) {
				target.AddBuff(ModContent.BuffType<MeltingDefense>(), BossRushUtils.ToSecond(3.5f));
			}
		}
		public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (proj.DamageType == DamageClass.Melee && Main.rand.NextBool(10)) {
				target.AddBuff(ModContent.BuffType<MeltingDefense>(), BossRushUtils.ToSecond(3.5f));
			}
		}
	}
	public class MeltingDefense : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff();
		}
		public override void Update(NPC npc, ref int buffIndex) {
			npc.lifeRegen -= Math.Clamp(npc.defense, 0, 40);
		}
	}
	public class BlessingOfVortex : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<BlessingOfVortex>();
			list_category.Add(PerkCategory.Starter);
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void UpdateEquip(Player player) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritDamage, Additive: 1.5f);
			player.GetModPlayer<ChestLootDropPlayer>().UpdateRangeChanceMutilplier += 1f;
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			if (Main.rand.NextFloat() <= .01f * StackAmount(player) && proj.DamageType == DamageClass.Ranged)
				modifiers.SourceDamage *= 4;
		}
		public override void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) {
			if (item.DamageType == DamageClass.Ranged)
				crit += 7 * StackAmount(player);
		}
	}
	public class BlessingOfNebula : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<BlessingOfNebula>();
			list_category.Add(PerkCategory.Starter);
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void UpdateEquip(Player player) {
			player.GetModPlayer<ChestLootDropPlayer>().UpdateMagicChanceMutilplier += 1f;
		}
		public override void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi) {
			multi -= .11f * StackAmount(player);
		}
		public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (Main.rand.NextFloat() <= .06f * StackAmount(player) && proj.DamageType == DamageClass.Magic) {
				Item.NewItem(proj.GetSource_FromThis(), target.Hitbox, new Item(ItemID.Star));
			}
		}
		public override void ModifyMaxStats(Player player, ref StatModifier health, ref StatModifier mana) {
			mana.Base += 78 * StackAmount(player);
		}
	}
	public class BlessingOfStarDust : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<BlessingOfStarDust>();
			list_category.Add(PerkCategory.Starter);
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void UpdateEquip(Player player) {
			player.maxMinions += 1;
			player.maxTurrets += 1;
			player.GetModPlayer<ChestLootDropPlayer>().UpdateSummonChanceMutilplier += 1f;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage.Base += (player.maxMinions + player.maxTurrets) / 2 * StackAmount(player);
		}
		public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (ProjectileID.Sets.IsAWhip[proj.type]) {
				target.AddBuff(ModContent.BuffType<StarGaze>(), BossRushUtils.ToSecond(Main.rand.Next(1, 4)));
			}
		}
	}
	public class StarGaze : ModBuff {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
		}
		public override bool ReApply(NPC npc, int time, int buffIndex) {
			return true;
		}
		public override void Update(NPC npc, ref int buffIndex) {
			npc.lifeRegen -= 15;
			if (Main.hardMode) {
				npc.lifeRegen -= 40;
			}
			if (npc.buffTime[buffIndex] == 0) {
				int damage = Math.Clamp((int)(npc.lifeMax * .01f), 0, 1000);
				npc.StrikeNPC(npc.CalculateHitInfo(damage, 1));
			}
		}
	}
	public class BlessingOfSynergy : Perk {
		public override void SetDefaults() {
			list_category.Add(PerkCategory.Starter);
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			if (player.GetModPlayer<SynergyModPlayer>().CompareOldvsNewItemType) {
				damage.Flat += 10 * StackAmount(player);
			}
		}
	}
	public class BlessingOfTitan : Perk {
		public override void SetDefaults() {
			list_category.Add(PerkCategory.Starter);
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void UpdateEquip(Player player) {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.MaxHP, Flat: 100 * StackAmount(player));
			modplayer.AddStatsToPlayer(PlayerStats.Defense, Additive: 1.15f * StackAmount(player), Flat: 10);
			modplayer.AddStatsToPlayer(PlayerStats.Thorn, Flat: 2f * StackAmount(player));
		}
	}
	public class BlessingOfPerk : Perk {
		public override void SetDefaults() {
			list_category.Add(PerkCategory.Starter);
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = true;
			CanBeChoosen = false;
			Tooltip =
				"+ Increases perk range amount by 1";
			StackLimit = 999;
		}
		public override string ModifyToolTip() {
			if (StackAmount(Main.LocalPlayer) == 10) {
				return "don't you think it is enough now ?";
			}
			return base.ModifyToolTip();
		}
		public override void UpdateEquip(Player player) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.LootDropIncrease, Base: 1);
		}
	}
	public class BlessingOfEvasive : Perk {
		public override void SetDefaults() {
			list_category.Add(PerkCategory.Starter);
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = true;
			StackLimit = 3;
		}
		public override void UpdateEquip(Player player) {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			player.GetJumpState<SimpleExtraJump>().Enable();
			modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1 + .15f * StackAmount(player));
			modplayer.AddStatsToPlayer(PlayerStats.JumpBoost, 1 + .25f * StackAmount(player));
			modplayer.DodgeChance += .04f * StackAmount(player);
		}
	}
	public class ArenaBlessing : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 4;
		}
		public override string ModifyToolTip() {
			switch (StackAmount(Main.LocalPlayer)) {
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
				target.Center.LookForHostileNPC(out List<NPC> npclist, 64);
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
			player.GetCritChance(DamageClass.Generic) += 5 * StackAmount(player);
		}
		public override void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers) {
			if (Main.rand.NextFloat() < .04f * StackLimit) {
				modifiers.FinalDamage *= 2.5f;
				modifiers.ScalingArmorPenetration += 0.9f;
			}
		}
		public override void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			if (Main.rand.NextFloat() < .04f * StackLimit) {
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
		}
		public override void UpdateEquip(Player player) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, 1.15f, 1, 10);
			if (player.ComparePlayerHealthInPercentage(.66f)) {
				player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.RegenHP, Flat: -32);
			}
			player.buffImmune[BuffID.OnFire] = true;
		}
		public override void Update(Player player) {
			float radius = player.GetModPlayer<PlayerStatsHandle>().GetAuraRadius(300);
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
					if (player.ComparePlayerHealthInPercentage(.67f)) {
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
				return Language.GetTextValue($"Mods.BossRush.ModPerk.{Name}1.Description");
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
			modplayer.AddStatsToPlayer(PlayerStats.RangeDMG, Additive: 1 + (player.maxMinions + player.maxTurrets) * .05f);
			modplayer.AddStatsToPlayer(PlayerStats.MaxMinion, Base: player.GetTotalDamage(DamageClass.Ranged).ApplyTo(1) * .01f);
			modplayer.AddStatsToPlayer(PlayerStats.MaxSentry, Base: player.GetTotalDamage(DamageClass.Ranged).ApplyTo(1) * .01f);
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
				modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, Multiplicative: .45f);
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
				&& perkplayer.perks.ContainsKey(GetPerkType<BlessingOfStarDust>())) {
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
			Vector2 pos = target.Center.Subtract(Main.rand.Next(-100, 100), Main.rand.Next(300, 350));
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
				Vector2 pos = target.Center.Subtract(Main.rand.Next(-100, 100), Main.rand.Next(300, 350));
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
			npc.GetGlobalNPC<RoguelikeOverhaulNPC>().StatDefense *= 0;
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
	public class LostInWonderLand : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			StackLimit = 10;
		}
		public override void UpdateEquip(Player player) {
			EnchantmentModplayer enchantplayer = player.GetModPlayer<EnchantmentModplayer>();
			AugmentsPlayer augmentplayer = player.GetModPlayer<AugmentsPlayer>();
			ModContent.GetInstance<MutationSystem>().MutationChance += .1f * StackAmount(player);
			augmentplayer.IncreasesChance += .05f * StackAmount(player);
			enchantplayer.RandomizeChanceEnchantment += .05f * StackAmount(player);
		}
	}
}
