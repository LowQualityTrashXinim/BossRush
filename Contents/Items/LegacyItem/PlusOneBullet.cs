using Terraria;
using Terraria.ID;
using BossRush.Texture;
using BossRush.Contents.Items.Weapon;
using BossRush.Common.RoguelikeChange.ItemOverhaul;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.LegacyItem {
	internal class PlusOneBullet : ModItem {
		public override string Texture => BossRushTexture.MissingTexture_Default;
		public override void SetDefaults() {
			Item.accessory = true;
			Item.height = 30;
			Item.width = 28;
			Item.rare = ItemRarityID.Green;
			Item.value = 1000000;
		}
		public override void UpdateEquip(Player player) {
			var modplayer = player.GetModPlayer<RangerOverhaulPlayer>();
			modplayer.ProjectileAmountModify += 1;
			modplayer.SpreadModify += .35f;
		}
	}
}
