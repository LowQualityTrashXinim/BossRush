using BossRush.Common.Global;
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
	public override void SetDefault() {
		PieceID = ItemID.CactusHelmet;
		Add_Defense = 4;
		AddTooltip = true;
		ArmorName = "CactusArmor";
		TypeEquipment = Type_Head;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.Thorn, Base: 10);
		modplayer.AddStatsToPlayer(PlayerStats.RegenHP, Base: 3);
	}
}
public class CactusBreastplate : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.CactusBreastplate;
		Add_Defense = 5;
		AddTooltip = true;
		ArmorName = "CactusArmor";
		TypeEquipment = Type_Body;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.Thorn, Base: 15);
		modplayer.AddStatsToPlayer(PlayerStats.MeleeDMG, Base: 4);
	}
}
public class CactusLeggings : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.CactusLeggings;
		Add_Defense = 4;
		AddTooltip = true;
		ArmorName = "CactusArmor";
		TypeEquipment = Type_Leg;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.Thorn, Base: 7);
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.Thorn, Base: 7);
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1.14f);
	}
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
