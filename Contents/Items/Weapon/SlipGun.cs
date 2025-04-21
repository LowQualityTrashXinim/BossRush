using System;
using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SteelSeries.GameSense;
using Terraria.GameContent;
using Terraria.Audio;

namespace BossRush.Contents.Items.Weapon;
internal class SlipGun : ModItem {
	public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Revolver);
	public override void SetDefaults() {
		Item.BossRushDefaultRange(30, 30, 50, 5f, 20, 20, ItemUseStyleID.Shoot, ProjectileID.Bullet, 15, true, AmmoID.Bullet);

		Item.rare = ItemRarityID.Pink;
		Item.value = Item.sellPrice(gold: 50);
	}
	public override void ModifyTooltips(List<TooltipLine> tooltips) {
		SlipGun_ModPlayer modplayer = Main.LocalPlayer.GetModPlayer<SlipGun_ModPlayer>();
		tooltips.Add(new TooltipLine(Mod, "Chamber", $"[c/{Color.Yellow.Hex3()}:Current ammo count {modplayer.Chamber}]"));
	}
	public override void HoldItem(Player player) {
		SlipGun_ModPlayer modplayer = player.GetModPlayer<SlipGun_ModPlayer>();
		int chamber = modplayer.Chamber;
		Item.stack = Math.Clamp(chamber, 1, 6);
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		SlipGun_ModPlayer modplayer = player.GetModPlayer<SlipGun_ModPlayer>();
		for (int i = 0; i < modplayer.Chamber; i++) {
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(5), type, damage, knockback, player.whoAmI);
		}
		if (modplayer.Chamber != 0) {
			SoundEngine.PlaySound(SoundID.Item38 with { Pitch = 1f - MathHelper.Lerp(0, 2, modplayer.Chamber / 6f) });
			modplayer.Chamber = 0;
		}
		else {
			SoundEngine.PlaySound(SoundID.Item98 with { Pitch = 1f });
		}
		return false;
	}
	public override void OnConsumeAmmo(Item ammo, Player player) {
		base.OnConsumeAmmo(ammo, player);
	}
	public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
		SlipGun_ModPlayer modplayer = Main.LocalPlayer.GetModPlayer<SlipGun_ModPlayer>();
		if (modplayer.Chamber >= 6) {
			return;
		}
		float quotient = 1 - modplayer.Reload_CoolDown / 30f; // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.
		quotient = Math.Clamp(quotient, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

		// Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
		Rectangle hitbox = frame;
		hitbox.X += 12 + (int)(position.X - origin.X);
		hitbox.Width += -24;
		hitbox.Y += 8 + (int)position.Y;
		hitbox.Height += -24;

		// Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
		int left = hitbox.Left;
		int right = hitbox.Right;
		int steps = (int)((right - left) * quotient);
		for (int i = 0; i < steps; i += 1) {
			// float percent = (float)i / steps; // Alternate Gradient Approach
			float percent = (float)i / (right - left);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(new(255, 255, 255, 0), new(255, 255, 255, 150), percent));
		}
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
		if (Reload_CoolDown <= 0 && Main.mouseRight && CanConsumeAmmo(Player.HeldItem, Player.ChooseAmmo(Player.HeldItem))) {
			Chamber = Math.Clamp(++Chamber, 0, 6);
			Reload_CoolDown = 30;
			SoundEngine.PlaySound(SoundID.Grab with { Pitch = -1f });
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
