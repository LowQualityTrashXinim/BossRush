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
	}
	public override void UpdateInventory(Player player) {
		if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul meleeItem)) {
			meleeItem.SwingType = BossRushUseStyle.Swipe;
		}
	}
	public override void UseStyle(Player player, Rectangle heldItemFrame) {
		base.UseStyle(player, heldItemFrame);
	}
}
