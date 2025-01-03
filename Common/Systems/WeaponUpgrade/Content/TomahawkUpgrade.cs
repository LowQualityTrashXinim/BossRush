using BossRush.Contents.Perks;
using BossRush.Contents.Projectiles;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.Systems.WeaponUpgrade.Content;
internal class TomahawkUpgrade_GlobalItem : GlobalItem {
	public override void SetDefaults(Item entity) {
		if (!UpgradePlayer.Check_Upgrade(Main.CurrentPlayer, WeaponUpgradeID.TomahawkUpgrade)) {
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
				entity.damage += 20;
				entity.ArmorPenetration += 10;
				entity.shoot = ModContent.ProjectileType<TomahawkProjectile>();
				break;
		}
	}
	public override bool AltFunctionUse(Item item, Player player) {
		if (UpgradePlayer.Check_Upgrade(Main.CurrentPlayer, WeaponUpgradeID.TomahawkUpgrade)) {
			switch (item.type) {
				case ItemID.CopperAxe:
				case ItemID.TinAxe:
				case ItemID.IronAxe:
				case ItemID.LeadAxe:
				case ItemID.SilverAxe:
				case ItemID.TungstenAxe:
				case ItemID.GoldAxe:
				case ItemID.PlatinumAxe:
					return true;
			}
		}
		return base.AltFunctionUse(item, player);
	}
	public override bool CanUseItem(Item item, Player player) {
		if (UpgradePlayer.Check_Upgrade(Main.CurrentPlayer, WeaponUpgradeID.TomahawkUpgrade)) {
			if (player.altFunctionUse == 2) {
				item.noUseGraphic = true;
			}
			else {
				item.noUseGraphic = false;
			}
			return player.ownedProjectileCounts[ModContent.ProjectileType<TomahawkProjectile>()] < 1;
		}
		return base.CanUseItem(item, player);
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (!UpgradePlayer.Check_Upgrade(Main.CurrentPlayer, WeaponUpgradeID.TomahawkUpgrade)) {
			return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
		}
		TomahawkUpgrade_ModPlayer modplayer = player.GetModPlayer<TomahawkUpgrade_ModPlayer>();
		switch (item.type) {
			case ItemID.CopperAxe:
			case ItemID.TinAxe:
			case ItemID.IronAxe:
			case ItemID.LeadAxe:
			case ItemID.SilverAxe:
			case ItemID.TungstenAxe:
			case ItemID.GoldAxe:
			case ItemID.PlatinumAxe:
				if (player.altFunctionUse == 2 && !modplayer.ThrownCD) {
					velocity = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
					Projectile.NewProjectile(source, position, velocity * 13, type, damage, knockback, player.whoAmI, ai2: item.type);
					player.AddBuff(ModContent.BuffType<TomahawkCD>(), modplayer.ThrownMaximumCD);
				}
				return false;
		}
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
}
class TomahawkCD : ModBuff {
	public override string Texture => BossRushTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = true;
	}
	public override void Update(Player player, ref int buffIndex) {
		TomahawkUpgrade_ModPlayer modplayer = player.GetModPlayer<TomahawkUpgrade_ModPlayer>();
		modplayer.ThrownCD = true;
	}
}
public class TomahawkUpgrade_ModPlayer : ModPlayer {
	public bool ThrownCD = false;
	public int ThrownMaximumCD = 90;
	public override void ResetEffects() {
		ThrownMaximumCD = 90;
		ThrownCD = false;
	}
}
public class TomahawkUpgrade : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
		list_category.Add(PerkCategory.WeaponUpgrade);
	}
	public override void OnChoose(Player player) {
		UpgradePlayer.Add_Upgrade(player, WeaponUpgradeID.TomahawkUpgrade);
		BossRushUtils.Reflesh_GlobalItem(Mod, player);
	}
}
