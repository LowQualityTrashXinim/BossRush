using BossRush.Common.RoguelikeChange.ItemOverhaul;
using BossRush.Texture;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace BossRush.Contents.Items.Weapon.UnfinishedItem;
class ShatterSky : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultMeleeShootCustomProjectile(94, 92, 100, 5f, 30, 30, ItemUseStyleID.Swing, 1, 1, true);
		MeleeWeaponOverhaul meleeItem = Item.GetGlobalItem<MeleeWeaponOverhaul>();
		meleeItem.SwingType = BossRushUseStyle.SwipeDown;
		meleeItem.SwingStrength = 11;
		meleeItem.CircleSwingAmount = 2.6f;
		meleeItem.DistanceThrust = 150;
		meleeItem.OffsetThrust = 20;
	}
	int ComboCounter = 0;
	public override bool CanUseItem(Player player) {
		if (!player.ItemAnimationActive) {
			ComboCounter++;
			MeleeWeaponOverhaul overhaul = Item.GetGlobalItem<MeleeWeaponOverhaul>();
			switch (ComboCounter) {
				case 1:
					overhaul.HideSwingVisual = false;
					overhaul.SwingType = BossRushUseStyle.SwipeDown;
					overhaul.SwingStrength = 11;
					break;
				case 2:
					overhaul.SwingType = BossRushUseStyle.SwipeUp;
					overhaul.SwingStrength = 11;
					break;
				case 3:
					overhaul.SwingType = BossRushUseStyle.Spin;
					break;
				case 4:
					overhaul.SwingType = BossRushUseStyle.Thrust;
					overhaul.SwingStrength = 15;
					overhaul.HideSwingVisual = true;
					break;
			}
			if (ComboCounter >= 4) {
				ComboCounter = 0;
			}
		}
		return base.CanUseItem(player);
	}
	public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
		base.HoldSynergyItem(player, modplayer);
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		type = ProjectileID.CultistBossLightningOrbArc;
		if (ComboCounter == 3) {
			for (int i = 0; i < 16; i++) {
				Vector2 vel = velocity.Vector2DistributeEvenlyPlus(16, 360, i) * 10;
				Projectile projectile = Projectile.NewProjectileDirect(source, position, vel, type, damage, knockback, player.whoAmI, vel.ToRotation());
				projectile.friendly = true;
				projectile.hostile = false;
				projectile.extraUpdates = 3;
				projectile.penetrate = -1;
				projectile.maxPenetrate = -1;
			}
		}
		else if (ComboCounter == 0) {
			Projectile projectile = Projectile.NewProjectileDirect(source, position, velocity * 10, type, damage, knockback, player.whoAmI, velocity.ToRotation());
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.extraUpdates = 5;
			projectile.penetrate = -1;
			projectile.maxPenetrate = -1;
		}
		CanShootItem = false;
	}
	public override float UseTimeMultiplier(Player player) {
		return base.UseTimeMultiplier(player);
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.BreakerBlade)
			.AddIngredient(ItemID.NimbusRod)
			.Register();
	}
}
