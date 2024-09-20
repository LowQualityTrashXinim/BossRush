using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Systems;

namespace BossRush.Contents.Items.Consumable.Scroll;
internal class ScrollOfInvincibility : ModItem {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<InvincibilitySpell>(), BossRushUtils.ToSecond(10));
	}
}
public class InvincibilitySpell : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, 1.12f, Base: 10);
		player.noFallDmg = true;
	}
}

public class ScrollOfInvincibilityPlayer : ModPlayer {
	public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot) {
		if (Player.HasBuff(ModContent.BuffType<InvincibilitySpell>())) {
			return false;
		}
		return base.CanBeHitByNPC(npc, ref cooldownSlot);
	}
	public override bool CanBeHitByProjectile(Projectile proj) {
		if (Player.HasBuff(ModContent.BuffType<InvincibilitySpell>())) {
			return false;
		}
		return base.CanBeHitByProjectile(proj);
	}
}
