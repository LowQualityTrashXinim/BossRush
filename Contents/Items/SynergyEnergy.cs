using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items {
	internal class SynergyEnergy : ModItem {
		public override void SetDefaults() {
			Item.rare = ItemRarityID.Red;
			Item.width = 54;
			Item.height = 20;
			Item.material = true;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<SynergyModPlayer>().acc_SynergyEnergy = true;
		}
	}
	public class SynergyModPlayer : ModPlayer {
		public int ItemTypeCurrent = 0;
		public int ItemTypeOld = 0;
		public bool acc_SynergyEnergy = false;
		public override void ResetEffects() {
			acc_SynergyEnergy = false;
			if(ItemTypeCurrent != Player.HeldItem.type) {
				int cache = ItemTypeCurrent;
				ItemTypeCurrent = Player.HeldItem.type;
				ItemTypeOld = cache;
			}
			if(Player.itemAnimation == 1) {
				ItemTypeOld = ItemTypeCurrent;
			}
		}
		public bool CompareOldvsNewItemType => ItemTypeCurrent != ItemTypeOld;
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if(ItemTypeOld != ItemTypeCurrent && acc_SynergyEnergy) {
				damage.Base += 5;
			}
		}
	}
}
