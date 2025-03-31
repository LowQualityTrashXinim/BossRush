using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using BossRush.Common.General;
using BossRush.Common.Systems;
using System;
using System.Linq.Expressions;
using ReLogic.Content;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul {
	public class BossRushUseStyle {
		public const int Swipe = 999;
		public const int Poke = 998;
		public const int DownChop = 997;
		public const int Spin = 996;
		public const int GenericSwingDownImprove = 990;
	}
	internal class MeleeWeaponOverhaul : GlobalItem {
		public int SwingType = 0;
		public float offset = 0;
		public override bool InstancePerEntity => true;
		public override void SetStaticDefaults() {
			if (!UniversalSystem.Check_RLOH()) {
				return;
			}
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.PearlwoodSword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.BorealWoodSword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.PalmWoodSword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.ShadewoodSword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.EbonwoodSword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.RichMahoganySword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.WoodenSword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.CactusSword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.BeeKeeper] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.CopperBroadsword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.TinBroadsword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.IronBroadsword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.LeadBroadsword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.SilverBroadsword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.TungstenBroadsword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.GoldBroadsword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.PlatinumBroadsword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.PurplePhaseblade] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.BluePhaseblade] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.GreenPhaseblade] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.YellowPhaseblade] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.OrangePhaseblade] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.RedPhaseblade] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.WhitePhaseblade] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.PurplePhasesaber] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.BluePhasesaber] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.GreenPhasesaber] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.YellowPhasesaber] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.OrangePhasesaber] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.RedPhasesaber] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.WhitePhasesaber] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.PurpleClubberfish] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.StylistKilLaKillScissorsIWish] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.BladeofGrass] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.FieryGreatsword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.LightsBane] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.MythrilSword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.AdamantiteSword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.OrichalcumSword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.TitaniumSword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.Excalibur] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.TheHorsemansBlade] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.Bladetongue] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.DD2SquireDemonSword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.BeamSword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.EnchantedSword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.Starfury] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.InfluxWaver] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.ChlorophyteClaymore] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.ChlorophyteSaber] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.ChristmasTreeSword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.CandyCaneSword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.Muramasa] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.DyeTradersScimitar] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.BloodButcherer] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.Katana] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.FalconBlade] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.BoneSword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.CobaltSword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.PalladiumSword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.IceBlade] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.BreakerBlade] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.Frostbrand] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.Cutlass] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.Seedler] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.DD2SquireBetsySword] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.ZombieArm] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.BatBat] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.TentacleSpike] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.SlapHand] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.Keybrand] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.AntlionClaw] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.HamBat] = .45f;
			//ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.PsychoKnife] = .45f;
		}
		public override void SetDefaults(Item item) {
			if (!UniversalSystem.Check_RLOH()) {
				return;
			}
			if (item.noMelee) {
				return;
			}
			//switch (item.type) {
			//	case ItemID.WoodenSword:
			//	case ItemID.BorealWoodSword:
			//	case ItemID.RichMahoganySword:
			//	case ItemID.PalmWoodSword:
			//	case ItemID.EbonwoodSword:
			//	case ItemID.ShadewoodSword:
			//	case ItemID.PearlwoodSword:
			//	case ItemID.AshWoodSword:
			//		item.width = item.height = 32;
			//		break;
			//	case ItemID.BluePhaseblade:
			//	case ItemID.RedPhaseblade:
			//	case ItemID.GreenPhaseblade:
			//	case ItemID.PurplePhaseblade:
			//	case ItemID.OrangePhaseblade:
			//	case ItemID.YellowPhaseblade:
			//	case ItemID.WhitePhaseblade:
			//		item.width = item.height = 48;
			//		break;
			//	case ItemID.BluePhasesaber:
			//	case ItemID.RedPhasesaber:
			//	case ItemID.GreenPhasesaber:
			//	case ItemID.PurplePhasesaber:
			//	case ItemID.OrangePhasesaber:
			//	case ItemID.YellowPhasesaber:
			//	case ItemID.WhitePhasesaber:
			//		item.width = item.height = 56;
			//		break;
			//	case ItemID.CopperBroadsword:
			//	case ItemID.TinBroadsword:
			//	case ItemID.LeadBroadsword:
			//	case ItemID.IronBroadsword:
			//	case ItemID.SilverBroadsword:
			//	case ItemID.TungstenBroadsword:
			//	case ItemID.GoldBroadsword:
			//	case ItemID.PlatinumBroadsword:
			//		item.width = item.height = 46;
			//		break;
			//	case ItemID.CobaltSword:
			//		item.width = 56;
			//		item.height = 58;
			//		break;
			//	case ItemID.PalladiumSword:
			//		item.width = 50;
			//		item.height = 60;
			//		break;
			//	case ItemID.MythrilSword:
			//		item.width = item.height = 58;
			//		break;
			//	case ItemID.OrichalcumSword:
			//		item.width = item.height = 54;
			//		break;
			//	case ItemID.AdamantiteSword:
			//	case ItemID.TitaniumSword:
			//		item.width = item.height = 60;
			//		break;
			//	case ItemID.Muramasa:
			//		item.width = 50;
			//		item.height = 64;
			//		offset += 12;
			//		break;
			//	case ItemID.LightsBane:
			//		item.width = item.height = 50;
			//		break;
			//	case ItemID.BloodButcherer:
			//		item.width = 50;
			//		item.height = 58;
			//		break;
			//	case ItemID.BladeofGrass:
			//		item.width = item.height = 70;
			//		break;
			//	case ItemID.FieryGreatsword:
			//		item.width = 84;
			//		item.height = 84;
			//		break;
			//	case ItemID.TheHorsemansBlade:
			//		item.width = item.height = 54;
			//		break;
			//	case ItemID.Frostbrand:
			//		item.width = 50;
			//		item.height = 58;
			//		break;
			//	case ItemID.CactusSword:
			//		item.width = item.height = 48;
			//		break;
			//	case ItemID.BeamSword:
			//		item.width = item.height = 52;
			//		break;
			//	case ItemID.Meowmere:
			//		item.width = 50;
			//		item.height = 58;
			//		break;
			//	case ItemID.Starfury:
			//		item.width = item.height = 42;
			//		break;
			//	case ItemID.StarWrath:
			//		item.width = 46;
			//		item.height = 54;
			//		break;
			//	case ItemID.BatBat:
			//		item.width = item.height = 52;
			//		break;
			//	case ItemID.TentacleSpike:
			//		item.width = 44;
			//		item.height = 40;
			//		break;
			//	case ItemID.InfluxWaver:
			//		item.width = item.height = 50;
			//		break;
			//	case ItemID.Seedler:
			//		item.width = 48;
			//		item.height = 68;
			//		break;
			//	case ItemID.Keybrand:
			//		item.width = 58;
			//		item.height = 62;
			//		break;
			//	case ItemID.ChlorophyteSaber:
			//		item.width += 10;
			//		item.height += 10;
			//		break;
			//	case ItemID.BreakerBlade:
			//		item.width = 80;
			//		item.height = 92;
			//		break;
			//	case ItemID.BoneSword:
			//		item.width = item.height = 50;
			//		break;
			//	case ItemID.ChlorophyteClaymore:
			//		item.width = item.height = 68;
			//		break;
			//	case ItemID.Bladetongue:
			//		item.width = item.height = 50;
			//		break;
			//	case ItemID.DyeTradersScimitar:
			//		item.width = 40;
			//		item.height = 48;
			//		break;
			//	case ItemID.BeeKeeper:
			//		item.width = item.height = 44;
			//		break;
			//	case ItemID.EnchantedSword:
			//		item.width = item.height = 34;
			//		break;
			//	case ItemID.ZombieArm:
			//		item.width = 38;
			//		item.height = 40;
			//		break;
			//	case ItemID.FalconBlade:
			//		item.width = 36;
			//		item.height = 40;
			//		break;
			//	case ItemID.Cutlass:
			//		item.width = 40;
			//		item.height = 48;
			//		break;
			//	case ItemID.CandyCaneSword:
			//		item.width = 44;
			//		item.height = 75;
			//		break;
			//	case ItemID.IceBlade:
			//		item.width = 38;
			//		item.height = 34;
			//		break;
			//	case ItemID.HamBat:
			//		item.width = 44;
			//		item.height = 40;
			//		break;
			//	case ItemID.DD2SquireBetsySword:
			//		item.width = 66; item.height = 66;
			//		break;
			//	case ItemID.PurpleClubberfish:
			//		item.width = item.height = 50;
			//		break;
			//	case ItemID.AntlionClaw:
			//		item.width = item.height = 32;
			//		break;
			//	case ItemID.Katana:
			//		item.width = 48;
			//		item.height = 54;
			//		break;
			//	case ItemID.DD2SquireDemonSword:
			//	case ItemID.ChristmasTreeSword:
			//		item.width = item.height = 60;
			//		break;
			//}
			switch (item.type) {
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
				case ItemID.AshWoodSword:
				//OrebroadSword
				case ItemID.BeeKeeper:
				case ItemID.CopperBroadsword:
				case ItemID.TinBroadsword:
				case ItemID.IronBroadsword:
				case ItemID.LeadBroadsword:
				case ItemID.SilverBroadsword:
				case ItemID.TungstenBroadsword:
				case ItemID.GoldBroadsword:
				case ItemID.Flymeal:
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
				case ItemID.MythrilSword:
				case ItemID.AdamantiteSword:
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
					SwingType = BossRushUseStyle.Swipe;
					item.useTurn = false;
					item.Set_ItemCriticalDamage(1f);
					break;
				//Poke Sword
				//Pre HM Sword
				case ItemID.DyeTradersScimitar:
				case ItemID.CandyCaneSword:
				case ItemID.Muramasa:
				case ItemID.BloodButcherer:
				case ItemID.Katana:
				case ItemID.FalconBlade:
				case ItemID.BoneSword:
				//HM sword
				case ItemID.CobaltSword:
				case ItemID.PalladiumSword:
				case ItemID.IceBlade:
				case ItemID.BreakerBlade:
				case ItemID.Frostbrand:
				case ItemID.Cutlass:
				case ItemID.Seedler:
					SwingType = BossRushUseStyle.Poke;
					item.useTurn = false;
					item.Set_ItemCriticalDamage(1f);
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
					item.Set_ItemCriticalDamage(1f);
					break;
				default:
					break;
			}
		}
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			if (!UniversalSystem.Check_RLOH()) {
				return;
			}
			if (SwingType == BossRushUseStyle.GenericSwingDownImprove || SwingType == BossRushUseStyle.Swipe || SwingType == BossRushUseStyle.Poke) {
				TooltipLine line = new TooltipLine(Mod, "SwingImprove", "Sword can swing in all direction");
				line.OverrideColor = Color.LightYellow;
				tooltips.Add(line);
			}
		}
		public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox) {
			//Since we are using entirely new collision detection, we no longer need this
			if (item.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModded)) {
				BossRushUtils.ModifyProjectileDamageHitbox(ref hitbox, player, item.width, item.height);
			}
		}
		public override bool? CanMeleeAttackCollideWithNPC(Item item, Rectangle meleeAttackHitbox, Player player, NPC target) {
			if (item.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModded)) {
				float extra = 0;
				if (target.boss) {
					extra += .25f;
				}
				Asset<Texture2D> texture = TextureAssets.Item[item.type];
				float itemlength = texture.Value.Size().Length() * .9f;
				float itemsize = itemlength * (player.GetAdjustedItemScale(player.HeldItem) + extra);
				int laserline = (int)itemsize;
				if (laserline <= 0) {
					laserline = 1;
				}
				MeleeOverhaulPlayer modplayer = player.GetModPlayer<MeleeOverhaulPlayer>();
				BossRushUtilsPlayer utilsplayer = player.GetModPlayer<BossRushUtilsPlayer>();
				if (modplayer.ComboNumber != 2) {
					Vector2 offset = player.Center - utilsplayer.PlayerLastPositionBeforeAnimation;
					Vector2 directionTo = (player.GetModPlayer<BossRushUtilsPlayer>().MouseLastPositionBeforeAnimation + offset - player.Center).SafeNormalize(Vector2.Zero);
					bool checkComboNum = modplayer.ComboNumber == 0;
					int LastCollideCheck, check;
					if (checkComboNum && player.direction == 1 || !checkComboNum && player.direction == -1) {
						LastCollideCheck =
							(int)Math.Ceiling(MathHelper.Lerp(0, laserline, BossRushUtils.InExpo((player.itemAnimation + 1) / (float)player.itemAnimationMax, 11f)));
						check =
							(int)Math.Ceiling(MathHelper.Lerp(0, laserline, BossRushUtils.InExpo(player.itemAnimation / (float)player.itemAnimationMax, 11f)));
					}
					else {
						LastCollideCheck =
							(int)Math.Ceiling(MathHelper.Lerp(laserline, 0, BossRushUtils.InExpo((player.itemAnimation + 1) / (float)player.itemAnimationMax, 11f)));
						check =
							(int)Math.Ceiling(MathHelper.Lerp(laserline, 0, BossRushUtils.InExpo(player.itemAnimation / (float)player.itemAnimationMax, 11f)));
					}
					if (player.itemAnimationMax <= 2) {
						for (int i = 0; i <= laserline; i++) {
							Vector2 point = player.Center + directionTo.Vector2DistributeEvenly(laserline, 270, i) * itemsize;
							if (BossRushUtils.Collision_PointAB_EntityCollide(target.Hitbox, player.Center, point)) {
								return true;
							}
						}
						return false;
					}
					int assigned = Math.Min(LastCollideCheck, check);
					int length = Math.Max(check, LastCollideCheck);
					for (int i = assigned; i <= length; i++) {
						Vector2 point = player.Center + directionTo.Vector2DistributeEvenly(laserline, 270, i) * itemsize;
						if (BossRushUtils.Collision_PointAB_EntityCollide(target.Hitbox, player.Center, point)) {
							return true;
						}
					}
					return false;
					//Mod.Logger.Debug($"Frame : {player.itemAnimation} | prev {previousAnimationFrame} | Check : {checkoutside} | prev {LastCollideCheck}");
				}
			}
			return base.CanMeleeAttackCollideWithNPC(item, meleeAttackHitbox, player, target);
		}
		public override float UseSpeedMultiplier(Item item, Player player) {
			float SpeedAdd = 0;
			if (!player.autoReuseAllWeapons) {
				SpeedAdd += .11f;
			}
			if (SwingType != BossRushUseStyle.Swipe &&
				SwingType != BossRushUseStyle.Poke ||
				item.noMelee) {
				return base.UseSpeedMultiplier(item, player) + SpeedAdd;
			}
			float useSpeedMultiplierOnCombo = base.UseSpeedMultiplier(item, player) - .15f + SpeedAdd;
			MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
			//This combo count is delay and because of so, we have to do set back, so swing number 1 = 0
			if (SwingType == BossRushUseStyle.Swipe) {
				if (modPlayer.ComboNumber == 2) {
					return useSpeedMultiplierOnCombo -= .25f;
				}
			}
			if (SwingType == BossRushUseStyle.Poke) {
				if (modPlayer.ComboNumber == 2) {
					return useSpeedMultiplierOnCombo += 1.5f;
				}
				if (Main.mouseRight) {
					return useSpeedMultiplierOnCombo -= .5f;
				}
			}
			return useSpeedMultiplierOnCombo;
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
							SwipeAttack(player, 1);
							break;
						case 1:
							SwipeAttack(player, -1);
							break;
						case 2:
							CircleSwingAttack(player);
							break;
					}
					break;
				case BossRushUseStyle.Poke:
					switch (modPlayer.ComboNumber) {
						case 0:
							if (Main.mouseRight)
								WideSwingAttack(player, 1);
							else
								SwipeAttack(player, 1);
							break;
						case 1:
							if (Main.mouseRight)
								WideSwingAttack(player, -1);
							else
								SwipeAttack(player, -1);
							break;
						case 2:
							StrongThrust(player, modPlayer);
							break;
					}
					break;
				case BossRushUseStyle.GenericSwingDownImprove:
					SwipeAttack(player, 1);
					break;
				default:
					break;
			}
		}
		public override void ModifyItemScale(Item item, Player player, ref float scale) {
			//if (item.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModded)) {
			//	int duration = player.itemAnimationMax;
			//	float thirdduration = duration / 3;
			//	float progress;
			//	if (player.itemAnimation < thirdduration) {
			//		progress = player.itemAnimation / thirdduration;
			//	}
			//	else {
			//		progress = (duration - player.itemAnimation) / thirdduration;
			//	}
			//	scale += MathHelper.SmoothStep(-.75f, .6f, progress);
			//}
		}
		public override void HoldItem(Item item, Player player) {
			if (!player.ItemAnimationActive) {
			}
		}
		private static void StrongThrust(Player player, MeleeOverhaulPlayer modPlayer) {
			float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
			Poke2(player, modPlayer, percentDone);
		}
		private static void Poke2(Player player, MeleeOverhaulPlayer modPlayer, float percentDone) {
			float rotation = player.GetModPlayer<BossRushUtilsPlayer>().MouseLastPositionBeforeAnimation.ToRotation();
			Vector2 poke = Vector2.SmoothStep(modPlayer.PlayerToMouseDirection * 30f, modPlayer.PlayerToMouseDirection, percentDone).RotatedBy(rotation);
			player.itemRotation = modPlayer.PlayerToMouseDirection.ToRotation();
			player.itemRotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f;
			player.compositeFrontArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Full, poke.ToRotation() - MathHelper.PiOver2);
			player.itemLocation = player.Center + poke - poke.SafeNormalize(Vector2.Zero) * 20f;
		}
		private static void WideSwingAttack(Player player, int direct) {
			float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
			percentDone = BossRushUtils.InOutExpo(percentDone);
			float baseAngle = player.GetModPlayer<MeleeOverhaulPlayer>().PlayerToMouseDirection.ToRotation();
			float angle = MathHelper.ToRadians(155) * player.direction;
			float start = baseAngle + angle * direct;
			float end = baseAngle - angle * direct;
			Swipe(start, end, percentDone, player, direct);
		}
		private static void SwipeAttack(Player player, int direct) {
			float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
			percentDone = BossRushUtils.InExpo(percentDone, 11f);
			float baseAngle = player.GetModPlayer<MeleeOverhaulPlayer>().PlayerToMouseDirection.ToRotation();
			float angle = MathHelper.ToRadians(135) * player.direction;
			float start = baseAngle + angle * direct;
			float end = baseAngle - angle * direct;
			Swipe(start, end, percentDone, player, direct);
		}
		private static void CircleSwingAttack(Player player) {
			float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
			float end = (MathHelper.TwoPi * 2) * -player.direction;
			float baseAngle = player.GetModPlayer<MeleeOverhaulPlayer>().ItemRotationBeforeSwitch;
			float addition = baseAngle;
			Swipe(addition, end + addition, BossRushUtils.InOutSine(percentDone), player, 1);
		}
		private static void Swipe(float start, float end, float percentDone, Player player, int direct) {
			bool directIsnegative = direct == -1;
			float currentAngle = MathHelper.Lerp(start, end, percentDone);
			MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
			player.itemRotation = currentAngle;
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, currentAngle - MathHelper.PiOver2);
			player.itemRotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3;
			if (directIsnegative) {
				modPlayer.CustomItemRotation = currentAngle;
				modPlayer.CustomItemRotation += player.direction > 0 ? MathHelper.PiOver4 * 3 : MathHelper.PiOver4;
			}
			player.itemLocation = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Quarter, player.itemRotation) + Vector2.UnitX.RotatedBy(currentAngle) * (BossRushUtilsPlayer.PLAYERARMLENGTH + 3);

		}
	}
	public class MeleeOverhaulSystem : ModSystem {
		public override void Load() {
			On_PlayerDrawLayers.DrawPlayer_RenderAllLayers += On_PlayerDrawLayers_DrawPlayer_RenderAllLayers;
			On_Player.ApplyAttackCooldown += On_Player_ApplyAttackCooldown;
		}

		private void On_Player_ApplyAttackCooldown(On_Player.orig_ApplyAttackCooldown orig, Player self) {
			if (!UniversalSystem.Check_RLOH()) {
				orig(self);
				return;
			}
			orig(self);
			self.attackCD = 0;
		}

		private void On_PlayerDrawLayers_DrawPlayer_RenderAllLayers(On_PlayerDrawLayers.orig_DrawPlayer_RenderAllLayers orig, ref PlayerDrawSet drawinfo) {
			Player player = Main.LocalPlayer;
			Item item = player.HeldItem;
			if (player.TryGetModPlayer(out MeleeOverhaulPlayer modplayer)
				&& item.TryGetGlobalItem(out MeleeWeaponOverhaul meleeItem)
				&& modplayer.ComboNumber == 1
				&& item.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModdedWithoutDefault)) {
				AdjustDrawingInfo(ref drawinfo, modplayer, meleeItem, player, item);
			}
			orig.Invoke(ref drawinfo);
		}
		private static void AdjustDrawingInfo(ref PlayerDrawSet drawinfo, MeleeOverhaulPlayer modplayer, MeleeWeaponOverhaul meleeItem, Player player, Item item) {
			DrawData drawdata;
			for (int i = 0; i < drawinfo.DrawDataCache.Count; i++) {
				if (drawinfo.DrawDataCache[i].texture == TextureAssets.Item[item.type].Value) {
					drawdata = drawinfo.DrawDataCache[i];
					float scale = drawdata.scale.X;
					Vector2 size = drawdata.texture.Size() * scale;
					Vector2 origin = size * .5f;
					if (meleeItem.SwingType == BossRushUseStyle.Poke) {
						if (item.ModItem == null || item.scale <= 1.1f) {
							origin = new Vector2(size.X, size.X) * .5f;
						}
					}
					drawdata.sourceRect = null;
					drawdata.ignorePlayerRotation = true;
					drawdata.rotation = modplayer.CustomItemRotation;
					if (meleeItem.SwingType == BossRushUseStyle.Swipe) {
						float rotationCs = player.direction == -1 ? MathHelper.PiOver4 : MathHelper.PiOver2 + MathHelper.PiOver4;
						float rotationAdjustment = (size.ToRotation() - MathHelper.PiOver4) * -player.direction;
						drawdata.origin = drawdata.texture.Size() * .5f;
						drawdata.position = drawdata.position.IgnoreTilePositionOFFSET((drawdata.rotation - rotationCs - rotationAdjustment).ToRotationVector2(), origin.X + 13);
					}
					if (meleeItem.SwingType == BossRushUseStyle.Poke) {
						drawdata.position +=
						Vector2.UnitX.RotatedBy(drawdata.rotation) *
						(origin.Length() + meleeItem.offset + BossRushUtilsPlayer.PLAYERARMLENGTH + 3) * -player.direction;
					}
					drawinfo.DrawDataCache[i] = drawdata;
				}
			}
		}
	}
	public class MeleeOverhaulPlayer : ModPlayer {
		public Vector2 PlayerToMouseDirection;
		public int ComboNumber = 0;
		public int delaytimer = 10;
		public int oldHeldItem;
		public int CountDownToResetCombo = 0;
		public float CustomItemRotation = 0;
		public StatModifier DelayReuse = new();
		public float ItemRotationBeforeSwitch = 0;
		public override void PreUpdate() {
			Item item = Player.HeldItem;
			if (oldHeldItem != item.type) {
				oldHeldItem = item.type;
				ComboNumber = -1;
				CountDownToResetCombo = 0;
			}
			delaytimer = BossRushUtils.CountDown(delaytimer);
			CountDownToResetCombo = BossRushUtils.CountDown(CountDownToResetCombo);
			if (CountDownToResetCombo <= 0) {
				ComboNumber = -1;
			}
			if (!RoguelikeOverhaul_ModSystem.Optimized_CheckItem(item) || item.noMelee) {
				return;
			}
			if (Player.ItemAnimationJustStarted) {
				if (delaytimer <= 0) {
					//Player.velocity += PlayerToMouseDirection.SafeNormalize(Vector2.Zero) * 3f;
					delaytimer = (int)DelayReuse.ApplyTo(Player.itemAnimationMax * 1.2f);
				}
			}
			DelayReuse = StatModifier.Default;
		}
		public override bool CanUseItem(Item item) {
			if (!Player.ItemAnimationActive) {
				PlayerToMouseDirection = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.Zero);
			}
			if (item.CheckUseStyleMelee(BossRushUtils.MeleeStyle.CheckOnlyModdedWithoutDefault)) {
				ComboHandleSystem();
				ItemRotationBeforeSwitch = Player.itemRotation;
			}
			//if (UniversalSystem.Check_RLOH()) {
			//	if (item.IsAWeapon()) {
			//		return delaytimer <= 0;
			//	}
			//}
			return base.CanUseItem(item);
		}
		public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo) {
			Item item = Player.HeldItem;
			if (!RoguelikeOverhaul_ModSystem.Optimized_CheckItem(item) || item.noMelee) {
				return;
			}
			if (ComboNumber == 1) {
				if (Player.direction == -1) {
					drawInfo.itemEffect = SpriteEffects.None;
				}
				else {
					drawInfo.itemEffect = SpriteEffects.FlipHorizontally;
				}
			}
		}
		private void ComboHandleSystem() {
			ComboNumber = BossRushUtils.Safe_SwitchValue(ComboNumber, 1);
		}
		public override void PostUpdate() {
			Item item = Player.HeldItem;
			if (!RoguelikeOverhaul_ModSystem.Optimized_CheckItem(item) || item.noMelee) {
				return;
			}
			if (Player.ItemAnimationJustStarted) {
				JustHitANPC = false;
			}
			if (Player.ItemAnimationActive) {
				Player.direction = PlayerToMouseDirection.X > 0 ? 1 : -1;
			}
		}
		bool JustHitANPC = false;
		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
			if (!RoguelikeOverhaul_ModSystem.Optimized_CheckItem(item) || item.noMelee) {
				return;
			}
			if (!JustHitANPC) {

				JustHitANPC = true;
			}
		}
	}
}
