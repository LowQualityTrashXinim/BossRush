using Terraria;
using Terraria.ModLoader;
using BossRush.Contents.Perks;
using Terraria.ID;
using BossRush.Contents.Projectiles;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Perks.WeaponUpgrade.Content;
public class EnhancedMagicStaff_GlobalItem : GlobalItem {
	public override void SetDefaults(Item entity) {
		if (!UpgradePlayer.Check_Upgrade(Main.CurrentPlayer, WeaponUpgradeID.EnhancedMagicStaff)) {
			return;
		}
		switch (entity.type) {
			case ItemID.AmethystStaff:
				entity.shoot = ModContent.ProjectileType<AmethystMagicalBolt>();
				entity.useTime = 3;
				entity.useAnimation = 15;
				entity.reuseDelay = 30;
				entity.mana = 6;
				entity.shootSpeed = 1;
				entity.damage += 5;
				break;
			case ItemID.TopazStaff:
				entity.shoot = ModContent.ProjectileType<TopazMagicalBolt>();
				entity.useTime = 3;
				entity.useAnimation = 18;
				entity.reuseDelay = 33;
				entity.mana = 6;
				entity.shootSpeed = 1;
				entity.damage += 5;
				break;
			case ItemID.SapphireStaff:
				entity.shoot = ModContent.ProjectileType<SapphireMagicalBolt>();
				entity.useTime = 3;
				entity.useAnimation = 18;
				entity.mana = 6;
				entity.damage -= 3;
				entity.shootSpeed = 1;
				break;
			case ItemID.EmeraldStaff:
				entity.shoot = ModContent.ProjectileType<EmeraldMagicalBolt>();
				entity.useTime = 10;
				entity.useAnimation = 20;
				entity.mana = 6;
				entity.shootSpeed = 1;
				break;
			case ItemID.RubyStaff:
				entity.damage += 10;
				entity.shoot = ModContent.ProjectileType<RubyMagicalBolt>();
				entity.shootSpeed = 1;
				break;
			case ItemID.DiamondStaff:
				entity.damage += 10;
				entity.shoot = ModContent.ProjectileType<DiamondMagicalBolt>();
				entity.shootSpeed = 1;
				break;
		}
	}
	public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (!UpgradePlayer.Check_Upgrade(player, WeaponUpgradeID.EnhancedMagicStaff)) {
			return;
		}
		switch (item.type) {
			case ItemID.AmethystStaff:
				velocity = velocity.Vector2RotateByRandom(10);
				position = position.PositionOFFSET(velocity, 50);
				break;
			case ItemID.TopazStaff:
				velocity = velocity.Vector2RotateByRandom(15) * Main.rand.NextFloat(.75f, 1.25f);
				position = position.PositionOFFSET(velocity, 50);
				break;
			case ItemID.SapphireStaff:
				velocity = velocity.Vector2RotateByRandom(6) * Main.rand.NextFloat(.75f, 1.25f);
				position = position.PositionOFFSET(velocity, 50);
				break;
			case ItemID.EmeraldStaff:
				velocity *= Main.rand.NextFloat(1, 1.5f);
				position = position.PositionOFFSET(velocity, 50);
				break;
			case ItemID.DiamondStaff:
				position = position.PositionOFFSET(velocity, 50);
				break;
			case ItemID.RubyStaff:
				position = position.PositionOFFSET(velocity, 50);
				break;
		}
	}
}
internal class EnhancedMagicStaff : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
		list_category.Add(PerkCategory.WeaponUpgrade);
	}
	public override void OnChoose(Player player) {
		UpgradePlayer.Add_Upgrade(player, WeaponUpgradeID.EnhancedMagicStaff);
		Mod.Reflesh_GlobalItem(player);
	}
}
