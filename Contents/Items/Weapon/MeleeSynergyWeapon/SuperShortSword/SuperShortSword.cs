using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.MeleeSynergyWeapon.SuperShortSword {
	class SuperShortSword : SynergyModItem {
		public override void SetDefaults() {
			Item.BossRushSetDefault(68, 68, 93, 6f, 36, 36, ItemUseStyleID.Shoot, true);
			Item.BossRushSetDefaultSpear(ModContent.ProjectileType<SuperShortSwordP>(), 2.4f);
			Item.rare = ItemRarityID.Green;
			Item.value = Item.buyPrice(platinum: 5);
			Item.UseSound = SoundID.Item1;
		}
		public override bool CanUseItem(Player player) {
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
		public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
			player.AddBuff(ModContent.BuffType<SuperShortSwordPower>(), 2);
			if (player.ownedProjectileCounts[ModContent.ProjectileType<SuperShortSwordOrbitShortSword>()] < 1) {
				for (int i = 0; i < 8; i++) {
					Projectile.NewProjectile(
						player.GetSource_FromThis(),
						player.Center,
						Vector2.Zero,
						ModContent.ProjectileType<SuperShortSwordOrbitShortSword>(),
						(int)(Item.damage * 0.25f),
						0,
						player.whoAmI,
						i, i);
				}
			}
		}
		public override void AddRecipes() {
			CreateRecipe()
			.AddIngredient(ItemID.CopperShortsword)
			.AddIngredient(ItemID.TinShortsword)
			.AddIngredient(ItemID.IronShortsword)
			.AddIngredient(ItemID.LeadShortsword)
			.AddIngredient(ItemID.SilverShortsword)
			.AddIngredient(ItemID.TungstenShortsword)
			.AddIngredient(ItemID.GoldShortsword)
			.AddIngredient(ItemID.PlatinumShortsword)
			.Register();
		}
	}
}
