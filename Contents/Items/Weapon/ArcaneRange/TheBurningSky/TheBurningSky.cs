using Terraria.ID;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using BossRush.Common;
using Terraria.ModLoader;
using System.Linq;

namespace BossRush.Contents.Items.Weapon.ArcaneRange.TheBurningSky;
internal class TheBurningSky : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(44, 80, 34, 5f, 6, 18, ItemUseStyleID.Shoot, ProjectileID.FireArrow, 12, true, AmmoID.Arrow);
		Item.reuseDelay = 14;
		Item.UseSound = SoundID.Item5;
		Item.DamageType = ModContent.GetInstance<RangeMageHybridDamageClass>();
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		CanShootItem = false;
		int[] met = new int[] { ProjectileID.Meteor1, ProjectileID.Meteor2, ProjectileID.Meteor3 };
		if (type == ProjectileID.WoodenArrowFriendly)
			type = ProjectileID.FireArrow;
		SoundEngine.PlaySound(Item.UseSound);
		Vector2 newPos = Main.MouseWorld.Add(Main.rand.NextFloat(-120, 120), 900 + Main.rand.NextFloat(-50, 50));
		Vector2 vel = (Main.MouseWorld - newPos).SafeNormalize(Vector2.Zero) * Item.shootSpeed;
		int arrow = Projectile.NewProjectile(source, newPos, vel.Vector2RotateByRandom(5), type, damage, knockback, player.whoAmI);
		Main.projectile[arrow].extraUpdates += 1;
		Main.projectile[arrow].tileCollide = false;
		if (Main.rand.NextBool(9)) {
			type = Main.rand.Next(met);
			newPos = Main.MouseWorld.Add(Main.rand.NextFloat(-120, 120), 900 + Main.rand.NextFloat(-50, 50));
			vel = (Main.MouseWorld - newPos).SafeNormalize(Vector2.Zero) * Item.shootSpeed;
			int proj = Projectile.NewProjectile(source, newPos, vel.Vector2RotateByRandom(5), type, damage * 2, knockback, player.whoAmI, ai1: Main.rand.NextFloat(1, 1.5f));
			Main.projectile[proj].tileCollide = false;
		}
		if (Main.rand.NextBool(5)) {
			type = ProjectileID.HellfireArrow;
			newPos = Main.MouseWorld.Add(Main.rand.NextFloat(-120, 120), 900 + Main.rand.NextFloat(-50, 50));
			vel = (Main.MouseWorld - newPos).SafeNormalize(Vector2.Zero) * Item.shootSpeed;
			int meteor = Projectile.NewProjectile(source, newPos, vel.Vector2RotateByRandom(5), type, (int)(damage * 1.2f), knockback, player.whoAmI, ai1: Main.rand.NextFloat(1, 1.5f));
			Main.projectile[meteor].extraUpdates += 1;
			Main.projectile[meteor].tileCollide = false;
		}
		int straightArrow = Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(5), type, damage, knockback, player.whoAmI);
		if (met.Contains(type)) {
			Main.projectile[straightArrow].ai[1] = Main.rand.NextFloat(.5f, 1f);
		}
		Main.projectile[straightArrow].extraUpdates += 1;
	}
	public override Vector2? HoldoutOffset() {
		return new(-15, 0);
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.MoltenFury)
			.AddIngredient(ItemID.MeteorStaff)
			.Register();
	}
}
