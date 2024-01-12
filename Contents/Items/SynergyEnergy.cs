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
			player.GetModPlayer<SynergyEnergyModPlayer>().EnableSwitch = true;
		}
	}
	public class SynergyEnergyModPlayer : ModPlayer {
		public int ItemType = 0;
		public bool EnableSwitch = false;
		public bool JustChangeWeapon = false;
		public override void ResetEffects() {
			EnableSwitch = false;
		}
		public override void PostUpdate() {
			if(Player.ItemAnimationActive) {
				ItemType = Player.HeldItem.type;
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
			if(EnableSwitch && ItemType != item.type) {
				damage.Base += 5;
			}
		}
	}
}
