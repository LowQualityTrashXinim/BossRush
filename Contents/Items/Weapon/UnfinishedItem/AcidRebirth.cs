using BossRush.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace BossRush.Contents.Items.Weapon.UnfinishedItem;

class AcidRebirth : SynergyModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("Synergy");
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.DD2PhoenixBow)
			.AddIngredient(ItemID.Toxikarp)
			.Register();
	}
}
