using BossRush.Common.RoguelikeChange;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.QuadDemonBlaster {
	class QuadDemonBlaster : SynergyModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(40, 30, 29, 3f, 15, 15, ItemUseStyleID.Shoot, ProjectileID.Bullet, 15, true, AmmoID.Bullet);
			Item.value = Item.buyPrice(gold: 50);
			Item.rare = ItemRarityID.Orange;
			Item.reuseDelay = 15;
			Item.UseSound = SoundID.Item41;
			if (Item.TryGetGlobalItem(out RangeWeaponOverhaul weapon)) {
				weapon.SpreadAmount = 0;
				weapon.OffSetPost = 30;
				weapon.NumOfProjectile = 1;
				weapon.itemIsAShotgun = true;
			}
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			base.ModifySynergyToolTips(ref tooltips, modplayer);
		}
		public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
			modplayer.QuadDemonBlaster_SpeedMultiplier -= modplayer.QuadDemonBlaster_SpeedMultiplier == 1 ? 0 : .25f;
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			var weapon = Item.GetGlobalItem<RangeWeaponOverhaul>();
			float rotation = MathHelper.ToRadians(weapon.SpreadAmount);
			for (int i = 0; i < 10; i++) {
				Vector2 Rotate = velocity.Vector2DistributeEvenly(10, rotation, i);
				float RandomSpeadx = Main.rand.NextFloat(0.5f, 1f);
				float RandomSpeady = Main.rand.NextFloat(0.5f, 1f);
				Projectile.NewProjectile(source, position.X, position.Y,
					Rotate.X * (modplayer.QuadDemonBlaster_SpeedMultiplier == 1 ? 1 : RandomSpeadx),
					Rotate.Y * (modplayer.QuadDemonBlaster_SpeedMultiplier == 1 ? 1 : RandomSpeady),
					type, damage, knockback, player.whoAmI);
			}
			modplayer.QuadDemonBlaster_SpeedMultiplier += modplayer.QuadDemonBlaster_SpeedMultiplier < 45 ? 20 : 1;
			weapon.SpreadAmount = modplayer.QuadDemonBlaster_SpeedMultiplier;
			CanShootItem = true;
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-4, 2);
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.QuadBarrelShotgun)
				.AddIngredient(ItemID.PhoenixBlaster)
				.Register();
		}
	}
}
