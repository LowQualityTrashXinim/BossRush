using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Weapon.ArcaneRange.MagicBow {
	internal class AmethystBow : MagicBow {
		public override void MagicBowSetDefault(out int mana, out int shoot, out float shootspeed, out int damage, out int useTime, out int dustType) {
			mana = 12;
			damage = 15;
			useTime = 25;
			shoot = ModContent.ProjectileType<AmethystBolt>();
			shootspeed = 5f;
			dustType = DustID.GemAmethyst;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.CopperBow)
				.AddIngredient(ItemID.AmethystStaff)
				.Register();
		}
	}
}