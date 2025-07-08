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
public class Roguelike_PhoenixBlaster : GlobalItem {
	public override void SetDefaults(Item entity) {
		if (entity.type == ItemID.PhoenixBlaster) {
			entity.damage += 12;
			entity.useTime = entity.useAnimation = 30;
			entity.knockBack += 1;
			entity.crit = 6;
		}
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (item.type == ItemID.PhoenixBlaster) {
			BossRushUtils.AddTooltip(ref tooltips, new(Mod, "Roguelike_PhoenixBlaster", BossRushUtils.LocalizationText("RoguelikeRework", item.Name)));
		}
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (item.type != ItemID.PhoenixBlaster) {
			return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
		}
		int Counter = player.GetModPlayer<Roguelike_PhoenixBlaster_ModPlayer>().PhoenixBlaster_Counter;
		if (++player.GetModPlayer<Roguelike_PhoenixBlaster_ModPlayer>().PhoenixBlaster_ShootCounter >= 5) {
			player.GetModPlayer<Roguelike_PhoenixBlaster_ModPlayer>().PhoenixBlaster_ShootCounter = 0;
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(30), ProjectileID.Flamelash, (int)(damage * 1.5f), knockback, player.whoAmI);
		}
		if (Counter >= 90) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.DD2PhoenixBowShot, damage * 3, knockback * 3, player.whoAmI);
			Counter -= 90;
			Counter = Counter / 10;
			for (int i = 0; i < Counter; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(30), ProjectileID.Flamelash, (int)(damage * 1.5f), knockback, player.whoAmI);
			}
		}
		player.GetModPlayer<Roguelike_PhoenixBlaster_ModPlayer>().PhoenixBlaster_Counter = -player.itemAnimationMax;
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
}
public class Roguelike_PhoenixBlaster_ModPlayer : ModPlayer {
	public int PhoenixBlaster_Counter = 0;
	public int PhoenixBlaster_ShootCounter = 0;
	public override void ResetEffects() {
		if (++PhoenixBlaster_Counter >= 150) {
			PhoenixBlaster_Counter = 150;
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.Check_ItemTypeSource(ItemID.PhoenixBlaster) && target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
			modifiers.FinalDamage.Base += 10;
		}
	}
}
