﻿using System;
using Terraria;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Common.RoguelikeChange {
	public class BossRushUseStyle {
		public const int Swipe = 999;
		public const int Poke = 998;
		public const int GenericSwingDownImprove = 990;
	}
	internal class MeleeWeaponOverhaul : GlobalItem {
		public int SwingType = 0;
		public override bool InstancePerEntity => true;
		public override void SetDefaults(Item item) {
			base.SetDefaults(item);
			if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul) {
				return;
			}
			VanillaBuff(item);
			if (item.noMelee) {
				return;
			}
			item.width += 10;
			item.height += 10;

			switch (item.type) {
				#region BossRushUseStyle.Swipe
				//Sword that have even end
				//WoodSword
				case ItemID.PearlwoodSword:
				case ItemID.BorealWoodSword:
				case ItemID.PalmWoodSword:
				case ItemID.ShadewoodSword:
				case ItemID.EbonwoodSword:
				case ItemID.RichMahoganySword:
				case ItemID.WoodenSword:
				case ItemID.CactusSword:
				//OrebroadSword
				case ItemID.BeeKeeper:
				case ItemID.CopperBroadsword:
				case ItemID.TinBroadsword:
				case ItemID.IronBroadsword:
				case ItemID.LeadBroadsword:
				case ItemID.SilverBroadsword:
				case ItemID.TungstenBroadsword:
				case ItemID.GoldBroadsword:
				case ItemID.PlatinumBroadsword:
				//LightSaber
				case ItemID.PurplePhaseblade:
				case ItemID.BluePhaseblade:
				case ItemID.GreenPhaseblade:
				case ItemID.YellowPhaseblade:
				case ItemID.OrangePhaseblade:
				case ItemID.RedPhaseblade:
				case ItemID.WhitePhaseblade:
				//Saber
				case ItemID.PurplePhasesaber:
				case ItemID.BluePhasesaber:
				case ItemID.GreenPhasesaber:
				case ItemID.YellowPhasesaber:
				case ItemID.OrangePhasesaber:
				case ItemID.RedPhasesaber:
				case ItemID.WhitePhasesaber:
				//Misc PreHM sword
				case ItemID.PurpleClubberfish:
				case ItemID.StylistKilLaKillScissorsIWish:
				case ItemID.BladeofGrass:
				case ItemID.FieryGreatsword:
				case ItemID.LightsBane:
				//HardmodeSword
				case ItemID.CobaltSword:
				case ItemID.MythrilSword:
				case ItemID.AdamantiteSword:
				case ItemID.PalladiumSword:
				case ItemID.OrichalcumSword:
				case ItemID.TitaniumSword:
				case ItemID.Excalibur:
				case ItemID.TheHorsemansBlade:
				case ItemID.Bladetongue:
				case ItemID.DD2SquireDemonSword:
				//Sword That shoot projectile
				case ItemID.BeamSword:
				case ItemID.EnchantedSword:
				case ItemID.Starfury:
				case ItemID.InfluxWaver:
				case ItemID.ChlorophyteClaymore:
				case ItemID.ChlorophyteSaber:
				case ItemID.ChristmasTreeSword:
				case ItemID.TrueExcalibur:
					SwingType = BossRushUseStyle.Swipe;
					item.useTurn = false;
					break;
				//Poke Sword
				//Pre HM Sword
				case ItemID.DyeTradersScimitar:
				case ItemID.CandyCaneSword:
				case ItemID.Muramasa:
				case ItemID.BloodButcherer:
				case ItemID.NightsEdge:
				case ItemID.Katana:
				case ItemID.FalconBlade:
				case ItemID.BoneSword:
				//HM sword
				case ItemID.IceBlade:
				case ItemID.BreakerBlade:
				case ItemID.Frostbrand:
				case ItemID.Cutlass:
				case ItemID.Seedler:
				case ItemID.TrueNightsEdge:
				case ItemID.TerraBlade:
				case ItemID.Meowmere:
				case ItemID.StarWrath:
					SwingType = BossRushUseStyle.Poke;
					item.useTurn = false;
					break;
				case ItemID.DD2SquireBetsySword:
				case ItemID.ZombieArm:
				case ItemID.BatBat:
				case ItemID.TentacleSpike:
				case ItemID.SlapHand:
				case ItemID.Keybrand:
				case ItemID.AntlionClaw:
				case ItemID.HamBat:
				case ItemID.PsychoKnife:
					SwingType = BossRushUseStyle.GenericSwingDownImprove;
					item.useTurn = false;
					break;
				#endregion
				default:
					break;
			}
		}
		private void VanillaBuff(Item item) {
			if (item.type == ItemID.TrueNightsEdge) {
				item.useTime = item.useAnimation = 25;
			}
		}
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			if (!ModContent.GetInstance<BossRushModConfig>().RoguelikeOverhaul) {
				return;
			}
			if (SwingType == BossRushUseStyle.GenericSwingDownImprove) {
				TooltipLine line = new TooltipLine(Mod, "SwingImprove", "Sword can swing in all direction");
				line.OverrideColor = Color.LightYellow;
				tooltips.Add(line);
			}
			if (SwingType == BossRushUseStyle.Swipe || SwingType == BossRushUseStyle.Poke) {
				TooltipLine line = new TooltipLine(Mod, "SwingImproveCombo", "Sword can swing in all direction" +
					"\nHold down right mouse to make your normal slash push you back and allow you to dash toward the direction you are facing on 3rd attack");
				line.OverrideColor = Color.Yellow;
				tooltips.Add(line);
				if (SwingType == BossRushUseStyle.Swipe) {
					TooltipLine line2 = new TooltipLine(Mod, "SwingImproveCombo", "3rd attack deal 65% more damage");
					line2.OverrideColor = Color.Yellow;
					tooltips.Add(line2);
				}
				if (SwingType == BossRushUseStyle.Poke) {
					TooltipLine line2 = new TooltipLine(Mod, "SwingImproveCombo",
						"Heavy attack deal 55% more damage (Activate by alt attack)" +
						"\nThurst attack deal 25% more damage");
					line2.OverrideColor = Color.Yellow;
					tooltips.Add(line2);
				}
			}
		}
		public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox) {
			if (item.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModded)) {
				BossRushUtils.ModifyProjectileDamageHitbox(ref hitbox, player, item.width, item.height);
			}
		}
		public override void ModifyItemScale(Item item, Player player, ref float scale) {
			if (item.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModded)) {
				int duration = player.itemAnimationMax;
				float thirdduration = duration / 3;
				float progress;
				if (player.itemAnimation < thirdduration) {
					progress = player.itemAnimation / thirdduration;
				}
				else {
					progress = (duration - player.itemAnimation) / thirdduration;
				}
				scale += MathHelper.SmoothStep(-.5f, .25f, progress);
			}
		}
		public override bool CanUseItem(Item item, Player player) {
			if ((SwingType != BossRushUseStyle.Swipe &&
				SwingType != BossRushUseStyle.Poke) ||
				item.noMelee) {
				return base.CanUseItem(item, player);
			}
			return player.GetModPlayer<MeleeOverhaulPlayer>().delaytimer <= 0;
		}
		public override float UseSpeedMultiplier(Item item, Player player) {
			if (SwingType != BossRushUseStyle.Swipe &&
				SwingType != BossRushUseStyle.Poke ||
				item.noMelee) {
				return base.UseSpeedMultiplier(item, player);
			}
			float useTimeMultiplierOnCombo = base.UseSpeedMultiplier(item, player) - .15f;
			MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
			//This combo count is delay and because of so, we have to do set back, so swing number 1 = 0
			if (SwingType == BossRushUseStyle.Swipe) {
				if (modPlayer.ComboNumber == 2) {
					return useTimeMultiplierOnCombo -= .25f;
				}
			}
			if (SwingType == BossRushUseStyle.Poke) {
				if (modPlayer.ComboNumber == 2) {
					return useTimeMultiplierOnCombo -= .25f;
				}
				if (Main.mouseRight) {
					return useTimeMultiplierOnCombo -= .5f;
				}
			}
			return useTimeMultiplierOnCombo;
		}
		public override void UseStyle(Item item, Player player, Rectangle heldItemFrame) {
			if (!item.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModded) || item.noMelee) {
				return;
			}
			MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
			modPlayer.CountDownToResetCombo = (int)(player.itemAnimationMax * 1.35f);
			switch (SwingType) {
				case BossRushUseStyle.Swipe:
					switch (modPlayer.ComboNumber) {
						case 0:
							SwipeAttack(player, modPlayer, 1);
							break;
						case 1:
							SwipeAttack(player, modPlayer, -1);
							break;
						case 2:
							CircleSwingAttack(player, modPlayer);
							break;
					}
					break;
				case BossRushUseStyle.Poke:
					switch (modPlayer.ComboNumber) {
						case 0:
							if (Main.mouseRight)
								WideSwingAttack(player, modPlayer, 1);
							else
								SwipeAttack(player, modPlayer, 1);
							break;
						case 1:
							if (Main.mouseRight)
								WideSwingAttack(player, modPlayer, -1);
							else
								SwipeAttack(player, modPlayer, -1);
							break;
						case 2:
							StrongThrust(player, modPlayer);
							break;
					}
					break;
				case BossRushUseStyle.GenericSwingDownImprove:
					SwipeAttack(player, modPlayer, 1);
					break;
				default:
					break;
			}
		}
		private void StrongThrust(Player player, MeleeOverhaulPlayer modPlayer) {
			float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
			Poke2(player, modPlayer, percentDone);
		}
		private void Poke2(Player player, MeleeOverhaulPlayer modPlayer, float percentDone) {
			float rotation = player.GetModPlayer<BossRushUtilsPlayer>().MouseLastPositionBeforeAnimation.ToRotation();
			Vector2 poke = Vector2.SmoothStep(modPlayer.data * 30f, modPlayer.data, percentDone).RotatedBy(rotation);
			player.itemRotation = modPlayer.data.ToRotation();
			player.itemRotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f;
			player.compositeFrontArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Full, poke.ToRotation() - MathHelper.PiOver2);
			player.itemLocation = player.Center + poke - poke.SafeNormalize(Vector2.Zero) * 20f;
		}
		private void WideSwingAttack(Player player, MeleeOverhaulPlayer modPlayer, int direct) {
			float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
			percentDone = BossRushUtils.InOutExpo(percentDone);
			float baseAngle = modPlayer.data.ToRotation();
			float angle = MathHelper.ToRadians(baseAngle + 145) * player.direction;
			float start = baseAngle + angle * direct;
			float end = baseAngle - angle * direct;
			Swipe(start, end, percentDone, player, direct);
		}
		private void SwipeAttack(Player player, MeleeOverhaulPlayer modPlayer, int direct) {
			float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
			//percentDone = BossRushUtils.InOutExpo(percentDone);
			float baseAngle = modPlayer.data.ToRotation();
			float angle = MathHelper.ToRadians(baseAngle + 120) * player.direction;
			float start = baseAngle + angle * direct;
			float end = baseAngle - angle * direct;
			Swipe(start, end, percentDone, player, direct);
		}
		private void CircleSwingAttack(Player player, MeleeOverhaulPlayer modPlayer) {
			float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
			float baseAngle = modPlayer.data.ToRotation() - MathHelper.Pi;
			float start = baseAngle + (MathHelper.Pi + MathHelper.PiOver2) * player.direction;
			float end = baseAngle - MathHelper.TwoPi * player.direction;
			Swipe(start, end, percentDone, player, 1);
		}
		private void Swipe(float start, float end, float percentDone, Player player, int direct) {
			float currentAngle = MathHelper.SmoothStep(start, end, percentDone);
			MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
			player.itemRotation = currentAngle;
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, currentAngle - MathHelper.PiOver2);
			if (SwingType == BossRushUseStyle.Poke) {
				player.itemRotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3;
				if (direct == -1) {
					modPlayer.CustomItemRotation = currentAngle;
					modPlayer.CustomItemRotation += player.direction > 0 ? MathHelper.PiOver4 * 3 : MathHelper.PiOver4;
				}
				player.itemLocation = player.Center + Vector2.UnitX.RotatedBy(currentAngle) * BossRushUtilsPlayer.PLAYERARMLENGTH;
			}
			else {
				player.itemRotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3;
				player.itemLocation = player.Center + Vector2.UnitX.RotatedBy(currentAngle) * BossRushUtilsPlayer.PLAYERARMLENGTH;
			}
		}
	}
	public class MeleeOverhaulSystem : ModSystem {
		public override void Load() {
			base.Load();
			On_PlayerDrawLayers.DrawPlayer_RenderAllLayers += On_PlayerDrawLayers_DrawPlayer_RenderAllLayers;
		}

		private void On_PlayerDrawLayers_DrawPlayer_RenderAllLayers(On_PlayerDrawLayers.orig_DrawPlayer_RenderAllLayers orig, ref PlayerDrawSet drawinfo) {
			Player player = Main.LocalPlayer;
			Item item = player.HeldItem;
			if (player.TryGetModPlayer(out MeleeOverhaulPlayer modplayer)) {
				if (item.TryGetGlobalItem(out MeleeWeaponOverhaul meleeItem)) {
					if (modplayer.ComboNumber == 1 && meleeItem.SwingType == BossRushUseStyle.Poke) {
						for (int i = 0; i < drawinfo.DrawDataCache.Count; i++) {
							if (drawinfo.DrawDataCache[i].texture == TextureAssets.Item[item.type].Value) {
								DrawData drawdata = drawinfo.DrawDataCache[i];
								Vector2 origin = drawdata.texture.Size() * .5f;
								drawdata.sourceRect = null;
								drawdata.ignorePlayerRotation = true;
								drawdata.rotation = modplayer.CustomItemRotation;
								drawdata.position += Vector2.UnitX.RotatedBy(modplayer.CustomItemRotation) * (origin.Length() * drawdata.scale.X + BossRushUtilsPlayer.PLAYERARMLENGTH) * -player.direction;
								drawinfo.DrawDataCache[i] = drawdata;
							}
						}
					}
				}
			}
			orig.Invoke(ref drawinfo);

		}
	}
	public class MeleeOverhaulPlayer : ModPlayer {
		public Vector2 data;
		public int ComboNumber = 0;
		public int delaytimer = 10;
		public int oldHeldItem;
		public int CountDownToResetCombo = 0;
		public int MouseXPosDirection = 1;
		bool InStateOfSwinging = false;
		Vector2 positionToDash = Vector2.Zero;
		Vector2 lastPlayerPositionBeforeAnimation = Vector2.Zero;
		bool IsAlreadyHeldDown = false;
		public float CustomItemRotation = 0;
		public override void PreUpdate() {
			Item item = Player.HeldItem;
			if (oldHeldItem != item.type) {
				oldHeldItem = item.type;
				ComboNumber = 0;
				CountDownToResetCombo = 0;
			}
			delaytimer = BossRushUtils.CoolDown(delaytimer);
			CountDownToResetCombo = BossRushUtils.CoolDown(CountDownToResetCombo);
			if (CountDownToResetCombo <= 0)
				ComboNumber = 0;
			if (!item.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModdedWithoutDefault) || item.noMelee) {
				return;
			}
			MouseXPosDirection = Main.MouseWorld.X - Player.MountedCenter.X > 0 ? 1 : -1;
			if (Main.mouseRight) {
				for (int i = 0; i < 4; i++) {
					int dust = Dust.NewDust(positionToDash, 0, 0, DustID.GemRuby);
					Main.dust[dust].velocity = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i)) * Main.rand.NextFloat(2.5f, 4f);
					Main.dust[dust].noGravity = true;
				}
			}
			if (Player.ItemAnimationJustStarted) {
				if (delaytimer <= 0) {
					delaytimer = (int)(Player.itemAnimationMax * 1.2f);
				}
			}
			if (Player.ItemAnimationActive) {
				if (IsAlreadyHeldDown && Main.mouseRight) {
					ExecuteSpecialComboOnActive(item);
				}
				else {
					IsAlreadyHeldDown = false;
				}
				InStateOfSwinging = true;
			}
			else {
				IsAlreadyHeldDown = Main.mouseRight;
				if (InStateOfSwinging) {
					InStateOfSwinging = false;
					if (!ComboConditionChecking()) {
						lastPlayerPositionBeforeAnimation.LookForHostileNPC(out List<NPC> npclist, 500);
						foreach (NPC target in npclist) {
							float point = 0;
							bool collide = Collision.CheckAABBvLineCollision(target.Hitbox.TopLeft(), target.Hitbox.Size(), lastPlayerPositionBeforeAnimation, positionToDash, 22, ref point);
							if (collide) {
								Player.StrikeNPCDirect(target, target.CalculateHitInfo(Player.HeldItem.damage, MouseXPosDirection, false, 1f, DamageClass.Melee, true, Player.luck));
							}
						}
					}
					ComboHandleSystem();
				}
				CanPlayerBeDamage = true;
			}
		}
		public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo) {
			base.ModifyDrawInfo(ref drawInfo);
			Item item = Player.HeldItem;
			if (item.TryGetGlobalItem(out MeleeWeaponOverhaul meleeItem)) {
				if (ComboNumber == 1 && meleeItem.SwingType == BossRushUseStyle.Poke) {
					if (Player.direction == -1) {
						drawInfo.itemEffect = SpriteEffects.None;
					}
					else {
						drawInfo.itemEffect = SpriteEffects.FlipHorizontally;
					}
				}
			}
		}
		private void ExecuteSpecialComboOnActive(Item item) {
			if (ComboConditionChecking()) {
				return;
			}
			Player.noFallDmg = true;
			CanPlayerBeDamage = false;
			float percentage = (Player.itemAnimationMax - Player.itemAnimation) / (float)Player.itemAnimationMax;
			if (item.TryGetGlobalItem(out MeleeWeaponOverhaul meleeItem)) {
				if (meleeItem.SwingType == BossRushUseStyle.Poke)
					percentage = BossRushUtils.OutExpo(percentage);

			}
			Player.Center = Vector2.Lerp(lastPlayerPositionBeforeAnimation, positionToDash, percentage);
		}
		private bool ComboConditionChecking() =>
			Player.mount.Active
			|| ComboNumber != 2
			|| !Main.mouseRight;
		public override bool? CanMeleeAttackCollideWithNPC(Item item, Rectangle meleeAttackHitbox, NPC target) {
			return base.CanMeleeAttackCollideWithNPC(item, meleeAttackHitbox, target);
		}
		private void ComboHandleSystem() {
			if (++ComboNumber >= 3)
				ComboNumber = 0;
		}
		bool CanPlayerBeDamage = true;
		public override void PostUpdate() {
			Item item = Player.HeldItem;
			if (!item.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModded) || item.noMelee) {
				return;
			}
			if (Player.ItemAnimationJustStarted) {
				JustHitANPC = false;
				if (delaytimer == 0) {
					data = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.Zero);
				}
			}
			if (Player.ItemAnimationActive) {
				Player.direction = data.X > 0 ? 1 : -1;
			}
			else {
				lastPlayerPositionBeforeAnimation = Player.Center;
				positionToDash = Player.Center.PositionOffsetDynamic(Main.MouseWorld - Player.Center, 500f);
			}
			Player.attackCD = 0;
			for (int i = 0; i < Player.meleeNPCHitCooldown.Length; i++) {
				if (Player.meleeNPCHitCooldown[i] > 0) {
					Player.meleeNPCHitCooldown[i]--;
				}
			}
		}
		public override void ModifyWeaponKnockback(Item item, ref StatModifier knockback) {
			if (!item.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModdedWithoutDefault) || item.noMelee) {
				return;
			}
			if (ComboNumber != 2 && Main.mouseRight && !JustHitANPC) {
				knockback *= 1.5f;
			}
		}
		bool JustHitANPC = false;
		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			if (!item.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModded) || item.noMelee) {
				return;
			}
			if (ComboNumber != 2 && Main.mouseRight && !JustHitANPC) {
				Player.velocity += (Player.Center - Main.MouseWorld).SafeNormalize(Vector2.Zero) * Player.GetWeaponKnockback(item);
				JustHitANPC = true;
			}
		}
		public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
			if (!item.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModded) || item.noMelee) {
				return;
			}
			modifiers.FinalDamage += DamageHandleSystem(item);
		}
		private float DamageHandleSystem(Item item) {
			if (item.TryGetGlobalItem(out MeleeWeaponOverhaul meleeItem)) {
				if (meleeItem.SwingType == BossRushUseStyle.Swipe && ComboNumber == 2) {
					return .65f;
				}
				if (meleeItem.SwingType == BossRushUseStyle.Poke) {
					if (ComboNumber == 0) {
						return .55f;
					}
					if (ComboNumber == 2) {
						return .25f;
					}
				}
			}
			return 0;
		}
		public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable) {
			Item item = Player.HeldItem;
			if (!item.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModded) ||
				item.noMelee) {
				return base.ImmuneTo(damageSource, cooldownCounter, dodgeable);
			}
			return !CanPlayerBeDamage;
		}
	}
}
