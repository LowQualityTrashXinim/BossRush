using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Accessories.EnragedBossAccessories.EmpressDelight {
	internal class EmpressDelight : ModItem {
		public override void SetDefaults() {
			Item.accessory = true;
			Item.height = 40;
			Item.width = 40;
			Item.rare = 7;
			Item.value = 10000000;
		}
		public override void UpdateAccessory(Player player, bool hideVisual) {
			if (Main.dayTime) {
				player.GetDamage(DamageClass.Magic) *= 2.0f;
				player.GetDamage(DamageClass.Ranged) *= 2.0f;
				player.GetDamage(DamageClass.Melee) *= 2.0f;
				player.GetDamage(DamageClass.Summon) *= 2.0f;
			}
		}

	}
}
