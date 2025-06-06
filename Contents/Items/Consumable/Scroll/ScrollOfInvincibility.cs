﻿using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Consumable.Scroll;
internal class ScrollOfInvincibility : ModItem {
	public override void SetStaticDefaults() {
		BossRushModSystem.LootboxPotion.Add(Item);
	}
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.Set_AdvancedBuffItem();
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<InvincibilitySpell>(), BossRushUtils.ToSecond(10));
		Item.Set_ItemIsRPG();
	}
}
public class InvincibilitySpell : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
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
