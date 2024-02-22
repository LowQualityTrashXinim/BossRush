using Terraria;
using Terraria.ID;
using BossRush.Texture;
using Terraria.ModLoader;
using BossRush.Common.RoguelikeChange;

namespace BossRush.Contents.Items.Potion {
	internal class GunslingerElixir : ModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultPotion(20, 26, ModContent.BuffType<GodVision>(), 12000);
			Item.rare = ItemRarityID.Orange;
		}
	}
	public class GunslingerElixir_ModPlayer : ModPlayer {
		public override void OnHurt(Player.HurtInfo info) {
			if (Player.HasBuff(ModContent.BuffType<GodVision>())) {
				Player.ClearBuff(ModContent.BuffType<GodVision>());
			}
		}
	}
	internal class GodVision : ModBuff {
		public override string Texture => BossRushTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.GetCritChance(DamageClass.Generic) += 50;
			player.GetModPlayer<RangerOverhaulPlayer>().SpreadModify -= 100;
		}
	}
}
