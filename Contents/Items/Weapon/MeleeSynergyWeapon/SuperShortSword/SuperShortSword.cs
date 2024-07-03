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
	public class SuperShortSwordPlayer : ModPlayer {
		public int SuperShortSword_Counter = 0;
		public int SuperShortSword_AttackType = 0;
		public int SuperShortSword_Delay = 0;
		public int SuperShortSword_ProjectileInReadyPosition = 0;
		public bool SuperShortSword_IsHoldingDownRightMouse = false;
		public override void PreUpdate() {
			Item item = Player.HeldItem;
			SuperShortSwordUpdate(item);
		}
		private void SuperShortSwordUpdate(Item item) {
			if (item.type == ModContent.ItemType<SuperShortSword>()) {
				SuperShortSword_Delay = BossRushUtils.CountDown(SuperShortSword_Delay);
				if (Main.mouseLeft && SuperShortSword_AttackType == 0 && SuperShortSword_Delay <= 0) {
					SuperShortSword_AttackType = 1;
				}
				if (SuperShortSword_ProjectileInReadyPosition >= 8 && SuperShortSword_AttackType == 1) {
					SuperShortSword_ProjectileInReadyPosition = 0;
					SuperShortSword_AttackType = 0;
					SuperShortSword_Delay = 10;
				}

				if (Main.mouseRight && SuperShortSword_AttackType == 0 && SuperShortSword_Delay <= 0) {
					SuperShortSword_AttackType = 2;
					SuperShortSword_IsHoldingDownRightMouse = true;
				}
				if (SuperShortSword_AttackType == 2) {
					if (SuperShortSword_IsHoldingDownRightMouse) {
						if (Main.mouseRightRelease && !Main.mouseRight)
							SuperShortSword_IsHoldingDownRightMouse = false;
					}
					if (!SuperShortSword_IsHoldingDownRightMouse && SuperShortSword_ProjectileInReadyPosition >= 8 && SuperShortSword_AttackType == 2) {
						SuperShortSword_ProjectileInReadyPosition = 0;
						SuperShortSword_AttackType = 0;
						SuperShortSword_Delay = 10;
					}
				}
				if (SuperShortSword_AttackType != 0) {
					return;
				}
				if (SuperShortSword_Counter == MathHelper.TwoPi * 100 || SuperShortSword_Counter == -MathHelper.TwoPi * 100) {
					SuperShortSword_Counter = 0;
				}
				SuperShortSword_Counter += Player.direction;
			}
			else {
				SuperShortSword_AttackType = 0;
				SuperShortSword_Delay = 10;
				SuperShortSword_Counter = 0;
				SuperShortSword_ProjectileInReadyPosition = 0;
			}
		}
	}
}
