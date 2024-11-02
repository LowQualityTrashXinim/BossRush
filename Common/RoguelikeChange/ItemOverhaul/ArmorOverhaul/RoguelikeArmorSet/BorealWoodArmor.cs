using Terraria;
using Terraria.ID;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class BorealwoodArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.BorealWoodHelmet;
		bodyID = ItemID.BorealWoodBreastplate;
		legID = ItemID.BorealWoodGreaves;
	}
}
class BorealWoodHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.BorealWoodHelmet;
		Add_Defense = 3;
	}
}
class BorealWoodBreastplate : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.BorealWoodBreastplate;
		Add_Defense = 4;
	}
}
class BorealWoodGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.BorealWoodGreaves;
		Add_Defense = 3;
	}
}
class BorealWoodArmorPlayer : PlayerArmorHandle {
	bool inZone = false;
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("BorealwoodArmor", this);
	}
	public override void Armor_UpdateEquipsSet() {
		if (Player.ZoneSnow) {
			Player.statDefense += 5;
			Player.moveSpeed += .20f;
			Player.buffImmune[BuffID.Chilled] = true;
			Player.buffImmune[BuffID.Slow] = true;
			inZone = true;
		}
	}
	public override void Armor_OnHitWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		OnHitNPC_BorealWoodArmor(target);
	}
	public override void Armor_OnHitWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		OnHitNPC_BorealWoodArmor(target);
	}
	private void OnHitNPC_BorealWoodArmor(NPC target) {
		if (Main.rand.NextFloat() <= .3f && inZone) {
			target.AddBuff(BuffID.Frostburn, BossRushUtils.ToSecond(10));
		}
	}
}
