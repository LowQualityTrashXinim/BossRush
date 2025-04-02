using BossRush.Contents.BuffAndDebuff;
using BossRush.Contents.Projectiles;
using System.Collections.Generic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using System;
using BossRush.Common.Systems;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using ReLogic.Content;
using BossRush.Common.RoguelikeChange.ItemOverhaul.AxeOverhaul;
using BossRush.Common.Global;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul {
	/// <summary>
	/// This is where we should modify vanilla item
	/// </summary>
	class RoguelikeItemOverhaul : GlobalItem {
		public override void SetDefaults(Item entity) {
			base.SetDefaults(entity);
			if (!UniversalSystem.Check_RLOH()) {
				return;
			}
			//Attempt to fix item size using texture asset
			if (Main.ActiveWorldFileData.Name != null && entity.IsAWeapon()) {
				Asset<Texture2D> texture = TextureAssets.Item[entity.type];
				if (texture != null) {
					if (texture.State == AssetState.Loaded) {
						entity.width = (int)(texture.Value.Width);
						entity.height = (int)(texture.Value.Height);
					}
				}
			}
			VanillaBuff(entity);
			if (entity.type == ItemID.LifeCrystal || entity.type == ItemID.ManaCrystal || entity.type == ItemID.LifeFruit) {
				entity.useTime = entity.useAnimation = 12;
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
					item.shootSpeed += 3;
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
				case ItemID.HeatRay:
					item.useTime = item.useAnimation = 4;
					item.mana = 4;
					item.damage = 40;
					break;
				case ItemID.AbigailsFlower:
					item.damage += 10;
					break;
				case ItemID.ChainKnife:
					item.damage += 12;
					break;
				case ItemID.ChlorophyteClaymore:
					item.damage += 10;
					item.useTime = item.useAnimation = 35;
					item.shootsEveryUse = true;
					break;
				case ItemID.EnchantedSword:
					item.scale += .5f;
					item.shootsEveryUse = true;
					break;
				case ItemID.IceBlade:
					item.shootsEveryUse = true;
					item.scale += .5f;
					break;
				case ItemID.InfluxWaver:
					item.useTime = item.useAnimation = 30;
					item.shootsEveryUse = true;
					break;
			}
		}
		public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (!UniversalSystem.Check_RLOH()) {
				return;
			}
			switch (item.type) {
				case ItemID.Stynger:
					SoundEngine.PlaySound(item.UseSound);
					position += (Vector2.UnitY * Main.rand.NextFloat(-6, 6)).RotatedBy(velocity.ToRotation());
					break;
				case ItemID.CopperBow:
				case ItemID.TinBow:
					velocity = velocity.Vector2RotateByRandom(5);
					break;
				case ItemID.ChlorophyteClaymore:
					break;
			}
		}
		public override bool AltFunctionUse(Item item, Player player) {
			if (!UniversalSystem.Check_RLOH()) {
				return base.AltFunctionUse(item, player);
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
					return true;
			}
			return base.AltFunctionUse(item, player);
		}
		public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (!UniversalSystem.Check_RLOH()) {
				return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
			}
			GlobalItemPlayer modplayer = player.GetModPlayer<GlobalItemPlayer>();
			if (modplayer.ModeSwitch_Revolver == 1) {
				SoundEngine.PlaySound(item.UseSound);
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
					if (player.altFunctionUse == 2 && !modplayer.ShortSword_OnCoolDown) {
						Projectile.NewProjectile(source, position, velocity * 7, ModContent.ProjectileType<ThrowShortSwordProjectile>(), damage, knockback, player.whoAmI, ai2: item.type);
						player.AddBuff(ModContent.BuffType<ThrowShortSwordCoolDown>(), modplayer.ShortSword_ThrownCD);
						return false;
					}
					return true;
				case ItemID.CopperBow:
				case ItemID.TinBow:
					int counter = 1;
					for (int i = 0; i < counter; i++) {
						if (Main.rand.NextBool(5)) {
							Vector2 newVelocity = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * velocity.Length();
							Projectile.NewProjectile(source, position, newVelocity.Vector2RotateByRandom(10), type, damage, knockback, player.whoAmI);
							counter++;
						}
					}
					return true;
				case ItemID.GoldBow:
				case ItemID.PlatinumBow:
					Projectile projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
					if (ContentSamples.ProjectilesByType[type].arrow) {
						projectile.extraUpdates += 1;
					}
					return false;
				case ItemID.ToxicFlask:
					if (++modplayer.ToxicFlask_SpecialCounter >= 2) {
						for (int i = 0; i < 3; i++) {
							Vector2 vel = velocity.Vector2DistributeEvenlyPlus(3, 45, i);
							Projectile.NewProjectile(source, position, vel, type, damage, knockback, player.whoAmI);
						}
						modplayer.ToxicFlask_DelayWeaponUse = 60;
						modplayer.ToxicFlask_SpecialCounter = -1;
						return false;
					}
					break;
			}
			return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
		}
		public override void UpdateAccessory(Item item, Player player, bool hideVisual) {
			if (item.type == ItemID.CopperWatch) {
				if (player.Center.LookForHostileNPC(out NPC npc, 400f)) {
					npc.GetGlobalNPC<RoguelikeGlobalNPC>().VelocityMultiplier -= .3f;
				}
				player.GetModPlayer<PlayerStatsHandle>().Hostile_ProjectileVelocityAddition -= .3f;
			}
		}
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			if (!UniversalSystem.Check_RLOH()) {
				return;
			}
			Player player = Main.LocalPlayer;
			GlobalItemPlayer modplayer = player.GetModPlayer<GlobalItemPlayer>();
			//We are using name format RoguelikeOverhaul_+ item name
			TooltipLine line;
			switch (item.type) {
				case ItemID.CopperShortsword:
				case ItemID.GoldShortsword:
				case ItemID.IronShortsword:
				case ItemID.LeadShortsword:
				case ItemID.PlatinumShortsword:
				case ItemID.SilverShortsword:
				case ItemID.TinShortsword:
				case ItemID.TungstenShortsword:
					line = new TooltipLine(Mod, "RoguelikeOverhaul_ShortSword", "Alt click to throw short sword ( 1.5s cool down )");
					line.OverrideColor = Color.Yellow;
					tooltips.Add(line);
					break;
				case ItemID.CopperBow:
				case ItemID.TinBow:
					line = new TooltipLine(Mod, "RoguelikeOverhaul_Tier1OreBow", "Have 20% to shoot out additional arrow");
					tooltips.Add(line);
					break;
				case ItemID.WoodenBoomerang:
				case ItemID.EnchantedBoomerang:
				case ItemID.IceBoomerang:
				case ItemID.Flamarang:
				case ItemID.Shroomerang:
					line = new(Mod, "RoguelikeOverhaul_Boomerang", "Reduce enemy's defenses by 10 on hit");
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
			else if (item.type == ItemID.Revolver) {
				string text;
				if (modplayer.ModeSwitch_Revolver == 1) {
					text = $"[c/{Color.Yellow.Hex3()}:Rapid fire Mode]";
				}
				else {
					text = $"[c/{Color.Blue.Hex3()}:Precise fire Mode]";
				}
				tooltips.Add(new TooltipLine(Mod, "RoguelikeOverhaul_Revolver",
					text));
			}
			else if (item.type == ItemID.CopperWatch) {
				tooltips.Add(new TooltipLine(Mod, "RoguelikeOverhaul_CopperWatch", "Decreases the nearest NPC speed by 30%\nDecreases hostile projectile velocity by 30%"));
			}
		}
		public override void HoldItem(Item item, Player player) {
			GlobalItemPlayer modplayer = player.GetModPlayer<GlobalItemPlayer>();
			if (modplayer.ModeSwitch_Revolver == 1) {
				player.GetModPlayer<RangerOverhaulPlayer>().SpreadModify += 1f;
			}
		}
	}
	public class GlobalItemPlayer : ModPlayer {
		public bool RoguelikeOverhaul_VikingHelmet = false;
		public int ToxicFlask_SpecialCounter = -1;
		public int ToxicFlask_DelayWeaponUse = 0;
		/// <summary>
		/// Use this during ResetEffect and after to set your value
		/// </summary>
		public bool WeaponKeyPressed = false;
		/// <summary>
		/// Use this during ResetEffect and after to set your value
		/// </summary>
		public bool WeaponKeyReleased = false;
		/// <summary>
		/// Use this during ResetEffect and after to set your value
		/// </summary>
		public bool WeaponKeyHeld = false;
		public int ReuseDelay = 0;

		public int ModeSwitch_Revolver = 0;

		public int ShortSword_ThrownCD = 90;
		public bool ShortSword_OnCoolDown = false;
		public override void ResetEffects() {
			ShortSword_ThrownCD = 90;
			ShortSword_OnCoolDown = false;
			RoguelikeOverhaul_VikingHelmet = false;
			Item item = Player.HeldItem;
			if (WeaponKeyPressed) {
				if (item.type == ItemID.Revolver) {
					ModeSwitch_Revolver = BossRushUtils.Safe_SwitchValue(ModeSwitch_Revolver, 1);
				}
			}
		}
		public override void ProcessTriggers(TriggersSet triggersSet) {
			WeaponKeyPressed = UniversalSystem.WeaponActionKey.JustPressed;
			WeaponKeyReleased = UniversalSystem.WeaponActionKey.JustReleased;
			WeaponKeyHeld = UniversalSystem.WeaponActionKey.Current;
		}
		public override bool CanUseItem(Item item) {
			if (!UniversalSystem.Check_RLOH()) {
				return base.CanUseItem(item);
			}
			if (item.type == ItemID.ToxicFlask && ToxicFlask_DelayWeaponUse > 0) {
				return false;
			}
			if (ReuseDelay > 0) {
				return false;
			}
			return base.CanUseItem(item);
		}
		public override void UpdateDead() {
			RoguelikeOverhaul_VikingHelmet = false;
		}
		public override float UseTimeMultiplier(Item item) {
			float time = base.UseTimeMultiplier(item);
			if (item.type == ItemID.Revolver && ModeSwitch_Revolver == 1) {
				float useAni = Player.itemAnimationMax;
				float SimulatedUseTime = useAni / 4f;
				return SimulatedUseTime / useAni;
			}
			return base.UseTimeMultiplier(item);
		}
		public override void PostUpdate() {
			if (!Player.ItemAnimationActive) {
				ReuseDelay = BossRushUtils.CountDown(ReuseDelay);
				ToxicFlask_DelayWeaponUse = BossRushUtils.CountDown(ToxicFlask_DelayWeaponUse);
			}
			else {
				Item item = Player.HeldItem;
				if (item.type == ItemID.Revolver && ModeSwitch_Revolver == 1) {
					ReuseDelay = 40;
				}
			}
		}
		public override void ModifyItemScale(Item item, ref float scale) {
			if (!UniversalSystem.Check_RLOH()) {
				return;
			}
			if (RoguelikeOverhaul_VikingHelmet && item.DamageType == DamageClass.Melee) {
				scale += .1f;
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if (!UniversalSystem.Check_RLOH()) {
				return;
			}
			if (item.type == ItemID.WaspGun && !NPC.downedPlantBoss) {
				damage *= .5f;
			}
			if (RoguelikeOverhaul_VikingHelmet && item.DamageType == DamageClass.Melee) {
				damage += .15f;
			}
		}
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (!UniversalSystem.Check_RLOH()) {
				return;
			}
			OnHitNPC_WoodBow(proj, target);
			OnHitNPC_TheUnderTaker(proj, target);
		}
		private void OnHitNPC_WoodBow(Projectile proj, NPC target) {
			if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == ItemID.AshWoodBow && Main.rand.NextBool(4)) {
				target.AddBuff(BuffID.OnFire, BossRushUtils.ToSecond(2));
			}
			if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == ItemID.BorealWoodBow && Main.rand.NextBool(4)) {
				target.AddBuff(BuffID.Frostburn, BossRushUtils.ToSecond(2));
			}
		}
		private void OnHitNPC_TheUnderTaker(Projectile proj, NPC npc) {
			if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == ItemID.TheUndertaker) {
				Player.Heal(1);
				npc.AddBuff(ModContent.BuffType<CrimsonAbsorbtion>(), 240);
			}
		}
		private void OnHitNPC_Boomerang(Projectile proj, NPC npc) {
			if (proj.type == ProjectileID.WoodenBoomerang
				|| proj.type == ProjectileID.IceBoomerang
				|| proj.type == ProjectileID.EnchantedBoomerang
				|| proj.type == ProjectileID.Flamarang
				|| proj.type == ProjectileID.Shroomerang) {
				npc.AddBuff(ModContent.BuffType<Penetrating>(), BossRushUtils.ToSecond(Main.rand.Next(10, 14)));
			}
		}
	}
	public class GlobalItemProjectile : GlobalProjectile {
		public override void OnSpawn(Projectile projectile, IEntitySource source) {
			if (!UniversalSystem.Check_RLOH()) {
				return;
			}
			if (projectile.type == ProjectileID.RollingCactusSpike && source is EntitySource_Parent parent && parent.Entity is Projectile parentProjectile) {
				projectile.friendly = parentProjectile.friendly;
				projectile.hostile = parentProjectile.hostile;
			}
		}
	}
	public class RoguelikeOverhaul_ModSystem : ModSystem {
		public static HashSet<int> ItemThatSubsTo_MeleeOverhaul = new();
		public override void Load() {
			base.Load();
			ItemThatSubsTo_MeleeOverhaul = new();
		}
		public override void Unload() {
			base.Unload();
			ItemThatSubsTo_MeleeOverhaul = null;
		}
		public static bool Optimized_CheckItem(Item item) {
			if (BossRushUtils.CheckUseStyleMelee(item, BossRushUtils.MeleeStyle.CheckOnlyModded)) {
				return true;
			}
			if (item.TryGetGlobalItem(out BattleAxeOverhaul axeItem)) {
				switch (axeItem.UseStyleType) {
					case BossRushUseStyle.DownChop:
						return true;
				}
			}
			return ItemThatSubsTo_MeleeOverhaul.Contains(item.type);
		}
		public override void PostSetupContent() {
			if (UniversalSystem.Check_RLOH()) {
				ItemThatSubsTo_MeleeOverhaul.Clear();
			}
		}
	}
}
