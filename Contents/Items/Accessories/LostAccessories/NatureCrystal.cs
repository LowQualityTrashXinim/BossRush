using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Items.Weapon;

namespace BossRush.Contents.Items.Accessories.LostAccessories {
	class NatureCrystal : ModItem {
		public override void SetDefaults() {
			Item.DefaultToAccessory(28, 30);
			Item.accessory = true;
			Item.rare = ItemRarityID.Green;
			Item.value = 1000000;
		}
		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.statLifeMax2 += 40;
			player.statManaMax2 += 40;
			player.GetModPlayer<PlayerSynergyItemHandle>().NatureSelection_NatureCrystal = true;
		}
	}
}
