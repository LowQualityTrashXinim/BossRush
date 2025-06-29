using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Common.RoguelikeChange.ItemOverhaul.ItemOverhaul;
public class Roguelike_CopperBow : GlobalItem {
	public override void SetDefaults(Item entity) {
		if (entity.type == ItemID.CopperBow) {
			entity.damage += 5;
			entity.useTime = entity.useAnimation = 23;
			entity.shootSpeed = 15;
		}
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (item.type != ItemID.CopperBow) {
			return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
		}
		int Counter = player.GetModPlayer<Roguelike_CopperBow_ModPlayer>().CopperBow_Counter;
		if (Counter >= 90) {
			int amount = 8;
			if (Counter == 150) {
				amount += 5;
			}
			for (int i = 0; i < amount; i++) {
				Projectile projectile = Projectile.NewProjectileDirect(source, position, velocity.Vector2RotateByRandom(30) * Main.rand.NextFloat(.7f, 1f), ProjectileID.ThunderSpearShot, (int)(damage * 1.25f), knockback, player.whoAmI);
				projectile.DamageType = DamageClass.Ranged;
				projectile.extraUpdates = 2;
				projectile.alpha -= 120;
			}
		}
		if (item.type == ItemID.CopperBow) {
			Projectile projectile = Projectile.NewProjectileDirect(source, position, velocity, ProjectileID.ThunderSpearShot, (int)(damage * 1.25f), knockback, player.whoAmI);
			projectile.DamageType = DamageClass.Ranged;
			projectile.extraUpdates = 2;
			projectile.alpha -= 120;
		}
		player.GetModPlayer<Roguelike_CopperBow_ModPlayer>().CopperBow_Counter = -player.itemAnimationMax;
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (item.type == ItemID.CopperBow) {
			BossRushUtils.AddTooltip(ref tooltips, new(Mod, "Roguelike_CoperBow", BossRushUtils.LocalizationText("RoguelikeRework", item.Name)));
		}
	}
}
public class Roguelike_CopperBow_ModPlayer : ModPlayer {
	public int CopperBow_Counter = 0;
	public override void ResetEffects() {
		CopperBow_Counter++;
		if (CopperBow_Counter > 150) {
			CopperBow_Counter = 150;
		}
	}
}
