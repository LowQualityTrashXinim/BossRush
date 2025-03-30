using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Consumable.Potion;
internal class ShadowPotion : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTUREPOTION;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<ShadowBuff>(), BossRushUtils.ToMinute(1.5f));
		Item.Set_ItemIsRPG();
	}
}
public class ShadowBuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.DodgeChance += 0.04f;
		modplayer.AddStatsToPlayer(PlayerStats.Iframe, 1.1f);
	}
}
