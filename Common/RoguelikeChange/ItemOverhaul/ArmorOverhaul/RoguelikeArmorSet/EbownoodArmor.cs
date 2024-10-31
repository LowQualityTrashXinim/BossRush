using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Projectiles;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class EbownoodArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.EbonwoodHelmet;
		bodyID = ItemID.EbonwoodBreastplate;
		legID = ItemID.EbonwoodGreaves;
	}
}
public class EbonwoodHelmet : ModArmorPiece {
	public override int Add_Defense => 2;
	public override int _pieceID => ItemID.EbonwoodHelmet;
}
public class EbonwoodBreastplate : ModArmorPiece {
	public override int Add_Defense => 3;
	public override int _pieceID => ItemID.EbonwoodBreastplate;
}
public class EbonwoodGreaves : ModArmorPiece {
	public override int Add_Defense => 2;
	public override int _pieceID => ItemID.EbonwoodGreaves;
}
public class EbonwoodArmorPlayer : PlayerArmorHandle {
	int EbonWoodArmorCD = 0;
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("EbonwoodArmor", this);
	}
	public override void Armor_UpdateEquipsSet() {
		if (Player.ZoneCorrupt) {
			Player.statDefense += 6;
			Player.moveSpeed += .35f;
			Player.GetDamage(DamageClass.Generic) += .05f;
		}
		if (--EbonWoodArmorCD <= 0 && Player.velocity != Vector2.Zero) {
			Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + Main.rand.NextVector2Circular(10, 10), -Player.velocity.SafeNormalize(Vector2.Zero), ModContent.ProjectileType<CorruptionTrail>(), 3, 0, Player.whoAmI);
			EbonWoodArmorCD = 45;
		}
	}
}
