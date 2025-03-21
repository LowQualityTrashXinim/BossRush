using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using BossRush.Common.Global;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.Underdog {
	class TheUnderdog : SynergyModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(102, 28, 36, 7f, 34, 34, ItemUseStyleID.Shoot, ProjectileID.Bullet, 10, false, AmmoID.Bullet);
			Item.scale = .7f;
			Item.UseSound = SoundID.Item11;
		}
		public override Vector2? HoldoutOffset() {
			return new(-11, 0);
		}
		int counter = 0;
		public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			position = position.PositionOFFSET(velocity, 30);
		}
		public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
			if (player.statLife <= player.statLifeMax2 * .5f) {
				PlayerStatsHandle statplayer = player.GetModPlayer<PlayerStatsHandle>();
				statplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: 10);
				statplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, 1.25f);
			}
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			if (++counter >= 6) {
				for (int i = 0; i < 5; i++) {
					Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(2).Vector2RandomSpread(2, Main.rand.NextFloat(.5f, 1.1f)), type, damage, knockback, player.whoAmI);
				}
				counter = 0;
			}
			CanShootItem = true;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Musket)
				.AddIngredient(ItemID.TheUndertaker)
				.Register();
		}
	}
}
