using BossRush.Common.RoguelikeChange.ItemOverhaul;
using BossRush.Contents.Items.Weapon;
using BossRush.Contents.Items.Weapon.RangeSynergyWeapon.SkullRevolver;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.NoneSynergy;
internal class OldSkullRevolver : ModItem {
	public override string Texture => BossRushUtils.GetTheSameTextureAsEntity<SkullRevolver>();
	int counter = 0;
	public override void SetDefaults() {
		Item.BossRushDefaultRange(26, 52, 25, 3f, 10, 60, ItemUseStyleID.Shoot, ProjectileID.Bullet, 20f, false, AmmoID.Bullet);
		Item.rare = ItemRarityID.Orange;
		Item.UseSound = SoundID.Item11;
		Item.crit = 15;
		Item.reuseDelay = 57;
		Item.value = Item.buyPrice(gold: 50);
		Item.UseSound = SoundID.Item41;
		if (Item.TryGetGlobalItem(out RangeWeaponOverhaul weapon)) {
			weapon.SpreadAmount = 10;
			weapon.OffSetPost = 50;
		}
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		SoundEngine.PlaySound(Item.UseSound);
		counter++;
		if (counter == 2) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.BookOfSkullsSkull, damage, knockback, player.whoAmI);
		}
		if (counter == 4) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.ClothiersCurse, damage, knockback, player.whoAmI);
			counter = 0;
		}
		return true;
	}
	public override Vector2? HoldoutOffset() {
		return new Vector2(-3, 0);
	}
}
