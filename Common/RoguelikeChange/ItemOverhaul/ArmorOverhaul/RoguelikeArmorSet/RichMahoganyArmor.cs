﻿using Microsoft.Xna.Framework;
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
	public override void SetDefault() {
		PieceID = ItemID.RichMahoganyHelmet;
		Add_Defense = 3;
	}
}
class RichMahoganyBreastplate : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.RichMahoganyBreastplate;
		Add_Defense = 4;
	}
}
class RichMahoganyGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.RichMahoganyGreaves;
		Add_Defense = 3;
	}
}
class RichMahoganyArmorPlayer : PlayerArmorHandle {
	bool inZone = false;
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("RichMahoganyArmor", this);
	}
	public override void Armor_ResetEffects() {
		inZone = false;
	}
	public override void Armor_UpdateEquipsSet() {
		if (Player.ZoneJungle) {
			Player.statDefense += 6;
			Player.moveSpeed += .30f;
			inZone = false;
		}
	}
	public override void Armor_OnHitByNPC(NPC target, Player.HurtInfo hurtInfo) {
		OnHitNPC_Armor(target);
	}
	public override void Armor_OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		OnHitNPC_Armor(proj);
	}
	private void OnHitNPC_Armor(Entity entity) {
		if (!inZone)
			return;
		for (int i = 0; i < 10; i++) {
			Vector2 spread = Vector2.One.Vector2DistributeEvenly(10f, 360, i);
			int proj = Projectile.NewProjectile(Player.GetSource_OnHurt(entity), Player.Center, spread * 2f, ProjectileID.BladeOfGrass, 12, 1f, Player.whoAmI);
			Main.projectile[proj].penetrate = -1;
		}
	}
}
