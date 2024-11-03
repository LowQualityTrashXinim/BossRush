using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Consumable.Scroll;
internal class ScrollofEvasive : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<EnvasionSpell>(), BossRushUtils.ToMinute(1));
		Item.Set_ItemIsRPG();
	}
}
public class EnvasionSpell : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
}
public class ScrollOfEnvasionPlayer : ModPlayer {
	public override bool FreeDodge(Player.HurtInfo info) {
		if (Player.HasBuff(ModContent.BuffType<EnvasionSpell>()) && Main.rand.NextBool()) {
			Player.AddImmuneTime(info.CooldownCounter, 60);
			return true;
		}
		return base.FreeDodge(info);
	}
}
