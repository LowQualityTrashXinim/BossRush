using BossRush.Common;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.ArcaneRange.LaserSniper;
internal class LaserSniper : SynergyModItem {
	public override void Synergy_SetStaticDefaults() {
		SynergyBonus_System.Add_SynergyBonus(Type, ItemID.LaserRifle);
	}
	public override void SetDefaults() {
		Item.BossRushDefaultRange(86, 26, 250, 20, 40, 40, ItemUseStyleID.Shoot, ProjectileID.LaserMachinegunLaser, 20, false, AmmoID.Bullet);
		Item.crit = 20;
		Item.UseSound = SoundID.Item91 with {
			Pitch = .9f
		};
		Item.rare = ItemRarityID.Orange;
		Item.value = Item.buyPrice(gold: 50);
		Item.scale = 0.9f;
		Item.mana = 20;
		Item.DamageType = ModContent.GetInstance<RangeMageHybridDamageClass>();
	}
	public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
		if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.LaserRifle)) {
			tooltips.Add(new(Mod, "LaserSniper_LaserRifle", $"[i:{ItemID.LaserRifle}] : Activate rifle mode"));
		}
	}
	public override bool? CanAutoReuseItem(Player player) {
		bool result = SynergyBonus_System.Check_SynergyBonus(Type, ItemID.LaserRifle);
		if (result) {
			return true;
		}
		return base.CanAutoReuseItem(player);
	}
	public override float UseSpeedMultiplier(Player player) {
		float speed = base.UseSpeedMultiplier(player);
		if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.LaserRifle)) {
			speed += 2.5f;
		}
		return speed;
	}
	public override void OnMissingMana(Player player, int neededMana) {
		player.GetModPlayer<LaserSniperPlayer>().ManaMissing = true;
		player.statMana += neededMana;
	}
	public override Vector2? HoldoutOffset() {
		return new Vector2(-20, 0);
	}
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (player.GetModPlayer<LaserSniperPlayer>().ManaMissing) {
			damage = (int)(damage * .8f);
		}
		type = ProjectileID.LaserMachinegunLaser;
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {

		Projectile projectile = Projectile.NewProjectileDirect(source, position.PositionOFFSET(velocity, 90), velocity, type, damage, knockback, player.whoAmI);

		projectile.extraUpdates += 10;
		if (!player.GetModPlayer<LaserSniperPlayer>().ManaMissing) {
			projectile.penetrate = 10;
			projectile.maxPenetrate = 10;
		}
		CanShootItem = false;
	}
}
public class LaserSniperPlayer : ModPlayer {
	public bool ManaMissing = false;
	public override void ResetEffects() {
		ManaMissing = false;
	}
}
