using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Consumable.Scroll;
internal class ScrollOfMaterialize : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<MaterializeSpell>(), BossRushUtils.ToMinute(4));
	}
}
public class MaterializeSpell : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
}
public class ScrollOfMaterializePlayer : ModPlayer {
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Player.HasBuff<MaterializeSpell>()) {
			if (Main.rand.NextBool(15)) {
				Item.NewItem(Player.GetSource_OnHit(target), target.Hitbox, Main.rand.NextBool() ? ItemID.Heart : ItemID.Star);
			}
		}
	}
}

