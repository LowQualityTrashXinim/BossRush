using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class RichMahoganyArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.RichMahoganyHelmet;
		bodyID = ItemID.RichMahoganyBreastplate;
		legID = ItemID.RichMahoganyGreaves;
	}
}
class RichMahoganyHelmet : ModArmorPiece {
	public override int Add_Defense => 3;
	public override int _pieceID => ItemID.RichMahoganyHelmet;
}
class RichMahoganyBreastplate : ModArmorPiece {
	public override int Add_Defense => 4;
	public override int _pieceID => ItemID.RichMahoganyBreastplate;
}
class RichMahoganyGreaves : ModArmorPiece {
	public override int Add_Defense => 3;
	public override int _pieceID => ItemID.RichMahoganyGreaves;
}
class RichMahoganyArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ModArmorSet armor = ArmorLoader.GetModArmor("RichMahoganyArmor");
		armor.modplayer = this;
	}
	public override void Armor_UpdateEquipsSet() {
		if (Player.ZoneJungle) {
			Player.statDefense += 6;
			Player.moveSpeed += .30f;
		}
	}
	public override void Armor_OnHitByNPC(NPC target, Player.HurtInfo hurtInfo) {
		OnHitNPC_Armor(target);
	}
	public override void Armor_OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		OnHitNPC_Armor(proj);
	}
	private void OnHitNPC_Armor(Entity entity) {
		for (int i = 0; i < 10; i++) {
			Vector2 spread = Vector2.One.Vector2DistributeEvenly(10f, 360, i);
			int proj = Projectile.NewProjectile(Player.GetSource_OnHurt(entity), Player.Center, spread * 2f, ProjectileID.BladeOfGrass, 12, 1f, Player.whoAmI);
			Main.projectile[proj].penetrate = -1;
		}
	}
}
