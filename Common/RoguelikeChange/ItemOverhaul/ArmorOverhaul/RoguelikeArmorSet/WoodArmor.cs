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
	public override int Add_Defense => 3;
	public override int _pieceID => ItemID.WoodHelmet;
}
class WoodBreastplate : ModArmorPiece {
	public override int Add_Defense => 4;
	public override int _pieceID => ItemID.WoodBreastplate;
}
class WoodGreaves : ModArmorPiece {
	public override int Add_Defense => 3;
	public override int _pieceID => ItemID.WoodGreaves;
}
class WoodArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ModArmorSet armor = ArmorLoader.GetModArmor("WoodArmor");
		armor.modplayer = this;
	}
	public override void Armor_UpdateEquipsSet() {
		if (Player.ZoneForest) {
			Player.statDefense += 5;
			Player.moveSpeed += .25f;
		}
	}
	public override void Armor_OnHitWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(4) && (proj is null || proj is not null && proj.ModProjectile is not AcornProjectile)) {
			Projectile.NewProjectile(Player.GetSource_FromThis(),
				target.Center - new Vector2(0, 400),
				Vector2.UnitY * 10,
				ModContent.ProjectileType<AcornProjectile>(), 10, 1f, Player.whoAmI);
		}
	}
	public override void Armor_OnHitWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(4)) {
			Projectile.NewProjectile(Player.GetSource_FromThis(),
				target.Center - new Vector2(0, 400),
				Vector2.UnitY * 10,
				ModContent.ProjectileType<AcornProjectile>(), 10, 1f, Player.whoAmI);
		}
	}
}
