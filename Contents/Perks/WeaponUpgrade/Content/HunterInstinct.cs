﻿using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Contents.Perks;
using BossRush.Contents.Perks.WeaponUpgrade;
using BossRush.Contents.Items.Chest;
using BossRush.Common.Global;

namespace BossRush.Contents.Perks.WeaponUpgrade.Content;
internal class HunterInstinct_GlobalItem : GlobalItem {
	public override void SetDefaults(Item entity) {
		if (!UpgradePlayer.Check_Upgrade(Main.CurrentPlayer, WeaponUpgradeID.HunterInstinct)) {
			return;
		}
		if (entity.axe <= 0 || entity.noMelee) {
			return;
		}
		switch (entity.type) {
			case ItemID.CopperAxe:
			case ItemID.TinAxe:
			case ItemID.IronAxe:
			case ItemID.LeadAxe:
			case ItemID.SilverAxe:
			case ItemID.TungstenAxe:
			case ItemID.GoldAxe:
			case ItemID.PlatinumAxe:
				entity.damage += 30;
				entity.ArmorPenetration += 20;
				entity.crit += 10;
				break;
		}
	}
	public override void HoldItem(Item item, Player player) {
		if (!UpgradePlayer.Check_Upgrade(player, WeaponUpgradeID.HunterInstinct)) {
			return;
		}
		switch (item.type) {
			case ItemID.CopperAxe:
			case ItemID.TinAxe:
			case ItemID.IronAxe:
			case ItemID.LeadAxe:
			case ItemID.SilverAxe:
			case ItemID.TungstenAxe:
			case ItemID.GoldAxe:
			case ItemID.PlatinumAxe:
				player.GetModPlayer<PlayerStatsHandle>().DodgeChance += .15f;
				break;
		}
	}
	public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!UpgradePlayer.Check_Upgrade(player, WeaponUpgradeID.HunterInstinct)) {
			return;
		}
		if (Main.rand.NextBool(10)) {
			switch (item.type) {
				case ItemID.CopperAxe:
				case ItemID.TinAxe:
				case ItemID.IronAxe:
				case ItemID.LeadAxe:
				case ItemID.SilverAxe:
				case ItemID.TungstenAxe:
				case ItemID.GoldAxe:
				case ItemID.PlatinumAxe:
					player.AddBuff(ModContent.BuffType<SwiftStrike_Axe>(), BossRushUtils.ToSecond(Main.rand.Next(4, 7)));
					break;
			}
		}
	}
}
public class SwiftStrike_Axe : ModBuff {
	public override string Texture => BossRushTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.AttackSpeed, 1.15f);
	}
}
public class HunterInstinct : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
		list_category.Add(PerkCategory.WeaponUpgrade);
	}
	public override void OnChoose(Player player) {
		UpgradePlayer.Add_Upgrade(player, WeaponUpgradeID.HunterInstinct);
		Mod.Reflesh_GlobalItem(player);
		int[] Orestaff = {
		ItemID.CopperAxe,
		ItemID.TinAxe,
		ItemID.IronAxe,
		ItemID.LeadAxe,
		ItemID.SilverAxe,
		ItemID.TungstenAxe,
		ItemID.GoldAxe,
		ItemID.PlatinumAxe
		};
		int weapontype = Main.rand.Next(Orestaff);
		player.QuickSpawnItem(player.GetSource_Misc("WeaponUpgrade"), weapontype);
		LootBoxBase.AmmoForWeapon(player, weapontype);
	}
}
