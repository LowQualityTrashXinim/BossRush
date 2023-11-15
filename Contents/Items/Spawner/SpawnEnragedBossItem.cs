using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Spawner {
	public class KingSlimeSpecialSpawner : EnragedSpawner {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.SlimeCrown);
		public override void PostSetStaticDefaults() {
			NPCID.Sets.MPAllowedEnemies[NPCID.KingSlime] = true;
		}
		public override bool CanUseItem(Player player) {
			return true;
		}
		public override int BossToSpawn => NPCID.KingSlime;

		//public override void AddRecipes() {
		//	CreateRecipe()
		//		.AddIngredient(ItemID.SlimeCrown)
		//		.AddIngredient(ModContent.ItemType<PowerEnergy>())
		//		.Register();
		//}
	}
	public class EyeOfCthulhuSpecialSpawner : EnragedSpawner {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.SuspiciousLookingEye);
		public override void PostSetStaticDefaults() {
			NPCID.Sets.MPAllowedEnemies[NPCID.EyeofCthulhu] = true;
		}
		public override int BossToSpawn => NPCID.EyeofCthulhu;
		public override bool CanUseItem(Player player) {
			return !Main.dayTime;
		}
		public override void OnUseItem(Player player) {
			base.OnUseItem(player);
			Main.bloodMoon = true;
		}
		//public override void AddRecipes() {
		//	CreateRecipe()
		//		.AddIngredient(ItemID.SuspiciousLookingEye)
		//		.AddIngredient(ModContent.ItemType<PowerEnergy>())
		//		.Register();
		//}
	}
	public class EaterOfWorldSpecialSpawner : EnragedSpawner {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.WormFood);
		public override void PostSetStaticDefaults() {
			NPCID.Sets.MPAllowedEnemies[NPCID.EaterofWorldsHead] = true;
			NPCID.Sets.MPAllowedEnemies[NPCID.EaterofWorldsBody] = true;
			NPCID.Sets.MPAllowedEnemies[NPCID.EaterofWorldsTail] = true;
		}
		public override bool CanUseItem(Player player) {
			return player.ZoneCorrupt;
		}
		public override int BossToSpawn => NPCID.EaterofWorldsHead;
		//public override void AddRecipes() {
		//	CreateRecipe()
		//		.AddIngredient(ItemID.WormFood)
		//		.AddIngredient(ModContent.ItemType<PowerEnergy>())
		//		.Register();
		//}
	}
	public class BrainOfCthulhuSpecialSpawner : EnragedSpawner {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.BloodySpine);
		public override void PostSetStaticDefaults() {
			NPCID.Sets.MPAllowedEnemies[NPCID.BrainofCthulhu] = true;
			NPCID.Sets.MPAllowedEnemies[NPCID.Creeper] = true;
		}
		public override bool CanUseItem(Player player) {
			return player.ZoneCrimson;
		}
		public override int BossToSpawn => NPCID.BrainofCthulhu;
		//public override void AddRecipes() {
		//	CreateRecipe()
		//		.AddIngredient(ItemID.BloodySpine)
		//		.AddIngredient(ModContent.ItemType<PowerEnergy>())
		//		.Register();
		//}
	}
	public class QueenBeeSpecialSpawner : EnragedSpawner {
		public override string Texture => BossRushUtils.GetVanillaTexture<Item>(ItemID.Abeemination);
		public override void PostSetStaticDefaults() {
			NPCID.Sets.MPAllowedEnemies[NPCID.QueenBee] = true;
		}
		public override int BossToSpawn => NPCID.QueenBee;
		//public override void AddRecipes() {
		//	CreateRecipe()
		//		.AddIngredient(ItemID.Abeemination)
		//		.AddIngredient(ModContent.ItemType<PowerEnergy>())
		//		.Register();
		//}
	}
}
