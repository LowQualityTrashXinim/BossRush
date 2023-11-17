using BossRush.Common.RoguelikeChange;
using BossRush.Contents.BuffAndDebuff;
using BossRush.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossRush.Contents.Items.Potion {
	internal class GunslingerElixir : ModItem {
		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 26;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useTurn = true;
			Item.maxStack = 30;
			Item.consumable = true;
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
