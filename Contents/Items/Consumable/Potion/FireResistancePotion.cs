using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Utils;

namespace BossRush.Contents.Items.Consumable.Potion;
internal class FireResistancePotion : ModItem{
	public override string Texture => BossRushTexture.MISSINGTEXTUREPOTION;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<FireResistanceBuff>(), BossRushUtils.ToSecond(1.5f));
	}
}
public class FireResistanceBuff : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		for (int i = 0; i < TerrariaArrayID.FireBuff.Length; i++) {
			player.buffImmune[TerrariaArrayID.FireBuff[i]] = true;
		}
	}
}
