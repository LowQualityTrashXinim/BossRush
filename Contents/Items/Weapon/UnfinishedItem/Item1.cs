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
	/*
	 * "Genshin ahh weapon"
	 * Shoot out star bolt that chase down enemy
	 * Hitting enemy with item trigger "name 1" for 5s ( 12s cool down )
	 * During "name 1" your mana regeneration rate is increases
	 * And each strike toward enemy increases star bolt damage toward that enemy by 5% ( capped at 100% )
	 */
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Starfury)
			.AddIngredient(ItemID.MagicMissile)
			.AddIngredient(ItemID.BabyBirdStaff)
			.Register();
	}
}

