using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul;
internal class BattleAxeOverhaul : GlobalItem {
	public override bool InstancePerEntity => true;
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.axe > 0 && !entity.noMelee;
	}
	public int UseStyleType = 0;
	public override void SetDefaults(Item entity) {
		if (entity.axe <= 0 || entity.noMelee) {
			return;
		}
		switch (entity.type) {
				//common ore axe
			case ItemID.CopperAxe:
			case ItemID.TinAxe:
			case ItemID.IronAxe:
			case ItemID.LeadAxe:
			case ItemID.SilverAxe:
			case ItemID.TungstenAxe:
			case ItemID.GoldAxe:
			case ItemID.PlatinumAxe:
				//uncommon ore axe
			case ItemID.BloodLustCluster:
			case ItemID.WarAxeoftheNight:
			case ItemID.MoltenPickaxe:
			case ItemID.MeteorHamaxe:
				//Hardmode ore axe
			case ItemID.CobaltWaraxe:
			case ItemID.PalladiumWaraxe:
			case ItemID.MythrilWaraxe:
			case ItemID.OrichalcumWaraxe:
			case ItemID.AdamantiteWaraxe:
			case ItemID.TitaniumWaraxe:
				entity.useTime = entity.useAnimation = 40;
				entity.damage += 13;
				entity.scale += .25f;
				entity.useTurn = false;
				UseStyleType = BossRushUseStyle.DownChop;
				entity.Set_ItemCriticalDamage(1.5f);
				break;
		}
	}
	public override bool? CanMeleeAttackCollideWithNPC(Item item, Rectangle meleeAttackHitbox, Player player, NPC target) {
		if (RoguelikeOverhaul_ModSystem.Optimized_CheckItem(item)) {
			if (UseStyleType == BossRushUseStyle.DownChop) {
				float itemsize = item.Size.Length() * player.GetAdjustedItemScale(player.HeldItem) + BossRushUtilsPlayer.PLAYERARMLENGTH;
				int laserline = (int)itemsize * 2;
				if (laserline <= 1) {
					laserline = 2;
				}
				MeleeOverhaulPlayer modplayer = player.GetModPlayer<MeleeOverhaulPlayer>();
				float baseAngle = modplayer.PlayerToMouseDirection.ToRotation();
				float angle = MathHelper.ToRadians(145);
				float start = baseAngle + angle - angle * .5f;
				float end = baseAngle - angle;
				int LastCollideCheck, check;
				if (player.direction == 1) {
					LastCollideCheck =
						(int)Math.Ceiling(MathHelper.Lerp(0, laserline, BossRushUtils.InOutBack((player.itemAnimation + 1) / (float)player.itemAnimationMax)));
					check =
						(int)Math.Ceiling(MathHelper.Lerp(0, laserline, BossRushUtils.InOutBack(player.itemAnimation / (float)player.itemAnimationMax)));
				}
				else {
					LastCollideCheck =
						(int)Math.Ceiling(MathHelper.Lerp(laserline, 0, BossRushUtils.InOutBack((player.itemAnimation + 1) / (float)player.itemAnimationMax)));
					check =
						(int)Math.Ceiling(MathHelper.Lerp(laserline, 0, BossRushUtils.InOutBack(player.itemAnimation / (float)player.itemAnimationMax)));
				}
				if (player.itemAnimationMax <= 2) {
					for (int i = 0; i <= laserline; i++) {
						float rotation = MathHelper.Lerp(start, end, i / (laserline - 1f));
						rotation += player.direction == 1 ? 0 : MathHelper.PiOver4 * 1.8f;
						Vector2 point = player.Center + rotation.ToRotationVector2() * itemsize;
						if (BossRushUtils.Collision_PointAB_EntityCollide(target.Hitbox, player.Center, point)) {
							return true;
						}
					}
					return false;
				}
				int assigned = Math.Min(LastCollideCheck, check);
				int length = Math.Max(check, LastCollideCheck);
				for (int i = assigned; i <= length; i++) {
					float rotation = MathHelper.Lerp(start, end, i / (laserline - 1f));
					rotation += player.direction == 1 ? 0 : MathHelper.PiOver4 * 1.8f;
					Vector2 point = player.Center + rotation.ToRotationVector2() * itemsize;
					if (BossRushUtils.Collision_PointAB_EntityCollide(target.Hitbox, player.Center, point)) {
						return true;
					}
					//Mod.Logger.Debug($"Frame : {player.itemAnimation} | prev {previousAnimationFrame} | Check : {checkoutside} | prev {LastCollideCheck}");
				}
				return false;
			}
		}
		return base.CanMeleeAttackCollideWithNPC(item, meleeAttackHitbox, player, target);
	}

	public override bool CanUseItem(Item item, Player player) {
		if (UseStyleType != BossRushUseStyle.Spin &&
			UseStyleType != BossRushUseStyle.DownChop ||
			item.noMelee) {
			return base.CanUseItem(item, player);
		}
		return player.GetModPlayer<MeleeOverhaulPlayer>().delaytimer <= 0;
	}
	public override void HoldItem(Item item, Player player) {
		if (!player.ItemAnimationActive) {
		}
	}
	public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers) {
	}
	public override void UseStyle(Item item, Player player, Rectangle heldItemFrame) {
		if (item.noMelee) {
			return;
		}
		switch (UseStyleType) {
			case BossRushUseStyle.DownChop:
				SwipeAttack(player);
				break;
			case BossRushUseStyle.GenericSwingDownImprove:
				SwipeAttack(player);
				break;
			default:
				break;
		}
	}
	private void SwipeAttack(Player player) {
		float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
		percentDone = BossRushUtils.InOutBack(percentDone);
		float baseAngle = player.GetModPlayer<MeleeOverhaulPlayer>().PlayerToMouseDirection.ToRotation();
		float angle = MathHelper.ToRadians(145) * player.direction;
		float start = baseAngle + angle - angle * .5f;
		float end = baseAngle - angle;
		Swipe(start, end, percentDone, player);
	}
	private void Swipe(float start, float end, float percentDone, Player player) {
		float currentAngle = MathHelper.Lerp(start, end, percentDone);
		player.itemRotation = currentAngle;
		player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, currentAngle - MathHelper.PiOver2);
		player.itemRotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3;
		player.itemLocation = player.Center + Vector2.UnitX.RotatedBy(currentAngle) * BossRushUtilsPlayer.PLAYERARMLENGTH;
	}
}
