using BossRush.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace BossRush.Contents.Items.Weapon.UnfinishedItem;
class BrokenForecast : SynergyModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("Synergy");
	public override void SetDefaults() {
		Item.BossRushDefaultMeleeCustomProjectile(32, 32, 100, 5f, 30, 30, ItemUseStyleID.Swing, 1, true);
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.BreakerBlade)
			.AddIngredient(ItemID.NimbusRod)
			.Register();
	}
}
