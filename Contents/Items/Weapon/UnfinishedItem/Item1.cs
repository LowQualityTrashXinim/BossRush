using BossRush.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace BossRush.Contents.Items.Weapon.UnfinishedItem;

class Item1 : SynergyModItem {
	public override string Texture => BossRushTexture.Get_MissingTexture("Synergy");
	//public override void AddRecipes() {
	//	CreateRecipe()
	//		.Register();
	//}
}
