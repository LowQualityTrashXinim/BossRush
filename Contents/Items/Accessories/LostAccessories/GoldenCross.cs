using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Contents.Items.Weapon;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Accessories.LostAccessories;
internal class GoldenCross : ModItem {
	public override void SetDefaults() {
		Item.DefaultToAccessory(32, 32);
		Item.GetGlobalItem<GlobalItemHandle>().LostAccessories = true;
	}
	public override void UpdateEquip(Player player) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Iframe, Additive: 1.33f, Flat: BossRushUtils.ToSecond(0.5f));
	}
}
