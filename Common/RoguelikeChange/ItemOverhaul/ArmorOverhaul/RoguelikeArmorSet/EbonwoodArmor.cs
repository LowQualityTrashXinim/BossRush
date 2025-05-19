using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BossRush.Contents.Projectiles;
using BossRush.Common.Global;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class EbonwoodArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.EbonwoodHelmet;
		bodyID = ItemID.EbonwoodBreastplate;
		legID = ItemID.EbonwoodGreaves;
	}
}
public class EbonwoodHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.EbonwoodHelmet;
		Add_Defense = 2;
		AddTooltip = true;
		ArmorName = "EbonwoodArmor";
		TypeEquipment = Type_Head;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.05f);
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 1.1f);
	}
}
public class EbonwoodBreastplate : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.EbonwoodBreastplate;
		Add_Defense = 2;
		AddTooltip = true;
		ArmorName = "EbonwoodArmor";
		TypeEquipment = Type_Body;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, Multiplicative: 1.11f);
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 1.12f);
	}
}
public class EbonwoodGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.EbonwoodGreaves;
		Add_Defense = 2;
		AddTooltip = true;
		ArmorName = "EbonwoodArmor";
		TypeEquipment = Type_Leg;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1.17f);
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.03f);
		modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 1.08f);
	}
}
public class EbonwoodArmorPlayer : PlayerArmorHandle {
	int EbonWoodArmorCD = 0;
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("EbonwoodArmor", this);
	}
	public override void Armor_UpdateEquipsSet() {
		PlayerStatsHandle modplayer = Player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.Defense, Base: 6);
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, 1.18f);
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, 1.1f);
		if (--EbonWoodArmorCD <= 0 && Player.velocity != Vector2.Zero) {
			Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + Main.rand.NextVector2Circular(10, 10), -Player.velocity.SafeNormalize(Vector2.Zero), ModContent.ProjectileType<CorruptionTrail>(), 3, 0, Player.whoAmI);
			EbonWoodArmorCD = 45;

		}
	}
}
