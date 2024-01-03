using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.RoguelikeChange;

namespace BossRush.Contents.Items.Potion {
	internal class GunslingerElixir : ModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultToConsume(20, 26, ItemUseStyleID.DrinkLiquid);
			Item.maxStack = 30;
			Item.rare = ItemRarityID.Orange;
			Item.buffType = ModContent.BuffType<GodVision>();
			Item.buffTime = 12000;
		}
	}
	internal class GodVision : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.GetCritChance(DamageClass.Generic) += 70;
			player.GetModPlayer<RangerOverhaulPlayer>().SpreadModify -= 100;
		}
	}
}
