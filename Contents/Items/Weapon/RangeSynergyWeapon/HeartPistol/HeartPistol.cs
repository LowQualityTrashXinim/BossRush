using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.HeartPistol {
	internal class HeartPistol : SynergyModItem {
		public override void Synergy_SetStaticDefaults() {
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.Vilethorn, $"[i:{ItemID.Vilethorn}] Heart projectile inflict venom");
		}
		public override void SetDefaults() {
			Item.BossRushDefaultRange(26, 52, 11, 3f, 10, 50, ItemUseStyleID.Shoot, ModContent.ProjectileType<HeartP>(), 10, false, AmmoID.Bullet);
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 50);
			Item.UseSound = SoundID.Item11;
			Item.reuseDelay = 18;
		}
		int counter = 0;
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.Vilethorn);
		}
		public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			position = position.PositionOFFSET(velocity, 30);
			if (++counter < 5) {
				velocity = velocity.Vector2DistributeEvenlyPlus(4, 40, counter - 1);
			}
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			SoundEngine.PlaySound(Item.UseSound, player.Center);
			if (counter >= 5) {
				Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<HeartP>(), damage * 2, knockback, player.whoAmI);
				counter = 0;
			}
			else {
				int proj = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<smallerHeart>(), damage, knockback, player.whoAmI, 1);
				Main.projectile[proj].scale = 1.5f;
			}
			CanShootItem = false;
		}

		public override Vector2? HoldoutOffset() {
			return new Vector2(-2, 0);
		}

		public override void AddRecipes() {
			CreateRecipe()
			.AddIngredient(ItemID.FlintlockPistol)
			.AddIngredient(ItemID.BandofRegeneration)
			.Register();
		}
	}
}
