using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class AshwoodArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.AshWoodHelmet;
		bodyID = ItemID.AshWoodBreastplate;
		legID = ItemID.AshWoodGreaves;
	}
}
public class AshWoodHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.AshWoodHelmet;
		Add_Defense = 3;
	}
}
public class AshWoodBreastplate : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.AshWoodBreastplate;
		Add_Defense = 4;
	}
}
public class AshWoodGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.AshWoodGreaves;
		Add_Defense = 3;
	}
}
public class AshwoodArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("AshwoodArmor", this);
	}
	public override void Armor_UpdateEquipsSet() {
		Player.statDefense += 8;
		Player.GetDamage(DamageClass.Generic) += .1f;
	}
	public override void Armor_OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (!Player.ZoneUnderworldHeight && !Player.ZoneUnderworldHeight) {
			return;
		}
		target.AddBuff(BuffID.OnFire, 300);
	}
	public override void Armor_OnHitByNPC(NPC target, Player.HurtInfo hurtInfo) {
		OnHitEffect_AshWoodArmor(target);
	}
	public override void Armor_OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		OnHitEffect_AshWoodArmor(proj);
	}
	private void OnHitEffect_AshWoodArmor(Entity entity) {
		if (!Player.ZoneUnderworldHeight && !Player.ZoneUnderworldHeight) {
			return;
		}
		int proj = Projectile.NewProjectile(Player.GetSource_OnHurt(entity), Player.Center, (entity.Center - Player.Center).SafeNormalize(Vector2.UnitX) * 10, ProjectileID.Flames, Main.rand.Next(5, 15), 1f, Player.whoAmI);
		Main.projectile[proj].penetrate = -1;
	}
}
