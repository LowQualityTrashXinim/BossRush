using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Systems;

namespace BossRush.Contents.Items.Consumable.Potion;
internal class LifeStealPotion : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTUREPOTION;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<LifeStealBuff>(), BossRushUtils.ToMinute(4));
	}
}
public class LifeStealBuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<PlayerStatsHandle>().LifeSteal += .02f;
	}
}
