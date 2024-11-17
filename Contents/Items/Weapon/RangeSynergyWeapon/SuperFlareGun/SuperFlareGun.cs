using BossRush.Common.Utils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.RangeSynergyWeapon.SuperFlareGun {
	internal class SuperFlareGun : SynergyModItem {
		public override void Synergy_SetStaticDefaults() {
			SynergyBonus_System.Add_SynergyGroupBonus(Type, ItemID.BluePhaseblade, new List<int>()
			{
				ItemID.BluePhaseblade,
				ItemID.RedPhaseblade,
				ItemID.GreenPhaseblade,
				ItemID.OrangePhaseblade,
				ItemID.YellowPhaseblade,
				ItemID.PurplePhaseblade,
				ItemID.WhitePhaseblade
			});
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.BluePhaseblade);
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.Boomstick);
		}
		public override void SetDefaults() {
			Item.BossRushDefaultRange(68, 38, 20, 2f, 20, 20, ItemUseStyleID.Shoot, ModContent.ProjectileType<SuperFlareP>(), 20, false, AmmoID.Flare);
			Item.rare = ItemRarityID.LightRed;
			Item.crit = 5;
			Item.scale = 0.75f;
			Item.UseSound = SoundID.Item11;
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(3, 0);
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.BluePhaseblade))
				tooltips.Add(new TooltipLine(Mod, "SuperFlareGun_Phaseblade", $"[i:{Main.rand.Next(TerrariaArrayID.Phaseblade)}] Decrease life time of super flare gun projectile"));
			if(SynergyBonus_System.Check_SynergyBonus(Type, ItemID.Boomstick)) {
				tooltips.Add(new(Mod, $"{DisplayName}_Boomstick", $"[i:{ItemID.Boomstick}] On flare explode have 10% chance to create a ring of bullet"));
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
