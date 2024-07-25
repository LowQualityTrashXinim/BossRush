using BossRush.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.LostAccessories {
	class EnergeticCrystal : ModItem {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override void SetDefaults() {
			Item.accessory = true;
			Item.height = 30;
			Item.width = 28;
			Item.rare = ItemRarityID.Green;
			Item.value = 1000000;
		}
		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.statLifeMax2 += 50;
			player.statManaMax2 += 50;
			player.lifeRegen += 5;
			player.manaRegen += 5;
		}
	}
}
