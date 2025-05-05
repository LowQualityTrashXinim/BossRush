using Terraria;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Consumable.Scroll;
internal class ScrollOfProtection : ModItem {
	public override void SetStaticDefaults() {
		BossRushModSystem.LootboxPotion.Add(Item);
	}
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<ProtectionSpell>(), BossRushUtils.ToSecond(20));
		Item.Set_ItemIsRPG();
	}
}
public class ProtectionSpell : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
}
public class ProtectionSpell_Player : ModPlayer {
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (Player.HasBuff<ProtectionSpell>()) {
			modifiers.SetMaxDamage(1);
			Player.DelBuff(Player.FindBuffIndex(ModContent.BuffType<ProtectionSpell>()));
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (Player.HasBuff<ProtectionSpell>()) {
			modifiers.SetMaxDamage(1);
			Player.DelBuff(Player.FindBuffIndex(ModContent.BuffType<ProtectionSpell>()));
		}
	}
}

