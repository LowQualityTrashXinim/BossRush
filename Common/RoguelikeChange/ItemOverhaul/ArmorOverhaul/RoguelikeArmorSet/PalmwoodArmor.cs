using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using BossRush.Contents.Projectiles;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class PalmwoodArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.PalmWoodHelmet;
		bodyID = ItemID.PalmWoodBreastplate;
		legID = ItemID.PalmWoodGreaves;
	}
}
public class PalmWoodHelmet : ModArmorPiece {
	public override int Add_Defense => 3;
	public override int _pieceID => ItemID.PalmWoodHelmet;
}
public class PalmWoodBreastplate : ModArmorPiece {
	public override int Add_Defense => 4;
	public override int _pieceID => ItemID.PalmWoodBreastplate;
}
public class PalmWoodGreaves : ModArmorPiece {
	public override int Add_Defense => 3;
	public override int _pieceID => ItemID.PalmWoodGreaves;
}
public class PalmwoodArmorPlayer : PlayerArmorHandle {
	int PalmWoodArmor_SandCounter = 0;
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("PalmwoodArmor", this);
	}
	public override void Armor_UpdateEquipsSet() {
		Player.statDefense += 10;
		Player.moveSpeed += .17f;
		if (Player.justJumped) {
			for (int i = 0; i < 4; i++) {
				Vector2 vec = new Vector2(-Player.velocity.X, Player.velocity.Y).Vector2RotateByRandom(20).LimitedVelocity(Main.rand.NextFloat(2, 3));
				Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, vec, ModContent.ProjectileType<SandProjectile>(), 12, 1f, Player.whoAmI);
			}
		}
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (++PalmWoodArmor_SandCounter >= 7) {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SandProjectile>(), (int)(damage * .5f), knockback, Player.whoAmI);
			if (PalmWoodArmor_SandCounter >= 10) {
				Projectile.NewProjectile(source, position, velocity.SafeNormalize(Vector2.Zero) * 20f, ModContent.ProjectileType<CoconutProjectile>(), (int)(damage * 1.25f), knockback, Player.whoAmI);
				PalmWoodArmor_SandCounter = 0;
			}
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
}
