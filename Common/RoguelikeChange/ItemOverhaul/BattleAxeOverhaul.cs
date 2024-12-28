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
			case ItemID.CopperAxe:
			case ItemID.TinAxe:
			case ItemID.IronAxe:
			case ItemID.LeadAxe:
			case ItemID.SilverAxe:
			case ItemID.TungstenAxe:
			case ItemID.GoldAxe:
			case ItemID.PlatinumAxe:
				entity.useTime = entity.useAnimation = 40;
				entity.damage += 13;
				entity.scale += .25f;
				entity.useTurn = false;
				UseStyleType = BossRushUseStyle.DownChop;
				entity.Set_ItemCriticalDamage(2.5f);
				break;
		}
	}
	public override bool? CanMeleeAttackCollideWithNPC(Item item, Rectangle meleeAttackHitbox, Player player, NPC target) {
		if (RoguelikeOverhaul_ModSystem.Optimized_CheckItem(item)) {
			float itemsize = item.Size.Length() * (player.GetAdjustedItemScale(item) + .2f) + BossRushUtilsPlayer.PLAYERARMLENGTH;
			int laserline = (int)itemsize;
			if (UseStyleType == BossRushUseStyle.DownChop) {
				Vector2 directionTo = player.GetModPlayer<MeleeOverhaulPlayer>().PlayerToMouseDirection;
				float percentDone = BossRushUtils.InOutBack((player.direction == 1).ToInt() - player.itemAnimation / (float)player.itemAnimationMax * player.direction);
				int check = (int)Math.Ceiling(MathHelper.Lerp(0, laserline, percentDone));
				int LastCollideCheck = (int)Math.Ceiling(MathHelper.Lerp(0, laserline, BossRushUtils.InOutBack((player.direction == 1).ToInt() - (player.itemAnimation - 1) / (float)player.itemAnimationMax * player.direction))); ;
				for (int i = Math.Min(LastCollideCheck, check); i <= Math.Max(LastCollideCheck, check); i++) {
					Vector2 point = player.Center + directionTo.Vector2DistributeEvenly(36, 270, i) * itemsize;
					if (Collision.CheckAABBvLineCollision(target.Hitbox.TopLeft(), target.Size * target.scale, player.Center, point)) {
						return true;
					}
				}
			}
			return false;
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
		if (RoguelikeOverhaul_ModSystem.Optimized_CheckItem(item)) {
			float percentDone = Math.Clamp(BossRushUtils.InOutBack(player.itemAnimation / (float)player.itemAnimationMax) * 2.5f, .1f, 2.5f);
			modifiers.FinalDamage *= percentDone;
		}
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
