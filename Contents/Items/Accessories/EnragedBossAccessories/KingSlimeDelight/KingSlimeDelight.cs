using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace BossRush.Contents.Items.Accessories.EnragedBossAccessories.KingSlimeDelight {
	internal class KingSlimeDelight : ModItem {
		public override void SetDefaults() {
			Item.height = Item.width = 40;
			Item.rare = ItemRarityID.Lime;
			Item.value = 10000000;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.jumpSpeedBoost += 2.5f;
			player.accRunSpeed += .1f;
			player.statDefense += 5;
			player.GetModPlayer<KingSlimePowerPlayer>().KingSlimePower = true;
		}
	}

	internal class KingSlimePowerPlayer : ModPlayer {
		int KSPcounter = 0;
		public bool KingSlimePower;
		public override void ResetEffects() {
			KingSlimePower = false;
		}
		public override void UpdateEquips() {
			if (!KingSlimePower) {
				return;
			}
			if (Player.Center.LookForAnyHostileNPC(100)) {
				if (++KSPcounter >= 100) {
					for (int i = 0; i < 18; i++) {
						Vector2 RotateSpeed = Vector2.One.Vector2DistributeEvenly(18, 360, i);
						Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, RotateSpeed * 8f, ModContent.ProjectileType<FriendlySlimeProjectile>(), 25, 1f, Player.whoAmI);
					}
					KSPcounter = 0;
				}
			}
		}

		public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (KingSlimePower) {
				Vector2 speed = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.UnitX) * 20f;
				Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, speed, ModContent.ProjectileType<FriendlySlimeProjectile>(), (int)(damage * 0.35f), 1f, Player.whoAmI);
			}
		}
	}
}
