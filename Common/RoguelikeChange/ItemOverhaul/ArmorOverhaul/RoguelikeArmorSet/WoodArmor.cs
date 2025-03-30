using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Projectiles;
using BossRush.Common.Global;

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
		AddTooltip = true;
		ArmorName = "WoodArmor";
		TypeEquipment = Type_Head;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetModPlayer<RoguelikeArmorPlayer>().AcornSpawnChance += .02f;
		player.GetCritChance<RangedDamageClass>() += 5;
	}
}
class WoodBreastplate : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.WoodBreastplate;
		Add_Defense = 4;
		AddTooltip = true;
		ArmorName = "WoodArmor";
		TypeEquipment = Type_Body;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetModPlayer<RoguelikeArmorPlayer>().AcornSpawnChance += .03f;
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.RangeDMG, Multiplicative: 1.1f);
	}
}
class WoodGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.WoodGreaves;
		Add_Defense = 3;
		AddTooltip = true;
		ArmorName = "WoodArmor";
		TypeEquipment = Type_Leg;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetModPlayer<RoguelikeArmorPlayer>().AcornSpawnChance += .01f;
	}
}
class WoodArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("WoodArmor", this);
	}
	public override void Armor_UpdateEquipsSet() {
		Player.statDefense += 5;
		Player.moveSpeed += .25f;
		Player.GetModPlayer<RoguelikeArmorPlayer>().AcornSpawnChance += .1f;
	}
	public override void Armor_OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		int typeProjectile = ModContent.ProjectileType<SunflowerProjectile>();
		if (Main.rand.NextBool(20) && Player.ownedProjectileCounts[typeProjectile] < 1) {
			Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.Zero, typeProjectile, 0, 0, Player.whoAmI, Main.rand.Next(0, 270));
		}
	}
}
