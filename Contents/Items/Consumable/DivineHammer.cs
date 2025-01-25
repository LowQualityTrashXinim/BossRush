using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Consumable;
internal class DivineHammer : ModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32);
		Item.noUseGraphic = true;
		Item.UseSound = SoundID.Item37;
	}
	public override bool? UseItem(Player player) {
		return true;
	}
}
