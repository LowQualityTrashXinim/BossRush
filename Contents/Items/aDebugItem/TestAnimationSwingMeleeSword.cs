using BossRush.Common.RoguelikeChange.ItemOverhaul;
using BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.EverlastingCold;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.aDebugItem;
internal class TestAnimationSwingMeleeSword : ModItem {
	public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<EverlastingCold>();
	public override void SetDefaults() {
		BossRushUtils.BossRushSetDefault(Item, 92, 92, 120, 5f, 20, 20, ItemUseStyleID.Swing, true);
		Item.DamageType = DamageClass.Melee;
		Item.UseSound = SoundID.Item1;
		if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul system)) {
			system.AnimationEndTime = 25;
			system.SwingDegree = 155;
			system.SwingStrength = 7f;
			system.SwingType = BossRushUseStyle.SwipeDown;
		}
	}
	public override void UseStyle(Player player, Rectangle heldItemFrame) {
			float rotation = MathHelper.ToRadians(50) * player.direction;
		if (player.itemAnimation <= 5) {
			player.itemRotation = Item.GetGlobalItem<MeleeWeaponOverhaul>().RotationAfterMainAnimationEnd + rotation;
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, player.itemRotation - MathHelper.PiOver2 - (player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3));
			player.itemLocation = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, player.itemRotation - MathHelper.PiOver2 - (player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3)) + Vector2.UnitX.RotatedBy(player.itemRotation);
			return;
		}
		if (player.itemAnimation <= 25) {
			player.itemRotation = Item.GetGlobalItem<MeleeWeaponOverhaul>().RotationAfterMainAnimationEnd + rotation * (1 - (player.itemAnimation - 5) / 20f);
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, player.itemRotation - MathHelper.PiOver2 - (player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3));
			player.itemLocation = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, player.itemRotation - MathHelper.PiOver2 - (player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3)) + Vector2.UnitX.RotatedBy(player.itemRotation);
		}
	}
	public override void ModifyItemScale(Player player, ref float scale) {
		//if (player.itemAnimation <= 20) {
		//	float progress;
		//	if (player.itemAnimation <= 10) {
		//		progress = player.itemAnimation / 20f;
		//	}
		//	else {
		//		progress = 1 - player.itemAnimation / 20f;
		//	}
		//	scale -= progress * 1.5f;
		//}
	}
}
