using BossRush.Contents.Items;
using BossRush.Contents.Items.Chest;
using BossRush.Contents.Items.Potion;
using BossRush.Contents.Items.Toggle;
using BossRush.Contents.Items.Weapon;
using BossRush.Contents.Projectiles;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Perks {
	public class PowerUp : Perk {
		public override void SetDefaults() {
			Tooltip =
					"+ Increase damage by 10%";
			CanBeStack = true;
			StackLimit = 3;
		}
		public override string ModifyToolTip() {
			if (StackAmount == 3)
				return "Increases damage by 30%" +
					"\nIncreases attack speed by 10%" +
					"\nIncreases critical strike chance by 10";
			if (StackAmount == 2)
				return "Increases damage by 20%" +
					"\nIncreases attack speed by 10%";
			return
				"+ Increases damage by 10%";
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage += .1f * StackAmount;
		}
		public override void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) {
			if (StackAmount >= 3)
				crit += 10;

		}
		public override void ModifyUseSpeed(Player player, Item item, ref float useSpeed) {
			if (StackAmount >= 2)
				useSpeed += .1f;
		}
	}
	public class LifeForceOrb : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<LifeForceOrb>();
			Tooltip = "+ Attacking enemy will periodically create a life orb that heal you";
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
	public class ImmunityToPoison : Perk {
		public override void SetDefaults() {
			Tooltip =
				"+ Give you immunity to poison" +
				"\n+ Make a poison aura around player";
			CanBeStack = false;
		}
		public override void UpdateEquip(Player player) {
			player.buffImmune[BuffID.Poisoned] = true;
		}
		public override void Update(Player player) {
			BossRushUtils.LookForHostileNPC(player.Center, out List<NPC> npclist, 150);
			for (int i = 0; i < 2; i++) {
				int dust = Dust.NewDust(player.Center + Main.rand.NextVector2Circular(100, 100), 0, 0, DustID.Poisoned);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Vector2.Zero;
				Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
			}
			if (npclist.Count > 0) {
				foreach (NPC npc in npclist) {
					npc.AddBuff(BuffID.Poisoned, 1);
				}
			}
		}
	}
	public class ImmunityToOnFire : Perk {
		public override void SetDefaults() {
			Tooltip =
				"+ Give you immunity to On Fire !" +
				"\n+ Make a fiery aura around player";
			CanBeStack = false;
		}
		public override void UpdateEquip(Player player) {
			player.buffImmune[BuffID.OnFire] = true;
		}
		public override void Update(Player player) {
			BossRushUtils.LookForHostileNPC(player.Center, out List<NPC> npclist, 150);
			for (int i = 0; i < 2; i++) {
				int dust = Dust.NewDust(player.Center + Main.rand.NextVector2Circular(100, 100), 0, 0, DustID.Torch);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = -Vector2.UnitY * 4f;
				Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
			}
			if (npclist.Count > 0) {
				foreach (NPC npc in npclist) {
					npc.AddBuff(BuffID.OnFire, 1);
				}
			}
		}
	}
	public class IllegalTrading : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			Tooltip =
				"+ Increase amount of weapon drop from chest by 1 !" +
				"\n- Decrease your damage by 10%";
			StackLimit = 5;
		}
		public override void ResetEffect(Player player) {
			player.GetModPlayer<ChestLootDropPlayer>().WeaponAmountAddition += 1 * StackAmount;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			damage -= .1f * StackAmount;
		}
	}
	public class BackUpMana : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<BackUpMana>();
			Tooltip =
				  "+ You can fire magic weapon forever" +
				  "\n- When you are out of mana, mana cost reduce is by 50% and you use your life instead";
			CanBeStack = false;
		}
		public override void OnMissingMana(Player player, Item item, int neededMana) {
			player.statMana += neededMana;
			player.statLife = Math.Clamp(player.statLife - (int)(neededMana * .5f), 0, player.statLifeMax2);
		}
	}
	public class PeaceWithGod : Perk {
		public override void SetDefaults() {
			Tooltip =
				"+ God no longer angry at you and now opening lootbox give you syngery weapon" +
				"\n- Synergy bonus no longer available";
			CanBeStack = false;
		}
		public override void ResetEffect(Player player) {
			player.GetModPlayer<PlayerSynergyItemHandle>().SynergyBonusBlock = true;
			player.GetModPlayer<ChestLootDropPlayer>().CanDropSynergyEnergy = true;
		}
	}
	public class AlchemistKnowledge : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			Tooltip = "+ Mysterious potion are slightly better than before";
			StackLimit = 3;
		}
		public override void ResetEffect(Player player) {
			player.GetModPlayer<MysteriousPotionPlayer>().PotionPointAddition += 1 * StackAmount;
		}
	}
	public class Dirt : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
			Tooltip = "+ Having a single dirt in your inventory increase defense by 15" +
				"\nAnd permanent exquisitely stuffed buff";
		}
		public override void ResetEffect(Player player) {
			base.ResetEffect(player);
			if (player.HasItem(ItemID.DirtBlock))
				player.statDefense += 15;
			player.AddBuff(BuffID.WellFed3, 1);
		}
	}
	public class PotionExpert : Perk {
		public override void SetDefaults() {
			textureString = BossRushUtils.GetTheSameTextureAsEntity<PotionExpert>();
			CanBeStack = false;
			Tooltip =
				"+ Potion have 35% to not be consumed";
		}
		public override void ResetEffect(Player player) {
			player.GetModPlayer<PerkPlayer>().perk_PotionExpert = true;
		}
	}
	public class SniperCharge : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
			Tooltip =
				"+ Range weapon can deal 2x damage when it is ready";
		}
		int RandomCountDown = 0;
		int OpportunityWindow = 0;
		public override void Update(Player player) {
			if (!player.ItemAnimationActive)
				RandomCountDown = BossRushUtils.CoolDown(RandomCountDown);
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
			Tooltip =
				"+ When a enemy hit you, you will do self explosion that deal 75 damage to surrounding enemies";
			StackLimit = 5;
		}
		public override void OnHitByAnything(Player player) {
			player.Center.LookForHostileNPC(out List<NPC> npclist, 300);
			foreach (NPC npc in npclist) {
				int direction = player.Center.X - npc.Center.X > 0 ? -1 : 1;
				npc.StrikeNPC(npc.CalculateHitInfo(75 * StackAmount, direction, false, 10));
			}
			for (int i = 0; i < 150; i++) {
				int smokedust = Dust.NewDust(player.Center, 0, 0, DustID.Smoke);
				Main.dust[smokedust].noGravity = true;
				Main.dust[smokedust].velocity = Main.rand.NextVector2Circular(25, 25);
				Main.dust[smokedust].scale = Main.rand.NextFloat(.75f, 2f);
				int dust = Dust.NewDust(player.Center, 0, 0, DustID.Torch);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(25, 25);
				Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
			}
		}
	}
	public class SpecialPotion : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
			Tooltip =
				"+ Grant you 1 random special potion";
		}
		public override void OnChoose(Player player) {
			int type = Main.rand.Next(new int[] { ModContent.ItemType<TitanElixir>(), ModContent.ItemType<BerserkerElixir>(), ModContent.ItemType<GunslingerElixir>(), ModContent.ItemType<CommanderElixir>(), ModContent.ItemType<SageElixir>(), });
			player.QuickSpawnItem(player.GetSource_FromThis(), type);
		}
	}
	public class ProjectileProtection : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			Tooltip =
				"+ You are 15% more resistant to projectile";
			StackLimit = 5;
		}
		public override void OnHitByProjectile(Player player, Projectile proj, Player.HurtInfo hurtInfo) {
			hurtInfo.SourceDamage = (int)(hurtInfo.SourceDamage * (1 - .15f * StackAmount));
		}
	}
	public class ProjectileDuplication : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			Tooltip =
				"+ Your weapon have a chance to shoot out duplicate projectile ( warning : may work weirdly on many weapon due to terraria code )";
			StackLimit = 5;
		}
		public override void Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (Main.rand.NextFloat() <= .1f * StackAmount)
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(10), type, damage, knockback, player.whoAmI);
		}
	}
	public class SpeedArmor : Perk {
		public override void SetDefaults() {
			CanBeStack = true;
			Tooltip =
				"+ Gain 10% movement speed in exchange for -2 defenses";
			StackLimit = 5;
		}
		public override void ResetEffect(Player player) {
			player.moveSpeed += .1f * StackAmount;
			player.statDefense -= 2 * StackAmount;
		}
	}
	public class CelestialRage : Perk {
		public override void SetDefaults() {
			CanBeStack = false;
			Tooltip =
				"+ A gift from celestial being";
		}
		public override void OnChoose(Player player) {
			player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<CelestialWrath>());
		}
	}
	public class BlessingOfSolar : Perk {
		public override void SetDefaults() {
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = true;
			StackLimit = 5;
		}
		public override string ModifyToolTip() {
			if (StackAmount > 0)
				return "+ 78% increased odds for melee" +
				"\n+ 15% thorn damage" +
				"\n+ 2% melee size increases" +
				"\nIncreases 1 armor penetration" +
				"\nHeart appear more when attacking enemy with melee item";
			return
				"+ 78% increased odds for melee" +
				"\n+ 15% thorn damage" +
				"\n+ 2% melee size increases" +
				"\nIncreases 1 armor penetration" +
				"\nAttacking enemy have a 0.5% with melee item have a chance to drop a heart";
		}
		public override void ResetEffect(Player player) {
			player.thorns += .15f * StackAmount;
			player.GetArmorPenetration(DamageClass.Melee) += 2 * StackAmount;
		}
		public override void Update(Player player) {
			player.GetModPlayer<ChestLootDropPlayer>().UpdateMeleeChanceMutilplier += .78f * StackAmount;
		}
		public override void ModifyItemScale(Player player, Item item, ref float scale) {
			if (item.DamageType == DamageClass.Melee)
				scale += .02f * StackAmount;
		}
		public override void OnHitNPCWithItem(Player player, Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			if (Main.rand.NextFloat() <= .005f * StackAmount && (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed)) {
				Item.NewItem(item.GetSource_FromThis(), target.Hitbox, new Item(ItemID.Heart));
			}
		}
	}
	public class BlessingOfVortex : Perk {
		public override void SetDefaults() {
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = true;
			Tooltip =
				"+ 78% increased odds for range" +
				"\n+ 5% range critical strike chance" +
				"\nYou have 0.2% chance to deal 4x damage";
			StackLimit = 5;
		}
		public override string ModifyToolTip() {
			if (StackAmount > 0)
				return "+ 78% increased odds for range" +
				"\n+ 5% range critical strike chance" +
				"\nIncrease chance to deal 4x damage by 1%";
			return
				"+ 78% increased odds for range" +
				"\n+ 5% range critical strike chance" +
				"\nYou have 1% chance to deal 4x damage";
		}
		public override void Update(Player player) {
			player.GetModPlayer<ChestLootDropPlayer>().UpdateRangeChanceMutilplier += .78f * StackAmount;
		}
		public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			base.OnHitNPCWithProj(player, proj, target, hit, damageDone);
			if (Main.rand.NextFloat() <= .002f * StackAmount)
				hit.Damage *= 4;
		}
		public override void ModifyCriticalStrikeChance(Player player, Item item, ref float crit) {
			if (item.DamageType == DamageClass.Ranged)
				crit += 5 * StackAmount;
		}
	}
	public class BlessingOfNebula : Perk {
		public override void SetDefaults() {
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = true;
			Tooltip =
				"+ 78% increased odds for magic" +
				"\n+ 5% magic cost reduction" +
				"\nMana star can spawn from hitting NPC with magic projectile (start at 2.5%)";
			StackLimit = 5;
		}
		public override string ModifyToolTip() {
			//if (StackAmount >= 5)
			//    return "+ 78% increased odds for magic" +
			//    "\n+ 5% magic cost reduction" +
			//    "\nMore Mana star can be spawn from hitting NPC with magic projectile" +
			//    "\nYou will now use 100 mana for each time you uses magic weapon, the mana that consumes will contribute to more damage";
			if (StackAmount > 0)
				return "+ 78% increased odds for magic" +
				"\n+ 5% magic cost reduction" +
				"\nMana star spawn more frequent from hitting NPC with magic projectile";
			return
				"+ 78% increased odds for magic" +
				"\n+ 5% magic cost reduction" +
				"\nMana star can spawn from hitting NPC with magic projectile (start at 2.5%)";
		}
		public override void Update(Player player) {
			player.GetModPlayer<ChestLootDropPlayer>().UpdateMagicChanceMutilplier += .78f * StackAmount;
		}
		public override void ModifyShootStat(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			//if (StackAmount >= 5)
			//{
			//    player.statMana = Math.Clamp(player.statMana - 100, 0, player.statManaMax2);
			//    if (player.statMana >= 100)
			//        damage *= 2;
			//}
		}
		public override void ModifyManaCost(Player player, Item item, ref float reduce, ref float multi) {
			multi -= .05f * StackAmount;
		}
		public override void OnHitNPCWithProj(Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (Main.rand.NextFloat() <= .025f * StackAmount && proj.DamageType == DamageClass.Magic) {
				Item.NewItem(proj.GetSource_FromThis(), target.Hitbox, new Item(ItemID.Star));
			}
		}
	}
	public class BlessingOfStarDust : Perk {
		public override void SetDefaults() {
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = true;
			CanBeChoosen = false;
			Tooltip =
				"+ 78% increased odds for summoner" +
				"\n+ 5% summoner damage" +
				"\n+ 1 minion slot" +
				"\n+ 1 sentry slot";
			StackLimit = 5;
		}
		public override void ResetEffect(Player player) {
			player.maxMinions += 1 * StackAmount;
			player.maxTurrets += 1 * StackAmount;
		}
		public override void Update(Player player) {
			player.GetModPlayer<ChestLootDropPlayer>().UpdateSummonChanceMutilplier += .78f * StackAmount;
		}
		public override void ModifyDamage(Player player, Item item, ref StatModifier damage) {
			if (item.DamageType == DamageClass.Summon || item.DamageType == DamageClass.SummonMeleeSpeed)
				damage += .05f * StackAmount;
		}
	}
	public class BlessingOfPerk : Perk {
		public override void SetDefaults() {
			textureString = BossRushTexture.ACCESSORIESSLOT;
			CanBeStack = true;
			CanBeChoosen = false;
			Tooltip =
				"+Increases perk range amount by 1";
			StackLimit = 999;
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
}
