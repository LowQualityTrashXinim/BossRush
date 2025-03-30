using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Skill;
using BossRush.Common.Utils;
using BossRush.Contents.Items.Chest;
using BossRush.Common.Global;

namespace BossRush.Contents.Perks.WeaponUpgrade.Content;
internal class TripletShot_GlobalItem : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.useAmmo == AmmoID.Arrow;
	}
	public override void SetDefaults(Item entity) {
		if (entity.useAmmo == AmmoID.Arrow && UpgradePlayer.Check_Upgrade(Main.CurrentPlayer, WeaponUpgradeID.TripletShot)) {
			entity.damage = Math.Clamp(entity.damage - 4, 1, int.MaxValue);
		}
	}
}
public class TripletShoot_ModPlayer : ModPlayer {
	public override void UpdateEquips() {
		Item item = Player.HeldItem;
		if (UpgradePlayer.Check_Upgrade(Player, WeaponUpgradeID.TripletShot) && item.useAmmo == AmmoID.Arrow) {
			Player.GetModPlayer<PlayerStatsHandle>().Request_ShootSpreadExtra(3, 25);
			Player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.AttackSpeed, 1 - .15f);
		}
	}
}
public class TripletShot : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
		list_category.Add(PerkCategory.WeaponUpgrade);
	}
	public override void OnChoose(Player player) {
		UpgradePlayer.Add_Upgrade(player, WeaponUpgradeID.TripletShot);
		Mod.Reflesh_GlobalItem(player);
		int[] Orestaff = {
ItemID.WoodenBow, ItemID.BorealWoodBow, ItemID.RichMahoganyBow,ItemID.PalmWoodBow, ItemID.EbonwoodBow, ItemID.ShadewoodBow,
				ItemID.CopperBow, ItemID.TinBow, ItemID.IronBow, ItemID.LeadBow, ItemID.SilverBow,ItemID.TungstenBow, ItemID.GoldBow, ItemID.PlatinumBow,
		};
		int weapon = Main.rand.Next(Orestaff);
		player.QuickSpawnItem(player.GetSource_Misc("WeaponUpgrade"), weapon);
		LootBoxBase.AmmoForWeapon(player, weapon);
	}
}
