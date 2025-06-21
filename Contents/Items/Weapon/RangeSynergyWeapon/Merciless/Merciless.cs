using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.RoguelikeChange.ItemOverhaul;
using Terraria.Audio;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Merciless {
	internal class Merciless : SynergyModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(102, 26, 20, 5f, 15, 30, ItemUseStyleID.Shoot, ProjectileID.Bullet, 8, true, AmmoID.Bullet);
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 50);
			Item.crit = 10;
			Item.reuseDelay = 30;
			Item.scale -= 0.15f;
			Item.UseSound = SoundID.Item38;
			if (Item.TryGetGlobalItem(out RangeWeaponOverhaul weapon)) {
				weapon.itemIsAShotgun = true;
			}
		}
		int counter = 0;
		public override Vector2? HoldoutOffset() {
			return new Vector2(-26, 0);
		}
		public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			position = position.PositionOFFSET(velocity, 60);
			if (type == ProjectileID.Bullet) {
				type = ProjectileID.ExplosiveBullet;
			}
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			SoundEngine.PlaySound(Item.UseSound, player.Center);
			int weapondamage = damage;
			float spread = 30, additional = 3, multi = .55f;
			if (++counter == 1) {
				spread = 5;
				additional = 1;
				multi = .85f;
			}
			else {
				weapondamage = (int)(weapondamage * 1.25f);
				Projectile.NewProjectile(source, position, velocity * 1.5f, ProjectileID.CannonballFriendly, damage * 4, knockback, player.whoAmI);
				counter = 0;
			}
			for (int i = 0; i < 10; i++) {
				Vector2 velocity2 = velocity.Vector2RotateByRandom(spread).Vector2RandomSpread(additional, Main.rand.NextFloat(multi, 1f));
				Projectile.NewProjectile(source, position, velocity2, type, weapondamage, knockback, player.whoAmI);
			}
			CanShootItem = true;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Boomstick)
				.AddIngredient(ItemID.QuadBarrelShotgun)
				.Register();
		}
	}
	public class Item_MercilessPlayer : ModPlayer {
		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
			if (proj.Check_ItemTypeSource(ModContent.ItemType<Merciless>())) {
				if (target.GetLifePercent() <= .3f || target.GetLifePercent() >= .8f) {
					modifiers.SourceDamage += .5f;
				}
			}
		}
	}
}
