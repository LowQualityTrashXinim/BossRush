using BossRush.Common.Systems;
using BossRush.Texture;
using Terraria;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Consumable.Scroll;
internal class ProtectionScroll : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<ProtectionSpell>(), BossRushUtils.ToSecond(1.5f));
	}
}
public class ProtectionSpell : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, 1.12f, Base: 10);
	}
}

