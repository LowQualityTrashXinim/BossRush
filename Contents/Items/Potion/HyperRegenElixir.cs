using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Potion;
internal class HyperRegenElixir : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTUREPOTION;
	public override void SetDefaults() {
		Item.BossRushDefaultToConsume(32, 32, ItemUseStyleID.DrinkLiquid);
		Item.maxStack = 30;
		Item.buffTime = 12000;
		Item.rare = ItemRarityID.Orange;
		Item.buffType = ModContent.BuffType<HyperRegen>();
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
		player.statDefense -= 35;
	}
}
