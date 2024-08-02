using BossRush.Contents.Items.Accessories.EnragedBossAccessories.EvilEye;
using BossRush.Contents.BuffAndDebuff;
using BossRush.Contents.Projectiles;
using System.Collections.Generic;
using BossRush.Common.Systems;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Audio;
using System.Linq;
using Terraria.ID;
using Terraria;
using System;

namespace BossRush.Common.RoguelikeChange {
	/// <summary>
	/// This is where we should modify vanilla item
	/// </summary>
	class RoguelikeItemOverhaul : GlobalItem {
		public override void SetDefaults(Item entity) {
			base.SetDefaults(entity);
			if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul) {
				return;
			}
			VanillaBuff(entity);
			if (entity.type == ItemID.LifeCrystal || entity.type == ItemID.ManaCrystal) {
				entity.autoReuse = true;
			}
		}
		private void VanillaBuff(Item item) {
			switch (item.type) {
				case ItemID.Sandgun:
					item.shoot = ModContent.ProjectileType<SandProjectile>();
					item.damage = 22;
					break;
				case ItemID.Stynger:
					item.useTime = 5;
					item.useAnimation = 40;
					item.reuseDelay = 30;
					item.damage += 10;
					break;
				case ItemID.ToxicFlask:
					item.damage += 5;
					item.useTime = item.useAnimation = 25;
					break;
				case ItemID.BeamSword:
					item.useTime = item.useAnimation;
					item.damage += 5;
					item.crit += 10;
					break;
				case ItemID.TrueNightsEdge:
					item.useTime = item.useAnimation = 25;
					break;
				case ItemID.TrueExcalibur:
					item.damage += 15;
					break;
				case ItemID.TheUndertaker:
					item.autoReuse = true;
					break;
				case ItemID.BoneSword:
					item.damage = 23;
					item.crit = 4;
					item.ArmorPenetration = 5;
					break;
				case ItemID.CopperBow:
				case ItemID.TinBow:
					item.useTime = item.useAnimation = 12;
					break;
				case ItemID.PlatinumBow:
				case ItemID.GoldBow:
					item.useTime = item.useAnimation = 42;
					item.damage += 10;
					item.shootSpeed += 1;
					item.crit += 6;
					break;
				case ItemID.CopperShortsword:
				case ItemID.TinShortsword:
				case ItemID.IronShortsword:
				case ItemID.LeadShortsword:
				case ItemID.SilverShortsword:
				case ItemID.TungstenShortsword:
				case ItemID.GoldShortsword:
				case ItemID.PlatinumShortsword:
					item.crit += 21;
					item.useTime = item.useAnimation = 9;
					break;
				case ItemID.WoodenBow:
				case ItemID.AshWoodBow:
				case ItemID.BorealWoodBow:
				case ItemID.RichMahoganyBow:
				case ItemID.PalmWoodBow:
				case ItemID.EbonwoodBow:
				case ItemID.ShadewoodBow:
					item.useTime = item.useAnimation = 15;
					item.crit += 6;
					break;
				case ItemID.HeatRay:
					item.useTime = item.useAnimation = 4;
					item.mana = 4;
					item.damage = 40;
					break;

			}
		}
		public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul) {
				return;
			}
			if (item.type == ItemID.Stynger) {
				SoundEngine.PlaySound(item.UseSound);
				position += (Vector2.UnitY * Main.rand.NextFloat(-6, 6)).RotatedBy(velocity.ToRotation());
			}
		}
		public override bool AltFunctionUse(Item item, Player player) {
			switch (item.type) {
				case ItemID.CopperShortsword:
				case ItemID.GoldShortsword:
				case ItemID.IronShortsword:
				case ItemID.LeadShortsword:
				case ItemID.PlatinumShortsword:
				case ItemID.SilverShortsword:
				case ItemID.TinShortsword:
				case ItemID.TungstenShortsword:
					return true;
			}
			return base.AltFunctionUse(item, player);
		}
		public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul) {
				return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
			}
			switch (item.type) {
				case ItemID.CopperShortsword:
				case ItemID.GoldShortsword:
				case ItemID.IronShortsword:
				case ItemID.LeadShortsword:
				case ItemID.PlatinumShortsword:
				case ItemID.SilverShortsword:
				case ItemID.TinShortsword:
				case ItemID.TungstenShortsword:
					if (player.altFunctionUse == 2 && !player.GetModPlayer<ThrownShortSwordPlayer>().OnCoolDown) {
						Projectile.NewProjectile(source, position, velocity * 7, ModContent.ProjectileType<ThrowShortSwordProjectile>(), damage, knockback, player.whoAmI, ai2: item.type);
						player.AddBuff(ModContent.BuffType<ThrowShortSwordCoolDown>(), BossRushUtils.ToSecond(3));
						return false;
					}
					return true;
			}
			if (item.type == ItemID.ToxicFlask) {
				GlobalItemPlayer modplayer = player.GetModPlayer<GlobalItemPlayer>();
				if (++modplayer.ToxicFlask_SpecialCounter >= 2) {
					for (int i = 0; i < 3; i++) {
						Vector2 vel = velocity.Vector2DistributeEvenlyPlus(3, 45, i);
						Projectile.NewProjectile(source, position, vel, type, damage, knockback, player.whoAmI);
					}
					modplayer.ToxicFlask_DelayWeaponUse = 60;
					modplayer.ToxicFlask_SpecialCounter = -1;
					return false;
				}
			}
			return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
		}
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul) {
				return;
			}
			Player player = Main.LocalPlayer;
			//We are using name format RoguelikeOverhaul_+ item name
			switch (item.type) {
				case ItemID.CopperShortsword:
				case ItemID.GoldShortsword:
				case ItemID.IronShortsword:
				case ItemID.LeadShortsword:
				case ItemID.PlatinumShortsword:
				case ItemID.SilverShortsword:
				case ItemID.TinShortsword:
				case ItemID.TungstenShortsword:
					TooltipLine line = new TooltipLine(Mod, "RoguelikeOverhaul_ShortSword", "Alt click to throw short sword ( 3s cool down )");
					line.OverrideColor = Color.Yellow;
					tooltips.Add(line);
					break;
			}
			switch (item.type) {
				case ItemID.WoodenBow:
				case ItemID.AshWoodBow:
				case ItemID.BorealWoodBow:
				case ItemID.RichMahoganyBow:
				case ItemID.PalmWoodBow:
				case ItemID.EbonwoodBow:
				case ItemID.ShadewoodBow:
					TooltipLine line = new TooltipLine(Mod, "RoguelikeOverhaul_WoodBow", "Holding the item increases user's movement speed by 15%");
					tooltips.Add(line);
					break;
			}
			if (item.type == ItemID.Sandgun) {
				tooltips.Add(new TooltipLine(Mod, "RoguelikeOverhaul_Sandgun", "Sand projectile no longer spawn upon kill"));
			}
			else if (item.type == ItemID.TheUndertaker) {
				tooltips.Add(new TooltipLine(Mod, "RoguelikeOverhaul_TheUndertaker", "Hitting your shot heal you for 1hp"));
			}
			else if (item.type == ItemID.NightVisionHelmet) {
				tooltips.Add(new TooltipLine(Mod, "RoguelikeOverhaul_NightVisionHelmet", "Increases gun accurancy by 25%"));
			}
			else if (item.type == ItemID.ObsidianRose || item.type == ItemID.ObsidianSkullRose) {
				tooltips.Add(new TooltipLine(Mod, "RoguelikeOverhaul_ObsidianRose", "Grant immunity to OnFire debuff !"));
			}
			else if (item.type == ItemID.VikingHelmet) {
				tooltips.Add(new TooltipLine(Mod, "RoguelikeOverhaul_VikingHelmet",
					"Increases melee damage by 15%" +
					"\nIncreases melee weapon size by 10%"));
			}
		}
		public override void HoldItem(Item item, Player player) {
			switch (item.type) {
				case ItemID.WoodenBow:
				case ItemID.AshWoodBow:
				case ItemID.BorealWoodBow:
				case ItemID.RichMahoganyBow:
				case ItemID.PalmWoodBow:
				case ItemID.EbonwoodBow:
				case ItemID.ShadewoodBow:
					player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.MovementSpeed, 1.15f);
					break;
			}
		}
	}
	public class GlobalItemPlayer : ModPlayer {
		public bool RoguelikeOverhaul_VikingHelmet = false;
		public int ToxicFlask_SpecialCounter = -1;
		public int ToxicFlask_DelayWeaponUse = 0;
		public override void ResetEffects() {
			RoguelikeOverhaul_VikingHelmet = false;
		}
		public override bool CanUseItem(Item item) {
			if (item.type == ItemID.ToxicFlask && ToxicFlask_DelayWeaponUse > 0) {
				return false;
			}
			return base.CanUseItem(item);
		}
		public override void UpdateDead() {
			RoguelikeOverhaul_VikingHelmet = false;
		}

		public override void PostUpdate() {
			if (!Player.ItemAnimationActive) {
				ToxicFlask_DelayWeaponUse = BossRushUtils.CountDown(ToxicFlask_DelayWeaponUse);
			}
		}
		public override void ModifyItemScale(Item item, ref float scale) {
			if (RoguelikeOverhaul_VikingHelmet && item.DamageType == DamageClass.Melee) {
				scale += .1f;
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (item.type == ItemID.WaspGun && !NPC.downedPlantBoss) {
				damage *= .5f;
			}
			if (RoguelikeOverhaul_VikingHelmet && item.DamageType == DamageClass.Melee) {
				damage += .15f;
			}
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			OnHitNPC_WoodBow(proj, target);
			OnHitNPC_TheUnderTaker(proj, target);

		}
		private void OnHitNPC_WoodBow(Projectile proj, NPC target) {
			if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == ItemID.AshWoodBow && Main.rand.NextBool()) {
				target.AddBuff(BuffID.OnFire, BossRushUtils.ToSecond(10));
			}
			if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == ItemID.BorealWoodBow && Main.rand.NextBool()) {
				target.AddBuff(BuffID.Frostburn, BossRushUtils.ToSecond(10));
			}
		}
		private void OnHitNPC_TheUnderTaker(Projectile proj, NPC npc) {
			if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == ItemID.TheUndertaker) {
				Player.Heal(1);
				npc.AddBuff(ModContent.BuffType<CrimsonAbsorbtion>(), 240);
			}
		}
	}
	public class GlobalItemProjectile : GlobalProjectile {
		public override void OnSpawn(Projectile projectile, IEntitySource source) {
			if (projectile.type == ProjectileID.RollingCactusSpike && source is EntitySource_Parent parent && parent.Entity is Projectile parentProjectile) {
				projectile.friendly = parentProjectile.friendly;
				projectile.hostile = parentProjectile.hostile;
			}
		}
	}
	public class GlobalItemMod_GlobalNPC : GlobalNPC {
		public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers) {
			if (npc.HasBuff(ModContent.BuffType<LeadIrradiation>()))
				modifiers.Defense.Base += 20;
		}
	}
	public class ArmorSet {
		public int headID, bodyID, legID;
		protected string ArmorSetBonusToolTip = "";
		public ArmorSet(int headID, int bodyID, int legID) {
			this.headID = headID;
			this.bodyID = bodyID;
			this.legID = legID;
		}
		public static string ConvertIntoArmorSetFormat(int headID, int bodyID, int legID) => $"{headID}:{bodyID}:{legID}";
		/// <summary>
		/// Expect there is only 3 item in a array
		/// </summary>
		/// <param name="armor"></param>
		/// <returns></returns>
		public static string ConvertIntoArmorSetFormat(int[] armor) => $"{armor[0]}:{armor[1]}:{armor[2]}";
		public override string ToString() => $"{headID}:{bodyID}:{legID}";

		public bool ContainAnyOfArmorPiece(int type) => type == headID || type == bodyID || type == legID;
	}
}
