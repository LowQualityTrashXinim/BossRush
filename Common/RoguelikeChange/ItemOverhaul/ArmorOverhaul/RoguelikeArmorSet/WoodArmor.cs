using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Projectiles;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class WoodArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.WoodHelmet;
		bodyID = ItemID.WoodBreastplate;
		legID = ItemID.WoodGreaves;
	}
}
class WoodHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.WoodHelmet;
		Add_Defense = 3;
	}
}
class WoodBreastplate : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.WoodBreastplate;
		Add_Defense = 4;
	}
}
class WoodGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.WoodGreaves;
		Add_Defense = 3;
	}
}
class WoodArmorPlayer : PlayerArmorHandle {
	bool inZone = false;
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("WoodArmor", this);
	}
	public override void Armor_ResetEffects() {
		inZone = false;
	}
	public override void Armor_UpdateEquipsSet() {
		if (Player.ZoneForest) {
			Player.statDefense += 5;
			Player.moveSpeed += .25f;
			inZone = true;
		}
	}
	public override void Armor_OnHitWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(4) && (proj is null || proj is not null && proj.ModProjectile is not AcornProjectile) && inZone) {
			Projectile.NewProjectile(Player.GetSource_FromThis(),
				target.Center - new Vector2(0, 400),
				Vector2.UnitY * 10,
				ModContent.ProjectileType<AcornProjectile>(), 10, 1f, Player.whoAmI);
		}
	}
	public override void Armor_OnHitWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(4) && inZone) {
			Projectile.NewProjectile(Player.GetSource_FromThis(),
				target.Center - new Vector2(0, 400),
				Vector2.UnitY * 10,
				ModContent.ProjectileType<AcornProjectile>(), 10, 1f, Player.whoAmI);
		}
	}
}
