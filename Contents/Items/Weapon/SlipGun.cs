using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Contents.Items.Weapon;
internal class SlipGun : ModItem {
	public override string Texture => BossRushTexture.MISSINGTEXTURE;
	public override void SetDefaults() {
		Item.BossRushDefaultRange(30, 30, 50, 5f, 20, 20, ItemUseStyleID.Shoot, ProjectileID.Bullet, 30, true, AmmoID.Bullet);

		Item.rare = ItemRarityID.Pink;
		Item.value = Item.sellPrice(gold: 50);
	}
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		SlipGun_ModPlayer modplayer = Main.LocalPlayer.GetModPlayer<SlipGun_ModPlayer>();
		tooltips.Add(new TooltipLine(Mod, "Chamber", $"[c/{Color.Yellow.Hex3()}:Current ammo count {modplayer.Chamber}"));
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		SlipGun_ModPlayer modplayer = player.GetModPlayer<SlipGun_ModPlayer>();
		for (int i = 0; i < modplayer.Chamber; i++) {
			Projectile.NewProjectile(source, position, velocity.RotatedBy(5), type, damage, knockback, player.whoAmI);
		}
		return base.Shoot(player, source, position, velocity, type, damage, knockback);
	}
	public override void OnConsumeAmmo(Item ammo, Player player) {
		base.OnConsumeAmmo(ammo, player);
	}
	public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
		
	}
}
class SlipGun_ModPlayer : ModPlayer {
	/// <summary>
	/// Represent the amount of ammo in the chamber
	/// </summary>
	public int Chamber = 0;
	public int Reload_CoolDown = 0;
	public override void PreUpdate() {
		if (Player.HeldItem.type != ModContent.ItemType<SlipGun>()) {
			return;
		}
		Reload_CoolDown = BossRushUtils.CountDown(Reload_CoolDown);
		if (Main.mouseRight && CanConsumeAmmo(Player.HeldItem, Player.ChooseAmmo(Player.HeldItem))) {
			Chamber = Math.Clamp(++Chamber, 0, 6);
			Reload_CoolDown = 60;
		}
	}
	public override bool CanConsumeAmmo(Item weapon, Item ammo) {
		if (ammo.stack > 0) {
			return base.CanConsumeAmmo(weapon, ammo);
		}
		return false;
	}
	public override bool CanUseItem(Item item) {
		if (item.ModItem is SlipGun) {
			return Reload_CoolDown == 0;
		}
		return base.CanUseItem(item);
	}
}
