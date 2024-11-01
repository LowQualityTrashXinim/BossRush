using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class CactusArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.CactusHelmet;
		bodyID = ItemID.CactusBreastplate;
		legID = ItemID.CactusLeggings;
	}
}
public class CactusHelmet : ModArmorPiece {
	public override int Add_Defense => 4;
	public override int _pieceID => ItemID.CactusHelmet;
}
public class CactusBreastplate : ModArmorPiece {
	public override int Add_Defense => 5;
	public override int _pieceID => ItemID.CactusBreastplate;
}
public class CactusLeggings : ModArmorPiece {
	public override int Add_Defense => 4;
	public override int _pieceID => ItemID.CactusLeggings;
}

public class CactusArmorPlayer : PlayerArmorHandle {
	int CactusArmorCD = 0;
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("CactusArmor", this);
	}
	public override void Armor_ResetEffects() {
		CactusArmorCD = BossRushUtils.CountDown(CactusArmorCD);
	}
	public override void Armor_UpdateEquipsSet() {
		Player.statDefense += 10;
	}
	public override void Armor_OnHitByNPC(NPC target, Player.HurtInfo hurtInfo) {
		OnHitEffect_CactusArmor(target);
	}
	public override void Armor_OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		OnHitEffect_CactusArmor(proj);
	}
	private void OnHitEffect_CactusArmor(Entity entity) {
		if (CactusArmorCD <= 0) {
			bool manualDirection = Player.Center.X < entity.Center.X;
			Vector2 AbovePlayer = Player.Center + new Vector2(Main.rand.NextFloat(-500, 500), -1000);
			int projectile = Projectile.NewProjectile(Player.GetSource_OnHurt(entity), AbovePlayer, Vector2.UnitX * .1f * manualDirection.ToDirectionInt(), ProjectileID.RollingCactus, 150, 0, Player.whoAmI);
			Main.projectile[projectile].friendly = true;
			Main.projectile[projectile].hostile = false;
			CactusArmorCD = 300;
		}
		for (int i = 0; i < 8; i++) {
			Vector2 vec = Vector2.One.Vector2DistributeEvenly(8, 360, i);
			int projectile = Projectile.NewProjectile(Player.GetSource_OnHurt(entity), Player.Center, vec, ProjectileID.RollingCactusSpike, 15, 0, Player.whoAmI);
			Main.projectile[projectile].friendly = true;
			Main.projectile[projectile].hostile = false;
			Main.projectile[projectile].penetrate = -1;
		}

	}
}
