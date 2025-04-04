using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BossRush.Contents.Items.Chest;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Perks.WeaponUpgrade.Content;
public class LegendaryWoodSword_GlobalItem : GlobalItem {
	public override void SetDefaults(Item entity) {
		if (!UpgradePlayer.Check_Upgrade(Main.CurrentPlayer, WeaponUpgradeID.LegendaryWoodSword)) {
			return;
		}
		switch (entity.type) {
			case ItemID.WoodenSword:
			case ItemID.AshWoodSword:
			case ItemID.BorealWoodSword:
			case ItemID.RichMahoganySword:
			case ItemID.PalmWoodSword:
			case ItemID.EbonwoodSword:
			case ItemID.ShadewoodSword:
			case ItemID.PearlwoodSword:
				entity.damage += 20;
				entity.scale += .5f;
				entity.shoot = ModContent.ProjectileType<WoodProjectile>();
				entity.shootSpeed = 12;
				break;
		}
	}
}
public class LegendaryWoodSword_ModPlayer : ModPlayer {
	public int Counter = 0;
	public override bool CanShoot(Item item) {
		if (!UpgradePlayer.Check_Upgrade(Player, WeaponUpgradeID.LegendaryWoodSword)) {
			return base.CanShoot(item);
		}
		switch (item.type) {
			case ItemID.WoodenSword:
			case ItemID.AshWoodSword:
			case ItemID.BorealWoodSword:
			case ItemID.RichMahoganySword:
			case ItemID.PalmWoodSword:
			case ItemID.EbonwoodSword:
			case ItemID.ShadewoodSword:
			case ItemID.PearlwoodSword:
				if (Player.ItemAnimationJustStarted) {
					Counter = BossRushUtils.Safe_SwitchValue(Counter, 3,1);
				}
				return Counter >= 3;
		}
		return base.CanShoot(item);
	}
	public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		switch (item.type) {
			case ItemID.WoodenSword:
			case ItemID.AshWoodSword:
			case ItemID.BorealWoodSword:
			case ItemID.RichMahoganySword:
			case ItemID.PalmWoodSword:
			case ItemID.EbonwoodSword:
			case ItemID.ShadewoodSword:
			case ItemID.PearlwoodSword:
				damage *= 2;
				break;
		}
	}
}
public class WoodProjectile : ModProjectile {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Wood);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 300;
		Projectile.penetrate = 1;
		Projectile.friendly = true;
	}
	public override void AI() {
		if (++Projectile.ai[0] >= 10) {
			Projectile.velocity.Y += .25f;
		}
		Projectile.rotation += MathHelper.ToRadians(20) * Projectile.direction;
		Projectile.velocity *= .99f;
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 10; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.WoodFurniture);
			dust.velocity = Main.rand.NextVector2Circular(5, 5);
		}
	}
}
public class LegendaryWoodSword : Perk {
	public override void SetDefaults() {
		CanBeStack = false;
		list_category.Add(PerkCategory.WeaponUpgrade);
	}
	public override void OnChoose(Player player) {
		UpgradePlayer.Add_Upgrade(player, WeaponUpgradeID.LegendaryWoodSword);
		Mod.Reflesh_GlobalItem(player);
		int[] Orestaff = {
			ItemID.WoodenSword,
		ItemID.AshWoodSword,
		ItemID.BorealWoodSword,
		ItemID.RichMahoganySword,
		ItemID.PalmWoodSword,
		ItemID.EbonwoodSword,
		ItemID.ShadewoodSword,
		ItemID.PearlwoodSword,
		};
		int weaponType = Main.rand.Next(Orestaff);
		player.QuickSpawnItem(player.GetSource_Misc("WeaponUpgrade"), weaponType);
		LootBoxBase.AmmoForWeapon(player, weaponType);
	}
}

