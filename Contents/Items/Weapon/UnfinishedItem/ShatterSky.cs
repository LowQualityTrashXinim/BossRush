using BossRush.Common.RoguelikeChange.ItemOverhaul;
using BossRush.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.Weapon.UnfinishedItem;
class ShatterSky : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultMeleeShootCustomProjectile(32, 32, 100, 5f, 30, 30, ItemUseStyleID.Swing, 1, 1, true);
		Item.GetGlobalItem<MeleeWeaponOverhaul>().SwingType = BossRushUseStyle.Swipe;
		Item.GetGlobalItem<MeleeWeaponOverhaul>().CircleSwingAmount = 1f;
	}
	int ComboCounter = 0;
	public override bool CanUseItem(Player player) {
		if (!player.ItemAnimationActive) {
			ComboCounter++;
			MeleeWeaponOverhaul overhaul = Item.GetGlobalItem<MeleeWeaponOverhaul>();
			switch (ComboCounter) {
				case 1:
					overhaul.SwingType = BossRushUseStyle.SwipeDown;
					break;
				case 2:
					overhaul.SwingType = BossRushUseStyle.SwipeUp;
					break;
				case 3:
					overhaul.SwingType = BossRushUseStyle.Spin;
					break;
			}
			if (ComboCounter >= 3) {
				ComboCounter = 0;
			}
		}
		return base.CanUseItem(player);
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.BreakerBlade)
			.AddIngredient(ItemID.NimbusRod)
			.Register();
	}
}
