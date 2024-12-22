using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Consumable.Potion;
internal class HyperRegenElixir : ModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<HyperRegen>(), 12000);
		Item.rare = ItemRarityID.Orange;
		Item.value = Item.sellPrice(gold: 25);
	}
}
internal class HyperRegen : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = false;
		Main.buffNoSave[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex) {
		player.lifeRegen += 50;
		player.statDefense -= 25;
	}
}
