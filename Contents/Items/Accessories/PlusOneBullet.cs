using BossRush.Common.RoguelikeChange;
using BossRush.Contents.Items.Weapon;
using BossRush.Texture;
using Terraria;
using Terraria.ID;

namespace BossRush.Contents.Items.Accessories {
	internal class PlusOneBullet : SynergyModItem {
		public override string Texture => BossRushTexture.MISSINGTEXTURE;
		public override void SetDefaults() {
			Item.accessory = true;
			Item.height = 30;
			Item.width = 28;
			Item.rare = ItemRarityID.Green;
			Item.value = 1000000;
		}
		public override void UpdateEquip(Player player) {
			RangerOverhaulPlayer modplayer = player.GetModPlayer<RangerOverhaulPlayer>();
			modplayer.ProjectileAmountModify += 1;
			modplayer.SpreadModify += .35f;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Minishark)
				.AddIngredient(ItemID.WhiteString)
				.Register();
		}
	}
}
