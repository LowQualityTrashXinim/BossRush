using BossRush.Common.Utils;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria;

namespace BossRush.Contents.Items.Consumable.Potion;
internal class AntiPoisonPotion : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTUREPOTION;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<AntiPoisonBuff>(), BossRushUtils.ToMinute(1.5f));
	}
}
public class AntiPoisonBuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		for (int i = 0; i < TerrariaArrayID.PoisonBuff.Length; i++) {
			player.buffImmune[TerrariaArrayID.PoisonBuff[i]] = true;
		}
	}
}
