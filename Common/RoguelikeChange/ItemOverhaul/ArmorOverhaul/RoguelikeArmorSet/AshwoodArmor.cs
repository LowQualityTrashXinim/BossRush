using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Common.Systems;

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
		AddTooltip = true;
		ArmorName = "AshwoodArmor";
		TypeEquipment = Type_Head;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetModPlayer<AshwoodPlayer>().Multiplier += .09f;
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.MaxHP, Base: 20);
	}
}
public class AshWoodBreastplate : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.AshWoodBreastplate;
		Add_Defense = 4;
		AddTooltip = true;
		ArmorName = "AshwoodArmor";
		TypeEquipment = Type_Body;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetModPlayer<AshwoodPlayer>().Multiplier += .11f;
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.MaxHP, Base: 30);
	}
}
public class AshWoodGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.AshWoodGreaves;
		Add_Defense = 3;
		AddTooltip = true;
		ArmorName = "AshwoodArmor";
		TypeEquipment = Type_Leg;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetModPlayer<AshwoodPlayer>().Multiplier += .05f;
		player.GetModPlayer<AshwoodPlayer>().FlameWalker = true;
	}
}
public class AshwoodPlayer : ModPlayer {
	public float Multiplier = 1;
	public bool FlameWalker = false;
	public override void ResetEffects() {
		Multiplier = 1;
	}
	public override void UpdateEquips() {
		Point tile = Player.position.ToTileCoordinates();
		bool CheckTileBelow1 = !WorldGen.TileEmpty(tile.X, tile.Y + 3);
		bool CheckTileBelow2 = !WorldGen.TileEmpty(tile.X, tile.Y + 4);
		if (FlameWalker && Player.velocity.IsLimitReached(3) && (CheckTileBelow1 || CheckTileBelow2)) {
			if (Main.rand.NextBool(10)) {
				Vector2 vel = Vector2.UnitX * Player.direction * 2;
				Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center.Subtract(0, 10), vel, ProjectileID.WandOfSparkingSpark, 8, 1f, Player.whoAmI);
			}
		}
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
			modifiers.SourceDamage *= Multiplier;
		}
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
	public override void Armor_OnHitByNPC(NPC target, Player.HurtInfo hurtInfo) {
		OnHitEffect_AshWoodArmor(target);
	}
	public override void Armor_OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		OnHitEffect_AshWoodArmor(proj);
	}
	private void OnHitEffect_AshWoodArmor(Entity entity) {
		for (int i = 0; i < 3; i++) {
			int proj = Projectile.NewProjectile(Player.GetSource_OnHurt(entity), Player.Center, (entity.Center - Player.Center).SafeNormalize(Vector2.UnitX).Vector2RotateByRandom(30) * Main.rand.NextFloat(7, 11), ProjectileID.Flames, Main.rand.Next(5, 15), 1f, Player.whoAmI);
			Main.projectile[proj].penetrate = -1;
		}
	}
}
