using BossRush.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace BossRush.Contents.Items.Weapon.UnfinishedItem;
internal class MasterSword : SynergyModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("Synergy");
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.WoodenSword)
			.AddIngredient(ItemID.BorealWoodSword)
			.AddIngredient(ItemID.RichMahoganySword)
			.AddIngredient(ItemID.EbonwoodSword)
			.AddIngredient(ItemID.ShadewoodSword)
			.AddIngredient(ItemID.PalmWoodSword)
			.Register();
	}
}
