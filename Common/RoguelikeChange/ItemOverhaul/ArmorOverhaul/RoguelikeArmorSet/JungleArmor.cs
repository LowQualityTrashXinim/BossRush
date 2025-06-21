using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using BossRush.Contents.Projectiles;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class JungleArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.JungleHat;
		bodyID = ItemID.JungleShirt;
		legID = ItemID.JunglePants;
	}
}
public class JungleHat : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.JungleHat;
		Add_Defense = 1;
	}
}
public class JungleShirt : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.JungleShirt;
		Add_Defense = 1;
	}
}
public class JunglePants : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.JunglePants;
		Add_Defense = 1;
	}
}
public class JungleArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("JungleArmor", this);
	}
	public float[] Projindex = [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1];
	public override bool Armor_Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (item.DamageType == DamageClass.Magic) {

			float indexThatIsMissing = 0;
			for (int i = 0; i < Projindex.Length; i++) {
				if (Projindex[i] != -1)
					continue;
				indexThatIsMissing = i;
				Projindex[i] = 1;
				break;
			}
			if (Player.ownedProjectileCounts[ModContent.ProjectileType<LeafProjectile>()] < 10) {
				Projectile.NewProjectile(source, Player.Center, Vector2.Zero, ModContent.ProjectileType<LeafProjectile>(), (int)(damage * 1.25f), knockback, Player.whoAmI, indexThatIsMissing);
			}
		}
		return true;
	}
}
