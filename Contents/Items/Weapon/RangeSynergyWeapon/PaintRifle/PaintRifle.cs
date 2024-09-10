using BossRush.Common.RoguelikeChange.ItemOverhaul;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.PaintRifle {
	internal class PaintRifle : SynergyModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(114, 40, 36, 4f, 4, 16, ItemUseStyleID.Shoot, ModContent.ProjectileType<CustomPaintProj>(), 7, true);
			Item.rare = ItemRarityID.Orange;
			Item.crit = 7;
			Item.reuseDelay = 11;
			Item.UseSound = SoundID.Item5;
			Item.value = Item.sellPrice(gold: 1000);
			Item.scale -= 0.25f;
			if (Item.TryGetGlobalItem(out RangeWeaponOverhaul weapon)) {
				weapon.SpreadAmount = 1;
				weapon.OffSetPost = 42;
			}
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-33, 3.5f);
		}

		public override bool AltFunctionUse(Player player) {
			return true;
		}

		public static int r = Main.rand.Next(256);
		public static int b = Main.rand.Next(256);
		public static int g = Main.rand.Next(256);
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			r = 0; b = 0; g = 0;
			for (int i = 0; i < 2; i++) {
				int randChooser = Main.rand.Next(3);
				switch (randChooser) {
					case 0:
						r = 255;
						break;
					case 1:
						b = 255;
						break;
					case 2:
						g = 255;
						break;
				}
			}
			if (player.altFunctionUse == 2) {
				Item.GetGlobalItem<RangeWeaponOverhaul>().SpreadAmount = 15;
				for (int i = 0; i < 3; i++) {
					velocity = velocity.Vector2RotateByRandom(15).Vector2RandomSpread(2, 1.2f);
					Projectile.NewProjectile(source, position, velocity, type, (int)(damage * .7f), knockback, player.whoAmI);
					for (int l = 0; l < 15; l++) {
						Vector2 spread = velocity.Vector2RotateByRandom(35).Vector2RandomSpread(3, .2f) + player.velocity;
						int dust = Dust.NewDust(position, 0, 0, DustID.Paint, spread.X, spread.Y, 0, new Color(r, g, b), Main.rand.NextFloat(1.2f, 1.45f));
						Main.dust[dust].noGravity = true;
					}
				}
				CanShootItem = false;
				return;
			}
			Item.GetGlobalItem<RangeWeaponOverhaul>().SpreadAmount = 1;
			for (int i = 0; i < 15; i++) {
				Vector2 spread = velocity.Vector2RotateByRandom(35).Vector2RandomSpread(3, .2f) + player.velocity;
				int dust = Dust.NewDust(position, 0, 0, DustID.Paint, spread.X, spread.Y, 0, new Color(r, g, b), Main.rand.NextFloat(1f, 1.45f));
				Main.dust[dust].noGravity = true;
			}
			CanShootItem = true;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.PainterPaintballGun)
				.AddIngredient(ItemID.ClockworkAssaultRifle)
				.Register();
		}
	}
}
