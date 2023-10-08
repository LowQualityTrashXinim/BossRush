using BossRush.Common.Utils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.SuperFlareGun {
	internal class SuperFlareGun : SynergyModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(68, 38, 20, 2f, 20, 20, ItemUseStyleID.Shoot, ModContent.ProjectileType<SuperFlareP>(), 20, false, AmmoID.Flare);
			Item.rare = 4;
			Item.crit = 5;
			Item.scale = 0.75f;
			Item.UseSound = SoundID.Item11;
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(3, 0);
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			if (modplayer.SuperFlareGun_Phaseblade)
				tooltips.Add(new TooltipLine(Mod, "SuperFlareGun_Phaseblade", $"[i:{Main.rand.Next(TerrariaArrayID.Phaseblade)}] Decrease life time of super flare gun projectile"));
		}
		public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
			if (player.HasItem(ItemID.BluePhaseblade) ||
				player.HasItem(ItemID.RedPhaseblade) ||
				player.HasItem(ItemID.GreenPhaseblade) ||
				player.HasItem(ItemID.OrangePhaseblade) ||
				player.HasItem(ItemID.YellowPhaseblade) ||
				player.HasItem(ItemID.PurplePhaseblade) ||
				player.HasItem(ItemID.WhitePhaseblade)) {
				modplayer.SuperFlareGun_Phaseblade = true;
				modplayer.SynergyBonus++;
			}
		}
		public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			type = ModContent.ProjectileType<SuperFlareP>();
			position = position.PositionOFFSET(velocity, 40);
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.FlareGun)
				.AddIngredient(ItemID.MoltenFury)
				.Register();
		}
	}
}
